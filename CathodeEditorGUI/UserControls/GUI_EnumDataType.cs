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
    public partial class GUI_EnumDataType : UserControl
    {
        CathodeEnum enumVal = null;

        public GUI_EnumDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(CathodeEnum cEnum, cGUID paramID)
        {
            enumVal = cEnum;
            ENUM_VARIABLE_DUMMY.Text = NodeDB.GetName(paramID) + " (" + paramID.ToString() + ")";
            comboBox1.Text = NodeDB.GetEnum(cEnum.enumID).Name;
            //comboBox1.Enabled = false;
            numericUpDown8.Value = cEnum.enumIndex;
            //numericUpDown8.DataBindings.Add("Value", cEnum.enumIndex, "");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            enumVal.enumID = Utilities.GenerateGUID(comboBox1.Text);
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            enumVal.enumIndex = (int)numericUpDown8.Value;
        }
    }
}
