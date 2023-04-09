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
using CathodeLib;
using CATHODE;

namespace CommandsEditor.UserControls
{
    public partial class GUI_VectorVariant_Colour : UserControl
    {
        cVector3 vector = null;

        public GUI_VectorVariant_Colour()
        {
            InitializeComponent();
        }

        public void PopulateUI(cVector3 cVec, string paramID)
        {
            vector = cVec;
            GUID_VARIABLE_DUMMY.Text = paramID;
            pictureBox1.BackColor = VectorToColour();
        }

        private Color VectorToColour()
        {
            return Color.FromArgb((int)vector.value.X, (int)vector.value.Y, (int)vector.value.Z);
        }
        private void SetVectorFromColour(Color colour)
        {
            vector.value.X = colour.R;
            vector.value.Y = colour.G;
            vector.value.Z = colour.B;
        }

        private void openColourPicker_Click(object sender, EventArgs e)
        {
            ColorDialog colourPicker = new ColorDialog();
            colourPicker.Color = VectorToColour();

            if (colourPicker.ShowDialog() == DialogResult.OK)
            {
                SetVectorFromColour(colourPicker.Color);
                pictureBox1.BackColor = VectorToColour();
            }
        }
    }
}
