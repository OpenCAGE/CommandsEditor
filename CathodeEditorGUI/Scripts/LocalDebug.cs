using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms.Design;
using System.CodeDom;
using System.Windows.Media.Media3D;
using static CATHODE.Models;
using System.Windows.Media.Animation;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using static CATHODE.Scripting.EnumUtils;
using CommandsEditor.UserControls;
using CATHODE.EXPERIMENTAL;

namespace CommandsEditor
{
    public static class LocalDebug
    {
        public static void MAPTEST(string path)
        {
            /*
            Console.WriteLine("Loading Commands");
            Commands commands = new Commands(path + "/WORLD/COMMANDS.PAK");
            Console.WriteLine("Generating Instances");
            EditorUtils.GenerateCompositeInstances(commands);

            int resolved = 0;
            Console.WriteLine("Loading Collision Maps");
            CollisionMaps collisionMaps = new CollisionMaps(path + "/WORLD/COLLISION.MAP");
            string json = JsonConvert.SerializeObject(collisionMaps.Entries, Formatting.Indented);
            File.WriteAllText("CollisionMaps.json", json);
            foreach (CollisionMaps.Entry entry in collisionMaps.Entries)
            {
                if (entry.entity.composite_instance_id == ShortGuid.Invalid || entry.entity.entity_id == ShortGuid.Invalid)
                {
                    Console.WriteLine("Skipping invalid");
                    continue;
                }

                EntityHierarchy hierarchy = EditorUtils.GetHierarchyFromReference(entry.entity);
                if (hierarchy == null)
                {
                    Console.WriteLine("FAILED TO RESOLVE");
                    continue;
                }
                Entity ent = hierarchy.GetPointedEntity(commands, out Composite comp);
                Console.WriteLine(hierarchy.GetHierarchyAsString(commands, comp, true));
                resolved++;
            }
            Console.WriteLine("Resolved: " + resolved);
            Console.WriteLine("Not Resolved: " + (collisionMaps.Entries.Count - resolved));

            /*
            resolved = 0;
            Console.WriteLine("Loading Physics Maps");
            PhysicsMaps physicsMaps = new PhysicsMaps(path + "/WORLD/PHYSICS.MAP");
            string json2 = JsonConvert.SerializeObject(physicsMaps.Entries, Formatting.Indented);
            File.WriteAllText("PhysicsMaps.json", json2);
            foreach (PhysicsMaps.Entry entry in physicsMaps.Entries)
            {
                if (entry.entity.composite_instance_id == ShortGuid.Invalid || entry.entity.entity_id == ShortGuid.Invalid)
                {
                    Console.WriteLine("Skipping invalid");
                    continue;
                }

                EntityHierarchy hierarchy = EditorUtils.GetHierarchyFromReference(entry.entity);
                if (hierarchy == null)
                {
                    Console.WriteLine("FAILED TO RESOLVE");
                    continue;
                }
                Entity ent = hierarchy.GetPointedEntity(commands, out Composite comp);
                Console.WriteLine(hierarchy.GetHierarchyAsString(commands, comp, true));
                resolved++;
            }
            Console.WriteLine("Resolved: " + resolved);
            Console.WriteLine("Not Resolved: " + (physicsMaps.Entries.Count - resolved));
            */

            string breakhere = "";
        }

        public static void mvr_test()
        {
#if DEBUG
            string level = "tech_rnd";

            Movers mvr = new Movers(SharedData.pathToAI + "\\data\\ENV\\PRODUCTION\\" + level + "\\WORLD\\MODELS.MVR");
            Lights lights = new Lights(SharedData.pathToAI + "\\data\\ENV\\PRODUCTION\\" + level + "\\WORLD\\LIGHTS.BIN");
            AlphaLightLevel lightsAlpha = new AlphaLightLevel(SharedData.pathToAI + "\\data\\ENV\\PRODUCTION\\" + level + "\\WORLD\\ALPHALIGHT_LEVEL.BIN");
            EnvironmentMaps env = new EnvironmentMaps(SharedData.pathToAI + "\\data\\ENV\\PRODUCTION\\" + level + "\\WORLD\\ENVIRONMENTMAP.BIN");
            //Resources res = new Resources(SharedData.pathToAI + "\\data\\ENV\\PRODUCTION\\" + level + "\\WORLD\\RESOURCES.BIN");

            mvr.Entries.Clear();
            env.Entries.Clear();
            //res.Entries.Clear();

            mvr.Save();
            env.Save();
            //res.Save();

            File.Delete(lights.Filepath);
            File.Delete(lightsAlpha.Filepath); //deleting this one causes significant visual changes (anything with alpha doesn't render, or render properly)
#endif
        }

        public static void DoCheckOnNodegraph()
        {
#if DEBUG

            List<string> files = Directory.GetFiles("F:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\data_orig\\ENV\\Production", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            foreach (string file in files)
            {
                Commands commands = new Commands(file);
                EntityFlowgraph flowgraph = new EntityFlowgraph();
                flowgraph.Show();
                flowgraph.DEBUG_LoadAll_Test(commands, true);
                flowgraph.Hide();
                flowgraph.Dispose();
            }
#endif
        }

        public static void checkphysicssystempositions()
        {
#if DEBUG

            List<string> files = Directory.GetFiles("F:\\Alien Isolation Versions\\Alien Isolation PC Final\\DATA\\ENV", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands commands = new Commands(file);
                Parallel.ForEach(commands.Entries, comp =>
                {
                    Parallel.ForEach(comp.functions, func =>
                    {
                        if (func.function == ShortGuidUtils.Generate("PhysicsSystem"))
                        {
                            Parameter pos = func.GetParameter("position");
                            if (pos != null)
                            {
                                string sdfsdf = "";
                            }
                        }
                    });
                });
            });
#endif
        }

        public static void checkprefabinstances()
        {
#if DEBUG

            List<string> files = Directory.GetFiles("F:\\Alien Isolation Versions\\Alien Isolation PC Final\\DATA\\ENV\\PRODUCTION", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands commands = new Commands(file);
                Parallel.ForEach(commands.Entries, comp =>
                {
                    Parallel.ForEach(comp.functions, func =>
                    {
                        if (func.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.Zone))
                        {
                            List<EntityConnector> parentLinks = func.GetParentLinks(comp);
                            if (parentLinks.FindAll(o => o.thisParamID == ShortGuidUtils.Generate("composites")).Count != 0)
                            {
                                string sdfsdf = "";
                            }
                            if (func.childLinks.FindAll(o => o.thisParamID == ShortGuidUtils.Generate("composites")).Count != 0)
                            {
                                string sdfsdf = "";
                            }
                        }
                    });
                });
            });
#endif
        }

        public static void checkresources()
        {
#if DEBUG

            List<string> files = Directory.GetFiles("F:\\Alien Isolation Versions\\Alien Isolation PC Final\\DATA\\ENV\\PRODUCTION", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands commands = new Commands(file);
                Parallel.ForEach(commands.Entries, comp =>
                {
                    Parallel.ForEach(comp.functions, func =>
                    {
                        ResourceReference resource = func.GetResource(ResourceType.COLLISION_MAPPING);
                        if (resource != null)
                        {
                            Console.WriteLine("COLLISION_MAPPING: " + func.function.ToString());
                        }
                        ResourceReference resource2 = func.GetResource(ResourceType.RENDERABLE_INSTANCE);
                        if (resource2 != null)
                        {
                            Console.WriteLine("RENDERABLE_INSTANCE: " + func.function.ToString());
                        }
                    });
                });
            });
#endif
        }

        public static void checkaliases()
        {
#if DEBUG

            List<string> files = Directory.GetFiles("D:\\Alien Isolation Modding\\Isolation Mod Tools\\Alien Isolation PC Final/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands commands = new Commands(file);
                Parallel.ForEach(commands.Entries, comp =>
                {
                    Parallel.ForEach(comp.aliases, func =>
                    {
                        for (int i = 0; i < func.childLinks.Count; i++)
                        {
                            Entity childEnt = comp.GetEntityByID(func.childLinks[i].linkedEntityID);
                            switch (childEnt.variant)
                            {
                                case EntityVariant.FUNCTION:
                                    Console.WriteLine("FUNCTION: " + ((FunctionEntity)childEnt).function.ToString());
                                    break;
                                case EntityVariant.VARIABLE:
                                    Console.WriteLine("VARIABLE: " + ((VariableEntity)childEnt).name.ToString());
                                    break;
                                case EntityVariant.PROXY:
                                    string sadfsfd = "";
                                    break;
                                case EntityVariant.ALIAS:
                                    Console.WriteLine("ALIAS! " );
                                    break;
                            }
                        }
                        //if (func.childLinks.Count != 0)
                        //{
                        //    Console.WriteLine(file + "\n\t" + comp.name + "\n\t" + func.shortGUID.ToByteString());
                        //}
                    });;
                });
            });
#endif
        }

        public static void checkvariables()
        {
#if DEBUG

            List<string> files = Directory.GetFiles("D:\\Alien Isolation Modding\\Isolation Mod Tools\\Alien Isolation PC Final/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands commands = new Commands(file);
                Parallel.ForEach(commands.Entries, comp =>
                {
                    Parallel.ForEach(comp.variables, func =>
                    {
                        if (func.childLinks.Count == 0 && func.parameters.Count == 0)
                        {
                            //Console.WriteLine("NO PARAMS OR LINKS");
                        }
                        if (func.parameters.Count > 1) 
                        {
                            Console.WriteLine("PARAMS: " + func.parameters.Count);
                        }
                        List<string> paramStrs = new List<string>();
                        foreach (EntityConnector connector in func.childLinks)
                        {
                            if (!paramStrs.Contains(connector.thisParamID.ToString()))
                                paramStrs.Add(connector.thisParamID.ToString());
                        }
                        if (paramStrs.Count > 1)
                        {
                            Console.WriteLine("UNIQUE LINKS: " + paramStrs.Count);
                        }
                    });
                });
            });
