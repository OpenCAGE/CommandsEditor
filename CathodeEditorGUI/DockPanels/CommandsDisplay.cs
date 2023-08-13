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
using System.Xml.Linq;
using System.Windows.Interop;

namespace CommandsEditor.DockPanels
{
    public partial class CommandsDisplay : DockContent
    {
        private LevelContent _content;
        public LevelContent Content => _content;

        private TreeUtility _treeHelper;
        private Task _currentHierarchyCacher = null;

        private Dictionary<Composite, CompositeDisplay> _compositeDisplays = new Dictionary<Composite, CompositeDisplay>();

        public CommandsDisplay(string levelName)
        {
            InitializeComponent();
            this.Text = levelName;

            _treeHelper = new TreeUtility(FileTree);
            _content = new LevelContent(levelName);

            //TODO: these utils should be moved into LevelContent and made less generic. makes no sense anymore.
            _content.editor_utils = new EditorUtils(_content);
            Task.Factory.StartNew(() => _content.editor_utils.GenerateEntityNameCache());
            CacheHierarchies();

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

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (FileTree.SelectedNode == null) return;
            if (((TreeItem)FileTree.SelectedNode.Tag).Item_Type != TreeItemType.EXPORTABLE_FILE) return;

            LoadComposite(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
        }
        private void OnCompositePanelClosed(object sender, FormClosedEventArgs e)
        {
            _compositeDisplays.Remove(((CompositeDisplay)sender).Composite);
        }

        public void CloseAllChildTabs()
        {
            List<CompositeDisplay> displays = new List<CompositeDisplay>();
            foreach (KeyValuePair<Composite, CompositeDisplay> display in _compositeDisplays)
                displays.Add(display.Value);
            foreach (CompositeDisplay display in displays)
            {
                display.CloseAllChildTabs();
                display.Close();
            }
        }

        public CompositeDisplay LoadComposite(string name)
        {
            return LoadComposite(_content.commands.GetComposite(name));
        }
        public CompositeDisplay LoadComposite(ShortGuid guid)
        {
            return LoadComposite(_content.commands.GetComposite(guid));
        }
        public CompositeDisplay LoadComposite(Composite composite)
        {
            if (composite == null) return null;

            if (_compositeDisplays.ContainsKey(composite))
            {
                _compositeDisplays[composite].Activate();
            }
            else
            {
                CompositeDisplay panel = new CompositeDisplay(this, composite);
                panel.Show(Singleton.Editor.DockPanel, DockState.Document);
                panel.FormClosed += OnCompositePanelClosed;
                _compositeDisplays.Add(composite, panel);
            }

            return _compositeDisplays[composite];
        }

        public void LoadCompositeAndEntity(ShortGuid compositeGUID, ShortGuid entityGUID)
        {
            Composite composite = _content.commands.GetComposite(compositeGUID);
            LoadCompositeAndEntity(composite, composite?.GetEntityByID(entityGUID));
        }
        public void LoadCompositeAndEntity(Composite composite, Entity entity)
        {
            CompositeDisplay panel = LoadComposite(composite);
            panel?.LoadEntity(entity);
        }

        public void DeleteComposite(Composite composite)
        {
            for (int i = 0; i < Content.commands.EntryPoints.Count(); i++)
            {
                if (composite.shortGUID == Content.commands.EntryPoints[i].shortGUID)
                {
                    MessageBox.Show("Cannot delete a composite which is the root, global, or pause menu!", "Cannot delete.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (MessageBox.Show("Are you sure you want to remove this composite?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            if (_compositeDisplays.ContainsKey(composite))
                _compositeDisplays[composite].Close();

            //Remove any entities or links that reference this composite
            for (int i = 0; i < Content.commands.Entries.Count; i++)
            {
                List<FunctionEntity> prunedFunctionEntities = new List<FunctionEntity>();
                for (int x = 0; x < Content.commands.Entries[i].functions.Count; x++)
                {
                    if (Content.commands.Entries[i].functions[x].function == composite.shortGUID) continue;
                    List<EntityLink> prunedEntityLinks = new List<EntityLink>();
                    for (int l = 0; l < Content.commands.Entries[i].functions[x].childLinks.Count; l++)
                    {
                        Entity linkedEntity = Content.commands.Entries[i].GetEntityByID(Content.commands.Entries[i].functions[x].childLinks[l].childID);
                        if (linkedEntity != null && linkedEntity.variant == EntityVariant.FUNCTION) if (((FunctionEntity)linkedEntity).function == composite.shortGUID) continue;
                        prunedEntityLinks.Add(Content.commands.Entries[i].functions[x].childLinks[l]);
                    }
                    Content.commands.Entries[i].functions[x].childLinks = prunedEntityLinks;
                    prunedFunctionEntities.Add(Content.commands.Entries[i].functions[x]);
                }
                Content.commands.Entries[i].functions = prunedFunctionEntities;
            }
            //TODO: remove proxies etc that also reference any of the removed entities

            //Remove the composite
            Content.commands.Entries.Remove(composite);

            //Refresh UI
            _treeHelper.UpdateFileTree(Content.commands.GetCompositeNames().ToList());
            CacheHierarchies();
        }

        /* Cache entity hierarchies */
        public void CacheHierarchies()
        {
            if (_currentHierarchyCacher != null) _currentHierarchyCacher.Dispose();
            _currentHierarchyCacher = Task.Factory.StartNew(() => Content.editor_utils.GenerateCompositeInstances(Content.commands));
        }
    }
}
