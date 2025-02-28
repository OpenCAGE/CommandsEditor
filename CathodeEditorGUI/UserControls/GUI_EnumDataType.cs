using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CATHODE.Scripting;
using CathodeLib;

namespace CommandsEditor.UserControls
{
    public partial class GUI_EnumDataType : ParameterUserControl
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

            this.ContextMenuStrip = contextMenuStrip1;
            this.deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
        }

        public void PopulateUI(cEnum cEnum, string paramID)
        {
            groupBox1.Text = paramID;

            enumVal = cEnum;
            enumDesc = EnumUtils.GetEnum(cEnum.enumID);
            this.deleteToolStripMenuItem.Text = "Delete '" + paramID + "'";

            comboBox1.Text = enumDesc.Name;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            PopulateEnumEntries();

            EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == cEnum.enumIndex);
            if (enumEntry == null) MessageBox.Show("Failed to match enum index for " + comboBox1.Text + "!\nThis behaviour is UNEXPECTED.\nPlease report this on GitHub!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else comboBox2.SelectedItem = enumEntry.Name;

            _hasDoneSetup = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            enumVal.enumID = ShortGuidUtils.Generate(comboBox1.Text);
            enumDesc = EnumUtils.GetEnum(enumVal.enumID);

            PopulateEnumEntries();
            comboBox2.SelectedIndex = 0;
            HighlightAsModified();
        }

        private void PopulateEnumEntries()
        {
            comboBox2.BeginUpdate();
            comboBox2.Items.Clear();
            foreach (EnumUtils.EnumDescriptor.Entry entry in enumDesc.Entries) comboBox2.Items.Add(entry.Name);
            comboBox2.EndUpdate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Name == comboBox2.SelectedItem.ToString());
            enumVal.enumIndex = enumEntry.Index;
            HighlightAsModified();
        }

        public override void HighlightAsModified(bool updateDatabase = true, Control fontToUpdate = null)
        {
            base.HighlightAsModified(updateDatabase, groupBox1);
        }
    }
}