#endif
        }

        //Go to the top of the connection chain, and follow it down to get the number of nodes in the sequence - keep a track of the longest sequences and where they are
        public static void CountEntitySequences()
        {
            string OnlyDoOneComp = "";//"SCRIPT_STORYMISSION\\M04_WELCOME_TO_THE_SEVASTOPOL\\M04_PART_01\\M04_PT01"; //keep this blank to dump all

            List<string> files = Directory.GetFiles("F:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\data_orig\\ENV\\Production", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            List<string> output = new List<string>();
            List<int> counts = new List<int>();
            int max = 0;
            foreach (string file in files)
            {
                output.Add("====== Loading =====");
                output.Add(file);
                output.Add("====================");

                Commands commands = new Commands(file);
                EntityUtils.LinkCommands(commands);
                ShortGuidUtils.LinkCommands(commands);

                foreach (Composite composite in commands.Entries)
                {
                    if (OnlyDoOneComp != "" && composite.name != OnlyDoOneComp)
                        continue;

                    CommandsUtils.PurgeDeadLinks(commands, composite);

                    List<Entity> entities = composite.GetEntities();
                    List<Entity> checkedEnts = new List<Entity>();
                    foreach (Entity entity in entities)
                    {
                        if (checkedEnts.Contains(entity))
                            continue;

                        List<Entity> connectedEntities = new List<Entity>();
                        RecurseEntityConnections(entity, composite, connectedEntities);

                        if (connectedEntities.Count != 1)
                        {
                            if (OnlyDoOneComp != "")
                            {
                                Console.WriteLine(composite.name + " -> " + connectedEntities.Count);
                            }
                            else
                            {
                                //foreach (Entity ent in connectedEntities)
                                //{
                                //    Console.WriteLine(EntityUtils.GetName(composite, ent));
                                //}

                                output.Add(composite.name + " -> " + connectedEntities.Count);
                                counts.Add(connectedEntities.Count);
                            }
                        }

                        if (max < connectedEntities.Count)
                            max = connectedEntities.Count;

                        checkedEnts.AddRange(connectedEntities);
                    }

                    if (OnlyDoOneComp != "")
                        return;
                }
            }
            output.Add("-----------");
            output.Add("Max: " + max);
            File.WriteAllLines("output.txt", output);

            counts.Sort();
            output.Clear();
            foreach (int count in counts)
            {
                output.Add(count.ToString());
            }
            File.WriteAllLines("counts.txt", output);
        }
        private static void RecurseEntityConnections(Entity entity, Composite comp, List<Entity> checkedEnts)
        {
            if (checkedEnts.Contains(entity))
                return;
            checkedEnts.Add(entity);

            List<EntityConnector> connectionsIn = entity.GetParentLinks(comp);
            foreach (EntityConnector connection in connectionsIn)
            {
                Entity ent = comp.GetEntityByID(connection.linkedEntityID);
                if (ent == null) continue;
                RecurseEntityConnections(ent, comp, checkedEnts);
            }

            List<EntityConnector> connectionsOut = entity.childLinks;
            foreach (EntityConnector connection in connectionsOut)
            {
                Entity ent = comp.GetEntityByID(connection.linkedEntityID);
                if (ent == null) continue;
                RecurseEntityConnections(ent, comp, checkedEnts);
            }
        }

        //This checks to make sure that variable entities don't have any links that aren't the variable name (spoiler: they don't)
        public static void CheckVariablesHaveNoRogueParams()
        {
            List<string> files = Directory.GetFiles("F:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\data_orig\\ENV\\Production", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            foreach (string file in files)
            {
                Commands commands = new Commands(file);
                foreach (Composite comp in commands.Entries)
                {
                    foreach (VariableEntity var in comp.variables)
                    {
                        List<EntityConnector> connectionsIn = var.GetParentLinks(comp);
                        List<EntityConnector> connectionsOut = var.childLinks;

                        foreach (EntityConnector connection in connectionsIn)
                        {
                            if (connection.thisParamID != var.name)
                            {
                                throw new Exception("Rogue");
                            }
                        }
                        foreach (EntityConnector connection in connectionsOut)
                        {
                            if (connection.thisParamID != var.name)
                            {
                                throw new Exception("Rogue");
                            }
                        }

                        foreach (Parameter param in var.parameters)
                        {
                            if (param.name != var.name)
                            {
                                throw new Exception("Rogue");
                            }
                        }
                    }
                }
            }
        }

        public static void checkanims()
        {
#if DEBUG

            List<string> files = Directory.GetFiles("D:\\Alien Isolation Modding\\Isolation Mod Tools\\Alien Isolation PC Final/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands commands = new Commands(file);
                Parallel.ForEach(commands.Entries, comp =>
                {
                    Parallel.ForEach(comp.functions, func =>
                    {
                        Parameter Animation = func.GetParameter("Animation");
                        Parameter AnimationSet = func.GetParameter("AnimationSet");
                        if (AnimationSet == null && Animation != null)
                        {
                            Console.WriteLine(comp.name + " -> " + EntityUtils.GetName(comp, func) + " (" + func.function.ToString() + ")" + "\n\tWARNING: Found Animation without AnimationSet\n\n");
                        }
                        if (AnimationSet != null && Animation == null)
                        {
                            Console.WriteLine(comp.name + " -> " + EntityUtils.GetName(comp, func) + " (" + func.function.ToString() + ")" + "\n\tWARNING: Found AnimationSet without Animation\n\n");
                        }
                        if (AnimationSet != null && Animation != null)
                        {
                            //Console.WriteLine("Animation matched with AnimationSet");
                        }
                    });
                });
            });
#endif
        }

        public static void DumpAllEnts(string alien_path, string output_append)
        {
#if DEBUG

            List<string> files = Directory.GetFiles(alien_path + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Dictionary<string, List<string>> usesOfFunction = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> usesOfComposite = new Dictionary<string, List<string>>();

            foreach (FunctionType func in (FunctionType[])Enum.GetValues(typeof(FunctionType)))
            {
                usesOfFunction.Add(func.ToString(), new List<string>());
            }

            foreach (string file in files)
            {
                string[] split = file.Replace("\\", "/").Split(new[] { "/DATA/ENV/PRODUCTION/" }, StringSplitOptions.None);
                string mapName = split[split.Length - 1].Substring(0, split[split.Length - 1].Length - ("/WORLD/COMMANDS.PAK").Length);

                Commands commands = new Commands(file);
                foreach (Composite comp in commands.Entries)
                {
                    if (!usesOfComposite.ContainsKey(comp.name))
                    {
                        usesOfComposite.Add(comp.name, new List<string>());
                    }
                    usesOfComposite[comp.name].Add(mapName);

                    foreach (FunctionEntity ent in comp.functions)
                    {
                        if (commands.GetComposite(ent.function) != null) continue;
                        //if (!CommandsUtils.FunctionTypeExists(ent.function)) continue; //NOT USING THIS ANYMORE AS DELETED FUNCTIONS COULD WILL NOT BE COUNTED FOR...

                        string type = !CommandsUtils.FunctionTypeExists(ent.function) ? "DELETED FUNCTION OR COMPOSITE: " + ent.function.ToByteString() : CommandsUtils.GetFunctionType(ent.function).ToString();
                        if (!usesOfFunction.ContainsKey(type))
                        {
                            usesOfFunction.Add(type, new List<string>());
                        }
                        if (!usesOfFunction[type].Contains(comp.name))
                        {
                            usesOfFunction[type].Add(comp.name);
                        }
                    }
                }
            }

            List<string> functionFile = new List<string>();
            foreach (KeyValuePair<string, List<string>> val in usesOfFunction)
            {
                val.Value.Sort();

                functionFile.Add("<h3>" + val.Key.ToString() + "</h3>");
                functionFile.Add("<h6>Seen in " + val.Value.Count + " composites:<h6><ul>");
                foreach (string val2 in val.Value)
                {
                    functionFile.Add("<li>" + val2 + "</li>");
                }
                functionFile.Add("</ul><hr>");
            }
            File.WriteAllLines("AlienFunctionUses" + output_append + ".html", functionFile);

            List<string> compFile = new List<string>();
            List<string> allComps = new List<string>();
            foreach (KeyValuePair<string, List<string>> val in usesOfComposite)
            {
                val.Value.Sort();
                allComps.Add(val.Key);

                compFile.Add("<h3>" + val.Key + "</h3>");
                compFile.Add("<h6>Seen in " + val.Value.Count + " levels:<h6><ul>");
                foreach (string val2 in val.Value)
                {
                    compFile.Add("<li>" + val2 + "</li>");
                }
                compFile.Add("</ul><hr>");
            }
            File.WriteAllLines("AlienCompositeUses" + output_append + ".html", compFile);
            allComps.Sort();
            File.WriteAllLines("AllComposites" + output_append + ".txt", allComps);

#endif
            }

        public static void TestOrders()
        {
#if DEBUG
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            GUI_EnumDataType enumUI = new GUI_EnumDataType();
            Parallel.ForEach(files, file =>
            {
                Commands phys = new Commands(file);
                Parallel.ForEach(phys.Entries, comp =>
                {
                    UInt32 prevVal = 0;
                    /*
                    Console.WriteLine("\t FUNCTIONS");
                    foreach (FunctionEntity ent in comp.functions)
                    {
                        UInt32 currVal = ent.shortGUID.ToUInt32();
                        if (prevVal > currVal)
                            Console.WriteLine("NOPE");
                        else
                            Console.WriteLine("YES");
                        prevVal = currVal;

                        //Console.WriteLine(ent.shortGUID.ToByteString() + " = " + ent.shortGUID.ToUInt32());
                    }*/
                    //Console.WriteLine("----");
                    //Console.WriteLine("\t ALIASES");
                    foreach (AliasEntity ent in comp.aliases)
                    {
                        UInt32 currVal = ent.shortGUID.ToUInt32();
                        if (prevVal > currVal)
                            Console.WriteLine("NOPE");
                        else
                            Console.WriteLine("YES");
                        prevVal = currVal;

                        //Console.WriteLine(ent.shortGUID.ToByteString() + " = " + ent.shortGUID.ToUInt32());
                    }
                    //Console.WriteLine("----");
                    //Console.WriteLine("\t PROXIES");
                    foreach (ProxyEntity ent in comp.proxies)
                    {
                        UInt32 currVal = ent.shortGUID.ToUInt32();
                        if (prevVal > currVal)
                            Console.WriteLine("NOPE");
                        else
                            Console.WriteLine("YES");
                        prevVal = currVal;

                        //Console.WriteLine(ent.shortGUID.ToByteString() + " = " + ent.shortGUID.ToUInt32());
                    }
                    /*
                    Console.WriteLine("----");
                    Console.WriteLine("\t VARIABLES");
                    foreach (VariableEntity ent in comp.variables)
                    {
                        UInt32 currVal = ent.shortGUID.ToUInt32();
                        if (prevVal > currVal)
                            Console.WriteLine("NOPE");
                        else
                            Console.WriteLine("YES");
                        prevVal = currVal;

                        //Console.WriteLine(ent.shortGUID.ToByteString() + " = " + ent.shortGUID.ToUInt32());
                    }
                    */
                    //Console.WriteLine("----");
                });
            });
#endif
        }

        public static void TestNewEnumDropdowns()
        {
#if DEBUG
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            GUI_EnumDataType enumUI = new GUI_EnumDataType();
            Parallel.ForEach(files, file =>
            {
                Commands phys = new Commands(file);
                Parallel.ForEach(phys.Entries, comp =>
                {
                    Parallel.ForEach(comp.GetEntities(), ent =>
                    {
                        Parallel.ForEach(ent.parameters, param =>
                        {
                            if (param.content.dataType == DataType.ENUM)
                                Console.WriteLine(file + "," + comp.name + "," + EntityUtils.GetName(comp, ent) + "," + param.name + "," + ((cEnum)param.content).enumID.ToString() + "," + ((cEnum)param.content).enumIndex);
                            //if (param.content.dataType == DataType.ENUM)
                            //    enumUI.PopulateUI((cEnum)param.content, file + "," + comp.name + "," + EntityUtils.GetName(comp, ent) + "," + param.name.ToString() + ",");
                        });
                    });
                });
            });
#endif
        }


        public static void SyncEnumValuesAndDump()
        {
#if DEBUG
            List<EnumDescriptor> lookup_enum = new List<EnumDescriptor>();

            EnumDescriptor _AGGRESSION_GAIN = GetEnum("AGGRESSION_GAIN");
            foreach (EnumDescriptor.Entry e in _AGGRESSION_GAIN.Entries) e.Index = (int)((AGGRESSION_GAIN)Enum.Parse(typeof(AGGRESSION_GAIN), e.Name));
            lookup_enum.Add(_AGGRESSION_GAIN);
            EnumDescriptor _ALERTNESS_STATE = GetEnum("ALERTNESS_STATE");
            foreach (EnumDescriptor.Entry e in _ALERTNESS_STATE.Entries) e.Index = (int)((ALERTNESS_STATE)Enum.Parse(typeof(ALERTNESS_STATE), e.Name));
            lookup_enum.Add(_ALERTNESS_STATE);
            EnumDescriptor _ALIEN_DEVELOPMENT_MANAGER_STAGES = GetEnum("ALIEN_DEVELOPMENT_MANAGER_STAGES");
            foreach (EnumDescriptor.Entry e in _ALIEN_DEVELOPMENT_MANAGER_STAGES.Entries) e.Index = (int)((ALIEN_DEVELOPMENT_MANAGER_STAGES)Enum.Parse(typeof(ALIEN_DEVELOPMENT_MANAGER_STAGES), e.Name));
            lookup_enum.Add(_ALIEN_DEVELOPMENT_MANAGER_STAGES);
            EnumDescriptor _ALLIANCE_GROUP = GetEnum("ALLIANCE_GROUP");
            foreach (EnumDescriptor.Entry e in _ALLIANCE_GROUP.Entries) e.Index = (int)((ALLIANCE_GROUP)Enum.Parse(typeof(ALLIANCE_GROUP), e.Name));
            lookup_enum.Add(_ALLIANCE_GROUP);
            EnumDescriptor _ALLIANCE_STANCE = GetEnum("ALLIANCE_STANCE");
            foreach (EnumDescriptor.Entry e in _ALLIANCE_STANCE.Entries) e.Index = (int)((ALLIANCE_STANCE)Enum.Parse(typeof(ALLIANCE_STANCE), e.Name));
            lookup_enum.Add(_ALLIANCE_STANCE);
            EnumDescriptor _AMBUSH_TYPE = GetEnum("AMBUSH_TYPE");
            foreach (EnumDescriptor.Entry e in _AMBUSH_TYPE.Entries) e.Index = (int)((AMBUSH_TYPE)Enum.Parse(typeof(AMBUSH_TYPE), e.Name));
            lookup_enum.Add(_AMBUSH_TYPE);
            EnumDescriptor _AMMO_TYPE = GetEnum("AMMO_TYPE");
            foreach (EnumDescriptor.Entry e in _AMMO_TYPE.Entries) e.Index = (int)((AMMO_TYPE)Enum.Parse(typeof(AMMO_TYPE), e.Name));
            lookup_enum.Add(_AMMO_TYPE);
            EnumDescriptor _ANIM_MODE = GetEnum("ANIM_MODE");
            foreach (EnumDescriptor.Entry e in _ANIM_MODE.Entries) e.Index = (int)((ANIM_MODE)Enum.Parse(typeof(ANIM_MODE), e.Name));
            lookup_enum.Add(_ANIM_MODE);
            EnumDescriptor _ANIMATION_EFFECT_TYPE = GetEnum("ANIMATION_EFFECT_TYPE");
            foreach (EnumDescriptor.Entry e in _ANIMATION_EFFECT_TYPE.Entries) e.Index = (int)((ANIMATION_EFFECT_TYPE)Enum.Parse(typeof(ANIMATION_EFFECT_TYPE), e.Name));
            lookup_enum.Add(_ANIMATION_EFFECT_TYPE);
            EnumDescriptor _AREA_SWEEP_TYPE = GetEnum("AREA_SWEEP_TYPE");
            foreach (EnumDescriptor.Entry e in _AREA_SWEEP_TYPE.Entries) e.Index = (int)((AREA_SWEEP_TYPE)Enum.Parse(typeof(AREA_SWEEP_TYPE), e.Name));
            lookup_enum.Add(_AREA_SWEEP_TYPE);
            EnumDescriptor _AUTODETECT = GetEnum("AUTODETECT");
            foreach (EnumDescriptor.Entry e in _AUTODETECT.Entries) e.Index = (int)((AUTODETECT)Enum.Parse(typeof(AUTODETECT), e.Name));
            lookup_enum.Add(_AUTODETECT);
            EnumDescriptor _BEHAVIOR_TREE_BRANCH_TYPE = GetEnum("BEHAVIOR_TREE_BRANCH_TYPE");
            foreach (EnumDescriptor.Entry e in _BEHAVIOR_TREE_BRANCH_TYPE.Entries) e.Index = (int)((BEHAVIOR_TREE_BRANCH_TYPE)Enum.Parse(typeof(BEHAVIOR_TREE_BRANCH_TYPE), e.Name));
            lookup_enum.Add(_BEHAVIOR_TREE_BRANCH_TYPE);
            EnumDescriptor _BEHAVIOUR_MOOD_SET = GetEnum("BEHAVIOUR_MOOD_SET");
            foreach (EnumDescriptor.Entry e in _BEHAVIOUR_MOOD_SET.Entries) e.Index = (int)((BEHAVIOUR_MOOD_SET)Enum.Parse(typeof(BEHAVIOUR_MOOD_SET), e.Name));
            lookup_enum.Add(_BEHAVIOUR_MOOD_SET);
            EnumDescriptor _BEHAVIOUR_TREE_FLAGS = GetEnum("BEHAVIOUR_TREE_FLAGS");
            foreach (EnumDescriptor.Entry e in _BEHAVIOUR_TREE_FLAGS.Entries) e.Index = (int)((BEHAVIOUR_TREE_FLAGS)Enum.Parse(typeof(BEHAVIOUR_TREE_FLAGS), e.Name));
            lookup_enum.Add(_BEHAVIOUR_TREE_FLAGS);
            EnumDescriptor _BLEND_MODE = GetEnum("BLEND_MODE");
            foreach (EnumDescriptor.Entry e in _BLEND_MODE.Entries) e.Index = (int)((BLEND_MODE)Enum.Parse(typeof(BLEND_MODE), e.Name));
            lookup_enum.Add(_BLEND_MODE);
            EnumDescriptor _BLUEPRINT_LEVEL = GetEnum("BLUEPRINT_LEVEL");
            foreach (EnumDescriptor.Entry e in _BLUEPRINT_LEVEL.Entries) e.Index = (int)((BLUEPRINT_LEVEL)Enum.Parse(typeof(BLUEPRINT_LEVEL), e.Name));
            lookup_enum.Add(_BLUEPRINT_LEVEL);
            EnumDescriptor _BUTTON_TYPE = GetEnum("BUTTON_TYPE");
            foreach (EnumDescriptor.Entry e in _BUTTON_TYPE.Entries) e.Index = (int)((BUTTON_TYPE)Enum.Parse(typeof(BUTTON_TYPE), e.Name));
            lookup_enum.Add(_BUTTON_TYPE);
            EnumDescriptor _CAMERA_PATH_CLASS = GetEnum("CAMERA_PATH_CLASS");
            foreach (EnumDescriptor.Entry e in _CAMERA_PATH_CLASS.Entries) e.Index = (int)((CAMERA_PATH_CLASS)Enum.Parse(typeof(CAMERA_PATH_CLASS), e.Name));
            lookup_enum.Add(_CAMERA_PATH_CLASS);
            EnumDescriptor _CAMERA_PATH_TYPE = GetEnum("CAMERA_PATH_TYPE");
            foreach (EnumDescriptor.Entry e in _CAMERA_PATH_TYPE.Entries) e.Index = (int)((CAMERA_PATH_TYPE)Enum.Parse(typeof(CAMERA_PATH_TYPE), e.Name));
            lookup_enum.Add(_CAMERA_PATH_TYPE);
            EnumDescriptor _CHARACTER_CLASS = GetEnum("CHARACTER_CLASS");
            foreach (EnumDescriptor.Entry e in _CHARACTER_CLASS.Entries) e.Index = (int)((CHARACTER_CLASS)Enum.Parse(typeof(CHARACTER_CLASS), e.Name));
            lookup_enum.Add(_CHARACTER_CLASS);
            EnumDescriptor _CHARACTER_CLASS_COMBINATION = GetEnum("CHARACTER_CLASS_COMBINATION");
            foreach (EnumDescriptor.Entry e in _CHARACTER_CLASS_COMBINATION.Entries) e.Index = (int)((CHARACTER_CLASS_COMBINATION)Enum.Parse(typeof(CHARACTER_CLASS_COMBINATION), e.Name));
            lookup_enum.Add(_CHARACTER_CLASS_COMBINATION);
            EnumDescriptor _CHARACTER_FOLEY_SOUND = GetEnum("CHARACTER_FOLEY_SOUND");
            foreach (EnumDescriptor.Entry e in _CHARACTER_FOLEY_SOUND.Entries) e.Index = (int)((CHARACTER_FOLEY_SOUND)Enum.Parse(typeof(CHARACTER_FOLEY_SOUND), e.Name));
            lookup_enum.Add(_CHARACTER_FOLEY_SOUND);
            EnumDescriptor _CHARACTER_NODE = GetEnum("CHARACTER_NODE");
            foreach (EnumDescriptor.Entry e in _CHARACTER_NODE.Entries) e.Index = (int)((CHARACTER_NODE)Enum.Parse(typeof(CHARACTER_NODE), e.Name));
            lookup_enum.Add(_CHARACTER_NODE);
            EnumDescriptor _CHARACTER_STANCE = GetEnum("CHARACTER_STANCE");
            foreach (EnumDescriptor.Entry e in _CHARACTER_STANCE.Entries) e.Index = (int)((CHARACTER_STANCE)Enum.Parse(typeof(CHARACTER_STANCE), e.Name));
            lookup_enum.Add(_CHARACTER_STANCE);
            EnumDescriptor _CHECKPOINT_TYPE = GetEnum("CHECKPOINT_TYPE");
            foreach (EnumDescriptor.Entry e in _CHECKPOINT_TYPE.Entries) e.Index = (int)((CHECKPOINT_TYPE)Enum.Parse(typeof(CHECKPOINT_TYPE), e.Name));
            lookup_enum.Add(_CHECKPOINT_TYPE);
            EnumDescriptor _CI_MESSAGE_TYPE = GetEnum("CI_MESSAGE_TYPE");
            foreach (EnumDescriptor.Entry e in _CI_MESSAGE_TYPE.Entries) e.Index = (int)((CI_MESSAGE_TYPE)Enum.Parse(typeof(CI_MESSAGE_TYPE), e.Name));
            lookup_enum.Add(_CI_MESSAGE_TYPE);
            EnumDescriptor _CLIPPING_PLANES_PRESETS = GetEnum("CLIPPING_PLANES_PRESETS");
            foreach (EnumDescriptor.Entry e in _CLIPPING_PLANES_PRESETS.Entries) e.Index = (int)((CLIPPING_PLANES_PRESETS)Enum.Parse(typeof(CLIPPING_PLANES_PRESETS), e.Name));
            lookup_enum.Add(_CLIPPING_PLANES_PRESETS);
            EnumDescriptor _COLLISION_TYPE = GetEnum("COLLISION_TYPE");
            foreach (EnumDescriptor.Entry e in _COLLISION_TYPE.Entries) e.Index = (int)((COLLISION_TYPE)Enum.Parse(typeof(COLLISION_TYPE), e.Name));
            lookup_enum.Add(_COLLISION_TYPE);
            EnumDescriptor _COMBAT_BEHAVIOUR = GetEnum("COMBAT_BEHAVIOUR");
            foreach (EnumDescriptor.Entry e in _COMBAT_BEHAVIOUR.Entries) e.Index = (int)((COMBAT_BEHAVIOUR)Enum.Parse(typeof(COMBAT_BEHAVIOUR), e.Name));
            lookup_enum.Add(_COMBAT_BEHAVIOUR);
            EnumDescriptor _CUSTOM_CHARACTER_ACCESSORY_OVERRIDE = GetEnum("CUSTOM_CHARACTER_ACCESSORY_OVERRIDE");
            foreach (EnumDescriptor.Entry e in _CUSTOM_CHARACTER_ACCESSORY_OVERRIDE.Entries) e.Index = (int)((CUSTOM_CHARACTER_ACCESSORY_OVERRIDE)Enum.Parse(typeof(CUSTOM_CHARACTER_ACCESSORY_OVERRIDE), e.Name));
            lookup_enum.Add(_CUSTOM_CHARACTER_ACCESSORY_OVERRIDE);
            EnumDescriptor _CUSTOM_CHARACTER_ASSETS = GetEnum("CUSTOM_CHARACTER_ASSETS");
            foreach (EnumDescriptor.Entry e in _CUSTOM_CHARACTER_ASSETS.Entries) e.Index = (int)((CUSTOM_CHARACTER_ASSETS)Enum.Parse(typeof(CUSTOM_CHARACTER_ASSETS), e.Name));
            lookup_enum.Add(_CUSTOM_CHARACTER_ASSETS);
            EnumDescriptor _CUSTOM_CHARACTER_POPULATION = GetEnum("CUSTOM_CHARACTER_POPULATION");
            foreach (EnumDescriptor.Entry e in _CUSTOM_CHARACTER_POPULATION.Entries) e.Index = (int)((CUSTOM_CHARACTER_POPULATION)Enum.Parse(typeof(CUSTOM_CHARACTER_POPULATION), e.Name));
            lookup_enum.Add(_CUSTOM_CHARACTER_POPULATION);
            EnumDescriptor _CUSTOM_CHARACTER_TYPE = GetEnum("CUSTOM_CHARACTER_TYPE");
            foreach (EnumDescriptor.Entry e in _CUSTOM_CHARACTER_TYPE.Entries) e.Index = (int)((CUSTOM_CHARACTER_TYPE)Enum.Parse(typeof(CUSTOM_CHARACTER_TYPE), e.Name));
            lookup_enum.Add(_CUSTOM_CHARACTER_TYPE);
            EnumDescriptor _DAMAGE_EFFECTS = GetEnum("DAMAGE_EFFECTS");
            foreach (EnumDescriptor.Entry e in _DAMAGE_EFFECTS.Entries) e.Index = (int)((DAMAGE_EFFECTS)Enum.Parse(typeof(DAMAGE_EFFECTS), e.Name));
            lookup_enum.Add(_DAMAGE_EFFECTS);
            EnumDescriptor _DAMAGE_MODE = GetEnum("DAMAGE_MODE");
            foreach (EnumDescriptor.Entry e in _DAMAGE_MODE.Entries) e.Index = (int)((DAMAGE_MODE)Enum.Parse(typeof(DAMAGE_MODE), e.Name));
            lookup_enum.Add(_DAMAGE_MODE);
            EnumDescriptor _DEATH_STYLE = GetEnum("DEATH_STYLE");
            foreach (EnumDescriptor.Entry e in _DEATH_STYLE.Entries) e.Index = (int)((DEATH_STYLE)Enum.Parse(typeof(DEATH_STYLE), e.Name));
            lookup_enum.Add(_DEATH_STYLE);
            EnumDescriptor _DIALOGUE_NPC_EVENT = GetEnum("DIALOGUE_NPC_EVENT");
            foreach (EnumDescriptor.Entry e in _DIALOGUE_NPC_EVENT.Entries) e.Index = (int)((DIALOGUE_NPC_EVENT)Enum.Parse(typeof(DIALOGUE_NPC_EVENT), e.Name));
            lookup_enum.Add(_DIALOGUE_NPC_EVENT);
            EnumDescriptor _DIALOGUE_VOICE_ACTOR = GetEnum("DIALOGUE_VOICE_ACTOR");
            foreach (EnumDescriptor.Entry e in _DIALOGUE_VOICE_ACTOR.Entries) e.Index = (int)((DIALOGUE_VOICE_ACTOR)Enum.Parse(typeof(DIALOGUE_VOICE_ACTOR), e.Name));
            lookup_enum.Add(_DIALOGUE_VOICE_ACTOR);
            EnumDescriptor _DIFFICULTY_SETTING_TYPE = GetEnum("DIFFICULTY_SETTING_TYPE");
            foreach (EnumDescriptor.Entry e in _DIFFICULTY_SETTING_TYPE.Entries) e.Index = (int)((DIFFICULTY_SETTING_TYPE)Enum.Parse(typeof(DIFFICULTY_SETTING_TYPE), e.Name));
            lookup_enum.Add(_DIFFICULTY_SETTING_TYPE);
            EnumDescriptor _DOOR_MECHANISM = GetEnum("DOOR_MECHANISM");
            foreach (EnumDescriptor.Entry e in _DOOR_MECHANISM.Entries) e.Index = (int)((DOOR_MECHANISM)Enum.Parse(typeof(DOOR_MECHANISM), e.Name));
            lookup_enum.Add(_DOOR_MECHANISM);
            EnumDescriptor _DUCK_HEIGHT = GetEnum("DUCK_HEIGHT");
            foreach (EnumDescriptor.Entry e in _DUCK_HEIGHT.Entries) e.Index = (int)((DUCK_HEIGHT)Enum.Parse(typeof(DUCK_HEIGHT), e.Name));
            lookup_enum.Add(_DUCK_HEIGHT);
            EnumDescriptor _ENEMY_TYPE = GetEnum("ENEMY_TYPE");
            foreach (EnumDescriptor.Entry e in _ENEMY_TYPE.Entries) e.Index = (int)((ENEMY_TYPE)Enum.Parse(typeof(ENEMY_TYPE), e.Name));
            lookup_enum.Add(_ENEMY_TYPE);
            EnumDescriptor _ENVIRONMENT_ARCHETYPE = GetEnum("ENVIRONMENT_ARCHETYPE");
            foreach (EnumDescriptor.Entry e in _ENVIRONMENT_ARCHETYPE.Entries) e.Index = (int)((ENVIRONMENT_ARCHETYPE)Enum.Parse(typeof(ENVIRONMENT_ARCHETYPE), e.Name));
            lookup_enum.Add(_ENVIRONMENT_ARCHETYPE);
            EnumDescriptor _EQUIPMENT_SLOT = GetEnum("EQUIPMENT_SLOT");
            foreach (EnumDescriptor.Entry e in _EQUIPMENT_SLOT.Entries) e.Index = (int)((EQUIPMENT_SLOT)Enum.Parse(typeof(EQUIPMENT_SLOT), e.Name));
            lookup_enum.Add(_EQUIPMENT_SLOT);
            EnumDescriptor _EXIT_WAYPOINT = GetEnum("EXIT_WAYPOINT");
            foreach (EnumDescriptor.Entry e in _EXIT_WAYPOINT.Entries) e.Index = (int)((EXIT_WAYPOINT)Enum.Parse(typeof(EXIT_WAYPOINT), e.Name));
            lookup_enum.Add(_EXIT_WAYPOINT);
            EnumDescriptor _FLASH_INVOKE_TYPE = GetEnum("FLASH_INVOKE_TYPE");
            foreach (EnumDescriptor.Entry e in _FLASH_INVOKE_TYPE.Entries) e.Index = (int)((FLASH_INVOKE_TYPE)Enum.Parse(typeof(FLASH_INVOKE_TYPE), e.Name));
            lookup_enum.Add(_FLASH_INVOKE_TYPE);
            EnumDescriptor _FLASH_SCRIPT_RENDER_TYPE = GetEnum("FLASH_SCRIPT_RENDER_TYPE");
            foreach (EnumDescriptor.Entry e in _FLASH_SCRIPT_RENDER_TYPE.Entries) e.Index = (int)((FLASH_SCRIPT_RENDER_TYPE)Enum.Parse(typeof(FLASH_SCRIPT_RENDER_TYPE), e.Name));
            lookup_enum.Add(_FLASH_SCRIPT_RENDER_TYPE);
            EnumDescriptor _FOG_BOX_TYPE = GetEnum("FOG_BOX_TYPE");
            foreach (EnumDescriptor.Entry e in _FOG_BOX_TYPE.Entries) e.Index = (int)((FOG_BOX_TYPE)Enum.Parse(typeof(FOG_BOX_TYPE), e.Name));
            lookup_enum.Add(_FOG_BOX_TYPE);
            EnumDescriptor _FOLDER_LOCK_TYPE = GetEnum("FOLDER_LOCK_TYPE");
            foreach (EnumDescriptor.Entry e in _FOLDER_LOCK_TYPE.Entries) e.Index = (int)((FOLDER_LOCK_TYPE)Enum.Parse(typeof(FOLDER_LOCK_TYPE), e.Name));
            lookup_enum.Add(_FOLDER_LOCK_TYPE);
            EnumDescriptor _FOLLOW_CAMERA_MODIFIERS = GetEnum("FOLLOW_CAMERA_MODIFIERS");
            foreach (EnumDescriptor.Entry e in _FOLLOW_CAMERA_MODIFIERS.Entries) e.Index = (int)((FOLLOW_CAMERA_MODIFIERS)Enum.Parse(typeof(FOLLOW_CAMERA_MODIFIERS), e.Name));
            lookup_enum.Add(_FOLLOW_CAMERA_MODIFIERS);
            EnumDescriptor _FOLLOW_TYPE = GetEnum("FOLLOW_TYPE");
            foreach (EnumDescriptor.Entry e in _FOLLOW_TYPE.Entries) e.Index = (int)((FOLLOW_TYPE)Enum.Parse(typeof(FOLLOW_TYPE), e.Name));
            lookup_enum.Add(_FOLLOW_TYPE);
            EnumDescriptor _FRONTEND_STATE = GetEnum("FRONTEND_STATE");
            foreach (EnumDescriptor.Entry e in _FRONTEND_STATE.Entries) e.Index = (int)((FRONTEND_STATE)Enum.Parse(typeof(FRONTEND_STATE), e.Name));
            lookup_enum.Add(_FRONTEND_STATE);
            EnumDescriptor _GAME_CLIP = GetEnum("GAME_CLIP");
            foreach (EnumDescriptor.Entry e in _GAME_CLIP.Entries) e.Index = (int)((GAME_CLIP)Enum.Parse(typeof(GAME_CLIP), e.Name));
            lookup_enum.Add(_GAME_CLIP);
            EnumDescriptor _GATING_TOOL_TYPE = GetEnum("GATING_TOOL_TYPE");
            foreach (EnumDescriptor.Entry e in _GATING_TOOL_TYPE.Entries) e.Index = (int)((GATING_TOOL_TYPE)Enum.Parse(typeof(GATING_TOOL_TYPE), e.Name));
            lookup_enum.Add(_GATING_TOOL_TYPE);
            EnumDescriptor _IDLE = GetEnum("IDLE");
            foreach (EnumDescriptor.Entry e in _IDLE.Entries) e.Index = (int)((IDLE)Enum.Parse(typeof(IDLE), e.Name));
            lookup_enum.Add(_IDLE);
            EnumDescriptor _IDLE_STYLE = GetEnum("IDLE_STYLE");
            foreach (EnumDescriptor.Entry e in _IDLE_STYLE.Entries) e.Index = (int)((IDLE_STYLE)Enum.Parse(typeof(IDLE_STYLE), e.Name));
            lookup_enum.Add(_IDLE_STYLE);
            EnumDescriptor _IMPACT_CHARACTER_BODY_LOCATION_TYPE = GetEnum("IMPACT_CHARACTER_BODY_LOCATION_TYPE");
            foreach (EnumDescriptor.Entry e in _IMPACT_CHARACTER_BODY_LOCATION_TYPE.Entries) e.Index = (int)((IMPACT_CHARACTER_BODY_LOCATION_TYPE)Enum.Parse(typeof(IMPACT_CHARACTER_BODY_LOCATION_TYPE), e.Name));
            lookup_enum.Add(_IMPACT_CHARACTER_BODY_LOCATION_TYPE);
            EnumDescriptor _INPUT_DEVICE_TYPE = GetEnum("INPUT_DEVICE_TYPE");
            foreach (EnumDescriptor.Entry e in _INPUT_DEVICE_TYPE.Entries) e.Index = (int)((INPUT_DEVICE_TYPE)Enum.Parse(typeof(INPUT_DEVICE_TYPE), e.Name));
            lookup_enum.Add(_INPUT_DEVICE_TYPE);
            EnumDescriptor _LEVER_TYPE = GetEnum("LEVER_TYPE");
            foreach (EnumDescriptor.Entry e in _LEVER_TYPE.Entries) e.Index = (int)((LEVER_TYPE)Enum.Parse(typeof(LEVER_TYPE), e.Name));
            lookup_enum.Add(_LEVER_TYPE);
            EnumDescriptor _LIGHT_ADAPTATION_MECHANISM = GetEnum("LIGHT_ADAPTATION_MECHANISM");
            foreach (EnumDescriptor.Entry e in _LIGHT_ADAPTATION_MECHANISM.Entries) e.Index = (int)((LIGHT_ADAPTATION_MECHANISM)Enum.Parse(typeof(LIGHT_ADAPTATION_MECHANISM), e.Name));
            lookup_enum.Add(_LIGHT_ADAPTATION_MECHANISM);
            EnumDescriptor _LIGHT_ANIM = GetEnum("LIGHT_ANIM");
            foreach (EnumDescriptor.Entry e in _LIGHT_ANIM.Entries) e.Index = (int)((LIGHT_ANIM)Enum.Parse(typeof(LIGHT_ANIM), e.Name));
            lookup_enum.Add(_LIGHT_ANIM);
            EnumDescriptor _LIGHT_FADE_TYPE = GetEnum("LIGHT_FADE_TYPE");
            foreach (EnumDescriptor.Entry e in _LIGHT_FADE_TYPE.Entries) e.Index = (int)((LIGHT_FADE_TYPE)Enum.Parse(typeof(LIGHT_FADE_TYPE), e.Name));
            lookup_enum.Add(_LIGHT_FADE_TYPE);
            EnumDescriptor _LIGHT_TRANSITION = GetEnum("LIGHT_TRANSITION");
            foreach (EnumDescriptor.Entry e in _LIGHT_TRANSITION.Entries) e.Index = (int)((LIGHT_TRANSITION)Enum.Parse(typeof(LIGHT_TRANSITION), e.Name));
            lookup_enum.Add(_LIGHT_TRANSITION);
            EnumDescriptor _LIGHT_TYPE = GetEnum("LIGHT_TYPE");
            foreach (EnumDescriptor.Entry e in _LIGHT_TYPE.Entries) e.Index = (int)((LIGHT_TYPE)Enum.Parse(typeof(LIGHT_TYPE), e.Name));
            lookup_enum.Add(_LIGHT_TYPE);
            EnumDescriptor _LOCOMOTION_STATE = GetEnum("LOCOMOTION_STATE");
            foreach (EnumDescriptor.Entry e in _LOCOMOTION_STATE.Entries) e.Index = (int)((LOCOMOTION_STATE)Enum.Parse(typeof(LOCOMOTION_STATE), e.Name));
            lookup_enum.Add(_LOCOMOTION_STATE);
            EnumDescriptor _LOCOMOTION_TARGET_SPEED = GetEnum("LOCOMOTION_TARGET_SPEED");
            foreach (EnumDescriptor.Entry e in _LOCOMOTION_TARGET_SPEED.Entries) e.Index = (int)((LOCOMOTION_TARGET_SPEED)Enum.Parse(typeof(LOCOMOTION_TARGET_SPEED), e.Name));
            lookup_enum.Add(_LOCOMOTION_TARGET_SPEED);
            EnumDescriptor _LOOK_SPEED = GetEnum("LOOK_SPEED");
            foreach (EnumDescriptor.Entry e in _LOOK_SPEED.Entries) e.Index = (int)((LOOK_SPEED)Enum.Parse(typeof(LOOK_SPEED), e.Name));
            lookup_enum.Add(_LOOK_SPEED);
            EnumDescriptor _MAP_ICON_TYPE = GetEnum("MAP_ICON_TYPE");
            foreach (EnumDescriptor.Entry e in _MAP_ICON_TYPE.Entries) e.Index = (int)((MAP_ICON_TYPE)Enum.Parse(typeof(MAP_ICON_TYPE), e.Name));
            lookup_enum.Add(_MAP_ICON_TYPE);
            EnumDescriptor _MELEE_ATTACK_TYPE = GetEnum("MELEE_ATTACK_TYPE");
            foreach (EnumDescriptor.Entry e in _MELEE_ATTACK_TYPE.Entries) e.Index = (int)((MELEE_ATTACK_TYPE)Enum.Parse(typeof(MELEE_ATTACK_TYPE), e.Name));
            lookup_enum.Add(_MELEE_ATTACK_TYPE);
            EnumDescriptor _MELEE_CONTEXT_TYPE = GetEnum("MELEE_CONTEXT_TYPE");
            foreach (EnumDescriptor.Entry e in _MELEE_CONTEXT_TYPE.Entries) e.Index = (int)((MELEE_CONTEXT_TYPE)Enum.Parse(typeof(MELEE_CONTEXT_TYPE), e.Name));
            lookup_enum.Add(_MELEE_CONTEXT_TYPE);
            EnumDescriptor _MOOD = GetEnum("MOOD");
            foreach (EnumDescriptor.Entry e in _MOOD.Entries) e.Index = (int)((MOOD)Enum.Parse(typeof(MOOD), e.Name));
            lookup_enum.Add(_MOOD);
            EnumDescriptor _MOOD_INTENSITY = GetEnum("MOOD_INTENSITY");
            foreach (EnumDescriptor.Entry e in _MOOD_INTENSITY.Entries) e.Index = (int)((MOOD_INTENSITY)Enum.Parse(typeof(MOOD_INTENSITY), e.Name));
            lookup_enum.Add(_MOOD_INTENSITY);
            EnumDescriptor _MOVE = GetEnum("MOVE");
            foreach (EnumDescriptor.Entry e in _MOVE.Entries) e.Index = (int)((MOVE)Enum.Parse(typeof(MOVE), e.Name));
            lookup_enum.Add(_MOVE);
            EnumDescriptor _MUSIC_RTPC_MODE = GetEnum("MUSIC_RTPC_MODE");
            foreach (EnumDescriptor.Entry e in _MUSIC_RTPC_MODE.Entries) e.Index = (int)((MUSIC_RTPC_MODE)Enum.Parse(typeof(MUSIC_RTPC_MODE), e.Name));
            lookup_enum.Add(_MUSIC_RTPC_MODE);
            EnumDescriptor _NAV_MESH_AREA_TYPE = GetEnum("NAV_MESH_AREA_TYPE");
            foreach (EnumDescriptor.Entry e in _NAV_MESH_AREA_TYPE.Entries) e.Index = (int)((NAV_MESH_AREA_TYPE)Enum.Parse(typeof(NAV_MESH_AREA_TYPE), e.Name));
            lookup_enum.Add(_NAV_MESH_AREA_TYPE);
            EnumDescriptor _NOISE_TYPE = GetEnum("NOISE_TYPE");
            foreach (EnumDescriptor.Entry e in _NOISE_TYPE.Entries) e.Index = (int)((NOISE_TYPE)Enum.Parse(typeof(NOISE_TYPE), e.Name));
            lookup_enum.Add(_NOISE_TYPE);
            EnumDescriptor _NPC_AGGRO_LEVEL = GetEnum("NPC_AGGRO_LEVEL");
            foreach (EnumDescriptor.Entry e in _NPC_AGGRO_LEVEL.Entries) e.Index = (int)((NPC_AGGRO_LEVEL)Enum.Parse(typeof(NPC_AGGRO_LEVEL), e.Name));
            lookup_enum.Add(_NPC_AGGRO_LEVEL);
            EnumDescriptor _NPC_COMBAT_STATE = GetEnum("NPC_COMBAT_STATE");
            foreach (EnumDescriptor.Entry e in _NPC_COMBAT_STATE.Entries) e.Index = (int)((NPC_COMBAT_STATE)Enum.Parse(typeof(NPC_COMBAT_STATE), e.Name));
            lookup_enum.Add(_NPC_COMBAT_STATE);
            EnumDescriptor _NPC_COVER_REQUEST_TYPE = GetEnum("NPC_COVER_REQUEST_TYPE");
            foreach (EnumDescriptor.Entry e in _NPC_COVER_REQUEST_TYPE.Entries) e.Index = (int)((NPC_COVER_REQUEST_TYPE)Enum.Parse(typeof(NPC_COVER_REQUEST_TYPE), e.Name));
            lookup_enum.Add(_NPC_COVER_REQUEST_TYPE);
            EnumDescriptor _NPC_GUN_AIM_MODE = GetEnum("NPC_GUN_AIM_MODE");
            foreach (EnumDescriptor.Entry e in _NPC_GUN_AIM_MODE.Entries) e.Index = (int)((NPC_GUN_AIM_MODE)Enum.Parse(typeof(NPC_GUN_AIM_MODE), e.Name));
            lookup_enum.Add(_NPC_GUN_AIM_MODE);
            EnumDescriptor _ORIENTATION_AXIS = GetEnum("ORIENTATION_AXIS");
            foreach (EnumDescriptor.Entry e in _ORIENTATION_AXIS.Entries) e.Index = (int)((ORIENTATION_AXIS)Enum.Parse(typeof(ORIENTATION_AXIS), e.Name));
            lookup_enum.Add(_ORIENTATION_AXIS);
            EnumDescriptor _PATH_DRIVEN_TYPE = GetEnum("PATH_DRIVEN_TYPE");
            foreach (EnumDescriptor.Entry e in _PATH_DRIVEN_TYPE.Entries) e.Index = (int)((PATH_DRIVEN_TYPE)Enum.Parse(typeof(PATH_DRIVEN_TYPE), e.Name));
            lookup_enum.Add(_PATH_DRIVEN_TYPE);
            EnumDescriptor _PICKUP_CATEGORY = GetEnum("PICKUP_CATEGORY");
            foreach (EnumDescriptor.Entry e in _PICKUP_CATEGORY.Entries) e.Index = (int)((PICKUP_CATEGORY)Enum.Parse(typeof(PICKUP_CATEGORY), e.Name));
            lookup_enum.Add(_PICKUP_CATEGORY);
            EnumDescriptor _PLATFORM_TYPE = GetEnum("PLATFORM_TYPE");
            foreach (EnumDescriptor.Entry e in _PLATFORM_TYPE.Entries) e.Index = (int)((PLATFORM_TYPE)Enum.Parse(typeof(PLATFORM_TYPE), e.Name));
            lookup_enum.Add(_PLATFORM_TYPE);
            EnumDescriptor _PLAYER_INVENTORY_SET = GetEnum("PLAYER_INVENTORY_SET");
            foreach (EnumDescriptor.Entry e in _PLAYER_INVENTORY_SET.Entries) e.Index = (int)((PLAYER_INVENTORY_SET)Enum.Parse(typeof(PLAYER_INVENTORY_SET), e.Name));
            lookup_enum.Add(_PLAYER_INVENTORY_SET);
            EnumDescriptor _POPUP_MESSAGE_ICON = GetEnum("POPUP_MESSAGE_ICON");
            foreach (EnumDescriptor.Entry e in _POPUP_MESSAGE_ICON.Entries) e.Index = (int)((POPUP_MESSAGE_ICON)Enum.Parse(typeof(POPUP_MESSAGE_ICON), e.Name));
            lookup_enum.Add(_POPUP_MESSAGE_ICON);
            EnumDescriptor _POPUP_MESSAGE_SOUND = GetEnum("POPUP_MESSAGE_SOUND");
            foreach (EnumDescriptor.Entry e in _POPUP_MESSAGE_SOUND.Entries) e.Index = (int)((POPUP_MESSAGE_SOUND)Enum.Parse(typeof(POPUP_MESSAGE_SOUND), e.Name));
            lookup_enum.Add(_POPUP_MESSAGE_SOUND);
            EnumDescriptor _PRIORITY = GetEnum("PRIORITY");
            foreach (EnumDescriptor.Entry e in _PRIORITY.Entries) e.Index = (int)((PRIORITY)Enum.Parse(typeof(PRIORITY), e.Name));
            lookup_enum.Add(_PRIORITY);
            EnumDescriptor _RANGE_TEST_SHAPE = GetEnum("RANGE_TEST_SHAPE");
            foreach (EnumDescriptor.Entry e in _RANGE_TEST_SHAPE.Entries) e.Index = (int)((RANGE_TEST_SHAPE)Enum.Parse(typeof(RANGE_TEST_SHAPE), e.Name));
            lookup_enum.Add(_RANGE_TEST_SHAPE);
            EnumDescriptor _RAYCAST_PRIORITY = GetEnum("RAYCAST_PRIORITY");
            foreach (EnumDescriptor.Entry e in _RAYCAST_PRIORITY.Entries) e.Index = (int)((RAYCAST_PRIORITY)Enum.Parse(typeof(RAYCAST_PRIORITY), e.Name));
            lookup_enum.Add(_RAYCAST_PRIORITY);
            EnumDescriptor _RESPAWN_MODE = GetEnum("RESPAWN_MODE");
            foreach (EnumDescriptor.Entry e in _RESPAWN_MODE.Entries) e.Index = (int)((RESPAWN_MODE)Enum.Parse(typeof(RESPAWN_MODE), e.Name));
            lookup_enum.Add(_RESPAWN_MODE);
            EnumDescriptor _REWIRE_SYSTEM_NAME = GetEnum("REWIRE_SYSTEM_NAME");
            foreach (EnumDescriptor.Entry e in _REWIRE_SYSTEM_NAME.Entries) e.Index = (int)((REWIRE_SYSTEM_NAME)Enum.Parse(typeof(REWIRE_SYSTEM_NAME), e.Name));
            lookup_enum.Add(_REWIRE_SYSTEM_NAME);
            EnumDescriptor _REWIRE_SYSTEM_TYPE = GetEnum("REWIRE_SYSTEM_TYPE");
            foreach (EnumDescriptor.Entry e in _REWIRE_SYSTEM_TYPE.Entries) e.Index = (int)((REWIRE_SYSTEM_TYPE)Enum.Parse(typeof(REWIRE_SYSTEM_TYPE), e.Name));
            lookup_enum.Add(_REWIRE_SYSTEM_TYPE);
            EnumDescriptor _SECONDARY_ANIMATION_LAYER = GetEnum("SECONDARY_ANIMATION_LAYER");
            foreach (EnumDescriptor.Entry e in _SECONDARY_ANIMATION_LAYER.Entries) e.Index = (int)((SECONDARY_ANIMATION_LAYER)Enum.Parse(typeof(SECONDARY_ANIMATION_LAYER), e.Name));
            lookup_enum.Add(_SECONDARY_ANIMATION_LAYER);
            EnumDescriptor _SENSE_SET = GetEnum("SENSE_SET");
            foreach (EnumDescriptor.Entry e in _SENSE_SET.Entries) e.Index = (int)((SENSE_SET)Enum.Parse(typeof(SENSE_SET), e.Name));
            lookup_enum.Add(_SENSE_SET);
            EnumDescriptor _SENSORY_TYPE = GetEnum("SENSORY_TYPE");
            foreach (EnumDescriptor.Entry e in _SENSORY_TYPE.Entries) e.Index = (int)((SENSORY_TYPE)Enum.Parse(typeof(SENSORY_TYPE), e.Name));
            lookup_enum.Add(_SENSORY_TYPE);
            EnumDescriptor _SHAKE_TYPE = GetEnum("SHAKE_TYPE");
            foreach (EnumDescriptor.Entry e in _SHAKE_TYPE.Entries) e.Index = (int)((SHAKE_TYPE)Enum.Parse(typeof(SHAKE_TYPE), e.Name));
            lookup_enum.Add(_SHAKE_TYPE);
            EnumDescriptor _SIDE = GetEnum("SIDE");
            foreach (EnumDescriptor.Entry e in _SIDE.Entries) e.Index = (int)((SIDE)Enum.Parse(typeof(SIDE), e.Name));
            lookup_enum.Add(_SIDE);
            EnumDescriptor _SOUND_POOL = GetEnum("SOUND_POOL");
            foreach (EnumDescriptor.Entry e in _SOUND_POOL.Entries) e.Index = (int)((SOUND_POOL)Enum.Parse(typeof(SOUND_POOL), e.Name));
            lookup_enum.Add(_SOUND_POOL);
            EnumDescriptor _SPEECH_PRIORITY = GetEnum("SPEECH_PRIORITY");
            foreach (EnumDescriptor.Entry e in _SPEECH_PRIORITY.Entries) e.Index = (int)((SPEECH_PRIORITY)Enum.Parse(typeof(SPEECH_PRIORITY), e.Name));
            lookup_enum.Add(_SPEECH_PRIORITY);
            EnumDescriptor _STEAL_CAMERA_TYPE = GetEnum("STEAL_CAMERA_TYPE");
            foreach (EnumDescriptor.Entry e in _STEAL_CAMERA_TYPE.Entries) e.Index = (int)((STEAL_CAMERA_TYPE)Enum.Parse(typeof(STEAL_CAMERA_TYPE), e.Name));
            lookup_enum.Add(_STEAL_CAMERA_TYPE);
            EnumDescriptor _SUB_OBJECTIVE_TYPE = GetEnum("SUB_OBJECTIVE_TYPE");
            foreach (EnumDescriptor.Entry e in _SUB_OBJECTIVE_TYPE.Entries) e.Index = (int)((SUB_OBJECTIVE_TYPE)Enum.Parse(typeof(SUB_OBJECTIVE_TYPE), e.Name));
            lookup_enum.Add(_SUB_OBJECTIVE_TYPE);
            EnumDescriptor _SUSPICIOUS_ITEM = GetEnum("SUSPICIOUS_ITEM");
            foreach (EnumDescriptor.Entry e in _SUSPICIOUS_ITEM.Entries) e.Index = (int)((SUSPICIOUS_ITEM)Enum.Parse(typeof(SUSPICIOUS_ITEM), e.Name));
            lookup_enum.Add(_SUSPICIOUS_ITEM);
            EnumDescriptor _SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY = GetEnum("SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY");
            foreach (EnumDescriptor.Entry e in _SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY.Entries) e.Index = (int)((SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY)Enum.Parse(typeof(SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY), e.Name));
            lookup_enum.Add(_SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY);
            EnumDescriptor _SUSPICIOUS_ITEM_REACTION = GetEnum("SUSPICIOUS_ITEM_REACTION");
            foreach (EnumDescriptor.Entry e in _SUSPICIOUS_ITEM_REACTION.Entries) e.Index = (int)((SUSPICIOUS_ITEM_REACTION)Enum.Parse(typeof(SUSPICIOUS_ITEM_REACTION), e.Name));
            lookup_enum.Add(_SUSPICIOUS_ITEM_REACTION);
            EnumDescriptor _SUSPICIOUS_ITEM_STAGE = GetEnum("SUSPICIOUS_ITEM_STAGE");
            foreach (EnumDescriptor.Entry e in _SUSPICIOUS_ITEM_STAGE.Entries) e.Index = (int)((SUSPICIOUS_ITEM_STAGE)Enum.Parse(typeof(SUSPICIOUS_ITEM_STAGE), e.Name));
            lookup_enum.Add(_SUSPICIOUS_ITEM_STAGE);
            EnumDescriptor _SUSPICIOUS_ITEM_TRIGGER = GetEnum("SUSPICIOUS_ITEM_TRIGGER");
            foreach (EnumDescriptor.Entry e in _SUSPICIOUS_ITEM_TRIGGER.Entries) e.Index = (int)((SUSPICIOUS_ITEM_TRIGGER)Enum.Parse(typeof(SUSPICIOUS_ITEM_TRIGGER), e.Name));
            lookup_enum.Add(_SUSPICIOUS_ITEM_TRIGGER);
            EnumDescriptor _TASK_CHARACTER_CLASS_FILTER = GetEnum("TASK_CHARACTER_CLASS_FILTER");
            foreach (EnumDescriptor.Entry e in _TASK_CHARACTER_CLASS_FILTER.Entries) e.Index = (int)((TASK_CHARACTER_CLASS_FILTER)Enum.Parse(typeof(TASK_CHARACTER_CLASS_FILTER), e.Name));
            lookup_enum.Add(_TASK_CHARACTER_CLASS_FILTER);
            EnumDescriptor _TASK_OPERATION_MODE = GetEnum("TASK_OPERATION_MODE");
            foreach (EnumDescriptor.Entry e in _TASK_OPERATION_MODE.Entries) e.Index = (int)((TASK_OPERATION_MODE)Enum.Parse(typeof(TASK_OPERATION_MODE), e.Name));
            lookup_enum.Add(_TASK_OPERATION_MODE);
            EnumDescriptor _TASK_PRIORITY = GetEnum("TASK_PRIORITY");
            foreach (EnumDescriptor.Entry e in _TASK_PRIORITY.Entries) e.Index = (int)((TASK_PRIORITY)Enum.Parse(typeof(TASK_PRIORITY), e.Name));
            lookup_enum.Add(_TASK_PRIORITY);
            EnumDescriptor _TERMINAL_LOCATION = GetEnum("TERMINAL_LOCATION");
            foreach (EnumDescriptor.Entry e in _TERMINAL_LOCATION.Entries) e.Index = (int)((TERMINAL_LOCATION)Enum.Parse(typeof(TERMINAL_LOCATION), e.Name));
            lookup_enum.Add(_TERMINAL_LOCATION);
            EnumDescriptor _TEXT_ALIGNMENT = GetEnum("TEXT_ALIGNMENT");
            foreach (EnumDescriptor.Entry e in _TEXT_ALIGNMENT.Entries) e.Index = (int)((TEXT_ALIGNMENT)Enum.Parse(typeof(TEXT_ALIGNMENT), e.Name));
            lookup_enum.Add(_TEXT_ALIGNMENT);
            EnumDescriptor _THRESHOLD_QUALIFIER = GetEnum("THRESHOLD_QUALIFIER");
            foreach (EnumDescriptor.Entry e in _THRESHOLD_QUALIFIER.Entries) e.Index = (int)((THRESHOLD_QUALIFIER)Enum.Parse(typeof(THRESHOLD_QUALIFIER), e.Name));
            lookup_enum.Add(_THRESHOLD_QUALIFIER);
            EnumDescriptor _TRANSITION_DIRECTION = GetEnum("TRANSITION_DIRECTION");
            foreach (EnumDescriptor.Entry e in _TRANSITION_DIRECTION.Entries) e.Index = (int)((TRANSITION_DIRECTION)Enum.Parse(typeof(TRANSITION_DIRECTION), e.Name));
            lookup_enum.Add(_TRANSITION_DIRECTION);
            EnumDescriptor _TRAVERSAL_ANIMS = GetEnum("TRAVERSAL_ANIMS");
            foreach (EnumDescriptor.Entry e in _TRAVERSAL_ANIMS.Entries) e.Index = (int)((TRAVERSAL_ANIMS)Enum.Parse(typeof(TRAVERSAL_ANIMS), e.Name));
            lookup_enum.Add(_TRAVERSAL_ANIMS);
            EnumDescriptor _UI_ICON_ICON = GetEnum("UI_ICON_ICON");
            foreach (EnumDescriptor.Entry e in _UI_ICON_ICON.Entries) e.Index = (int)((UI_ICON_ICON)Enum.Parse(typeof(UI_ICON_ICON), e.Name));
            lookup_enum.Add(_UI_ICON_ICON);
            EnumDescriptor _UI_KEYGATE_TYPE = GetEnum("UI_KEYGATE_TYPE");
            foreach (EnumDescriptor.Entry e in _UI_KEYGATE_TYPE.Entries) e.Index = (int)((UI_KEYGATE_TYPE)Enum.Parse(typeof(UI_KEYGATE_TYPE), e.Name));
            lookup_enum.Add(_UI_KEYGATE_TYPE);
            EnumDescriptor _VIEWCONE_TYPE = GetEnum("VIEWCONE_TYPE");
            foreach (EnumDescriptor.Entry e in _VIEWCONE_TYPE.Entries) e.Index = (int)((VIEWCONE_TYPE)Enum.Parse(typeof(VIEWCONE_TYPE), e.Name));
            lookup_enum.Add(_VIEWCONE_TYPE);
            EnumDescriptor _WAVE_SHAPE = GetEnum("WAVE_SHAPE");
            foreach (EnumDescriptor.Entry e in _WAVE_SHAPE.Entries) e.Index = (int)((WAVE_SHAPE)Enum.Parse(typeof(WAVE_SHAPE), e.Name));
            lookup_enum.Add(_WAVE_SHAPE);
            EnumDescriptor _WEAPON_HANDEDNESS = GetEnum("WEAPON_HANDEDNESS");
            foreach (EnumDescriptor.Entry e in _WEAPON_HANDEDNESS.Entries) e.Index = (int)((WEAPON_HANDEDNESS)Enum.Parse(typeof(WEAPON_HANDEDNESS), e.Name));
            lookup_enum.Add(_WEAPON_HANDEDNESS);
            EnumDescriptor _WEAPON_IMPACT_EFFECT_ORIENTATION = GetEnum("WEAPON_IMPACT_EFFECT_ORIENTATION");
            foreach (EnumDescriptor.Entry e in _WEAPON_IMPACT_EFFECT_ORIENTATION.Entries) e.Index = (int)((WEAPON_IMPACT_EFFECT_ORIENTATION)Enum.Parse(typeof(WEAPON_IMPACT_EFFECT_ORIENTATION), e.Name));
            lookup_enum.Add(_WEAPON_IMPACT_EFFECT_ORIENTATION);
            EnumDescriptor _WEAPON_IMPACT_EFFECT_TYPE = GetEnum("WEAPON_IMPACT_EFFECT_TYPE");
            foreach (EnumDescriptor.Entry e in _WEAPON_IMPACT_EFFECT_TYPE.Entries) e.Index = (int)((WEAPON_IMPACT_EFFECT_TYPE)Enum.Parse(typeof(WEAPON_IMPACT_EFFECT_TYPE), e.Name));
            lookup_enum.Add(_WEAPON_IMPACT_EFFECT_TYPE);
            EnumDescriptor _WEAPON_IMPACT_FILTER_ORIENTATION = GetEnum("WEAPON_IMPACT_FILTER_ORIENTATION");
            foreach (EnumDescriptor.Entry e in _WEAPON_IMPACT_FILTER_ORIENTATION.Entries) e.Index = (int)((WEAPON_IMPACT_FILTER_ORIENTATION)Enum.Parse(typeof(WEAPON_IMPACT_FILTER_ORIENTATION), e.Name));
            lookup_enum.Add(_WEAPON_IMPACT_FILTER_ORIENTATION);
            EnumDescriptor _WEAPON_TYPE = GetEnum("WEAPON_TYPE");
            foreach (EnumDescriptor.Entry e in _WEAPON_TYPE.Entries) e.Index = (int)((WEAPON_TYPE)Enum.Parse(typeof(WEAPON_TYPE), e.Name));
            lookup_enum.Add(_WEAPON_TYPE);

            BinaryWriter writer = new BinaryWriter(File.OpenWrite("out_enums.bin"));
            writer.Write(lookup_enum.Count);
            foreach (EnumDescriptor e in lookup_enum)
            {
                Utilities.Write<ShortGuid>(writer, e.ID);
                writer.Write(e.Name);
                writer.Write(e.Entries.Count);
                for (int i = 0; i < e.Entries.Count; i++)
                {
                    writer.Write(e.Entries[i].Name);
                    writer.Write(e.Entries[i].Index);
                }
            }
            writer.Close();
#endif
        }

        public static void CommandsTest()
        {
#if DEBUG
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands phys = new Commands(file);
                Parallel.ForEach(phys.Entries, comp =>
                {
                    Parallel.ForEach(comp.functions, ent =>
                    {
                        if (comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.PhysicsModifyGravity)).Count != 0)
                        {
                            Console.WriteLine(file + "\n\t" + comp.name);
                        }
                    });
                });
            });
