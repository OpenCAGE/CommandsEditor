using CATHODE;
using CATHODE.ShaderTypes;
using CathodeLib;
using CommandsEditor;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static CATHODE.Materials.Material;

//This is ported directly from AlienPAK, with its editing functionality stripped out.
//Eventually we should support editing here too, and deprecate AlienPAK.

namespace AlienPAK
{
    public partial class SelectMaterial : BaseWindow
    {
        List<Materials.Material> _sortedMaterials = new List<Materials.Material>();
        List<Textures.TEX4> _materialTextures = new List<Textures.TEX4>();

        MaterialEditorControlsWPF _controls = null;

        public Action<Materials.Material> OnMaterialSelected;

        public SelectMaterial(Materials.Material material = null) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            this.Text = "Select Material";

            _controls = (MaterialEditorControlsWPF)elementHost1.Child;
            _controls.OnMaterialTextureIndexSelected += OnMaterialTextureIndexSelected;
            _controls.OnFeatureSelected += OnFeatureSelected;
            _controls.OnParameterSelected += OnParameterSelected;

            _sortedMaterials.Clear();
            _sortedMaterials.AddRange(Content.Level.Materials.Entries);
            _sortedMaterials = _sortedMaterials.OrderBy(o => o.Name).ToList();

            materialList.BeginUpdate();
            materialList.Items.Clear();
            for (int i = 0; i < _sortedMaterials.Count; i++)
            {
                materialList.Items.Add(_sortedMaterials[i].Name);
                if (_sortedMaterials[i] == material) materialList.SelectedIndex = i;
            }
            materialList.EndUpdate();
        }

        private void materialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controls.materialTextureSelection.Items.Clear();
            _materialTextures.Clear();
            _controls.featureSelection.Items.Clear();
            _controls.parameterSelection.Items.Clear();
            _controls.featureDetailsPanel.Children.Clear();
            _controls.parameterDetailsPanel.Children.Clear();
            _controls.shaderType.Text = "";

            if (materialList.SelectedIndex == -1) return;

            Materials.Material material = _sortedMaterials[materialList.SelectedIndex];

            _controls.shaderType.Text = material.Shader.Ubershader.ToString();

            List<string> samplers = ShaderUtility.GetSamplers(material.Shader.Ubershader);
            foreach (string sampler in samplers)
            {
                int samplerIndex = ShaderUtility.GetShaderFunctionalityIndex(material.Shader.Ubershader, ShaderIndexType.SAMPLERS, sampler).Value;
                if (samplerIndex >= material.Shader.SamplerRemaps.Count) continue;

                int diffuseMapIndex = material.Shader.SamplerRemaps[samplerIndex];
                if (diffuseMapIndex != 255)
                {
                    if (material.TextureReferences[diffuseMapIndex]?.Texture != null)
                    {
                        _controls.materialTextureSelection.Items.Add(sampler);
                        _materialTextures.Add(material.TextureReferences[diffuseMapIndex].Texture);
                    }
                }
            }
            if (_controls.materialTextureSelection.Items.Count != 0)
                _controls.materialTextureSelection.SelectedIndex = 0;

            List<string> features = ShaderUtility.GetFeatures(material.Shader.Ubershader);
            foreach (string feature in features)
            {
                _controls.featureSelection.Items.Add(feature);
            }
            if (_controls.featureSelection.Items.Count != 0)
                _controls.featureSelection.SelectedIndex = 0;

            List<string> parameters = ShaderUtility.GetParameters(material.Shader.Ubershader);
            foreach (string parameter in parameters)
            {
                int parameterIndex = ShaderUtility.GetShaderFunctionalityIndex(material.Shader.Ubershader, ShaderIndexType.PARAMETERS, parameter).Value;
                if (parameterIndex >= material.Shader.PixelShaderParameterRemaps.Count) continue;

                int remappedIndex = material.Shader.PixelShaderParameterRemaps[parameterIndex];
                if (remappedIndex != 255 && remappedIndex < material.PixelShaderConstants.Count)
                {
                    _controls.parameterSelection.Items.Add(parameter);
                }
            }
            if (_controls.parameterSelection.Items.Count != 0)
                _controls.parameterSelection.SelectedIndex = 0;

