using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using System;
using System.Windows.Forms;

namespace CommandsEditor.UserControls
{
    public partial class GUI_Link : ParameterUserControl
    {
        public Action<Entity> GoToEntity;
        public Action<Entity, Entity> OnLinkEdited;

        private Entity _linkedEntity;

        private EntityConnector _link;
        private bool _isLinkOut;

        private EntityDisplay _entityDisplay;

        public GUI_Link(EntityDisplay editor) : base()
        {
            _entityDisplay = editor;
            InitializeComponent();
            this.ContextMenuStrip = contextMenuStrip1;
        }

        public void PopulateUI(EntityConnector link, bool isLinkOut, ShortGuid linkInGuid = new ShortGuid()) // linkInGuid only needs to be given if linkInGuid is false
        {
            _link = link;
            _isLinkOut = isLinkOut;

            if (isLinkOut)
            {
                _linkedEntity = _entityDisplay.Composite.GetEntityByID(link.childID);
                group.Text = ShortGuidUtils.FindString(link.parentParamID);
                label1.Text = "Connects OUT to \"" + ShortGuidUtils.FindString(link.childParamID) + "\" on: ";
                this.deleteToolStripMenuItem.Text = "Delete '" + ShortGuidUtils.FindString(link.parentParamID) + "'";
            }
            else
            {
                _linkedEntity = _entityDisplay.Composite.GetEntityByID(linkInGuid);
                group.Text = ShortGuidUtils.FindString(link.childParamID);
                label1.Text = "Connects IN from \"" + ShortGuidUtils.FindString(link.parentParamID) + "\" on: ";
                GoTo.Image = invIconResource.Image;
                this.deleteToolStripMenuItem.Text = "Delete '" + ShortGuidUtils.FindString(link.childParamID) + "'";
            }

            textBox1.Text = Content.editor_utils.GenerateEntityName(_linkedEntity, _entityDisplay.Composite);
        }

        private void GoTo_Click(object sender, EventArgs e)
        {
            GoToEntity?.Invoke(_linkedEntity);
        }

        private void EditLink_Click(object sender, EventArgs e)
        {
            AddOrEditLink editor;
            if (_isLinkOut)
                editor = new AddOrEditLink(_entityDisplay, _entityDisplay.Entity, _linkedEntity, ShortGuidUtils.FindString(_link.parentParamID), ShortGuidUtils.FindString(_link.childParamID), true, _link.connectionID);
            else
                editor = new AddOrEditLink(_entityDisplay, _linkedEntity, _entityDisplay.Entity, ShortGuidUtils.FindString(_link.parentParamID), ShortGuidUtils.FindString(_link.childParamID), false, _link.connectionID);

            editor.Show();
            editor.OnSaved += link_editor_OnSaved;
        }
        private void link_editor_OnSaved()
        {
            OnLinkEdited?.Invoke(_entityDisplay.Entity, _linkedEntity);
        }

        private void DeleteLink_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this link?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (_isLinkOut)
                _entityDisplay.Entity.childLinks.RemoveAll(o => o.connectionID == _link.connectionID);
            else
                _linkedEntity.childLinks.RemoveAll(o => o.connectionID == _link.connectionID);

            OnLinkEdited?.Invoke(_entityDisplay.Entity, _linkedEntity);
        }
    }
}
