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

        private ShortGuid guid_parent;
        private int current_ui_offset = 7;

        public CathodeEditorGUI_AddOrEditResource(List<CathodeResourceReference> resRefs, ShortGuid parent, string windowTitle)
        {
            Resources = resRefs;
            guid_parent = parent;

            InitializeComponent();

            this.Text += " - " + windowTitle;

            RefreshUI();
        }

        private void RefreshUI()
        {
            current_ui_offset = 7;
            resource_panel.Controls.Clear();

            for (int i = 0; i < Resources.Count; i++)
            {
                ResourceUserControl resourceGroup;
                switch (Resources[i].entryType)
                {
                    case CathodeResourceReferenceType.RENDERABLE_INSTANCE:
                        //Convert model BIN index from REDs to PAK index
                        int pakModelIndex = -1;
                        for (int y = 0; y < CurrentInstance.modelDB.Models.Count; y++)
                        {
                            for (int z = 0; z < CurrentInstance.modelDB.Models[y].Submeshes.Count; z++)
                            {
                                if (CurrentInstance.modelDB.Models[y].Submeshes[z].binIndex == CurrentInstance.redsDB.RenderableElements[Resources[i].startIndex].ModelIndex)
                                {
                                    pakModelIndex = y;
                                    break;
                                }
                            }
                            if (pakModelIndex != -1) break;
                        }

                        //Get all remapped materials from REDs
                        List<int> modelMaterialIndexes = new List<int>();
                        for (int y = 0; y < Resources[i].count; y++)
                            modelMaterialIndexes.Add(CurrentInstance.redsDB.RenderableElements[Resources[i].startIndex + y].MaterialLibraryIndex);

                        GUI_Resource_RenderableInstance ui = new GUI_Resource_RenderableInstance();
                        ui.PopulateUI(pakModelIndex, modelMaterialIndexes);
                        resourceGroup = ui;
                        break;
                    default:
                        GUI_Resource_TempPlaceholder ui2 = new GUI_Resource_TempPlaceholder();
                        ui2.PopulateUI(Resources[i].entryType.ToString());
                        resourceGroup = ui2;
                        break;
                }
                resourceGroup.ResourceReference = Resources[i];
                resourceGroup.Location = new Point(15, current_ui_offset);
                current_ui_offset += resourceGroup.Height + 6;
                resource_panel.Controls.Add(resourceGroup);
            }
        }

        private void addResource_Click(object sender, EventArgs e)
        {
            CathodeResourceReference test = new CathodeResourceReference();
            test.entityID = guid_parent;
            test.entryType = CathodeResourceReferenceType.RENDERABLE_INSTANCE;
            test.startIndex = 50;
            test.count = 1;
            Resources.Add(test);

            RefreshUI();

            //TODO: when making new resources, link them back to guid_parent
        }

        private void deleteResource_Click(object sender, EventArgs e)
        {
            //TODO
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