            OnMaterialTextureIndexSelected();
        }

        private void OnMaterialTextureIndexSelected(int i = 0)
        {
            _controls.textureFile.Text = "";
            _controls.materialTexturePreview.Source = null;

            if (materialList.SelectedIndex == -1 || _controls.materialTextureSelection.SelectedIndex == -1) return;

            Textures.TEX4 texture = _materialTextures[_controls.materialTextureSelection.SelectedIndex];
            _controls.textureFile.Text = texture.Name;
            _controls.materialTexturePreview.Source = texture?.ToDDS()?.ToBitmap()?.ToImageSource();
        }

        private void OnFeatureSelected(string featureName)
        {
            _controls.featureDetailsPanel.Children.Clear();

            if (materialList.SelectedIndex == -1) return;

            Materials.Material material = _sortedMaterials[materialList.SelectedIndex];

            int featureIndex = ShaderUtility.GetShaderFunctionalityIndex(material.Shader.Ubershader, ShaderIndexType.FEATURES, featureName).Value;
            bool isEnabled = (material.Shader.UbershaderFeatureFlags & (1L << featureIndex)) != 0;

            System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox
            {
                Content = "Enabled",
                IsChecked = isEnabled,
                IsEnabled = false 
            };
            _controls.featureDetailsPanel.Children.Add(checkBox);
        }

        private void OnParameterSelected(string parameterName)
        {
            _controls.parameterDetailsPanel.Children.Clear();

            if (materialList.SelectedIndex == -1) return;

            Materials.Material material = _sortedMaterials[materialList.SelectedIndex];

            int parameterIndex = ShaderUtility.GetShaderFunctionalityIndex(material.Shader.Ubershader, ShaderIndexType.PARAMETERS, parameterName).Value;
            UberShaderParameterType parameterType = ShaderUtility.GetParameterType(material.Shader.Ubershader, parameterName).Value;
            int remappedIndex = material.Shader.PixelShaderParameterRemaps[parameterIndex];
            int floatCount = GetFloatCountForParameterType(parameterType);

            TextBlock textBlock = new TextBlock { Margin = new System.Windows.Thickness(0, 0, 0, 5) };
            if (floatCount == 1)
            {
                float value = material.PixelShaderConstants[remappedIndex];
                textBlock.Text = parameterType == UberShaderParameterType.Int ? ((int)value).ToString() : value.ToString("F6");
            }
            else if (floatCount == 2)
            {
                float x = material.PixelShaderConstants[remappedIndex];
                float y = remappedIndex + 1 < material.PixelShaderConstants.Count ? material.PixelShaderConstants[remappedIndex + 1] : 0;
                textBlock.Text = $"X: {x:F6}, Y: {y:F6}";
            }
            else if (floatCount == 3)
            {
                float x = material.PixelShaderConstants[remappedIndex];
                float y = remappedIndex + 1 < material.PixelShaderConstants.Count ? material.PixelShaderConstants[remappedIndex + 1] : 0;
                float z = remappedIndex + 2 < material.PixelShaderConstants.Count ? material.PixelShaderConstants[remappedIndex + 2] : 0;
                textBlock.Text = $"X: {x:F6}, Y: {y:F6}, Z: {z:F6}";
            }
            else if (floatCount == 4)
            {
                float x = material.PixelShaderConstants[remappedIndex];
                float y = remappedIndex + 1 < material.PixelShaderConstants.Count ? material.PixelShaderConstants[remappedIndex + 1] : 0;
                float z = remappedIndex + 2 < material.PixelShaderConstants.Count ? material.PixelShaderConstants[remappedIndex + 2] : 0;
                float w = remappedIndex + 3 < material.PixelShaderConstants.Count ? material.PixelShaderConstants[remappedIndex + 3] : 0;
                textBlock.Text = $"X: {x:F6}, Y: {y:F6}, Z: {z:F6}, W: {w:F6}";
            }
            _controls.parameterDetailsPanel.Children.Add(textBlock);
        }

        private int GetFloatCountForParameterType(UberShaderParameterType parameterType)
        {
            switch (parameterType)
            {
                case UberShaderParameterType.Float:
                case UberShaderParameterType.Half:
                case UberShaderParameterType.Int:
                    return 1;
                case UberShaderParameterType.Float2:
                case UberShaderParameterType.Half2:
                    return 2;
                case UberShaderParameterType.Float3:
                case UberShaderParameterType.Half3:
                    return 3;
                case UberShaderParameterType.Float4:
                case UberShaderParameterType.Half4:
                    return 4;
                default:
                    return 1;
            }
        }

        private void selectMaterial_Click(object sender, EventArgs e)
        {
            OnMaterialSelected?.Invoke(_sortedMaterials[materialList.SelectedIndex]);
            this.Close();
        }
    }
}
