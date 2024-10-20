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
using System.Runtime.Remoting.Messaging;
using ListViewItem = System.Windows.Forms.ListViewItem;
using ListViewGroupCollapse;
using System.Threading;
using System.Windows.Media.Animation;
using CathodeLib;

namespace CommandsEditor.DockPanels
{
    public partial class CommandsDisplay : DockContent
    {
        private LevelContent _content;
        public LevelContent Content => _content;

        private TreeUtility _treeUtility = null;
        private CancellationTokenSource _prevTaskToken = null;

        private string _currentDisplayFolderPath = "";

        private CompositeDisplay _compositeDisplay = null;
        public CompositeDisplay CompositeDisplay => _compositeDisplay;

        private Composite3D _renderer = null;

        AddComposite _addCompositeDialog = null;
        AddFolder _addFolderDialog = null;

        private int _defaultSplitterDistance = 330;

        public CommandsDisplay(string levelName)
        {
            InitializeComponent();

            //this.Text = levelName;
            this.FormClosed += CommandsDisplay_FormClosed;
            this.Load += CommandsDisplay_Load;

            _content = new LevelContent(levelName);
            _treeUtility = new TreeUtility(treeView1);

#if !DEBUG 
            DEBUG_LoadNextEmpty.Visible = false;
#endif

            Singleton.OnCompositeRenamed += OnCompositeRenamed;
        }

        private void OnCompositeRenamed(Composite composite, string name)
        {
            ReloadList();
        }

        private void CommandsDisplay_Load(object sender, EventArgs e)
        {
            if (Enum.TryParse<View>(SettingsManager.GetString(Singleton.Settings.FileBrowserViewOpt), out View view))
                SetViewMode(view);

            //TODO: these utils should be moved into LevelContent and made less generic. makes no sense anymore.
            _content.editor_utils = new EditorUtils(_content);
            Task.Factory.StartNew(() => _content.editor_utils.GenerateEntityNameCache(Singleton.Editor));
            Content.editor_utils.GenerateCompositeInstances(Content.commands);

            SelectCompositeAndReloadList(_content.commands.EntryPoints[0]);
            //Singleton.OnCompositeSelected?.Invoke(_content.commands.EntryPoints[0]); //need to call this again b/c the activation event doesn't fire here
        }

        private void CommandsDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.FormClosed -= CommandsDisplay_FormClosed;
            this.Load -= CommandsDisplay_Load;
            Singleton.OnCompositeRenamed -= OnCompositeRenamed;

            if (_compositeDisplay != null)
                _compositeDisplay.FormClosing -= CompositeDisplay_FormClosing;
            if (_renameComposite != null)
                _renameComposite.FormClosed -= _renameComposite_FormClosed;
            if (_addCompositeDialog != null)
            {
                _addCompositeDialog.FormClosed -= addCompositeDialogClosed;
                _addCompositeDialog.OnCompositeAdded -= SelectCompositeAndReloadList;
            }
            if (_addFolderDialog != null)
            {
                _addFolderDialog.FormClosed -= addFolderDialogClosed;
                _addFolderDialog.OnFolderAdded -= SelectCompositeAndReloadList;
            }
            if (_functionUsesDialog != null)
            {
                _functionUsesDialog.OnEntitySelected -= LoadCompositeAndEntity;
                _functionUsesDialog.FormClosed -= _functionUsesDialog_FormClosed;
            }

            _content = null;

            _treeUtility?.ForceClearTree();
            _treeUtility = null;

            _prevTaskToken?.Cancel();

            _compositeDisplay?.Close();
            _renderer?.Close();

            _addCompositeDialog?.Close();
            _addFolderDialog?.Close();

            imageList.Images.Clear();
            imageList.Dispose();
            FileBrowserImageListLarge.Images.Clear();
            FileBrowserImageListLarge.Dispose();
            FileBrowserImageListSmall.Images.Clear();
            FileBrowserImageListSmall.Dispose();
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
            string[] currentPathSplit = _currentDisplayFolderPath.Split('/');
            bool currentPathIsRoot = currentPathSplit.Length == 1 && currentPathSplit[0] == "";

