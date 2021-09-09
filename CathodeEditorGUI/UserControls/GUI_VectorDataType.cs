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

        public void PopulateUI(CathodeVector3 cVec, cGUID paramID)
        {
            vectorVal = cVec;
            VECTOR_VARIABLE_DUMMY.Text = NodeDB.GetCathodeName(paramID) + " (" + paramID.ToString() + ")";
            POS_X_1.Value = (decimal)cVec.value.X;
            POS_Y_1.Value = (decimal)cVec.value.Y;
            POS_Z_1.Value = (decimal)cVec.value.Z;
            //POS_X_1.DataBindings.Add("Value", (decimal)cVec.value.X, "");
            //POS_Y_1.DataBindings.Add("Value", (decimal)cVec.value.Y, "");
            //POS_Z_1.DataBindings.Add("Value", (decimal)cVec.value.Z, "");
        }

        private void POS_X_1_ValueChanged(object sender, EventArgs e)
        {
            vectorVal.value.X = (float)POS_X_1.Value;
        }

        private void POS_Y_1_ValueChanged(object sender, EventArgs e)
        {
            vectorVal.value.Y = (float)POS_Y_1.Value;
        }

        private void POS_Z_1_ValueChanged(object sender, EventArgs e)
        {
            vectorVal.value.Z = (float)POS_Z_1.Value;
        }
    }
}
