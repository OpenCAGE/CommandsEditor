using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using OpenCAGE;
using System;
using System.Collections.Generic;

namespace CommandsEditor
{
    public partial class ShowCompositeUses : BaseWindow
    {
        public Action<Composite, Entity> OnEntitySelected;

        private List<EntityRef> entities = new List<EntityRef>();
        private string _baseText = "Function Uses";

        public ShowCompositeUses(Composite composite = null) : base(composite == null ? WindowClosesOn.COMMANDS_RELOAD : WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            if (composite != null)
            {
                _baseText = "Composite Uses";
                label.Text = "Entities that instance the composite '" + composite.name + "':";
                entityVariant.Visible = false;
                Search(composite.shortGUID);
            }
            else
            {
                var entities = CathodeEntityDatabase.GetEntities();
                for (int i = 0; i < entities.Count; i++)
                    entityVariant.Items.Add(entities[i].className);
                entityVariant.SelectedIndex = 0;
            }
        }

        private void jumpToEntity_Click(object sender, EventArgs e)
        {
            if (referenceList.SelectedIndex == -1) return;
            OnEntitySelected?.Invoke(entities[referenceList.SelectedIndex].composite, entities[referenceList.SelectedIndex].entity);

            if (!SettingsManager.GetBool(Singleton.Settings.KeepUsesWindowOpen))
                this.Close();
        }

        private void entityVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            Search(ShortGuidUtils.Generate(entityVariant.Text));
        }

        private void Search(ShortGuid guid)
        {
            referenceList.BeginUpdate();
            referenceList.Items.Clear();
            entities.Clear();
            foreach (Composite comp in Content.commands.Entries)
            {
                foreach (FunctionEntity ent in comp.functions.FindAll(o => o.function == guid))
                {
                    entities.Add(new EntityRef() { composite = comp, entity = ent });
                    referenceList.Items.Add(comp.name + ": " + EntityUtils.GetName(comp.shortGUID, ent.shortGUID));
                }
            }
            Text = _baseText + " - " + (entityVariant.Text != "" ? entityVariant.Text + " " : "") + "(" + entities.Count + ")";
            referenceList.EndUpdate();
        }

        private struct EntityRef
        {
            public Entity entity;
            public Composite composite;
        }
    }
}
