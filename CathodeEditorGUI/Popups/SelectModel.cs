using AlienPAK;
using CATHODE;
using CATHODE.LEGACY;
using CommandsEditor.Popups.Base;
using CommandsEditor.Popups.UserControls;
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
    //TODO: The logic here is really crap. Need to bring over the new improvements made for AlienPAK.


    public partial class SelectModel : BaseWindow
    {
        GUI_ModelViewer modelViewer = null;
        TreeUtility treeHelper;

        public int SelectedModelIndex = -1;
        public List<int> SelectedModelMaterialIndexes = new List<int>();

        public SelectModel(int defaultModelIndex = -1) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

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

            string test = ((TreeItem)FileTree.SelectedNode.Tag).String_Value;
            int index = Convert.ToInt32(test);
            ShowModel(index);
        }

        private void ShowModel(int i)
        {
            List<GUI_ModelViewer.Model> models = new List<GUI_ModelViewer.Model>();
            Models.CS2.Component component = Content.resource.models.FindModelComponentForSubmesh(Content.resource.models.GetAtWriteIndex(i));
            for (int x = 0; x < component.LODs.Count; x++)
                for (int y = 0; y < component.LODs[x].Submeshes.Count; y++)
                    models.Add(new GUI_ModelViewer.Model(Content.resource.models.GetWriteIndex(component.LODs[x].Submeshes[y])));
            modelViewer.ShowModel(models);
            modelPreviewArea.Text = GenerateNodeTag(i);
        }

        private void selectModel_Click(object sender, EventArgs e)
        {
            SelectedModelIndex = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            SelectedModelMaterialIndexes.Clear();

            Models.CS2.Component component = Content.resource.models.FindModelComponentForSubmesh(Content.resource.models.GetAtWriteIndex(SelectedModelIndex));
            for (int x = 0; x < component.LODs.Count; x++)
                for (int i = 0; i < component.LODs[x].Submeshes.Count; i++)
                    SelectedModelMaterialIndexes.Add(component.LODs[x].Submeshes[i].MaterialLibraryIndex);
            
            this.Close();
        }
    }
}
