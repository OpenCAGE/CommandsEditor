using System;
using System.Windows.Forms;
using CATHODE.Scripting;

namespace CommandsEditor.UserControls
{
    public partial class GUI_StringVariant_AssetDropdown : UserControl
    {
        cString stringVal = null;

        public GUI_StringVariant_AssetDropdown()
        {
            InitializeComponent();
        }

        public void PopulateUI(cString cString, ShortGuid paramID, AssetType assets)
        {
            switch (assets)
            {
                //POPULATE ASSET DROPDOWN
            }


            stringVal = cString;
            label1.Text = ShortGuidUtils.FindString(paramID);
            comboBox1.Text = cString.value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            stringVal.value = comboBox1.Text;
        }

        public enum AssetType
        {

        }
    }
}
