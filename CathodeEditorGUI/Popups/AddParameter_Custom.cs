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
    public partial class AddParameter_Custom : AddParameter
    {
        EntityInspector _entityDisplay;
        ParameterCreator _creator;

        public AddParameter_Custom(EntityInspector entityDisplay)
        {
            _entityDisplay = entityDisplay;
            _creator = new ParameterCreator(entityDisplay.Entity, entityDisplay.Composite);

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
            
            _entityDisplay.Entity.AddParameter(param_name.Text, (DataType)param_datatype.SelectedIndex);

            OnSaved?.Invoke();
            this.Close();
        }

        private void param_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            param_datatype.Text = _creator.GetInfo(param_name.Text);
        }
        private void param_name_TextChanged(object sender, EventArgs e)
        {
            param_datatype.Text = _creator.GetInfo(param_name.Text);
        }
    }
}
