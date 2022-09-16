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
        CathodeModels ModelsPAK = null;

        int currentIndex = 100;

        public CathodeEditorGUI_SelectModel()
        {
            InitializeComponent();

            ModelsPAK = new CathodeModels(@"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\HAB_AIRPORT\RENDERABLE\MODELS_LEVEL.BIN",
                                          @"G:\SteamLibrary\steamapps\common\Alien Isolation\DATA\ENV\PRODUCTION\HAB_AIRPORT\RENDERABLE\LEVEL_MODELS.PAK");

            modelViewer = new GUI_ModelViewer();
            modelRendererHost.Child = modelViewer;

            modelViewer.ShowModel(ModelsPAK, currentIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentIndex++;
            modelViewer.ShowModel(ModelsPAK, currentIndex);
        }
    }
}
