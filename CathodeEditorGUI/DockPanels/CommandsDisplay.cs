﻿using CATHODE.Scripting;
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

namespace CommandsEditor.DockPanels
{
    public partial class CommandsDisplay : DockContent
    {
        private LevelContent _content;
        public LevelContent Content => _content;

        private Task _currentHierarchyCacher = null;

        private string _currentDisplayFolderPath = "";

        private CompositeDisplay _compositeDisplay = null;
        private Composite3D _renderer = null;

        public CommandsDisplay(string levelName)
        {
            InitializeComponent();
            this.Text = levelName;

            _content = new LevelContent(levelName);

            //TODO: these utils should be moved into LevelContent and made less generic. makes no sense anymore.
            _content.editor_utils = new EditorUtils(_content);
            Task.Factory.StartNew(() => _content.editor_utils.GenerateEntityNameCache(Singleton.Editor));
            CacheHierarchies();

            SelectCompositeAndReloadList(_content.commands.EntryPoints[0]);
            Singleton.OnCompositeSelected?.Invoke(_content.commands.EntryPoints[0]); //need to call this again b/c the activation event doesn't fire here
        }

        AddComposite _addCompositeDialog = null;
        private void createComposite_Click(object sender, EventArgs e)
        {
            if (_addCompositeDialog == null)
                _addCompositeDialog = new AddComposite(this, GetSelectedFolder());

            _addCompositeDialog.Show();
            _addCompositeDialog.OnCompositeAdded += SelectCompositeAndReloadList;
            _addCompositeDialog.FormClosed += addCompositeDialogClosed;
        }
        private void addCompositeDialogClosed(object sender, FormClosedEventArgs e)
        {
            _addCompositeDialog = null;
        }

        AddFolder _addFolderDialog = null;
        private void createFolder_Click(object sender, EventArgs e)
        {
            if (_addFolderDialog == null)
                _addFolderDialog = new AddFolder(this, GetSelectedFolder());

            _addFolderDialog.Show();
            _addFolderDialog.OnFolderAdded += SelectCompositeAndReloadList;
            _addFolderDialog.FormClosed += addFolderDialogClosed;
        }
        private void addFolderDialogClosed(object sender, FormClosedEventArgs e)
        {
            _addFolderDialog = null;
        }

        public void SelectCompositeAndReloadList(Composite composite)
        {
            ReloadList();
            SelectComposite(composite);
        }

        /* Reload the folder/composite display */
        private void ReloadList()
        {
            listView1.Items.Clear();
            pathDisplay.Text = _currentDisplayFolderPath.Replace("/", " > ");

            List<string> items = new List<string>();
            foreach (Composite composite in _content.commands.Entries)
            {
                //Make sure this folder/composite should be visible at the current folder path
                string name = composite.name.Replace('\\', '/');
                bool isRoot = _content.commands.EntryPoints[0] == composite;
                if (isRoot) name = Path.GetFileName(composite.name);
                if (name.Length < _currentDisplayFolderPath.Length) continue;
                if (name.Substring(0, _currentDisplayFolderPath.Length) != _currentDisplayFolderPath) continue;

                //Get formatting
                string nameExt = name.Substring(_currentDisplayFolderPath.Length != 0 ? _currentDisplayFolderPath.Length + 1 : 0);
                bool isFolder = nameExt.Contains('/');
                string text = isFolder ? nameExt.Split('/')[0] : nameExt;

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
                    ImageIndex = isFolder ? 1 : isRoot ? 2 : 0,
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

                ReloadList();
            }
            else
            {
                LoadComposite(content.Composite);
            }
        }

        /* File path: go back */
        private void goBackOnPath_Click(object sender, EventArgs e)
        {
            if (_currentDisplayFolderPath == "") return;

            string[] pathSplit = (_currentDisplayFolderPath + "/").Split('/');
            _currentDisplayFolderPath = _currentDisplayFolderPath.Substring(0, _currentDisplayFolderPath.Length - pathSplit[pathSplit.Length - 2].Length);
            if (pathSplit.Length != 2) _currentDisplayFolderPath = _currentDisplayFolderPath.Substring(0, _currentDisplayFolderPath.Length - 1);

            ReloadList();
        }

        private class ListViewItemContent
        {
            public bool IsFolder;
            public Composite Composite;
            public string FolderName;
        }

        private void SelectComposite(Composite composite)
        {
            //_treeHelper.SelectNode(composite.name);

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
                CloseAllChildTabs();

            CompositeDisplay panel = new CompositeDisplay(this, composite);
            panel.Show(Singleton.Editor.DockPanel, DockState.Document);
            panel.FormClosed += OnCompositePanelClosed;
            _compositeDisplay = panel;

#if DEBUG
            //if (_renderer != null) _renderer.Close();
            //_renderer = new Composite3D(_compositeDisplays[composite]);
            //_renderer.Show(Singleton.Editor.DockPanel, DockState.DockRight);
#endif

            return _compositeDisplay;
        }

        private void OnCompositePanelClosed(object sender, FormClosedEventArgs e)
        {
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

        /* Work out the base path for the selected composite/directory */
        private string GetSelectedFolder()
        {
            string baseFolderPath = "";
            /*
            if (FileTree.SelectedNode != null)
            {
                TreeItem selectedItem = (TreeItem)FileTree.SelectedNode.Tag;
                baseFolderPath = selectedItem.String_Value.Replace('\\', '/');
                if (selectedItem.Item_Type != TreeItemType.DIRECTORY)
                {
                    string[] pathSplit = baseFolderPath.Split('/');
                    baseFolderPath = baseFolderPath.Substring(0, baseFolderPath.Length - pathSplit[pathSplit.Length - 1].Length);
                    if (baseFolderPath.Length > 0 && baseFolderPath[baseFolderPath.Length - 1] == '/')
                        baseFolderPath = baseFolderPath.Substring(0, baseFolderPath.Length - 1);
                }
            }
            */
            return baseFolderPath;
        }

        private string _currentSearch = "";
        private void entity_search_btn_Click(object sender, EventArgs e)
        {
            if (entity_search_box.Text == _currentSearch) return;

            List<string> filteredComposites = new List<string>();
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
                filteredComposites.Add(_content.commands.Entries[i].name.Replace('\\', '/'));
            }

            //TODO!!!!

            //_treeHelper.UpdateFileTree(filteredComposites);

            //if (entity_search_box.Text != "")
            //    FileTree.ExpandAll();
        }

        private void findFuncs_Click(object sender, EventArgs e)
        {
            ShowCompositeUses uses = new ShowCompositeUses(Content);
            uses.Show();
            uses.OnEntitySelected += LoadCompositeAndEntity;
        }

        private void loadRootComposite_Click(object sender, EventArgs e)
        {
            LoadComposite(Content.commands.EntryPoints[0]);
        }
        private void loadPauseMenuComposite_Click(object sender, EventArgs e)
        {
            LoadComposite(Content.commands.EntryPoints[2]);
        }
        private void loadGlobalComposite_Click(object sender, EventArgs e)
        {
            LoadComposite(Content.commands.EntryPoints[1]);
        }
    }
}
