using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
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
        public Action<string, Entity> OnRenamed;

        private Entity _entity;
        private Composite _composite;

        public RenameEntity(LevelContent editor, Entity entity, Composite composite) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            InitializeComponent();

            _entity = entity;
            _composite = composite;

            switch (_entity.variant)
            {
                case EntityVariant.VARIABLE:
                    entity_name.Text = ShortGuidUtils.FindString(((VariableEntity)_entity).name);
                    break;
                default:
                    entity_name.Text = EntityUtils.GetName(_composite, _entity);
                    break;
            }
        }

        private void save_entity_name_Click(object sender, EventArgs e)
        {
            if (entity_name.Text == "") return;

            switch (_entity.variant)
            {
                case EntityVariant.VARIABLE:
                    ((VariableEntity)_entity).name = ShortGuidUtils.Generate(entity_name.Text);
                    break;
                default:
                    EntityUtils.SetName(_composite, _entity, entity_name.Text);
                    break;
            }

            OnRenamed?.Invoke(entity_name.Text, _entity);
            this.Close();
        }
    }
}
