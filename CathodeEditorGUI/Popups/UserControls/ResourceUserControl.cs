﻿using CATHODE.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI.Popups.UserControls
{
    public partial class ResourceUserControl : UserControl
    {
        public CathodeResourceReference ResourceReference;
        public bool IsFromParams = false;

        public ResourceUserControl()
        {
            InitializeComponent();
        }
    }
}