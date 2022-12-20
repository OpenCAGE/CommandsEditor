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
using CATHODE.Assets.Utilities;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddOrEditResource : Form
    {
        public List<CathodeResourceReference> Resources = null;

        private int current_ui_offset = 7;

        public CathodeEditorGUI_AddOrEditResource(List<CathodeResourceReference> resRefs, string windowTitle, bool enableAdding = false)
        {
            Resources = resRefs;

            InitializeComponent();

            this.Text += " - " + windowTitle;
            addNewResource.Visible = enableAdding;

            current_ui_offset = 7;
            resource_panel.Controls.Clear();

            for (int i = 0; i < resRefs.Count; i++)
            {
                ResourceUserControl resourceGroup;
                switch (resRefs[i].entryType)
                {
                    case CathodeResourceReferenceType.RENDERABLE_INSTANCE:
                        //Convert model BIN index from REDs to PAK index
                        int pakModelIndex = -1;
                        for (int y = 0; y < CurrentInstance.modelDB.Models.Count; y++)
                        {
                            for (int z = 0; z < CurrentInstance.modelDB.Models[y].Submeshes.Count; z++)
                            {
                                if (CurrentInstance.modelDB.Models[y].Submeshes[z].binIndex == CurrentInstance.redsDB.RenderableElements[resRefs[i].startIndex].ModelIndex)
                                {
                                    pakModelIndex = y;
                                    break;
                                }
                            }
                            if (pakModelIndex != -1) break;
                        }

                        //Get all remapped materials from REDs
                        List<int> modelMaterialIndexes = new List<int>();
                        for (int y = 0; y < resRefs[i].count; y++)
                            modelMaterialIndexes.Add(CurrentInstance.redsDB.RenderableElements[resRefs[i].startIndex + y].MaterialLibraryIndex);

                        GUI_Resource_RenderableInstance ui = new GUI_Resource_RenderableInstance();
                        ui.PopulateUI(pakModelIndex, modelMaterialIndexes, WorkOutWhoUsesThisResource(resRefs[i]));
                        resourceGroup = ui;
                        break;
                    default:
                        GUI_Resource_TempPlaceholder ui2 = new GUI_Resource_TempPlaceholder();
                        ui2.PopulateUI(resRefs[i].entryType.ToString(), WorkOutWhoUsesThisResource(resRefs[i]));
                        resourceGroup = ui2;
                        break;
                }
                resourceGroup.ResourceReference = resRefs[i];
                resourceGroup.Location = new Point(15, current_ui_offset);
                current_ui_offset += resourceGroup.Height + 6;
                resource_panel.Controls.Add(resourceGroup);
            }

            //TODO
#if !DEBUG
            addNewResource.Visible = false;
#endif
        }

        private string WorkOutWhoUsesThisResource(CathodeResourceReference resRef)
        {
            /*
                string otherUsers = "";
                foreach (CathodeComposite comp in CurrentInstance.commandsPAK.Composites)
                {
                    if (EditComposite == comp) continue;

                    foreach (CathodeResourceReference re in comp.resources)
                        if (re.resourceID == resRef.resourceID)
                            otherUsers += comp.name + ", ";

                    foreach (CathodeEntity ent in comp.GetEntities())
                    {
                        if (EditEntity == ent) continue;
                        foreach (CathodeResourceReference re in ent.resources)
                            if (re.resourceID == resRef.resourceID)
                                otherUsers += EditorUtils.GenerateEntityName(ent, comp) + ", ";
                    }
                }
                if (otherUsers != "")
                    otherUsers = otherUsers.Substring(0, otherUsers.Length - 2);
                return otherUsers;
            */
            return "";
        }

        private void SaveChanges_Click(object sender, EventArgs e)
        {
            List<CathodeResourceReference> newResourceReferences = new List<CathodeResourceReference>();
            for (int i = 0; i < resource_panel.Controls.Count; i++)
            {
                CathodeResourceReference resourceRef = ((ResourceUserControl)resource_panel.Controls[i]).ResourceReference;
                switch (resourceRef.entryType)
                {
                    case CathodeResourceReferenceType.RENDERABLE_INSTANCE:
                        GUI_Resource_RenderableInstance ui = (GUI_Resource_RenderableInstance)resource_panel.Controls[i];
                        resourceRef.count = ui.SelectedMaterialIndexes.Count;
                        resourceRef.startIndex = CurrentInstance.redsDB.RenderableElements.Count;
                        for (int y = 0; y < ui.SelectedMaterialIndexes.Count; y++)
                        {
                            RenderableElementsDatabase.RenderableElement newRed = new RenderableElementsDatabase.RenderableElement();
                            newRed.ModelIndex = CurrentInstance.modelDB.Models[ui.SelectedModelIndex].Submeshes[y].binIndex;
                            newRed.MaterialLibraryIndex = ui.SelectedMaterialIndexes[y];
                            CurrentInstance.redsDB.RenderableElements.Add(newRed);
                        }
                        break;
                }
                newResourceReferences.Add(resourceRef);
            }

            Resources = newResourceReferences;
            this.Close();
        }
    }
}
