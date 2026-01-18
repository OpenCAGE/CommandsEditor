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
        private Dictionary<ListViewItem, MaterialMappings.MaterialMapping.Mapping> _listItemToMapping = new Dictionary<ListViewItem, MaterialMappings.MaterialMapping.Mapping>();

        public EditMaterialMapping(MaterialMappings.MaterialMapping currentMapping = null, bool showSelectBtn = true) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();
            _currentMapping = currentMapping;

            selectButton.Visible = showSelectBtn;
            treeHelper = new TreeUtility(materialMappingTreeView, false);

            PopulateTreeView();

            this.Disposed += EditMaterialMapping_Disposed;
        }

        private void EditMaterialMapping_Disposed(object sender, EventArgs e)
        {
            treeHelper?.ForceClearTree();
            treeHelper = null;
            _indexToMapping.Clear();
            _listItemToMapping.Clear();
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

            _listItemToMapping.Clear();
            foreach (var mapping in selectedMapping.Mappings)
            {
                ListViewItem item = new ListViewItem(mapping.from ?? "");
                item.SubItems.Add(mapping.to ?? "");
                mappingsListView.Items.Add(item);
                _listItemToMapping[item] = mapping;
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

        private void addNewSetButton_Click(object sender, EventArgs e)
        {
            string name = ShowInputDialog("Add New Material Mapping Set", "Name");
            if (string.IsNullOrEmpty(name))
                return;

            if (Content.Level.MaterialMaps.Entries.Any(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("A material mapping set with this name already exists.", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newMapping = new MaterialMappings.MaterialMapping
            {
                Name = name,
                Mappings = new List<MaterialMappings.MaterialMapping.Mapping>()
            };

            Content.Level.MaterialMaps.Entries.Add(newMapping);
            Content.Level.MaterialMaps.Save();
            PopulateTreeView();
            
            int index = Content.Level.MaterialMaps.Entries.IndexOf(newMapping);
            if (index >= 0)
            {
                string filePath = newMapping.Name.Replace('\\', '/');
                if (filePath.Length > 0 && filePath[0] == '/')
                    filePath = filePath.Substring(1);
                treeHelper.SelectNode(filePath);
            }
        }

        private void addMappingButton_Click(object sender, EventArgs e)
        {
            if (materialMappingTreeView.SelectedNode == null)
            {
                MessageBox.Show("Please select a material mapping set first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TreeItem treeItem = (TreeItem)materialMappingTreeView.SelectedNode.Tag;
            if (treeItem.Item_Type != TreeItemType.EXPORTABLE_FILE)
            {
                MessageBox.Show("Please select a material mapping set first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!int.TryParse(treeItem.String_Value, out int index) || !_indexToMapping.ContainsKey(index))
                return;

            MaterialMappings.MaterialMapping selectedMapping = _indexToMapping[index];
            if (selectedMapping == null)
                return;

            var result = ShowTwoInputDialog("Add Mapping", "From", "To");
            if (result == null)
                return;

            var newMapping = new MaterialMappings.MaterialMapping.Mapping
            {
                from = result.Item1,
                to = result.Item2
            };

            selectedMapping.Mappings.Add(newMapping);
            Content.Level.MaterialMaps.Save();
            UpdateMappingsPanel();
        }

        private void removeMappingButton_Click(object sender, EventArgs e)
        {
            if (mappingsListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a mapping to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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

            ListViewItem selectedItem = mappingsListView.SelectedItems[0];
            if (_listItemToMapping.ContainsKey(selectedItem))
            {
                var mapping = _listItemToMapping[selectedItem];
                selectedMapping.Mappings.Remove(mapping);
                Content.Level.MaterialMaps.Save();
                UpdateMappingsPanel();
            }
        }

        private void mappingsListView_DoubleClick(object sender, EventArgs e)
        {
            if (mappingsListView.SelectedItems.Count == 0)
                return;

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

            ListViewItem selectedItem = mappingsListView.SelectedItems[0];
            if (!_listItemToMapping.ContainsKey(selectedItem))
                return;

            var mapping = _listItemToMapping[selectedItem];

            var result = ShowTwoInputDialog("Edit Mapping", "From", "To", mapping.from ?? "", mapping.to ?? "");
            if (result == null)
                return;

            mapping.from = result.Item1;
            mapping.to = result.Item2;
            Content.Level.MaterialMaps.Save();
            UpdateMappingsPanel();
        }

        private void mappingsListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && mappingsListView.SelectedItems.Count > 0)
            {
                removeMappingButton_Click(sender, e);
            }
        }

        private string ShowInputDialog(string title, string labelText, string initialValue = "")
        {
            using (var dialog = new Form())
            {
                dialog.Text = title;
                dialog.Size = new System.Drawing.Size(400, 120);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                var label = new Label() { Text = labelText + ":", Left = 10, Top = 15, Width = 70 };
                var textBox = new TextBox() { Left = 90, Top = 12, Width = 280, Text = initialValue };
                var okButton = new Button() { Text = "OK", Left = 220, Top = 45, Width = 70, DialogResult = DialogResult.OK };
                var cancelButton = new Button() { Text = "Cancel", Left = 300, Top = 45, Width = 70, DialogResult = DialogResult.Cancel };

                dialog.Controls.Add(label);
                dialog.Controls.Add(textBox);
                dialog.Controls.Add(okButton);
                dialog.Controls.Add(cancelButton);
                dialog.AcceptButton = okButton;
                dialog.CancelButton = cancelButton;

                textBox.Select();
                textBox.SelectAll();

                if (dialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return textBox.Text.Trim();
                }
            }
            return null;
        }

        private Tuple<string, string> ShowTwoInputDialog(string title, string label1, string label2, string initialValue1 = "", string initialValue2 = "")
        {
            using (var dialog = new Form())
            {
                dialog.Text = title;
                dialog.Size = new System.Drawing.Size(450, 150);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                var labelFrom = new Label() { Text = label1 + ":", Left = 10, Top = 15, Width = 70 };
                var textBoxFrom = new TextBox() { Left = 90, Top = 12, Width = 330, Text = initialValue1 };
                var labelTo = new Label() { Text = label2 + ":", Left = 10, Top = 45, Width = 70 };
                var textBoxTo = new TextBox() { Left = 90, Top = 42, Width = 330, Text = initialValue2 };
                var okButton = new Button() { Text = "OK", Left = 270, Top = 75, Width = 70, DialogResult = DialogResult.OK };
                var cancelButton = new Button() { Text = "Cancel", Left = 350, Top = 75, Width = 70, DialogResult = DialogResult.Cancel };

                dialog.Controls.Add(labelFrom);
                dialog.Controls.Add(textBoxFrom);
                dialog.Controls.Add(labelTo);
                dialog.Controls.Add(textBoxTo);
                dialog.Controls.Add(okButton);
                dialog.Controls.Add(cancelButton);
                dialog.AcceptButton = okButton;
                dialog.CancelButton = cancelButton;

                textBoxFrom.Select();
                if (!string.IsNullOrEmpty(initialValue1))
                    textBoxFrom.SelectAll();

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    return new Tuple<string, string>(textBoxFrom.Text.Trim(), textBoxTo.Text.Trim());
                }
            }
            return null;
        }
    }
}

