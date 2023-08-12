using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Scripting;
using CathodeLib;
using CommandsEditor.Popups.Base;

namespace CommandsEditor
{
    public partial class AddComposite : BaseWindow
    {
        public AddComposite(CommandsEditor editor) : base(WindowClosesOn.COMMANDS_RELOAD, editor)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;

            string[] pathParts = textBox1.Text.Replace("\\", "/").Split('/');
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (pathParts[i] == "")
                {
                    MessageBox.Show("Failed to create composite: a part of the path is blank.\nRemove trailing slashes and use complete folder names, e.g.:\nSOME/FILE/PATH/TO/COMPOSITE", "Composite path/name invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            for (int i = 0; i < _editor.Editor.commands.Entries.Count; i++)
            {
                if (_editor.Editor.commands.Entries[i].name == textBox1.Text)
                {
                    MessageBox.Show("Failed to create composite.\nA composite with this name already exists.", "Composite already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            _editor.Editor.commands.AddComposite(textBox1.Text);
            this.Close();
        }
    }
}
