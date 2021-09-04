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

        public GUI_NumericDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI_Float(CathodeFloat cFloat, cGUID paramID)
        {
            floatVal = cFloat;
            NUMERIC_VARIABLE_DUMMY.Text = NodeDB.GetName(paramID) + " (" + paramID.ToString() + ")";
            //numericUpDown7.Value = (decimal)cFloat.value;
            textBox1.Text = cFloat.value.ToString();
            //numericUpDown7.DataBindings.Add("Value", (decimal)cFloat.value, "");
            //((NumericUpDown)parameterGUI.Controls[0]).   - Set float type not int
        }

        public void PopulateUI_Int(CathodeInteger cInt, cGUID paramID)
        {
            intVal = cInt;
            NUMERIC_VARIABLE_DUMMY.Text = NodeDB.GetName(paramID) + " (" + paramID.ToString() + ")";
            //numericUpDown7.Value = (decimal)cInt.value;
            textBox1.Text = cInt.value.ToString();
            //numericUpDown7.DataBindings.Add("Value", (decimal)cInt.value, "");
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (floatVal != null) floatVal.value = (float)numericUpDown7.Value;
            if (intVal != null) intVal.value = (int)numericUpDown7.Value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (floatVal != null) floatVal.value = Convert.ToSingle(textBox1.Text);
            if (intVal != null) intVal.value = Convert.ToInt32(textBox1.Text);
        }
    }
}
