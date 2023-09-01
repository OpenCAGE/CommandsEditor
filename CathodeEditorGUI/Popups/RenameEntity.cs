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
        public Action<string> OnRenamed;

        private EntityDisplay _display;

        public RenameEntity(EntityDisplay editor) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor.Content)
        {
            InitializeComponent();

            _display = editor;

            switch (_display.Entity.variant)
            {
                case EntityVariant.VARIABLE:
                    entity_name.Text = ShortGuidUtils.FindString(((VariableEntity)_display.Entity).name);
                    break;
                default:
                    entity_name.Text = EntityUtils.GetName(_display.Composite, _display.Entity);
                    break;
            }
        }

        private void save_entity_name_Click(object sender, EventArgs e)
        {
            if (entity_name.Text == "") return;

            switch (_display.Entity.variant)
            {
                case EntityVariant.VARIABLE:
                    ((VariableEntity)_display.Entity).name = ShortGuidUtils.Generate(entity_name.Text);
                    break;
                default:
                    EntityUtils.SetName(_display.Composite, _display.Entity, entity_name.Text);
                    break;
            }

            OnRenamed?.Invoke(entity_name.Text);
            this.Close();
        }
    }
}
