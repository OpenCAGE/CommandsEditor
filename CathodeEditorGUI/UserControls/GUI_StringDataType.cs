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
using CATHODE;
using CathodeLib;

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_StringDataType : UserControl
    {
        CathodeString stringVal = null;

        public GUI_StringDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(CathodeString cString, ShortGuid paramID)
        {
            stringVal = cString;
            STRING_VARIABLE_DUMMY.Text = ShortGuidUtils.FindString(paramID) + " (" + paramID.ToString() + ")";
            textBox1.Text = cString.value;
            //textBox1.DataBindings.Add("Text", cString.value, "");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            stringVal.value = textBox1.Text;
        }
    }
}
