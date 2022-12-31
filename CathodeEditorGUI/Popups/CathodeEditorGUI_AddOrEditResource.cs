using CATHODE;
using CATHODE.Commands;
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
        public Action<List<ResourceReference>> OnSaved;
        
        private List<ResourceReference> resources = null;
        private ShortGuid guid_parent;
        private int current_ui_offset = 7;

        public CathodeEditorGUI_AddOrEditResource(List<ResourceReference> resRefs, ShortGuid parent, string windowTitle)
        {
            ResourceReference[] copy = new ResourceReference[resRefs.Count];
            resRefs.CopyTo(copy);
            resources = copy.ToList<ResourceReference>();
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
                ResourceUserControl resourceGroup = null;
                switch (resources[i].entryType)
                {
                    case ResourceType.ANIMATED_MODEL:
                        {
                            resourceGroup = new GUI_Resource_AnimatedModel();
                            ((GUI_Resource_AnimatedModel)resourceGroup).PopulateUI(resources[i].startIndex);
                            break;
                        }
                    case ResourceType.COLLISION_MAPPING:
                        {
                            resourceGroup = new GUI_Resource_CollisionMapping();
                            ((GUI_Resource_CollisionMapping)resourceGroup).PopulateUI();
                            break;
                        }
                        //TODO: should we even bother showing this?
                    case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                        {
                            resourceGroup = new GUI_Resource_DynamicPhysicsSystem();
                            ((GUI_Resource_DynamicPhysicsSystem)resourceGroup).PopulateUI(resources[i].startIndex);
                            break;
                        }
                    case ResourceType.EXCLUSIVE_MASTER_STATE_RESOURCE:
                        {
                            resourceGroup = new GUI_Resource_ExclusiveMasterStateResource();
                            ((GUI_Resource_ExclusiveMasterStateResource)resourceGroup).PopulateUI();
                            break;
                        }
                    case ResourceType.NAV_MESH_BARRIER_RESOURCE:
                        {
                            resourceGroup = new GUI_Resource_NavMeshBarrierResource();
                            ((GUI_Resource_NavMeshBarrierResource)resourceGroup).PopulateUI();
                            break;
                        }
                    case ResourceType.RENDERABLE_INSTANCE:
                        {
                            //Convert model BIN index from REDs to PAK index
                            int pakModelIndex = -1;
                            for (int y = 0; y < Editor.resource.models.Models.Count; y++)
                            {
                                for (int z = 0; z < Editor.resource.models.Models[y].Submeshes.Count; z++)
                                {
                                    if (Editor.resource.models.Models[y].Submeshes[z].binIndex == Editor.resource.reds.RenderableElements[resources[i].startIndex].ModelIndex)
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
                                modelMaterialIndexes.Add(Editor.resource.reds.RenderableElements[resources[i].startIndex + y].MaterialLibraryIndex);

                            resourceGroup = new GUI_Resource_RenderableInstance();
                            ((GUI_Resource_RenderableInstance)resourceGroup).PopulateUI(pakModelIndex, modelMaterialIndexes);
                            break;
                        }
                    case ResourceType.TRAVERSAL_SEGMENT:
                        {
                            resourceGroup = new GUI_Resource_TraversalSegment();
                            ((GUI_Resource_TraversalSegment)resourceGroup).PopulateUI();
                            break;
                        }
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
            ResourceType type = (ResourceType)resourceType.SelectedIndex;

            //A resource reference list can only ever point to one of a type
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].entryType == type)
                {
                    MessageBox.Show("This resource type is already referenced!\nOnly one reference to " + type + " can be added.", "Can't add duplicate type.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            ResourceReference newReference = new ResourceReference();
            newReference.resourceID = guid_parent;
            newReference.entryType = type;
            switch (newReference.entryType)
            {
                case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                case ResourceType.RENDERABLE_INSTANCE:
                case ResourceType.ANIMATED_MODEL: 
                    newReference.startIndex = 0;
                    break;            
                case ResourceType.EXCLUSIVE_MASTER_STATE_RESOURCE:
                case ResourceType.NAV_MESH_BARRIER_RESOURCE:      
                case ResourceType.TRAVERSAL_SEGMENT:              //Sure this one doesn't use startIndex?
                case ResourceType.COLLISION_MAPPING:              //Sure this one doesn't use startIndex?
                    //This type just uses the default values
                    break;
            }
            resources.Add(newReference);

            RefreshUI();
        }

        /* Delete an existing resource reference from the list */
        private void deleteResource_Click(object sender, EventArgs e)
        {
            ResourceType type = (ResourceType)resourceType.SelectedIndex;
            ResourceReference reference = resources.FirstOrDefault(o => o.entryType == type);
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
            List<ResourceReference> newResourceReferences = new List<ResourceReference>();
            for (int i = 0; i < resource_panel.Controls.Count; i++)
            {
                ResourceReference resourceRef = ((ResourceUserControl)resource_panel.Controls[i]).ResourceReference;
                switch (resourceRef.entryType)
                {
                    case ResourceType.ANIMATED_MODEL:
                        {
                            GUI_Resource_AnimatedModel ui = (GUI_Resource_AnimatedModel)resource_panel.Controls[i];
                            resourceRef.startIndex = ui.EnvironmentAnimIndex;
                            break;
                        }
                    case ResourceType.COLLISION_MAPPING:
                        {
                            break;
                        }
                    case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                        {
                            GUI_Resource_DynamicPhysicsSystem ui = (GUI_Resource_DynamicPhysicsSystem)resource_panel.Controls[i];
                            resourceRef.startIndex = ui.UnknownIndex;
                            break;
                        }
                    case ResourceType.EXCLUSIVE_MASTER_STATE_RESOURCE:
                        {
                            break;
                        }
                    case ResourceType.NAV_MESH_BARRIER_RESOURCE:
                        {
                            break;
                        }
                    case ResourceType.RENDERABLE_INSTANCE:
                        {
                            GUI_Resource_RenderableInstance ui = (GUI_Resource_RenderableInstance)resource_panel.Controls[i];
                            resourceRef.count = ui.SelectedMaterialIndexes.Count;
                            resourceRef.startIndex = Editor.resource.reds.RenderableElements.Count;
                            for (int y = 0; y < ui.SelectedMaterialIndexes.Count; y++)
                            {
                                RenderableElementsDatabase.RenderableElement newRed = new RenderableElementsDatabase.RenderableElement();
                                newRed.ModelIndex = Editor.resource.models.Models[ui.SelectedModelIndex].Submeshes[y].binIndex;
                                newRed.MaterialLibraryIndex = ui.SelectedMaterialIndexes[y];
                                Editor.resource.reds.RenderableElements.Add(newRed);
                            }
                            break;
                        }
                    case ResourceType.TRAVERSAL_SEGMENT:
                        {
                            break;
                        }
                }
                newResourceReferences.Add(resourceRef);
            }
            resources = newResourceReferences;
            OnSaved?.Invoke(resources);
            this.Close();
        }
    }
}
