using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Commands;
using CathodeLib;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddParameter : Form
    {
        CathodeEntity node = null;
        bool loadedParamsFromDB = false;
        public CathodeEditorGUI_AddParameter(CathodeEntity _node)
        {
            node = _node;
            InitializeComponent();
            param_datatype.SelectedIndex = 0;

            List<string> options = EditorUtils.GenerateParameterList(_node, out loadedParamsFromDB);
            param_name.BeginUpdate();
            for (int i = 0; i < options.Count; i++) param_name.Items.Add(options[i]);
            param_name.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (param_name.Text == "") return;
            ShortGuid thisParamID = ShortGuidUtils.Generate(param_name.Text);

            foreach (CathodeLoadedParameter param in node.parameters)
            {
                if (param.shortGUID == thisParamID)
                {
                    MessageBox.Show("This parameter already exists on the entity!");
                    return;
                }
            }

            //TODO: when we have custom ShortGuid saving, this can be deprecated.
            if (ShortGuidUtils.FindString(thisParamID) != param_name.Text)
            {
                MessageBox.Show("This parameter name is not supported by the Cathode scripting system!");
                return;
            }

            //TODO: Remove this when resource adding is supported
            if ((CathodeDataType)param_datatype.SelectedIndex == CathodeDataType.RESOURCE)
            {
                MessageBox.Show("Adding resources is currently unsupported and will cause issues.\nThis functionality will be added in an upcoming OpenCAGE release.", "Coming soon...", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CathodeParameter thisParam = null;
            switch ((CathodeDataType)param_datatype.SelectedIndex)
            {
                case CathodeDataType.POSITION:
                    thisParam = new CathodeTransform();
                    break;
                case CathodeDataType.FLOAT:
                    thisParam = new CathodeFloat();
                    break;
                case CathodeDataType.FILEPATH:
                case CathodeDataType.STRING:
                    thisParam = new CathodeString();
                    break;
                case CathodeDataType.SPLINE_DATA:
                    thisParam = new CathodeSpline();
                    break;
                case CathodeDataType.ENUM:
                    thisParam = new CathodeEnum();
                    ((CathodeEnum)thisParam).enumID = new ShortGuid("4C-B9-82-48"); //ALERTNESS_STATE is the first alphabetically
                    break;
                case CathodeDataType.RESOURCE:
                    thisParam = new CathodeResource();
                    ((CathodeResource)thisParam).resourceID = new ShortGuid("00-00-00-00"); //TODO: This will cause issues
                    break;
                case CathodeDataType.BOOL:
                    thisParam = new CathodeBool();
                    break;
                case CathodeDataType.DIRECTION:
                    thisParam = new CathodeVector3();
                    break;
                case CathodeDataType.INTEGER:
                    thisParam = new CathodeInteger();
                    break;
            }
            node.parameters.Add(new CathodeLoadedParameter(thisParamID, thisParam));

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
                    if (!loadedParamsFromDB) return;
                    CathodeEntityDatabase.ParameterDefinition def = CathodeEntityDatabase.GetParameterFromEntity(((FunctionEntity)node).function, param_name.Text);
                    if (def.name == null) return;
                    if (def.usage == CathodeEntityDatabase.ParameterUsage.TARGET)
                    {
                        //"TARGET" usage type does not have a datatype since it is not data, it's an event trigger.
                        //The FLOAT datatype is a placeholder for this.
                        param_datatype.Text = "FLOAT";
                    }
                    else
                    {
                        CathodeParameter param = CathodeEntityDatabase.ParameterDefinitionToParameter(def);
                        if (param == null) return;
                        param_datatype.Text = param.dataType.ToString();
                    }
                    param_datatype.Enabled = false;
                    break;
                default:
                    return;
            }
        }
    }
}
