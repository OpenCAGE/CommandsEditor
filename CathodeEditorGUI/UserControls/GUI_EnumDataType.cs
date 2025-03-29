using System;
using System.Collections.Generic;
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

        public GUI_EnumDataType()
        {
            InitializeComponent();

            this.ContextMenuStrip = contextMenuStrip1;
            this.deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
        }

        public void PopulateUI(cEnum cEnum, string paramID, bool allowTypeSelection)
        {
            groupBox1.Text = paramID;

            enumVal = cEnum;
            this.deleteToolStripMenuItem.Text = "Delete '" + paramID + "'";

            if (allowTypeSelection)
            {
                comboBox1.BeginUpdate();
                comboBox1.Items.Clear();
                List<string> orderedEnums = new List<string>();
                foreach (EnumType enumType in Enum.GetValues(typeof(EnumType)))
                {
                    orderedEnums.Add(enumType.ToString());
                }
                orderedEnums.Sort();
                foreach (string enumType in orderedEnums)
                {
                    comboBox1.Items.Add(enumType);
                }
                comboBox1.EndUpdate();
                comboBox1.SelectedIndex = 0;

                this.Height = 80;
                groupBox1.Height = 75;
                groupBox1.Location = new Point(0, 0);
                comboBox1.Visible = true;
                comboBox1.Location = new Point(6, 19);
                comboBox2.Location = new Point(6, 48);
                
                if (enumVal.enumID == ShortGuid.Invalid)
                {
                    //if this entity has no default enum applied, apply one
                    EnumUtils.EnumDescriptor enumDesc = EnumUtils.GetEnum(ShortGuidUtils.Generate(comboBox1.Text));
                    EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == -1);
                    comboBox2.SelectedItem = enumEntry.Name;
                }
                else
                {
                    comboBox1.SelectedItem = enumVal.enumID.ToString();
                    EnumUtils.EnumDescriptor enumDesc = EnumUtils.GetEnum(enumVal.enumID);
                    EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == enumVal.enumIndex);
                    comboBox2.SelectedItem = enumEntry.Name;
                }
            }
            else
            {
                EnumUtils.EnumDescriptor enumDesc = EnumUtils.GetEnum(cEnum.enumID);
                comboBox1.Items.Add(enumDesc.Name);
                comboBox1.Text = enumDesc.Name;

                EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == cEnum.enumIndex);
                if (enumEntry == null)
                    MessageBox.Show("Failed to match index " + cEnum.enumIndex + " for " + enumDesc.Name + "!\nThis behaviour is unexpected.\nPlease report this on GitHub!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    comboBox2.SelectedItem = enumEntry.Name;
            }

            _hasDoneSetup = true;
        }

        private void UpdateEnumOptions(EnumUtils.EnumDescriptor enumDesc)
        {
            comboBox2.BeginUpdate();
            comboBox2.Items.Clear();
            foreach (EnumUtils.EnumDescriptor.Entry entry in enumDesc.Entries)
                comboBox2.Items.Add(entry.Name);
            comboBox2.EndUpdate();
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumUtils.EnumDescriptor enumDesc = EnumUtils.GetEnum(comboBox1.Text);
            UpdateEnumOptions(enumDesc);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnumUtils.EnumDescriptor enumDesc = EnumUtils.GetEnum(comboBox1.Text);
            UpdateEnum(enumDesc);
        }

        private void UpdateEnum(EnumUtils.EnumDescriptor enumDesc)
        {
            if (!_hasDoneSetup)
                return;

            EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Name == comboBox2.SelectedItem.ToString());
            enumVal.enumID = enumDesc.ID;
            enumVal.enumIndex = enumEntry.Index;
            HighlightAsModified();
        }

        public override void HighlightAsModified(bool updateDatabase = true, Control fontToUpdate = null)
        {
            base.HighlightAsModified(updateDatabase, groupBox1);
        }
    }
}
