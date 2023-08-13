using System;
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
        protected LevelContent _editor;
        protected LevelContent Editor => _editor;

        [Obsolete("Designer only", true)]
        public BaseUserControl()
        {
            InitializeComponent();
        }

        public BaseUserControl(LevelContent editor)
        {
            InitializeComponent();

            _editor = editor;
        }

        public void SetEditor(LevelContent editor)
        {
            _editor = editor;
        }
    }
}
