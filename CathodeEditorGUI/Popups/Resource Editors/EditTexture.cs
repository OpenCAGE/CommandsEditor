using AlienPAK;
using CATHODE;
using CATHODE.Animations;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CommandsEditor
{
    public partial class EditTexture : BaseWindow
    {
        public Action<Textures.TEX4> OnTextureSelected;

        TreeUtility _treeHelper;
        Textures.TEX4 _selectedTexture;

        public EditTexture(Textures.TEX4 currentMapping = null, bool showSelectBtn = true) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _treeHelper = new TreeUtility(FileTree, true);
            {
                List<string> textureNames = new List<string>();
                for (int i = 0; i < Content.Level.Textures.Entries.Count; i++)
                {
                    string texPath = Content.Level.Textures.Entries[i].Name;
                    Content.Level.Textures.Entries[i].Name = texPath;
                    textureNames.Add(texPath);
                }
                _treeHelper.UpdateFileTree(textureNames, null);
            }

            selectTextureBtn.Visible = showSelectBtn;
        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            texturePreview.BackgroundImage = null;
            textureName.Text = "";
            textureResolutionStreamed.Text = "";
            textureResolutionPersistent.Text = "";
            selectTextureBtn.Enabled = false;
            _selectedTexture = null;

            if (FileTree.SelectedNode == null) return;

            TreeItemType nodeType = ((TreeItem)FileTree.SelectedNode.Tag).Item_Type;
            string nodeVal = ((TreeItem)FileTree.SelectedNode.Tag).String_Value;
            switch (nodeType)
            {
                case TreeItemType.EXPORTABLE_FILE:
                    Textures.TEX4 texture = Content.Level.Textures.Entries.FirstOrDefault(o => o.Name.Replace('\\', '/') == nodeVal.Replace('\\', '/'));
                    texturePreview.BackgroundImage = texture?.ToBitmap();
                    textureName.Text = texture.Name;
                    textureResolutionStreamed.Text = texture.TextureStreamed != null ? texture.TextureStreamed.Width + "x" + texture.TextureStreamed.Height : "";
                    textureResolutionPersistent.Text = texture.TexturePersistent != null ? texture.TexturePersistent.Width + "x" + texture.TexturePersistent.Height : "";
                    selectTextureBtn.Enabled = true;
                    _selectedTexture = texture;
                    break;
            }
        }

        private void selectTextureBtn_Click(object sender, EventArgs e)
        {
            OnTextureSelected?.Invoke(_selectedTexture);
            this.Close();
        }
    }
}
