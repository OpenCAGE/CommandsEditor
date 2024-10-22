using CATHODE.Scripting;
using CATHODE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CATHODE.Scripting.Internal;
using CathodeLib;
using WebSocketSharp;
using System.IO;
using CommandsEditor.DockPanels;

namespace CommandsEditor.Scripts
{
    public static class InstancedResolver
    {
        static bool done_once = false;
        static LevelContent _content;

        static ShortGuid GUID_ANIMATED_MODEL = ShortGuidUtils.Generate("AnimatedModel");
        static ShortGuid GUID_DYNAMIC_PHYSICS_SYSTEM = ShortGuidUtils.Generate("DYNAMIC_PHYSICS_SYSTEM");
        static ShortGuid GUID_resource = ShortGuidUtils.Generate("resource");

        //This assumes that you've already finished doing all the threaded stuff like generating composite instance IDs
        public static void Read(LevelContent content)
        {
            _content = content;

            /*
            //These should all resolve
            #region Resolve Physics Maps
            bool resolvedAll = true;
            for (int x = 0; x < _content.resource.physics_maps.Entries.Count; x++)
            {
                PhysicsMaps.Entry item = _content.resource.physics_maps.Entries[x];

                //This gives us the composite that the DYNAMIC_PHYSICS_SYSTEM resource is contained within - I apply it to the PhysicsSystem entity as that makes sense to me, so we should always have one in the composite that we match against.
                (Composite comp, EntityPath path) = _content.editor_utils.GetCompositeFromInstanceID(_content.commands, item.composite_instance_id);
                List<FunctionEntity> ents = comp?.GetFunctionEntitiesOfType(FunctionType.PhysicsSystem);
                if (ents == null || ents.Count != 1)
                {
                    Console.WriteLine("Failed to find PhysicsSystem for entry " + x);
                    resolvedAll = false;
                }

                //This gives us the entity that instanced the composite instance above (kinda weird, why do we need to know this?)
                EntityPath parentPath = _content.editor_utils.GetHierarchyFromHandle(item.entity);
                if (parentPath != null)
                {
                    Entity parentEnt = path.GetPointedEntity(_content.commands, out Composite parentComp);
                    if (parentEnt == null)
                    {
                        Console.WriteLine("Failed to resolve parent entity for path: " + parentPath.GetAsString());
                        resolvedAll = false;
                    }
                }
                else
                {
                    Console.WriteLine("Failed to resolve parent path for entry " + x);
                    resolvedAll = false;
                }
            }
            if (resolvedAll)
            {
                Console.WriteLine("Resolved all physics maps!");
            }
            else
            {
                Console.WriteLine("Did not resolve all physics maps!");
            }
            #endregion

            //First 18 are always empty
            //Next set aren't resolvable as they have no composite_instance_id values - they are non-instanced collisions which are referenced by the PAK
            //Next set aren't resolvable but have composite_instance_ids - need to investigate these: perhaps related to the implicitly created and also unresolvable RESOURCE.BIN entries (does the instance id match?)
            //Next set are resolvable instanced
            #region Resolve Collision Maps
            for (int x = 0; x < _content.resource.collision_maps.Entries.Count; x++)
            {
                CollisionMaps.Entry item = _content.resource.collision_maps.Entries[x];

                (Composite comp, EntityPath path) = _content.editor_utils.GetCompositeFromInstanceID(_content.commands, item.entity.composite_instance_id);
                if (comp != null)
                {
                    Entity ent = comp?.GetEntityByID(item.entity.entity_id);
                    if (ent == null)
                    {
                        Console.WriteLine("Failed to resolve entity for path: " + path.GetAsString());
                    }
                }
                else
                {
                    Console.WriteLine("Failed to resolve path for entry " + x);
                    if (item.entity.composite_instance_id != new ShortGuid(0))
                    {
                        Console.WriteLine("\tComposite instance was set: " + item.entity.composite_instance_id.ToByteString());
                    }
                    foreach (Composite comp2 in _content.commands.Entries)
                    {
                        FunctionEntity ent2 = comp2.functions.FirstOrDefault(o => o.shortGUID == item.entity.entity_id);
                        if (ent2 == null) continue;
                        Console.WriteLine("\tFound entity in " + comp2.name + " -> " + EntityUtils.GetName(comp2, ent2));
                    }
                }

                //We can use this to get the zone entity
                //(Composite compZone, EntityPath pathZone, Entity entZone) = content.editor_utils.GetZoneFromInstanceID(content.commands, item.zone_id);
            }
            #endregion


            //First 83 are unresolvable on TORRENS
            //First 300 are unresolvable on SOLACE
            #region Resolve MVRs
            for (int x = 0; x < _content.mvr.Entries.Count; x++)
            {
                EntityPath path = _content.editor_utils.GetHierarchyFromHandle(_content.mvr.Entries[x].entity);
                if (path != null)
                {
                    Entity ent = path.GetPointedEntity(_content.commands, out Composite comp);
                    if (ent == null)
                    {
                        Console.WriteLine("Failed to resolve entity for path: " + path.GetAsString());
                    }
                }
                else
                {
                    Console.WriteLine("Failed to resolve path for entry " + x);
                }
            }
            #endregion
            */

            //First 77 are unresolvable on TORRENS (they point to 0-83 on MVR)
            //First 294 are unresolvable on SOLACE (they point to 0-300 on MVR)
            #region Resolve Resources
            ShortGuid resourceShortGUID = ShortGuidUtils.Generate("resource");
            List<string> debug = new List<string>();
            for (int x = 0; x < _content.resource.resources.Entries.Count; x++)
            {
                var entry = _content.resource.resources.Entries[x];

                (Composite comp, EntityPath path) = _content.editor_utils.GetCompositeFromInstanceID(_content.commands, entry.composite_instance_id);
                if (comp != null)
                {
                    FunctionEntity ent = null;
                    for (int i = 0; i < comp.functions.Count; i++)
                    {
                        if (comp.functions[i].resources.FindAll(o => o.resource_id == entry.resource_id).Count != 0)
                        {
                            ent = comp.functions[i];
                            break;
                        }

                        Parameter resourceParam = comp.functions[i].GetParameter(resourceShortGUID);
                        if (resourceParam != null && resourceParam.content != null && resourceParam.content.dataType == DataType.RESOURCE)
                        {
                            cResource resource = (cResource)resourceParam.content;
                            if (resource.shortGUID == entry.resource_id)
                            {
                                ent = comp.functions[i];
                                break;
                            }
                        }
                    }
                    if (ent == null)
                    {
                        Console.WriteLine("Could not find resource for index " + x + " -> " + entry.resource_id.ToString() + " [" + entry.resource_id.ToByteString() + "]");
                        Console.WriteLine("\t" + comp.name + "\n\t\t" + path.GetAsString(_content.commands, comp, true));
                    }
                    else
                    {
                        debug.Add(path.GetAsString(_content.commands, comp, false) + " -> [" + entry.resource_id.ToString() + "] " + EntityUtils.GetName(comp, ent));
                    }
                }
                else
                {
                    Console.WriteLine("Could not resolve composite for index " + x + " -> " + entry.resource_id.ToString() + " [" + entry.resource_id.ToByteString() + "]");

                    //These unresolvable entries are always first in the RESOURCES.BIN, and are usually at world origin.
                    //They map to the first few MVR entries, which seem to be instances of all FX related stuff.
                    //These aren't placed by the scripting system, but instead seem to just be inherently spawned in the level as some sort of precache.

                    foreach (Composite comp2 in _content.commands.Entries)
                    {
                        FunctionEntity ent2 = comp2.functions.FirstOrDefault(o => o.shortGUID == entry.resource_id);
                        if (ent2 == null) continue;
                        Console.WriteLine("\tFound entity in " + comp2.name + " -> " + EntityUtils.GetName(comp2, ent2));
                    }

                    List<Movers.MOVER_DESCRIPTOR> mvrs = _content.mvr.Entries.FindAll(o => o.resource_index == x);
                    foreach (var mvr in mvrs)
                    {
                        string output = _content.editor_utils.PrettyPrintMoverRenderable(mvr);
                        Matrix4x4.Decompose(mvr.transform, out Vector3 scale, out Quaternion rotation, out Vector3 position);
                        Console.WriteLine("\tFound entity in MVR " + _content.mvr.Entries.IndexOf(mvr) + " -> " + output);
                        Console.WriteLine("\t\tPosition: " + position + ", Rotation: " + rotation + ", Scale: " + scale);
                    }
                }
            }
            debug.Sort();
            if (done_once)
                File.WriteAllLines("modified.txt", debug);
            else
                File.WriteAllLines("vanilla.txt", debug);
            done_once = true;
            #endregion
        }

