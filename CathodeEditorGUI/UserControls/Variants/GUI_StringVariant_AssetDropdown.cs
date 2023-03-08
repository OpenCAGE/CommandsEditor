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
            stringVal = cString;
            label1.Text = paramID;

            List<string> itemsToAdd = new List<string>();
            switch (assets)
            {
                case AssetType.SOUND_BANK:
                    foreach (string entry in Editor.resource.sound_bankdata.Entries)
                    {
                        if (!itemsToAdd.Contains(entry))
                            itemsToAdd.Add(entry);
                    }
                    break;
                case AssetType.SOUND_DIALOGUE:
                    foreach (SoundDialogueLookups.Sound entry in Editor.resource.sound_dialoguelookups.Entries)
                    {
                        if (!itemsToAdd.Contains(entry.ToString()))
                            itemsToAdd.Add(entry.ToString());
                    }
                    break;
                case AssetType.SOUND_REVERB:
                    foreach (string entry in Editor.resource.sound_environmentdata.Entries)
                    {
                        if (!itemsToAdd.Contains(entry))
                            itemsToAdd.Add(entry);
                    }
                    break;
                case AssetType.SOUND_EVENT:
                    //TODO: perhaps show these by soundbank - need to load in soundbank name IDs
                    foreach (SoundEventData.Soundbank entry in Editor.resource.sound_eventdata.Entries)
                    {
                        foreach (SoundEventData.Soundbank.Event e in entry.events)
                        {
                            if (!itemsToAdd.Contains(e.name))
                                itemsToAdd.Add(e.name);
                        }
                    }
                    break;
                case AssetType.LOCALISED_STRING:
                    foreach (KeyValuePair<string, Strings> entry in Editor.strings)
                    {
                        if (arg != "" && arg != entry.Key) continue;
                        foreach (KeyValuePair<string, string> e in entry.Value.Entries)
                        {
                            if (!itemsToAdd.Contains(e.Key))
                                itemsToAdd.Add(e.Key);
                        }
                    }
                    break;
                case AssetType.MATERIAL:
                    foreach (Materials.Material entry in Editor.resource.materials.Entries)
                    {
                        if (!itemsToAdd.Contains(entry.Name))
                            itemsToAdd.Add(entry.Name);
                    }
                    break;
                case AssetType.TEXTURE:
                    foreach (Textures.TEX4 entry in Editor.resource.textures.Entries)
                    {
                        if (!itemsToAdd.Contains(entry.Name))
                            itemsToAdd.Add(entry.Name);
                    }
                    foreach (Textures.TEX4 entry in Editor.resource.textures_global.Entries)
                    {
                        if (!itemsToAdd.Contains(entry.Name))
                            itemsToAdd.Add(entry.Name);
                    }
                    break;
                case AssetType.ANIMATION:
                    //TODO: This is NOT the correct way to populate this field, as it'll give us ALL anim strings, not just animations.
                    //      We should populate it by parsing the contents of ANIMATIONS.PAK, loading skeletons relative to animations, and then populating animations relative to the selected skeleton.
                    foreach (KeyValuePair<uint, string> entry in Editor.animstrings.Entries)
                    {
                        //comboBox1.Items.Add(entry.Value);
                    }
                    break;
            }
            itemsToAdd.Sort();

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(itemsToAdd.ToArray());
            comboBox1.Text = cString.value;
            comboBox1.SelectedItem = cString.value;
            comboBox1.EndUpdate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            stringVal.value = comboBox1.Text;
        }

        public enum AssetType
        {
            NONE,

            TEXTURE,
            MATERIAL,

            SOUND_DIALOGUE,
            SOUND_BANK,
            SOUND_EVENT,
            SOUND_REVERB,

            ANIMATION,

            LOCALISED_STRING, //You can filter this with args to a specific string DB
        }
    }
}
