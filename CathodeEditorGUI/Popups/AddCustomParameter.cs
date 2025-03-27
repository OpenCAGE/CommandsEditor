using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;

namespace CommandsEditor
{
    public partial class AddCustomParameter : BaseWindow
    {
        public Action<string, DataType> OnSelected;
        
        EntityInspector _entityDisplay;

        public AddCustomParameter(EntityInspector entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            _entityDisplay = entityDisplay;

            InitializeComponent();
            param_datatype.SelectedIndex = 0;

            List<string> options = entityDisplay.Content.editor_utils.GenerateParameterListAsString(entityDisplay.Entity, entityDisplay.Composite);
            param_name.BeginUpdate();
            for (int i = 0; i < options.Count; i++) param_name.Items.Add(options[i]);
            param_name.EndUpdate();

            param_name.AutoSelectOff();
            param_name.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (param_name.Text == "") 
                return;

            OnSelected?.Invoke(param_name.Text, (DataType)param_datatype.SelectedIndex);
            this.Close();
        }

        private void param_name_TextChanged(object sender, EventArgs e)
        {
            param_name_SelectedIndexChanged(null, null);
        }
        private void param_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            (ParameterVariant?, DataType?, ShortGuid) metadata = ParameterUtils.GetParameterMetadata(_entityDisplay.Entity, param_name.Text);
            if (metadata.Item2 != null)
                param_datatype.Text = metadata.Item2.ToString();
        }
    }
}
