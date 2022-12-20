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

        private CathodeResource resRef = null;
        public void PopulateUI(CathodeResource cResource, ShortGuid paramID)
        {
            GUID_VARIABLE_DUMMY.Text = ShortGuidUtils.FindString(paramID);
            resRef = cResource;
        }

        /* Edit resources referenced by the resource param */
        private void openResourceEditor_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_AddOrEditResource resourceEditor = new CathodeEditorGUI_AddOrEditResource(resRef.value, GUID_VARIABLE_DUMMY.Text, false); //todo: can we allow adding? unsure if supported by cathode even though we could support it here
            resourceEditor.Show();
            resourceEditor.FormClosed += ResourceEditor_FormClosed1;
        }
        private void ResourceEditor_FormClosed1(object sender, FormClosedEventArgs e)
        {
            resRef.value = ((CathodeEditorGUI_AddOrEditResource)sender).Resources;
            this.BringToFront();
            this.Focus();
        }
    }
}
