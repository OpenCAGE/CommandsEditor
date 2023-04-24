using CATHODE;
using CATHODE.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandsEditor.Popups.UserControls;
using System.Windows.Interop;
using CommandsEditor.Popups.Base;

namespace CommandsEditor
{
    public partial class AddOrEditResource : BaseWindow
    {
        public Action<List<ResourceReference>> OnSaved;
        
        private List<ResourceReference> resources = null;
        private ShortGuid guid_parent;
        private int current_ui_offset = 7;

        public AddOrEditResource(CommandsEditor editor, List<ResourceReference> resRefs, ShortGuid parent, string windowTitle) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
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
                            resourceGroup = new GUI_Resource_AnimatedModel(_editor);
                            ((GUI_Resource_AnimatedModel)resourceGroup).PopulateUI(resources[i].startIndex);
                            break;
                        }
                    case ResourceType.COLLISION_MAPPING:
                        {
                            resourceGroup = new GUI_Resource_CollisionMapping(_editor);
                            ((GUI_Resource_CollisionMapping)resourceGroup).PopulateUI(resources[i].position, resources[i].rotation, resources[i].collisionID);
                            break;
                        }
                    case ResourceType.NAV_MESH_BARRIER_RESOURCE:
                        {
                            resourceGroup = new GUI_Resource_NavMeshBarrierResource(_editor);
                            ((GUI_Resource_NavMeshBarrierResource)resourceGroup).PopulateUI(resources[i].position, resources[i].rotation);
                            break;
                        }
                    case ResourceType.RENDERABLE_INSTANCE:
                        {
                            resourceGroup = new GUI_Resource_RenderableInstance(_editor);
                            ((GUI_Resource_RenderableInstance)resourceGroup).PopulateUI(resources[i].position, resources[i].rotation, resources[i].startIndex, resources[i].count);
                            break;
                        }
                    default:
                        continue;
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
            ResourceType type = (ResourceType)Enum.Parse(typeof(ResourceType), resourceType.Items[resourceType.SelectedIndex].ToString());

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
            ResourceType type = (ResourceType)Enum.Parse(typeof(ResourceType), resourceType.Items[resourceType.SelectedIndex].ToString());
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
                            GUI_Resource_CollisionMapping ui = (GUI_Resource_CollisionMapping)resource_panel.Controls[i];
                            resourceRef.position = ui.Position;
                            resourceRef.rotation = ui.Rotation;
                            resourceRef.collisionID = ui.CollisionID;
                            break;
                        }
                    case ResourceType.NAV_MESH_BARRIER_RESOURCE:
                        {
                            GUI_Resource_NavMeshBarrierResource ui = (GUI_Resource_NavMeshBarrierResource)resource_panel.Controls[i];
                            resourceRef.position = ui.Position;
                            resourceRef.rotation = ui.Rotation;
                            break;
                        }
                    case ResourceType.RENDERABLE_INSTANCE:
                        {
                            GUI_Resource_RenderableInstance ui = (GUI_Resource_RenderableInstance)resource_panel.Controls[i];
                            resourceRef.position = ui.Position;
                            resourceRef.rotation = ui.Rotation;
                            resourceRef.count = ui.SelectedMaterialIndexes.Count;
                            resourceRef.startIndex = Editor.resource.reds.Entries.Count;

                            for (int y = 0; y < ui.SelectedMaterialIndexes.Count; y++)
                            {
                                RenderableElements.Element newRed = new RenderableElements.Element();
                                newRed.ModelIndex = ui.SelectedModelIndex + y; //assumes sequential write
                                newRed.MaterialIndex = ui.SelectedMaterialIndexes[y];
                                if (y == 0)
                                {
                                    newRed.LODIndex = Editor.resource.reds.Entries.Count;
                                    //newRed.LODCount = (byte)ui.SelectedMaterialIndexes.Count;
                                    newRed.LODCount = 0; //TODO!!
                                }
                                Editor.resource.reds.Entries.Add(newRed);
                            }
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
