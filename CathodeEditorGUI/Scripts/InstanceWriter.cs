using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CommandsEditor.Scripts
{
    public class InstanceWriter
    {
        public InstanceWriter()
        {

        }

        public void WriteInstances(LevelContent content)
        {
            /*
            Dictionary<ShortGuid, List<ShortGuid>> cachedCompInstances = new Dictionary<ShortGuid, Dictionary<ShortGuid, Composite>>();
            for (int i = 0; i < content.commands.Entries.Count; i++)
            {
                Dictionary<ShortGuid, Composite> compInstances = new Dictionary<ShortGuid, Composite>();
                for (int x = 0; x < content.commands.Entries[i].functions.Count; x++)
                    if (!CommandsUtils.FunctionTypeExists(content.commands.Entries[i].functions[x].function))
                        compInstances.Add(content.commands.Entries[i].functions[x].shortGUID, content.commands.GetComposite(content.commands.Entries[i].functions[x].function));
                cachedCompInstances.Add(content.commands.Entries[i].shortGUID, compInstances);
            }
            
            */
            content.resource.collision_maps.Save();
            content.resource.collision_maps.Entries.Clear();

            for (int i= 0; i < 18; i++)
                content.resource.collision_maps.Entries.Add(new CollisionMaps.Entry());

            for (int i = 0; i < content.commands.Entries.Count; i++)
            {
                for (int x = 0; x < content.commands.Entries[i].functions.Count; x++)
                {
                    ResourceReference resource = content.commands.Entries[i].functions[x].GetResource(ResourceType.COLLISION_MAPPING, true);
                    if (resource == null) continue;

                    ShortGuid resourceID = ShortGuidUtils.Generate(EntityUtils.GetName(content.commands.Entries[i], content.commands.Entries[i].functions[x]));

                    content.resource.collision_maps.Entries.Add(new CollisionMaps.Entry()
                    {
                        id = resourceID,
                        entity = new EntityHandle() { entity_id = content.commands.Entries[i].functions[x].shortGUID, composite_instance_id = ShortGuid.Invalid },
                        zone_id = ShortGuid.Invalid
                    });

                    content.resource.resources.Entries.Add(new Resources.Resource()
                    {
                        composite_instance_id = ShortGuid.Invalid,
                        resource_id = resourceID
                    });
                }
            }

            ShortGuid GUID_Zone = CommandsUtils.GetFunctionTypeGUID(FunctionType.Zone);
            ShortGuid GUID_composites = ShortGuidUtils.Generate("composites");
            ShortGuid GUID_reference = ShortGuidUtils.Generate("reference");
            for (int i = 0; i < content.commands.Entries.Count; i++)
            {
                for (int x = 0; x < content.commands.Entries[i].functions.Count; x++)
                {
                    if (content.commands.Entries[i].functions[x].function != GUID_Zone)
                        continue;

                    List<EntityPath> zonePaths = content.editor_utils.GetHierarchiesForEntity(content.commands.Entries[i], content.commands.Entries[i].functions[x]);
                    List<ShortGuid> zoneInstanceIDs = new List<ShortGuid>();
                    for (int p = 0; p < zonePaths.Count; p++)
                        zoneInstanceIDs.Add(zonePaths[p].GenerateZoneID());

                    List<EntityConnector> compositesParams = content.commands.Entries[i].functions[x].childLinks.FindAll(o => o.thisParamID == GUID_composites && o.linkedParamID == GUID_reference);
                    for (int z = 0; z < compositesParams.Count; z++)
                    {
                        FunctionEntity linked = content.commands.Entries[i].functions.FirstOrDefault(o => o.shortGUID == compositesParams[z].linkedEntityID);
                        if (linked == null)
                        {
                            AliasEntity linkedAlias = content.commands.Entries[x].aliases.FirstOrDefault(o => o.shortGUID == compositesParams[z].linkedEntityID);
                            if (linkedAlias != null)
                                linked = ResolveHierarchyToFunction(linkedAlias.alias.path, content.commands, content.commands.Entries[i]);
                        }
                        if (linked == null)
                            continue;

                        for (int p = 0; p < zonePaths.Count; p++)
                        {
                            ShortGuid zoneInstanceID = zoneInstanceIDs[p];
                            if (linked is TriggerSequence linkedTrigSeq)
                            {
                                for (int e = 0; e < linkedTrigSeq.entities.Count; e++)
                                {
                                    FunctionEntity func = ResolveHierarchyToFunction(linkedTrigSeq.entities[e].connectedEntity.path, content.commands, content.commands.Entries[i]);
                                    if (func == null) continue;
                                    Composite comp = content.commands.Entries.FirstOrDefault(o => o.shortGUID == func.function);
                                    if (comp == null) continue;

                                    List<FunctionEntity> resourceEnts = new List<FunctionEntity>();
                                    for (int l = 0; l < comp.functions.Count; l++)
                                    {
                                        ResourceReference resource = comp.functions[l].GetResource(ResourceType.COLLISION_MAPPING, true);
                                        if (resource == null) continue;
                                        resourceEnts.Add(comp.functions[l]);
                                    }

                                    if (resourceEnts.Count == 0) continue;

                                    for (int l = 0; l < resourceEnts.Count; l++)
                                    {
                                        EntityPath path = new EntityPath(new ShortGuid[1] { func.shortGUID });
                                        //path.PrependPath(linkedTrigSeq.entities[e].connectedEntity);
                                        //path.PrependPath(zonePaths[p]);

                                        EntityHandle instanceInfo = new EntityHandle()
                                        {
                                            entity_id = func.shortGUID,
                                            composite_instance_id = path.GenerateCompositeInstanceID()
                                        };

                                        ShortGuid resourceID = ShortGuidUtils.Generate(EntityUtils.GetName(comp, func));

                                        content.resource.collision_maps.Entries.Add(new CollisionMaps.Entry()
                                        {
                                            id = resourceID,
                                            entity = instanceInfo,
                                            zone_id = zoneInstanceID
                                        });

                                        content.resource.resources.Entries.Add(new Resources.Resource()
                                        {
                                            composite_instance_id = instanceInfo.composite_instance_id,
                                            resource_id = resourceID
                                        });
                                    }
                                }
                            }
                            else
                            {
                                //TODO: not implemented yet as i'm not entirely sure if we need to
                                //throw new Exception("not implemented yet");
                            }
                        }
                    }
                }
            }

            content.resource.collision_maps.Save();

            string sdfdf = "";


                /*

                Action < EntityPath, out Composite, out Entity > add = delegate (EntityPath test, out Composite comp, out Entity ent)
                {
                    comp = content.commands.EntryPoints[0];
                    ent = null;
                    for (int i = 0; i < test.path.Count; i++)
                    {
                        Composite next = cachedCompInstances[comp.shortGUID][test.path[i]];
                        if (next == null)
                        {
                            ent = comp.GetEntityByID(test.path[i]);
                            break;
                        }
                        comp = next;
                    }
                };

                */
            }

        private FunctionEntity ResolveHierarchyToFunction(ShortGuid[] hierarchy, Commands commands, Composite start)
        {
            Composite c = start;
            for (int p = 0; p < hierarchy.Length; p++)
            {
                FunctionEntity f = c.functions.FirstOrDefault(o => o.shortGUID == hierarchy[p]);
                c = f != null ? commands.Entries.FirstOrDefault(o => o.shortGUID == f.function) : null;

                if (c == null)
                    break;

                if (p == hierarchy.Length - 1 || hierarchy[p + 1] == ShortGuid.Invalid)
                    return f;
            }
            return null;
        }

    }
}
