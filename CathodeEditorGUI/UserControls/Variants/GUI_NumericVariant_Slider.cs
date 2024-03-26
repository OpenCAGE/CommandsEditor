using System;
using System.Windows.Forms;
using CATHODE.Scripting;

namespace CommandsEditor.UserControls
{
    public partial class GUI_NumericVariant_Slider : ParameterUserControl
    {
        cFloat floatVal = null;
        cInteger intVal = null;
        bool isIntInput = false;
        int floatPrecision = 1;

        public GUI_NumericVariant_Slider()
        {
            InitializeComponent();
            this.ContextMenuStrip = contextMenuStrip1;
        }

        public void PopulateUI_Float(cFloat cFloat, ShortGuid paramID, float min = 0, float max = 1, int precision = 5)
        {
            floatVal = cFloat;
            floatPrecision = precision;
            label1.Text = ShortGuidUtils.FindString(paramID);
            this.deleteToolStripMenuItem.Text = "Delete '" + ShortGuidUtils.FindString(paramID) + "'";

            trackBar1.Minimum = (int)(min * (10 ^ precision));
            trackBar1.Maximum = (int)(max * (10 ^ precision));
            trackBar1.Value = (int)(cFloat.value * (10 ^ precision));
        }

        public void PopulateUI_Int(cInteger cInt, ShortGuid paramID, int min = 0, int max = 1)
        {
            isIntInput = true;
            intVal = cInt;
            label1.Text = ShortGuidUtils.FindString(paramID);
            this.deleteToolStripMenuItem.Text = "Delete '" + ShortGuidUtils.FindString(paramID) + "'";

            trackBar1.Minimum = min;
            trackBar1.Maximum = max;
            trackBar1.Value = cInt.value;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (isIntInput) intVal.value = trackBar1.Value;
            else floatVal.value = trackBar1.Value / (10 ^ floatPrecision);
        }
    }
}
