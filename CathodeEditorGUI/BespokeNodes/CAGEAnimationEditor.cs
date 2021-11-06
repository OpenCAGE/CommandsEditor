using CATHODE;
using CATHODE.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CAGEAnimationEditor : Form
    {
        CAGEAnimation animNode = null;
        public CAGEAnimationEditor(CAGEAnimation _node)
        {
            animNode = _node;
            InitializeComponent();

            animNode.parameterNames = animNode.parameterNames.OrderBy(o => o.paramGroupName).ToList();
            string previousGroup = "";
            int groupCount = 0;
            GroupBox currentGroupBox = null;
            for (int i = 0; i < animNode.parameterNames.Count; i++)
            {
                string paramGroupName = NodeDBEx.GetParameterName(animNode.parameterNames[i].paramGroupName);
                if (i == 0 || previousGroup != paramGroupName)
                {
                    currentGroupBox = new GroupBox();
                    currentGroupBox.Text = paramGroupName;
                    currentGroupBox.Size = new Size(1101, 338);
                    currentGroupBox.Location = new Point(3, 3 + (344 * groupCount));
                    NodeParams.Controls.Add(currentGroupBox);
                    groupCount++;
                }
                previousGroup = paramGroupName;

                TextBox paramName = new TextBox();
                paramName.Text = NodeDBEx.GetParameterName(animNode.parameterNames[i].paramName);
                paramName.ReadOnly = true;
                paramName.Location = new Point(6, 19 + (i * 23));
                paramName.Size = new Size(119, 20);
                currentGroupBox.Controls.Add(paramName);

                TEMP_CAGEAnimationExtraDataHolder2 paramData = animNode.parameterValues.FirstOrDefault(o => o.ID == animNode.parameterNames[i].paramValueID);
                //TODO: populate full min max keyframes so new ones can be created
                for (int x = 0; x < paramData.keyframes.Count; x++)
                {
                    Button keyframeBtn = new Button();
                    keyframeBtn.Size = new Size(27, 23);
                    keyframeBtn.Location = new Point(134 + ((keyframeBtn.Size.Width + 6) * x), 18 + (i * 23));
                    keyframeBtn.Text = paramData.keyframes[x].secondsSinceStart.ToString();
                    keyframeBtn.AccessibleDescription = paramData.ID.ToString() + " " + x;
                    keyframeBtn.Click += KeyframeBtn_Click;
                    currentGroupBox.Controls.Add(keyframeBtn);
                }
            }
        }

        TEMP_CAGEAnimationExtraDataHolder2_1 currentEditData = null;
        private void KeyframeBtn_Click(object sender, EventArgs e)
        {
            string info = ((Button)sender).AccessibleDescription;
            cGUID id = new cGUID(info.Split(' ')[0]);
            currentEditData = animNode.parameterValues.FirstOrDefault(o => o.ID == id).keyframes[Convert.ToInt32(info.Split(' ')[1])];
            textBox2.Text = currentEditData.paramValue.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (currentEditData == null) return;
            currentEditData.paramValue = Convert.ToSingle(textBox2.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = EditorUtils.ForceStringNumeric(textBox2.Text, true);
        }
    }

    public class CAGEAnimationKeyframes
    {

    }
}
