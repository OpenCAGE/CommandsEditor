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
    public partial class GUI_TransformDataType : UserControl
    {
        CathodeTransform transformVal = null;

        public GUI_TransformDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(CathodeTransform cTrans, cGUID paramID)
        {
            transformVal = cTrans;
            POSITION_VARIABLE_DUMMY.Text = EntityDBEx.GetParameterName(paramID) + " (" + paramID.ToString() + ")";
            POS_X.Value = (decimal)cTrans.position.x;
            POS_Y.Value = (decimal)cTrans.position.y;
            POS_Z.Value = (decimal)cTrans.position.z;
            ROT_X.Value = (decimal)cTrans.rotation.x;
            ROT_Y.Value = (decimal)cTrans.rotation.y;
            ROT_Z.Value = (decimal)cTrans.rotation.z;
            //POS_X.DataBindings.Add("Value", (decimal)cTrans.position.X, "");
            //POS_Y.DataBindings.Add("Value", (decimal)cTrans.position.Y, "");
            //POS_Z.DataBindings.Add("Value", (decimal)cTrans.position.Z, "");
            //ROT_X.DataBindings.Add("Value", (decimal)cTrans.rotation.X, "");
            //ROT_Y.DataBindings.Add("Value", (decimal)cTrans.rotation.Y, "");
            //ROT_Z.DataBindings.Add("Value", (decimal)cTrans.rotation.Z, "");
        }

        private void POS_X_ValueChanged(object sender, EventArgs e)
        {
            transformVal.position.x = (float)POS_X.Value;
        }

        private void POS_Y_ValueChanged(object sender, EventArgs e)
        {
            transformVal.position.y = (float)POS_Y.Value;
        }

        private void POS_Z_ValueChanged(object sender, EventArgs e)
        {
            transformVal.position.z = (float)POS_Z.Value;
        }

        private void ROT_X_ValueChanged(object sender, EventArgs e)
        {
            transformVal.rotation.x = (float)ROT_X.Value;
        }

        private void ROT_Y_ValueChanged(object sender, EventArgs e)
        {
            transformVal.rotation.y = (float)ROT_Y.Value;
        }

        private void ROT_Z_ValueChanged(object sender, EventArgs e)
        {
            transformVal.rotation.z = (float)ROT_Z.Value;
        }
    }
}
