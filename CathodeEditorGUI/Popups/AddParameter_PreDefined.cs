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
using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using OpenCAGE;

namespace CommandsEditor
{
    public partial class AddParameter_PreDefined : AddParameter
    {
        private ParameterCreator _creator;
        private List<ListViewItem> _items = new List<ListViewItem>();
        private ListViewColumnSorter _sorter = new ListViewColumnSorter();

        private EntityInspector _entDisplay;

        public AddParameter_PreDefined(EntityInspector entityDisplay)
        {
            _entDisplay = entityDisplay;
            _creator = new ParameterCreator(_entDisplay.Entity, _entDisplay.Composite);

            InitializeComponent();
            param_name.ListViewItemSorter = _sorter;

            List<ListViewItem> options = _entDisplay.Content.editor_utils.GenerateParameterListAsListViewItem(_entDisplay.Entity, _entDisplay.Composite);
            for (int i = 0; i < options.Count; i++)
            {
                if (_entDisplay.Entity.GetParameter(options[i].Text) != null)
                    continue;

                options[i].SubItems[1].Text = _creator.GetInfo(options[i].Text);
                options[i].ImageIndex = 0;
                _items.Add(options[i]);
            }

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
                item.Group = param_name.Groups[(int)item.Tag];
                param_name.Items.Add(item);
            }
            param_name.EndUpdate();

            typesCount.Text = "Showing " + param_name.Items.Count;
        }

        private void createParams_Click(object sender, EventArgs e)
        {
            if (param_name.CheckedItems.Count == 0)
                return;

            foreach (ListViewItem item in param_name.CheckedItems)
                _creator.Create(item.Text);

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
