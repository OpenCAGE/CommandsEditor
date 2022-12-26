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
        cEnum enumVal = null;

        public GUI_EnumDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(cEnum cEnum, ShortGuid paramID)
        {
            enumVal = cEnum;
            label13.Text = ShortGuidUtils.FindString(paramID);
            comboBox1.Text = EntityDB.GetEnum(cEnum.enumID).Name;
            textBox1.Text = cEnum.enumIndex.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            enumVal.enumID = ShortGuidUtils.Generate(comboBox1.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = EditorUtils.ForceStringNumeric(textBox1.Text);
            enumVal.enumIndex = Convert.ToInt32(textBox1.Text);
        }
    }
}