#endif
        }

        public static void CheckMissingCageAnimParams()
        {
#if DEBUG
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands phys = new Commands(file);
                Composite gui = phys.GetComposite("ARCHETYPES\\GAMEPLAY\\UI_OBJECTIVE");
                if (gui != null)
                {
                    ShortGuid id = gui.shortGUID;
                    Parallel.ForEach(phys.Entries, comp =>
                    {
                        Parallel.ForEach(comp.functions.FindAll(o => o.function == id), ent =>
                        {
                            Console.WriteLine(file + "\n\t" + comp.name + "\n\t\t" + EntityUtils.GetName(comp, ent));

                            //Parallel.ForEach(ent.parameters, param =>
                            //{
                            //    if (list.Contains(param.name))
                            //    {
                            //        Console.WriteLine(file + "\n\t" + comp.name + "\n\t\t" + EntityUtils.GetName(comp, ent) + " -> " + param.name.ToString());
                            //    }
                            //});
                        });
                    });
                }
            });
#endif
        }

        public static void SanityCheckCAGEAnimation()
        {
#if DEBUG
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            Parallel.ForEach(files, file =>
            {
                Commands phys = new Commands(file);
                Parallel.ForEach(phys.Entries, comp =>
                {
                    List<FunctionEntity> ents = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation));
                    Parallel.ForEach(ents, ent =>
                    {
                        CAGEAnimation anim = (CAGEAnimation)ent;
                        //File.WriteAllText("out.json", JsonConvert.SerializeObject(anim, Formatting.Indented));

                        List<CAGEAnimation.Connection> prunedConnections = new List<CAGEAnimation.Connection>();
                        foreach (CAGEAnimation.Connection connection in anim.connections)
                        {
                            List<CAGEAnimation.Animation> anim_target = anim.animations.FindAll(o => o.shortGUID == connection.keyframeID);
                            List<CAGEAnimation.Event> event_target = anim.events.FindAll(o => o.shortGUID == connection.keyframeID);
                            if (anim_target.Count == 0 && event_target.Count == 0) continue;
                            prunedConnections.Add(connection);
                        }
                        anim.connections = prunedConnections;

                        foreach (CAGEAnimation.Connection connection in anim.connections)
                        {
                            List<CAGEAnimation.Animation> anim_target = anim.animations.FindAll(o => o.shortGUID == connection.keyframeID);
                            List<CAGEAnimation.Event> event_target = anim.events.FindAll(o => o.shortGUID == connection.keyframeID);

                            //We expect to never point to both
                            if (anim_target.Count != 0 && event_target.Count != 0)
                            {
                                throw new Exception();
                            }
                            if (anim_target.Count > 1 || event_target.Count > 1)
                            {
                                throw new Exception();
                            }

                            if (connection.objectType == ObjectType.ENTITY)
                            {
                                //ENTITY links always point to Animation keyframes
                                if (anim_target.Count == 0 || event_target.Count != 0)
                                {
                                    throw new Exception();
                                }

                                //ENTITY links must always point to params, these appear to only be TRANSFORM or FLOAT in vanilla PAKs
                                if (connection.parameterDataType != DataType.TRANSFORM &&
                                    connection.parameterDataType != DataType.FLOAT)
                                {
                                    throw new Exception();
                                }

                                //Check to make sure all TRANSFORM keys happen on the same intervals & are complete
                                if (connection.parameterDataType == DataType.TRANSFORM)
                                {
                                    List<CAGEAnimation.Connection> transform = anim.connections.FindAll(o => o.connectedEntity == connection.connectedEntity && o.parameterID.ToString() == "position");
                                    if (transform.Count != 6 && transform.Count != 3 && transform.Count != 5) //x,y,z,Yaw,Pitch,Roll
                                    {
                                        throw new Exception();
                                    }
                                    List<float> keyframeIntervals = null;
                                    foreach (CAGEAnimation.Connection transformPart in transform)
                                    {
                                        CAGEAnimation.Animation keyframes = anim.animations.FirstOrDefault(o => o.shortGUID == connection.keyframeID);
                                        if (keyframeIntervals == null)
                                        {
                                            keyframeIntervals = new List<float>();
                                            foreach (CAGEAnimation.Animation.Keyframe keyframe in keyframes.keyframes)
                                            {
                                                keyframeIntervals.Add(keyframe.secondsSinceStart);
                                            }
                                        }
                                        else
                                        {
                                            if (keyframeIntervals.Count != keyframes.keyframes.Count)
                                            {
                                                throw new Exception();
                                            }
                                            for (int i = 0; i < keyframes.keyframes.Count; i++)
                                            {
                                                if (keyframeIntervals[i] != keyframes.keyframes[i].secondsSinceStart)
                                                {
                                                    throw new Exception();
                                                }
                                            }
                                        }
                                    }
                                }

                                //Check sub IDs for pointed datatypes
                                if (connection.parameterDataType == DataType.TRANSFORM)
                                {
                                    if (connection.parameterSubID.ToString() != "Yaw" &&
                                        connection.parameterSubID.ToString() != "Pitch" &&
                                        connection.parameterSubID.ToString() != "Roll" &&
                                        connection.parameterSubID.ToString() != "x" &&
                                        connection.parameterSubID.ToString() != "y" &&
                                        connection.parameterSubID.ToString() != "z")
                                    {
                                        throw new Exception();
                                    }
                                    //TODO: validate that all these vals are modified at the same keyframe times (can simplify UI!)
                                }
                                if (connection.parameterDataType == DataType.FLOAT)
                                {
                                    if (connection.parameterSubID.ToString() != "")
                                    {
                                        throw new Exception();
                                    }
                                }
                            }
                            else
                            {
                                //CHARACTER and MARKER links always point to Event keyframes
                                if (anim_target.Count != 0 || event_target.Count == 0)
                                {
                                    throw new Exception();
                                }

                                //CHARACTER links usually pair with MARKER links - check that
                                if (connection.objectType == ObjectType.CHARACTER)
                                {
                                    List<CAGEAnimation.Connection> pairedMarker = anim.connections.FindAll(o => o.objectType == ObjectType.MARKER && o.keyframeID == connection.keyframeID);
                                    if (pairedMarker.Count != 1)
                                    {
                                        //throw new Exception();
                                    }
                                    List<CAGEAnimation.Connection> duplicateCharRef = anim.connections.FindAll(o => o.objectType == ObjectType.CHARACTER && o.keyframeID == connection.keyframeID && o.shortGUID != connection.shortGUID);
                                    if (duplicateCharRef.Count != 0)
                                    {
                                        throw new Exception();
                                    }
                                }

                                //As we point to events and not parameters, this info should always be empty
                                if (connection.parameterID.ToString() != "" ||
                                    connection.parameterDataType != DataType.NONE ||
                                    connection.parameterSubID.ToString() != "")
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    });
                });
            });
#endif
        }

        private static List<string> unnamed_params = new List<string>();
        public static void DumpAllUnnamedParams()
        {
#if DEBUG
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            foreach (string file in files)
            {
                Commands phys = new Commands(file);
                foreach (Composite comp in phys.Entries)
                {
                    List<FunctionEntity> anims = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.CAGEAnimation));
                    foreach (FunctionEntity ent in anims)
                    {
                        CAGEAnimation anim = (CAGEAnimation)ent;
                        foreach (CAGEAnimation.Event key in anim.events)
                        {
                            foreach (CAGEAnimation.Event.Keyframe keyData in key.keyframes)
                            {
                                AddToListIfUnnamed(keyData.start);
                                AddToListIfUnnamed(keyData.unk3);
                            }
                        }
                    }

                    List<FunctionEntity> trigs = comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence));
                    foreach (FunctionEntity ent in trigs)
                    {
                        TriggerSequence trig = (TriggerSequence)ent;
                        foreach (TriggerSequence.Event e in trig.events)
                        {
                            AddToListIfUnnamed(e.start);
                            AddToListIfUnnamed(e.end);
                        }
                    }

                    foreach (Entity ent in comp.GetEntities())
                    {
                        foreach (Parameter p in ent.parameters)
                        {
                            AddToListIfUnnamed(p.name);
                        }
                    }
                }
            }
            File.WriteAllLines("unnamed.txt", unnamed_params);
