using CATHODE.Assets;
using CATHODE.LEGACY;
using CATHODE.Misc;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CATHODE.LEGACY.CathodeModels;

namespace CathodeEditorGUI.Popups.UserControls
{
    /// <summary>
    /// Interaction logic for GUI_ModelViewer.xaml
    /// </summary>
    public partial class GUI_ModelViewer : UserControl
    {
        CS2Reader reader = null;

        public GUI_ModelViewer()
        {
            InitializeComponent();
            reader = new CS2Reader();
        }

        public void ShowModel(int modelIndex)
        {
            modelPreview.Content = reader.Read(modelIndex);
        }
        
    }
}
