using CATHODE;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class EditMaterialMapping : BaseWindow
    {
        public Action<MaterialMappings.MaterialMapping> OnMaterialMappingSelected;

        private MaterialMappings.MaterialMapping _currentMapping;
        private TreeUtility treeHelper;
        private Dictionary<int, MaterialMappings.MaterialMapping> _indexToMapping = new Dictionary<int, MaterialMappings.MaterialMapping>();

        //todo: add support for modifying entries

        public EditMaterialMapping(MaterialMappings.MaterialMapping currentMapping = null) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();
            _currentMapping = currentMapping;

            treeHelper = new TreeUtility(materialMappingTreeView, false);

            PopulateTreeView();

            this.Disposed += EditMaterialMapping_Disposed;
        }

        private void EditMaterialMapping_Disposed(object sender, EventArgs e)
        {
            treeHelper?.ForceClearTree();
            treeHelper = null;
            _indexToMapping.Clear();
        }

        private void PopulateTreeView()
        {
            List<string> filePaths = new List<string>();
            List<string> tags = new List<string>();
            _indexToMapping.Clear();

            for (int i = 0; i < Content.Level.MaterialMaps.Entries.Count; i++)
            {
                string filePath = Content.Level.MaterialMaps.Entries[i].Name.Replace('\\', '/');
                if (filePath.Length > 0 && filePath[0] == '/')
                    filePath = filePath.Substring(1);

                filePaths.Add(filePath);
                tags.Add(i.ToString());
                _indexToMapping[i] = Content.Level.MaterialMaps.Entries[i];
            }

            treeHelper.UpdateFileTree(filePaths, null, tags);

            if (_currentMapping != null)
            {
                int index = Content.Level.MaterialMaps.Entries.IndexOf(_currentMapping);
                if (index >= 0 && index < filePaths.Count)
                {
                    treeHelper.SelectNode(filePaths[index]);
                }
            }

            UpdateMappingsPanel();
        }

        private void materialMappingTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateMappingsPanel();
        }

        private void UpdateMappingsPanel()
        {
            mappingsListView.BeginUpdate();
            mappingsListView.Items.Clear();

            if (materialMappingTreeView.SelectedNode == null)
            {
                mappingsListView.EndUpdate();
                return;
            }

            TreeItem treeItem = (TreeItem)materialMappingTreeView.SelectedNode.Tag;
            if (treeItem.Item_Type != TreeItemType.EXPORTABLE_FILE)
            {
                mappingsListView.EndUpdate();
                return;
            }

            if (!int.TryParse(treeItem.String_Value, out int index) || !_indexToMapping.ContainsKey(index))
            {
                mappingsListView.EndUpdate();
                return;
            }

            MaterialMappings.MaterialMapping selectedMapping = _indexToMapping[index];
            if (selectedMapping == null)
            {
                mappingsListView.EndUpdate();
                return;
            }

            foreach (var mapping in selectedMapping.Mappings)
            {
                ListViewItem item = new ListViewItem(mapping.from ?? "");
                item.SubItems.Add(mapping.to ?? "");
                mappingsListView.Items.Add(item);
            }

            mappingsListView.EndUpdate();
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (materialMappingTreeView.SelectedNode == null)
                return;

            TreeItem treeItem = (TreeItem)materialMappingTreeView.SelectedNode.Tag;
            if (treeItem.Item_Type != TreeItemType.EXPORTABLE_FILE)
                return;

            if (!int.TryParse(treeItem.String_Value, out int index) || !_indexToMapping.ContainsKey(index))
                return;

            MaterialMappings.MaterialMapping selectedMapping = _indexToMapping[index];
            if (selectedMapping == null)
                return;

            OnMaterialMappingSelected?.Invoke(selectedMapping);
            this.Close();
        }
    }
}

