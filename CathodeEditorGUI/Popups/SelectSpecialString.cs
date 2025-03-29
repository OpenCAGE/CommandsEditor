using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using CommandsEditor.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using static CATHODE.Scripting.TriggerSequence;

namespace CommandsEditor
{
    public partial class SelectSpecialString : BaseWindow
    {
        public Action<string> OnSelected;

        private cEnumString _defaultVal;

        private static List<AssetList> _assetList = new List<AssetList>();
        private AssetList _content = null;
        private ListViewColumnSorter _sorter = new ListViewColumnSorter();

        public SelectSpecialString(string paramName, cEnumString enumString, bool allowTypeSelect) : base(WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION | WindowClosesOn.COMMANDS_RELOAD)
        {
            InitializeComponent();

            _defaultVal = enumString;
            this.Text = "Select for '" + paramName + "'";

            if (allowTypeSelect)
            {
                List<string> types = new List<string>();
                foreach (EnumStringType type in Enum.GetValues(typeof(EnumStringType)))
                    types.Add(type.ToString());
                types.Clear();
                enumStringTypeSelect.BeginUpdate();
                enumStringTypeSelect.Items.Add(types);
                enumStringTypeSelect.EndUpdate();
            }
            else
            {
                enumStringTypeSelect.Visible = false;
            }

            //TODO: we never clear up these lists for old levels, which could lead to a slow memory leak!
            _content = _assetList.FirstOrDefault(o => o.level == Content.commands.Filepath && o.type == (EnumStringType)enumString.enumID.ToUInt32());
            if (_content == null)
            {
                _content = new AssetList() { level = Content.commands.Filepath, type = (EnumStringType)enumString.enumID.ToUInt32() };
                List<ListViewItem> strings = new List<ListViewItem>();
                switch (_content.type)
                {
                    case EnumStringType.ACHIEVEMENT_ID:
                        foreach (string str in ParseXML("AWARDS/MAIN_AWARD_LIST.BML", "awards/award", "game_id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.ACHIEVEMENT_STAT_ID:
                        foreach (string str in ParseXML("AWARDS/MAIN_AWARD_LIST.BML", "awards/stat", "stat_id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.ANIM_SET:
                        break;
                    case EnumStringType.ANIM_TREE_SET:
                        break;
                    case EnumStringType.ATTRIBUTE_SET:
                        break;
                    case EnumStringType.BLUEPRINT_TYPE:
                        foreach (string str in ParseXML("GBL_ITEM.BML", "item_database/recipes/recipe", "name"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.CHR_SKELETON_SET:
                        foreach (string str in Singleton.AllSkeletons)
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.GAME_VARIABLE:
                        foreach (string str in ParseXML("SCRIPT_READABLE_VARIABLES.XML", "game_variables/variable", "id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.GAMEPLAY_TIP_STRING_ID:
                        foreach (string str in ParseXML("GBL_ITEM.BML", "item_database/custom_gameplay_tips/custom_gameplay_tip", "string_id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.IDTAG_ID:
                        {
                            List<string> uids = ParseXML("GBL_ITEM.BML", "item_database/journal_idtag_entries/idtag_entry", "uid");
                            List<string> names = ParseXML("GBL_ITEM.BML", "item_database/journal_idtag_entries/idtag_entry", "name");
                            for (int i = 0; i < uids.Count; i++)
                            {
                                ListViewItem item = new ListViewItem() { Text = uids[i] };
                                item.SubItems.Add(names[i]);
                                strings.Add(item);
                            }
                            _content.use_desc_column = true;
                        }
                        break;
                    case EnumStringType.MAP_KEYFRAME_ID:
                        foreach (string str in ParseXML("GBL_ITEM.BML", "item_database/map_available_keyframes/map_keyframe", "name"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.NOSTROMO_LOG_ID:
                        {
                            List<string> uids = ParseXML("GBL_ITEM.BML", "item_database/journal_nostromo_entries/log_entry", "uid");
                            List<string> headers = ParseXML("GBL_ITEM.BML", "item_database/journal_nostromo_entries/log_entry", "heading_text");
                            for (int i = 0; i < uids.Count; i++)
                            {
                                ListViewItem item = new ListViewItem() { Text = uids[i] };
                                item.SubItems.Add(Singleton.TryLocalise(headers[i]));
                                strings.Add(item);
                            }
                            _content.use_desc_column = true;
                        }
                        break;
                    case EnumStringType.OBJECTIVE_ENTRY_ID:
                        foreach (string str in ParseXML("OBJECTIVE_ENTRIES.XML", "localisation_entries/localisation_entry", "id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.PRESENCE_ID:
                        foreach (string str in ParseXML("AWARDS/MAIN_AWARD_LIST.BML", "awards/presence", "game_id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.SEVASTOPOL_LOG_ID:
                        {
                            List<string> uids = ParseXML("GBL_ITEM.BML", "item_database/journal_sevastopol_entries/log_entry", "uid");
                            List<string> headers = ParseXML("GBL_ITEM.BML", "item_database/journal_sevastopol_entries/log_entry", "heading_text");
                            for (int i = 0; i < uids.Count; i++)
                            {
                                ListViewItem item = new ListViewItem() { Text = uids[i] };
                                item.SubItems.Add(Singleton.TryLocalise(headers[i]));
                                strings.Add(item);
                            }
                            _content.use_desc_column = true;
                        }
                        break;
                    case EnumStringType.SOUND_ARGUMENT:
                        break;
                    case EnumStringType.SOUND_EVENT:
                        foreach (SoundEventData.Soundbank entry in Content.resource.sound_eventdata.Entries)
                        {
                            foreach (SoundEventData.Soundbank.Event e in entry.events)
                            {
                                if (strings.FirstOrDefault(o => o.Text == e.name) == null)
                                {
                                    strings.Add(new ListViewItem() { Text = e.name });
                                    strings[strings.Count - 1].SubItems.Add(Singleton.TryLocalise(e.name));
                                }
                            }
                        }
                        _content.use_desc_column = true;
                        break;
                    case EnumStringType.SOUND_FOOTWEAR_GROUP:
                        foreach (string str in ParseXML("FOLEY_MATERIALS.XML", "foley_materials/feet", "id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.SOUND_LEG_GROUP:
                        foreach (string str in ParseXML("FOLEY_MATERIALS.XML", "foley_materials/leg", "id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.SOUND_PARAMETER:
                        break;
                    case EnumStringType.SOUND_REVERB:
                        foreach (string entry in Content.resource.sound_environmentdata.Entries)
                            if (strings.FirstOrDefault(o => o.Text == entry) == null)
                                strings.Add(new ListViewItem() { Text = entry });
                        break;
                    case EnumStringType.SOUND_RTPC:
                        break;
                    case EnumStringType.SOUND_STATE:
                        break;
                    case EnumStringType.SOUND_SWITCH:
                        break;
                    case EnumStringType.SOUND_TORSO_GROUP:
                        foreach (string str in ParseXML("FOLEY_MATERIALS.XML", "foley_materials/torso", "id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;
                    case EnumStringType.TUTORIAL_ENTRY_ID:
                        foreach (string str in ParseXML("TUTORIAL_ENTRIES.XML", "localisation_entries/localisation_entry", "id"))
                            strings.Add(new ListViewItem() { Text = str });
                        break;


                    //case EnumStringType.SOUND_BANK:
                    //    foreach (string entry in Content.resource.sound_bankdata.Entries)
                    //        if (strings.FirstOrDefault(o => o.Text == entry) == null)
                    //            strings.Add(new ListViewItem() { Text = entry });
                    //    break;
                    //case EnumStringType.SOUND_DIALOGUE:
                    //    foreach (SoundDialogueLookups.Sound entry in Content.resource.sound_dialoguelookups.Entries)
                    //    {
                    //        if (strings.FirstOrDefault(o => o.Text == entry.ToString()) == null)
                    //        {
                    //            strings.Add(new ListViewItem() { Text = entry.ToString() });
                    //            strings[strings.Count - 1].SubItems.Add(Singleton.TryLocalise(entry.ToString()));
                    //        }
                    //    }
                    //    _content.use_desc_column = true;
                    //    break;
                    //case AssetList.Type.LOCALISED_STRING:
                    //    string[] argsSplit = args.Split('/');
                    //    foreach (string arg in argsSplit)
                    //    {
                    //        foreach (KeyValuePair<string, Strings> entry in Singleton.Strings)
                    //        {
                    //            if (arg != "" && arg != entry.Key) continue;
                    //            foreach (KeyValuePair<string, string> e in entry.Value.Entries)
                    //            {
                    //                if (strings.FirstOrDefault(o => o.Text == e.Key) == null)
                    //                {
                    //                    strings.Add(new ListViewItem() { Text = e.Key });
                    //                    strings[strings.Count - 1].SubItems.Add(e.Value);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    _content.use_desc_column = true;
                    //    break;
                    //case AssetList.Type.MATERIAL:
                    //    foreach (Materials.Material entry in Content.resource.materials.Entries)
                    //    {
                    //        if (strings.FirstOrDefault(o => o.Text == entry.Name) == null)
                    //            strings.Add(new ListViewItem() { Text = entry.Name });
                    //    }
                    //    break;
                    //case AssetList.Type.TEXTURE:
                    //    foreach (Textures.TEX4 entry in Content.resource.textures.Entries)
                    //    {
                    //        if (strings.FirstOrDefault(o => o.Text == entry.Name) == null)
                    //            strings.Add(new ListViewItem() { Text = entry.Name });
                    //    }
                    //    foreach (Textures.TEX4 entry in Content.resource.textures_global.Entries)
                    //    {
                    //        if (strings.FirstOrDefault(o => o.Text == entry.Name) == null)
                    //            strings.Add(new ListViewItem() { Text = entry.Name });
                    //    }
                    //    break;
                    //case AssetList.Type.ANIMATION:
                    //    //TODO: This is NOT the correct way to populate this field, as it'll give us ALL anim strings, not just animations.
                    //    //      We should populate it by parsing the contents of ANIMATIONS.PAK, loading skeletons relative to animations, and then populating animations relative to the selected skeleton.
                    //    foreach (KeyValuePair<uint, string> entry in Singleton.AnimationStrings.Entries)
                    //    {
                    //        if (strings.FirstOrDefault(o => o.Text == entry.Value) == null)
                    //            strings.Add(new ListViewItem() { Text = entry.Value });
                    //    }
                    //    break;
                }
                strings.OrderBy(o => o.Text);
                _content.items = strings.ToArray();
                _assetList.Add(_content);
            }

            Search();
            clearSearchBtn.Visible = false;
            ShowMetadata.Visible = _content.type == EnumStringType.SOUND_EVENT;
            strings.ListViewItemSorter = _sorter;
        }

        private static List<string> ParseXML(string file, string path, string attribute)
        {
            XDocument xml = System.IO.Path.GetExtension(file) == ".BML" ? XDocument.Load(new XmlNodeReader(new BML(SharedData.pathToAI + "/DATA/" + file).Content)) : XDocument.Load(File.ReadAllText(file));
            foreach (var elem in xml.Descendants())
                elem.Name = elem.Name.LocalName;
            List<XElement> entries = xml.XPathSelectElements(path).ToList();
            List<string> strings = new List<string>();
            foreach (XElement entry in entries)
                strings.Add(entry.Attribute(attribute).Value);
            return strings;
        }

        private void SelectSpecialString_Load(object sender, EventArgs e)
        {
            int selectedIndex = -1;
            for (int i = 0; i < strings.Items.Count; i++)
            {
                strings.Items[i].Selected = false;
                if (strings.Items[i].Text == _defaultVal.value)
                {
                    selectedIndex = i;
                    strings.Items[i].Selected = true;
                }
            }
            strings.Invalidate();
            if (selectedIndex != -1)
                strings.EnsureVisible(selectedIndex);
        }

        private void ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            // Determine if the clicked column is already the column that is being sorted.
            if (e.Column == _sorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_sorter.Order == SortOrder.Ascending)
                {
                    _sorter.Order = SortOrder.Descending;
                }
                else
                {
                    _sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _sorter.SortColumn = e.Column;
                _sorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.strings.Sort();
        }

        private void search_box_TextChanged(object sender, EventArgs e)
        {
            Search();
            clearSearchBtn.Visible = search_box.Text != "";
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

            public EnumStringType type;

            public ListViewItem[] items = null;
            public bool use_desc_column = false;
        }

        private void ShowMetadata_Click(object sender, EventArgs e)
        {
            if (strings.SelectedItems.Count == 0)
                return;

            string selectedString = strings.SelectedItems[0].Text;

            string msg = "This event is contained within the following soundbanks:\n";
            foreach (SoundEventData.Soundbank entry in Content.resource.sound_eventdata.Entries)
            {
                if (entry.events.FirstOrDefault(o => o.name == selectedString) == null)
                    continue;

                string soundbankName = entry.id.ToString();
                for (int i = 0; i < Content.resource.sound_bankdata.Entries.Count; i++)
                {
                    if (Utilities.SoundHashedString(Content.resource.sound_bankdata.Entries[i]) != entry.id)
                        continue;

                    soundbankName = Content.resource.sound_bankdata.Entries[i];
                    break;
                }

                msg += " - " + soundbankName + "\n";
            }
            MessageBox.Show(msg);
        }
    }
}
