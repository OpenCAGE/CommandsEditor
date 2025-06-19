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
using System.Runtime.Remoting.Contexts;
using CATHODE.EXPERIMENTAL;
using System.Diagnostics.Eventing.Reader;

namespace CommandsEditor.Scripts
{
    public static class InstancedResolver
    {
        static bool done_once = false;
        static LevelContent _content;

        static ShortGuid GUID_ANIMATED_MODEL = ShortGuidUtils.Generate("AnimatedModel");
        static ShortGuid GUID_DYNAMIC_PHYSICS_SYSTEM = ShortGuidUtils.Generate("DYNAMIC_PHYSICS_SYSTEM");
        static ShortGuid GUID_resource = ShortGuidUtils.Generate("resource");
        static ShortGuid GUID_deleted = ShortGuidUtils.Generate("deleted");

        //This assumes that you've already finished doing all the threaded stuff like generating composite instance IDs
        public static void Read(LevelContent content)
        {
            _content = content;
            List<string> debug = new List<string>();

            //These should all resolve
            #region Resolve Physics Maps
            for (int x = 0; x < _content.resource.physics_maps.Entries.Count; x++)
            {
                PhysicsMaps.Entry item = _content.resource.physics_maps.Entries[x];

                //This gives us the composite that the DYNAMIC_PHYSICS_SYSTEM resource is contained within - I apply it to the PhysicsSystem entity as that makes sense to me, so we should always have one in the composite that we match against.
                (Composite comp, EntityPath path) = _content.editor_utils.GetCompositeFromInstanceID(_content.commands, item.composite_instance_id);
                List<FunctionEntity> ents = comp?.GetFunctionEntitiesOfType(FunctionType.PhysicsSystem);
                if (ents == null || ents.Count != 1)
                {
                    Console.WriteLine("Failed to find PhysicsSystem for entry " + x);
                }
                else
                {
                    debug.Add(path.GetAsString(content.commands, comp) + "\n\t" + EntityUtils.GetName(comp, ents[0]));
                }

                //This gives us the entity that instanced the composite instance above (kinda weird, why do we need to know this?)
                EntityPath parentPath = _content.editor_utils.GetHierarchyFromHandle(item.entity);
                if (parentPath != null)
                {
                    Entity parentEnt = path.GetPointedEntity(_content.commands, out Composite parentComp);
                    if (parentEnt == null)
                    {
                        Console.WriteLine("Failed to resolve parent entity for path: " + parentPath.GetAsString());
                    }
                }
                else
                {
                    Console.WriteLine("Failed to resolve parent path for entry " + x);
                }


            }
            debug.Sort();
            if (done_once)
                File.WriteAllLines("modified_physics.txt", debug);
            else
                File.WriteAllLines("vanilla_physics.txt", debug);
            debug.Clear();
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
                    else
                    {
                        debug.Add("[" + item.id.ToByteString() + "] " + path.GetAsString(content.commands, comp) + " -> " + EntityUtils.GetName(comp, ent));
                    }
                }
                else
                {
                    //Console.WriteLine("COLLISION Failed to resolve path for entry " + x);
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
            debug.Sort();
            if (done_once)
                File.WriteAllLines("modified_collisions.txt", debug);
            else
                File.WriteAllLines("vanilla_collisions.txt", debug);
            debug.Clear();
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
                    else
                    {
                        debug.Add(path.GetAsString(content.commands, comp));
                    }
                }
                else
                {
                    Console.WriteLine("Failed to resolve path for entry " + x);
                }
            }
            debug.Sort();
            if (done_once)
                File.WriteAllLines("modified_mvrs.txt", debug);
            else
                File.WriteAllLines("vanilla_mvrs.txt", debug);
            debug.Clear();
            #endregion

            //First 77 are unresolvable on TORRENS (they point to 0-83 on MVR)
            //First 294 are unresolvable on SOLACE (they point to 0-300 on MVR)
            #region Resolve Resources
            ShortGuid resourceShortGUID = ShortGuidUtils.Generate("resource");
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
                File.WriteAllLines("modified_resources.txt", debug);
            else
                File.WriteAllLines("vanilla_resources.txt", debug);
            debug.Clear();
            #endregion

