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
    public partial class RemoveParameter : BaseWindow
    {
        private EntityDisplay _entityDisplay;

        public RemoveParameter(EntityDisplay entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, entityDisplay.Content)
        {
            InitializeComponent();

            _entityDisplay = entityDisplay;

            parameterToDelete.BeginUpdate();
            parameterToDelete.Items.Clear();
            for (int i = 0; i < _entityDisplay.Entity.parameters.Count; i++)
            {
                parameterToDelete.Items.Add(ShortGuidUtils.FindString(_entityDisplay.Entity.parameters[i].name));
            }
            for (int i = 0; i < _entityDisplay.Entity.childLinks.Count; i++)
            {
                parameterToDelete.Items.Add("Link out: [" + ShortGuidUtils.FindString(_entityDisplay.Entity.childLinks[i].parentParamID) + "] -> " + 
                    _entityDisplay.Content.editor_utils.GenerateEntityName(_entityDisplay.Composite.GetEntityByID(_entityDisplay.Entity.childLinks[i].childID), _entityDisplay.Composite) + 
                    " [" + ShortGuidUtils.FindString(_entityDisplay.Entity.childLinks[i].childParamID) + "]");
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
            int link_index = parameterToDelete.SelectedIndex - _entityDisplay.Entity.parameters.Count;
            if (link_index >= 0) 
            {
                _entityDisplay.Entity.childLinks.RemoveAt(link_index);
            }
            else
            {
                _entityDisplay.Entity.parameters.RemoveAt(parameterToDelete.SelectedIndex);
            }
            this.Close();
        }
    }
}