        public static void Write(LevelContent content)
        {
            _content = content;


            // I'm adding RENDERABLE_INSTANCE back to RadiosityProxy just so that we can find it for RESOURCES.BIN, but really that resource gets stripped out & isn't written to MVR.
            for (int i = 0; i < content.commands.Entries.Count; i++)
            {
                for (int x = 0; x < content.commands.Entries[i].functions.Count; x++)
                {
                    if (content.commands.Entries[i].functions[x].function != CommandsUtils.GetFunctionTypeGUID(FunctionType.RadiosityProxy))
                        continue;

                    Parameter parameter = content.commands.Entries[i].functions[x].GetParameter("resource");
                    if (parameter == null || parameter.content.dataType != DataType.RESOURCE)
                    {
                        Console.WriteLine("Adding new cResource parameter"); //I'm not expecting to hit this.
                        parameter = content.commands.Entries[i].functions[x].AddParameter("resource", DataType.RESOURCE);
                    }
                    cResource parameterResource = (cResource)parameter.content;
                    if (parameterResource.GetResource(ResourceType.RENDERABLE_INSTANCE) == null)
                    {
                        parameterResource.value.Add(
                            new ResourceReference()
                            {
                                resource_type = ResourceType.RENDERABLE_INSTANCE,
                                resource_id = ShortGuidUtils.Generate(EntityUtils.GetName(content.commands.Entries[i], content.commands.Entries[i].functions[x]))
                            });
                    }
                    else
                    {
                        Console.WriteLine("RadiosityProxy cResource already had RENDERABLE_INSTANCE!"); //I'm not expecting to hit this. The RENDERABLE_INSTANCE is stripped, even though the RESOURCE parameter isn't.
                    }
                }
            }


            _content.resource.physics_maps.Entries.Clear();
            //_content.resource.resources.Entries.RemoveAll(o => o.resource_id == GUID_DYNAMIC_PHYSICS_SYSTEM);
            _content.resource.resources.Entries.Clear();


            //Recurse(_content.commands.EntryPoints[0]);


            //Update additional resource stuff
            List<int> written_indexes = new List<int>();
            foreach (Composite composite in _content.commands.Entries)
            {
                List<Entity> ents = composite.GetEntities();
                foreach (var ent in ents)
                    ShortGuidUtils.Generate(EntityUtils.GetName(composite, ent));

                foreach (FunctionEntity func in composite.functions)
                {
                    List<ResourceReference> resources = func.resources;
                    Parameter resourceParam = func.GetParameter(GUID_resource);
                    if (resourceParam != null && resourceParam.content != null && resourceParam.content.dataType == DataType.RESOURCE)
                        resources.AddRange(((cResource)resourceParam.content).value);

                    if (resources.Count == 0)
                        continue;

                    ShortGuid nameHash = ShortGuidUtils.Generate(EntityUtils.GetName(composite, func));

                    List<EntityPath> hierarchies = _content.editor_utils.GetHierarchiesForEntity(composite, func);
                    List<ShortGuid> instanceIDs = new List<ShortGuid>();
                    for (int i = 0; i < hierarchies.Count; i++)
                        instanceIDs.Add(hierarchies[i].GenerateCompositeInstanceID());

                    foreach (ResourceReference resRef in resources)
                    {
                        ShortGuid id;
                        switch (resRef.resource_type)
                        {
                            case ResourceType.RENDERABLE_INSTANCE:
                            case ResourceType.COLLISION_MAPPING:
                                //if (resRef.entityID == ShortGuid.Max)
                                //    continue;
                                id = nameHash;
                                resRef.entityID = func.shortGUID;
                                WriteCollisionMap(id, null, composite, func);
                                break;
                            case ResourceType.ANIMATED_MODEL:
                                id = GUID_ANIMATED_MODEL;
                                break;
                            case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                                id = GUID_DYNAMIC_PHYSICS_SYSTEM;
                                break;
                            //case ResourceType.NAV_MESH_BARRIER_RESOURCE:
                            //TODO
                            //    break;
                            default:
                                continue;
                        }

                        //TODO: also validate the positional values are correct on resource

                        for (int i = 0; i < hierarchies.Count; i++)
                        {
                            switch (resRef.resource_type)
                            {
                                case ResourceType.COLLISION_MAPPING:
                                    WriteCollisionMap(resRef.resource_id, hierarchies[i], composite, func);
                                    break;
                                case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                                    if (written_indexes.Contains(resRef.index))
                                        continue;
                                    //written_indexes.Add(resRef.index);
                                    if (!WritePhysicsMap(hierarchies[i], resRef.index))
                                        continue;
                                    break;
                                case ResourceType.RENDERABLE_INSTANCE:
                                //Write REDS
                                case ResourceType.ANIMATED_MODEL:
                                    break;
                                default:
                                    continue;
                            }
                            _content.resource.resources.AddUniqueResource(instanceIDs[i], resRef.resource_id);
                        }
                    }
                }
            }


            /*
            //Remove everything except the unresolvable ones as i can't rewrite them...
            //content.resource.resources.Entries.RemoveRange(77, content.resource.resources.Entries.Count - 77);
            _content.resource.resources.Entries.Clear();

            //Rewrite all the ones I can
            ShortGuid resourceShortGUID = ShortGuidUtils.Generate("resource");
            for (int x = 0; x < _content.commands.Entries.Count; x++)
            {
                Composite comp = _content.commands.Entries[x];
                ShortGuid[] instanceIDs = _content.editor_utils.GetInstanceIDsForComposite(comp);
                List<Resources.Resource> resources = new List<Resources.Resource>();
                for (int i = 0; i < comp.functions.Count; i++)
                {
                    //This is wrong really, and due to how we handle these resources kinda hackily.
                    //There are resources that match entity IDs or DYNAMIC_PHYSICS_SYSTEM which I apply directly to entities.
                    //It's not the best solution and I'm not even sure if I handle it correctly when adding new ones in editor.
                    //Need to have a think about it.
                    for (int z = 0; z < comp.functions[i].resources.Count; z++)
                    {
                        for (int y = 0; y < instanceIDs.Length; y++)
                        {
                            _content.resource.resources.AddUniqueResource(instanceIDs[y], comp.functions[i].resources[z].resource_id);
                        }
                    }

                    Parameter resourceParam = comp.functions[i].GetParameter(resourceShortGUID);
                    if (resourceParam != null && resourceParam.content != null && resourceParam.content.dataType == DataType.RESOURCE)
                    {
                        cResource resource = (cResource)resourceParam.content;
                        //So here we can either loop for every resource.value, or we can just have one entry. The confusion is that typically things like models have RENDERABLE_INSTANCE and COLLISION_MAPPING both on the "resource" param
                        //Need to validate in the resolver above if there are ever multiple hits to the same function entity.

                        for (int y = 0; y < instanceIDs.Length; y++)
                        {
                            _content.resource.resources.AddUniqueResource(instanceIDs[y], resource.shortGUID);
                        }
                    }
                }
            }

            //content.resource.resources.Save();
            */
        }

