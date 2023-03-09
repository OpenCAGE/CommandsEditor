using CATHODE;
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

namespace CommandsEditor
{
    public partial class RenameEntity : BaseWindow
    {
        public Action<Composite, Entity> OnSaved;
        private Entity _ent;
        private Composite _comp;

        public RenameEntity(Composite comp, Entity entity) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _comp = comp;
            _ent = entity;

            switch (_ent.variant)
            {
                case EntityVariant.VARIABLE:
                    entity_name.Text = ShortGuidUtils.FindString(((VariableEntity)_ent).name);
                    break;
                default:
                    entity_name.Text = EntityUtils.GetName(comp, _ent);
                    break;
            }
        }

        private void save_entity_name_Click(object sender, EventArgs e)
        {
            if (entity_name.Text == "") return;

            switch (_ent.variant)
            {
                case EntityVariant.VARIABLE:
                    ((VariableEntity)_ent).name = ShortGuidUtils.Generate(entity_name.Text);
                    break;
                default:
                    EntityUtils.SetName(_comp, _ent, entity_name.Text);
                    break;
            }

            OnSaved?.Invoke(_comp, _ent);
            this.Close();
        }
    }
}
