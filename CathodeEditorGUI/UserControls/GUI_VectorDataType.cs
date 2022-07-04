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
    public partial class GUI_VectorDataType : UserControl
    {
        CathodeVector3 vectorVal = null;

        public GUI_VectorDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(CathodeVector3 cVec, ShortGuid paramID)
        {
            vectorVal = cVec;
            label1.Text = ShortGuidUtils.FindString(paramID);
            POS_X_1.Value = (decimal)cVec.value.x;
            POS_Y_1.Value = (decimal)cVec.value.y;
            POS_Z_1.Value = (decimal)cVec.value.z;
        }

        private void POS_X_1_ValueChanged(object sender, EventArgs e)
        {
            vectorVal.value.x = (float)POS_X_1.Value;
        }

        private void POS_Y_1_ValueChanged(object sender, EventArgs e)
        {
            vectorVal.value.y = (float)POS_Y_1.Value;
        }

        private void POS_Z_1_ValueChanged(object sender, EventArgs e)
        {
            vectorVal.value.z = (float)POS_Z_1.Value;
        }
    }
}
