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
    public partial class ShowCrossRefs : BaseWindow
    {
        public Action<Composite, Entity> OnEntitySelected;

        private List<EntityRef> entities = new List<EntityRef>();

        private EntityDisplay _entityDisplay;

        public ShowCrossRefs(EntityDisplay entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, entityDisplay.Content)
        {
            _entityDisplay = entityDisplay;
            InitializeComponent();
            UpdateUI(CurrentDisplay.PROXIES);
        }

        private void jumpToEntity_Click(object sender, EventArgs e)
        {
            if (referenceList.SelectedIndex == -1) return;
            OnEntitySelected?.Invoke(entities[referenceList.SelectedIndex].composite, entities[referenceList.SelectedIndex].entity);
            this.Close();
        }

        private void showLinkedProxies_Click(object sender, EventArgs e)
        {
            UpdateUI(CurrentDisplay.PROXIES);
        }
        private void showLinkedOverrides_Click(object sender, EventArgs e)
        {
            UpdateUI(CurrentDisplay.ALIASES);
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
            showLinkedOverrides.Enabled = display != CurrentDisplay.ALIASES;
            showLinkedTriggerSequences.Enabled = display != CurrentDisplay.TRIGGERSEQUENCES;
            showLinkedCageAnimations.Enabled = display != CurrentDisplay.CAGEANIMATIONS;

            switch (display)
            {
                case CurrentDisplay.PROXIES:
                    label.Text = "Proxies";
                    break;
                case CurrentDisplay.ALIASES:
                    label.Text = "Aliases";
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

            bool showIDs = SettingsManager.GetBool("CS_ShowEntityIDs");

            foreach (Composite comp in Content.commands.Entries)
            {
                switch (display)
                {
                    case CurrentDisplay.PROXIES:
                        foreach (ProxyEntity prox in comp.proxies)
                        {
                            Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, comp, prox.proxy.path, out Composite compRef, out string str, showIDs);
                            if (ent != _entityDisplay.Entity) continue;
                            entities.Add(new EntityRef() { composite = comp, entity = prox });
                            referenceList.Items.Add(_entityDisplay.Content.editor_utils.GenerateEntityName(prox, comp));
                        }
                        break;
                    case CurrentDisplay.ALIASES:
                        foreach (AliasEntity ovr in comp.aliases)
                        {
                            Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, comp, ovr.alias.path, out Composite compRef, out string str, showIDs);
                            if (ent != _entityDisplay.Entity) continue;
                            entities.Add(new EntityRef() { composite = comp, entity = ovr });
                            referenceList.Items.Add(_entityDisplay.Content.editor_utils.GenerateEntityName(ovr, comp));
                        }
                        break;
                    case CurrentDisplay.TRIGGERSEQUENCES:
                        foreach (TriggerSequence trig in comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence)))
                        {
                            foreach (TriggerSequence.Entity trigger in trig.entities)
                            {
                                Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, comp, trigger.connectedEntity.path, out Composite compRef, out string str, showIDs);
                                if (ent != _entityDisplay.Entity) continue;
                                entities.Add(new EntityRef() { composite = comp, entity = trig });
                                referenceList.Items.Add(_entityDisplay.Content.editor_utils.GenerateEntityName(trig, comp));
                            }
                        }
                        break;
                    case CurrentDisplay.CAGEANIMATIONS:
                        foreach (CAGEAnimation anim in comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation)))
                        {
                            foreach (CAGEAnimation.Connection connection in anim.connections)
                            {
                                Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, comp, connection.connectedEntity.path, out Composite compRef, out string str, showIDs);
                                if (ent != _entityDisplay.Entity) continue;
                                entities.Add(new EntityRef() { composite = comp, entity = anim });
                                referenceList.Items.Add(_entityDisplay.Content.editor_utils.GenerateEntityName(anim, comp));
                            }
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
            ALIASES,
            TRIGGERSEQUENCES,
            CAGEANIMATIONS,
        }

        private struct EntityRef
        {
            public Entity entity;
            public Composite composite;
        }
    }
}
