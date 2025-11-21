using AlienPAK;
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

namespace CommandsEditor
{
    public partial class EditMaterial : BaseWindow
    {
        List<Materials.Material> _sortedMaterials = new List<Materials.Material>();

        //Sampler information: (sampler name, sampler index, texture reference index)
        List<Tuple<string, int, int>> _samplerInfo = new List<Tuple<string, int, int>>();

        MaterialInfoWPF _controls = null;

        public Action<Materials.Material> OnMaterialSelected;

        public EditMaterial(Materials.Material material = null) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            this.Text = "Select Material";

            _controls = (MaterialInfoWPF)elementHost1.Child;
            _controls.OnSamplerSelected += OnSamplerSelected;
            _controls.OnParameterSelected += OnParameterSelected;
            _controls.OnPickTexture += OnPickTexture;

            _sortedMaterials.Clear();
            _sortedMaterials.AddRange(Content.Level.Materials.Entries);
            _sortedMaterials = _sortedMaterials.OrderBy(o => o.Name).ToList();

            materialList.BeginUpdate();
            materialList.Items.Clear();
            materialList.Groups.Clear();
            materialList.Columns.Clear();
            
            materialList.Columns.Add("Material Name", 360);
            
            var groupedMaterials = _sortedMaterials.Where(m => m.Shader != null).GroupBy(m => m.Shader.Ubershader).OrderBy(g => g.Key.ToString());

            foreach (var group in groupedMaterials)
            {
                string groupName = group.Key.ToString();
                System.Windows.Forms.ListViewGroup listGroup = new System.Windows.Forms.ListViewGroup(groupName, groupName);
                materialList.Groups.Add(listGroup);
                
                foreach (var mat in group.OrderBy(m => m.Name))
                {
                    System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(mat.Name);
                    item.Group = listGroup;
                    item.Tag = mat;
                    materialList.Items.Add(item);

                    if (mat == material)
                        item.Selected = true;
                }
            }
            materialList.EndUpdate();
        }

        private void EditMaterial_Load(object sender, EventArgs e)
        {
            if (materialList.SelectedItems.Count > 0)
            {
                materialList.SelectedItems[0].EnsureVisible();
                materialList.EnsureVisible(materialList.SelectedItems[0].Index);
            }
        }

        private void materialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controls.SamplerTabControl.Items.Clear();
            _samplerInfo.Clear();
            _controls.parameterSelection.Items.Clear();
            _controls.featureDetailsPanel.Children.Clear();
            _controls.parameterDetailsPanel.Children.Clear();
            _controls.shaderType.Text = "";

            if (materialList.SelectedItems.Count == 0) return;

            Materials.Material material = materialList.SelectedItems[0].Tag as Materials.Material;
            if (material == null) return;

            _controls.shaderType.Text = material.Shader.Ubershader.ToString();

            List<string> samplers = ShaderUtility.GetSamplers(material.Shader.Ubershader);
            int firstSamplerWithTextureIndex = -1;
            for (int i = 0; i < samplers.Count; i++)
            {
                string sampler = samplers[i];
                int? samplerIndexNullable = ShaderUtility.GetShaderFunctionalityIndex(material.Shader.Ubershader, ShaderIndexType.SAMPLERS, sampler);
                if (!samplerIndexNullable.HasValue) continue;

                int samplerIndex = samplerIndexNullable.Value;
                int textureRefIndex = 255;
                if (samplerIndex < material.Shader.SamplerRemaps.Count)
                    textureRefIndex = material.Shader.SamplerRemaps[samplerIndex];

                bool hasTexture = false;
                Textures.TEX4 texture = null;
                if (textureRefIndex != 255 && textureRefIndex < material.TextureReferences.Count)
                {
                    var textureRef = material.TextureReferences[textureRefIndex];
                    if (textureRef?.Texture != null)
                    {
                        hasTexture = true;
                        texture = textureRef.Texture;
                    }
                }

                if (hasTexture && firstSamplerWithTextureIndex == -1)
                    firstSamplerWithTextureIndex = _controls.SamplerTabControl.Items.Count;

                TextBlock tabHeader = new System.Windows.Controls.TextBlock
                {
                    Text = sampler,
                    FontWeight = hasTexture ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal
                };

                StackPanel tabContent = new StackPanel { Margin = new System.Windows.Thickness(10) };
                
                TextBlock textureFileText = new TextBlock 
                { 
                    Text = $"Sampler: {sampler}",
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(0, 0, 0, 10)
                };
                
                ScrollViewer imageScrollViewer = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
                };
                
                System.Windows.Controls.Image texturePreview = new System.Windows.Controls.Image
                {
                    Source = texture?.ToDDS()?.ToBitmap()?.ToImageSource(),
                    Stretch = Stretch.Uniform,
                    MaxHeight = 400
                };
                imageScrollViewer.Content = texturePreview;
                
                /*
                 * TODO!
                System.Windows.Controls.Button pickTextureButton = new System.Windows.Controls.Button
                {
                    Content = "Pick Texture...",
                    Margin = new System.Windows.Thickness(0, 10, 0, 0),
                    Tag = i
                };
                pickTextureButton.Click += (s, args) => OnPickTexture();
                */
                
