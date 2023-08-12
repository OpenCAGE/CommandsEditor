using CommandsEditor.DockPanels;
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

namespace CommandsEditor
{
    public partial class CommandsEditorNew : Form
    {
        public CommandsEditorNew()
        {
            InitializeComponent();

            CompositeDisplay f2 = new CompositeDisplay();
            f2.Show(dockPanel, DockState.DockLeft);
            CompositeContentDisplay f3 = new CompositeContentDisplay();
            f3.Show(dockPanel);
            EntityDisplay f4 = new EntityDisplay();
            f4.Show(dockPanel, DockState.DockRight);
        }
    }
}
