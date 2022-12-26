﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Commands;
using CATHODE;
using CathodeLib;

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_StringDataType : UserControl
    {
        CATHODE.Commands.cString stringVal = null;

        public GUI_StringDataType()
        {
            InitializeComponent();
        }

        public void PopulateUI(CATHODE.Commands.cString cString, ShortGuid paramID)
        {
            stringVal = cString;
            label1.Text = ShortGuidUtils.FindString(paramID);
            textBox1.Text = cString.value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            stringVal.value = textBox1.Text;
        }
    }
}
