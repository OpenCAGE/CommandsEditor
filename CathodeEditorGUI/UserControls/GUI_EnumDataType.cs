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
        bool _couldFindEnum = false;

        public GUI_EnumDataType()
        {
            InitializeComponent();

            this.ContextMenuStrip = contextMenuStrip1;
            this.deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
        }

        //NOTE:LightReference "type" is not being added as enum - but only via parameter window
        //NOTE: SetEnum adding "delete_me" comes up as enum

        public void PopulateUI(cEnum cEnum, string paramID)
        {
            groupBox1.Text = paramID;

            enumVal = cEnum;
            EnumUtils.EnumDescriptor enumDesc = EnumUtils.GetEnum(cEnum.enumID);
            this.deleteToolStripMenuItem.Text = "Delete '" + paramID + "'";

            _couldFindEnum = enumDesc != null;
            //todo: if enumDesc is null, should allow selection of type

            if (!_couldFindEnum)
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
                
                //todo: this doesn't work after the first go as the value is now applied. need to check parameterutils.

                if (enumVal.enumID == ShortGuid.Invalid)
                {
                    //if this entity has no default enum applied, apply one
                    enumDesc = EnumUtils.GetEnum(ShortGuidUtils.Generate(comboBox1.Text));
                    EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == -1);
                    comboBox2.SelectedItem = enumEntry.Name;
                }
                else
                {
                    comboBox1.SelectedItem = enumVal.enumID.ToString();
                    enumDesc = EnumUtils.GetEnum(enumVal.enumID);
                    EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == enumVal.enumIndex);
                    comboBox2.SelectedItem = enumEntry.Name;
                }
            }
            else
            {
                comboBox1.Items.Add(enumDesc.Name);
                comboBox1.Text = enumDesc.Name;

                EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == cEnum.enumIndex);
                if (enumEntry == null)
                    MessageBox.Show("Failed to match index for " + enumDesc.Name + "!\nThis behaviour is UNEXPECTED.\nPlease report this on GitHub!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Name == comboBox2.SelectedItem.ToString());
            if (!_couldFindEnum)
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