                TextBlock detailsText = new TextBlock
                {
                    TextWrapping = System.Windows.TextWrapping.Wrap,
                    Margin = new System.Windows.Thickness(0, 10, 0, 0)
                };
                
                StringBuilder details = new StringBuilder();
                details.AppendLine($"Sampler Name: {sampler}");
                details.AppendLine($"Sampler Index: {samplerIndex}");
                details.AppendLine($"Texture Reference Index: {(textureRefIndex == 255 ? "Not assigned" : textureRefIndex.ToString())}");
                
                if (hasTexture && texture != null)
                {
                    textureFileText.Text += $"\nTexture: {texture.Name}";
                    details.AppendLine($"Texture Name: {texture.Name}");
                    //details.AppendLine($"Texture Width: {texture.Width}"); // TODO: get from child tex
                    //details.AppendLine($"Texture Height: {texture.Height}");
                    details.AppendLine($"Texture Format: {texture.Format}");
                }
                else if (textureRefIndex != 255 && textureRefIndex < material.TextureReferences.Count)
                {
                    textureFileText.Text += "\nTexture: (Empty slot)";
                    details.AppendLine("Texture: Empty slot (no texture assigned)");
                }
                else
                {
                    textureFileText.Text += "\nTexture: (Not assigned)";
                    details.AppendLine("Texture: Not assigned to this sampler");
                }
                
                detailsText.Text = details.ToString();
                
                tabContent.Children.Add(textureFileText);
                tabContent.Children.Add(imageScrollViewer);
                //tabContent.Children.Add(pickTextureButton); todo
                tabContent.Children.Add(detailsText);
                
                TabItem tabItem = new TabItem
                {
                    Header = tabHeader,
                    Content = tabContent
                };
                
                _controls.SamplerTabControl.Items.Add(tabItem);
                _samplerInfo.Add(new Tuple<string, int, int>(sampler, samplerIndex, textureRefIndex));
            }
            if (_controls.SamplerTabControl.Items.Count != 0)
            {
                int selectedIndex = firstSamplerWithTextureIndex >= 0 ? firstSamplerWithTextureIndex : 0;
                _controls.SamplerTabControl.SelectedIndex = selectedIndex;
            }

            List<string> features = ShaderUtility.GetFeatures(material.Shader.Ubershader);
            foreach (string feature in features)
            {
                int? featureIndexNullable = ShaderUtility.GetShaderFunctionalityIndex(material.Shader.Ubershader, ShaderIndexType.FEATURES, feature);
                if (!featureIndexNullable.HasValue) continue;

                int featureIndex = featureIndexNullable.Value;
                bool isEnabled = (material.Shader.UbershaderFeatureFlags & (1L << featureIndex)) != 0;

                System.Windows.Controls.CheckBox checkBox = new System.Windows.Controls.CheckBox
                {
                    Content = feature,
                    IsChecked = isEnabled,
                    Margin = new System.Windows.Thickness(0, 0, 0, 5)
                };

                checkBox.Checked += (s, args) => OnFeatureCheckboxChanged(material, featureIndex, true);
                checkBox.Unchecked += (s, args) => OnFeatureCheckboxChanged(material, featureIndex, false);

                _controls.featureDetailsPanel.Children.Add(checkBox);
            }

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

        }

        private void OnSamplerSelected(int samplerTabIndex)
        {
            //todo
        }

        private void OnPickTexture()
        {
            if (materialList.SelectedItems.Count == 0 || _controls.SamplerTabControl.SelectedIndex < 0) return;

            Materials.Material material = materialList.SelectedItems[0].Tag as Materials.Material;
            if (material == null) return;
            int samplerTabIndex = _controls.SamplerTabControl.SelectedIndex;
            if (samplerTabIndex >= _samplerInfo.Count) return;
            
            var samplerInfo = _samplerInfo[samplerTabIndex];
            string samplerName = samplerInfo.Item1;
            int samplerIndex = samplerInfo.Item2;
            int textureRefIndex = samplerInfo.Item3;

            //todo
        }

        private void OnFeatureCheckboxChanged(Materials.Material material, int featureIndex, bool isChecked)
        {
            if (isChecked)
                material.Shader.UbershaderFeatureFlags |= (1L << featureIndex);
            else
                material.Shader.UbershaderFeatureFlags &= ~(1L << featureIndex);
        }

        private void OnParameterSelected(string parameterName)
        {
            _controls.parameterDetailsPanel.Children.Clear();

            if (materialList.SelectedItems.Count == 0) return;

            Materials.Material material = materialList.SelectedItems[0].Tag as Materials.Material;
            if (material == null) return;

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
            if (materialList.SelectedItems.Count == 0) return;
            
            Materials.Material material = materialList.SelectedItems[0].Tag as Materials.Material;
            if (material == null) return;
            
            OnMaterialSelected?.Invoke(material);
            this.Close();
        }
    }
}
