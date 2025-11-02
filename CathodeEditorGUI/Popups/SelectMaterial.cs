using CATHODE;
using CATHODE.LEGACY;
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

        public Action<int> OnMaterialSelected;

        public SelectMaterial(Materials.Material material = null) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            this.Text = "Select Material";
            duplicateMaterial.Visible = false;

            _controls = (MaterialEditorControlsWPF)elementHost1.Child;
            _controls.OnMaterialTextureIndexSelected += OnMaterialTextureIndexSelected;

            _sortedMaterials.Clear();
            _sortedMaterials.AddRange(Content.resource.materials.Entries);
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
            _controls.fileNameText.Text = "";
            _controls.shaderName.Text = "";

            _controls.materialTextureSelection.Items.Clear();
            _materialTextures.Clear();

            if (materialList.SelectedIndex == -1) return;

            Materials.Material material = _sortedMaterials[materialList.SelectedIndex];
            Shaders.Shader shader = Content.resource.shaders.Entries[material.ShaderIndex];

            List<string> samplers = ShaderUtility.GetSamplers(shader.Ubershader);
            foreach (string sampler in samplers)
            {
                int samplerIndex = ShaderUtility.GetShaderFunctionalityIndex(shader.Ubershader, ShaderIndexType.SAMPLERS, sampler).Value;

                if (shader.SamplerRemaps.Count > samplerIndex)
                {
                    int diffuseMapIndex = shader.SamplerRemaps[samplerIndex];
                    if (diffuseMapIndex != 255)
                    {
                        TexturePtr ptr = material.TextureReferences[diffuseMapIndex];
                        if (ptr != null && ptr.Index != -1)
                        {
                            _controls.materialTextureSelection.Items.Add(sampler);
                            _materialTextures.Add((ptr.Location == TexturePtr.Source.GLOBAL ? Singleton.GlobalTextures : Content.resource.textures).GetAtWriteIndex(ptr.Index));
                        }
                    }
                }
            }

            _controls.fileNameText.Text = _sortedMaterials[materialList.SelectedIndex].Name;
            _controls.shaderName.Text = shader.Ubershader.ToString();

            if (_controls.materialTextureSelection.Items.Count != 0)
                _controls.materialTextureSelection.SelectedIndex = 0;

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

        private void selectMaterial_Click(object sender, EventArgs e)
        {
            OnMaterialSelected?.Invoke(Content.resource.materials.GetWriteIndex(_sortedMaterials[materialList.SelectedIndex]));
            this.Close();
        }
    }
}
