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
    public partial class AddFolder : BaseWindow
    {
        public Action<Composite> OnFolderAdded;

        CommandsDisplay _commands;
        string _folder;

        public AddFolder(CommandsDisplay editor, string folderPath) : base(WindowClosesOn.COMMANDS_RELOAD, editor.Content)
        {
            _commands = editor;
            InitializeComponent();

            this.Text = "Create Folder Within '" + folderPath + "'";
            _folder = folderPath;
        }

        private void create_param_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;

            string path = _folder + "/" + textBox1.Text.Replace("\\", "/");

            string[] pathParts = path.Split('/');
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (pathParts[i] == "")
                {
                    MessageBox.Show("Failed to create folder: a part of the path is blank.\nRemove trailing slashes and use complete folder names, e.g.:\nSOME/FILE/PATH/TO", "Folder path invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            path += "/";

            for (int i = 0; i < _commands.Content.commands.Entries.Count; i++)
            {
                string[] thisPathParts = _commands.Content.commands.Entries[i].name.Replace("\\", "/").Split('/');
                if (thisPathParts.Length != pathParts.Length) continue;

                bool isMatch = true;
                for (int x = 0; x < thisPathParts.Length; x++)
                {
                    if (thisPathParts[x] != pathParts[x])
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    MessageBox.Show("Failed to create folder.\nA folder with this name already exists.", "Folder already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Composite comp = _commands.Content.commands.AddComposite(path);
            OnFolderAdded?.Invoke(comp);
            this.Close();
        }
    }
}