            done_once = true;
        }

        public static void Write(LevelContent content)
        {
            _content = content;


            // I'm adding RENDERABLE_INSTANCE back to RadiosityProxy just so that we can find it for RESOURCES.BIN, but really that resource gets stripped out & isn't written to MVR.
            for (int i = 0; i < content.commands.Entries.Count; i++)
            {
                for (int x = 0; x < content.commands.Entries[i].functions.Count; x++)
                {
                    if (content.commands.Entries[i].functions[x].function != FunctionType.RadiosityProxy)
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


            Resources.Resource[] oldResourceMVRMappings = new Resources.Resource[_content.mvr.Entries.Count];
            for (int x = 0; x < _content.mvr.Entries.Count; x++)
            {
                oldResourceMVRMappings[x] = _content.mvr.Entries[x].resource_index == -1 ? null : _content.resource.resources.Entries[_content.mvr.Entries[x].resource_index];
            }




            //_content.resource.physics_maps.Entries.Clear();
            //_content.resource.collision_maps.Entries.Clear();
            _content.resource.resources.Entries.Clear();


            //dont remove the padding at the start
            //_content.resource.collision_maps.Entries.RemoveRange(18, _content.resource.collision_maps.Entries.Count - 18);

            //trying to see if leaving the first few on torrens helps
            //_content.resource.resources.Entries.RemoveRange(78, _content.resource.resources.Entries.Count - 78);

            bool do_logically = true;
            if (do_logically)
            {
                //Slower, but writes in a more correct order and allows evaluation of script logic
                Recurse(_content.commands.EntryPoints[0], new List<ShortGuid>());
            }
            else
            {
                //Update additional resource stuff
                foreach (Composite composite in _content.commands.Entries)
                {
                    ShortGuid[] instanceIDs = _content.editor_utils.GetInstanceIDsForComposite(composite);
                    EntityPath[] hierarchies = _content.editor_utils.GetHierarchiesForComposite(composite);
                    foreach (FunctionEntity func in composite.functions)
                    {
                        List<ResourceReference> resources = func.resources;
                        Parameter resourceParam = func.GetParameter(GUID_resource);
                        if (resourceParam != null && resourceParam.content != null && resourceParam.content.dataType == DataType.RESOURCE)
                            resources.AddRange(((cResource)resourceParam.content).value);

                        if (resources.Count == 0)
                            continue;

                        foreach (ResourceReference resRef in resources)
                        {
                            switch (resRef.resource_type)
                            {
                                case ResourceType.COLLISION_MAPPING:
                                    //WriteCollisionMap(resRef.resource_id, ShortGuid.Invalid, composite, func);
                                    break;
                            }

                            //TODO: also need to validate the positional values are correct on resource (they should match the entity)


                            //There's something wrong with this, i can't resolve a lot of stuff after rewriting - are the instance IDs wrong? Might be why the game's crashing rather than writing too many things. Would make more sense.
                            for (int i = 0; i < instanceIDs.Length; i++)
                            {
                                switch (resRef.resource_type)
                                {
                                    case ResourceType.COLLISION_MAPPING:
                                        //WriteCollisionMap(resRef.resource_id, instanceIDs[i], composite, func);
                                        break;
                                    case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                                        //if (!WritePhysicsMap(hierarchies[i], resRef.index))
                                        //    continue;
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
            }

            for (int x = 0; x < _content.mvr.Entries.Count; x++)
            {
                if (oldResourceMVRMappings[x] == null)
                {
                    _content.mvr.Entries[x].resource_index = -1;
                    continue;
                }

                Resources.Resource newMapping = _content.resource.resources.Entries.FirstOrDefault(o => o.composite_instance_id == oldResourceMVRMappings[x].composite_instance_id && o.resource_id == oldResourceMVRMappings[x].resource_id);
                _content.mvr.Entries[x].resource_index = _content.resource.resources.Entries.IndexOf(newMapping);
            }

            bool reorder_mvr = false;
            if (reorder_mvr)
            {
                Movers.MOVER_DESCRIPTOR[] oldEnvMapMVRMappings = new Movers.MOVER_DESCRIPTOR[_content.resource.env_maps.Entries.Count];
                for (int i = 0; i < _content.resource.env_maps.Entries.Count; i++)
                {
                    oldEnvMapMVRMappings[i] = _content.mvr.Entries[_content.resource.env_maps.Entries[i].MoverIndex];
                }
                
                _content.mvr.Entries = _content.mvr.Entries.OrderBy(o => o.resource_index).ToList();
                
                for (int i = 0; i < _content.resource.env_maps.Entries.Count; i++)
                {
                    _content.resource.env_maps.Entries[i].MoverIndex = _content.mvr.Entries.IndexOf(oldEnvMapMVRMappings[i]);
                }

                _content.resource.env_maps.Save();
            }


            _content.mvr.Save();

            _content.resource.collision_maps.Save();
            _content.resource.physics_maps.Save();
            _content.resource.resources.Save();
        }


        private static void Recurse(Composite comp, List<ShortGuid> hierarchy)
        {
            if (comp == null)
                return;

            List<FunctionEntity> instances = new List<FunctionEntity>();
            for (int i = 0; i < comp.functions.Count; i++)
            {

                //Parameter deleted = comp.functions[i].GetParameter(GUID_deleted);
                //if (deleted != null && ((cBool)deleted.content).value == true)
                //    continue;
                //
                //if (comp.functions[i].childLinks.FindAll(o => o.thisParamID == GUID_deleted).Count != 0)
                //    continue;
                //
                //if (comp.functions[i].GetParentLinks(comp).FindAll(o => o.linkedParamID == GUID_deleted).Count != 0)
                //    continue;

                if (!comp.functions[i].function.IsFunctionType)
                {
                    instances.Add(comp.functions[i]);
                    continue;
                }

                List<ResourceReference> resources = comp.functions[i].resources;
                Parameter resourceParam = comp.functions[i].GetParameter(GUID_resource);
                if (resourceParam != null && resourceParam.content != null && resourceParam.content.dataType == DataType.RESOURCE)
                    resources.AddRange(((cResource)resourceParam.content).value);

                ShortGuid instance_id = hierarchy.GenerateCompositeInstanceID(false);
                foreach (ResourceReference resRef in resources)
                    _content.resource.resources.AddUniqueResource(instance_id, resRef.resource_id);
            }

            for (int i = 0; i < instances.Count; i++)
            {
                //Parameter is_template = instances[i].GetParameter("is_template");
                //if (is_template != null && ((cBool)is_template.content).value == true)
                //    continue;
                //Parameter is_shared = instances[i].GetParameter("is_shared");
                //if (is_shared != null && ((cBool)is_shared.content).value == true)
                //    continue;

                List<ShortGuid> continuedHierarchy = new List<ShortGuid>(hierarchy);
                continuedHierarchy.Add(instances[i].shortGUID);
                Recurse(_content.commands.GetComposite(instances[i].function), continuedHierarchy);
            }
        }


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
            hierarchy.GoBackOneStep();
            EntityHandle compositeInstanceReference = new EntityHandle()
            {
                entity_id = hierarchy.path[hierarchy.path.Length - 2],
                composite_instance_id = hierarchy.GenerateCompositeInstanceID()
            };

            //Remove all entries that already exist for this instance
            //_content.resource.physics_maps.Entries.RemoveAll(o => o.composite_instance_id == compositeInstanceID && o.entity == compositeInstanceReference);

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

        private static void WriteCollisionMap(ShortGuid resourceID, ShortGuid comp_instance_id, Composite composite, FunctionEntity func)
        {
            //Get instance info
            EntityHandle compositeInstanceReference = new EntityHandle()
            {
                entity_id = func.shortGUID,
                composite_instance_id = comp_instance_id
            };

            //if (_content.resource.collision_maps.Entries.FindAll(o => o.entity == compositeInstanceReference && o.id == resourceID).Count != 0)
            //    return;

            //TODO: similar to PHYSICS.MAP, do we only write out if there's not another collision further up the chain?

            //Get zone ID
            ShortGuid zoneID = comp_instance_id == ShortGuid.Invalid ? ShortGuid.Invalid : new ShortGuid(1);
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
