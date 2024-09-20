using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using CommandsEditor.Popups.UserControls;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class AddEntity_Variable : BaseWindow
    {
        private Composite _composite;

        public AddEntity_Variable(Composite composite) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _composite = composite;

            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            entityVariant.Items.AddRange(new object[] {
                                    "STRING",
                                    "FLOAT",
                                    "INTEGER",
                                    "BOOL",
                                    "VECTOR",
                                    "TRANSFORM",
                                    "ENUM",
                                    "SPLINE",
                                    "RESOURCE"
                                    // TODO: we should support other types here, such as ZONE_LINK_PTR used in doors
            });
            entityVariant.EndUpdate();

            entityVariant.SelectedIndex = 0;

            textBox1.Select();
        }

        private void createEntity(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter a name!", "No name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Singleton.OnEntityAddPending?.Invoke();
            Entity newEntity = _composite.AddVariable(textBox1.Text, (DataType)entityVariant.SelectedIndex, true);
            Singleton.OnEntityAdded?.Invoke(newEntity);

            this.Close();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                createNewEntity.PerformClick();
        }
    }
}
