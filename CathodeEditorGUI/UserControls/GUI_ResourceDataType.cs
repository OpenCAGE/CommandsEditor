using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Commands;
using CATHODE;
using CathodeLib;

namespace CathodeEditorGUI.UserControls
{
    public partial class GUI_ResourceDataType : UserControl
    {
        public GUI_ResourceDataType()
        {
            InitializeComponent();
        }

        private cResource resRef = null;
        public void PopulateUI(cResource cResource, ShortGuid paramID)
        {
            GUID_VARIABLE_DUMMY.Text = ShortGuidUtils.FindString(paramID);
            resRef = cResource;
        }

        /* Edit resources referenced by the resource param */
        private void openResourceEditor_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(resRef.value, resRef.resourceID, GUID_VARIABLE_DUMMY.Text);
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
