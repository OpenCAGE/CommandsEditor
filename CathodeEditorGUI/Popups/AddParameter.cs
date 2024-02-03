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
    public partial class AddParameter : BaseWindow
    {
        public Action OnSaved;

        FunctionEntity _funcEnt = null; //NOTE: This is not necessarily the entity we want to apply the parameters to - it's instead the "truth" that we should get the parameter list and datatypes from
        ParameterData _param = null;

        EntityDisplay _entityDisplay;

        public AddParameter(EntityDisplay entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, entityDisplay.Content)
        {
            _entityDisplay = entityDisplay;
            InitializeComponent();
            param_datatype.SelectedIndex = 0;

            List<string> options = entityDisplay.Content.editor_utils.GenerateParameterList(entityDisplay.Entity, entityDisplay.Composite);
            param_name.BeginUpdate();
            for (int i = 0; i < options.Count; i++) param_name.Items.Add(options[i]);
            param_name.EndUpdate();

            param_name.AutoSelectOff();
            param_name.Select();

            switch (_entityDisplay.Entity.variant)
            {
                case EntityVariant.PROXY:
                    Entity proxiedEntity = CommandsUtils.ResolveHierarchy(_content.commands, entityDisplay.Composite, ((ProxyEntity)entityDisplay.Entity).proxy.path, out Composite c, out string h);
                    if (proxiedEntity.variant == EntityVariant.FUNCTION)
                        _funcEnt = (FunctionEntity)proxiedEntity;
                    break;
                case EntityVariant.ALIAS:
                    Entity aliasedEntity = CommandsUtils.ResolveHierarchy(_content.commands, entityDisplay.Composite, ((AliasEntity)entityDisplay.Entity).alias.path, out Composite c2, out string h2);
                    if (aliasedEntity.variant == EntityVariant.FUNCTION)
                        _funcEnt = (FunctionEntity)aliasedEntity;
                    break;
                case EntityVariant.FUNCTION:
                    _funcEnt = (FunctionEntity)_entityDisplay.Entity;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (param_name.Text == "") return;
            if (_param != null) _entityDisplay.Entity.AddParameter(param_name.Text, _param);
            else _entityDisplay.Entity.AddParameter(param_name.Text, (DataType)param_datatype.SelectedIndex);

            OnSaved?.Invoke();
            this.Close();
        }

        private void param_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            AutoSelectDataType();
        }
        private void param_name_TextChanged(object sender, EventArgs e)
        {
            AutoSelectDataType();
        }
        private void AutoSelectDataType()
        {
            _param = null;
            param_datatype.Enabled = true;

            if (_funcEnt == null)
                return;

            bool isComposite = !CommandsUtils.FunctionTypeExists(_funcEnt.function);
            ShortGuid function = (isComposite) ? CommandsUtils.GetFunctionTypeGUID(FunctionType.CompositeInterface) : _funcEnt.function;

            CathodeEntityDatabase.ParameterDefinition def = CathodeEntityDatabase.GetParameterFromEntity(function, param_name.Text);

            //Fallback for Proxied entities: also check ProxyInterface
            if (def.name == null && _entityDisplay.Entity.variant == EntityVariant.PROXY)
                def = CathodeEntityDatabase.GetParameterFromEntity(ShortGuidUtils.Generate("ProxyInterface"), param_name.Text);

            if (def.name != null)
            {
                switch (def.usage)
                {
                    case CathodeEntityDatabase.ParameterUsage.REFERENCE:
                    case CathodeEntityDatabase.ParameterUsage.TARGET:
                    case CathodeEntityDatabase.ParameterUsage.METHOD:
                    case CathodeEntityDatabase.ParameterUsage.FINISHED:
                    case CathodeEntityDatabase.ParameterUsage.RELAY:
                        param_datatype.Text = "FLOAT"; //The FLOAT datatype is used a placeholder for this.
                        break;
                    default:
                        _param = CathodeEntityDatabase.ParameterDefinitionToParameter(def);
                        if (_param == null) return;
                        param_datatype.Text = _param.dataType.ToString();
                        break;
                }
                //param_datatype.Enabled = false;
                return;
            }
                    
            //if we're a composite & didn't find the param from CompositeInterface, try check the actual composite
            if (isComposite)
            {
                ShortGuid param = ShortGuidUtils.Generate(param_name.Text);
                VariableEntity var = Content.commands.GetComposite(_funcEnt.function).variables.FirstOrDefault(o => o.name == param);
                if (var == null) return;
                if (var.type == DataType.NONE)
                {
                    param_datatype.Text = "FLOAT";
                }
                else
                {
                    param_datatype.Text = var.type.ToString();
                }
                //param_datatype.Enabled = false;
            }
        }

        private void param_datatype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_param != null && _param.dataType.ToString() != param_datatype.Text)
                _param = null;
        }
    }
}