        /*
        private static void Recurse(Composite composite)
        {
            foreach (FunctionEntity func in composite.functions)
            {
                List<ResourceReference> resources = func.resources;
                Parameter resourceParam = func.GetParameter(GUID_resource);
                if (resourceParam != null && resourceParam.content != null && resourceParam.content.dataType == DataType.RESOURCE)
                    resources.AddRange(((cResource)resourceParam.content).value);

                if (resources.Count == 0)
                    continue;

                ShortGuid nameHash = ShortGuidUtils.Generate(EntityUtils.GetName(composite, func));

                List<EntityPath> hierarchies = _content.editor_utils.GetHierarchiesForEntity(composite, func);
                List<ShortGuid> instanceIDs = new List<ShortGuid>();
                for (int i = 0; i < hierarchies.Count; i++)
                    instanceIDs.Add(hierarchies[i].GenerateCompositeInstanceID());

                foreach (ResourceReference resRef in resources)
                {
                    ShortGuid id;
                    switch (resRef.resource_type)
                    {
                        case ResourceType.RENDERABLE_INSTANCE:
                        case ResourceType.COLLISION_MAPPING:
                            //if (resRef.entityID == ShortGuid.Max)
                            //    continue;
                            id = nameHash;
                            resRef.entityID = func.shortGUID;
                            WriteCollisionMap(id, null, composite, func);
                            break;
                        case ResourceType.ANIMATED_MODEL:
                            id = GUID_ANIMATED_MODEL;
                            break;
                        case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                            id = GUID_DYNAMIC_PHYSICS_SYSTEM;
                            break;
                        //case ResourceType.NAV_MESH_BARRIER_RESOURCE:
                        //TODO
                        //    break;
                        default:
                            continue;
                    }

                    //TODO: also validate the positional values are correct on resource

                    for (int i = 0; i < hierarchies.Count; i++)
                    {
                        switch (resRef.resource_type)
                        {
                            case ResourceType.COLLISION_MAPPING:
                                WriteCollisionMap(resRef.resource_id, hierarchies[i], composite, func);
                                break;
                            case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                                if (!WritePhysicsMap(hierarchies[i], resRef.index))
                                    continue;
                                break;
                            case ResourceType.RENDERABLE_INSTANCE:
                            //Write REDS
                            case ResourceType.ANIMATED_MODEL:
                                break;
                            default:
                                continue;
                        }
                        _content.resource.resources.AddUniqueResource(instanceIDs[i], resRef.resource_id);
                    }
                }
            }
        }
        */