#endif
        }
        private static void AddToListIfUnnamed(ShortGuid id)
        {
            if (id.ToString() != id.ToByteString()) return;
            if (unnamed_params.Contains(id.ToByteString())) return;
            unnamed_params.Add(id.ToByteString());
        }


        public static void ModelTestStuff()
        {
#if DEBUG
            /*
            AlienVBF vertexFormat = new AlienVBF();
            vertexFormat.Elements.Add(new AlienVBF.Element()
            {
                ArrayIndex = 0,
                Offset = 0,
                ShaderSlot = VBFE_InputSlot.VERTEX,
                Unknown_ = 2,
                VariableType = VBFE_InputType.VECTOR3,
                VariantIndex = 0,
            });
            vertexFormat.Elements.Add(new AlienVBF.Element()
            {
                ArrayIndex = 255,
                Offset = 0,
                ShaderSlot = VBFE_InputSlot.VERTEX,
                Unknown_ = 2,
                VariableType = VBFE_InputType.AlienVertexInputType_u16, // TODO!!!!! IS THIS ALWAYS THE LAST?
                VariantIndex = 0,
            });

            byte[] content = new byte[240];
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream(content)))
            {
                writer.Write((float)(1)); writer.Write((float)(1)); writer.Write((float)(-1));
                writer.Write((float)(1)); writer.Write((float)(-1)); writer.Write((float)(-1));
                writer.Write((float)(1)); writer.Write((float)(1)); writer.Write((float)(1));
                writer.Write((float)(1)); writer.Write((float)(-1)); writer.Write((float)(1));
                writer.Write((float)(-1)); writer.Write((float)(1)); writer.Write((float)(-1));
                writer.Write((float)(-1)); writer.Write((float)(-1)); writer.Write((float)(-1));
                writer.Write((float)(-1)); writer.Write((float)(1)); writer.Write((float)(1));
                writer.Write((float)(-1)); writer.Write((float)(-1)); writer.Write((float)(1));
                //96
            
                writer.Write((Int16)0); writer.Write((Int16)0); writer.Write((Int16)0);
                writer.Write((Int16)4); writer.Write((Int16)4); writer.Write((Int16)0);
                writer.Write((Int16)6); writer.Write((Int16)8); writer.Write((Int16)0);
                writer.Write((Int16)2); writer.Write((Int16)2); writer.Write((Int16)0);

                writer.Write((Int16)3); writer.Write((Int16)3); writer.Write((Int16)1);
                writer.Write((Int16)2); writer.Write((Int16)2); writer.Write((Int16)1);
                writer.Write((Int16)6); writer.Write((Int16)9); writer.Write((Int16)1);
                writer.Write((Int16)7); writer.Write((Int16)11); writer.Write((Int16)1);

                writer.Write((Int16)7); writer.Write((Int16)12); writer.Write((Int16)2);
                writer.Write((Int16)6); writer.Write((Int16)10); writer.Write((Int16)2);
                writer.Write((Int16)4); writer.Write((Int16)5); writer.Write((Int16)2);
                writer.Write((Int16)5); writer.Write((Int16)7); writer.Write((Int16)2);

                writer.Write((Int16)5); writer.Write((Int16)6); writer.Write((Int16)3);
                writer.Write((Int16)1); writer.Write((Int16)1); writer.Write((Int16)3);
                writer.Write((Int16)3); writer.Write((Int16)3); writer.Write((Int16)3);
                writer.Write((Int16)7); writer.Write((Int16)13); writer.Write((Int16)3);

                writer.Write((Int16)1); writer.Write((Int16)1); writer.Write((Int16)4);
                writer.Write((Int16)0); writer.Write((Int16)0); writer.Write((Int16)4);
                writer.Write((Int16)2); writer.Write((Int16)2); writer.Write((Int16)4);
                writer.Write((Int16)3); writer.Write((Int16)3); writer.Write((Int16)4);

                writer.Write((Int16)5); writer.Write((Int16)7); writer.Write((Int16)5);
                writer.Write((Int16)4); writer.Write((Int16)5); writer.Write((Int16)5);
                writer.Write((Int16)0); writer.Write((Int16)0); writer.Write((Int16)5);
                writer.Write((Int16)1); writer.Write((Int16)1); writer.Write((Int16)5);
                //144
            }

            */
            /*
            CS2 model = new CS2();
            model.Name = "SomeTestShit";
            model.Submeshes.Add(new CS2.Submesh()
            {
                Name = "Test",
                LODMinDistance_ = 0,
                LODMaxDistance_ = 10000,
                AABBMin = new Vector3(),
                AABBMax = new Vector3(),
                MaterialLibraryIndex = 1,
                Unknown2_ = 134239232,
                UnknownIndex = -1,
                CollisionIndex_ = -1,
                // VertexFormat = vertexFormat,
                // VertexFormatLowDetail = vertexFormat,
                ScaleFactor = 4,
                HeadRelated_ = -1,
            });
            //model.Submeshes[0].content = content;
            model.Submeshes[0].IndexCount = 48;
            model.Submeshes[0].VertexCount = 8;

            CathodeLib.Level lvl = new Level(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/opencage/");
            /*
            CS2 model = lvl.AllModels.Entries.FirstOrDefault(o => o.Name == "SomeTestShit");
            if (model == null)
            {
                throw new Exception("bruh");
            }
            */
            /*
            CS2 modelCopy = lvl.Models.Entries.FirstOrDefault(o => o.Submeshes.FirstOrDefault(x => x.Name.Contains("Sphere")) != null);
            if (modelCopy == null)
            {
                throw new Exception("bruh");
            }

            model.Submeshes[0].VertexFormat = modelCopy.Submeshes[0].VertexFormat;
            model.Submeshes[0].VertexFormatLowDetail = modelCopy.Submeshes[0].VertexFormatLowDetail;
            model.Submeshes[0].content = modelCopy.Submeshes[0].content;
            model.Submeshes[0].IndexCount = modelCopy.Submeshes[0].IndexCount;
            model.Submeshes[0].VertexCount = modelCopy.Submeshes[0].VertexCount;
            model.Submeshes[0].Unknown2_ = modelCopy.Submeshes[0].Unknown2_;
            model.Submeshes[0].AABBMax = modelCopy.Submeshes[0].AABBMax;
            model.Submeshes[0].AABBMin = modelCopy.Submeshes[0].AABBMin;

            File.WriteAllBytes("out.bin", model.Submeshes[0].content);

            AlienVBF vertexFormat = new AlienVBF();
            /*
            vertexFormat.Elements.Add(new AlienVBF.Element()
            {
                //ArrayIndex = 0,
                Offset = 0,
                ShaderSlot = VBFE_InputSlot.VERTEX,
                VariableType = VBFE_InputType.VECTOR4_INT16_DIVMAX,
                VariantIndex = 0,
            });
            vertexFormat.Elements.Add(new AlienVBF.Element()
            {
                //ArrayIndex = 255,
                Offset = 0,
                ShaderSlot = VBFE_InputSlot.VERTEX,
                VariableType = VBFE_InputType.AlienVertexInputType_u16, // TODO!!!!! IS THIS ALWAYS THE LAST?
                VariantIndex = 0,
            });*/

            //TODO: go through using vertex format and pull out verts and indices, and then only write them
            //using (BinaryWriter writer = new BinaryWriter(new MemoryStream(model.Submeshes[0].content)))
            //{
            //
            //}
            //model.Submeshes[0].content = content;
            //model.Submeshes[0].VertexFormat = vertexFormat;
            //model.Submeshes[0].VertexFormatLowDetail = vertexFormat;

            //lvl.Save();
#endif
        }

        public static void LoadAllFileTests()
        {
#if DEBUG
            //Models mdls = new Models(SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\BSP_TORRENS\\RENDERABLE\\LEVEL_MODELS.PAK");
            //mdls.Save();
            /*
            CathodeModels mdls_old = new CathodeModels(
                SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\BSP_TORRENS\\RENDERABLE\\MODELS_LEVEL.BIN",
                SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\BSP_TORRENS\\RENDERABLE\\LEVEL_MODELS.PAK");

            int binIndex = 0;
            for (int i = 0; i < mdls.Entries.Count; i++)
            {
                for (int y = 0; y < mdls.Entries[i].Submeshes.Count; y++)
                {
                    File.WriteAllBytes(binIndex + "_new.bin", mdls.Entries[i].Submeshes[y].content);
                    binIndex++;
                }
            }

            for (int i = 0; i < mdls_old.Models.Count; i++)
            {
                for (int y = 0; y < mdls_old.Models[i].Submeshes.Count; y++)
                {
                    File.WriteAllBytes(mdls_old.Models[i].Submeshes[y].binIndex + "_old.bin", mdls_old.Models[i].Submeshes[y].content);
                }
            }

           // mdls.Save();

            for (int i = 0; i < mdls_old.Models.Count; i++)
            {
                for (int x = 0;x < mdls_old.Models[i].Submeshes.Count; x++)
                {
                    int correct = mdls_old.Models[i].Submeshes[x].content.Length;
                    int newmethod = mdls.Entries[i].Submeshes[x].content.Length;
                    Console.WriteLine("Correct = " + correct + ", new = " + newmethod);
                }
            }
            */
            //return;

            //string sdffds = "";

            //Models mdls = new Models(SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\ENG_ALIEN_NEST\\RENDERABLE\\LEVEL_MODELS.PAK");
            //mdls.Save();
            ////Textures tex34 = new Textures(SharedData.pathToAI + "\\DATA\\ENV\\PRODUCTION\\BSP_LV426_PT01\\RENDERABLE\\LEVEL_TEXTURES.ALL.PAK");
            ////tex34.Save();
            //return;

            /*
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "LEVEL_TEXTURES.ALL.PAK", SearchOption.AllDirectories).ToList<string>();
            Textures texBase = new Textures("LEVEL_TEXTURES.ALL.PAK");
            foreach (string file in files)
            {
                Textures tex = new Textures(file);
                for (int i = 0; i < tex.Entries.Count; i++)
                {
                    Textures.TEX4 texture = texBase.Entries.FirstOrDefault(o => o.FileName == tex.Entries[i].FileName);
                    if (texture != null) continue;
                    texBase.Entries.Add(tex.Entries[i]);
                }


                //Models mdl = new Models(file);
                //Console.WriteLine(mdl.Loaded + " -> " + file);
                //Console.WriteLine("Saved: " + mdl.Save());
                //return;
            }
            texBase.Save();

            return;
            */

            /*
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "LEVEL_MODELS.PAK", SearchOption.AllDirectories).ToList<string>();
            //Models texBase = new Models("LEVEL_MODELS.PAK");
            List<AlienVBF> formats = new List<AlienVBF>();
            foreach (string file in files)
            {
                Console.WriteLine(file);
                Models tex = new Models(file);
                for (int i = 0; i < tex.Entries.Count; i++)
                {
                    for (int y = 0; y < tex.Entries[i].Submeshes.Count; y++)
                    {
                        /*
                        if (tex.Entries[i].Submeshes[y].VertexFormat.Elements.Count == 2)
                        {
                            if (!formats.Contains(tex.Entries[i].Submeshes[y].VertexFormat))
                                formats.Add(tex.Entries[i].Submeshes[y].VertexFormat);
                        }
                        if (tex.Entries[i].Submeshes[y].VertexFormatLowDetail.Elements.Count == 2)
                        {
                            if (!formats.Contains(tex.Entries[i].Submeshes[y].VertexFormatLowDetail))
                                formats.Add(tex.Entries[i].Submeshes[y].VertexFormatLowDetail);
                        }
                        */
            /*
                        for (int x= 0; x < tex.Entries[i].Submeshes[y].VertexFormat.Elements.Count; x++)
                        {
                            if (tex.Entries[i].Submeshes[y].VertexFormat.Elements[x].ArrayIndex != 0 &&
                                tex.Entries[i].Submeshes[y].VertexFormat.Elements[x].ArrayIndex != 255)
                            {
                                string sdfsdf = "";
                            }
                        }
                    }
                }
                 
                //for (int i = 0; i < tex.Entries.Count; i++)
                //{
                //     Models.CS2 texture = texBase.Entries.FirstOrDefault(o => o.name == tex.Entries[i].name);
                //    if (texture != null) continue;
                //     texBase.Entries.Add(tex.Entries[i]);
                // }


                //Models mdl = new Models(file);
                //Console.WriteLine(mdl.Loaded + " -> " + file);
                //Console.WriteLine("Saved: " + mdl.Save());
                //return;
            }
            //texBase.Save();

            return;*/
#endif
        }

        public static void TestAllPhysMap()
        {
#if DEBUG
            List<string> files = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "LEVEL_MODELS.PAK", SearchOption.AllDirectories).ToList<string>();
            foreach (string file in files)
            {
                Models phys = new Models(file);
                Console.WriteLine("[" + phys.Loaded + "] " + file);
                for (int i = 0; i < phys.Entries.Count; i++)
                {

                }
                //phys.Save();
            }
