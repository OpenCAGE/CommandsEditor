using CATHODE;
using CATHODE.Scripting;
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
        TriggerSequence node = null;
        public TriggerSequenceEditor(CommandsEditor editor, TriggerSequence _node) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            InitializeComponent();
            node = _node;

            entityTriggerDelay.Text = "0.0";
            this.Text = "TriggerSequence Editor: " + EntityUtils.GetName(Editor.selected.composite.shortGUID, _node.shortGUID);
            selectedEntityDetails.Visible = false;
            selectedTriggerDetails.Visible = false;

            ReloadEntityList();
            ReloadTriggerList();
        }

        private void ReloadEntityList()
        {
            entity_list.BeginUpdate();
            entity_list.Items.Clear();
            for (int i = 0; i < node.entities.Count; i++)
            {
                string thisHierarchy;
                CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, node.entities[i].connectedEntity.hierarchy, out Composite comp, out thisHierarchy);

                string toAdd = "[" + node.entities[i].timing + "s] " + thisHierarchy;
                entity_list.Items.Add(toAdd);
            }
            entity_list.EndUpdate();
        }
        private void ReloadTriggerList()
        {
            trigger_list.BeginUpdate();
            trigger_list.Items.Clear();
            for (int i = 0; i < node.events.Count; i++)
            {
                trigger_list.Items.Add(ShortGuidUtils.FindString(node.events[i].start) + " -> " + ShortGuidUtils.FindString(node.events[i].end));
            }
            trigger_list.EndUpdate();
        }

        private void triggerDelay_TextChanged(object sender, EventArgs e)
        {
            entityTriggerDelay.Text = EditorUtils.ForceStringNumeric(entityTriggerDelay.Text, true);

            if (entity_list.SelectedIndex == -1) return;
            int index = entity_list.SelectedIndex;
            node.entities[index].timing = Convert.ToSingle(entityTriggerDelay.Text);
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
            CommandsUtils.ResolveHierarchy(Editor.commands, Editor.selected.composite, node.entities[entity_list.SelectedIndex].connectedEntity.hierarchy, out Composite comp, out thisHierarchy);

            entityHierarchy.Text = thisHierarchy;
            entityTriggerDelay.Text = node.entities[entity_list.SelectedIndex].timing.ToString();
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

            triggerStartParam.Text = ShortGuidUtils.FindString(node.events[trigger_list.SelectedIndex].start);
            triggerEndParam.Text = ShortGuidUtils.FindString(node.events[trigger_list.SelectedIndex].end);
            selectedTriggerDetails.Visible = true;
        }

        private void selectEntToPointTo_Click(object sender, EventArgs e)
        {
            EditHierarchy hierarchyEditor = new EditHierarchy(_editor, Editor.selected.composite, true);
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            if (entity_list.SelectedIndex == -1) return;
            int index = entity_list.SelectedIndex;
            node.entities[index].connectedEntity.hierarchy = generatedHierarchy;
            LoadSelectedEntity();
            ReloadEntityList();
            entity_list.SelectedIndex = index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < node.entities.Count; i++)
            {
                if (node.entities[i].connectedEntity.hierarchy.Count == 0 || node.entities[i].connectedEntity.hierarchy.Count == 1)
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

            int insertIndex = (entity_list.SelectedIndex == -1) ? node.entities.Count : entity_list.SelectedIndex + 1;
            node.entities.Insert(insertIndex, trigger);

            ReloadEntityList();
            entity_list.SelectedIndex = insertIndex;
            LoadSelectedEntity();
        }
        private void deleteSelectedEntity_Click(object sender, EventArgs e)
        {
            if (entity_list.SelectedIndex == -1) return;
            node.entities.RemoveAt(entity_list.SelectedIndex);
            ReloadEntityList();
            LoadSelectedEntity();
        }

        private void addNewParamTrigger_Click(object sender, EventArgs e)
        {
            TriggerSequence.Event trigger = new TriggerSequence.Event(ShortGuidUtils.Generate(triggerStartParam.Text), ShortGuidUtils.Generate(triggerEndParam.Text));

            int insertIndex = (trigger_list.SelectedIndex == -1) ? node.events.Count : trigger_list.SelectedIndex + 1;
            node.events.Insert(insertIndex, trigger);

            ReloadTriggerList();
            trigger_list.SelectedIndex = insertIndex;
            LoadSelectedTriggers();
        }
        private void deleteParamTrigger_Click(object sender, EventArgs e)
        {
            if (trigger_list.SelectedIndex == -1) return;
            node.events.RemoveAt(trigger_list.SelectedIndex);
            ReloadTriggerList();
            LoadSelectedTriggers();
        }

        private void saveTrigger_Click(object sender, EventArgs e)
        {
            if (trigger_list.SelectedIndex == -1) return;
            int index = trigger_list.SelectedIndex;
            node.events[index].start = ShortGuidUtils.Generate(triggerStartParam.Text);
            node.events[index].end = ShortGuidUtils.Generate(triggerEndParam.Text);
            LoadSelectedTriggers();
            ReloadTriggerList();
            trigger_list.SelectedIndex = index;
        }

        private void trigger_list_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadSelectedTriggers();
        }

    }
}
