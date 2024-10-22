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

namespace CommandsEditor.Scripts
{
    public static class InstancedResolver
    {
        //This assumes that you've already finished doing all the threaded stuff like generating composite instance IDs
        public static void Read(LevelContent content)
        {
            /*
            //These should all resolve
            #region Resolve Physics Maps
            bool resolvedAll = true;
            for (int x = 0; x < content.resource.physics_maps.Entries.Count; x++)
            {
                PhysicsMaps.Entry item = content.resource.physics_maps.Entries[x];

                //This gives us the composite that the DYNAMIC_PHYSICS_SYSTEM resource is contained within - I apply it to the PhysicsSystem entity as that makes sense to me, so we should always have one in the composite that we match against.
                (Composite comp, EntityPath path) = content.editor_utils.GetCompositeFromInstanceID(content.commands, item.composite_instance_id);
                List<FunctionEntity> ents = comp?.GetFunctionEntitiesOfType(FunctionType.PhysicsSystem);
                if (ents == null || ents.Count != 1)
                {
                    Console.WriteLine("Failed to find PhysicsSystem for entry " + x);
                    resolvedAll = false;
                }

                //This gives us the entity that instanced the composite instance above (kinda weird, why do we need to know this?)
                EntityPath parentPath = content.editor_utils.GetHierarchyFromHandle(item.entity);
                if (parentPath != null)
                {
                    Entity parentEnt = path.GetPointedEntity(content.commands, out Composite parentComp);
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
            for (int x = 0; x < content.resource.collision_maps.Entries.Count; x++)
            {
                CollisionMaps.Entry item = content.resource.collision_maps.Entries[x];

                (Composite comp, EntityPath path) = content.editor_utils.GetCompositeFromInstanceID(content.commands, item.entity.composite_instance_id);
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
                    foreach (Composite comp2 in content.commands.Entries)
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
            for (int x = 0; x < content.mvr.Entries.Count; x++)
            {
                EntityPath path = content.editor_utils.GetHierarchyFromHandle(content.mvr.Entries[x].entity);
                if (path != null)
                {
                    Entity ent = path.GetPointedEntity(content.commands, out Composite comp);
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
            for (int x = 0; x < content.resource.resources.Entries.Count; x++)
            {
                var entry = content.resource.resources.Entries[x];

                (Composite comp, EntityPath path) = content.editor_utils.GetCompositeFromInstanceID(content.commands, entry.composite_instance_id);
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
                        Console.WriteLine("\t" + comp.name + "\n\t\t" + path.GetAsString(content.commands, comp, true));
                    }
                }
                else
                {
                    Console.WriteLine("Could not resolve composite for index " + x + " -> " + entry.resource_id.ToString() + " [" + entry.resource_id.ToByteString() + "]");

                    //These unresolvable entries are always first in the RESOURCES.BIN, and are usually at world origin.
                    //They map to the first few MVR entries, which seem to be instances of all FX related stuff.
                    //These aren't placed by the scripting system, but instead seem to just be inherently spawned in the level as some sort of precache.

                    foreach (Composite comp2 in content.commands.Entries)
                    {
                        FunctionEntity ent2 = comp2.functions.FirstOrDefault(o => o.shortGUID == entry.resource_id);
                        if (ent2 == null) continue;
                        Console.WriteLine("\tFound entity in " + comp2.name + " -> " + EntityUtils.GetName(comp2, ent2));
                    }

                    List<Movers.MOVER_DESCRIPTOR> mvrs = content.mvr.Entries.FindAll(o => o.resource_index == x);
                    foreach (var mvr in mvrs)
                    {
                        string output = content.editor_utils.PrettyPrintMoverRenderable(mvr);
                        Matrix4x4.Decompose(mvr.transform, out Vector3 scale, out Quaternion rotation, out Vector3 position);
                        Console.WriteLine("\tFound entity in MVR " + content.mvr.Entries.IndexOf(mvr) + " -> " + output);
                        Console.WriteLine("\t\tPosition: " + position + ", Rotation: " + rotation + ", Scale: " + scale);
                    }
                }
            }
            #endregion
        }

        public static void Write(LevelContent content)
        {
            //Remove everything except the unresolvable ones as i can't rewrite them...
            content.resource.resources.Entries.RemoveRange(77, content.resource.resources.Entries.Count - 77);

            //Rewrite all the ones I can
            ShortGuid resourceShortGUID = ShortGuidUtils.Generate("resource");
            for (int x = 0; x < content.commands.Entries.Count; x++)
            {
                Composite comp = content.commands.Entries[x];
                ShortGuid[] instanceIDs = content.editor_utils.GetInstanceIDsForComposite(comp);
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
                            content.resource.resources.AddUniqueResource(instanceIDs[y], comp.functions[i].resources[z].resource_id);
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
                            content.resource.resources.AddUniqueResource(instanceIDs[y], resource.shortGUID);
                        }
                    }
                }
            }

            content.resource.resources.Save();
        }
    }
}
