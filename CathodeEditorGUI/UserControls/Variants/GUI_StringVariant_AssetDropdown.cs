using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Scripting;

namespace CommandsEditor.UserControls
{
    public class AssetList
    {
        public string level = "";

        public Type assets = Type.NONE;
        public string args = "";

        public string[] strings = null;

        public enum Type
        {
            NONE,

            TEXTURE,
            MATERIAL,

            SOUND_DIALOGUE,
            SOUND_BANK,
            SOUND_EVENT,
            SOUND_REVERB,

            ANIMATION,

            LOCALISED_STRING,
        }
    }

    public partial class GUI_StringVariant_AssetDropdown : UserControl
    {
        cString stringVal = null;

        static List<AssetList> assetlist_cache = new List<AssetList>();

        public GUI_StringVariant_AssetDropdown()
        {
            InitializeComponent();
        }

        public void PopulateUI(cString cString, string paramID, AssetList.Type assets, string args = "")
        {
            stringVal = cString;
            label1.Text = paramID;

            //TODO: we never clear up these lists for old levels, which could lead to a slow memory leak!

            AssetList content = assetlist_cache.FirstOrDefault(o => o.level == Editor.commands.Filepath && o.assets == assets && o.args == args);
            if (content == null)
            {
                content = new AssetList() { level = Editor.commands.Filepath, args = args, assets = assets };
                List<string> strings = new List<string>();
                switch (assets)
                {
                    case AssetList.Type.SOUND_BANK:
                        foreach (string entry in Editor.resource.sound_bankdata.Entries)
                        {
                            if (!strings.Contains(entry))
                                strings.Add(entry);
                        }
                        break;
                    case AssetList.Type.SOUND_DIALOGUE:
                        foreach (SoundDialogueLookups.Sound entry in Editor.resource.sound_dialoguelookups.Entries)
                        {
                            if (!strings.Contains(entry.ToString()))
                                strings.Add(entry.ToString());
                        }
                        break;
                    case AssetList.Type.SOUND_REVERB:
                        foreach (string entry in Editor.resource.sound_environmentdata.Entries)
                        {
                            if (!strings.Contains(entry))
                                strings.Add(entry);
                        }
                        break;
                    case AssetList.Type.SOUND_EVENT:
                        //TODO: perhaps show these by soundbank - need to load in soundbank name IDs
                        foreach (SoundEventData.Soundbank entry in Editor.resource.sound_eventdata.Entries)
                        {
                            foreach (SoundEventData.Soundbank.Event e in entry.events)
                            {
                                if (!strings.Contains(e.name))
                                    strings.Add(e.name);
                            }
                        }
                        break;
                    case AssetList.Type.LOCALISED_STRING:
                        foreach (KeyValuePair<string, Strings> entry in Editor.strings)
                        {
                            if (args != "" && args != entry.Key) continue;
                            foreach (KeyValuePair<string, string> e in entry.Value.Entries)
                            {
                                if (!strings.Contains(e.Key))
                                    strings.Add(e.Key);
                            }
                        }
                        break;
                    case AssetList.Type.MATERIAL:
                        foreach (Materials.Material entry in Editor.resource.materials.Entries)
                        {
                            if (!strings.Contains(entry.Name))
                                strings.Add(entry.Name);
                        }
                        break;
                    case AssetList.Type.TEXTURE:
                        foreach (Textures.TEX4 entry in Editor.resource.textures.Entries)
                        {
                            if (!strings.Contains(entry.Name))
                                strings.Add(entry.Name);
                        }
                        foreach (Textures.TEX4 entry in Editor.resource.textures_global.Entries)
                        {
                            if (!strings.Contains(entry.Name))
                                strings.Add(entry.Name);
                        }
                        break;
                    case AssetList.Type.ANIMATION:
                        //TODO: This is NOT the correct way to populate this field, as it'll give us ALL anim strings, not just animations.
                        //      We should populate it by parsing the contents of ANIMATIONS.PAK, loading skeletons relative to animations, and then populating animations relative to the selected skeleton.
                        foreach (KeyValuePair<uint, string> entry in Editor.animstrings.Entries)
                        {
                            //if (!strings.Contains(entry.Value))
                            //    strings.Add(entry.Value);
                        }
                        break;
                }
                strings.Sort();
                content.strings = strings.ToArray();
                assetlist_cache.Add(content);
            }

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(content.strings);
            comboBox1.Text = cString.value;
            comboBox1.SelectedItem = cString.value;
            comboBox1.EndUpdate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            stringVal.value = comboBox1.Text;
        }

    }
}
