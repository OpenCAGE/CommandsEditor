using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI.Popups.UserControls
{
    public partial class GUI_Resource_DynamicPhysicsSystem : ResourceUserControl
    {
        public int UnknownIndex = -1;

        public GUI_Resource_DynamicPhysicsSystem()
        {
            InitializeComponent();
        }

        public void PopulateUI(int index)
        {
            type_placeholder.Text = index.ToString();
            UnknownIndex = index;
        }

        private void type_placeholder_TextChanged(object sender, EventArgs e)
        {
            UnknownIndex = Convert.ToInt32(type_placeholder.Text);
        }
    }
}
