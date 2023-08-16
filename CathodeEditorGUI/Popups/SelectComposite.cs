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
using CATHODE.Scripting.Internal;
using CommandsEditor.Nodes;
using CommandsEditor.Popups.Base;

namespace CommandsEditor
{
    public partial class SelectComposite : BaseWindow
    {
        public Action<Composite> OnCompositeGenerated;

        private TreeUtility _treeHelper;

        public SelectComposite(LevelContent content, string starting = null) : base(WindowClosesOn.NEW_COMPOSITE_SELECTION | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.COMMANDS_RELOAD, content)
        {
            InitializeComponent();

            _treeHelper = new TreeUtility(FileTree);
            _treeHelper.UpdateFileTree(Content.commands.GetCompositeNames().ToList());
            _treeHelper.SelectNode(starting == null || starting == "" ? Content.commands.EntryPoints[0].name : starting);
        }

        private void SelectEntity_Click(object sender, EventArgs e)
        {
            if (FileTree.SelectedNode == null) return;
            if (((TreeItem)FileTree.SelectedNode.Tag).Item_Type != TreeItemType.EXPORTABLE_FILE) return;
            OnCompositeGenerated?.Invoke(Content.commands.GetComposite(((TreeItem)FileTree.SelectedNode.Tag).String_Value));
            this.Close();
        }
    }
}
