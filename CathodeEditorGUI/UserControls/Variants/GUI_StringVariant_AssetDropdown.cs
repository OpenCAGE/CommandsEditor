using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CATHODE;
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

        public void PopulateUI(cString cString, string paramID, AssetType assets, string arg = "")
        {
            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            switch (assets)
            {
                case AssetType.SOUND_BANKS:
                    foreach (string entry in Editor.resource.sound_bankdata.Entries)
                    {
                        comboBox1.Items.Add(entry);
                    }
                    break;
                case AssetType.SOUND_DIALOGUE:
                    foreach (SoundDialogueLookups.Sound entry in Editor.resource.sound_dialoguelookups.Entries)
                    {
                        comboBox1.Items.Add(entry.ToString());
                    }
                    break;
                case AssetType.SOUND_EVENTS:
                    //TODO: perhaps show these by soundbank - need to load in soundbank name IDs
                    foreach (SoundEventData.Soundbank entry in Editor.resource.sound_eventdata.Entries)
                    {
                        foreach (SoundEventData.Soundbank.Event e in entry.events)
                        {
                            comboBox1.Items.Add(e.name);
                        }
                    }
                    break;
                case AssetType.STRINGS:
                    foreach (KeyValuePair<string, Strings> entry in Editor.strings)
                    {
                        if (arg != "" && arg != entry.Key) continue;
                        foreach (KeyValuePair<string, string> e in entry.Value.Entries)
                        {
                            comboBox1.Items.Add(e.Key);
                        }
                    }
                    break;
                case AssetType.MATERIALS:
                    foreach (Materials.Material entry in Editor.resource.materials.Entries)
                    {
                        comboBox1.Items.Add(entry.Name);
                    }
                    break;
                case AssetType.TEXTURES:
                    foreach (Textures.TEX4 entry in Editor.resource.textures.Entries)
                    {
                        comboBox1.Items.Add(entry.Name);
                    }
                    foreach (Textures.TEX4 entry in Editor.resource.textures_global.Entries)
                    {
                        comboBox1.Items.Add(entry.Name);
                    }
                    break;
                case AssetType.ANIMATIONS:
                    //TODO: This is NOT the correct way to populate this field, as it'll give us ALL anim strings, not just animations.
                    //      We should populate it by parsing the contents of ANIMATIONS.PAK, loading skeletons relative to animations, and then populating animations relative to the selected skeleton.
                    foreach (KeyValuePair<uint, string> entry in Editor.animstrings.Entries)
                    {
                        //comboBox1.Items.Add(entry.Value);
                    }
                    break;
            }
            comboBox1.Sorted = true;
            comboBox1.EndUpdate();


            stringVal = cString;
            label1.Text = paramID;
            comboBox1.Text = cString.value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            stringVal.value = comboBox1.Text;
        }

        public enum AssetType
        {
            TEXTURES,
            MATERIALS,

            SOUND_DIALOGUE,
            SOUND_BANKS,
            SOUND_EVENTS,

            ANIMATIONS,

            STRINGS, //You can filter this with args to a specific string DB
        }
    }
}
