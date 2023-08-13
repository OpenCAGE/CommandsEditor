using CommandsEditor.DockPanels;
using CommandsEditor.Popups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor
{
    public partial class CommandsEditor : Form
    {
        public DockPanel DockPanel => dockPanel;

        private string layoutConfigFile;

        public CommandsEditor()
        {
            Singleton.Editor = this;

            layoutConfigFile = SharedData.pathToAI + "/DATA/MODTOOLS/CommandsEditor.config";
            InitializeComponent();

            /*
            if (File.Exists(layoutConfigFile))
            {
                dockPanel.LoadFromXml(layoutConfigFile, new DeserializeDockContent(DockContent));
            }
            else
            {
                CommandsDisplay f2 = new CommandsDisplay();
                f2.Show(dockPanel, DockState.DockLeft);
                EntityDisplay f4 = new EntityDisplay();
                f4.Show(dockPanel, DockState.Document);
                EntityDisplay f5 = new EntityDisplay();
                f5.Show(dockPanel, DockState.Document);
                CompositeDisplay f3 = new CompositeDisplay();
                f3.Show(dockPanel, DockState.DockRight);
            }
            */

            statusStrip.Text = "bruh";
        }

        private IDockContent DockContent(string persistString)
        {
            /*
            if (persistString == typeof(CompositeDisplay).ToString())
                return new CompositeDisplay();
            else if (persistString == typeof(CommandsDisplay).ToString())
                return new CommandsDisplay();
            else if (persistString == typeof(EntityDisplay).ToString())
                return new EntityDisplay();
            else*/
                return null;
        }

        private void OnFormClose(object sender, CancelEventArgs e)
        {
            dockPanel.SaveAsXml(layoutConfigFile);
        }

        private void loadLevel_Click(object sender, EventArgs e)
        {
            SelectLevel dialog = new SelectLevel();
            dialog.Show();
        }

        private void saveLevel_Click(object sender, EventArgs e)
        {

        }
    }
}
