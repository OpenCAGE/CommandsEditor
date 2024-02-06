using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
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
using System.Windows.Input;

namespace CommandsEditor
{
    public partial class RenameComposite : BaseWindow
    {
        public Action<string> OnRenamed;

        private Composite _composite;
        private string _folder;

        public RenameComposite(Composite composite, string path) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();
            
            _folder = path;
            _composite = composite;
            entity_name.Text = EditorUtils.GetCompositeName(_composite);
        }

        private void save_entity_name_Click(object sender, EventArgs e)
        {
            if (entity_name.Text == "") return;

            //THIS LOGIC IS COPIED FROM AddComposite
            string path = (_folder == "" ? _folder : _folder + "/") + entity_name.Text.Replace("\\", "/");

            string[] pathParts = path.Split('/');
            for (int i = 0; i < pathParts.Length; i++)
            {
                if (pathParts[i] == "")
                {
                    MessageBox.Show("Failed to create composite: a part of the path is blank.\nRemove trailing slashes and use complete folder names, e.g.:\nSOME/FILE/PATH/TO/COMPOSITE", "Composite path/name invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            for (int i = 0; i < Content.commands.Entries.Count; i++)
            {
                if (Content.commands.Entries[i].name.Replace("\\", "/") == path)
                {
                    MessageBox.Show("Failed to create composite.\nA composite with this name already exists.", "Composite already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //--


            _composite.name = path;

            OnRenamed?.Invoke(path);
            this.Close();
        }
    }
}
