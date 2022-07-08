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
    public partial class GUI_HexDataType : UserControl
    {
        public GUI_HexDataType()
        {
            InitializeComponent();
        }

        private CathodeResource resRef = null;

        public void PopulateUI(CathodeResource cResource, ShortGuid paramID, CathodeComposite selected_flowgraph)
        {
            GUID_VARIABLE_DUMMY.Text = ShortGuidUtils.FindString(paramID);
            resRef = cResource;

            if (cResource.resourceID.val != null)
            {
                textBox2.Text = BitConverter.ToString(new byte[] { cResource.resourceID.val[0] });
                textBox3.Text = BitConverter.ToString(new byte[] { cResource.resourceID.val[1] });
                textBox5.Text = BitConverter.ToString(new byte[] { cResource.resourceID.val[2] });
                textBox4.Text = BitConverter.ToString(new byte[] { cResource.resourceID.val[3] });
            }

            /*
            List<RenderableElement> redsList = new List<RenderableElement>();
            CathodeResourceReference resRef = selected_flowgraph.resources.FirstOrDefault(o => o.resourceRefID == cResource.resourceID);
            if (resRef == null || resRef.entryType != CathodeResourceReferenceType.RENDERABLE_INSTANCE) return;
            for (int p = 0; p < resRef.entryCountREDS; p++) redsList.Add(redsBIN.GetRenderableElement(resRef.entryIndexREDS + p));
            if (resRef.entryCountREDS != redsList.Count || redsList.Count == 0) return; //TODO: handle this nicer
            CathodeEditorGUI_EditResource res_editor = new CathodeEditorGUI_EditResource(modelPAK.GetCS2s(), redsList);
            res_editor.Show();
            res_editor.EditComplete += new FinishedEditingIndexes(res_editor_submitted);
            */
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                resRef.resourceID.val[0] = Convert.ToByte(textBox2.Text, 16);
            }
            catch { }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                resRef.resourceID.val[1] = Convert.ToByte(textBox3.Text, 16);
            }
            catch { }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                resRef.resourceID.val[2] = Convert.ToByte(textBox5.Text, 16);
            }
            catch { }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                resRef.resourceID.val[3] = Convert.ToByte(textBox4.Text, 16);
            }
            catch { }
        }
    }
}
