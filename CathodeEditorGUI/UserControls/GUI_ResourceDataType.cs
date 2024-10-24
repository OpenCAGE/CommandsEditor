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
    public partial class GUI_ResourceDataType : ParameterUserControl
    {
        public GUI_ResourceDataType() : base()
        {
            InitializeComponent();
            this.ContextMenuStrip = contextMenuStrip1;
            this.deleteToolStripMenuItem.Click += new EventHandler(deleteToolStripMenuItem_Click);
        }

        private EntityInspector _entDisplay = null;

        private cResource resRef = null;
        public void PopulateUI(EntityInspector entDisplay, cResource cResource, string paramID)
        {
            _entDisplay = entDisplay;
            GUID_VARIABLE_DUMMY.Text = paramID;
            resRef = cResource;
            this.deleteToolStripMenuItem.Text = "Delete '" + paramID + "'";
        }

        /* Edit resources referenced by the resource param */
        private void openResourceEditor_Click(object sender, EventArgs e)
        {
            AddOrEditResource resourceEditor = new AddOrEditResource(_entDisplay, resRef.value, resRef.shortGUID, GUID_VARIABLE_DUMMY.Text);
            resourceEditor.Show();
            resourceEditor.OnSaved += OnResourceEditorSaved;
            resourceEditor.FormClosed += ResourceEditor_FormClosed;
        }
        private void OnResourceEditorSaved(List<ResourceReference> resources)
        {
            resRef.value = resources;
        }
        private void ResourceEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.BringToFront();
            this.Focus();
        }
    }
}
