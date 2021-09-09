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

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddParameter : Form
    {
        CathodeEntity node = null;
        public CathodeEditorGUI_AddParameter(CathodeEntity _node)
        {
            node = _node;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cGUID thisParamID = Utilities.GenerateGUID(textBox1.Text);

            foreach (CathodeLoadedParameter param in node.parameters)
            {
                if (param.paramID == thisParamID)
                {
                    MessageBox.Show("This parameter already exists on the entity!");
                    return;
                }
            }

            CathodeParameter thisParam = null;
            switch ((CathodeDataType)comboBox1.SelectedIndex)
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
