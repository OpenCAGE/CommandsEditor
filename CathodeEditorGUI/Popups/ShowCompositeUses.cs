using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;

namespace CommandsEditor
{
    public partial class ShowCompositeUses : BaseWindow
    {
        public Action<Composite, Entity> OnEntitySelected;

        private List<EntityRef> entities = new List<EntityRef>();

        public ShowCompositeUses(LevelContent editor, Composite composite = null) : base(composite == null ? WindowClosesOn.COMMANDS_RELOAD : WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            InitializeComponent();

            if (composite != null)
            {
                Text = "Prefab Uses";
                label.Text = "Entities that instance the prefab '" + composite.name + "':";
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
            foreach (Composite comp in Content.commands.Entries)
            {
                foreach (FunctionEntity ent in comp.functions.FindAll(o => o.function == guid))
                {
                    entities.Add(new EntityRef() { composite = comp, entity = ent });
                    referenceList.Items.Add(comp.name + ": " + EntityUtils.GetName(comp.shortGUID, ent.shortGUID));
                }
            }
            referenceList.EndUpdate();
        }

        private struct EntityRef
        {
            public Entity entity;
            public Composite composite;
        }
    }
}
