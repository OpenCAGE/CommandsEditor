using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
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
    public partial class ShowCrossRefs : Form
    {
        public Action<ShortGuid, Composite> OnEntitySelected;

        private List<EntityRef> entities = new List<EntityRef>();

        public ShowCrossRefs()
        {
            InitializeComponent();
#if !DEBUG
            showLinkedCageAnimations.Visible = false;
#endif
            UpdateUI(CurrentDisplay.PROXIES);
        }

        private void jumpToEntity_Click(object sender, EventArgs e)
        {
            if (referenceList.SelectedIndex == -1) return;
            OnEntitySelected?.Invoke(entities[referenceList.SelectedIndex].entity, entities[referenceList.SelectedIndex].composite);
            this.Close();
        }

        private void showLinkedProxies_Click(object sender, EventArgs e)
        {
            UpdateUI(CurrentDisplay.PROXIES);
        }
        private void showLinkedOverrides_Click(object sender, EventArgs e)
        {
            UpdateUI(CurrentDisplay.OVERRIDES);
        }
        private void showLinkedTriggerSequences_Click(object sender, EventArgs e)
        {
            UpdateUI(CurrentDisplay.TRIGGERSEQUENCES);
        }
        private void showLinkedCageAnimations_Click(object sender, EventArgs e)
        {
            UpdateUI(CurrentDisplay.CAGEANIMATIONS);
        }

        private void UpdateUI(CurrentDisplay display)
        {
            referenceList.BeginUpdate();
            Cursor.Current = Cursors.WaitCursor;

            showLinkedProxies.Enabled = display != CurrentDisplay.PROXIES;
            showLinkedOverrides.Enabled = display != CurrentDisplay.OVERRIDES;
            showLinkedTriggerSequences.Enabled = display != CurrentDisplay.TRIGGERSEQUENCES;
            showLinkedCageAnimations.Enabled = display != CurrentDisplay.CAGEANIMATIONS;

            switch (display)
            {
                case CurrentDisplay.PROXIES:
                    label.Text = "Proxies";
                    break;
                case CurrentDisplay.OVERRIDES:
                    label.Text = "Overrides";
                    break;
                case CurrentDisplay.TRIGGERSEQUENCES:
                    label.Text = "TriggerSequences";
                    break;
                case CurrentDisplay.CAGEANIMATIONS:
                    label.Text = "CAGEAnimations";
                    break;
            }
            label.Text += " pointing to this entity:";

            referenceList.Items.Clear();
            entities.Clear();

            foreach (Composite comp in Editor.commands.Entries)
            {
                switch (display)
                {
                    case CurrentDisplay.PROXIES:
                        foreach (ProxyEntity prox in comp.proxies)
                        {
                            Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, prox.hierarchy, out Composite compRef, out string str);
                            if (ent != Editor.selected.entity) continue;
                            entities.Add(new EntityRef() { composite = comp, entity = prox.shortGUID });
                            referenceList.Items.Add(EditorUtils.GenerateEntityName(prox, comp));
                        }
                        break;
                    case CurrentDisplay.OVERRIDES:
                        foreach (OverrideEntity ovr in comp.overrides)
                        {
                            Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, ovr.hierarchy, out Composite compRef, out string str);
                            if (ent != Editor.selected.entity) continue;
                            entities.Add(new EntityRef() { composite = comp, entity = ovr.shortGUID });
                            referenceList.Items.Add(EditorUtils.GenerateEntityName(ovr, comp));
                        }
                        break;
                    case CurrentDisplay.TRIGGERSEQUENCES:
                        foreach (TriggerSequence trig in comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence)))
                        {
                            foreach (TriggerSequence.Entity trigger in trig.entities)
                            {
                                Entity ent = CommandsUtils.ResolveHierarchy(Editor.commands, comp, trigger.hierarchy, out Composite compRef, out string str);
                                if (ent != Editor.selected.entity) continue;
                                entities.Add(new EntityRef() { composite = comp, entity = trig.shortGUID });
                                referenceList.Items.Add(EditorUtils.GenerateEntityName(trig, comp));
                            }
                        }
                        break;
                    case CurrentDisplay.CAGEANIMATIONS:
                        foreach (CAGEAnimation anim in comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation)))
                        {
                            //TODO!
                        }
                        break;
                }
            }

            Cursor.Current = Cursors.Default;
            referenceList.EndUpdate();
        }

        private enum CurrentDisplay
        {
            PROXIES,
            OVERRIDES,
            TRIGGERSEQUENCES,
            CAGEANIMATIONS,
        }

        private struct EntityRef
        {
            public ShortGuid entity;
            public Composite composite;
        }
    }
}
