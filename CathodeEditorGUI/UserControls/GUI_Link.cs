using CATHODE.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_Link : UserControl
    {
        public Action<Entity> GoToEntity;
        private Entity _linkedEntity;

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

        }
    }
}
