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
            Task.Factory.StartNew(() => _content.editor_utils.GenerateEntityNameCache(Singleton.Editor));
            CacheHierarchies();

            SelectCompositeAndReloadList(_content.commands.EntryPoints[0]);
            Singleton.OnCompositeSelected?.Invoke(_content.commands.EntryPoints[0]); //need to call this again b/c the activation event doesn't fire here
        }

        public void Reload(bool reloadAllComposites = true, bool reloadAllEntities = true)
        {
            //TODO: do we want to select this composite?
            SelectCompositeAndReloadList(_content.commands.EntryPoints[0]);

            if (reloadAllComposites)
            {
                foreach (KeyValuePair<Composite, CompositeDisplay> display in _compositeDisplays)
                {
                    display.Value.Reload(reloadAllEntities);
                }
            }
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

        private void ReloadList()
        {
            _treeHelper.UpdateFileTree(_content.commands.GetCompositeNames().ToList());
        }

        private void SelectComposite(Composite composite)
        {
            _treeHelper.SelectNode(composite.name);

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

        public void CloseAllChildTabsExcept(Composite composite)
        {
            List<CompositeDisplay> displays = new List<CompositeDisplay>();
            foreach (KeyValuePair<Composite, CompositeDisplay> display in _compositeDisplays)
            {
                if (display.Key == composite) continue;
                displays.Add(display.Value);
            }
            foreach (CompositeDisplay display in displays)
            {
                display.CloseAllChildTabs();
                display.Close();
            }
        }
        public void CloseAllChildTabs()
        {
            CloseAllChildTabsExcept(null);
        }

        public void ReloadAllEntities()
        {
            foreach (KeyValuePair<Composite, CompositeDisplay> display in _compositeDisplays)
                display.Value.ReloadAllEntities();
        }

        public void Reload(bool alsoReloadEntities = true)
        {
            foreach (KeyValuePair<Composite, CompositeDisplay> display in _compositeDisplays)
                display.Value.Reload(alsoReloadEntities);
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

        /* Work out the base path for the selected composite/directory */
        private string GetSelectedFolder()
        {
            string baseFolderPath = "";
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

                if (SettingsManager.GetBool("CS_SearchOnlyCompName") == true)
                {
                    string[] nameSplit = name.Split('/');
                    name = nameSplit[nameSplit.Length - 1];
                }

                if (!name.ToUpper().Contains(_currentSearch)) continue;
                filteredComposites.Add(_content.commands.Entries[i].name.Replace('\\', '/'));
            }
            _treeHelper.UpdateFileTree(filteredComposites);

            if (entity_search_box.Text != "")
                FileTree.ExpandAll();
        }

        private void findFuncs_Click(object sender, EventArgs e)
        {
            ShowCompositeUses uses = new ShowCompositeUses(Content);
            uses.Show();
            uses.OnEntitySelected += LoadCompositeAndEntity;
        }
    }
}