        private static bool WritePhysicsMap(EntityPath hierarchy, int physics_system_index)
        {
            //If a composite further up in the path contains a PhysicsSystem too we shouldn't write this one out (NOTE: We also shouldn't write static stuff out by the looks of it)
            Composite comp = _content.commands.EntryPoints[0];
            //for (int x = 0; x < hierarchy.path.Count - 1; x++)
            //{
            //    FunctionEntity compInst = comp.functions.FirstOrDefault(o => o.shortGUID == hierarchy.path[x]);
            //    if (compInst == null)
            //        break;
            //
            //    comp = _content.commands.GetComposite(compInst.function);
            //    if (x < hierarchy.path.Count - 3 && comp.GetFunctionEntitiesOfType(FunctionType.PhysicsSystem).Count != 0)
            //    {
            //        Console.WriteLine(comp.name);
            //        return false;
            //    }
            //}

            //Get instance info
            (Vector3 position, Quaternion rotation) = CommandsUtils.CalculateInstancedPosition(hierarchy);
            ShortGuid compositeInstanceID = hierarchy.GenerateCompositeInstanceID();
            hierarchy.path.RemoveAt(hierarchy.path.Count - 2);
            EntityHandle compositeInstanceReference = new EntityHandle()
            {
                entity_id = hierarchy.path[hierarchy.path.Count - 2],
                composite_instance_id = hierarchy.GenerateCompositeInstanceID()
            };

            //Remove all entries that already exist for this instance
            _content.resource.physics_maps.Entries.RemoveAll(o => o.composite_instance_id == compositeInstanceID && o.entity == compositeInstanceReference);

            //Make a new entry for the instance
            _content.resource.physics_maps.Entries.Add(new PhysicsMaps.Entry()
            {
                physics_system_index = physics_system_index,
                resource_type = GUID_DYNAMIC_PHYSICS_SYSTEM,
                composite_instance_id = compositeInstanceID,
                entity = compositeInstanceReference,
                Position = position,
                Rotation = rotation
            });
            return true;
        }

