using CATHODE.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class TriggerSequenceEditor : Form
    {
        TriggerSequence node = null;
        public TriggerSequenceEditor(TriggerSequence _node)
        {
            InitializeComponent();
            node = _node;

            triggerDelay.Text = "0.0";
            this.Text = "TriggerSequence Editor: " + CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, _node.shortGUID);

            ReloadTriggerList();
            ReloadEventList();
        }

        private void ReloadTriggerList()
        {
            trigger_list.BeginUpdate();
            trigger_list.Items.Clear();
            for (int i = 0; i < node.triggers.Count; i++)
            {
                string thisHierarchy;
                EditorUtils.ResolveHierarchy(node.triggers[i].hierarchy, out CathodeComposite comp, out thisHierarchy);

                string toAdd = "[" + node.triggers[i].timing + "s] " + thisHierarchy;
                trigger_list.Items.Add(toAdd);
            }
            trigger_list.EndUpdate();
        }
        private void ReloadEventList()
        {
            for (int i = 0; i < node.events.Count; i++)
            {
                event_list.Items.Add(ShortGuidUtils.FindString(node.events[i].EventID) + "\n  - StartedID: " + ShortGuidUtils.FindString(node.events[i].StartedID) + "\n  - FinishedID: " + ShortGuidUtils.FindString(node.events[i].FinishedID));
            }
        }

        private void triggerDelay_TextChanged(object sender, EventArgs e)
        {
            triggerDelay.Text = EditorUtils.ForceStringNumeric(triggerDelay.Text, true);
        }

        private void trigger_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSelectedTrigger();
        }

        private void LoadSelectedTrigger()
        {
            if (trigger_list.SelectedIndex == -1)
            {
                triggerHierarchy.Text = "";
                triggerDelay.Text = "0.0";
                return;
            }

            string thisHierarchy;
            EditorUtils.ResolveHierarchy(node.triggers[trigger_list.SelectedIndex].hierarchy, out CathodeComposite comp, out thisHierarchy);

            triggerHierarchy.Text = thisHierarchy;
            triggerDelay.Text = node.triggers[trigger_list.SelectedIndex].timing.ToString();
        }

        private void selectEntToPointTo_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_EditHierarchy hierarchyEditor = new CathodeEditorGUI_EditHierarchy(CurrentInstance.selectedComposite);
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            int index = trigger_list.SelectedIndex;
            node.triggers[index].hierarchy = generatedHierarchy;
            LoadSelectedTrigger();
            ReloadTriggerList();
            trigger_list.SelectedIndex = index;
        }

        private void saveTriggerTime_Click(object sender, EventArgs e)
        {
            int index = trigger_list.SelectedIndex;
            node.triggers[index].timing = Convert.ToSingle(triggerDelay.Text);
            LoadSelectedTrigger();
            ReloadTriggerList();
            trigger_list.SelectedIndex = index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < node.triggers.Count; i++)
            {
                if (node.triggers[i].hierarchy.Count == 0 || node.triggers[i].hierarchy.Count == 1)
                {
                    MessageBox.Show("One or more triggers does not point to a node!", "Trigger setup incorrectly!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            this.Close();
        }

        private void addNewTrigger_Click(object sender, EventArgs e)
        {
            CathodeTriggerSequenceTrigger trigger = new CathodeTriggerSequenceTrigger();
            trigger.timing = 0.0f;
            trigger.hierarchy = new List<ShortGuid>();
            trigger.hierarchy.Add(new ShortGuid("00-00-00-00"));

            int insertIndex = (trigger_list.SelectedIndex == -1) ? node.triggers.Count : trigger_list.SelectedIndex;
            node.triggers.Insert(insertIndex, trigger);

            ReloadTriggerList();
            trigger_list.SelectedIndex = insertIndex;
            LoadSelectedTrigger();
        }

        private void deleteSelectedTrigger_Click(object sender, EventArgs e)
        {
            if (trigger_list.SelectedIndex == -1) return;
            node.triggers.RemoveAt(trigger_list.SelectedIndex);
        }
    }
}
