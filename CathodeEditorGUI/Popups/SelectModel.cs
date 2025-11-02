using AlienPAK;
using CATHODE;
using CATHODE.LEGACY;
using CommandsEditor.Popups.Base;
using CommandsEditor.Popups.UserControls;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class SelectModel : BaseWindow
    {
        GUI_ModelViewer modelViewer = null;
        TreeUtility treeHelper;

        public int SelectedModelIndex = -1;
        public List<int> SelectedModelMaterialIndexes = new List<int>();

        private Dictionary<int, GUI_ModelViewer.Model> allSubmeshes = new Dictionary<int, GUI_ModelViewer.Model>();
        private List<CheckBox> submeshCheckboxes = new List<CheckBox>();
        private Dictionary<CheckBox, int> checkboxToModelIndex = new Dictionary<CheckBox, int>();
        private Dictionary<int, List<CheckBox>> lodToCheckboxes = new Dictionary<int, List<CheckBox>>();
        private Dictionary<int, GroupBox> lodGroups = new Dictionary<int, GroupBox>();

        public SelectModel(int defaultModelIndex = -1) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            splitContainer2.FixedPanel = FixedPanel.Panel2;
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Resize += SplitContainer2_Resize;
            UpdateFilterPanelWidth();

            useMaterials.Checked = SettingsManager.GetBool(Singleton.Settings.ShowTexOpt);

            treeHelper = new TreeUtility(FileTree, true);
            List<string> allModelFileNames = new List<string>();
            List<string> allModelTagsNames = new List<string>();

            foreach (Models.CS2 mesh in Content.resource.models.Entries)
            {
                foreach (Models.CS2.Component component in mesh.Components)
                {
                    foreach (Models.CS2.Component.LOD lod in component.LODs)
                    {
                        foreach (Models.CS2.Component.LOD.Submesh submesh in lod.Submeshes)
                        {
                            if (lod.Name == "") allModelFileNames.Add(mesh.Name.Replace('\\', '/'));
                            else allModelFileNames.Add(mesh.Name.Replace('\\', '/') + "/" + lod.Name.Replace('\\', '/'));

                            allModelTagsNames.Add(Content.resource.models.GetWriteIndex(submesh).ToString());
                        }
                    }
                }
            }
            treeHelper.UpdateFileTree(allModelFileNames, null, allModelTagsNames);

            modelViewer = new GUI_ModelViewer();
            modelRendererHost.Child = modelViewer;

            if (defaultModelIndex != -1)
                SelectModelNode(defaultModelIndex);

            this.Disposed += SelectModel_Disposed;
        }

        private void SelectModel_Disposed(object sender, EventArgs e)
        {
            ClearSubmeshCheckboxes();
            allSubmeshes.Clear();
            
            treeHelper?.ForceClearTree();
            treeHelper = null;

            modelViewer = null;

            if (modelRendererHost != null)
                modelRendererHost.Dispose();
        }

        private string GenerateNodeTag(int i)
        {
            Models.CS2.Component.LOD.Submesh submesh = Content.resource.models.GetAtWriteIndex(i);
            Models.CS2.Component.LOD lod = Content.resource.models.FindModelLODForSubmesh(submesh);
            //Models.CS2.Component component = Editor.resource.models.FindModelComponentForSubmesh(submesh);
            Models.CS2 mesh = Content.resource.models.FindModelForSubmesh(submesh);

            if (mesh == null || submesh == null) return ""; //we currently skip empty submeshes, e.g. ballistics

            if (lod.Name == "")
                return mesh.Name.Replace('\\', '/');
            else
                return mesh.Name.Replace('\\', '/') + "/" + lod.Name.Replace('\\', '/');
        }

        private void SelectModelNode(int pakIndex)
        {
            string thisTag = GenerateNodeTag(pakIndex);
            treeHelper.SelectNode(thisTag);
        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (((TreeItem)FileTree.SelectedNode.Tag).Item_Type != TreeItemType.EXPORTABLE_FILE) return;
            SelectedModelIndex = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            ShowModel(SelectedModelIndex);
        }

        private void ShowModel(int i)
        {
            if (i == -1)
                return;

            ClearSubmeshCheckboxes();

            allSubmeshes.Clear();
            Models.CS2.Component component = Content.resource.models.FindModelComponentForSubmesh(Content.resource.models.GetAtWriteIndex(i));
            
            int highestLODIndex = 0;
            if (component.LODs.Count > 0)
                highestLODIndex = 0; 
            
            for (int x = 0; x < component.LODs.Count; x++)
                CreateLODGroup(x, component.LODs[x].Name);
            
            for (int x = 0; x < component.LODs.Count; x++)
            {
                int yOffset = 0;
                for (int y = 0; y < component.LODs[x].Submeshes.Count; y++)
                {
                    int modelIndex = Content.resource.models.GetWriteIndex(component.LODs[x].Submeshes[y]);
                    allSubmeshes[modelIndex] = new GUI_ModelViewer.Model(modelIndex);
                    
                    bool isEnabled = (x == highestLODIndex);
                    CreateSubmeshCheckbox(modelIndex, x, y, component.LODs[x].Submeshes.Count, isEnabled, yOffset);
                    yOffset += 25;
                }
            }

            UpdateLODGroupLayouts();
            UpdateFilteredModel(true); 
            modelPreviewArea.Text = GenerateNodeTag(i);

            Debug.Log("Model Viewer", "Showing model at index " + i);
        }

        private void ClearSubmeshCheckboxes()
        {
            foreach (CheckBox cb in submeshCheckboxes)
            {
                cb.CheckedChanged -= SubmeshCheckbox_CheckedChanged;
                checkboxToModelIndex.Remove(cb);
                cb.Dispose();
            }
            submeshCheckboxes.Clear();
            checkboxToModelIndex.Clear();
            
            foreach (var lodGroup in lodGroups.Values)
            {
                lodGroup.Dispose();
            }
            lodGroups.Clear();
            lodToCheckboxes.Clear();
            submeshFilterPanel.Controls.Clear();
        }

        private void CreateLODGroup(int lodIndex, string lodName)
        {
            GroupBox lodGroup = new GroupBox();
            lodGroup.Text = string.IsNullOrEmpty(lodName) ? $"LOD {lodIndex}" : $"LOD {lodIndex} - {lodName}";
            lodGroup.AutoSize = false;
            lodGroup.Width = 185;
            
            Button selectAllBtn = new Button();
            selectAllBtn.Text = "Select All";
            selectAllBtn.Size = new Size(80, 23);
            selectAllBtn.Location = new Point(5, 15);
            selectAllBtn.Tag = lodIndex;
            selectAllBtn.Click += (s, e) => {
                int lod = (int)((Button)s).Tag;
                SetLODCheckboxesState(lod, true);
            };
            
            Button deselectAllBtn = new Button();
            deselectAllBtn.Text = "Deselect All";
            deselectAllBtn.Size = new Size(80, 23);
            deselectAllBtn.Location = new Point(90, 15);
            deselectAllBtn.Tag = lodIndex;
            deselectAllBtn.Click += (s, e) => {
                int lod = (int)((Button)s).Tag;
                SetLODCheckboxesState(lod, false);
            };
            
            lodGroup.Controls.Add(selectAllBtn);
            lodGroup.Controls.Add(deselectAllBtn);
            
            lodGroups[lodIndex] = lodGroup;
            lodToCheckboxes[lodIndex] = new List<CheckBox>();
        }

        private void SetLODCheckboxesState(int lodIndex, bool state)
        {
            if (lodToCheckboxes.ContainsKey(lodIndex))
            {
                foreach (CheckBox cb in lodToCheckboxes[lodIndex])
                    cb.CheckedChanged -= SubmeshCheckbox_CheckedChanged;
                foreach (CheckBox cb in lodToCheckboxes[lodIndex])
                    cb.Checked = state;
                foreach (CheckBox cb in lodToCheckboxes[lodIndex])
                    cb.CheckedChanged += SubmeshCheckbox_CheckedChanged;
                
                UpdateFilteredModel();
            }
        }

        private void CreateSubmeshCheckbox(int modelIndex, int lodIndex, int submeshIndex, int totalSubmeshes, bool isChecked, int yOffset)
        {
            CheckBox checkbox = new CheckBox();
            checkbox.AutoSize = true;
            checkbox.Checked = isChecked;
            
            GroupBox lodGroup = lodGroups[lodIndex];
            checkbox.Location = new Point(10, 45 + yOffset);
            checkbox.Text = $"Submesh {submeshIndex} (Index {modelIndex})";
            checkbox.Tag = modelIndex;
            
            checkbox.CheckedChanged += SubmeshCheckbox_CheckedChanged;
            
            submeshCheckboxes.Add(checkbox);
            checkboxToModelIndex[checkbox] = modelIndex;
            lodToCheckboxes[lodIndex].Add(checkbox);
            lodGroup.Controls.Add(checkbox);
        }

        private void UpdateLODGroupLayouts()
        {
            int currentYPos = 5;
            foreach (var kvp in lodGroups.OrderBy(x => x.Key))
            {
                int lodIndex = kvp.Key;
                GroupBox lodGroup = kvp.Value;
                
                int checkboxCount = lodToCheckboxes[lodIndex].Count;
                int groupHeight = 45 + (checkboxCount * 25) + 5; 
                lodGroup.Height = groupHeight;
                lodGroup.Location = new Point(5, currentYPos);
                
                currentYPos += groupHeight + 5; 
                
                submeshFilterPanel.Controls.Add(lodGroup);
            }
        }

        private void SubmeshCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilteredModel();
        }

        private void UpdateFilteredModel()
        {
            UpdateFilteredModel(false);
        }

        private void UpdateFilteredModel(bool zoomExtents)
        {
            if (allSubmeshes.Count == 0)
                return;

            List<GUI_ModelViewer.Model> filteredModels = new List<GUI_ModelViewer.Model>();
            
            foreach (CheckBox cb in submeshCheckboxes)
            {
                if (cb.Checked && checkboxToModelIndex.ContainsKey(cb))
                {
                    int modelIndex = checkboxToModelIndex[cb];
                    if (allSubmeshes.ContainsKey(modelIndex))
                        filteredModels.Add(allSubmeshes[modelIndex]);
                }
            }

            modelViewer.ShowModel(filteredModels, zoomExtents);
        }

        private void selectModel_Click(object sender, EventArgs e)
        {
            SelectedModelIndex = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            SelectedModelMaterialIndexes.Clear();

            Models.CS2.Component component = Content.resource.models.FindModelComponentForSubmesh(Content.resource.models.GetAtWriteIndex(SelectedModelIndex));
            for (int x = 0; x < component.LODs.Count; x++)
                for (int i = 0; i < component.LODs[x].Submeshes.Count; i++)
                    SelectedModelMaterialIndexes.Add(component.LODs[x].Submeshes[i].MaterialIndex);
            
            this.Close();
        }

        private void useMaterials_CheckedChanged(object sender, EventArgs e)
        {
            SettingsManager.SetBool(Singleton.Settings.ShowTexOpt, useMaterials.Checked);
            UpdateFilteredModel();
        }

        private void SplitContainer2_Resize(object sender, EventArgs e)
        {
            UpdateFilterPanelWidth();
        }

        private void UpdateFilterPanelWidth()
        {
            const int filterPanelWidth = 220;
            const int splitterWidth = 5;
            
            if (splitContainer2.Width > filterPanelWidth + splitterWidth)
            {
                splitContainer2.SplitterDistance = splitContainer2.Width - filterPanelWidth - splitterWidth;
            }
        }
    }
}
