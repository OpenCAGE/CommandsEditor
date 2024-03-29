using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
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
    public partial class ParameterUserControl : UserControl
    {
        public Action<Parameter> OnDeleted;
        public Parameter Parameter = null;

        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        public ParameterUserControl()
        {
            InitializeComponent();
        }

        protected void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnDeleted?.Invoke(Parameter);
        }
    }
}
