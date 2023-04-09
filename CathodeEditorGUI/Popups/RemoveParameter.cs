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
    public partial class RemoveParameter : BaseWindow
    {
        private Entity _entity;

        public RemoveParameter(CommandsEditor editor, Entity entity) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            InitializeComponent();

            _entity = entity;

            parameterToDelete.BeginUpdate();
            parameterToDelete.Items.Clear();
            for (int i = 0; i < _entity.parameters.Count; i++)
            {
                parameterToDelete.Items.Add(ShortGuidUtils.FindString(_entity.parameters[i].name));
            }
            for (int i = 0; i < _entity.childLinks.Count; i++)
            {
                parameterToDelete.Items.Add("Link out: [" + ShortGuidUtils.FindString(_entity.childLinks[i].parentParamID) + "] -> " + 
                    EditorUtils.GenerateEntityName(Editor.selected.composite.GetEntityByID(_entity.childLinks[i].childID), Editor.selected.composite) + 
                    " [" + ShortGuidUtils.FindString(_entity.childLinks[i].childParamID) + "]");
            }

            if (parameterToDelete.Items.Count == 0)
            {
                this.Close();
                return;
            }

            parameterToDelete.SelectedIndex = 0;
            parameterToDelete.EndUpdate();
        }

        private void delete_param_Click(object sender, EventArgs e)
        {
            if (parameterToDelete.SelectedIndex == -1) return;
            int link_index = parameterToDelete.SelectedIndex - _entity.parameters.Count;
            if (link_index >= 0) 
            {
                _entity.childLinks.RemoveAt(link_index);
            }
            else
            {
                _entity.parameters.RemoveAt(parameterToDelete.SelectedIndex);
            }
            this.Close();
        }
    }
}
