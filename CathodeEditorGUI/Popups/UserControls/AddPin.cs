using CommandsEditor.Popups.Base;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class AddPin : BaseWindow
    {
        public Action OnSaved;

        protected STNode _node;
        protected Mode _mode;

        public enum Mode
        {
            ADD_IN,
            REMOVE_IN,
            ADD_OUT,
            REMOVE_OUT,
        }

        public AddPin() : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();
        }

        public AddPin(STNode node, Mode mode) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _node = node;
            _mode = mode;
        }
    }
}
