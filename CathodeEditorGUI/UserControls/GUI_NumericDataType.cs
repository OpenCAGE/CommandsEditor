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
    public partial class GUI_NumericDataType : UserControl
    {
        CathodeFloat floatVal = null;
        CathodeInteger intVal = null;
        bool isIntInput = false;

        public GUI_NumericDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI_Float(CathodeFloat cFloat, ShortGuid paramID)
        {
            floatVal = cFloat;
            NUMERIC_VARIABLE_DUMMY.Text = ShortGuidUtils.FindString(paramID) + " (" + paramID.ToString() + ")";
            textBox1.Text = cFloat.value.ToString();
        }

        public void PopulateUI_Int(CathodeInteger cInt, ShortGuid paramID)
        {
            isIntInput = true;
            intVal = cInt;
            NUMERIC_VARIABLE_DUMMY.Text = ShortGuidUtils.FindString(paramID) + " (" + paramID.ToString() + ")";
            textBox1.Text = cInt.value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = EditorUtils.ForceStringNumeric(textBox1.Text, !isIntInput);
            if (isIntInput) intVal.value = Convert.ToInt32(textBox1.Text);
            else floatVal.value = Convert.ToSingle(textBox1.Text);
        }
    }
}
