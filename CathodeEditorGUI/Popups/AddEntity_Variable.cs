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

        public AddEntity_Variable(Composite composite, bool flowgraphMode) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _composite = composite;

            variableEnumType.BeginUpdate();
            variableEnumType.Items.Clear();
            variableEnumType.Items.AddRange(Enum.GetNames(typeof(EnumType)).OrderBy(o => o).ToArray());
            variableEnumType.EndUpdate();
            variableEnumType.SelectedIndex = SettingsManager.GetInteger(Singleton.Settings.PrevVariableType_Enum);

            variableEnumStringType.BeginUpdate();
            variableEnumStringType.Items.Clear();
            variableEnumStringType.Items.AddRange(Enum.GetNames(typeof(EnumStringType)).OrderBy(o => o).ToArray());
            variableEnumStringType.EndUpdate();
            variableEnumStringType.SelectedIndex = SettingsManager.GetInteger(Singleton.Settings.PrevVariableType_EnumString);

            variableType.BeginUpdate();
            variableType.Items.Clear();
            variableType.Items.AddRange(Enum.GetNames(typeof(CompositePinType)).OrderBy(o => o).ToArray());
            variableType.EndUpdate();
            variableType.SelectedIndex = SettingsManager.GetInteger(Singleton.Settings.PrevVariableType);

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

            //TODO: verify the ones that aren't listed here are definitely float

            DataType datatype = DataType.FLOAT;
            ShortGuid enumType = new ShortGuid(0);
            switch ((CompositePinType)Enum.Parse(typeof(CompositePinType), variableType.SelectedItem.ToString()))
            {
                case CompositePinType.CompositeInputBoolVariablePin:
                case CompositePinType.CompositeOutputBoolVariablePin:
                    datatype = DataType.BOOL;
                    break;
                case CompositePinType.CompositeInputDirectionVariablePin:
                case CompositePinType.CompositeOutputDirectionVariablePin:
                    datatype = DataType.VECTOR;
                    break;
                case CompositePinType.CompositeInputEnumVariablePin:
                case CompositePinType.CompositeOutputEnumVariablePin:
                    enumType = ShortGuidUtils.Generate(variableEnumType.SelectedItem.ToString());
                    datatype = DataType.ENUM;
                    break;
                case CompositePinType.CompositeInputEnumStringVariablePin:
                case CompositePinType.CompositeOutputEnumStringVariablePin:
                    enumType = ShortGuidUtils.Generate(variableEnumStringType.SelectedItem.ToString());
                    datatype = DataType.STRING;
                    break;
                case CompositePinType.CompositeInputFloatVariablePin:
                case CompositePinType.CompositeOutputFloatVariablePin:
                    datatype = DataType.FLOAT;
                    break;
                case CompositePinType.CompositeInputIntVariablePin:
                case CompositePinType.CompositeOutputIntVariablePin:
                    datatype = DataType.INTEGER;
                    break;
                case CompositePinType.CompositeInputPositionVariablePin:
                case CompositePinType.CompositeOutputPositionVariablePin:
                    datatype = DataType.TRANSFORM;
                    break;
                case CompositePinType.CompositeInputStringVariablePin:
                case CompositePinType.CompositeOutputStringVariablePin:
                    datatype = DataType.STRING;
                    break;
                case CompositePinType.CompositeInputAnimationInfoVariablePin:
                case CompositePinType.CompositeOutputAnimationInfoVariablePin:
                    //datatype = DataType.ANIMATION_INFO; TODO: need to add a ui for this (?)
                    break;
                case CompositePinType.CompositeInputObjectVariablePin:
                case CompositePinType.CompositeOutputObjectVariablePin:
                    //datatype = DataType.OBJECT; TODO: need to add a ui for this (?)
                    break;
                case CompositePinType.CompositeInputZoneLinkPtrVariablePin:
                case CompositePinType.CompositeOutputZoneLinkPtrVariablePin:
                    //datatype = DataType.ZONE_LINK_PTR; TODO: need to add a ui for this (?)
                    break;
                case CompositePinType.CompositeInputZonePtrVariablePin:
                case CompositePinType.CompositeOutputZonePtrVariablePin:
                    //datatype = DataType.ZONE_PTR; TODO: need to add a ui for this (?)
                    break;
            }

            Singleton.OnEntityAddPending?.Invoke();
            ShortGuid entityID = ShortGuidUtils.GenerateRandom();
            VariableEntity newEntity = _composite.AddVariable(variableName.Text, datatype);
            CompositeUtils.SetParameterInfo(_composite, new CompositePinInfoTable.PinInfo()
            {
                VariableGUID = newEntity.shortGUID,
                PinTypeGUID = ShortGuidUtils.Generate(variableType.SelectedItem.ToString()),
                PinEnumTypeGUID = enumType
            });
            ParameterUtils.AddAllDefaultParameters(newEntity, _composite);
            if (newEntity.parameters[0].content.dataType == DataType.ENUM)
            {
                cEnum enumParam = (cEnum)newEntity.parameters[0].content;
                enumParam.enumID = enumType; //todo: this should be applied above...
                enumParam.enumIndex = EnumUtils.GetEnum(enumType).Entries[0].Index;
            }
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
            CompositePinType type = (CompositePinType)Enum.Parse(typeof(CompositePinType), variableType.SelectedItem.ToString());

            bool isEnum = type == CompositePinType.CompositeInputEnumVariablePin || type == CompositePinType.CompositeOutputEnumVariablePin;
            variableEnumType.Enabled = isEnum;
            bool isEnumString = type == CompositePinType.CompositeInputEnumStringVariablePin || type == CompositePinType.CompositeOutputEnumStringVariablePin;
            variableEnumStringType.Visible = isEnumString;
            variableEnumStringType.Enabled = isEnumString;
            SettingsManager.SetInteger(Singleton.Settings.PrevVariableType, variableType.SelectedIndex);
        }

        private void variableEnumType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.SetInteger(Singleton.Settings.PrevVariableType_Enum, variableEnumType.SelectedIndex);
        }

        private void variableEnumStringType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.SetInteger(Singleton.Settings.PrevVariableType_EnumString, variableEnumStringType.SelectedIndex);
        }
    }
}
