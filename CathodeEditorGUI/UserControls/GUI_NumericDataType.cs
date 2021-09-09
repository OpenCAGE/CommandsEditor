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
            NUMERIC_VARIABLE_DUMMY.Text = NodeDB.GetCathodeName(paramID) + " (" + paramID.ToString() + ")";
            textBox1.Text = cFloat.value.ToString();
        }

        public void PopulateUI_Int(CathodeInteger cInt, cGUID paramID)
        {
            intVal = cInt;
            NUMERIC_VARIABLE_DUMMY.Text = NodeDB.GetCathodeName(paramID) + " (" + paramID.ToString() + ")";
            textBox1.Text = cInt.value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (floatVal != null) floatVal.value = Convert.ToSingle(textBox1.Text);
            if (intVal != null) intVal.value = Convert.ToInt32(textBox1.Text);
        }
    }
}