#endif
        }

        private static void WriteVert(float x, float y, float z, BinaryWriter writer)
        {
            Int16 x_16 = ((Int16)(x * Int16.MaxValue));
            Int16 y_16 = ((Int16)(y * Int16.MaxValue));
            Int16 z_16 = ((Int16)(z * Int16.MaxValue));
            writer.Write(x_16);
            writer.Write(y_16);
            writer.Write(z_16);
            writer.Write((Int16)(-Int16.MaxValue));
        }


        public static void DumpEnumList()
        {
#if DEBUG
            List<string> enumfile = File.ReadAllLines(@"C:\Users\mattf\Downloads\enums.txt").ToList<string>(); //https://myfiles.mattfiler.co.uk/enums.txt
            Dictionary<string, List<string>> outputtt = new Dictionary<string, List<string>>();
            string currenteunm = "";
            foreach (string str in enumfile)
            {
                if (str.Length > 2 && str.Substring(0, 2) == "##")
                {
                    currenteunm = str.Substring(3).Trim();
                    outputtt.Add(currenteunm, new List<string>());
                }
                if (str.Length > 2 && str.Substring(0, 2) == " *")
                {
                    outputtt[currenteunm].Add(str.Substring(3).Trim());
                }
            }
            File.WriteAllText("enums.json", JsonConvert.SerializeObject(outputtt, Formatting.Indented));

            List<string> enums = new List<string>();
            foreach (var val in outputtt)
            {
                enums.Add(val.Key);
            }
            File.WriteAllLines("enums.txt", enums);

            BinaryWriter write = new BinaryWriter(File.OpenWrite("enums.bin"));
            write.BaseStream.SetLength(0);
            foreach (var str in outputtt)
            {
                write.Write(ShortGuidUtils.Generate(str.Key).ToUInt32());
                write.Write(str.Key);
                write.Write(str.Value.Count);
                foreach (string str_ in str.Value)
                {
                    write.Write(str_);
                }
            }
            write.Close();
            return;
#endif
        }

