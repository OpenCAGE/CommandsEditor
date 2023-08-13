using CATHODE.Scripting;
using CATHODE;
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
using CATHODE.Scripting.Internal;
using System.Windows.Controls;

namespace CommandsEditor.DockPanels
{
    public partial class CommandsDisplay : DockContent
    {
        private LevelContent _content;
        public LevelContent Content => _content;

        private TreeUtility _treeHelper;

        private Dictionary<Composite, CompositeDisplay> _compositeDisplays = new Dictionary<Composite, CompositeDisplay>();

        public CommandsDisplay(string levelName)
        {
            InitializeComponent();

            _treeHelper = new TreeUtility(FileTree);
            _content = new LevelContent(levelName);

            //TODO: these utils should be moved into LevelContent and made less generic. makes no sense anymore.
            _content.editor_utils = new EditorUtils(_content);
            Task.Factory.StartNew(() => _content.editor_utils.GenerateEntityNameCache());
            Task.Factory.StartNew(() => _content.editor_utils.GenerateCompositeInstances(_content.commands));

            _treeHelper.UpdateFileTree(_content.commands.GetCompositeNames().ToList());
            _treeHelper.SelectNode(_content.commands.EntryPoints[0].name);
        }

        private void createComposite_Click(object sender, EventArgs e)
        {
            AddComposite dialog = new AddComposite(this);
            dialog.Show();
            dialog.FormClosed += new FormClosedEventHandler(add_flow_closed);
        }
        private void add_flow_closed(Object sender, FormClosedEventArgs e)
        {
            _treeHelper.UpdateFileTree(_content.commands.GetCompositeNames().ToList());
            _treeHelper.SelectNode(_content.commands.EntryPoints[0].name);

            this.BringToFront();
            this.Focus();
        }

        private void removeSelected_Click(object sender, EventArgs e)
        {

        }

        private void findUsesOfSelected_Click(object sender, EventArgs e)
        {

        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (FileTree.SelectedNode == null) return;
            if (((TreeItem)FileTree.SelectedNode.Tag).Item_Type != TreeItemType.EXPORTABLE_FILE) return;

            Composite composite = _content.commands.GetComposite(((TreeItem)FileTree.SelectedNode.Tag).String_Value);

            if (_compositeDisplays.ContainsKey(composite))
            {
                _compositeDisplays[composite].Activate();
            }
            else
            {
                CompositeDisplay panel = new CompositeDisplay(this, composite);
                panel.Show(Singleton.Editor.DockPanel, DockState.DockRight);
                panel.FormClosed += OnCompositePanelClosed;
                _compositeDisplays.Add(composite, panel);
            }

            //Close all entity tabs when opening a new composite tab (TODO: should probs make this an option) (TODO: should move this to activate when a composite tab is focussed)
            foreach (KeyValuePair<Composite, CompositeDisplay> display in _compositeDisplays)
            {
                display.Value.CloseAllEntityTabs();
            }
        }
        private void OnCompositePanelClosed(object sender, FormClosedEventArgs e)
        {
            _compositeDisplays.Remove(((CompositeDisplay)sender).Composite);
        }
    }
}
