using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Scripting;
using CATHODE;
using CathodeLib;

namespace CommandsEditor.UserControls
{
    public partial class GUI_SplineDataType : UserControl
    {
        public GUI_SplineDataType()
        {
            InitializeComponent();
        }

        private cSpline spline = null;
        public void PopulateUI(cSpline cSpline, string paramID)
        {
            SPLINE_CONTAINER.Text = paramID;
            spline = cSpline;
        }

        private void openSplineEditor_Click(object sender, EventArgs e)
        {
            EditSpline splineEditor = new EditSpline(spline, Editor.selected.entity.GetParameter("loop"));
            splineEditor.Show();
            splineEditor.OnSaved += OnSplineEditorSaved;
            splineEditor.FormClosed += SplineEditor_FormClosed;
        } 
        private void OnSplineEditorSaved(cSpline newSpline)
        {
            spline.splinePoints = newSpline.splinePoints;
        }
        private void SplineEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();
        }
    }
}