            List<string> items = new List<string>();
            foreach (Composite composite in _content.commands.Entries)
            {
                //Make sure this folder/composite should be visible at the current folder path
                string name = composite.name.Replace('\\', '/');
                string[] nameSplit = name.Split('/');
                bool shouldAdd = true;
                if (!currentPathIsRoot)
                {
                    for (int i = 0; i < currentPathSplit.Length; i++)
                    {
                        if (i >= nameSplit.Length) break;
                        if (currentPathSplit[i] != nameSplit[i])
                        {
                            shouldAdd = false;
                            break;
                        }
                    }
                }
                if (!shouldAdd) continue;

                //Get formatting
                bool isFolder = nameSplit.Length > (currentPathIsRoot ? currentPathSplit.Length : currentPathSplit.Length + 1);
                string text = nameSplit[currentPathIsRoot ? 0 : currentPathSplit.Length];
                if (text == "") continue;

                EditorUtils.CompositeType type = Content.editor_utils.GetCompositeType(composite);

                //Make sure this hasn't already been added
                if (items.Contains(text)) continue;
                items.Add(text);

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

        /* Enable/disable the file browser UI */
        public void UpdateDockState()
        {
            DockAreas = DockAreas.DockBottom | DockAreas.DockLeft;
            if (SettingsManager.GetBool(Singleton.Settings.AutoHideCompositeDisplay))
                Show(Singleton.Editor.DockPanel, SettingsManager.GetBool(Singleton.Settings.EnableFileBrowser) ? DockState.DockBottomAutoHide : DockState.DockLeftAutoHide);
            else
                Show(Singleton.Editor.DockPanel, SettingsManager.GetBool(Singleton.Settings.EnableFileBrowser) ? DockState.DockBottom : DockState.DockLeft);
            DockAreas = SettingsManager.GetBool(Singleton.Settings.EnableFileBrowser) ? DockAreas.DockBottom : DockAreas.DockLeft;

            splitContainer1.Panel2Collapsed = !SettingsManager.GetBool(Singleton.Settings.EnableFileBrowser);
            splitContainer1.FixedPanel = FixedPanel.Panel1;

            try
            {
                splitContainer1.SplitterDistance = SettingsManager.GetInteger(Singleton.Settings.CompositeSplitWidth, _defaultSplitterDistance);
            }
            catch { }

            if (SettingsManager.GetBool(Singleton.Settings.AutoHideCompositeDisplay))
                Singleton.Editor.DockPanel.ActiveAutoHideContent = this;
            else
                Singleton.Editor.DockPanel.ActiveAutoHideContent = null;
        }

        public void ResetSplitter()
        {
            splitContainer1.SplitterDistance = _defaultSplitterDistance;
        }

        //UI: handle saving split container width between commands/runs 
        private void treeView1_Resize(object sender, EventArgs e)
        {
            SettingsManager.SetInteger(Singleton.Settings.CompositeSplitWidth, splitContainer1.SplitterDistance);
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
            treeView1.SelectedNode?.Expand();
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
            _compositeDisplay?.DepopulateUI();
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
            if (composite == null) 
                return null;

            if (_compositeDisplay?.Composite == composite)
                return _compositeDisplay;

            if (_compositeDisplay == null)
            {
                _compositeDisplay = new CompositeDisplay(this);
                _compositeDisplay.Show(Singleton.Editor.DockPanel, DockState.Document);
                _compositeDisplay.FormClosing += CompositeDisplay_FormClosing;
            }
            _compositeDisplay.PopulateUI(composite);

#if DEBUG
            //if (_renderer != null) _renderer.Close();
            //_renderer = new Composite3D(_compositeDisplay);
            //_renderer.Show(Singleton.Editor.DockPanel, DockState.DockRight);
#endif

            SelectComposite(composite);
            return _compositeDisplay;
        }
        private void CompositeDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            CompositeDisplay display = (CompositeDisplay)sender;
            display.DepopulateUI();
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
            if (prompt && MessageBox.Show("Are you sure you want to remove " + Path.GetFileName(composite.name) + "?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

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
                        Entity linkedEntity = Content.commands.Entries[i].GetEntityByID(Content.commands.Entries[i].functions[x].childLinks[l].linkedEntityID);
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
            CommandsUtils.PurgedComposites.purged.Clear(); //TODO: we should smartly remove from this list, rather than removing all

            //Refresh UI
            ReloadList();
            Content.editor_utils.GenerateCompositeInstances(Content.commands);
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

            if (entity_search_box.Text != "")
            {
                treeView1.ExpandAll();

                /*
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
                */
            }
            else
            {
                //ReloadList();
            }
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
        TreeNode _rightClickedNode = null;
        private void FileTree_MouseDown(object sender, MouseEventArgs e)
        {
            if (SettingsManager.GetBool(Singleton.Settings.EnableFileBrowser))
                return;

            if (e.Button == MouseButtons.Right)
            {
                var lv = sender as System.Windows.Forms.TreeView;
                _rightClickedNode = lv.HitTest(e.Location).Node;

                toolStripMenuItem4.Enabled = _rightClickedNode != null;
                toolStripMenuItem5.Enabled = _rightClickedNode != null;

                if (_rightClickedNode == null)
                {
                    _currentDisplayFolderPath = "";
                }
                else
                {
                    TreeItem item = (TreeItem)_rightClickedNode.Tag;
                    switch (item.Item_Type)
                    {
                        case TreeItemType.EXPORTABLE_FILE:
                            Composite c = Content.commands.GetComposite(item.String_Value);
                            int nameLength = EditorUtils.GetCompositeName(c).Length;
                            _currentDisplayFolderPath = (c.name.Length != nameLength ? c.name.Substring(0, c.name.Length - nameLength - 1) : "");
                            break;
                        case TreeItemType.DIRECTORY:
                            _currentDisplayFolderPath = item.String_Value;
                            break;
                    }
                }

                FileTreeContextMenuNew.Show(lv, e.Location);
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

                //TODO: does this work?

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
        private void deleteViaTreeView_Click(object sender, EventArgs e)
        {
            TreeItem item = (TreeItem)_rightClickedNode.Tag;
            switch (item.Item_Type)
            {
                case TreeItemType.EXPORTABLE_FILE:
                    DeleteComposite(Content.commands.GetComposite(item.String_Value));
                    break;
                case TreeItemType.DIRECTORY:
                    //TODO
                    MessageBox.Show("Support for deleting folders is coming soon.");
                    break;
            }
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
                RenameComposite(content.Composite);
            }
        }
        private void renameViaTreeView_Click(object sender, EventArgs e)
        {
            TreeItem item = (TreeItem)_rightClickedNode.Tag;
            switch (item.Item_Type)
            {
                case TreeItemType.EXPORTABLE_FILE:
                    RenameComposite(Content.commands.GetComposite(item.String_Value));
                    break;
                case TreeItemType.DIRECTORY:
                    //_currentDisplayFolderPath = item.String_Value;
                    //TODO
                    MessageBox.Show("Support for renaming folders is coming soon.");
                    break;
            }
        }
        private void RenameComposite(Composite composite)
        {
            if (_renameComposite != null)
                _renameComposite.Close();

            _renameComposite = new RenameComposite(composite);
            _renameComposite.Show();
            _renameComposite.FormClosed += _renameComposite_FormClosed;
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

        private void largeIconsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SetViewMode(View.LargeIcon);
        }
        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetViewMode(View.SmallIcon);
        }
        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetViewMode(View.List);
        }
        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetViewMode(View.Tile);
        }
        private void SetViewMode(View view)
        {
            listView1.View = view;

            SettingsManager.SetString(Singleton.Settings.FileBrowserViewOpt, view.ToString());

            largeIconsToolStripMenuItem.Checked = view == View.LargeIcon;
            listToolStripMenuItem.Checked = view == View.List;
        }

        private void createComposite_Click(object sender, EventArgs e)
        {
            compositeToolStripMenuItem_Click(null, null);
        }
        private void createCompositeViaTreeView_Click(object sender, EventArgs e)
        {
            compositeToolStripMenuItem_Click(null, null);
        }
        private void createFolder_Click(object sender, EventArgs e)
        {
            folderToolStripMenuItem_Click(null, null);
        }
        private void createFolderViaTreeView_Click(object sender, EventArgs e)
        {
            folderToolStripMenuItem_Click(null, null);
        }

        ShowCompositeUses _functionUsesDialog = null;
        private void findFunctionUses_Click(object sender, EventArgs e)
        {
            if (_functionUsesDialog != null)
            {
                _functionUsesDialog.Focus();
                _functionUsesDialog.BringToFront();
                return;
            }

            _functionUsesDialog = new ShowCompositeUses();
            _functionUsesDialog.Show();
            _functionUsesDialog.OnEntitySelected += LoadCompositeAndEntity;
            _functionUsesDialog.FormClosed += _functionUsesDialog_FormClosed;
        }
        private void _functionUsesDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _functionUsesDialog = null;
        }

