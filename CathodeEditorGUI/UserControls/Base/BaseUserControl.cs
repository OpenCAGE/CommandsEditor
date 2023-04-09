﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor.UserControls
{
    public partial class BaseUserControl : UserControl
    {
        protected CommandsEditor _editor;
        protected Editor Editor { get { return _editor.Loaded; } } //hotfix for old Editor. static

        [Obsolete("Designer only", true)]
        public BaseUserControl()
        {
            InitializeComponent();
        }

        public BaseUserControl(CommandsEditor editor)
        {
            InitializeComponent();

            _editor = editor;
        }
    }
}
