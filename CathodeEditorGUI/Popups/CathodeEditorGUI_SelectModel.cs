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

        public CathodeEditorGUI_SelectModel(int defaultModelIndex = -1)
        {
            InitializeComponent();

            treeHelper = new TreeUtility(FileTree, true);
            List<string> allModelFileNames = new List<string>();
            List<string> allModelTagsNames = new List<string>();

            for (int i = 0; i < CurrentInstance.modelDB.Models.Count; i++)
            {
                //We always just pull the name of the first submesh as they'll all be the same (or at least should be)
                int binIndex = CurrentInstance.modelDB.Models[i].Submeshes[0].binIndex;
                if (CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex] == "")
                    allModelFileNames.Add(CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex]);
                else
                    allModelFileNames.Add(CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex] + "/" + CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex]);
                allModelTagsNames.Add(i.ToString());
            }
            treeHelper.UpdateFileTree(allModelFileNames, null, allModelTagsNames);

            modelViewer = new GUI_ModelViewer();
            modelRendererHost.Child = modelViewer;

            if (defaultModelIndex != -1) 
                ShowModel(defaultModelIndex);
        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (((TreeItem)FileTree.SelectedNode.Tag).Item_Type != TreeItemType.EXPORTABLE_FILE) return;

            int index = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);
            ShowModel(index);
        }

        private void ShowModel(int index)
        {
            //This renders all submeshes within the model index!!
            modelViewer.ShowModel(index);
            int binIndex = CurrentInstance.modelDB.Models[index].Submeshes[0].binIndex;
            modelPreviewArea.Text = CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex];
            if (CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex] != "")
                modelPreviewArea.Text += " -> [" + CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex] + "]";
        }

        private void selectModel_Click(object sender, EventArgs e)
        {

        }
    }
}
