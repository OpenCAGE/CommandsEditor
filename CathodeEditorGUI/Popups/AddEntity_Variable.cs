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

        //TODO: We should add a compatibility window to add the additional metadata for already added new pins that people have made.
        //      This should work using the LocalDebug func logic of checking all entries against the DB. We can also know if the composite has been modified by checking the ShortGuid table.

        public AddEntity_Variable(Composite composite, bool flowgraphMode) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _composite = composite;

            variableType.BeginUpdate();
            variableType.Items.Clear();
            variableType.Items.AddRange(new object[] {
                "CompositeReferencePin",
                "CompositeOutputVariablePin", //todo: is this defo a float?
                "CompositeOutputAnimationInfoVariablePin",
                "CompositeOutputBoolVariablePin",
                "CompositeOutputDirectionVariablePin",
                "CompositeOutputEnumVariablePin",
                "CompositeOutputFloatVariablePin",
                "CompositeOutputIntVariablePin",
                "CompositeOutputObjectVariablePin",
                "CompositeOutputPositionVariablePin",
                "CompositeOutputStringVariablePin",
                "CompositeOutputZoneLinkPtrVariablePin",
                "CompositeOutputZonePtrVariablePin",
                "CompositeTargetPin",
                "CompositeInputVariablePin", //todo: is this defo a float?
                "CompositeInputAnimationInfoVariablePin",
                "CompositeInputBoolVariablePin",
                "CompositeInputDirectionVariablePin",
                "CompositeInputEnumVariablePin",
                "CompositeInputFloatVariablePin",
                "CompositeInputIntVariablePin",
                "CompositeInputObjectVariablePin",
                "CompositeInputPositionVariablePin",
                "CompositeInputStringVariablePin",
                "CompositeInputZoneLinkPtrVariablePin",
                "CompositeInputZonePtrVariablePin",
                "CompositeMethodPin"
            });
            variableType.EndUpdate();
            variableType.SelectedIndex = SettingsManager.GetInteger(Singleton.Settings.PrevVariableType);

            variableEnumType.BeginUpdate();
            variableEnumType.Items.Clear();
            variableEnumType.Items.AddRange(Enum.GetNames(typeof(EnumType)));
            variableEnumType.EndUpdate();
            variableEnumType.SelectedIndex = 0;
            variableEnumType.Enabled = false;

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

            DataType datatype = DataType.FLOAT;
            switch (variableType.SelectedItem.ToString())
            {
                case "CompositeInputAnimationInfoVariablePin":
                case "CompositeOutputAnimationInfoVariablePin":
                    datatype = DataType.ANIMATION_INFO; //TODO: need to add a ui for this... not sure what it is, is it the skeleton/anim pair?
                    break;
                case "CompositeInputBoolVariablePin":
                case "CompositeOutputBoolVariablePin":
                    datatype = DataType.BOOL;
                    break;
                case "CompositeInputDirectionVariablePin":
                case "CompositeOutputDirectionVariablePin":
                    datatype = DataType.VECTOR;
                    break;
                case "CompositeInputEnumVariablePin":
                case "CompositeOutputEnumVariablePin":
                    datatype = DataType.ENUM;
                    break;
                case "CompositeInputFloatVariablePin":
                case "CompositeOutputFloatVariablePin":
                    datatype = DataType.FLOAT;
                    break;
                case "CompositeInputIntVariablePin":
                case "CompositeOutputIntVariablePin":
                    datatype = DataType.INTEGER;
                    break;
                case "CompositeInputObjectVariablePin":
                case "CompositeOutputObjectVariablePin":
                    datatype = DataType.OBJECT;
                    break;
                case "CompositeInputPositionVariablePin":
                case "CompositeOutputPositionVariablePin":
                    datatype = DataType.TRANSFORM;
                    break;
                case "CompositeInputStringVariablePin":
                case "CompositeOutputStringVariablePin":
                    datatype = DataType.STRING;
                    break;
                case "CompositeInputZoneLinkPtrVariablePin":
                case "CompositeOutputZoneLinkPtrVariablePin":
                    datatype = DataType.ZONE_LINK_PTR;
                    break;
                case "CompositeInputZonePtrVariablePin":
                case "CompositeOutputZonePtrVariablePin":
                    datatype = DataType.ZONE_PTR;
                    break;
            }

            Singleton.OnEntityAddPending?.Invoke();
            VariableEntity newEntity = _composite.AddVariable(variableName.Text, datatype, true);
            if (newEntity.parameters[0].content.dataType == DataType.ENUM)
            {
                cEnum enumParam = (cEnum)newEntity.parameters[0].content;
                enumParam.enumID = ShortGuidUtils.Generate(variableEnumType.SelectedItem.ToString());
            }
            CompositeUtils.SetParameterInfo(_composite, new CompositePinInfoTable.PinInfo()
            {
                VariableGUID = newEntity.shortGUID,
                PinTypeGUID = ShortGuidUtils.Generate(variableType.SelectedItem.ToString())
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
            variableEnumType.Enabled = variableType.SelectedItem.ToString() == "CompositeInputEnumVariablePin" || variableType.SelectedItem.ToString() == "CompositeOutputEnumVariablePin"; 
            SettingsManager.SetInteger(Singleton.Settings.PrevVariableType, variableType.SelectedIndex);
        }
    }
}
