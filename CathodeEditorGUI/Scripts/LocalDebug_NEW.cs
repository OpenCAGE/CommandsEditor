using CATHODE.Scripting;
using CATHODE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CATHODE.Scripting.Internal;

namespace CommandsEditor.Scripts
{
    //New local debugging helpers for figuring out instanced resource related stuff

    public static class LocalDebug_NEW
    {
        [Obsolete("This function is safe to use but not performant. It's intended for test code only.")]
        public static void DEBUG_DumpAllInstancedStuff(string level, string outputdir)
        {
            Console.WriteLine("Starting " + level);
            Directory.CreateDirectory(outputdir);

            LevelContent content = LevelContent.DEBUG_LoadUnthreadedAndPopulateShortGuids(level);

            /*
            for (int i = 0; i < 12; i++)
            {
                for (int x = 0; x < content.mvr.Entries[i].renderable_element_count; x++)
                {
                    var reds = content.resource.reds.Entries[(int)content.mvr.Entries[i].renderable_element_index + x];

                    var submesh = _commandsDisplay.Content.resource.models.GetAtWriteIndex(reds.ModelIndex);
                    var model = _commandsDisplay.Content.resource.models.FindModelForSubmesh(submesh);
                    var component = _commandsDisplay.Content.resource.models.FindModelComponentForSubmesh(submesh);
                    var lod = _commandsDisplay.Content.resource.models.FindModelLODForSubmesh(submesh);
                    var material = _commandsDisplay.Content.resource.materials.GetAtWriteIndex(reds.MaterialIndex);

                    Console.WriteLine(level + " [" + i + "] -> " + model.Name + " (" + lod.Name + ") -> " + material.Name);
                }
            }

            return;
            */

            //correct for the 18 entries we remove
            for (int i = 0; i < 18; i++)
                content.resource.collision_maps.Entries.Insert(0, new CollisionMaps.Entry());

            List<string> resources_dump = new List<string>();
            int resource_index = -1;

            bool DO_MVR = false;
            bool DO_RESOURCES = true;
            bool DO_COLLISION = true;
            bool DO_PHYSICS = true;

            bool ORDER = false;

            if (DO_MVR)
            {
                if (ORDER)
                    content.mvr.Entries = content.mvr.Entries.OrderBy(o => o.entity.composite_instance_id).ThenBy(o => o.entity.entity_id).ThenBy(o => o.primary_zone_id).ToList();

                foreach (var entry in content.mvr.Entries)
                {
                    resource_index++;

                    (Composite entComp, EntityPath entPath) = content.editor_utils.GetCompositeFromInstanceID(content.commands, entry.entity.composite_instance_id);
                    Entity entEnt = entComp?.GetEntityByID(entry.entity.entity_id);
                    (Composite zoneComp1, EntityPath zonePath1, Entity zoneEnt1) = content.editor_utils.GetZoneFromInstanceID(content.commands, entry.primary_zone_id);
                    (Composite zoneComp2, EntityPath zonePath2, Entity zoneEnt2) = content.editor_utils.GetZoneFromInstanceID(content.commands, entry.secondary_zone_id);

                    string convertedResoureName = "[" + resource_index + "] ";

                    string renderableInfo = content.editor_utils.PrettyPrintMoverRenderable(entry);
                    if (renderableInfo != "")
                    {
                        convertedResoureName += "\n\t REDS INFO: " + renderableInfo;
                    }

                    bool wroteSomething = false;
                    if (entComp != null)
                    {
                        convertedResoureName += "\n\t Entity Composite: " + entComp.name;
                        wroteSomething = true;
                    }
                    if (entPath != null)
                    {
                        convertedResoureName += "\n\t Entity Instance: " + entPath.GetAsString(content.commands, content.commands.EntryPoints[0], true);
                        wroteSomething = true;
                    }
                    if (entEnt != null && entComp == null)
                    {
                        convertedResoureName += "\n\t Entity Entity: " + entEnt.shortGUID + " -> can't resolve name";
                        wroteSomething = true;
                    }
                    if (entEnt != null && entComp != null)
                    {
                        convertedResoureName += "\n\t Entity Entity: " + entEnt.shortGUID + " -> " + EntityUtils.GetName(entComp, entEnt);
                        wroteSomething = true;
                    }

                    if (!wroteSomething)
                    {
                        convertedResoureName += "\n\t COULDNTRESOLVE - Entity EntityPath stuff: " + entry.entity.entity_id.ToByteString() + " -> " + entry.entity.composite_instance_id.ToByteString();

                        foreach (Composite comp2 in content.commands.Entries)
                        {
                            Entity ent2 = comp2.GetEntityByID(entry.entity.entity_id);
                            if (ent2 == null) continue;
                            convertedResoureName += "\n\t\t[ENTITY ID FOUND IN " + comp2.name + ": " + ShortGuidUtils.Generate(EntityUtils.GetName(comp2, ent2)) + "]";

                            convertedResoureName += content.editor_utils.GetAllZonesForEntity(ent2);

                            //?//continue;
                        }
                    }

                    if (zonePath1 != null && zonePath1.path.Length == 2 && zonePath1.path[0] == new ShortGuid("01-00-00-00"))
                    {
                        convertedResoureName += "\n\t Primary Zone: GLOBAL ZONE";
                    }
                    else if (zonePath1 != null && zonePath1.path.Length == 1 && zonePath1.path[0] == new ShortGuid("00-00-00-00"))
                    {
                        convertedResoureName += "\n\t Primary Zone: ZERO ZONE";
                    }
                    else
                    {
                        convertedResoureName += "\n\t Primary Zone: " + entry.primary_zone_id.ToByteString();
                        if (zoneComp1 != null)
                            convertedResoureName += "\n\t Primary Zone Composite: " + zoneComp1.name;
                        if (zonePath1 != null)
                            convertedResoureName += "\n\t Primary Zone Instance: " + zonePath1.GetAsString(content.commands, content.commands.EntryPoints[0], true);
                        if (zoneEnt1 != null && zoneComp1 == null)
                            convertedResoureName += "\n\t Primary Zone Entity: " + zoneEnt1.shortGUID + " -> can't resolve name";
                        if (zoneEnt1 != null && zoneComp1 != null)
                            convertedResoureName += "\n\t Primary Zone Entity: " + zoneEnt1.shortGUID + " -> " + EntityUtils.GetName(zoneComp1, zoneEnt1);
                    }

                    if (zonePath2 != null && zonePath2.path.Length == 2 && zonePath2.path[0] == new ShortGuid("01-00-00-00"))
                    {
                        convertedResoureName += "\n\t Secondary Zone: GLOBAL ZONE";
                    }
                    else if (zonePath2 != null && zonePath2.path.Length == 1 && zonePath2.path[0] == new ShortGuid("00-00-00-00"))
                    {
                        convertedResoureName += "\n\t Secondary Zone: ZERO ZONE";
                    }
                    else
                    {
                        convertedResoureName += "\n\t Secondary Zone: " + entry.secondary_zone_id.ToByteString();
                        if (zoneComp2 != null)
                            convertedResoureName += "\n\t Secondary Zone Composite: " + zoneComp2.name;
                        if (zonePath2 != null)
                            convertedResoureName += "\n\t Secondary Zone Instance: " + zonePath2.GetAsString(content.commands, content.commands.EntryPoints[0], true);
                        if (zoneEnt2 != null && zoneComp2 == null)
                            convertedResoureName += "\n\t Secondary Zone Entity: " + zoneEnt2.shortGUID + " -> can't resolve name";
                        if (zoneEnt2 != null && zoneComp2 != null)
                            convertedResoureName += "\n\t Secondary Zone Entity: " + zoneEnt2.shortGUID + " -> " + EntityUtils.GetName(zoneComp2, zoneEnt2);
                    }

                    resources_dump.Add(convertedResoureName);
                }
                File.WriteAllLines(outputdir + "/mover_dump_" + level.Replace("\\", "_").Replace("/", "_") + ".txt", resources_dump);
            }

            resources_dump.Clear();
            resource_index = -1;

            if (DO_RESOURCES)
            {
                foreach (var entry in content.resource.resources.Entries)
                {
                    resource_index++;

                    (Composite comp, EntityPath path) = content.editor_utils.GetCompositeFromInstanceID(content.commands, entry.composite_instance_id);

                    string convertedResoureName = "[" + resource_index + "] " + entry.resource_id.ToString() + " (" + entry.resource_id.ToByteString() + ") ";
                    if (convertedResoureName != entry.resource_id.ToByteString())
                    {
                        convertedResoureName += " [CONVERTED FROM SHORTGUID]";
                    }
                    else if (comp != null)
                    {
                        Entity ent = comp.GetEntityByID(entry.resource_id);
                        if (ent != null)
                        {
                            convertedResoureName = EntityUtils.GetName(comp, ent);
                            if (convertedResoureName != entry.resource_id.ToByteString())
                            {
                                convertedResoureName += " [CONVERTED FROM ENTITY - " + ent.variant + "]";
                            }
                            else
                            {
                                convertedResoureName += " [COULDN'T RESOLVE, BUT HAS COMP & ENT]";
                            }
                        }
                        else
                        {
                            convertedResoureName += " [COULDN'T RESOLVE, BUT HAS COMP, AND NO ENTITY]";
                        }
                    }
                    else
                    {
                        convertedResoureName += " [COULDN'T RESOLVE]";
                    }

                    convertedResoureName += " -> " + entry.composite_instance_id.ToByteString();

                    // Look at AYZ\CONTROLS\LOCKERDRESSING_A - some very odd stuff there. all the Postit composites are listed with NULL COMPOSITE NAMES, plus a load of resources that aren't there (maybe it's the sub-composite entities? -> i think it is)

                    if (comp == null && path != null)
                    {
                        resources_dump.Add(convertedResoureName + "\n\tINSTANCE: NULL COMPOSITE NAME!! (" + path.GetAsString() + ")");
                    }
                    else if (comp != null && path == null)
                    {
                        resources_dump.Add(convertedResoureName + "\n\tINSTANCE: " + comp.name + " (NULL PATH !!!!)");
                    }
                    else if (comp == null && path == null)
                    {
                        if (entry.resource_id.ToString() != "Bolt_Gun_Structural_Metal_Decal" && entry.resource_id.ToString() != "AnimatedModel")
                        {
                            foreach (Composite comp2 in content.commands.Entries)
                            {
                                Entity ent2 = comp2.GetEntityByID(entry.resource_id);
                                if (ent2 == null) continue;
                                convertedResoureName += "\n\t[ENTITY ID FOUND IN " + comp2.name + ": " + ShortGuidUtils.Generate(EntityUtils.GetName(comp2, ent2)) + "]";
                                break;
                            }

                            int ll = 0;
                            foreach (Composite comp2 in content.commands.Entries)
                            {
                                foreach (FunctionEntity funcEnt in comp2.functions)
                                {
                                    List<ResourceReference> resRefs = funcEnt.resources;
                                    Parameter resParam = funcEnt.GetParameter("resource");
                                    if (resParam != null && resParam.content != null && resParam.content.dataType == DataType.RESOURCE)
                                    {
                                        resRefs.AddRange(((cResource)resParam.content).value);
                                    }

                                    foreach (ResourceReference resRef in resRefs)
                                    {
                                        if (resRef.resource_id == entry.resource_id)
                                        {
                                            convertedResoureName += "\n\t[RESOURCE ID FOUND IN " + comp2.name + ": " + ShortGuidUtils.Generate(EntityUtils.GetName(comp2, funcEnt)) + "] (" + resRef.resource_type + ")";
                                            ll++;
                                            if (ll > 3)
                                            {
                                                convertedResoureName += "\n\t... omitting more results";
                                                break;
                                            }
                                        }
                                    }
                                    if (ll > 3)
                                        break;
                                }
                                if (ll > 3)
                                    break;
                            }
                        }
                        else
                        {
                            convertedResoureName += "\n\tNOTE: skipping " + entry.resource_id.ToString();
                        }


                        resources_dump.Add(convertedResoureName + "\n\tINSTANCE: NULL COMPOSITE NAME!! (NULL PATH !!!!)");
                        //Console.WriteLine("INSTANCE: ??");
                    }
                    else
                    {
                        resources_dump.Add(convertedResoureName + "\n\tINSTANCE: " + comp.name + " (" + path.GetAsString(content.commands, content.commands.EntryPoints[0], true) + ")");

                        //Console.WriteLine("INSTANCE: " + comp.name + " (" + path.GetAsString(content.commands, content.commands.EntryPoints[0], true) + ")");
                    }

                }
                File.WriteAllLines(outputdir + "/resources_dump_" + level.Replace("\\", "_").Replace("/", "_") + ".txt", resources_dump);
            }

            resources_dump.Clear();
            resource_index = -1;

            if (DO_COLLISION)
            {
                if (ORDER)
                    content.resource.collision_maps.Entries = content.resource.collision_maps.Entries.OrderBy(o => o.entity.composite_instance_id).ThenBy(o => o.entity.entity_id).ThenBy(o => o.zone_id).ToList();

                foreach (var entry in content.resource.collision_maps.Entries)
                {
                    resource_index++;

                    (Composite entComp, EntityPath entPath) = content.editor_utils.GetCompositeFromInstanceID(content.commands, entry.entity.composite_instance_id);
                    Entity entEnt = entComp?.GetEntityByID(entry.entity.entity_id);
                    (Composite zoneComp1, EntityPath zonePath1, Entity zoneEnt1) = content.editor_utils.GetZoneFromInstanceID(content.commands, entry.zone_id);

                    string convertedResoureName = "[" + resource_index + "] " + entry.id;

                    if (entComp != null)
                        convertedResoureName += "\n\t Entity Composite: " + entComp.name;
                    if (entPath != null)
                        convertedResoureName += "\n\t Entity Instance: " + entPath.GetAsString(content.commands, content.commands.EntryPoints[0], true);
                    if (entEnt != null && entComp == null)
                        convertedResoureName += "\n\t Entity Entity: " + entEnt.shortGUID + " -> can't resolve name";
                    if (entEnt != null && entComp != null)
                        convertedResoureName += "\n\t Entity Entity: " + entEnt.shortGUID + " -> " + EntityUtils.GetName(entComp, entEnt);

                    if (zonePath1 != null && zonePath1.path.Length == 2 && zonePath1.path[0] == new ShortGuid("01-00-00-00"))
                    {
                        convertedResoureName += "\n\t Primary Zone: GLOBAL ZONE";
                    }
                    else if (zonePath1 != null && zonePath1.path.Length == 1 && zonePath1.path[0] == new ShortGuid("00-00-00-00"))
                    {
                        convertedResoureName += "\n\t Primary Zone: ZERO ZONE";
                    }
                    else
                    {
                        convertedResoureName += "\n\t Primary Zone: " + entry.zone_id.ToByteString();
                        if (zoneComp1 != null)
                            convertedResoureName += "\n\t Primary Zone Composite: " + zoneComp1.name;
                        if (zonePath1 != null)
                            convertedResoureName += "\n\t Primary Zone Instance: " + zonePath1.GetAsString(content.commands, content.commands.EntryPoints[0], true);
                        if (zoneEnt1 != null && zoneComp1 == null)
                            convertedResoureName += "\n\t Primary Zone Entity: " + zoneEnt1.shortGUID + " -> can't resolve name";
                        if (zoneEnt1 != null && zoneComp1 != null)
                            convertedResoureName += "\n\t Primary Zone Entity: " + zoneEnt1.shortGUID + " -> " + EntityUtils.GetName(zoneComp1, zoneEnt1);
                    }

                    resources_dump.Add(convertedResoureName);
                }
                File.WriteAllLines(outputdir + "/collision_dump_" + level.Replace("\\", "_").Replace("/", "_") + ".txt", resources_dump);
            }

            resources_dump.Clear();
            resource_index = -1;

            if (DO_PHYSICS)
            {
                if (ORDER)
                    content.resource.physics_maps.Entries = content.resource.physics_maps.Entries.OrderBy(o => o.entity.composite_instance_id).ThenBy(o => o.entity.entity_id).ThenBy(o => o.composite_instance_id).ToList();

                foreach (var entry in content.resource.physics_maps.Entries)
                {
                    resource_index++;

                    (Composite entComp, EntityPath entPath) = content.editor_utils.GetCompositeFromInstanceID(content.commands, entry.entity.composite_instance_id);
                    Entity entEnt = entComp?.GetEntityByID(entry.entity.entity_id);

                    (Composite entCompParent, EntityPath entPathParent) = content.editor_utils.GetCompositeFromInstanceID(content.commands, entry.composite_instance_id);

                    string convertedResoureName = "[" + resource_index + "] " + entry.physics_system_index;

                    if (entComp != null)
                        convertedResoureName += "\n\t Parent Entity Composite: " + entComp.name;
                    if (entPath != null)
                        convertedResoureName += "\n\t Parent Entity Instance: " + entPath.GetAsString(content.commands, content.commands.EntryPoints[0], true);
                    if (entEnt != null && entComp == null)
                        convertedResoureName += "\n\t Parent Entity Entity: " + entEnt.shortGUID + " -> can't resolve name";
                    if (entEnt != null && entComp != null)
                        convertedResoureName += "\n\t Parent Entity Entity: " + entEnt.shortGUID + " -> " + EntityUtils.GetName(entComp, entEnt);

                    if (entCompParent != null)
                        convertedResoureName += "\n\t Composite Composite: " + entCompParent.name;
                    if (entPathParent != null)
                        convertedResoureName += "\n\t Composite Instance: " + entPathParent.GetAsString(content.commands, content.commands.EntryPoints[0], true);

                    resources_dump.Add(convertedResoureName);
                }
                File.WriteAllLines(outputdir + "/physics_dump_" + level.Replace("\\", "_").Replace("/", "_") + ".txt", resources_dump);
            }
        }
    }
}
