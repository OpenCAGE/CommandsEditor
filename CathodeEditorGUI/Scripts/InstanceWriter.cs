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
#if NO
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
                foreach (var function in content.commands.Entries[i].functions_dictionary.Values)
                    if (!CommandsUtils.FunctionTypeExists(function.function))
                        compInstances.Add(function.shortGUID, content.commands.GetComposite(function.function));
                cachedCompInstances.Add(content.commands.Entries[i].shortGUID, compInstances);
            }
            
            */
            content.resource.collision_maps.Save();
            content.resource.collision_maps.Entries.Clear();

            for (int i= 0; i < 18; i++)
                content.resource.collision_maps.Entries.Add(new CollisionMaps.COLLISION_MAPPING());

            for (int i = 0; i < content.commands.Entries.Count; i++)
            {
                foreach (var function in content.commands.Entries[i].functions_dictionary.Values)
                {
                    ResourceReference resource = function.GetResource(ResourceType.COLLISION_MAPPING, true);
                    if (resource == null) continue;

                    ShortGuid resourceID = ShortGuidUtils.Generate(content.commands.Utils.GetEntityName(content.commands.Entries[i], function));

                    content.resource.collision_maps.Entries.Add(new CollisionMaps.COLLISION_MAPPING()
                    {
                        ResourceGUID = resourceID,
                        Entity = new EntityHandle() { entity_id = function.shortGUID, composite_instance_id = ShortGuid.Invalid },
                        ZoneID = ShortGuid.Invalid
                    });

                    content.resource.resources.Entries.Add(new Resources.Resource()
                    {
                        composite_instance_id = ShortGuid.Invalid,
                        resource_id = resourceID
                    });
                }
            }

            ShortGuid GUID_composites = ShortGuidUtils.Generate("composites");
            ShortGuid GUID_reference = ShortGuidUtils.Generate("reference");
            for (int i = 0; i < content.commands.Entries.Count; i++)
            {
                foreach (var function in content.commands.Entries[i].functions_dictionary.Values)
                {
                    if (function.function != FunctionType.Zone)
                        continue;

                    List<EntityPath> zonePaths = content.EditorUtils.GetHierarchiesForEntity(content.commands.Entries[i], function);
                    List<ShortGuid> zoneInstanceIDs = new List<ShortGuid>();
                    for (int p = 0; p < zonePaths.Count; p++)
                        zoneInstanceIDs.Add(zonePaths[p].GenerateZoneID());

                    List<EntityConnector> compositesParams = function.childLinks.FindAll(o => o.thisParamID == GUID_composites && o.linkedParamID == GUID_reference);
                    for (int z = 0; z < compositesParams.Count; z++)
                    {
                        FunctionEntity linked = content.commands.Entries[i].functions_dictionary.Values.FirstOrDefault(o => o.shortGUID == compositesParams[z].linkedEntityID);
                        if (linked == null)
                        {
                            AliasEntity linkedAlias = content.commands.Entries[i].aliases.FirstOrDefault(o => o.shortGUID == compositesParams[z].linkedEntityID);
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
                                for (int e = 0; e < linkedTrigSeq.sequence.Count; e++)
                                {
                                    FunctionEntity func = ResolveHierarchyToFunction(linkedTrigSeq.sequence[e].connectedEntity.path, content.commands, content.commands.Entries[i]);
                                    if (func == null) continue;
                                    Composite comp = content.commands.Entries.FirstOrDefault(o => o.shortGUID == func.function);
                                    if (comp == null) continue;

                                    List<FunctionEntity> resourceEnts = new List<FunctionEntity>();
                                    foreach (var resFunc in comp.functions_dictionary.Values)
                                    {
                                        ResourceReference resource = resFunc.GetResource(ResourceType.COLLISION_MAPPING, true);
                                        if (resource == null) continue;
                                        resourceEnts.Add(resFunc);
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

                                        ShortGuid resourceID = ShortGuidUtils.Generate(content.commands.Utils.GetEntityName(comp, func));

                                        content.resource.collision_maps.Entries.Add(new CollisionMaps.COLLISION_MAPPING()
                                        {
                                            ResourceGUID = resourceID,
                                            Entity = instanceInfo,
                                            ZoneID = zoneInstanceID
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

            content.Level.CollisionMaps.Save();

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
#endif
}
