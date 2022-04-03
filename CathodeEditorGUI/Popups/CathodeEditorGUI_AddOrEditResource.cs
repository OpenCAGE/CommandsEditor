using CATHODE;
using CATHODE.Commands;
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
    public partial class CathodeEditorGUI_AddOrEditResource : Form
    {
        CathodeEntity _ent = null;
        CathodeComposite _flow = null;
        List<CathodeResourceReference> resRef = new List<CathodeResourceReference>(); //FOR TESTING ONLY
        public CathodeEditorGUI_AddOrEditResource(CathodeEntity entity)
        {
            _ent = entity;

            //FOR TESTING ONLY
            resRef.AddRange(_ent.resources);
            cGUID resourceParamID = Utilities.GenerateGUID("resource");
            CathodeLoadedParameter resourceParam = CurrentInstance.selectedEntity.parameters.FirstOrDefault(o => o.shortGUID == resourceParamID);
            if (resourceParam != null) resRef.AddRange(((CathodeResource)resourceParam.content).value);

            Setup();
        }
        public CathodeEditorGUI_AddOrEditResource(CathodeComposite flowgraph)
        {
            _flow = flowgraph;

            //FOR TESTING ONLY
            resRef.AddRange(_flow.resources);

            Setup();
        }

        private void Setup()
        {
            InitializeComponent();

            //FOR TESTING ONLY
            MessageBox.Show(resRef.Count + " resource references");
            for (int i =0; i < resRef.Count; i++)
            {
                MessageBox.Show(resRef[i].entryType.ToString());
            }
        }
    }
}
