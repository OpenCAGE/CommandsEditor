using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using System;
using System.Windows.Forms;

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_Link : UserControl
    {
        public Action<Entity> GoToEntity;
        private Entity _linkedEntity;

        private EntityLink _link;
        private bool _isLinkOut;

        public GUI_Link()
        {
            InitializeComponent();
        }

        public void PopulateUI(EntityLink link, bool isLinkOut, ShortGuid linkInGuid = new ShortGuid()) // linkInGuid only needs to be given if linkInGuid is false
        {
            _link = link;
            _isLinkOut = isLinkOut;

            if (isLinkOut)
            {
                _linkedEntity = Editor.selected.composite.GetEntityByID(link.childID);
                group.Text = ShortGuidUtils.FindString(link.parentParamID);
                label1.Text = "Connects OUT to \"" + ShortGuidUtils.FindString(link.childParamID) + "\" on: ";
            }
            else
            {
                _linkedEntity = Editor.selected.composite.GetEntityByID(linkInGuid);
                group.Text = ShortGuidUtils.FindString(link.childParamID);
                label1.Text = "Connects IN from \"" + ShortGuidUtils.FindString(link.parentParamID) + "\" on: ";
            }

            textBox1.Text = EditorUtils.GenerateEntityName(_linkedEntity, Editor.selected.composite);
        }

        private void GoTo_Click(object sender, EventArgs e)
        {
            GoToEntity?.Invoke(_linkedEntity);
        }

        private void EditLink_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_AddOrEditLink editor;
            if (_isLinkOut)
                editor = new CathodeEditorGUI_AddOrEditLink(Editor.selected.composite, Editor.selected.entity, _linkedEntity, _link, true);
            else
                editor = new CathodeEditorGUI_AddOrEditLink(Editor.selected.composite, _linkedEntity, Editor.selected.entity, _link, false);

            editor.Show();
            editor.OnSaved += link_editor_OnSaved;
        }
        private void link_editor_OnSaved()
        {
            GoToEntity?.Invoke(Editor.selected.entity);
        }
    }
}
