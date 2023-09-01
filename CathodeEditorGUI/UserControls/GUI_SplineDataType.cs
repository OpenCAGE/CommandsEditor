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
using CommandsEditor.DockPanels;

namespace CommandsEditor.UserControls
{
    public partial class GUI_SplineDataType : BaseUserControl
    {
        private EntityDisplay _entityDisplay;

        public GUI_SplineDataType(EntityDisplay entityDisplay) : base(entityDisplay.Content)
        {
            _entityDisplay = entityDisplay;
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
            EditSpline splineEditor = new EditSpline(_content, spline, _entityDisplay.Entity.GetParameter("loop"));
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
