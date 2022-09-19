using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI.Popups
{
    public partial class CathodeEditorGUI_SelectMaterial : Form
    {
        public int SelectedMaterialIndex = -1;
        public int MaterialIndexToEdit = -1;

        public CathodeEditorGUI_SelectMaterial(int materialIndexToEdit, int defaultMaterialIndex = -1)
        {
            InitializeComponent();

            for (int i = 0; i < CurrentInstance.materialDB.MaterialNames.Count; i++)
            {
                materialList.Items.Add(CurrentInstance.materialDB.MaterialNames[i]);
            }

            MaterialIndexToEdit = materialIndexToEdit;
            if (defaultMaterialIndex != -1) materialList.SelectedIndex = defaultMaterialIndex;
        }

        private void selectMaterial_Click(object sender, EventArgs e)
        {
            SelectedMaterialIndex = materialList.SelectedIndex;
            this.Close();
        }
    }
}
