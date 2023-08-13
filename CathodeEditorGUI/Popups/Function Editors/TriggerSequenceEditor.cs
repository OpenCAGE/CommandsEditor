using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
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
        EntityDisplay _entityDisplay;

        public TriggerSequenceEditor(EntityDisplay entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, entityDisplay.Content)
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
                string thisHierarchy;
                CommandsUtils.ResolveHierarchy(Editor.commands, _entityDisplay.Composite, _triggerSequence.entities[i].connectedEntity.hierarchy, out Composite comp, out thisHierarchy);

                string toAdd = "[" + _triggerSequence.entities[i].timing + "s] " + thisHierarchy;
                entity_list.Items.Add(toAdd);
            }
            entity_list.EndUpdate();
            entity_list.SelectedIndex = indexToSelect;
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

            if (entity_list.SelectedIndex == -1) return;
            int index = entity_list.SelectedIndex;
            _triggerSequence.entities[index].timing = Convert.ToSingle(entityTriggerDelay.Text);
            LoadSelectedEntity();
            ReloadEntityList();
            entity_list.SelectedIndex = index;
        }

        private void trigger_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedEntity();
        }

        private void LoadSelectedEntity()
        {
            if (entity_list.SelectedIndex == -1)
            {
                entityHierarchy.Text = "";
                entityTriggerDelay.Text = "0.0";
                selectedEntityDetails.Visible = false;
                return;
            }

            string thisHierarchy;
            CommandsUtils.ResolveHierarchy(Editor.commands, _entityDisplay.Composite, _triggerSequence.entities[entity_list.SelectedIndex].connectedEntity.hierarchy, out Composite comp, out thisHierarchy);

            entityHierarchy.Text = thisHierarchy;
            entityTriggerDelay.Text = _triggerSequence.entities[entity_list.SelectedIndex].timing.ToString();
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
            EditHierarchy hierarchyEditor = new EditHierarchy(_content, _entityDisplay.Composite, true);
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            if (entity_list.SelectedIndex == -1) return;
            int index = entity_list.SelectedIndex;
            _triggerSequence.entities[index].connectedEntity.hierarchy = generatedHierarchy;
            LoadSelectedEntity();
            ReloadEntityList();
            entity_list.SelectedIndex = index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _triggerSequence.entities.Count; i++)
            {
                if (_triggerSequence.entities[i].connectedEntity.hierarchy.Count == 0 || _triggerSequence.entities[i].connectedEntity.hierarchy.Count == 1)
                {
                    MessageBox.Show("One or more triggers does not point to a node!", "Trigger setup incorrectly!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            this.Close();
        }

        private void addNewEntity_Click(object sender, EventArgs e)
        {
            TriggerSequence.Entity trigger = new TriggerSequence.Entity();

            int insertIndex = (entity_list.SelectedIndex == -1) ? _triggerSequence.entities.Count : entity_list.SelectedIndex + 1;
            _triggerSequence.entities.Insert(insertIndex, trigger);

            ReloadEntityList();
            entity_list.SelectedIndex = insertIndex;
            LoadSelectedEntity();
        }
        private void deleteSelectedEntity_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedIndex == -1) return;
            _triggerSequence.entities.RemoveAt(entity_list.SelectedIndex);
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

        private void trigger_list_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadSelectedTriggers();
        }

        private void moveUp_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedIndex == -1) return;
            if (entity_list.SelectedIndex == 0) return;

            TriggerSequence.Entity toMoveDown = _triggerSequence.entities[entity_list.SelectedIndex - 1];
            TriggerSequence.Entity toMoveUp = _triggerSequence.entities[entity_list.SelectedIndex];

            _triggerSequence.entities[entity_list.SelectedIndex - 1] = toMoveUp;
            _triggerSequence.entities[entity_list.SelectedIndex] = toMoveDown;

            ReloadEntityList(entity_list.SelectedIndex - 1);
        }

        private void moveDown_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedIndex == -1) return;
            if (entity_list.SelectedIndex == _triggerSequence.entities.Count - 1) return;

            TriggerSequence.Entity toMoveUp = _triggerSequence.entities[entity_list.SelectedIndex + 1];
            TriggerSequence.Entity toMoveDown = _triggerSequence.entities[entity_list.SelectedIndex];

            _triggerSequence.entities[entity_list.SelectedIndex + 1] = toMoveDown;
            _triggerSequence.entities[entity_list.SelectedIndex] = toMoveUp;

            ReloadEntityList(entity_list.SelectedIndex + 1);
        }

        private void open_entity_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedIndex == -1) return;

            Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, _entityDisplay.Composite, _triggerSequence.entities[entity_list.SelectedIndex].connectedEntity.hierarchy, out Composite comp, out string h);
            if (comp == null || ent == null)
            {
                MessageBox.Show("Failed to resolve entity! Can not load to it.", "Entity pointer corrupted!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _entityDisplay.CompositeDisplay.CommandsDisplay.LoadCompositeAndEntity(comp, ent);
        }
    }
}
