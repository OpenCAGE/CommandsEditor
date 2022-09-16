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

        public CathodeEditorGUI_SelectModel()
        {
            InitializeComponent();

            string baseLevelPath = (CurrentInstance.commandsPAK == null) ? @"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\BSP_TORRENS\"
                                                                         : CurrentInstance.commandsPAK.Filepath.Substring(0, CurrentInstance.commandsPAK.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);

            CurrentInstance.modelDB = new CathodeModels(baseLevelPath + "RENDERABLE/MODELS_LEVEL.BIN",
                                                        baseLevelPath + "RENDERABLE/LEVEL_MODELS.PAK");
            CurrentInstance.materialDB = new MaterialDatabase(baseLevelPath + "RENDERABLE/LEVEL_MODELS.MTL");
            CurrentInstance.textureDB = new CathodeTextures(baseLevelPath + "RENDERABLE/LEVEL_TEXTURES.ALL.PAK",
                                                            baseLevelPath + "RENDERABLE/LEVEL_TEXTURE_HEADERS.ALL.BIN");

            treeHelper = new TreeUtility(FileTree, true);
            List<string> allModelFileNames = new List<string>();
            List<string> allModelTagsNames = new List<string>();
            for (int i = 0; i < CurrentInstance.modelDB.Models.Count; i++)
            {
                int binIndex = CurrentInstance.modelDB.Models[i].Submeshes[0].binIndex;
                allModelFileNames.Add(CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex] + "/" + CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex]);
                allModelTagsNames.Add(i.ToString());
            }
            treeHelper.UpdateFileTree(allModelFileNames, null, allModelTagsNames);

            modelViewer = new GUI_ModelViewer();
            modelRendererHost.Child = modelViewer;
        }

        private void FileTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            int index = Convert.ToInt32(((TreeItem)FileTree.SelectedNode.Tag).String_Value);

            modelViewer.ShowModel(index);
            int binIndex = CurrentInstance.modelDB.Models[index].Submeshes[0].binIndex;
            label1.Text = CurrentInstance.modelDB.modelBIN.ModelFilePaths[binIndex];
            label2.Text = CurrentInstance.modelDB.modelBIN.ModelLODPartNames[binIndex];
        }
    }
}
