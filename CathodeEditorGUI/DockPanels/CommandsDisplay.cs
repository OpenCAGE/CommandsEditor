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
using WebSocketSharp;
using CommandsEditor.Popups;
using OpenCAGE;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.Remoting.Messaging;
using ListViewItem = System.Windows.Forms.ListViewItem;
using ListViewGroupCollapse;

namespace CommandsEditor.DockPanels
{
    public partial class CommandsDisplay : DockContent
    {
        private LevelContent _content;
        public LevelContent Content => _content;

        private TreeUtility _treeUtility = null;
        private Task _currentHierarchyCacher = null;

        private string _currentDisplayFolderPath = "";

        private CompositeDisplay _compositeDisplay = null;
        private Composite3D _renderer = null;

        AddComposite _addCompositeDialog = null;
        AddFolder _addFolderDialog = null;

        public CommandsDisplay(string levelName)
        {
            InitializeComponent();
            this.Text = levelName;

            _content = new LevelContent(levelName);
            _treeUtility = new TreeUtility(treeView1, _content);

            //TODO: these utils should be moved into LevelContent and made less generic. makes no sense anymore.
            _content.editor_utils = new EditorUtils(_content);
            Task.Factory.StartNew(() => _content.editor_utils.GenerateEntityNameCache(Singleton.Editor));
            CacheHierarchies();

            SelectCompositeAndReloadList(_content.commands.EntryPoints[0]);
            Singleton.OnCompositeSelected?.Invoke(_content.commands.EntryPoints[0]); //need to call this again b/c the activation event doesn't fire here
        }

        public void SelectCompositeAndReloadList(Composite composite)
        {
            Content.commands.Entries = Content.commands.Entries.OrderBy(o => o.name).ToList();
            ReloadList();
            SelectComposite(composite);
        }

        /* Reload the folder/composite display */
        private void ReloadList(bool updateListViewToo = true)
        {
            if (updateListViewToo)
            {
                _treeUtility.UpdateFileTree(_content.commands.GetCompositeNames().ToList());
            }

            listView1.Items.Clear();
            pathDisplay.Text = _currentDisplayFolderPath.Replace("/", " > ");

            List<string> items = new List<string>();
            foreach (Composite composite in _content.commands.Entries)
            {
                //Make sure this folder/composite should be visible at the current folder path
                string name = composite.name.Replace('\\', '/');
                if (name.Length < _currentDisplayFolderPath.Length) continue;
                if (name.Substring(0, _currentDisplayFolderPath.Length) != _currentDisplayFolderPath) continue;

                //Get formatting
                string nameExt = name.Substring(_currentDisplayFolderPath.Length != 0 ? _currentDisplayFolderPath.Length + 1 : 0);
                bool isFolder = nameExt.Contains('/');
                string text = isFolder ? nameExt.Split('/')[0] : nameExt;
                if (text == "") continue;

                EditorUtils.CompositeType type = Content.editor_utils.GetCompositeType(composite);

                //Make sure this folder/composite hasn't already been added
                string identifier = text + isFolder;
                if (items.Contains(identifier)) continue;
                items.Add(identifier);

                //Add it to the view
                ListViewItemContent content = new ListViewItemContent() { IsFolder = isFolder };
                if (isFolder) content.FolderName = text;
                else content.Composite = composite;
                listView1.Items.Add(new ListViewItem()
                {
                    Text = text,
                    ImageIndex = isFolder ? 1 : type == EditorUtils.CompositeType.IS_ROOT ? 2 : type == EditorUtils.CompositeType.IS_PAUSE_MENU || type == EditorUtils.CompositeType.IS_GLOBAL ? 3 : type == EditorUtils.CompositeType.IS_DISPLAY_MODEL ? 4 : 0,
                    Tag = content
                });
            }
        }

        /* File browser: select folder/composite */
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            ListViewItem item = listView1.SelectedItems[0];
            ListViewItemContent content = (ListViewItemContent)item.Tag;
            if (content.IsFolder)
            {
                if (_currentDisplayFolderPath == "") _currentDisplayFolderPath = content.FolderName;
                else _currentDisplayFolderPath = _currentDisplayFolderPath + "/" + content.FolderName;

                ReloadList(false);
            }
            else
            {
                LoadComposite(content.Composite);
            }

            _treeUtility.SelectNode(_currentDisplayFolderPath);
        }

