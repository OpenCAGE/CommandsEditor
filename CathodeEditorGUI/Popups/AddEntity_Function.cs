using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class AddEntity_Function : BaseWindow
    {
        private List<ListViewItem> _items = new List<ListViewItem>();
        private Composite _composite;

        private ListViewColumnSorter _sorter = new ListViewColumnSorter();

        public AddEntity_Function(Composite composite, bool flowgraphMode) : base (WindowClosesOn.NEW_COMPOSITE_SELECTION | WindowClosesOn.COMMANDS_RELOAD)
        {
            InitializeComponent();
            _composite = composite;
            functionTypeList.ListViewItemSorter = _sorter;

            List<CathodeEntityDatabase.EntityDefinition> entDefs = CathodeEntityDatabase.GetEntities();
            for (int i = 0; i < entDefs.Count;i++)
            {
                if (!Enum.TryParse(entDefs[i].className, out FunctionType type))
                    continue;

                FunctionType? inherited = EntityUtils.GetBaseFunction(type);

                ListViewItem item = new ListViewItem(entDefs[i].className);
                item.ImageIndex = 0;
                item.SubItems.Add(inherited == null ? "" : inherited.Value.ToString());
                item.Tag = entDefs[i];

                _items.Add(item);
            }

            searchText.Text = SettingsManager.GetString(Singleton.Settings.PreviouslySearchedFunctionType);
            Search();

            SelectFuncType(SettingsManager.GetString(Singleton.Settings.PreviouslySelectedFunctionType));

            addDefaultParams.Checked = SettingsManager.GetBool(Singleton.Settings.PreviouslySearchedParamPopulation, false);
            createNode.Checked = SettingsManager.GetBool(Singleton.Settings.MakeNodeWhenMakeEntity);
            createNode.Visible = flowgraphMode;

#if AUTO_POPULATE_PARAMS
            addDefaultParams.Checked = true;
            addDefaultParams.Visible = false;
#endif
        }

        private void searchText_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string selected = functionTypeList.SelectedItems.Count > 0 ? functionTypeList.SelectedItems[0].Text : "";

            functionTypeList.BeginUpdate();
            functionTypeList.Items.Clear();
            functionTypeList.Items.AddRange(_items.Where(o => o.Text.ToUpper().Contains(searchText.Text.ToUpper())).ToList().ToArray());
            functionTypeList.EndUpdate();

            SelectFuncType(selected);

            typesCount.Text = "Showing " + functionTypeList.Items.Count;
            SettingsManager.SetString(Singleton.Settings.PreviouslySearchedFunctionType, searchText.Text);
        }

        private void clearSearchBtn_Click(object sender, EventArgs e)
        {
            searchText.Text = "";
            Search();
        }

        private void FunctionTypeList_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
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
            this.functionTypeList.Sort();
        }

        private void createEntity_Click(object sender, EventArgs e)
        {
            if (entityName.Text == "")
            {
                MessageBox.Show("Please enter an entity name!", "No name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (functionTypeList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a function type!", "No type.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CathodeEntityDatabase.EntityDefinition entDef = (CathodeEntityDatabase.EntityDefinition)functionTypeList.SelectedItems[0].Tag;
            if (!Enum.TryParse(entDef.className, out FunctionType function))
            {
                MessageBox.Show("Failed to lookup function type.", "Invalid function type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //A composite can only have one PhysicsSystem
            if (function == FunctionType.PhysicsSystem && _composite.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.PhysicsSystem)) != null)
            {
                MessageBox.Show("You are trying to add a PhysicsSystem entity to a composite that already has one applied.", "PhysicsSystem error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //A composite can only have one EnvironmentModelReference
            if (function == FunctionType.EnvironmentModelReference && _composite.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.EnvironmentModelReference)) != null)
            {
                MessageBox.Show("You are trying to add a EnvironmentModelReference entity to a composite that already has one applied.", "EnvironmentModelReference error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Singleton.OnEntityAddPending?.Invoke();
            Entity newEntity = _composite.AddFunction(function);

            if (addDefaultParams.Checked)
            {
                ParameterUtils.AddAllDefaultParameters(newEntity, _composite); //todo: don't want to apply all variants

                newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.NONE); //TODO
                newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.RESOURCE); //TODO

                if (function != FunctionType.Zone)
                    newEntity.RemoveParameter("name");
            }

            EntityUtils.SetName(_composite, newEntity, entityName.Text);
            SettingsManager.SetString(Singleton.Settings.PreviouslySelectedFunctionType, entDef.className);
            SettingsManager.SetBool(Singleton.Settings.PreviouslySearchedParamPopulation, addDefaultParams.Checked);

            Singleton.OnEntityAdded?.Invoke(newEntity);
            this.Close();
        }

        private void CreateEntityOnEnterKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                createEntity.PerformClick();
        }

        private void SelectFuncType(string type)
        {
            if (type == "")
            {
                return;
            }

            for (int i = 0; i < functionTypeList.Items.Count; i++)
            {
                if (functionTypeList.Items[i].Text == type)
                {
                    functionTypeList.Items[i].Selected = true;
                    break;
                }
            }
        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://opencage.co.uk/docs/cathode-entities/#entities");
        }

        private void createNode_CheckedChanged(object sender, EventArgs e)
        {
            if (createNode.Checked != SettingsManager.GetBool(Singleton.Settings.MakeNodeWhenMakeEntity))
                Singleton.Editor.ToggleMakeNodeWhenMakeEntity();
        }
    }
}
