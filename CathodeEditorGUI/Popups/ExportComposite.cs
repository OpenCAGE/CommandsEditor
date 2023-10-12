using CATHODE.Scripting;
using System.Collections.Generic;
using CommandsEditor.Popups.Base;
using CommandsEditor.DockPanels;
using CathodeLib;
using CATHODE;
using System.Windows;
using System.Linq;
using System;
using System.Windows.Forms.Design;
using System.IO;

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
                                throw new Exception("Unhandled");

                            case ResourceType.RENDERABLE_INSTANCE:
                                throw new Exception("Unhandled");
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
                                    int newIndex = lvl.RenderableElements.Entries.Count;
                                    for (int x = resourceRefs[i].index; x < resourceRefs[i].index + resourceRefs[i].count; x++)
                                    {
                                        //Create the REDs entry in the destination level
                                        RenderableElements.Element renderable = new RenderableElements.Element();
                                        lvl.RenderableElements.Entries.Add(renderable);

                                        #region Copy Model
                                        //Find the submesh and associated LOD/CS2 that the original REDs points to
                                        Models.CS2.Component.LOD.Submesh origSubmesh = Content.resource.models.GetAtWriteIndex(Content.resource.reds.Entries[x].ModelIndex);
                                        Models.CS2.Component.LOD origLOD = Content.resource.models.FindModelLODForSubmesh(origSubmesh);
                                        Models.CS2.Component origComponent = Content.resource.models.FindModelComponentForSubmesh(origSubmesh);
                                        Models.CS2 origModel = Content.resource.models.FindModelForSubmesh(origSubmesh);

                                        //Check to see if the LOD exists in the destination level
                                        Models.CS2 destModel = null;
                                        List<Models.CS2> matchingDuplicates = lvl.Models.Entries.FindAll(o => o.Name == origModel.Name);
                                        for (int m = 0 ; m < matchingDuplicates.Count; m++)
                                        {
                                            for (int z = 0; z < matchingDuplicates[m].Components.Count; z++)
                                            {
                                                for (int p = 0; p < matchingDuplicates[m].Components[z].LODs.Count; p++)
                                                {
                                                    if (matchingDuplicates[m].Components[z].LODs[p].Name == origLOD.Name)
                                                    {
                                                        destModel = matchingDuplicates[m];
                                                        break;
                                                    }
                                                }
                                                if (destModel != null) break;
                                            }
                                            if (destModel != null) break;
                                        }

                                        //If it doesn't exist, copy the entire CS2 across & save so our write indexes are updated
                                        if (destModel == null)
                                        {
                                            destModel = origModel.Copy();
                                            for (int z = 0; z < destModel.Components.Count; z++)
                                            {
                                                for (int m = 0; m < destModel.Components[z].LODs.Count; m++)
                                                {
                                                    for (int p = 0; p < destModel.Components[z].LODs[m].Submeshes.Count; p++)
                                                    {
                                                        //TODO: setting mtl index to zero until we copy materials.
                                                        destModel.Components[z].LODs[m].Submeshes[p].MaterialLibraryIndex = 0;

                                                        //TODO: these are unknown
                                                        destModel.Components[z].LODs[m].Submeshes[p].UnknownIndex = -1;
                                                        destModel.Components[z].LODs[m].Submeshes[p].CollisionIndex_ = -1;
                                                        destModel.Components[z].LODs[m].Submeshes[p].HeadRelated_ = -1;
                                                    }
                                                }
                                            }
                                            lvl.Models.Entries.Add(destModel);
                                            lvl.Save();
                                        }

                                        //Make sure to point to the same submesh
                                        int origComponentIndex = origModel.Components.IndexOf(origComponent);
                                        int origLODIndex = origModel.Components[origComponentIndex].LODs.IndexOf(origLOD);
                                        int origSubmeshIndex = origModel.Components[origComponentIndex].LODs[origLODIndex].Submeshes.IndexOf(origSubmesh);

                                        //Get its index in the destination and write to renderable
                                        renderable.ModelIndex = lvl.Models.GetWriteIndex(destModel.Components[origComponentIndex].LODs[origLODIndex].Submeshes[origSubmeshIndex]);
                                        #endregion

                                        #region Find Material & Associated Textures/Shaders
                                        //Find the material that the original REDs points to & take a copy
                                        Materials.Material material = Content.resource.materials.GetAtWriteIndex(Content.resource.reds.Entries[x].MaterialIndex).Copy();

                                        //Copy textures if they don't exist in the destination level & save so our indexes are updated
                                        for (int z = 0; z < material.TextureReferences.Length; z++)
                                        {
                                            if (material.TextureReferences[z] == null) continue;
                                            if (material.TextureReferences[z].Source == Materials.Material.Texture.TextureSource.GLOBAL) continue;

                                            Textures.TEX4 origTex = Content.resource.textures.GetAtWriteIndex(material.TextureReferences[z].BinIndex);
                                            Textures.TEX4 destTex = lvl.Textures.Entries.FirstOrDefault(o => o.Name == origTex.Name);
                                            if (destTex == null)
                                            {
                                                destTex = origTex.Copy();
                                                lvl.Textures.Entries.Add(destTex);
                                            }
                                        }
                                        lvl.Save();

                                        //Get all destination level texture indexes & set to material
                                        for (int z = 0; z < material.TextureReferences.Length; z++)
                                        {
                                            if (material.TextureReferences[z] == null) continue;
                                            if (material.TextureReferences[z].Source == Materials.Material.Texture.TextureSource.GLOBAL) continue;

                                            Textures.TEX4 origTex = Content.resource.textures.GetAtWriteIndex(material.TextureReferences[z].BinIndex);
                                            Textures.TEX4 destTex = lvl.Textures.Entries.FirstOrDefault(o => o.Name == origTex.Name);
                                            material.TextureReferences[z].BinIndex = lvl.Textures.GetWriteIndex(destTex);
                                        }

                                        //TODO: copy shader
                                        material.UberShaderIndex = 0;

                                        //Copy over CST data to the destination level and update the pointer
                                        for (int z = 0; z < material.ConstantBuffers.Length; z++)
                                        {
                                            byte[] cstData = Content.resource.materials.CSTData[z];
                                            using (BinaryReader reader = new BinaryReader(new MemoryStream(cstData)))
                                            {
                                                reader.BaseStream.Position = material.ConstantBuffers[z].Offset;
                                                using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
                                                {
                                                    writer.Write(lvl.Materials.CSTData[z]);
                                                    material.ConstantBuffers[z].Offset = (int)writer.BaseStream.Position;
                                                    writer.Write(reader.ReadBytes(material.ConstantBuffers[z].Length));
                                                }
                                            }
                                        }

                                        //Write material & update indexes
                                        lvl.Materials.Entries.Add(material);
                                        lvl.Save();
                                        renderable.MaterialIndex = lvl.Materials.GetWriteIndex(material);
                                        #endregion
                                    }
                                    resourceRefs[i].index = newIndex;
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
