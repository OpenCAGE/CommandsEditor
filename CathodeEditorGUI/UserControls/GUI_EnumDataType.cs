using System;
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

        //Temp fix for legacy enums that I'm not 100% on the indexes for -> show the old manual number input
        private bool UseLegacyInput(string enumName, bool updateUI = false)
        {
            switch (enumName)
            {
                case "CHECKPOINT_TYPE":
                case "LIGHT_ANIM":
                case "AMMO_TYPE":
                case "AUTODETECT":
                case "BEHAVIOUR_TREE_FLAGS":
                case "CUSTOM_CHARACTER_ACCESSORY_OVERRIDE":
                case "DAMAGE_EFFECTS":
                case "DUCK_HEIGHT":
                case "ENEMY_TYPE":
                case "GAME_CLIP":
                case "IMPACT_CHARACTER_BODY_LOCATION_TYPE":
                case "LIGHT_FADE_TYPE":
                case "LIGHT_TRANSITION":
                case "NPC_GUN_AIM_MODE":
                case "ORIENTATION_AXIS":
                case "POPUP_MESSAGE_ICON":
                case "TASK_CHARACTER_CLASS_FILTER":
                case "TRANSITION_DIRECTION":
                case "WEAPON_HANDEDNESS":
                case "EQUIPMENT_SLOT":
                case "LEVER_TYPE":
                case "DOOR_MECHANISM":
                case "REWIRE_SYSTEM_NAME":
                case "REWIRE_SYSTEM_TYPE":
                case "CHARACTER_STANCE":
                case "CHARACTER_CLASS_COMBINATION":
                    if (updateUI)
                    {
                        textBox1.Visible = true;
                        comboBox2.Visible = false;
                    }
                    return true;
                default:
                    if (updateUI)
                    {
                        PopulateEnumEntries();
                        textBox1.Visible = false;
                        comboBox2.Visible = true;
                    }
                    return false;
            }
        }

        public void PopulateUI(cEnum cEnum, string paramID)
        {
            groupBox1.Text = paramID;

            enumVal = cEnum;
            enumDesc = EnumUtils.GetEnum(cEnum.enumID);
            this.deleteToolStripMenuItem.Text = "Delete '" + paramID + "'";

            comboBox1.Text = enumDesc.Name;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            if (UseLegacyInput(comboBox1.Text, true))
            {
                textBox1.Text = cEnum.enumIndex.ToString();
            }
            else
            {
                EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Index == cEnum.enumIndex);
                if (enumEntry == null) MessageBox.Show("Failed to match enum index for " + comboBox1.Text + "!\nThis behaviour is UNEXPECTED.\nPlease report this on GitHub!", "Error!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else comboBox2.SelectedItem = enumEntry.Name;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            enumVal.enumID = ShortGuidUtils.Generate(comboBox1.Text);
            enumDesc = EnumUtils.GetEnum(enumVal.enumID);

            if (UseLegacyInput(comboBox1.Text, true))
            {
                textBox1.Text = "0";
            }
            else
            {
                comboBox2.SelectedIndex = 0;
            }
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
            if (UseLegacyInput(comboBox1.Text)) return;
            EnumUtils.EnumDescriptor.Entry enumEntry = enumDesc.Entries.FirstOrDefault(o => o.Name == comboBox2.SelectedItem.ToString());
            enumVal.enumIndex = enumEntry.Index;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!UseLegacyInput(comboBox1.Text)) return;
            textBox1.Text = EditorUtils.ForceStringNumeric(textBox1.Text);
            enumVal.enumIndex = Convert.ToInt32(textBox1.Text);
        }
    }
}
