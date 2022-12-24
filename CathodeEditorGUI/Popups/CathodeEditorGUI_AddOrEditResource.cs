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
using System.Windows.Interop;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddOrEditResource : Form
    {
        public Action<List<CathodeResourceReference>> OnSaved;
        
        private List<CathodeResourceReference> resources = null;
        private ShortGuid guid_parent;
        private int current_ui_offset = 7;

        public CathodeEditorGUI_AddOrEditResource(List<CathodeResourceReference> resRefs, ShortGuid parent, string windowTitle)
        {
            CathodeResourceReference[] copy = new CathodeResourceReference[resRefs.Count];
            resRefs.CopyTo(copy);
            resources = copy.ToList<CathodeResourceReference>();
            guid_parent = parent;

            InitializeComponent();

            this.Text += " - " + windowTitle;
            resourceType.SelectedIndex = 0;

            RefreshUI();
        }

        private void RefreshUI()
        {
            current_ui_offset = 7;
            resource_panel.Controls.Clear();

            for (int i = 0; i < resources.Count; i++)
            {
                ResourceUserControl resourceGroup;
                switch (resources[i].entryType)
                {
                    case CathodeResourceReferenceType.RENDERABLE_INSTANCE:
                        //Convert model BIN index from REDs to PAK index
                        int pakModelIndex = -1;
                        for (int y = 0; y < CurrentInstance.modelDB.Models.Count; y++)
                        {
                            for (int z = 0; z < CurrentInstance.modelDB.Models[y].Submeshes.Count; z++)
                            {
                                if (CurrentInstance.modelDB.Models[y].Submeshes[z].binIndex == CurrentInstance.redsDB.RenderableElements[resources[i].startIndex].ModelIndex)
                                {
                                    pakModelIndex = y;
                                    break;
                                }
                            }
                            if (pakModelIndex != -1) break;
                        }

                        //Get all remapped materials from REDs
                        List<int> modelMaterialIndexes = new List<int>();
                        for (int y = 0; y < resources[i].count; y++)
                            modelMaterialIndexes.Add(CurrentInstance.redsDB.RenderableElements[resources[i].startIndex + y].MaterialLibraryIndex);

                        GUI_Resource_RenderableInstance ui = new GUI_Resource_RenderableInstance();
                        ui.PopulateUI(pakModelIndex, modelMaterialIndexes);
                        resourceGroup = ui;
                        break;
                    default:
                        GUI_Resource_TempPlaceholder ui2 = new GUI_Resource_TempPlaceholder();
                        ui2.PopulateUI(resources[i].entryType.ToString());
                        resourceGroup = ui2;
                        break;
                }
                resourceGroup.ResourceReference = resources[i];
                resourceGroup.Location = new Point(15, current_ui_offset);
                current_ui_offset += resourceGroup.Height + 6;
                resource_panel.Controls.Add(resourceGroup);
            }
        }

        /* Add a new resource reference to the list */
        private void addResource_Click(object sender, EventArgs e)
        {
            CathodeResourceReferenceType type = (CathodeResourceReferenceType)resourceType.SelectedIndex;

            //A resource reference list can only ever point to one of a type
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].entryType == type)
                {
                    MessageBox.Show("This resource type is already referenced!\nOnly one reference to " + type + " can be added.", "Can't add duplicate type.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            //TODO: remove this once we support other types in the GUI
            if (type != CathodeResourceReferenceType.RENDERABLE_INSTANCE)
            {
                MessageBox.Show("Type " + type + " is currently unsupported.\nThis functionality will be added in a future OpenCAGE update!", "Coming soon...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CathodeResourceReference newReference = new CathodeResourceReference();
            newReference.resourceID = guid_parent;
            newReference.entryType = type;
            newReference.startIndex = 0;
            newReference.count = 1;
            newReference.entityID = new ShortGuid("00-00-00-00");
            resources.Add(newReference);

            RefreshUI();
        }

        /* Delete an existing resource reference from the list */
        private void deleteResource_Click(object sender, EventArgs e)
        {
            CathodeResourceReferenceType type = (CathodeResourceReferenceType)resourceType.SelectedIndex;
            CathodeResourceReference reference = resources.FirstOrDefault(o => o.entryType == type);
            if (reference == null)
            {
                MessageBox.Show("Resource type " + type + " is not referenced!\nThere is nothing to delete.", "Type not referenced!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("You are about to remove resource reference for type " + type + ".\nThis is a destructive action - are you sure?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    resources.Remove(reference);
                    RefreshUI();
                }
            }
        }

        /* Save all changes back out */
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
            resources = newResourceReferences;
            OnSaved?.Invoke(resources);
            this.Close();
        }
    }
}
