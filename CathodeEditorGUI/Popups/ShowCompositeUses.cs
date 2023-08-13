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

        private CompositeDisplay _compositeDisplay;

        public ShowCompositeUses(CompositeDisplay editor) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor.Content)
        {
            _compositeDisplay = editor;
            InitializeComponent();

            label.Text = "Entities that instance the composite '" + _compositeDisplay.Composite.name + "':";

            referenceList.BeginUpdate();
            foreach (Composite comp in Editor.commands.Entries)
            {
                foreach (FunctionEntity ent in comp.functions.FindAll(o => o.function == _compositeDisplay.Composite.shortGUID))
                {
                    entities.Add(new EntityRef() { composite = comp, entity = ent });
                    referenceList.Items.Add(comp.name + ": " + EntityUtils.GetName(comp.shortGUID, ent.shortGUID));
                }
            }
            referenceList.EndUpdate();
        }

        private void jumpToEntity_Click(object sender, EventArgs e)
        {
            if (referenceList.SelectedIndex == -1) return;
            OnEntitySelected?.Invoke(entities[referenceList.SelectedIndex].composite, entities[referenceList.SelectedIndex].entity);
            this.Close();
        }

        private struct EntityRef
        {
            public Entity entity;
            public Composite composite;
        }
    }
}
