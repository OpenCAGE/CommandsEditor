using System;
using System.Linq;
using System.Windows.Forms;
using CATHODE.Scripting;
using CathodeLib;

namespace CommandsEditor.UserControls
{
    public partial class GUI_EnumDataType : UserControl
    {
        cEnum enumVal = null;
        EnumUtils.EnumDescriptor enumDesc = null;

        public GUI_EnumDataType()
        {
            InitializeComponent();

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(EnumType)));
            comboBox1.EndUpdate();
        }

        public void PopulateUI(cEnum cEnum, string paramID)
        {
            label13.Text = paramID;

            enumVal = cEnum;
            enumDesc = EnumUtils.GetEnum(cEnum.enumID);

            comboBox1.Text = enumDesc.Name;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            PopulateEnumEntries();

            EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == cEnum.enumIndex);
            if (enumEntry == null)
                MessageBox.Show("WARNING!!! COULD NOT MATCH ENUM!!!");
            else
                comboBox2.SelectedItem = enumEntry.Name;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            enumVal.enumID = ShortGuidUtils.Generate(comboBox1.Text);
            enumDesc = EnumUtils.GetEnum(enumVal.enumID);
            PopulateEnumEntries();
            comboBox2.SelectedIndex = 0;
        }

        private void PopulateEnumEntries()
        {
            comboBox2.BeginUpdate();
            comboBox2.Items.Clear();
            foreach (EnumUtils.EnumDescriptor.Entry entry in enumDesc.Entries)
            {
                comboBox2.Items.Add(entry.Name);
            }
            comboBox2.EndUpdate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Name == comboBox2.SelectedItem.ToString());
            enumVal.enumIndex = enumEntry.Index;
        }
    }
}