        private static void WriteCollisionMap(ShortGuid resourceID, EntityPath hierarchy, Composite composite, FunctionEntity func)
        {
            //Get instance info
            EntityHandle compositeInstanceReference = new EntityHandle()
            {
                entity_id = func.shortGUID,
                composite_instance_id = hierarchy == null ? ShortGuid.Invalid : hierarchy.GenerateCompositeInstanceID()
            };

            if (_content.resource.collision_maps.Entries.FindAll(o => o.entity == compositeInstanceReference && o.id == resourceID).Count != 0)
                return;

            //TODO: similar to PHYSICS.MAP, do we only write out if there's not another collision further up the chain?

            //Get zone ID
            ShortGuid zoneID = hierarchy == null ? ShortGuid.Invalid : new ShortGuid(1);
            /*
            if (hierarchy != null)
            {
                //TODO: this needs speeding up
                CancellationToken ct = new CancellationToken();
                _content.editor_utils.TryFindZoneForEntity(func, composite, out Composite zoneComp, out FunctionEntity zoneEnt, ct);
                if (zoneComp != null && zoneEnt != null)
                {
                    //TODO: need to figure out how we know which zone instance to point at!
                    List<EntityPath> zoneInstances = _content.editor_utils.GetHierarchiesForEntity(zoneComp, zoneEnt);
                    if (zoneInstances.Count > 0)
                        zoneID = zoneInstances[0].GenerateZoneID();
                }
            }
            */

            //Make a new entry for the instance
            _content.resource.collision_maps.Entries.Add(new CollisionMaps.Entry()
            {
                id = resourceID,
                entity = compositeInstanceReference,
                zone_id = zoneID
            });
        }
    }
}
