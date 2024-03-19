using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using OpenCAGE;
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
    public partial class AddEntity_Function : BaseWindow
    {
        private List<CathodeEntityDatabase.EntityDefinition> _entDefs;
        private CompositeDisplay _compositeDisplay;

        public AddEntity_Function(CompositeDisplay compositeDisplay) : base (WindowClosesOn.NEW_COMPOSITE_SELECTION | WindowClosesOn.COMMANDS_RELOAD)
        {
            InitializeComponent();

            _entDefs = CathodeEntityDatabase.GetEntities();
            _compositeDisplay = compositeDisplay;

            searchText.Text = SettingsManager.GetString(Singleton.Settings.PreviouslySearchedFunctionType);
            searchBtn_Click(null, null);

            string funcToSelect = SettingsManager.GetString(Singleton.Settings.PreviouslySelectedFunctionType);
            if (funcToSelect != "") 
                functionTypeList.SelectedItem = funcToSelect;

            addDefaultParams.Checked = SettingsManager.GetBool(Singleton.Settings.PreviouslySearchedParamPopulation, false);
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string selected = functionTypeList.SelectedItem?.ToString();
            List<CathodeEntityDatabase.EntityDefinition> filteredEntDefs = _entDefs.Where(o => o.className.ToUpper().Contains(searchText.Text.ToUpper())).ToList();

            functionTypeList.BeginUpdate();
            functionTypeList.Items.Clear();
            for (int i = 0; i < filteredEntDefs.Count; i++)
                functionTypeList.Items.Add(filteredEntDefs[i].className);
            functionTypeList.EndUpdate();

            if (selected != "")
                functionTypeList.SelectedItem = selected;

            typesCount.Text = "Showing " + functionTypeList.Items.Count + " Types";
            SettingsManager.SetString(Singleton.Settings.PreviouslySearchedFunctionType, searchText.Text);
        }

        private void clearSearchBtn_Click(object sender, EventArgs e)
        {
            searchText.Text = "";
            searchBtn_Click(null, null);
        }

        private void createEntity_Click(object sender, EventArgs e)
        {
            if (entityName.Text == "")
            {
                MessageBox.Show("Please enter an entity name!", "No name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (functionTypeList.SelectedItem == null)
            {
                MessageBox.Show("Please select a function type!", "No type.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CathodeEntityDatabase.EntityDefinition entDef = _entDefs.FirstOrDefault(o => o.className == functionTypeList.SelectedItem.ToString());
            if (!Enum.TryParse(entDef.className, out FunctionType function))
            {
                MessageBox.Show("Failed to lookup function type.", "Invalid function type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //A composite can only have one PhysicsSystem
            if (function == FunctionType.PhysicsSystem && _compositeDisplay.Composite.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.PhysicsSystem)) != null)
            {
                MessageBox.Show("You are trying to add a PhysicsSystem entity to a composite that already has one applied.", "PhysicsSystem error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //A composite can only have one EnvironmentModelReference
            if (function == FunctionType.EnvironmentModelReference && _compositeDisplay.Composite.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.EnvironmentModelReference)) != null)
            {
                MessageBox.Show("You are trying to add a EnvironmentModelReference entity to a composite that already has one applied.", "EnvironmentModelReference error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Singleton.OnEntityAddPending?.Invoke();
            Entity newEntity = _compositeDisplay.Composite.AddFunction(function, addDefaultParams.Checked);

            //TODO: currently we don't support these properly
            if (addDefaultParams.Checked)
            {
                newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.NONE); //TODO
                newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.RESOURCE); //TODO
            }

            EntityUtils.SetName(_compositeDisplay.Composite, newEntity, entityName.Text);
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
        private void SearchFuncTypeOnEnterKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                searchBtn.PerformClick();
        }
    }
}
