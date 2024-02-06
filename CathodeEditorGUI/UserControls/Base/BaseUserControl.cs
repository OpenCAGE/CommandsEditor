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
        protected LevelContent _content;
        protected LevelContent Content => _content;

        [Obsolete("Designer only", true)]
        public BaseUserControl()
        {
            InitializeComponent();

            this.Disposed += BaseUserControl_Disposed;
        }

        private void BaseUserControl_Disposed(object sender, EventArgs e)
        {
            _content = null;
        }

        public BaseUserControl(LevelContent editor)
        {
            InitializeComponent();

            _content = editor;
        }

        public void SetEditor(LevelContent editor)
        {
            _content = editor;
        }
    }
}
