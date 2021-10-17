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
        public CathodeEditorGUI_AddParameter(CathodeEntity _node)
        {
            node = _node;
            InitializeComponent();
            param_datatype.SelectedIndex = 0;

            List<string> options = EditorUtils.GenerateParameterList(_node);
            for (int i = 0; i < options.Count; i++) param_name.Items.Add(options[i]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (param_name.Text == "") return;
            cGUID thisParamID = Utilities.GenerateGUID(param_name.Text);

            foreach (CathodeLoadedParameter param in node.parameters)
            {
                if (param.paramID == thisParamID)
                {
                    MessageBox.Show("This parameter already exists on the entity!");
                    return;
                }
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
                    ((CathodeEnum)thisParam).enumID = new cGUID("4C-B9-82-48"); //ALERTNESS_STATE is the first alphabetically
                    break;
                case CathodeDataType.SHORT_GUID:
                    thisParam = new CathodeResource();
                    ((CathodeResource)thisParam).resourceID = new cGUID("00-00-00-00");
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
    }
}
