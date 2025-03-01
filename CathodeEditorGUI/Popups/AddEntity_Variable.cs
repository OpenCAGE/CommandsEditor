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
        //TODO: allow people to specify if this should be a pin in or out when the composite is instanced. should also display this alongside the datatype in the UI (e.g. STRING [IN])
        //TODO: i should also load this information in, and store it in a new table (Which i also add this user-inputted info to)
        //TODO: when i make the node for this entity, i should auto populate a pin either on the left or right. i should also update the logic for all existing variable pins so that if i know if they're in/out they only show a pin on that side.
        //TODO ^ and on this, i should validate this doesn't break any existing links.

        private Composite _composite;

        public AddEntity_Variable(Composite composite, bool flowgraphMode) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _composite = composite;

            //TODO: really we should just add the below from the enum. then filter any we don't want. then cast the string to enum at the end rather using index.

            variableType.BeginUpdate();
            variableType.Items.Clear();
            //These should match DataType
            variableType.Items.AddRange(new object[] {
                                    "STRING",
                                    "FLOAT",
                                    "INTEGER",
                                    "BOOL",
                                    "VECTOR",
                                    "TRANSFORM",
                                    "ENUM",
                                    "SPLINE",
                                    "RESOURCE"
                                    // TODO: we should support other types here
            });
            variableType.EndUpdate();
            variableType.SelectedIndex = SettingsManager.GetInteger(Singleton.Settings.PrevVariableType);

            variableDirection.BeginUpdate();
            variableDirection.Items.Clear();
            //These should match PinDirection
            variableDirection.Items.AddRange(new object[] {
                                    "IN",
                                    "OUT"
            });
            variableDirection.EndUpdate();
            variableDirection.SelectedIndex = 0;

            createNode.Checked = SettingsManager.GetBool(Singleton.Settings.MakeNodeWhenMakeEntity);
            createNode.Visible = flowgraphMode;

            variableName.Select();
        }

        private void createEntity(object sender, EventArgs e)
        {
            if (variableName.Text == "")
            {
                MessageBox.Show("Please enter a name!", "No name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ShortGuid name_guid = ShortGuidUtils.Generate(variableName.Text);
            foreach (VariableEntity varEnt in _composite.variables)
            {
                if (varEnt.name == name_guid)
                {
                    MessageBox.Show("A parameter within this composite already has the same name, please pick another!", "Duplicate name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            Singleton.OnEntityAddPending?.Invoke();
            Entity newEntity = _composite.AddVariable(variableName.Text, (DataType)variableType.SelectedIndex, true);
            CompositeUtils.SetParameterInfo(_composite, new CompositePinInfoTable.PinInfo()
            {
                VariableGUID = newEntity.shortGUID,
                Direction = (PinDirection)variableDirection.SelectedIndex
            });
            Singleton.OnEntityAdded?.Invoke(newEntity);

            this.Close();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                createVariable.PerformClick();
        }

        private void createNode_CheckedChanged(object sender, EventArgs e)
        {
            if (createNode.Checked != SettingsManager.GetBool(Singleton.Settings.MakeNodeWhenMakeEntity))
                Singleton.Editor.ToggleMakeNodeWhenMakeEntity();
        }

        private void entityVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.SetInteger(Singleton.Settings.PrevVariableType, variableType.SelectedIndex);
        }
    }
}
