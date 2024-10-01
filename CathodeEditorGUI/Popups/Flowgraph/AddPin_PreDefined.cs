using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using OpenCAGE;
using ST.Library.UI.NodeEditor;
using static CommandsEditor.AddPin;
using static CommandsEditor.EditorUtils;

namespace CommandsEditor
{
    //NOTE: This is a reworked version of AddParameter_PreDefined - the logic of both should be combined/tidied.
    public partial class AddPin_PreDefined : AddPin
    {
        private ParameterCreator _creator = null;
        private List<ListViewItem> _items = new List<ListViewItem>();
        private ListViewColumnSorter _sorter = new ListViewColumnSorter();

        public AddPin_PreDefined(STNode node, Mode mode) : base(node, mode)
        {
            Composite comp = Singleton.Editor.CommandsDisplay.CompositeDisplay.Composite;
            Entity ent = comp.GetEntityByID(node.ShortGUID);
            _creator = new ParameterCreator(ent, comp);

            InitializeComponent();
            param_name.ListViewItemSorter = _sorter;

            switch (_mode)
            {
                case Mode.ADD_IN:
                case Mode.REMOVE_IN:
                    this.Text = "Modify Pins In [" + _node.Title + "]";
                    break;
                case Mode.ADD_OUT:
                case Mode.REMOVE_OUT:
                    this.Text = "Modify Pins Out [" + _node.Title + "]";
                    label2.Text = "Pins Out";
                    createParams.Text = "Set Selected As Pins Out";
                    break;
            }

            List<ListViewItem> options = Singleton.Editor.CommandsDisplay.Content.editor_utils.GenerateParameterListAsListViewItem(ent, comp);
            for (int i = 0; i < options.Count; i++)
            {
                switch (_mode)
                {
                    case Mode.ADD_IN:
                    case Mode.REMOVE_IN:
                        options[i].Checked = node.GetInputOptions().FirstOrDefault(o => o.ShortGUID == ((ParameterListViewItemTag)options[i].Tag).ShortGUID) != null;
                        break;
                    case Mode.ADD_OUT:
                    case Mode.REMOVE_OUT:
                        options[i].Checked = node.GetOutputOptions().FirstOrDefault(o => o.ShortGUID == ((ParameterListViewItemTag)options[i].Tag).ShortGUID) != null;
                        break;
                }

                options[i].SubItems[1].Text = _creator.GetInfo(options[i].Text);
                options[i].ImageIndex = 0;
                _items.Add(options[i]);
            }
            //TODO: need to support removing additional params like cageanim ones

            searchBtn_Click(null, null);
        }

        private void clearSearchBtn_Click(object sender, EventArgs e)
        {
            searchText.Text = "";
            searchBtn_Click(null, null);
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            param_name.BeginUpdate();
            param_name.Items.Clear();
            ListViewItem[] items = _items.Where(o => o.Text.ToUpper().Contains(searchText.Text.ToUpper())).ToList().ToArray();
            foreach (ListViewItem item in items)
            {
                ParameterListViewItemTag tag = (ParameterListViewItemTag)item.Tag;
                item.Group = param_name.Groups[(int)tag.Usage];
                param_name.Items.Add(item);
            }
            param_name.EndUpdate();

            typesCount.Text = "Showing " + param_name.Items.Count;
        }

        private void createParams_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in param_name.Items)
            {
                ParameterListViewItemTag tag = (ParameterListViewItemTag)item.Tag;
                switch (_mode)
                {
                    case Mode.ADD_IN:
                    case Mode.REMOVE_IN:
                        if (item.Checked)
                            _node.AddInputOption(tag.ShortGUID);
                        else
                            _node.RemoveInputOption(tag.ShortGUID);
                        break;
                    case Mode.ADD_OUT:
                    case Mode.REMOVE_OUT:
                        if (item.Checked)
                            _node.AddOutputOption(tag.ShortGUID);
                        else
                            _node.RemoveOutputOption(tag.ShortGUID);
                        break;
                }
            }

            OnSaved?.Invoke();
            this.Close();
        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in param_name.Items)
            {
                item.Selected = true;
                item.Checked = true;
            }
        }

        private void deSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in param_name.Items)
            {
                item.Selected = false;
                item.Checked = false;
            }
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
            this.param_name.Sort();
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            if (_creator.RootFunc != null)
            {
                string func = _creator.RootFunc.function.ToString();
                if (func == _creator.RootFunc.function.ToByteString())
                    Process.Start("https://opencage.co.uk/docs/cathode-entities/#CompositeInterface");
                else
                    Process.Start("https://opencage.co.uk/docs/cathode-entities/#" + _creator.RootFunc.function.ToString());
            }
            else
                Process.Start("https://opencage.co.uk/docs/cathode-entities/#entities");
        }
    }
}
