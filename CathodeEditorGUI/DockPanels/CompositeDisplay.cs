using CATHODE.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.DockPanels
{
    public partial class CompositeDisplay : DockContent
    {
        private CommandsDisplay _commandsDisplay;
        public CommandsDisplay CommandsDisplay => _commandsDisplay;
        public LevelContent Content => _commandsDisplay.Content;

        private Composite _composite;
        public Composite Composite => _composite;

        public CompositeDisplay(CommandsDisplay commandsDisplay, Composite composite)
        {
            _commandsDisplay = commandsDisplay;
            _composite = composite;

            InitializeComponent();
        }

        private void createEntity_Click(object sender, EventArgs e)
        {

        }

        private void renameSelected_Click(object sender, EventArgs e)
        {

        }

        private void duplicateSelected_Click(object sender, EventArgs e)
        {

        }

        private void removeSelected_Click(object sender, EventArgs e)
        {

        }
    }
}
