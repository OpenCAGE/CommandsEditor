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

        private CurrentDisplay _currentDisplay = CurrentDisplay.PROXIES;
        private Dictionary<CurrentDisplay, SynchronizedCollection<EntityRef>> _entityRefs = new Dictionary<CurrentDisplay, SynchronizedCollection<EntityRef>>();

        private EntityInspector _entityDisplay;

        public ShowCrossRefs(EntityInspector entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            _entityDisplay = entityDisplay;
            InitializeComponent();

            Parallel.For(0, 5, (i) =>
            {
                _entityRefs.Add((CurrentDisplay)i, GetEntityRefs((CurrentDisplay)i));
            });

            showLinkedProxies.Text = "Proxies (" + _entityRefs[CurrentDisplay.PROXIES].Count + ")";
            showLinkedOverrides.Text = "Aliases (" + _entityRefs[CurrentDisplay.ALIASES].Count + ")";
            showLinkedCageAnimations.Text = "CAGEAnimations (" + _entityRefs[CurrentDisplay.CAGEANIMATIONS].Count + ")";
            showLinkedTriggerSequences.Text = "TriggerSequences (" + _entityRefs[CurrentDisplay.TRIGGERSEQUENCES].Count + ")";

            showLinkedProxies.PerformClick();
        }

        private void jumpToEntity_Click(object sender, EventArgs e)
        {
            if (referenceList.SelectedIndex == -1) return;
            OnEntitySelected?.Invoke(_entityRefs[_currentDisplay][referenceList.SelectedIndex].composite, _entityRefs[_currentDisplay][referenceList.SelectedIndex].entity);
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
            Cursor.Current = Cursors.WaitCursor;

            _currentDisplay = display;

            showLinkedProxies.Enabled = display != CurrentDisplay.PROXIES;
            showLinkedOverrides.Enabled = display != CurrentDisplay.ALIASES;
            showLinkedTriggerSequences.Enabled = display != CurrentDisplay.TRIGGERSEQUENCES;
            showLinkedCageAnimations.Enabled = display != CurrentDisplay.CAGEANIMATIONS;

            label.Text = _entityRefs[display].Count + " ";
            switch (display)
            {
                case CurrentDisplay.PROXIES:
                    label.Text += "Proxies";
                    break;
                case CurrentDisplay.ALIASES:
                    label.Text += "Aliases";
                    break;
                case CurrentDisplay.TRIGGERSEQUENCES:
                    label.Text += "TriggerSequences";
                    break;
                case CurrentDisplay.CAGEANIMATIONS:
                    label.Text += "CAGEAnimations";
                    break;
            }
            label.Text += " pointing to this entity:";

            referenceList.BeginUpdate();
            referenceList.Items.Clear();

            foreach (EntityRef entityRef in _entityRefs[display])
                referenceList.Items.Add(_entityDisplay.Content.editor_utils.GenerateEntityName(entityRef.entity, entityRef.composite));

            referenceList.EndUpdate();
            Cursor.Current = Cursors.Default;
        }

        private SynchronizedCollection<EntityRef> GetEntityRefs(CurrentDisplay display)
        {
            bool showIDs = SettingsManager.GetBool(Singleton.Settings.EntIdOpt);
            SynchronizedCollection<EntityRef> entityRefs = new SynchronizedCollection<EntityRef>();
            Parallel.ForEach(Content.commands.Entries, (comp) =>
            {
                switch (display)
                {
                    case CurrentDisplay.PROXIES:
                        Parallel.ForEach(comp.proxies, (prox) =>
                        {
                            Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, comp, prox.proxy.path, out Composite compRef, out string str, showIDs);
                            if (ent == _entityDisplay.Entity) entityRefs.Add(new EntityRef() { composite = comp, entity = prox });
                        });
                        break;
                    case CurrentDisplay.ALIASES:
                        Parallel.ForEach(comp.aliases, (alias) =>
                        {
                            Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, comp, alias.alias.path, out Composite compRef, out string str, showIDs);
                            if (ent == _entityDisplay.Entity) entityRefs.Add(new EntityRef() { composite = comp, entity = alias });
                        });
                        break;
                    case CurrentDisplay.TRIGGERSEQUENCES:
                        List<FunctionEntity> triggerSequences = comp.functions.FindAll(o => o.function == FunctionType.TriggerSequence);
                        Parallel.ForEach(triggerSequences, (trigEnt) =>
                        {
                            TriggerSequence trig = (TriggerSequence)trigEnt;
                            Parallel.ForEach(trig.entities, (trigger) =>
                            {
                                Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, comp, trigger.connectedEntity.path, out Composite compRef, out string str, showIDs);
                                if (ent == _entityDisplay.Entity) entityRefs.Add(new EntityRef() { composite = comp, entity = trig });
                            });
                        });
                        break;
                    case CurrentDisplay.CAGEANIMATIONS:
                        List<FunctionEntity> cageAnims = comp.functions.FindAll(o => o.function == FunctionType.CAGEAnimation);
                        Parallel.ForEach(cageAnims, (animEnt) =>
                        {
                            CAGEAnimation anim = (CAGEAnimation)animEnt;
                            Parallel.ForEach(anim.connections, (connection) =>
                            {
                                Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, comp, connection.connectedEntity.path, out Composite compRef, out string str, showIDs);
                                if (ent == _entityDisplay.Entity) entityRefs.Add(new EntityRef() { composite = comp, entity = anim });
                            });
                        });
                        break;
                }
            });
            return entityRefs;
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
