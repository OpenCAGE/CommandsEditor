﻿using System;
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
using CommandsEditor.Popups.Base;

namespace CommandsEditor
{
    public partial class AddParameter : BaseWindow
    {
        Entity node = null;
        public AddParameter(CommandsEditor editor, Entity _node) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            node = _node;
            InitializeComponent();
            param_datatype.SelectedIndex = 0;

            List<string> options = EditorUtils.GenerateParameterList(_node);
            param_name.BeginUpdate();
            for (int i = 0; i < options.Count; i++) param_name.Items.Add(options[i]);
            param_name.EndUpdate();

            param_name.AutoSelectOff();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (param_name.Text == "") return;
            node.AddParameter(param_name.Text, (DataType)param_datatype.SelectedIndex);
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
            param_datatype.Enabled = true;
            switch (node.variant)
            {
                case EntityVariant.FUNCTION:
                    FunctionEntity ent = (FunctionEntity)node;
                    bool isComposite = !CommandsUtils.FunctionTypeExists(ent.function);
                    ShortGuid function = (isComposite) ? CommandsUtils.GetFunctionTypeGUID(FunctionType.CompositeInterface) : ent.function;

                    CathodeEntityDatabase.ParameterDefinition def = CathodeEntityDatabase.GetParameterFromEntity(function, param_name.Text);
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
                                ParameterData param = CathodeEntityDatabase.ParameterDefinitionToParameter(def);
                                if (param == null) return;
                                param_datatype.Text = param.dataType.ToString();
                                break;
                        }
                        param_datatype.Enabled = false;
                        return;
                    }
                    
                    //if we're a composite & didn't find the param from CompositeInterface, try check the actual composite
                    if (isComposite)
                    {
                        ShortGuid param = ShortGuidUtils.Generate(param_name.Text);
                        VariableEntity var = Editor.commands.GetComposite(ent.function).variables.FirstOrDefault(o => o.name == param);
                        if (var == null) return;
                        if (var.type == DataType.NONE)
                        {
                            param_datatype.Text = "FLOAT";
                        }
                        else
                        {
                            param_datatype.Text = var.type.ToString();
                        }
                        param_datatype.Enabled = false;
                    }
                    break;
                default:
                    return;
            }
        }
    }
}
