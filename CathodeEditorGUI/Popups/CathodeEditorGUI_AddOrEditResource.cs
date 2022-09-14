using CATHODE;
using CATHODE.Commands;
using CATHODE.Misc;
using CATHODE.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CathodeEditorGUI.Popups.UserControls;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddOrEditResource : Form
    {
        CathodeEntity _ent = null;
        CathodeComposite _flow = null;
        List<CathodeResourceReference> resRef = new List<CathodeResourceReference>(); //FOR TESTING ONLY
        public CathodeEditorGUI_AddOrEditResource(CathodeEntity entity, CathodeComposite composite)
        {
            InitializeComponent();
            this.Text += " - " + EditorUtils.GenerateEntityName(entity, composite);
            
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
            InitializeComponent();
            this.Text += " - " + flowgraph.name;
            
            _flow = flowgraph;

            //FOR TESTING ONLY
            resRef.AddRange(_flow.resources);

            Setup();
        }

        private void Setup()
        {
            string baseLevelPath = CurrentInstance.commandsPAK.Filepath.Substring(0, CurrentInstance.commandsPAK.Filepath.Length - ("WORLD/COMMANDS.PAK").Length);

            RenderableElementsDatabase db = new RenderableElementsDatabase(baseLevelPath + "WORLD/REDS.BIN");
            Models models = new Models(baseLevelPath + "RENDERABLE/LEVEL_MODELS.PAK"); models.Load();
            MaterialDatabase materials = new MaterialDatabase(baseLevelPath + "RENDERABLE/LEVEL_MODELS.MTL");

            int current_ui_offset = 7;
            for (int i = 0; i < resRef.Count; i++)
            {
                GroupBox resourceGroup = new GroupBox();
                resourceGroup.Text = resRef[i].entryType.ToString();
                resourceGroup.Height = 30; //TEMP
                resourceGroup.Width = 850; //TEMP
                switch (resRef[i].entryType)
                {
                    case CathodeResourceReferenceType.RENDERABLE_INSTANCE:
                        for (int x = resRef[i].startIndex; x < resRef[i].startIndex + resRef[i].count; x++)
                        {
                            RenderableElementsDatabase.RenderableElement e = db.RenderableElements[x];

                            GUI_Resource_RenderableInstance ui = new GUI_Resource_RenderableInstance();
                            ui.PopulateUI(models.GetModelNameByIndex(e.ModelIndex) + " -> " + models.GetModelSubmeshNameByIndex(e.ModelIndex), materials.MaterialNames[e.MaterialLibraryIndex]);
                            ui.Location = new Point(15, 20 + ((ui.Height + 6) * (x - resRef[i].startIndex)));
                            resourceGroup.Controls.Add(ui);

                            resourceGroup.Height += ui.Height + 20;
                            resourceGroup.Width = ui.Width + 30;

                            /*
                            MessageBox.Show("Model: " + models.GetModelNameByIndex(e.ModelIndex) + "\n" +
                                            "Submesh: " + models.GetModelSubmeshNameByIndex(e.ModelIndex) + "\n" +
                                            "Size: " + models.GetModelByIndex(e.ModelIndex).BlockSize + "\n" +
                                            "Material: " + materials.MaterialNames[e.MaterialLibraryIndex] + "\n" +
                                            "Additional Nums: " + e.ModelLODIndex + " " + e.ModelLODPrimitiveCount);
                            */
                        }
                        break;
                }
                resourceGroup.Location = new Point(15, current_ui_offset);
                current_ui_offset += resourceGroup.Height + 6;
                resource_panel.Controls.Add(resourceGroup);
            }
        }
    }
}
