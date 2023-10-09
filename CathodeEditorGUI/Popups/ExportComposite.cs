using CATHODE.Scripting;
using System.Collections.Generic;
using CommandsEditor.Popups.Base;
using CommandsEditor.DockPanels;
using CathodeLib;
using CATHODE;
using System.Windows;
using System.Linq;
using System;

namespace CommandsEditor
{
    public partial class ExportComposite : BaseWindow
    {
        CompositeDisplay _display;

        public ExportComposite(CompositeDisplay display, bool canExportChildren) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, display.Content)
        {
            _display = display;

            InitializeComponent();

            levelList.BeginUpdate();
            levelList.Items.AddRange(Level.GetLevels(SharedData.pathToAI, true).ToArray());
            levelList.Items.Remove(Content.level);
            levelList.EndUpdate();

            levelList.SelectedIndex = 0;

            this.Text = "Port '" + _display.Composite.name + "'";
            
            if (!canExportChildren)
            {
                recurse.Checked = false;
                recurse.Enabled = false;
            }
        }

        private void export_Click(object sender, System.EventArgs e)
        {
            Global gbl = new Global() { AnimationStrings_Debug = Singleton.AnimationStrings_Debug };
            Level lvl = new Level(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + levelList.SelectedItem.ToString(), gbl);
            AddCompositesRecursively(_display.Composite, lvl);
            lvl.Save();

            MessageBox.Show("Finished porting '" + _display.Composite.name + "' to ' " + levelList.SelectedItem.ToString() + "!", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void AddCompositesRecursively(Composite composite, Level lvl)
        {
            //Check to see if the composite already exists at our destination
            Composite dest = lvl.Commands.Entries.FirstOrDefault(o => o.shortGUID == composite.shortGUID);

            //If the user opted to overwrite & we found an existing matching comp in the destination, delete it
            if (overwrite.Checked)
            {
                if (dest != null)
                    lvl.Commands.Entries.Remove(dest);
                dest = null;
            }

            //TODO: reduce duplication here 

            if (dest == null)
            {
                //We need to add the composite to the new location
                Composite copiedComp = composite.Copy();
                lvl.Commands.Entries.Add(copiedComp);

                //Check to see if we should copy resources to the destination, and point to them if we do
                foreach (FunctionEntity ent in copiedComp.functions)
                {
                    //TEMP: remove all collision mappings for now
                    ent.resources.RemoveAll(o => o.entryType == ResourceType.COLLISION_MAPPING);

                    for (int i = 0; i < ent.resources.Count; i++)
                    {
                        switch (ent.resources[i].entryType)
                        {
                            case ResourceType.ANIMATED_MODEL:
                                //Get EnvironmentAnimation from base level and copy it
                                EnvironmentAnimations.EnvironmentAnimation anim = Content.resource.env_animations.Entries.FirstOrDefault(o => o.ResourceIndex == ent.resources[i].index).Copy();

                                //Update the ResourceIndex to suit the destination EnvironmentAnimation count
                                anim.ResourceIndex = lvl.EnvironmentAnimations.Entries[lvl.EnvironmentAnimations.Entries.Count - 1].ResourceIndex + 1;
                                ent.resources[i].index = anim.ResourceIndex;

                                //Add to dest level
                                lvl.EnvironmentAnimations.Entries.Add(anim);
                                break;

                            case ResourceType.RENDERABLE_INSTANCE:
                                //Get CS2 from base level and copy it (this assumes the REDs entry is always pointing to one CS2, which I don't think is the case
                                Models.CS2 model = Content.resource.models.FindModelForSubmesh(Content.resource.models.GetAtWriteIndex(Content.resource.reds.Entries[ent.resources[i].index].ModelIndex)).Copy();
                                if (model.GetSubmeshCount() != ent.resources[i].count) MessageBox.Show("FAILED! I WAS EXPECTING THESE TO BE THE SAME");
                                lvl.Models.Entries.Add(model);

                                //Save to update our write indexes
                                lvl.Save();

                                //Add REDs entries in dest level pointing to write indexes in dest level
                                for (int x = 0; x < model.Components.Count; x++)
                                {
                                    for (int z = 0; z < model.Components[x].LODs.Count; z++)
                                    {
                                        for (int y = 0; y < model.Components[x].LODs[z].Submeshes.Count; y++)
                                        {
                                            RenderableElements.Element renderable = new RenderableElements.Element();
                                            lvl.RenderableElements.Entries.Add(renderable);

                                            renderable.ModelIndex = lvl.Models.GetWriteIndex(model.Components[x].LODs[z].Submeshes[y]);
                                            renderable.MaterialIndex = 0; //TODO
                                        }
                                    }
                                }

                                /*
                                List<Models.CS2.Component.LOD.Submesh> submeshes = new List<Models.CS2.Component.LOD.Submesh>();
                                List<Materials.Material> materials = new List<Materials.Material>();

                                for (int x = ent.resources[i].index; x < ent.resources[i].index + ent.resources[i].count; x++)
                                {
                                    Models.CS2.Component.LOD.Submesh submesh = Content.resource.models.GetAtWriteIndex(Content.resource.reds.Entries[x].ModelIndex).Copy();
                                    submesh.MaterialLibraryIndex = 0; //TODO
                                    submeshes.Add(submesh);

                                    Materials.Material material = Content.resource.materials.GetAtWriteIndex(Content.resource.reds.Entries[x].MaterialIndex).Copy();
                                    for (int z = 0; z < material.TextureReferences.Length; z++)
                                    {
                                        if (material.TextureReferences[z].Source == Materials.Material.Texture.TextureSource.LEVEL)
                                        {
                                            //TODO: we should copy across level textures
                                        }
                                    }
                                    materials.Add(material);
                                }
                                */

                                break;
                        }
                    }

                    Parameter resources = ent.GetParameter("resource");
                    if (resources != null)
                    {
                        List<ResourceReference> resourceRefs = ((cResource)resources.content).value;
                        for (int i = 0; i < resourceRefs.Count; i++)
                        {
                            //TEMP: remove all collision mappings for now
                            resourceRefs.RemoveAll(o => o.entryType == ResourceType.COLLISION_MAPPING);

                            switch (resourceRefs[i].entryType)
                            {
                                case ResourceType.ANIMATED_MODEL:
                                    //Get EnvironmentAnimation from base level and copy it
                                    EnvironmentAnimations.EnvironmentAnimation anim = Content.resource.env_animations.Entries.FirstOrDefault(o => o.ResourceIndex == resourceRefs[i].index).Copy();

                                    //Update the ResourceIndex to suit the destination EnvironmentAnimation count
                                    anim.ResourceIndex = lvl.EnvironmentAnimations.Entries[lvl.EnvironmentAnimations.Entries.Count - 1].ResourceIndex + 1;
                                    resourceRefs[i].index = anim.ResourceIndex;

                                    //Add to dest level
                                    lvl.EnvironmentAnimations.Entries.Add(anim);
                                    break;

                                case ResourceType.RENDERABLE_INSTANCE:
                                    //for (int x = resourceRefs[i].index; x < resourceRefs[i].index + resourceRefs[i].count; x++)
                                    //{
                                    //
                                    //}


                                    //Get CS2 from base level and copy it (this assumes the REDs entry is always pointing to one CS2, which I don't think is the case
                                    Models.CS2 model = Content.resource.models.FindModelForSubmesh(Content.resource.models.GetAtWriteIndex(Content.resource.reds.Entries[resourceRefs[i].index].ModelIndex)).Copy();
                                    if (model.GetSubmeshCount() != resourceRefs[i].count) MessageBox.Show("FAILED! I WAS EXPECTING THESE TO BE THE SAME");
                                    lvl.Models.Entries.Add(model);

                                    //Save to update our write indexes
                                    lvl.Save();

                                    //Add REDs entries in dest level pointing to write indexes in dest level
                                    for (int x = 0; x < model.Components.Count; x++)
                                    {
                                        for (int z = 0; z < model.Components[x].LODs.Count; z++)
                                        {
                                            for (int y = 0; y < model.Components[x].LODs[z].Submeshes.Count; y++)
                                            {
                                                RenderableElements.Element renderable = new RenderableElements.Element();
                                                lvl.RenderableElements.Entries.Add(renderable);

                                                renderable.ModelIndex = lvl.Models.GetWriteIndex(model.Components[x].LODs[z].Submeshes[y]);
                                                renderable.MaterialIndex = 0; //TODO
                                            }
                                        }
                                    }

                                    /*
                                    List<Models.CS2.Component.LOD.Submesh> submeshes = new List<Models.CS2.Component.LOD.Submesh>();
                                    List<Materials.Material> materials = new List<Materials.Material>();

                                    for (int x = resourceRefs[i].index; x < resourceRefs[i].index + resourceRefs[i].count; x++)
                                    {
                                        Models.CS2.Component.LOD.Submesh submesh = Content.resource.models.GetAtWriteIndex(Content.resource.reds.Entries[x].ModelIndex).Copy();
                                        submesh.MaterialLibraryIndex = 0; //TODO
                                        submeshes.Add(submesh);

                                        Materials.Material material = Content.resource.materials.GetAtWriteIndex(Content.resource.reds.Entries[x].MaterialIndex).Copy();
                                        for (int z = 0; z < material.TextureReferences.Length; z++)
                                        {
                                            if (material.TextureReferences[z].Source == Materials.Material.Texture.TextureSource.LEVEL)
                                            {
                                                //TODO: we should copy across level textures
                                            }
                                        }
                                        materials.Add(material);
                                    }
                                    */

                                    break;
                            }
                        }
                    }
                }
            }

            //If the user opted to recurse, follow any composite instances through, and copy those too
            if (!recurse.Checked) return;
            foreach (FunctionEntity ent in composite.functions)
            {
                if (CommandsUtils.FunctionTypeExists(ent.function)) continue;

                Composite nestedComp = Content.commands.GetComposite(ent.function);
                if (nestedComp != null)
                    AddCompositesRecursively(nestedComp, lvl);
            }
        }
    }
}
