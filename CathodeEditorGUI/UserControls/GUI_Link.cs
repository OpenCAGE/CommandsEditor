using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using System;
using System.Windows.Forms;

namespace CommandsEditor.UserControls
{
    public partial class GUI_Link : BaseUserControl
    {
        public Action<Entity> GoToEntity;
        private Entity _linkedEntity;

        private EntityLink _link;
        private bool _isLinkOut;

        private EntityDisplay _entityDisplay;

        public GUI_Link(EntityDisplay editor) : base(editor.Content)
        {
            _entityDisplay = editor;
            InitializeComponent();
        }

        public void PopulateUI(EntityLink link, bool isLinkOut, ShortGuid linkInGuid = new ShortGuid()) // linkInGuid only needs to be given if linkInGuid is false
        {
            _link = link;
            _isLinkOut = isLinkOut;

            if (isLinkOut)
            {
                _linkedEntity = _entityDisplay.Composite.GetEntityByID(link.childID);
                group.Text = ShortGuidUtils.FindString(link.parentParamID);
                label1.Text = "Connects OUT to \"" + ShortGuidUtils.FindString(link.childParamID) + "\" on: ";
            }
            else
            {
                _linkedEntity = _entityDisplay.Composite.GetEntityByID(linkInGuid);
                group.Text = ShortGuidUtils.FindString(link.childParamID);
                label1.Text = "Connects IN from \"" + ShortGuidUtils.FindString(link.parentParamID) + "\" on: ";
            }

            textBox1.Text = Editor.editor_utils.GenerateEntityName(_linkedEntity, _entityDisplay.Composite);
        }

        private void GoTo_Click(object sender, EventArgs e)
        {
            GoToEntity?.Invoke(_linkedEntity);
        }

        private void EditLink_Click(object sender, EventArgs e)
        {
            AddOrEditLink editor;
            if (_isLinkOut)
                editor = new AddOrEditLink(_entityDisplay, _entityDisplay.Composite, _entityDisplay.Entity, _linkedEntity, ShortGuidUtils.FindString(_link.parentParamID), ShortGuidUtils.FindString(_link.childParamID), true, _link.connectionID);
            else
                editor = new AddOrEditLink(_entityDisplay, _entityDisplay.Composite, _linkedEntity, _entityDisplay.Entity, ShortGuidUtils.FindString(_link.parentParamID), ShortGuidUtils.FindString(_link.childParamID), false, _link.connectionID);

            editor.Show();
            editor.OnSaved += link_editor_OnSaved;
        }
        private void link_editor_OnSaved()
        {
            GoToEntity?.Invoke(_entityDisplay.Entity);
        }

        private void DeleteLink_Click(object sender, EventArgs e)
        {
            if (_isLinkOut)
                _entityDisplay.Entity.childLinks.RemoveAll(o => o.connectionID == _link.connectionID);
            else
                _linkedEntity.childLinks.RemoveAll(o => o.connectionID == _link.connectionID);

            GoToEntity?.Invoke(_entityDisplay.Entity);
        }
    }
}