        private static List<Composite> _toSkip = new List<Composite>();

        private void DEBUG_LoadNextEmpty_Click(object sender, EventArgs e)
        {
            DEBUG_LoadNextToConstruct();
        }
        public void DEBUG_LoadNextToConstruct()
        {
#if !DEBUG
            MessageBox.Show("This should not be reached in production code...");
            return;
#endif

            if (FlowgraphLayoutManager.DEBUG_IsUnfinished)
            {
                _toSkip.Add(CompositeDisplay.Composite);
            }

            FlowgraphLayoutManager.DEBUG_UsePreDefinedTable = true;
            List<Composite> ordered = Content.commands.Entries.OrderBy(o => CompositeUtils.CountLinks(o)).Where(o => CompositeUtils.CountLinks(o) != 0 && !FlowgraphLayoutManager.HasLayout(o)).ToList();
            for (int i = 0; i < ordered.Count; i++)
                Console.WriteLine(ordered[i].name);
            Console.WriteLine("Still " + ordered.Count + " to go in this PAK (count includes those not purged, so may be lower)");
            FlowgraphLayoutManager.DEBUG_UsePreDefinedTable = false;

            int index = 0;
            while (_toSkip.Contains(ordered[index]))
                index++;
                
            LoadComposite(ordered[index]);
        }
    }
}
