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
using Newtonsoft.Json;

namespace CommandsEditor.UserControls
{
    public partial class GUI_TransformDataType : ParameterUserControl
    {
        public Action OnValueChanged;

        cTransform transformVal = null;

        public GUI_TransformDataType()
        {
            InitializeComponent();
            this.ContextMenuStrip = contextMenuStrip1;
        }

        public void PopulateUI(cTransform cTrans, ShortGuid paramID)
        {
            PopulateUI(cTrans, ShortGuidUtils.FindString(paramID));
        }
        public void PopulateUI(cTransform cTrans, string title, bool disableInput = false)
        {
            POSITION_VARIABLE_DUMMY.Text = title;
            transformVal = cTrans;
            this.deleteToolStripMenuItem.Text = "Delete '" + title + "'";

            UpdateUI();

            if (disableInput)
            {
                POS_X.Enabled = false;
                POS_Y.Enabled = false;
                POS_Z.Enabled = false;
                ROT_X.Enabled = false;
                ROT_Y.Enabled = false;
                ROT_Z.Enabled = false;
            }
        }

        private void UpdateUI()
        {
            POS_X.Value = (decimal)transformVal.position.X;
            POS_Y.Value = (decimal)transformVal.position.Y;
            POS_Z.Value = (decimal)transformVal.position.Z;
            ROT_X.Value = (decimal)transformVal.rotation.X;
            ROT_Y.Value = (decimal)transformVal.rotation.Y;
            ROT_Z.Value = (decimal)transformVal.rotation.Z;
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

        private void copyTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(JsonConvert.SerializeObject(transformVal));
        }

        private void pasteTransformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!POS_X.Enabled) return;

            string val = Clipboard.GetText()?.ToString();
            cTransform transform = null;
            try
            {
                transform = JsonConvert.DeserializeObject<cTransform>(val);
            }
            catch { }
            if (transform == null)
            {
                MessageBox.Show("Failed to paste transform.", "Invalid clipboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            transformVal.position.X = transform.position.X;
            transformVal.position.Y = transform.position.Y;
            transformVal.position.Z = transform.position.Z;
            transformVal.rotation.X = transform.rotation.X;
            transformVal.rotation.Y = transform.rotation.Y;
            transformVal.rotation.Z = transform.rotation.Z;

            UpdateUI();
        }
    }
}
