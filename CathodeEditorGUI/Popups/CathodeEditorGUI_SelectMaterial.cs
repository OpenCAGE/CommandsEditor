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

        List<ListedMaterial> _materials = new List<ListedMaterial>();

        public CathodeEditorGUI_SelectMaterial(int materialIndexToEdit, int defaultMaterialIndex = -1)
        {
            InitializeComponent();

            MaterialIndexToEdit = materialIndexToEdit;
            for (int i = 0; i < Editor.resource.materials.MaterialNames.Count; i++)
                _materials.Add(new ListedMaterial(Editor.resource.materials.MaterialNames[i], i));
            _materials.Sort();
            _materials.Reverse();

            for (int i = 0; i < _materials.Count; i++)
            {
                materialList.Items.Add(_materials[i].MaterialName);
                if (materialIndexToEdit != -1 && _materials[i].Index == defaultMaterialIndex)
                    materialList.SelectedIndex = i;
            }
        }

        private void selectMaterial_Click(object sender, EventArgs e)
        {
            SelectedMaterialIndex = _materials[materialList.SelectedIndex].Index;
            this.Close();
        }

        private class ListedMaterial : IComparable
        {
            public ListedMaterial(string name, int index)
            {
                MaterialName = name;
                Index = index;
            }

            public string MaterialName;
            public int Index;

            public int CompareTo(object obj)
            {
                if (!(obj is ListedMaterial)) return 0;
                return string.Compare(((ListedMaterial)obj).MaterialName, MaterialName);
            }
        }
    }
}
