using System;
using System.Windows.Forms;
using CATHODE.Scripting;

namespace CommandsEditor.UserControls
{
    public partial class GUI_StringDataType : UserControl
    {
        cString stringVal = null;

        public GUI_StringDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(cString cString, string paramID)
        {
            stringVal = cString;
            label1.Text = paramID;
            textBox1.Text = cString.value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            stringVal.value = textBox1.Text;
        }
    }
}
