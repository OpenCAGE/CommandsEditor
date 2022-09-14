using CATHODE;
using CATHODE.Commands;
using CATHODE.Misc;
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
            ShortGuid resourceParamID = ShortGuidUtils.Generate("resource");
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

            for (int i =0; i < resRef.Count; i++)
            {
                switch (resRef[i].entryType)
                {
                    case CathodeResourceReferenceType.RENDERABLE_INSTANCE:
                        RenderableElementsDatabase reds = new RenderableElementsDatabase(CurrentInstance.commandsPAK.Filepath.Substring(0, CurrentInstance.commandsPAK.Filepath.Length - ("COMMANDS.PAK").Length) + "REDS.BIN");
                        for (int x = resRef[i].startIndex; x < resRef[i].startIndex + resRef[i].count; x++)
                        {
                            MessageBox.Show("Model Index " + reds.RenderableElements[x].ModelIndex);
                        }
                        break;
                }
            }
        }
    }
}
