using CATHODE;
using CATHODE.LEGACY;
using CathodeEditorGUI.Popups.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_SelectModel : Form
    {
        GUI_ModelViewer modelViewer = null;
        TreeUtility treeHelper;

        public int SelectedModelIndex = -1;
        public List<int> SelectedModelMaterialIndexes = new List<int>();

        public CathodeEditorGUI_SelectModel(int defaultModelIndex = -1)
        {
            InitializeComponent();

            treeHelper = new TreeUtility(FileTree, true);
            List<string> allModelFileNames = new List<string>();
            List<string> allModelTagsNames = new List<string>();

            int i = 0;
            while (Editor.resource.models.GetAtWriteIndex(i) != null)
            {
                allModelFileNames.Add(GenerateNodeTag(i));
                allModelTagsNames.Add(i.ToString());
                i++;
            }
            treeHelper.UpdateFileTree(allModelFileNames, null, allModelTagsNames);

            modelViewer = new GUI_ModelViewer();
            modelRendererHost.Child = modelViewer;

            if (defaultModelIndex != -1)
                SelectModelNode(defaultModelIndex);
        }

        private string GenerateNodeTag(int i)
        {
            //TODO: submesh names are not necessarily always the same
            Models.CS2.Submesh submesh = Editor.resource.models.GetAtWriteIndex(i);
            Models.CS2 mesh = Editor.resource.models.FindModelForSubmesh(submesh);
            if (mesh == null || submesh == null) return ""; //TODO: we currently skip empty submeshes, e.g. ballistics
            if (submesh.Name == "")
                return mesh.Name.Replace('\\', '/');
            else
                return mesh.Name.Replace('\\', '/') + "/" + submesh.Name.Replace('\\', '/');
        }

        private void SelectModelNode(int pakIndex)
        {
            string thisTag = GenerateNodeTag(pakIndex);
            treeHelper.SelectNode(thisTag);
        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (((TreeItem)FileTree.SelectedNode.Tag).Item_Type != TreeItemType.EXPORTABLE_FILE) return;

            int index = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            ShowModel(index);
        }

        private void ShowModel(int i)
        {
            List<GUI_ModelViewer.Model> models = new List<GUI_ModelViewer.Model>();
            models.Add(new GUI_ModelViewer.Model(i));
            modelViewer.ShowModel(models);
            modelPreviewArea.Text = GenerateNodeTag(i);
        }

        private void selectModel_Click(object sender, EventArgs e)
        {
            SelectedModelIndex = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            SelectedModelMaterialIndexes.Clear();

            //TODO TODO TODO URGENT! THIS CANNOT WORK WITH OUR NEW MODEL PARSER

            //for (int i = 0; i < Editor.resource.models.Entries[SelectedModelIndex].Header.SubmeshCount; i++)
            //    SelectedModelMaterialIndexes.Add(Editor.resource.models.modelBIN.Models[Editor.resource.models.Models[SelectedModelIndex].Submeshes[i].binIndex].MaterialLibraryIndex);
            this.Close();
        }
    }
}
