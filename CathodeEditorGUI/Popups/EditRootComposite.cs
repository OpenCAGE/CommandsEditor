using CATHODE.Scripting;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
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
    public partial class EditRootComposite : BaseWindow
    {
        List<Composite> composites = null;

        public EditRootComposite(CommandsDisplay editor) : base(WindowClosesOn.COMMANDS_RELOAD, editor.Content)
        {
            InitializeComponent();
            rootComposite.BeginUpdate();
            rootComposite.Items.Clear();
            rootComposite.SelectedIndex = -1;
            composites = Content.commands.Entries.OrderBy(o => o.name).ToList();
            for (int i = 0; i < composites.Count; i++)
            {
                rootComposite.Items.Add(composites[i].name);
                if (rootComposite.SelectedIndex == -1 && 
                    composites[i].shortGUID == Content.commands.EntryPoints[0].shortGUID)
                {
                    rootComposite.SelectedIndex = i;
                }
            }
            rootComposite.EndUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rootComposite.SelectedIndex == -1) return;
            Content.commands.SetRootComposite(composites[rootComposite.SelectedIndex].shortGUID);
            this.Close();
        }
    }
}
