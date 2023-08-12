using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
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
using static CATHODE.Scripting.TriggerSequence;

namespace CommandsEditor
{
    public partial class ShowCompositeUses : BaseWindow
    {
        public Action<ShortGuid, Composite> OnEntitySelected;

        private List<EntityRef> entities = new List<EntityRef>();

        public ShowCompositeUses(CommandsEditor editor) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            InitializeComponent();

            label.Text = "Entities that instance the composite '" + Editor.selected.composite.name + "':";

            referenceList.BeginUpdate();
            foreach (Composite comp in Editor.commands.Entries)
            {
                foreach (FunctionEntity ent in comp.functions.FindAll(o => o.function == Editor.selected.composite.shortGUID))
                {
                    entities.Add(new EntityRef() { composite = comp, entity = ent.shortGUID });
                    referenceList.Items.Add(comp.name + ": " + EntityUtils.GetName(comp.shortGUID, ent.shortGUID));
                }
            }
            referenceList.EndUpdate();
        }

        private void jumpToEntity_Click(object sender, EventArgs e)
        {
            if (referenceList.SelectedIndex == -1) return;
            OnEntitySelected?.Invoke(entities[referenceList.SelectedIndex].entity, entities[referenceList.SelectedIndex].composite);
            this.Close();
        }

        private struct EntityRef
        {
            public ShortGuid entity;
            public Composite composite;
        }
    }
}
