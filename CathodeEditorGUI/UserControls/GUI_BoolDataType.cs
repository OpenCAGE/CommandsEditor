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
        CathodeBool boolVal = null;

        public GUI_BoolDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(CathodeBool cBool, cGUID paramID)
        {
            boolVal = cBool;
            BOOL_VARIABLE_DUMMY.Text = NodeDB.GetCathodeName(paramID) + " (" + paramID.ToString() + ")";
            checkBox1.Checked = cBool.value;
            //checkBox1.DataBindings.Add("Checked", cBool.value, "");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            boolVal.value = checkBox1.Checked;
        }
    }
}
