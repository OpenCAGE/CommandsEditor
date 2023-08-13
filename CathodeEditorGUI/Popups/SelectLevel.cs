using CATHODE.Scripting;
using CathodeLib;
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

namespace CommandsEditor.Popups
{
    public partial class SelectLevel : Form
    {
        public SelectLevel()
        {
            InitializeComponent();

            env_list.BeginUpdate();
            env_list.Items.AddRange(Level.GetLevels(SharedData.pathToAI, true).ToArray());
            env_list.EndUpdate();

            if (env_list.Items.Contains("FRONTEND")) 
                env_list.SelectedItem = "FRONTEND";
            else 
                env_list.SelectedIndex = 0;
        }

        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            CommandsDisplay panel = new CommandsDisplay(env_list.SelectedItem.ToString());
            panel.Show(Singleton.Editor.DockPanel, DockState.DockLeft);

            this.Close();
        }
    }
}
