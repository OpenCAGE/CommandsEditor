using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Commands;
using CathodeLib;
using CATHODE;

namespace CathodeEditorGUI.Popups.UserControls
{
    public partial class GUI_Resource_RenderableInstance : UserControl
    {
        public GUI_Resource_RenderableInstance()
        {
            InitializeComponent();
        }

        public void PopulateUI(string test1, string test2)
        {
            modelInfoTextbox.Text = test1;
            materialInfoTextbox.Text = test2;
        }

        private void showModelInfo_Click(object sender, EventArgs e)
        {

        }

        private void showMaterialInfo_Click(object sender, EventArgs e)
        {

        }

        private void selectNewModel_Click(object sender, EventArgs e)
        {

        }

        private void selectNewMaterial_Click(object sender, EventArgs e)
        {

        }
    }
}
