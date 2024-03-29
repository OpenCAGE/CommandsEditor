using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private EntityDisplay _entDisplay;

        public AddParameter_PreDefined(EntityDisplay entityDisplay)
        {
            _entDisplay = entityDisplay;
            _creator = new ParameterCreator(_entDisplay.Entity, _entDisplay.Composite);

            InitializeComponent();

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
    }
}
