using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
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

namespace CommandsEditor
{
    public partial class SelectSpecialString : BaseWindow
    {
        public Action<string> OnSelected;

        private string _defaultVal;
        private AssetList.Type _type = AssetList.Type.NONE;
        private string _typeArgs = "";

        private static List<AssetList> _assetList = new List<AssetList>();
        private AssetList _content = null;

        public SelectSpecialString(string paramName, string defaultVal, AssetList.Type assets, string args = "") : base(WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION | WindowClosesOn.COMMANDS_RELOAD)
        {
            InitializeComponent();

            _type = assets;
            _typeArgs = args;
            _defaultVal = defaultVal;

            this.Text = "Select for '" + paramName + "'";

            //TODO: we never clear up these lists for old levels, which could lead to a slow memory leak!

            _content = _assetList.FirstOrDefault(o => o.level == Content.commands.Filepath && o.assets == assets && o.args == args);
            if (_content == null)
            {
                _content = new AssetList() { level = Content.commands.Filepath, args = args, assets = assets };
                List<ListViewItem> strings = new List<ListViewItem>();
                switch (assets)
                {
                    case AssetList.Type.SOUND_BANK:
                        foreach (string entry in Content.resource.sound_bankdata.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.Text == entry) == null)
                                strings.Add(new ListViewItem() { Text = entry });
                        }
                        break;
                    case AssetList.Type.SOUND_DIALOGUE:
                        foreach (SoundDialogueLookups.Sound entry in Content.resource.sound_dialoguelookups.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.Text == entry.ToString()) == null)
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
                                strings.Add(new ListViewItem() { Text = entry.ToString() });
                                strings[strings.Count - 1].SubItems.Add(englishTranslation);
                            }
                        }
                        _content.use_desc_column = true;
                        break;
                    case AssetList.Type.SOUND_REVERB:
                        foreach (string entry in Content.resource.sound_environmentdata.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.Text == entry) == null)
                                strings.Add(new ListViewItem() { Text = entry });
                        }
                        break;
                    case AssetList.Type.SOUND_EVENT:
                        //TODO: perhaps show these by soundbank - need to load in soundbank name IDs
                        foreach (SoundEventData.Soundbank entry in Content.resource.sound_eventdata.Entries)
                        {
                            foreach (SoundEventData.Soundbank.Event e in entry.events)
                            {
                                if (strings.FirstOrDefault(o => o.Text == e.name) == null)
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
                                    strings.Add(new ListViewItem() { Text = e.name });
                                    strings[strings.Count - 1].SubItems.Add(englishTranslation);
                                }
                            }
                        }
                        _content.use_desc_column = true;
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
                                    if (strings.FirstOrDefault(o => o.Text == e.Key) == null)
                                    {
                                        strings.Add(new ListViewItem() { Text = e.Key });
                                        strings[strings.Count - 1].SubItems.Add(e.Value);
                                    }
                                }
                            }
                        }
                        _content.use_desc_column = true;
                        break;
                    case AssetList.Type.MATERIAL:
                        foreach (Materials.Material entry in Content.resource.materials.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.Text == entry.Name) == null)
                                strings.Add(new ListViewItem() { Text = entry.Name });
                        }
                        break;
                    case AssetList.Type.TEXTURE:
                        foreach (Textures.TEX4 entry in Content.resource.textures.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.Text == entry.Name) == null)
                                strings.Add(new ListViewItem() { Text = entry.Name });
                        }
                        foreach (Textures.TEX4 entry in Content.resource.textures_global.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.Text == entry.Name) == null)
                                strings.Add(new ListViewItem() { Text = entry.Name });
                        }
                        break;
                    case AssetList.Type.ANIMATION:
                        //TODO: This is NOT the correct way to populate this field, as it'll give us ALL anim strings, not just animations.
                        //      We should populate it by parsing the contents of ANIMATIONS.PAK, loading skeletons relative to animations, and then populating animations relative to the selected skeleton.
                        foreach (KeyValuePair<uint, string> entry in Singleton.AnimationStrings.Entries)
                        {
                            if (strings.FirstOrDefault(o => o.Text == entry.Value) == null)
                                strings.Add(new ListViewItem() { Text = entry.Value });
                        }
                        break;
                }
                strings.OrderBy(o => o.Text);
                _content.items = strings.ToArray();
                _assetList.Add(_content);
            }

            Search();

            for (int i = 0; i < strings.Items.Count; i++)
                strings.Items[i].Selected = strings.Items[i].Text == _defaultVal;
        }

        private void search_box_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void clearSearchBtn_Click(object sender, EventArgs e)
        {
            search_box.Text = "";
            //Search();
        }

        private void Search()
        {
            strings.BeginUpdate();
            strings.SuspendLayout();
            strings.Items.Clear();
            strings.Items.AddRange(_content.items.Where(o => o.Text.ToUpper().Contains(search_box.Text.ToUpper()) || o.SubItems[o.SubItems.Count - 1].Text.ToUpper().Contains(search_box.Text.ToUpper())).ToList().ToArray());
            strings.EndUpdate();
        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            if (strings.SelectedItems.Count == 0)
                return;

            OnSelected?.Invoke(strings.SelectedItems[0].Text);
            this.Close();
        }

        public class AssetList
        {
            ~AssetList()
            {
                items = null;
            }

            public string level = "";

            public Type assets = Type.NONE;
            public string args = "";

            public ListViewItem[] items = null;
            public bool use_desc_column = false;

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
    }
}