#if DEBUG
        private class EntityDef
        {
            public string title = "";
            public Dictionary<ParameterVariant, List<ParameterDef>> stuff = new Dictionary<ParameterVariant, List<ParameterDef>>();
        }
        private class ParameterDef
        {
            public string name;
            public string datatype;
            public string defaultval;
        }
#endif

        public static void DumpCathodeEntities()
        {
#if DEBUG
            List<string> all_datatypes = new List<string>();
            List<string> all_types = new List<string>();

            List<string> content = File.ReadAllLines(@"C:\Users\mattf\Downloads\all_nodes_with_params.html").ToList<string>(); //https://myfiles.mattfiler.co.uk/all_nodes_with_params.html
            List<string> content2 = File.ReadAllLines(@"C:\Users\mattf\Downloads\all_nodes_with_params1.html").ToList<string>(); //https://myfiles.mattfiler.co.uk/all_nodes_with_params1.html
            List<EntityDef> ents = new List<EntityDef>();
            EntityDef currentEnt = null;
            for (int i = 0; i < content.Count; i++)
            {
                if (content[i].Length > 3 && content[i].Substring(0, 3) == "<h3")
                {
                    if (currentEnt != null)
                    {
                        ents.Add(currentEnt);
                    }
                    currentEnt = new EntityDef();
                    string split = content[i].Split('>')[1];
                    currentEnt.title = split.Substring(0, split.Length - 4);
                }
                else if (content[i].Length > 3 && content[i].Substring(0, 3) == "<li")
                {
                    string type_s = (content[i].Split('[')[1].Split(']')[0]).ToUpper();
                    if (type_s == "RESOURCE") continue;

                    Enum.TryParse(type_s, out ParameterVariant type);
                    if (!currentEnt.stuff.ContainsKey(type)) currentEnt.stuff.Add(type, new List<ParameterDef>());

                    string split = content[i].Split(']')[1];
                    if (split.Contains("["))
                    {
                        split = split.Split('[')[0];
                        split = split.Substring(1, split.Length - 2);
                    }
                    else
                    {
                        split = split.Substring(1, split.Length - 6);
                    }
                    string[] split2_l = content[i].Split(new string[] { "DataType: " }, StringSplitOptions.None);
                    string[] split_3 = content2[i].Split(new string[] { "DefaultVal: " }, StringSplitOptions.None);
                    string datatype = (split2_l.Length > 1) ? split2_l[1].Substring(0, split2_l[1].Length - 6) : "";
                    string defaultval = (split_3.Length > 1) ? split_3[1].Substring(0, split_3[1].Length - 6) : "";

                    currentEnt.stuff[type].Add(new ParameterDef() { datatype = datatype, name = split, defaultval = defaultval });

                    if (!all_types.Contains(type.ToString()))
                        all_types.Add(type.ToString());
                    if (!all_datatypes.Contains(datatype))
                        all_datatypes.Add(datatype);

                }
            }
            ents.Add(currentEnt);

            List<string> type_dump = new List<string>();
            type_dump.Add("{\"data\":[");
            foreach (EntityDef def in ents)
            {
                if (def.title == @"n:\\content\\build\\library\\archetypes\\gameplay\\gcip_worldpickup")
                    def.title = "GCIP_WorldPickup";
                if (def.title == @"n:\\content\\build\\library\\ayz\\animation\\logichelpers\\playforminduration")
                    def.title = "PlayForMinDuration";
                if (def.title == @"n:\\content\\build\\library\\archetypes\\script\\gameplay\\torch_control")
                    def.title = "Torch_Control";

                type_dump.Add("{\"type\": \"" + def.title + "\", \"data\": {");
                foreach (var val in def.stuff)
                {
                    type_dump.Add("\"" + val.Key + "\": [");
                    foreach (ParameterDef valu in val.Value)
                    {
                        type_dump.Add("\"" + valu.name + "\", ");
                    }
                    if (val.Value.Count != 0) type_dump[type_dump.Count - 1] = type_dump[type_dump.Count - 1].Substring(0, type_dump[type_dump.Count - 1].Length - 2);
                    type_dump.Add("], ");
                }
                if (def.stuff.Count != 0) type_dump[type_dump.Count - 1] = "]";
                type_dump.Add("}},");
            }
            if (ents.Count != 0) type_dump[type_dump.Count - 1] = "}}";
            type_dump.Add("]}");
            string ffff = "";
            for (int i = 0; i < type_dump.Count; i++)
                ffff += type_dump[i];
            File.WriteAllText("types.json", JObject.Parse(ffff).ToString(Formatting.Indented));

            List<string> resource_types = new List<string>();

            List<string> scripting = new List<string>();
            foreach (EntityDef def in ents)
            {
                if (def.title == @"n:\\content\\build\\library\\archetypes\\gameplay\\gcip_worldpickup")
                    def.title = "GCIP_WorldPickup";
                if (def.title == @"n:\\content\\build\\library\\ayz\\animation\\logichelpers\\playforminduration")
                    def.title = "PlayForMinDuration";
                if (def.title == @"n:\\content\\build\\library\\archetypes\\script\\gameplay\\torch_control")
                    def.title = "Torch_Control";

                if (def.stuff.Count != 0)
                {
                    scripting.Add("case FunctionType." + def.title + ":");
                    if (def.title == "CAGEAnimation") scripting.Add("\tnewEntity = new CAGEAnimation(thisID);");
                    if (def.title == "TriggerSequence") scripting.Add("\tnewEntity = new TriggerSequence(thisID);");
                }
                foreach (var val in def.stuff)
                {
                    foreach (ParameterDef valu in val.Value)
                    {
                        if (valu.name == "resource")
                        {
                            if (def.title != "ModelReference" && def.title != "EnvironmentModelReference")
                                scripting.Add("\tnewEntity.AddResource(ResourceType." + valu.datatype + ");");
                        }
                        else
                        {
                            string defaults = "";
                            string type = "";
                            defaults = valu.defaultval;
                            switch (valu.datatype)
                            {
                                case "":
                                case "Object":
                                case "ZonePtr":
                                case "ZoneLinkPtr":
                                case "ResourceID":
                                case "ReferenceFramePtr":
                                case "AnimationInfoPtr":
                                    type = "ParameterData"; //TODO
                                    defaults = "";
                                    break;

                                case "int":
                                    type = "cInteger";
                                    break;
                                case "bool":
                                    type = "cBool";
                                    break;
                                case "float":
                                    type = "cFloat";
                                    if (defaults != "") defaults += "f";
                                    break;
                                case "String":
                                    type = "cString";
                                    defaults = "\"" + defaults + "\"";
                                    break;
                                case "Position":
                                    type = "cTransform";
                                    break;
                                case "FilePath":
                                    type = "cString";
                                    break;
                                case "SPLINE":
                                case "SplineData":
                                    type = "cSpline";
                                    if (defaults == "0") defaults = "";
                                    break;
                                case "Direction":
                                    type = "cVector3";
                                    if (defaults == "0") defaults = "";
                                    if (defaults == "default_Direction") defaults = "";
                                    break;
                                case "Enum":
                                    type = "cEnum";
                                    if (defaults == "0xffffffff00000000") defaults = "";
                                    break;
                                default:
                                    type = "cEnum";
                                    string[] spl = valu.defaultval.Split('(');
                                    if (spl.Length > 1) defaults = "EnumType." + spl[0].Substring(0, spl[0].Length - 1) + ", " + spl[1].Substring(0, spl[1].Length - 1);
                                    else
                                    {
                                        if (EnumUtils.GetEnum(ShortGuidUtils.Generate(valu.datatype)) == null)
                                        {
                                            type = "cResource";
                                            defaults = "new ResourceReference[]{ new ResourceReference(ResourceType." + valu.datatype + ") }.ToList<ResourceReference>(), newEntity.shortGUID";
                                            if (!resource_types.Contains(valu.datatype.ToString()))
                                                resource_types.Add(valu.datatype.ToString());
                                        }
                                        else
                                        {
                                            defaults = "\"" + valu.datatype + "\", 0";
                                        }
                                    }
                                    break;
                            }
                            if (defaults.Length > ("NEON_fmov").Length && defaults.Substring(0, ("NEON_fmov").Length) == "NEON_fmov") defaults = "";
                            scripting.Add("\tnewEntity.AddParameter(\"" + valu.name + "\", new " + type + "(" + defaults + "), ParameterVariant." + val.Key + "); //" + valu.datatype);
                        }
                    }
                }
                if (def.stuff.Count != 0)
                {
                    if (def.title == "ModelReference")
                    {
                        scripting.Add("\tcResource resourceData = new cResource(newEntity.shortGUID);");
                        scripting.Add("\tresourceData.AddResource(ResourceType.RENDERABLE_INSTANCE);");
                        scripting.Add("\tnewEntity.parameters.Add(new Parameter(\"resource\", resourceData, ParameterVariant.INTERNAL));");
                    }
                    if (def.title == "EnvironmentModelReference")
                    {
                        scripting.Add("\tcResource resourceData2 = new cResource(newEntity.shortGUID);");
                        scripting.Add("\tresourceData2.AddResource(ResourceType.ANIMATED_MODEL);");
                        scripting.Add("\tnewEntity.parameters.Add(new Parameter(\"resource\", resourceData2, ParameterVariant.INTERNAL));");
                    }
                    if (def.title == "PhysicsSystem") scripting.Add("\tnewEntity.AddResource(ResourceType.DYNAMIC_PHYSICS_SYSTEM).startIndex = 0;");
                    scripting.Add("break;");
                }
            }
            File.WriteAllLines("out.cs", scripting);

            Console.WriteLine(JsonConvert.SerializeObject(all_types, Formatting.Indented));
            Console.WriteLine(JsonConvert.SerializeObject(all_datatypes, Formatting.Indented));
            Console.WriteLine(JsonConvert.SerializeObject(resource_types, Formatting.Indented));

            string bleh = JsonConvert.SerializeObject(ents, Formatting.Indented);
            File.WriteAllText("out.json", bleh);
#endif
        }

        public static void FindAllNodesInCommands(CommandsEditor editor)
        {
#if DEBUG
            /*
            List<string> mapList = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            for (int i = 0; i < mapList.Count; i++)
            {
                string[] fileSplit = mapList[i].Split(new[] { "PRODUCTION" }, StringSplitOptions.None);
                string mapName = fileSplit[fileSplit.Length - 1].Substring(1, fileSplit[fileSplit.Length - 1].Length - 20);
                mapList[i] = (mapName);
            }
            mapList.Remove("DLC\\BSPNOSTROMO_RIPLEY"); mapList.Remove("DLC\\BSPNOSTROMO_TWOTEAMS");

            for (int mm = 0; mm < mapList.Count; mm++)
            {
                //if (env_list.Items[mm].ToString() != "BSP_TORRENS") continue;

                editor.Editor.commands = new Commands(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + mapList[mm].ToString() + "/WORLD/COMMANDS.PAK");
                Console.WriteLine("Loading: " + editor.Editor.commands.Filepath + "...");
                //CurrentInstance.compositeLookup = new EntityNameLookup(CurrentInstance.commandsPAK);

                //EnvironmentAnimationDatabase db = new EnvironmentAnimationDatabase(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + env_list.Items[mm].ToString() + "/WORLD/ENVIRONMENT_ANIMATION.DAT");
                //Console.WriteLine(db.Header.MatrixCount0);
                //Console.WriteLine(db.Header.MatrixCount1);
                //Console.WriteLine(db.Header.EntryCount1);
                //Console.WriteLine(db.Header.EntryCount0);
                //Console.WriteLine(db.Header.IDCount0);
                //Console.WriteLine(db.Header.IDCount1);
                //Console.WriteLine(db.Header.Unknown1_);

                string[] towrite = new string[200];
                for (int i = 0; i < editor.Editor.commands.Entries.Count; i++)
                {
                    for (int x = 0; x < editor.Editor.commands.Entries[i].functions.Count; x++)
                    {
                        if (!CommandsUtils.FunctionTypeExists(editor.Editor.commands.Entries[i].functions[x].function)) continue;
                        FunctionType type = CommandsUtils.GetFunctionType(editor.Editor.commands.Entries[i].functions[x].function);
                        switch (type)
                        {
                            case FunctionType.CameraShake:
                            case FunctionType.BoneAttachedCamera:
                            case FunctionType.CamPeek:
                            case FunctionType.ClipPlanesController:
                            case FunctionType.ControllableRange:
                            case FunctionType.FixedCamera:
                                //ResourceReference rr = CurrentInstance.commandsPAK.Composites[i].functions[x].resources.FirstOrDefault(o => o.entryType == ResourceType.COLLISION_MAPPING);
                                //if (rr == null)
                                //{
                                //    Console.WriteLine("NULL");
                                //}
                                //else
                                //{
                                //    Console.WriteLine(rr.startIndex + " / " + rr.count + " / " + rr.entityID);
                                //    //Console.WriteLine(JsonConvert.SerializeObject(rr));
                                //}

                                //if (CurrentInstance.commandsPAK.Composites[i].functions[x].resources.Count != 0)
                                //{
                                //    string breasdfdf = "";
                                //    if (CurrentInstance.commandsPAK.Composites[i].functions[x].GetResource(ResourceType.COLLISION_MAPPING) == null)
                                //    {
                                //        string sdfsd = "";
                                //    }
                                //}


                                //Console.WriteLine(CurrentInstance.commandsPAK.Composites[i].name + " -> " + CurrentInstance.commandsPAK.Composites[i].functions[x].shortGUID + " -> " +  type);
                                //for (int y = 0; y < CurrentInstance.commandsPAK.Composites[i].functions[x].resources.Count; y++)
                                //{
                                //    Console.WriteLine("\t" + CurrentInstance.commandsPAK.Composites[i].functions[x].resources[y].entryType);
                                //}


                                //Console.WriteLine(type);

                                //Console.WriteLine("");

                                //Composite comp = CurrentInstance.commandsPAK.Composites[i];
                                //List<ResourceReference> rr = ((cResource)comp.functions[x].GetParameter("resource").content).value;
                                //towrite[rr[0].startIndex] = rr[0].startIndex  + "\n\t" + comp.name + "\n\t" + CurrentInstance.compositeLookup.GetEntityName(comp.shortGUID, comp.functions[x].shortGUID);

                                //Console.WriteLine(rr.Count);


                                Console.WriteLine(editor.Editor.commands.Entries[i].name + " -> " + editor.Editor.commands.Entries[i].functions[x].shortGUID + " -> " + type);

                                //for (int y = 0; y < CurrentInstance.commandsPAK.Composites[i].functions[x].resources.Count; y++)
                                //{
                                //    ResourceReference rr = CurrentInstance.commandsPAK.Composites[i].functions[x].resources[y];
                                //    if (rr.entryType != ResourceType.RENDERABLE_INSTANCE)
                                //    {
                                //        Console.WriteLine(" !!!!!!!  FOUND " + rr.entryType);
                                //    }
                                //    Console.WriteLine(JsonConvert.SerializeObject(rr));
                                //}
                                break;
                        }
                    }
                }
                //foreach (string line in towrite)
                //{
                //    if (line == null)
                //    {
                //        Console.WriteLine("--");
                //    }
                //    else
                //    {
                //        Console.WriteLine(line);
                //    }
                //}

                //CurrentInstance.commandsPAK.Save();
            }
            */
#endif
        }

        public static void LoadAndSaveAllCommands()
        {
#if DEBUG
            List<string> pairs = new List<string>();
            List<string> commandsFiles = Directory.GetFiles(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            for (int i = 0; i < commandsFiles.Count; i++)
            {
                Console.WriteLine(commandsFiles[i]);
                Commands cmd = new Commands(commandsFiles[i]);

                /*
                foreach (Composite comp in cmd.Composites)
                {
                    int numberOfFunctionNodes = comp.functions.FindAll(o => CommandsUtils.FunctionTypeExists(o.function)).Count;
                    int numberOfFunctionNodesIncludingCompositeRefs = comp.functions.Count;

                    int numberOfExcludedNodes = 0;
                    numberOfExcludedNodes += comp.functions.FindAll(o => o.resources.Count != 0).Count;

                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.Zone)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.TriggerSequence)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.ParticleEmitterReference)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.Master)).Count; //OR LogicGate

                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.ModelReference)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.SoundEnvironmentMarker)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.LightReference)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.ModelReference)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.EnvironmentModelReference)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.GPU_PFXEmitterReference)).Count;
                    //numberOfExcludedNodes += comp.functions.FindAll(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.RibbonEmitterReference)).Count;

                    numberOfFunctionNodes -= numberOfExcludedNodes;
                    numberOfFunctionNodesIncludingCompositeRefs -= numberOfExcludedNodes;

                    pairs.Add(comp.name + " => " + comp.unk1 + ", " + comp.unk2 + " => " + numberOfFunctionNodes + ", " + numberOfFunctionNodesIncludingCompositeRefs);

                    if (comp.unk1 != numberOfFunctionNodes || comp.unk2 != numberOfFunctionNodesIncludingCompositeRefs)
                    {
                        Dictionary<string, int> counts = new Dictionary<string, int>();
                        foreach (FunctionEntity ent in comp.functions.FindAll(o => CommandsUtils.FunctionTypeExists(o.function)))
                        {
                            if (!counts.ContainsKey(ent.function.ToString()))
                                counts.Add(ent.function.ToString(), 0);
                            counts[ent.function.ToString()]++;
                        }

                        string breakhere = "";
                    }
                }
                */
                cmd.Save();
            }
            string fdfsdf = "";
#endif
        }

        public static List<string> CommandsToScript(Commands cmd)
        {
#if DEBUG
            List<string> script = new List<string>();
            script.Add("Commands cmd = new Commands(\"COMMANDS.PAK\");");
            script.Add("cmd.Composites.Clear();");
            for (int i = 0; i < cmd.Entries.Count; i++)
            {
                string compositeName = "COMP_" + cmd.Entries[i].shortGUID.ToByteString().Replace('-', '_');
                script.Add("Composite " + compositeName + " = cmd.AddComposite(@\"" + cmd.Entries[i].name + "\");");

                for (int x = 0; x < cmd.Entries[i].functions.Count; x++)
                {
                    string entityName = "ENT_" + cmd.Entries[i].functions[x].shortGUID.ToByteString().Replace('-', '_');
                    script.Add("FunctionEntity " + entityName + " = " + compositeName + ".AddFunction(");
                    if (CommandsUtils.FunctionTypeExists(cmd.Entries[i].functions[x].function)) script[script.Count - 1] += "FunctionType." + CommandsUtils.GetFunctionType(cmd.Entries[i].functions[x].function) + ");";
                    else script[script.Count - 1] += "@\"" + cmd.GetComposite(cmd.Entries[i].functions[x].function).name + "\");";

                    for (int y = 0; y < cmd.Entries[i].functions[x].resources.Count; y++)
                    {
                        string resourceName = "RES_" + cmd.Entries[i].functions[x].resources[y].GetHashCode().ToString().Replace('-', '_');
                        switch (cmd.Entries[i].functions[x].resources[y].resource_type)
                        {
                            case ResourceType.RENDERABLE_INSTANCE:
                                script.Add("ResourceReference " + resourceName + " = " + entityName + ".AddResource(ResourceType." + cmd.Entries[i].functions[x].resources[y].resource_type + ");");
                                Vector3 pos = cmd.Entries[i].functions[x].resources[y].position;
                                script.Add(resourceName + ".position = new Vector3(" + pos.X + "f, " + pos.Y + "f, " + pos.Z + "f);");
                                Vector3 rot = cmd.Entries[i].functions[x].resources[y].rotation;
                                script.Add(resourceName + ".rotation = new Vector3(" + rot.X + "f, " + rot.Y + "f, " + rot.Z + "f);");
                                script.Add(resourceName + ".index = " + cmd.Entries[i].functions[x].resources[y].index + ";");
                                script.Add(resourceName + ".count = " + cmd.Entries[i].functions[x].resources[y].count + ";");
                                break;
                            default:
                                throw new Exception("Unhandled resource");
                        }
                    }
                }
                for (int x = 0; x < cmd.Entries[i].variables.Count; x++)
                {
                    string entityName = "ENT_" + cmd.Entries[i].variables[x].shortGUID.ToByteString().Replace('-', '_');
                    script.Add("VariableEntity " + entityName + " = " + compositeName + ".AddVariable(\"" + ShortGuidUtils.FindString(cmd.Entries[i].variables[x].name) + "\", DataType." + cmd.Entries[i].variables[x].type.ToString() + ");");
                }
                for (int x = 0; x < cmd.Entries[i].proxies.Count; x++)
                {
                    throw new Exception("Unhandled proxy");
                }
                for (int x = 0; x < cmd.Entries[i].aliases.Count; x++)
                {
                    throw new Exception("Unhandled alias");
                }

                List<Entity> entities = cmd.Entries[i].GetEntities();
                for (int x = 0; x < entities.Count; x++)
                {
                    string entityName = "ENT_" + entities[x].shortGUID.ToByteString().Replace('-', '_');
                    for (int y = 0; y < entities[x].parameters.Count; y++)
                    {
                        string paramName = ShortGuidUtils.FindString(entities[x].parameters[y].name);
                        script.Add(entityName + ".AddParameter(" + ((paramName == entities[x].parameters[y].name.ToByteString()) ? "new ShortGuid(\"" : "\"") + paramName + "\"" + ((paramName == entities[x].parameters[y].name.ToByteString()) ? ")" : "") + ", new ");
                        switch (entities[x].parameters[y].content.dataType)
                        {
                            case DataType.FLOAT:
                                script[script.Count - 1] += "cFloat(" + ((cFloat)entities[x].parameters[y].content).value + "f)";
                                break;
                            case DataType.BOOL:
                                script[script.Count - 1] += "cBool(" + ((cBool)entities[x].parameters[y].content).value.ToString().ToLower() + ")";
                                break;
                            case DataType.ENUM:
                                cEnum en = ((cEnum)entities[x].parameters[y].content);
                                script[script.Count - 1] += "cEnum(\"" + ShortGuidUtils.FindString(en.enumID) + "\", " + en.enumIndex + ")";
                                break;
                            case DataType.FILEPATH:
                            case DataType.STRING:
                                script[script.Count - 1] += "cString(@\"" + ((cString)entities[x].parameters[y].content).value + "\")";
                                break;
                            case DataType.INTEGER:
                                script[script.Count - 1] += "cInteger(" + ((cInteger)entities[x].parameters[y].content).value + ")";
                                break;
                            case DataType.VECTOR:
                                Vector3 vc = ((cVector3)entities[x].parameters[y].content).value;
                                script[script.Count - 1] += "cVector3(new Vector3(" + vc.X + "f, " + vc.Y + "f, " + vc.Z + "f))";
                                break;
                            case DataType.TRANSFORM:
                                Vector3 rot = ((cTransform)entities[x].parameters[y].content).rotation;
                                Vector3 pos = ((cTransform)entities[x].parameters[y].content).position;
                                script[script.Count - 1] += "cTransform(new Vector3(" + pos.X + "f, " + pos.Y + "f, " + pos.Z + "f), new Vector3(" + rot.X + "f, " + rot.Y + "f, " + rot.Z + "f))";
                                break;
                            default:
                                throw new Exception("Unhandled parameter datatype");
                        }
                        script[script.Count - 1] += ");";
                    }
                    for (int y = 0; y < entities[x].childLinks.Count; y++)
                    {
                        string connectedEntityName = "ENT_" + entities[x].childLinks[y].linkedEntityID.ToByteString().Replace('-', '_');
                        script.Add(entityName + ".AddParameterLink(\"" + ShortGuidUtils.FindString(entities[x].childLinks[y].thisParamID) + "\", " + connectedEntityName + ", \"" + ShortGuidUtils.FindString(entities[x].childLinks[y].linkedParamID) + "\");");
                    }
                }
            }
            script.Add("cmd.Save();");
            return script;
#else
            return null;
#endif
        }
    }
}
