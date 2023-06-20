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
using CathodeLib;
using CATHODE;
using CATHODE.LEGACY;
using System.Numerics;
using static CATHODE.Models.CS2.Component;

namespace CommandsEditor.Popups.UserControls
{
    public partial class GUI_Resource_Default : ResourceUserControl
    {
        [Obsolete("Designer only", true)]
        public GUI_Resource_Default() : base(null)
        {
            InitializeComponent();
        }

        public GUI_Resource_Default(CommandsEditor editor) : base(editor)
        {
            InitializeComponent();
        }

        public void PopulateUI(ResourceType type)
        {
            groupBox1.Text = type.ToString();
        }
    }
}