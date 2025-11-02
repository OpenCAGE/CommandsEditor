using CATHODE;
using CATHODE.LEGACY;
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
using static CATHODE.Materials.Material;

//This is ported directly from AlienPAK, with its editing functionality stripped out.
//Eventually we should support editing here too, and deprecate AlienPAK.

namespace AlienPAK
{
    public partial class SelectMaterial : BaseWindow
    {
        List<Materials.Material> _sortedMaterials = new List<Materials.Material>();
        ShadersPAK.ShaderMaterialMetadata _selectedMaterialMeta = null;
        ShadersPAK.ShaderEntry _selectedMaterialShader = null;

        MaterialEditorControlsWPF _controls = null;

        public Action<int> OnMaterialSelected;

        public SelectMaterial(Materials.Material material = null) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            this.Text = "Select Material";
            duplicateMaterial.Visible = false;

            _controls = (MaterialEditorControlsWPF)elementHost1.Child;
            _controls.OnMaterialTextureIndexSelected += OnMaterialTextureIndexSelected;

            PopulateUI(material);
        }

        private void UpdateTextureDropdown(bool global, bool changeIndex = false)
        {
            Debug.Log("Material Selector", "UpdateTextureDropdown");
            List<string> textures = new List<string>();
            Textures textureDB = global ? Singleton.GlobalTextures : Content.resource.textures;
            for (int i = 0; i < textureDB.Entries.Count; i++) textures.Add(textureDB.Entries[i].Name);
            textures.Add("NONE"); //temp holder for no texture

            if (!changeIndex) return;
            Debug.Log("Material Selector", " --> UPDATING INDEX");
            _controls.textureFile.Text = "";
            OnTextureIndexChange(0, global);
        }

        private void OnTextureIndexChange(int index, bool global)
        {
            Debug.Log("Material Selector", "OnTextureIndexChange");
            Textures texDB = (global ? Singleton.GlobalTextures : Content.resource.textures);
            ShadersPAK.MaterialTextureContext textureInfo = _selectedMaterialMeta.textures[_controls.materialTextureSelection.SelectedIndex];
            if (textureInfo.TextureInfo == null)
            {
                TexturePtr tex = new TexturePtr();
                textureInfo.TextureInfo = tex;
                _sortedMaterials[materialList.SelectedIndex].TextureReferences[_controls.materialTextureSelection.SelectedIndex] = tex;
            }
            if (index >= texDB.Entries.Count)
            {
                textureInfo.TextureInfo = null;
                _sortedMaterials[materialList.SelectedIndex].TextureReferences[_controls.materialTextureSelection.SelectedIndex] = null;
            }
            else
            {
                textureInfo.TextureInfo.Index = texDB.GetWriteIndex(texDB.Entries[index]);
                textureInfo.TextureInfo.Location = global ? TexturePtr.Source.GLOBAL : TexturePtr.Source.LEVEL;
            }
            ShowTextureForMaterial(_controls.materialTextureSelection.SelectedIndex);
        }

        private void PopulateUI(Materials.Material material = null)
        {
            Debug.Log("Material Selector", "PopulateUI");
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

        private void OnMaterialTextureIndexSelected(int index)
        {
            Debug.Log("Material Selector", "OnMaterialTextureIndexSelected");
            ShowTextureForMaterial(_controls.materialTextureSelection.SelectedIndex);
        }

        private void materialList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.Log("Material Selector", "materialList_SelectedIndexChanged");
            _selectedMaterialMeta = null;
            _selectedMaterialShader = null;
            if (materialList.SelectedIndex == -1)
            {
                return;
            }

            _selectedMaterialMeta = Content.resource.shaders_legacy.GetMaterialMetadataFromShader(_sortedMaterials[materialList.SelectedIndex]);
            _selectedMaterialShader = Content.resource.shaders_legacy.Shaders[_sortedMaterials[materialList.SelectedIndex].ShaderIndex];

            _controls.fileNameText.Text = _sortedMaterials[materialList.SelectedIndex].Name;

            _controls.materialTextureSelection.Items.Clear();
            for (int i = 0; i < _selectedMaterialMeta.textures.Count; i++)
            {
                _controls.materialTextureSelection.Items.Add(_selectedMaterialMeta.textures[i].Type.ToString());
                if (_selectedMaterialMeta.textures[i].Type == ShadersPAK.ShaderSlot.DIFFUSE_MAP) _controls.materialTextureSelection.SelectedIndex = i;
            }
            _controls.ShowTexturePreview(_selectedMaterialMeta.textures.Count != 0);
            if (_controls.materialTextureSelection.SelectedIndex == -1 && _selectedMaterialMeta.textures.Count != 0)
                _controls.materialTextureSelection.SelectedIndex = 0;
            ShowTextureForMaterial(_controls.materialTextureSelection.SelectedIndex);

            _controls.shaderName.Text = _selectedMaterialShader.Index + " (" + _selectedMaterialMeta.shaderCategory.ToString() + ")";
        }

