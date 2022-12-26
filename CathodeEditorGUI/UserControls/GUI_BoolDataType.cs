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

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_BoolDataType : UserControl
    {
        cBool boolVal = null;

        public GUI_BoolDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(cBool cBool, ShortGuid paramID)
        {
            boolVal = cBool;
            checkBox1.Text = ShortGuidUtils.FindString(paramID);
            checkBox1.Checked = cBool.value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            boolVal.value = checkBox1.Checked;
        }
    }
}
