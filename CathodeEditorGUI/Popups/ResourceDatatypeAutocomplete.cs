using CATHODE.Scripting;
using CATHODE;
using CommandsEditor.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor.Popups
{
    public partial class ResourceDatatypeAutocomplete : Form
    {
        public cString stringVal = null;

        AssetList.Type type = AssetList.Type.NONE;
        string typeArgs = "";

        static List<AssetList> assetlist_cache = new List<AssetList>(); //TODO: cache controls, not just the contents of the controls
        AssetList content = null;

        public ResourceDatatypeAutocomplete()
        {
            InitializeComponent();
        }

        public void PopulateUI(LevelContent Content, cString cString, string paramID, AssetList.Type assets, string args = "")
        {
            stringVal = cString;
            Text = paramID;
            type = assets;
            typeArgs = args;

            //TODO: we never clear up these lists for old levels, which could lead to a slow memory leak!

            content = assetlist_cache.FirstOrDefault(o => o.level == Content.commands.Filepath && o.assets == assets && o.args == args);
            if (content == null)
            {
                content = new AssetList() { level = Content.commands.Filepath, args = args, assets = assets };
                List<AssetList.Value> strings = new List<AssetList.Value>();
                switch (assets)
                {
                    case AssetList.Type.SOUND_BANK:
                        foreach (string entry in Content.resource.sound_bankdata.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.value == entry) == null)
                                strings.Add(new AssetList.Value() { value = entry });
                        }
                        break;
                    case AssetList.Type.SOUND_DIALOGUE:
                        foreach (SoundDialogueLookups.Sound entry in Content.resource.sound_dialoguelookups.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.value == entry.ToString()) == null)
                            {
                                string englishTranslation = "";
                                foreach (KeyValuePair<string, Strings> stringdb in Singleton.Strings)
                                {
                                    foreach (KeyValuePair<string, string> stringdb_val in stringdb.Value.Entries)
                                    {
                                        if (stringdb_val.Key != entry.ToString()) continue;
                                        englishTranslation = stringdb_val.Value;
                                        break;
                                    }
                                    if (englishTranslation != "") break;
                                }
                                strings.Add(new AssetList.Value() { value = entry.ToString(), tooltip = englishTranslation });
                            }
                        }
                        break;
                    case AssetList.Type.SOUND_REVERB:
                        foreach (string entry in Content.resource.sound_environmentdata.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.value == entry) == null)
                                strings.Add(new AssetList.Value() { value = entry });
                        }
                        break;
                    case AssetList.Type.SOUND_EVENT:
                        //TODO: perhaps show these by soundbank - need to load in soundbank name IDs
                        foreach (SoundEventData.Soundbank entry in Content.resource.sound_eventdata.Entries)
                        {
                            foreach (SoundEventData.Soundbank.Event e in entry.events)
                            {
                                if (strings.FirstOrDefault(o => o.value == e.name) == null)
                                {
                                    string englishTranslation = "";
                                    foreach (KeyValuePair<string, Strings> stringdb in Singleton.Strings)
                                    {
                                        foreach (KeyValuePair<string, string> stringdb_val in stringdb.Value.Entries)
                                        {
                                            if (stringdb_val.Key != e.name) continue;
                                            englishTranslation = stringdb_val.Value;
                                            break;
                                        }
                                        if (englishTranslation != "") break;
                                    }
                                    strings.Add(new AssetList.Value() { value = e.name, tooltip = englishTranslation });
                                }
                            }
                        }
                        break;
                    case AssetList.Type.LOCALISED_STRING:
                        string[] argsSplit = args.Split('/');
                        foreach (string arg in argsSplit)
                        {
                            foreach (KeyValuePair<string, Strings> entry in Singleton.Strings)
                            {
                                if (arg != "" && arg != entry.Key) continue;
                                foreach (KeyValuePair<string, string> e in entry.Value.Entries)
                                {
                                    if (strings.FirstOrDefault(o => o.value == e.Key) == null)
                                    {
                                        strings.Add(new AssetList.Value() { value = e.Key, tooltip = e.Value });
                                    }
                                }
                            }
                        }
                        break;
                    case AssetList.Type.MATERIAL:
                        foreach (Materials.Material entry in Content.resource.materials.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.value == entry.Name) == null)
                                strings.Add(new AssetList.Value() { value = entry.Name });
                        }
                        break;
                    case AssetList.Type.TEXTURE:
                        foreach (Textures.TEX4 entry in Content.resource.textures.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.value == entry.Name) == null)
                                strings.Add(new AssetList.Value() { value = entry.Name });
                        }
                        foreach (Textures.TEX4 entry in Content.resource.textures_global.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.value == entry.Name) == null)
                                strings.Add(new AssetList.Value() { value = entry.Name });
                        }
                        break;
                    case AssetList.Type.ANIMATION:
                        //TODO: This is NOT the correct way to populate this field, as it'll give us ALL anim strings, not just animations.
                        //      We should populate it by parsing the contents of ANIMATIONS.PAK, loading skeletons relative to animations, and then populating animations relative to the selected skeleton.
                        foreach (KeyValuePair<uint, string> entry in Singleton.AnimationStrings.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.value == entry.Value) == null)
                                strings.Add(new AssetList.Value() { value = entry.Value });
                        }
                        break;
                }
                strings.OrderBy(o => o.value);
                content.strings = strings.ToArray();
                assetlist_cache.Add(content);
            }

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            for (int i = 0; i < content.strings.Count(); i++)
                comboBox1.Items.Add(content.strings[i].value);
            comboBox1.Text = cString.value;
            comboBox1.SelectedItem = cString.value;
            comboBox1.EndUpdate();

            comboBox1.AutoSelectOff();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            stringVal.value = comboBox1.Text;

            if (content != null && content.strings != null && comboBox1.SelectedIndex != -1 && content.strings.Length > comboBox1.SelectedIndex)
                toolTip1.SetToolTip(comboBox1, content.strings[comboBox1.SelectedIndex].tooltip);

            if (type == AssetList.Type.LOCALISED_STRING)
            {
                foreach (KeyValuePair<string, Strings> entry in Singleton.Strings)
                {
                    if (typeArgs != "" && typeArgs != entry.Key) continue;
                    if (entry.Value.Entries.ContainsKey(comboBox1.Text))
                    {
                        toolTip1.SetToolTip(comboBox1, entry.Value.Entries[comboBox1.Text]);
                        return;
                    }
                }
            }
        }
    }
}
