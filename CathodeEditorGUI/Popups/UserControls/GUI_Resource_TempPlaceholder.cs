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
    public partial class GUI_Resource_TempPlaceholder : ResourceUserControl
    {
        public GUI_Resource_TempPlaceholder()
        {
            InitializeComponent();
        }

        public void PopulateUI(string type, string alsoUsedBy)
        {
            resourceTypeInfo.Text = type;

            //TEMP
            if (alsoUsedBy != "")
                groupBox1.Text += " (Also used by: " + alsoUsedBy + ")";
        }
    }
}
