using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Drawing.Imaging;
using CATHODE;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Numerics;
using CommandsEditor;

namespace AlienPAK
{
    /// <summary>
    /// Interaction logic for MaterialEditorControlsWPF.xaml
    /// </summary>
    public partial class MaterialEditorControlsWPF : UserControl
    {
        public Action<int> OnMaterialTextureIndexSelected;

        public MaterialEditorControlsWPF()
        {
            InitializeComponent();
        }

        public void ShowTexturePreview(bool show)
        {
            materialPreviewGroup.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MaterialTextureSelected(object sender, EventArgs e)
        {
            OnMaterialTextureIndexSelected?.Invoke(materialTextureSelection.SelectedIndex);
        }
    }
}
