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
        public Action<string> OnLevelSelected;

        public SelectLevel()
        {
            InitializeComponent();
            EditorUtils.PopulateLevelDropdown(env_list);
        }

        private void load_commands_pak_Click(object sender, EventArgs e)
        {
            OnLevelSelected?.Invoke(env_list.SelectedItem.ToString());
            this.Close();
        }
    }
}