        private void ShowTextureForMaterial(int index)
        {
            Debug.Log("Material Selector", "ShowTextureForMaterial");
            _controls.materialTexturePreview.Source = null;
            if (index == -1) return;
            ShadersPAK.MaterialTextureContext mdlMetaDiff = _selectedMaterialMeta.textures[index];
            if (mdlMetaDiff == null || mdlMetaDiff.TextureInfo == null)
            {
                _controls.textureFile.Text = "NONE";
                _controls.materialTexturePreview.Source = null;
                return;
            }

            UpdateTextureDropdown(mdlMetaDiff.TextureInfo.Location == TexturePtr.Source.GLOBAL);

            Textures.TEX4 diff = (mdlMetaDiff.TextureInfo.Location == TexturePtr.Source.GLOBAL ? Singleton.GlobalTextures : Content.resource.textures).GetAtWriteIndex(mdlMetaDiff.TextureInfo.Index);
            _controls.textureFile.Text = diff == null ? "NONE" : diff.Name;
            _controls.materialTexturePreview.Source = diff?.ToDDS()?.ToBitmap()?.ToImageSource();
        }

        private T LoadFromCST<T>(ref BinaryReader cstReader, int offset)
        {
            cstReader.BaseStream.Position = offset;
            return Utilities.Consume<T>(cstReader);
        }
        private void WriteToCST<T>(ref BinaryWriter cstWriter, int offset, T content)
        {
            cstWriter.BaseStream.Position = offset;
            Utilities.Write<T>(cstWriter, content);
        }
        private bool CSTIndexValid(int i, ref ShadersPAK.ShaderEntry Shader)
        {
            return i >= 0 && i < Shader.Header.CSTCounts[2] && (int)Shader.CSTLinks[2][i] != -1 && Shader.CSTLinks[2][i] != 255;
        }

        private void selectMaterial_Click(object sender, EventArgs e)
        {
            OnMaterialSelected?.Invoke(Content.resource.materials.GetWriteIndex(_sortedMaterials[materialList.SelectedIndex]));
            this.Close();
        }

        private void duplicateMaterial_Click(object sender, EventArgs e)
        {
            //This code has been ported from AlienPAK and will not work out the box because it'll potentially screw with indexes in the files that we reference elsewhere.
            //TODO: Need to review.

            /*
            Materials.Material newMaterial = _sortedMaterials[materialList.SelectedIndex].Copy();
            newMaterial.Name += " Clone";
            for (int i = 0; i < newMaterial.ConstantBuffers.Length; i++)
            {
                if (newMaterial.ConstantBuffers == null) continue;
                byte[] cstData = null;
                using (MemoryStream stream = new MemoryStream(Content.resource.materials.CSTData[i]))
                {
                    using (BinaryReader cstReader = new BinaryReader(stream))
                    {
                        cstReader.BaseStream.Position = newMaterial.ConstantBuffers[i].Offset * 4;
                        cstData = cstReader.ReadBytes(newMaterial.ConstantBuffers[i].Length * 4);
                    }
                }
                newMaterial.ConstantBuffers[i] = new Materials.Material.ConstantBuffer();
                if (cstData != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (BinaryWriter cstWriter = new BinaryWriter(stream))
                        {
                            cstWriter.Write(Content.resource.materials.CSTData[i]);
                            newMaterial.ConstantBuffers[i].Offset = (int)cstWriter.BaseStream.Position / 4;
                            newMaterial.ConstantBuffers[i].Length = cstData.Length / 4;
                            cstWriter.Write(cstData);

                            Content.resource.materials.CSTData[i] = stream.ToArray();
                        }
                    }
                }
            }
            Content.resource.materials.Entries.Add(newMaterial);
            Explorer.SaveTexturesAndUpdateMaterials(Content.resource.textures, Content.resource.materials);
            PopulateUI(newMaterial);
            */
        }
    }
}
