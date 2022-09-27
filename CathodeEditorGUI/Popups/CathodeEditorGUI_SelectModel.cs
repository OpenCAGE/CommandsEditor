using CATHODE.LEGACY;
using CATHODE.Misc;
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

            for (int i = 0; i < CurrentInstance.modelDB.Models.Count; i++)
            {
                //We always just pull the name of the first submesh as they'll all be the same
                allModelFileNames.Add(GenerateNodeTag(i));
                allModelTagsNames.Add(i.ToString());
            }
            treeHelper.UpdateFileTree(allModelFileNames, null, allModelTagsNames);

            modelViewer = new GUI_ModelViewer();
            modelRendererHost.Child = modelViewer;

            if (defaultModelIndex != -1)
                SelectModelNode(defaultModelIndex);
        }

        private string GenerateNodeTag(int pakIndex)
        {
            //We always just pull the name of the first submesh as they'll all be the same
            int binIndex = CurrentInstance.modelDB.Models[pakIndex].Submeshes[0].binIndex;
            if (CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex] == "")
                return CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex];
            else
                return CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex] + "/" + CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex];
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

        private void ShowModel(int pakIndex)
        {
            //This renders all submeshes within the model index!!
            modelViewer.ShowModel(pakIndex);
            int binIndex = CurrentInstance.modelDB.Models[pakIndex].Submeshes[0].binIndex;
            modelPreviewArea.Text = CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex];
            if (CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex] != "")
                modelPreviewArea.Text += " -> [" + CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex] + "]";
        }

        private void selectModel_Click(object sender, EventArgs e)
        {
            SelectedModelIndex = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            SelectedModelMaterialIndexes.Clear();
            for (int i = 0; i < CurrentInstance.modelDB.Models[SelectedModelIndex].Header.SubmeshCount; i++)
                SelectedModelMaterialIndexes.Add(CurrentInstance.modelDB.modelBIN.Models[CurrentInstance.modelDB.Models[SelectedModelIndex].Submeshes[i].binIndex].MaterialLibraryIndex);
            this.Close();
        }
    }
}
