using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using CommandsEditor.Popups.UserControls;
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
    public partial class TriggerSequenceEditor : BaseWindow
    {
        TriggerSequence _triggerSequence = null;
        EntityInspector _entityDisplay;

        public TriggerSequenceEditor(EntityInspector entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();
            _entityDisplay = entityDisplay; 
            _triggerSequence = (TriggerSequence)_entityDisplay.Entity;

            entityTriggerDelay.Text = "0.0";
            this.Text = "TriggerSequence Editor: " + EntityUtils.GetName(_entityDisplay.Composite.shortGUID, _triggerSequence.shortGUID);
            selectedEntityDetails.Visible = false;
            selectedTriggerDetails.Visible = false;

            ReloadEntityList();
            ReloadTriggerList();
        }

        private void ReloadEntityList(int indexToSelect = -1)
        {
            entity_list.BeginUpdate();
            entity_list.Items.Clear();
            for (int i = 0; i < _triggerSequence.entities.Count; i++)
            {
                ListViewItem item = new ListViewItem();

                string thisHierarchy;
                CommandsUtils.ResolveHierarchy(Content.commands, _entityDisplay.Composite, _triggerSequence.entities[i].connectedEntity.path, out Composite comp, out thisHierarchy, SettingsManager.GetBool("CS_ShowEntityIDs"));
                item.Text = thisHierarchy;

                item.SubItems.Add(_triggerSequence.entities[i].timing + "s");
                entity_list.Items.Add(item);
            }
            entity_list.EndUpdate();

            if (indexToSelect != -1)
                entity_list.Items[indexToSelect].Selected = true;
        }
        private void ReloadTriggerList()
        {
            trigger_list.BeginUpdate();
            trigger_list.Items.Clear();
            for (int i = 0; i < _triggerSequence.events.Count; i++)
            {
                trigger_list.Items.Add(ShortGuidUtils.FindString(_triggerSequence.events[i].start) + " -> " + ShortGuidUtils.FindString(_triggerSequence.events[i].end));
            }
            trigger_list.EndUpdate();
        }

        private void triggerDelay_TextChanged(object sender, EventArgs e)
        {
            entityTriggerDelay.Text = EditorUtils.ForceStringNumeric(entityTriggerDelay.Text, true);

            if (entity_list.SelectedItems.Count == 0) 
                return;

            _triggerSequence.entities[entity_list.SelectedItems[0].Index].timing = Convert.ToSingle(entityTriggerDelay.Text);
            entity_list.SelectedItems[0].SubItems[1].Text = entityTriggerDelay.Text + "s";
        }

        private void entity_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedEntity();
        }

        private void LoadSelectedEntity()
        {
            moveUp.Enabled = entity_list.SelectedItems.Count != 0;
            moveDown.Enabled = entity_list.SelectedItems.Count != 0;

            if (entity_list.SelectedItems.Count == 0)
            {
                entityHierarchy.Text = "";
                entityTriggerDelay.Text = "0.0";
                selectedEntityDetails.Visible = false;
                return;
            }

            int index = entity_list.SelectedItems[0].Index;

            CommandsUtils.ResolveHierarchy(Content.commands, _entityDisplay.Composite, _triggerSequence.entities[index].connectedEntity.path, out Composite comp, out string thisHierarchy, SettingsManager.GetBool("CS_ShowEntityIDs"));

            entityHierarchy.Text = thisHierarchy;
            entityTriggerDelay.Text = _triggerSequence.entities[index].timing.ToString();
            selectedEntityDetails.Visible = true;
        }

        private void LoadSelectedTriggers()
        {
            if (trigger_list.SelectedIndex == -1)
            {
                triggerStartParam.Text = "";
                triggerEndParam.Text = "";
                selectedTriggerDetails.Visible = false;
                return;
            }

            triggerStartParam.Text = ShortGuidUtils.FindString(_triggerSequence.events[trigger_list.SelectedIndex].start);
            triggerEndParam.Text = ShortGuidUtils.FindString(_triggerSequence.events[trigger_list.SelectedIndex].end);
            selectedTriggerDetails.Visible = true;
        }

        private void selectEntToPointTo_Click(object sender, EventArgs e)
        {
            SelectHierarchy hierarchyEditor = new SelectHierarchy(_entityDisplay.Composite, new CompositeEntityList.DisplayOptions()
            {
                DisplayAliases = false,
                DisplayFunctions = true,
                DisplayProxies = false,
                DisplayVariables = false,
            });
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(ShortGuid[] generatedHierarchy)
        {
            if (entity_list.SelectedItems.Count == 0) return;
            int index = entity_list.SelectedItems[0].Index;
            _triggerSequence.entities[index].connectedEntity.path = generatedHierarchy;
            LoadSelectedEntity();
            ReloadEntityList();
            entity_list.Items[index].Selected = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _triggerSequence.entities.Count; i++)
            {
                if (_triggerSequence.entities[i].connectedEntity.path.Length == 0 || _triggerSequence.entities[i].connectedEntity.path.Length == 1)
                {
                    MessageBox.Show("One or more triggers does not point to a node!", "Trigger setup incorrectly!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            this.Close();
        }

        private void addNewEntity_Click(object sender, EventArgs e)
        {
            SelectHierarchy hierarchyEditor = new SelectHierarchy(_entityDisplay.Composite, new CompositeEntityList.DisplayOptions()
            {
                DisplayAliases = false,
                DisplayFunctions = true,
                DisplayProxies = false,
                DisplayVariables = false,
            });
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += addNewEntity_HierarchyGenerated;
        }
        private void addNewEntity_HierarchyGenerated(ShortGuid[] generatedHierarchy)
        {
            TriggerSequence.Entity trigger = new TriggerSequence.Entity();
            trigger.connectedEntity.path = generatedHierarchy;

            int insertIndex = (entity_list.SelectedItems.Count == 0) ? _triggerSequence.entities.Count : entity_list.SelectedItems[0].Index + 1;
            _triggerSequence.entities.Insert(insertIndex, trigger);

            ReloadEntityList();
            entity_list.Items[insertIndex].Selected = true;
            entity_list.EnsureVisible(insertIndex);
            LoadSelectedEntity();
        }

        private void deleteSelectedEntity_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedItems.Count == 0) 
                return;
            _triggerSequence.entities.RemoveAt(entity_list.SelectedItems[0].Index);
            ReloadEntityList();
            LoadSelectedEntity();
        }

        private void addNewParamTrigger_Click(object sender, EventArgs e)
        {
            TriggerSequence.Event trigger = new TriggerSequence.Event(ShortGuidUtils.Generate(triggerStartParam.Text), ShortGuidUtils.Generate(triggerEndParam.Text));

            int insertIndex = (trigger_list.SelectedIndex == -1) ? _triggerSequence.events.Count : trigger_list.SelectedIndex + 1;
            _triggerSequence.events.Insert(insertIndex, trigger);

            ReloadTriggerList();
            trigger_list.SelectedIndex = insertIndex;
            LoadSelectedTriggers();
        }
        private void deleteParamTrigger_Click(object sender, EventArgs e)
        {
            if (trigger_list.SelectedIndex == -1) return;
            _triggerSequence.events.RemoveAt(trigger_list.SelectedIndex);
            ReloadTriggerList();
            LoadSelectedTriggers();
        }

        private void saveTrigger_Click(object sender, EventArgs e)
        {
            if (trigger_list.SelectedIndex == -1) return;
            int index = trigger_list.SelectedIndex;
            _triggerSequence.events[index].start = ShortGuidUtils.Generate(triggerStartParam.Text);
            _triggerSequence.events[index].end = ShortGuidUtils.Generate(triggerEndParam.Text);
            LoadSelectedTriggers();
            ReloadTriggerList();
            trigger_list.SelectedIndex = index;
        }

        private void trigger_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedTriggers();
        }

        private void moveUp_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedItems.Count == 0) return;
            int index = entity_list.SelectedItems[0].Index;
            if (index == 0) return;

            TriggerSequence.Entity toMoveDown = _triggerSequence.entities[index - 1];
            TriggerSequence.Entity toMoveUp = _triggerSequence.entities[index];

            _triggerSequence.entities[index - 1] = toMoveUp;
            _triggerSequence.entities[index] = toMoveDown;

            ReloadEntityList(index - 1);
        }

        private void moveDown_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedItems.Count == 0) return;
            int index = entity_list.SelectedItems[0].Index;
            if (index == _triggerSequence.entities.Count - 1) return;

            TriggerSequence.Entity toMoveUp = _triggerSequence.entities[index + 1];
            TriggerSequence.Entity toMoveDown = _triggerSequence.entities[index];

            _triggerSequence.entities[index + 1] = toMoveDown;
            _triggerSequence.entities[index] = toMoveUp;

            ReloadEntityList(index + 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (entity_list.CheckedItems.Count == 0)
                return;

            if (MessageBox.Show("You are about to remove " + entity_list.CheckedItems.Count + " triggers. Are you sure?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) 
                return;

            List<int> invalidIndexes = new List<int>();
            foreach (ListViewItem item in entity_list.CheckedItems)
                invalidIndexes.Add(item.Index);

            List<TriggerSequence.Entity> filteredEnts = new List<TriggerSequence.Entity>();
            for (int i = 0; i < _triggerSequence.entities.Count; i++)
            {
                if (invalidIndexes.Contains(i))
                    continue;
                filteredEnts.Add(_triggerSequence.entities[i]);
            }
            _triggerSequence.entities = filteredEnts;

            ReloadEntityList();
            LoadSelectedEntity();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedItems.Count == 0) 
                return;

            if (MessageBox.Show("Going to this entity will close the TriggerSequence editor.\nAre you sure you want to continue?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, _entityDisplay.Composite, _triggerSequence.entities[entity_list.SelectedItems[0].Index].connectedEntity.path, out Composite comp, out string h);
            if (comp == null || ent == null)
            {
                MessageBox.Show("Failed to resolve entity! Can not load to it.", "Entity pointer corrupted!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _entityDisplay.CompositeDisplay.CommandsDisplay.LoadCompositeAndEntity(comp, ent);
        }
    }
}
