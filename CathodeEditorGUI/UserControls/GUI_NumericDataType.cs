using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Scripting;
using CATHODE;
using CathodeLib;

namespace CommandsEditor.UserControls
{
    public partial class GUI_NumericDataType : UserControl
    {
        cFloat floatVal = null;
        cInteger intVal = null;
        bool isIntInput = false;

        public GUI_NumericDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI_Float(cFloat cFloat, string paramID)
        {
            floatVal = cFloat;
            label1.Text = paramID;
            textBox1.Text = cFloat.value.ToString();
        }

        public void PopulateUI_Int(cInteger cInt, string paramID)
        {
            isIntInput = true;
            intVal = cInt;
            label1.Text = paramID;
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
