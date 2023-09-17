using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using CATHODE;
using CATHODE.Scripting;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;

namespace CommandsEditor
{
    public partial class AddComposite : BaseWindow
    {
        public Action<Composite> OnCompositeAdded;

        CommandsDisplay _commands;
        string _folder;

        public AddComposite(CommandsDisplay editor, string folderPath) : base(WindowClosesOn.COMMANDS_RELOAD, editor.Content)
        {
            _commands = editor;
            InitializeComponent();

            _folder = folderPath;
            this.Text = _folder == "" ? "Create New Composite" : "Create New Composite In Folder '" + _folder + "'";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;

            string path = (_folder == "" ? _folder : _folder + "/") + textBox1.Text.Replace("\\", "/");

            string[] pathParts = path.Split('/');
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (pathParts[i] == "")
                {
                    MessageBox.Show("Failed to create composite: a part of the path is blank.\nRemove trailing slashes and use complete folder names, e.g.:\nSOME/FILE/PATH/TO/COMPOSITE", "Composite path/name invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            for (int i = 0; i < _commands.Content.commands.Entries.Count; i++)
            {
                if (_commands.Content.commands.Entries[i].name.Replace("\\", "/") == path)
                {
                    MessageBox.Show("Failed to create composite.\nA composite with this name already exists.", "Composite already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Composite comp = _commands.Content.commands.AddComposite(path);
            OnCompositeAdded?.Invoke(comp);
            this.Close();
        }
    }
}
