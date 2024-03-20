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
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class AddEntity_CompositeInstance : BaseWindow
    {
        private TreeUtility _treeUtility;
        private CompositeDisplay _compositeDisplay;

        public AddEntity_CompositeInstance(CompositeDisplay compositeDisplay) : base(WindowClosesOn.NEW_COMPOSITE_SELECTION | WindowClosesOn.COMMANDS_RELOAD)
        {
            InitializeComponent();

            _treeUtility = new TreeUtility(compositeTree);
            _compositeDisplay = compositeDisplay;

            _treeUtility.UpdateFileTree(Content.commands.GetCompositeNames().ToList());

            searchText.Text = SettingsManager.GetString(Singleton.Settings.PreviouslySearchedCompInstType);
            searchBtn_Click(null, null);

            string funcToSelect = SettingsManager.GetString(Singleton.Settings.PreviouslySelectedCompInstType);
            if (funcToSelect != "")
                _treeUtility.SelectNode(funcToSelect);

            addDefaultParams.Checked = SettingsManager.GetBool(Singleton.Settings.PreviouslySearchedParamPopulationComp, false);
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            List<string> filteredCompositeNames = new List<string>();
            List<Composite> filteredComposites = new List<Composite>();
            for (int i = 0; i < Content.commands.Entries.Count; i++)
            {
                string name = Content.commands.Entries[i].name.Replace('\\', '/');

                if (SettingsManager.GetBool(Singleton.Settings.CompNameOnlyOpt) == true)
                {
                    string[] nameSplit = name.Split('/');
                    name = nameSplit[nameSplit.Length - 1];
                }

                if (!name.ToUpper().Contains(searchText.Text.Replace('\\', '/').ToUpper())) 
                    continue;

                filteredCompositeNames.Add(Content.commands.Entries[i].name.Replace('\\', '/'));
                filteredComposites.Add(Content.commands.Entries[i]);
            }
            _treeUtility.UpdateFileTree(filteredCompositeNames);

            if (searchText.Text != "")
                compositeTree.ExpandAll();

            SettingsManager.SetString(Singleton.Settings.PreviouslySearchedCompInstType, searchText.Text);
        }

        private void clearSearchBtn_Click(object sender, EventArgs e)
        {
            searchText.Text = "";
            searchBtn_Click(null, null);
        }

        private void compositeTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (compositeTree.SelectedNode == null || compositeTree.SelectedNode.Tag == null)
            {
                compositeNameDisplay.Text = "";
                return;
            }
            compositeNameDisplay.Text = ((TreeItem)compositeTree.SelectedNode.Tag).String_Value;
        }

        private void createEntity_Click(object sender, EventArgs e)
        {
            if (entityName.Text == "")
            {
                MessageBox.Show("Please enter an entity name!", "No name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (compositeTree.SelectedNode == null)
            {
                MessageBox.Show("Please select a composite to instance!", "No type.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TreeItem item = (TreeItem)compositeTree.SelectedNode.Tag;
            Composite comp = Content.commands.GetComposite(item.String_Value);
            if (item.Item_Type != TreeItemType.EXPORTABLE_FILE || comp == null)
            {
                MessageBox.Show("Failed to lookup composite.", "Invalid composite", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Check logic errors (we can't have cyclical references)
            if (comp == _compositeDisplay.Composite)
            {
                MessageBox.Show("You cannot create an entity which instances the composite it is contained with - this will result in an infinite loop at runtime! Please check your logic!.", "Logic error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Singleton.OnEntityAddPending?.Invoke();

            Entity newEntity = _compositeDisplay.Composite.AddFunction(comp, addDefaultParams.Checked);
            EntityUtils.SetName(_compositeDisplay.Composite, newEntity, entityName.Text);

            Content.editor_utils.GenerateCompositeInstances(Content.commands);

            SettingsManager.SetString(Singleton.Settings.PreviouslySelectedCompInstType, item.String_Value);
            SettingsManager.SetBool(Singleton.Settings.PreviouslySearchedParamPopulationComp, addDefaultParams.Checked);

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
