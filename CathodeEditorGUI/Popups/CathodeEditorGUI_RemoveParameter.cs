using CATHODE.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_RemoveParameter : Form
    {
        private CathodeEntity _entity;

        public CathodeEditorGUI_RemoveParameter(CathodeEntity entity)
        {
            InitializeComponent();

            if (entity.parameters.Count == 0)
            {
                this.Close();
                return;
            }
            _entity = entity;

            parameterToDelete.BeginUpdate();
            parameterToDelete.Items.Clear();
            for (int i = 0; i < _entity.parameters.Count; i++)
            {
                parameterToDelete.Items.Add(EntityDBEx.GetParameterName(_entity.parameters[i].shortGUID));
            }
            parameterToDelete.SelectedIndex = 0;
            parameterToDelete.EndUpdate();
        }

        private void delete_param_Click(object sender, EventArgs e)
        {
            if (parameterToDelete.SelectedIndex == -1) return;
            _entity.parameters.RemoveAt(parameterToDelete.SelectedIndex);
            this.Close();
        }
    }
}