        /* File list: select folder/composite */
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            TreeItem item = (TreeItem)treeView1.SelectedNode.Tag;
            switch (item.Item_Type)
            {
                case TreeItemType.EXPORTABLE_FILE:
                    LoadComposite(item.String_Value);
                    break;
                case TreeItemType.DIRECTORY:
                    _currentDisplayFolderPath = item.String_Value;
                    ReloadList(false);
                    break;
            }
        }

        /* File path: go back */
        private void goBackOnPath_Click(object sender, EventArgs e)
        {
            if (_currentDisplayFolderPath == "") return;

            string[] pathSplit = (_currentDisplayFolderPath + "/").Split('/');
            _currentDisplayFolderPath = _currentDisplayFolderPath.Substring(0, _currentDisplayFolderPath.Length - pathSplit[pathSplit.Length - 2].Length);
            if (pathSplit.Length != 2) _currentDisplayFolderPath = _currentDisplayFolderPath.Substring(0, _currentDisplayFolderPath.Length - 1);

            ReloadList(false);
        }

        private class ListViewItemContent
        {
            public bool IsFolder;
            public Composite Composite;
            public string FolderName;
        }

        private void SelectComposite(Composite composite)
        {
            _treeUtility.SelectNode(composite.name);

            //TODO: select in file viewer too
            //_currentDisplayFolderPath = composite.name;

            this.BringToFront();
            this.Focus();
        }

        public void CloseAllChildTabs()
        {
            _compositeDisplay?.CloseAllChildTabs();
            _compositeDisplay?.Close();
        }

        public void ReloadAllEntities()
        {
            _compositeDisplay?.ReloadAllEntities();
        }

        public void Reload(bool alsoReloadEntities = true)
        {
            _compositeDisplay?.Reload(alsoReloadEntities);
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

            if (_compositeDisplay != null)
            {
                if (_compositeDisplay?.Composite == composite) return _compositeDisplay;
                CloseAllChildTabs();
            }

            CompositeDisplay panel = new CompositeDisplay(this, composite);
            panel.Show(Singleton.Editor.DockPanel, DockState.Document);
            panel.FormClosed += OnCompositePanelClosed;
            _compositeDisplay = panel;

#if DEBUG
            //if (_renderer != null) _renderer.Close();
            //_renderer = new Composite3D(_compositeDisplay);
            //_renderer.Show(Singleton.Editor.DockPanel, DockState.DockRight);
#endif

            SelectComposite(composite);
            return _compositeDisplay;
        }

        private void OnCompositePanelClosed(object sender, FormClosedEventArgs e)
        {
            _compositeDisplay?.CloseAllChildTabs();
            _compositeDisplay?.Dispose();
            _compositeDisplay = null;
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

        public void DeleteComposite(Composite composite, bool prompt = true)
        {
            for (int i = 0; i < Content.commands.EntryPoints.Count(); i++)
            {
                if (composite.shortGUID == Content.commands.EntryPoints[i].shortGUID)
                {
                    MessageBox.Show("Cannot delete a composite which is the root, global, or pause menu!", "Cannot delete.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            if (prompt && MessageBox.Show("Are you sure you want to remove this composite?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            if (_compositeDisplay != null && _compositeDisplay.Composite == composite)
                CloseAllChildTabs();

            //Remove any entities or links that reference this composite
            for (int i = 0; i < Content.commands.Entries.Count; i++)
            {
                List<FunctionEntity> prunedFunctionEntities = new List<FunctionEntity>();
                for (int x = 0; x < Content.commands.Entries[i].functions.Count; x++)
                {
                    if (Content.commands.Entries[i].functions[x].function == composite.shortGUID) continue;
                    List<EntityConnector> prunedEntityLinks = new List<EntityConnector>();
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
            ReloadList();
            CacheHierarchies();
        }

        /* Cache entity hierarchies */
        public void CacheHierarchies()
        {
            if (_currentHierarchyCacher != null) _currentHierarchyCacher.Dispose();
            _currentHierarchyCacher = Task.Factory.StartNew(() => Content.editor_utils.GenerateCompositeInstances(Content.commands));
        }

        private string _currentSearch = "";
        private void entity_search_btn_Click(object sender, EventArgs e)
        {
            if (entity_search_box.Text == _currentSearch) return;

            List<string> filteredCompositeNames = new List<string>();
            List<Composite> filteredComposites = new List<Composite>();
            _currentSearch = entity_search_box.Text.Replace('\\', '/').ToUpper();
            for (int i = 0; i < _content.commands.Entries.Count; i++)
            {
                string name = _content.commands.Entries[i].name.Replace('\\', '/');

                if (SettingsManager.GetBool(Singleton.Settings.CompNameOnlyOpt) == true)
                {
                    string[] nameSplit = name.Split('/');
                    name = nameSplit[nameSplit.Length - 1];
                }

                if (!name.ToUpper().Contains(_currentSearch)) continue;

                filteredCompositeNames.Add(_content.commands.Entries[i].name.Replace('\\', '/'));
                filteredComposites.Add(_content.commands.Entries[i]);
            }

            _treeUtility.UpdateFileTree(filteredCompositeNames);

            /*
            if (entity_search_box.Text != "")
            {
                treeView1.ExpandAll();

                listView1.Items.Clear();
                pathDisplay.Text = "";
                foreach (Composite composite in filteredComposites)
                {
                    bool isRoot = _content.commands.EntryPoints[0] == composite;
                    listView1.Items.Add(new ListViewItem()
                    {
                        Text = Path.GetFileName(composite.name),
                        ImageIndex = isRoot ? 2 : 0,
                        Tag = new ListViewItemContent() { IsFolder = false, Composite = composite }
                    });
                }
            }
            else
            {
                ReloadList();
            }
            */
        }

        /* File Browser Context Menu */
        private void FooListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var lv = sender as System.Windows.Forms.ListView; 
                var item = lv.HitTest(e.Location).Item;

                deleteFolderToolStripMenuItem.Enabled = item != null;
                renameToolStripMenuItem.Enabled = item != null;

                if (item != null)
                    lv.FocusedItem = item;

                FileBrowserContextMenu.Show(lv, e.Location);
            }
        }
        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            ListViewItem item = listView1.SelectedItems[0];
            ListViewItemContent content = (ListViewItemContent)item.Tag;
            if (content.IsFolder)
            {
                string folderFullPath = "";
                if (_currentDisplayFolderPath == "") folderFullPath = content.FolderName;
                else folderFullPath = _currentDisplayFolderPath + "/" + content.FolderName;

                List<Composite> toDelete = new List<Composite>();
                for (int i = 0; i < _content.commands.Entries.Count; i++)
                    if (_content.commands.Entries[i].name.Length >= folderFullPath.Length && _content.commands.Entries[i].name.Substring(0, folderFullPath.Length) == folderFullPath)
                        toDelete.Add(_content.commands.Entries[i]);

                if (MessageBox.Show("Are you sure you want to delete this folder, including the " + toDelete.Count + " composites it contains?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) 
                    return;

                for (int i = 0; i < toDelete.Count; i++)
                    DeleteComposite(toDelete[i], false);
            }
            else
            {
                DeleteComposite(content.Composite);
            }

            _compositeDisplay?.Reload();
            ReloadList();
        }
        RenameComposite _renameComposite;
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            ListViewItem item = listView1.SelectedItems[0];
            ListViewItemContent content = (ListViewItemContent)item.Tag;
            if (content.IsFolder)
            {
                //TODO
                MessageBox.Show("Support for renaming folders is coming soon.");
            }
            else
            {
                if (_renameComposite != null)
                    _renameComposite.Close();

                _renameComposite = new RenameComposite(this.Content, content.Composite, _currentDisplayFolderPath);
                _renameComposite.Show();
                _renameComposite.OnRenamed += OnCompositeRenamed;
                _renameComposite.FormClosed += _renameComposite_FormClosed;
            }
        }
        private void OnCompositeRenamed(string name)
        {
            _compositeDisplay?.Reload();
            ReloadList();

            //TODO-URGENT: Also need to update cached entity names that use this composite.
        }
        private void _renameComposite_FormClosed(object sender, FormClosedEventArgs e)
        {
            _renameComposite = null;
        }
        private void compositeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_addCompositeDialog == null)
                _addCompositeDialog = new AddComposite(this, _currentDisplayFolderPath);

            _addCompositeDialog.Show();
            _addCompositeDialog.OnCompositeAdded += SelectCompositeAndReloadList;
            _addCompositeDialog.FormClosed += addCompositeDialogClosed;
        }
        private void addCompositeDialogClosed(object sender, FormClosedEventArgs e)
        {
            _addCompositeDialog = null;
        }
        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_addFolderDialog == null)
                _addFolderDialog = new AddFolder(this, _currentDisplayFolderPath);

            _addFolderDialog.Show();
            _addFolderDialog.OnFolderAdded += SelectCompositeAndReloadList;
            _addFolderDialog.FormClosed += addFolderDialogClosed;
        }
        private void addFolderDialogClosed(object sender, FormClosedEventArgs e)
        {
            _addFolderDialog = null;
        }
    }
}
