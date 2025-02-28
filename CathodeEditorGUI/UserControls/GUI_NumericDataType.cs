﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Scripting;
using CATHODE;
using CathodeLib;

namespace CommandsEditor.UserControls
{
    public partial class GUI_NumericDataType : ParameterUserControl
    {
        cFloat floatVal = null;
        cInteger intVal = null;
        bool isIntInput = false;

        public GUI_NumericDataType()
        {
            InitializeComponent();
            this.ContextMenuStrip = contextMenuStrip1;
            this.deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
        }

        public void PopulateUI_Float(cFloat cFloat, string paramID)
        {
            floatVal = cFloat;
            label1.Text = paramID;
            textBox1.Text = cFloat.value.ToString();
            this.deleteToolStripMenuItem.Text = "Delete '" + paramID + "'";

            _hasDoneSetup = true;
        }

        public void PopulateUI_Int(cInteger cInt, string paramID)
        {
            isIntInput = true;
            intVal = cInt;
            label1.Text = paramID;
            textBox1.Text = cInt.value.ToString();
            this.deleteToolStripMenuItem.Text = "Delete '" + paramID + "'";

            _hasDoneSetup = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = EditorUtils.ForceStringNumeric(textBox1.Text, !isIntInput);
            if (isIntInput)
            {
                try
                {
                    intVal.value = Convert.ToInt32(textBox1.Text);
                }
                catch (OverflowException)
                {
                    if (textBox1.Text.StartsWith("-"))
                        intVal.value = Int32.MinValue;
                    else
                        intVal.value = Int32.MaxValue;
                    textBox1.Text = intVal.value.ToString();
                }
            }
            else
            {
                try
                {
                    floatVal.value = Convert.ToSingle(textBox1.Text);
                }
                catch (OverflowException)
                {
                    if (textBox1.Text.StartsWith("-"))
                        floatVal.value = float.MinValue;
                    else
                        floatVal.value = float.MaxValue;
                    textBox1.Text = floatVal.value.ToString();
                }
            }
            HighlightAsModified();
        }

        public override void HighlightAsModified(bool updateDatabase = true, Control fontToUpdate = null)
        {
            base.HighlightAsModified(updateDatabase, label1);
        }
    }
}
