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
    public partial class GUI_TransformDataType : UserControl
    {
        public Action OnValueChanged;

        cTransform transformVal = null;

        public GUI_TransformDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(cTransform cTrans, ShortGuid paramID)
        {
            PopulateUI(cTrans, (paramID.val == null) ? "" : ShortGuidUtils.FindString(paramID));
        }
        public void PopulateUI(cTransform cTrans, string title)
        {
            POSITION_VARIABLE_DUMMY.Text = title;
            transformVal = cTrans;
            POS_X.Value = (decimal)cTrans.position.X;
            POS_Y.Value = (decimal)cTrans.position.Y;
            POS_Z.Value = (decimal)cTrans.position.Z;
            ROT_X.Value = (decimal)cTrans.rotation.X;
            ROT_Y.Value = (decimal)cTrans.rotation.Y;
            ROT_Z.Value = (decimal)cTrans.rotation.Z;
        }

        private void POS_X_ValueChanged(object sender, EventArgs e)
        {
            transformVal.position.X = (float)POS_X.Value;
            OnValueChanged?.Invoke();
        }

        private void POS_Y_ValueChanged(object sender, EventArgs e)
        {
            transformVal.position.Y = (float)POS_Y.Value;
            OnValueChanged?.Invoke();
        }

        private void POS_Z_ValueChanged(object sender, EventArgs e)
        {
            transformVal.position.Z = (float)POS_Z.Value;
            OnValueChanged?.Invoke();
        }

        private void ROT_X_ValueChanged(object sender, EventArgs e)
        {
            transformVal.rotation.X = (float)ROT_X.Value;
            OnValueChanged?.Invoke();
        }

        private void ROT_Y_ValueChanged(object sender, EventArgs e)
        {
            transformVal.rotation.Y = (float)ROT_Y.Value;
            OnValueChanged?.Invoke();
        }

        private void ROT_Z_ValueChanged(object sender, EventArgs e)
        {
            transformVal.rotation.Z = (float)ROT_Z.Value;
            OnValueChanged?.Invoke();
        }
    }
}
