using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_Link : UserControl
    {
        public Action<Entity> GoToEntity;
        private Entity _linkedEntity;

        private CathodeEditorGUI_AddOrEditLink _editor;

        public GUI_Link()
        {
            InitializeComponent();
        }

        public void PopulateUI(EntityLink link, bool isLinkOut, ShortGuid linkInGuid = new ShortGuid()) // linkInGuid only needs to be given if linkInGuid is false
        {
            if (isLinkOut)
            {
                _linkedEntity = Editor.selected.composite.GetEntityByID(link.childID);
                group.Text = ShortGuidUtils.FindString(link.parentParamID);
                label1.Text = "Connects OUT to \"" + ShortGuidUtils.FindString(link.childParamID) + "\" on: ";
                _editor = new CathodeEditorGUI_AddOrEditLink(Editor.selected.composite, Editor.selected.entity, _linkedEntity, link, true);
            }
            else
            {
                _linkedEntity = Editor.selected.composite.GetEntityByID(linkInGuid);
                group.Text = ShortGuidUtils.FindString(link.childParamID);
                label1.Text = "Connects IN from \"" + ShortGuidUtils.FindString(link.parentParamID) + "\" on: ";
                _editor = new CathodeEditorGUI_AddOrEditLink(Editor.selected.composite, _linkedEntity, Editor.selected.entity, link, false);
            }

            textBox1.Text = EditorUtils.GenerateEntityName(_linkedEntity, Editor.selected.composite);
        }

        private void GoTo_Click(object sender, EventArgs e)
        {
            GoToEntity?.Invoke(_linkedEntity);
        }

        private void EditLink_Click(object sender, EventArgs e)
        {
            _editor.Show();
            _editor.OnSaved += link_editor_OnSaved;
        }
        private void link_editor_OnSaved()
        {
            GoToEntity?.Invoke(Editor.selected.entity);
        }
    }
}
