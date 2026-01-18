using AlienPAK;
using CATHODE;
using CathodeLib;
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
    public partial class EditModel : BaseWindow
    {
        GUI_ModelViewer modelViewer = null;
        TreeUtility treeHelper;

        private Dictionary<Models.CS2.Component.LOD.Submesh, GUI_ModelViewer.Model> allSubmeshes = new Dictionary<Models.CS2.Component.LOD.Submesh, GUI_ModelViewer.Model>();
        private List<CheckBox> submeshCheckboxes = new List<CheckBox>();
        private Dictionary<CheckBox, Models.CS2.Component.LOD.Submesh> checkboxToModelIndex = new Dictionary<CheckBox, Models.CS2.Component.LOD.Submesh>();
        private Dictionary<int, List<CheckBox>> lodToCheckboxes = new Dictionary<int, List<CheckBox>>();
        private Dictionary<int, GroupBox> lodGroups = new Dictionary<int, GroupBox>();

        public Action<Models.CS2.Component> OnModelSelected;

        public EditModel(Models.CS2.Component.LOD.Submesh defaultSubmesh = null, bool showSelectBtn = true) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            splitContainer2.FixedPanel = FixedPanel.Panel2;
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Resize += SplitContainer2_Resize;
            UpdateFilterPanelWidth();

            useMaterials.Checked = SettingsManager.GetBool(Singleton.Settings.ShowTexOpt);

            treeHelper = new TreeUtility(FileTree, true);
            {
                List<string> allModelFileNames = new List<string>();
                List<string> allModelTagsNames = new List<string>();
                foreach (Models.CS2 mesh in Content.Level.Models.Entries)
                {
                    foreach (Models.CS2.Component component in mesh.Components)
                    {
                        if (component.LODs.Count == 0)
                            continue;

                        Models.CS2.Component.LOD lod0 = component.LODs[0];

                        if (lod0.Submeshes.Count == 0)
                            continue;

                        Models.CS2.Component.LOD.Submesh submesh0 = lod0.Submeshes[0];
                        allModelFileNames.Add(CreateTagForMesh(mesh, submesh0, lod0, component));
                        allModelTagsNames.Add(Content.Level.Models.GetWriteIndex(submesh0).ToString());
                    }
                }
                treeHelper.UpdateFileTree(allModelFileNames, null, allModelTagsNames);
            }

            modelViewer = new GUI_ModelViewer();
            modelRendererHost.Child = modelViewer;

            if (defaultSubmesh != null)
                SelectModelNode(Content.Level.Models.GetWriteIndex(defaultSubmesh));
            
            selectModelBtn.Visible = showSelectBtn;

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
            Models.CS2.Component.LOD.Submesh submesh = Content.Level.Models.GetAtWriteIndex(i);
            Models.CS2.Component.LOD lod = Content.Level.Models.FindModelLODForSubmesh(submesh);
            Models.CS2.Component component = Content.Level.Models.FindModelComponentForSubmesh(submesh);
            Models.CS2 mesh = Content.Level.Models.FindModelForSubmesh(submesh);

            if (mesh == null || submesh == null) return ""; //we currently skip empty submeshes, e.g. ballistics

            return CreateTagForMesh(mesh, submesh, lod, component);
        }

        private string CreateTagForMesh(Models.CS2 cs2, Models.CS2.Component.LOD.Submesh submesh, Models.CS2.Component.LOD lod, Models.CS2.Component component)
        {
            string tag = cs2.Name.Replace('\\', '/') + "/[" + cs2.Components.IndexOf(component).ToString("00") + "] " + lod.Name.Replace('\\', '/');
            if (tag.Length > 0 && tag[0] == '/')
                tag = tag.Substring(1);
            return tag;
        }

        private void SelectModelNode(int pakIndex)
        {
            string thisTag = GenerateNodeTag(pakIndex);
            treeHelper.SelectNode(thisTag);
        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearSubmeshCheckboxes();
            selectModelBtn.Enabled = false;
            allSubmeshes.Clear();
            modelPreviewArea.Text = "";
            modelViewer.ShowModel(new List<GUI_ModelViewer.Model>());

            int selectedModelIndex = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            if (selectedModelIndex == -1)
                return;

            switch (((TreeItem)FileTree.SelectedNode.Tag).Item_Type)
            {
                case TreeItemType.EXPORTABLE_FILE:
                    {
                        //Shows an individual model component, which can be selected to be used as a RENDERABLE resource
                        Models.CS2.Component component = Content.Level.Models.FindModelComponentForSubmesh(Content.Level.Models.GetAtWriteIndex(selectedModelIndex));
                        AddComponent(component);
                        modelPreviewArea.Text = GenerateNodeTag(selectedModelIndex);
                        selectModelBtn.Enabled = true;
                    }
                    break;
                case TreeItemType.DIRECTORY:
                    {
                        if (!(FileTree.SelectedNode.Nodes.Count > 0 && FileTree.SelectedNode.Nodes[0].Nodes.Count == 0))
                            return;

                        //Shows a combined model made up of multiple components, cannot be selected as a RENDERABLE resource
                        Models.CS2.Component.LOD.Submesh submesh = Content.Level.Models.GetAtWriteIndex(selectedModelIndex);
                        Models.CS2 mesh = Content.Level.Models.FindModelForSubmesh(submesh);
                        int index = 0;
                        foreach (Models.CS2.Component component in mesh.Components)
                        {
                            AddComponent(component, index);
                            index += component.LODs.Count;
                        }
                        modelPreviewArea.Text = mesh.Name.Replace('\\', '/');
                    }
                    break;
                default:
                    return;
            }

            UpdateFilteredModel(true);
            UpdateLODGroupLayouts();

            Debug.Log("Model Viewer", "Showing from index " + selectedModelIndex);
        }

        private void AddComponent(Models.CS2.Component component, int baseIndex = 0)
        {
            for (int x = baseIndex; x < component.LODs.Count + baseIndex; x++)
                CreateLODGroup(x, component.LODs[x - baseIndex].Name);

            for (int x = 0; x < component.LODs.Count; x++)
            {
                int yOffset = 0;
                for (int y = 0; y < component.LODs[x].Submeshes.Count; y++)
                {
                    allSubmeshes[component.LODs[x].Submeshes[y]] = new GUI_ModelViewer.Model(component.LODs[x].Submeshes[y]);

                    bool isEnabled = (x == 0);
                    CreateSubmeshCheckbox(component.LODs[x].Submeshes[y], x + baseIndex, y, component.LODs[x].Submeshes.Count, isEnabled, yOffset);
                    yOffset += 25;
                }
            }
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
            lodGroup.Text = lodName;
            lodGroup.AutoSize = false;
            lodGroup.Width = 207;
            
            Button selectAllBtn = new Button();
            selectAllBtn.Text = "Show All";
            selectAllBtn.Size = new Size(80, 23);
            selectAllBtn.Location = new Point(5, 15);
            selectAllBtn.Tag = lodIndex;
            selectAllBtn.Click += (s, e) => {
                int lod = (int)((Button)s).Tag;
                CheckAllSubmeshes(lod, true);
            };
            lodGroup.Controls.Add(selectAllBtn);

            Button deselectAllBtn = new Button();
            deselectAllBtn.Text = "Hide All";
            deselectAllBtn.Size = new Size(80, 23);
            deselectAllBtn.Location = new Point(90, 15);
            deselectAllBtn.Tag = lodIndex;
            deselectAllBtn.Click += (s, e) => {
                int lod = (int)((Button)s).Tag;
                CheckAllSubmeshes(lod, false);
            };
            lodGroup.Controls.Add(deselectAllBtn);

            lodGroups[lodIndex] = lodGroup;
            lodToCheckboxes[lodIndex] = new List<CheckBox>();
        }

        private void UncheckAllLODs()
        {
            foreach (KeyValuePair<int, List<CheckBox>> kvp in lodToCheckboxes)
            {
                foreach (CheckBox cb in kvp.Value)
                    cb.CheckedChanged -= SubmeshCheckbox_CheckedChanged;
                foreach (CheckBox cb in kvp.Value)
                    cb.Checked = false;
                foreach (CheckBox cb in kvp.Value)
                    cb.CheckedChanged += SubmeshCheckbox_CheckedChanged;
            }
            UpdateFilteredModel();
        }

        private void CheckAllSubmeshes(int lodIndex, bool state)
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

        private void CreateSubmeshCheckbox(Models.CS2.Component.LOD.Submesh model, int lodIndex, int submeshIndex, int totalSubmeshes, bool isChecked, int yOffset)
        {
            CheckBox checkbox = new CheckBox();
            checkbox.AutoSize = true;
            checkbox.Checked = isChecked;
            
            GroupBox lodGroup = lodGroups[lodIndex];
            checkbox.Location = new Point(10, 45 + yOffset);
            checkbox.Text = "Submesh " + submeshIndex + " (" + model.VertexCount + " verts)";
            checkbox.Tag = model;
            
            checkbox.CheckedChanged += SubmeshCheckbox_CheckedChanged;
            
            submeshCheckboxes.Add(checkbox);
            checkboxToModelIndex[checkbox] = model;
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
                    if (allSubmeshes.TryGetValue(checkboxToModelIndex[cb], out GUI_ModelViewer.Model model))
                        filteredModels.Add(model);
                }
            }

            modelViewer.ShowModel(filteredModels, zoomExtents);
        }

        private void selectModel_Click(object sender, EventArgs e)
        {
            int selectedModelIndex = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            OnModelSelected?.Invoke(Content.Level.Models.FindModelComponentForSubmesh(Content.Level.Models.GetAtWriteIndex(selectedModelIndex)));
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
