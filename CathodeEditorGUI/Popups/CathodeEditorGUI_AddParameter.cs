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

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddParameter : Form
    {
        Entity node = null;
        public CathodeEditorGUI_AddParameter(Entity _node)
        {
            node = _node;
            InitializeComponent();
            param_datatype.SelectedIndex = 0;

            List<string> options = EditorUtils.GenerateParameterList(_node);
            param_name.BeginUpdate();
            for (int i = 0; i < options.Count; i++) param_name.Items.Add(options[i]);
            param_name.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (param_name.Text == "") return;

            ParameterData thisParam = null;
            switch ((DataType)param_datatype.SelectedIndex)
            {
                case DataType.STRING:
                    thisParam = new cString("");
                    break;
                case DataType.FLOAT:
                    thisParam = new cFloat(0.0f);
                    break;
                case DataType.INTEGER:
                    thisParam = new cInteger(0);
                    break;
                case DataType.BOOL:
                    thisParam = new cBool(true);
                    break;
                case DataType.VECTOR:
                    thisParam = new cVector3(new Vector3(0, 0, 0));
                    break;
                case DataType.TRANSFORM:
                    thisParam = new cTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
                    break;
                case DataType.ENUM:
                    thisParam = new cEnum(EnumType.ALERTNESS_STATE, 0); 
                    break;
                case DataType.SPLINE:
                    thisParam = new cSpline();
                    break;
            }
            node.AddParameter(param_name.Text, thisParam);

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
                    FunctionEntity ent = ((FunctionEntity)node);
                    if (CommandsUtils.FunctionTypeExists(ent.function))
                    {
                        //Function entity
                        CathodeEntityDatabase.ParameterDefinition def = CathodeEntityDatabase.GetParameterFromEntity(ent.function, param_name.Text);
                        if (def.name == null) return;
                        if (def.usage == CathodeEntityDatabase.ParameterUsage.TARGET)
                        {
                            //"TARGET" usage type does not have a datatype since it is not data, it's an event trigger.
                            //The FLOAT datatype is a placeholder for this.
                            param_datatype.Text = "FLOAT";
                        }
                        else
                        {
                            ParameterData param = CathodeEntityDatabase.ParameterDefinitionToParameter(def);
                            if (param == null) return;
                            param_datatype.Text = param.dataType.ToString();
                        }
                    }
                    else
                    {
                        //Composite link
                        ShortGuid param = ShortGuidUtils.Generate(param_name.Text);
                        VariableEntity var = Editor.commands.GetComposite(ent.function).variables.FirstOrDefault(o => o.parameter == param);
                        if (var == null) return;
                        if (var.type == DataType.NONE)
                        {
                            param_datatype.Text = "FLOAT";
                        }
                        else
                        {
                            param_datatype.Text = var.type.ToString();
                        }
                    }
                    param_datatype.Enabled = false;
                    break;
                default:
                    return;
            }
        }
    }
}
