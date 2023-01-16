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

namespace CathodeEditorGUI
{
    public static class LocalDebug
    {
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
                write.Write(ShortGuidUtils.Generate(str.Key).val);
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

        public static void FindAllNodesInCommands()
        {
#if DEBUG
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

                Editor.commands = new Commands(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + mapList[mm].ToString() + "/WORLD/COMMANDS.PAK");
                Console.WriteLine("Loading: " + Editor.commands.Filepath + "...");
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
                for (int i = 0; i < Editor.commands.Composites.Count; i++)
                {
                    for (int x = 0; x < Editor.commands.Composites[i].functions.Count; x++)
                    {
                        if (!CommandsUtils.FunctionTypeExists(Editor.commands.Composites[i].functions[x].function)) continue;
                        FunctionType type = CommandsUtils.GetFunctionType(Editor.commands.Composites[i].functions[x].function);
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


                                Console.WriteLine(Editor.commands.Composites[i].name + " -> " + Editor.commands.Composites[i].functions[x].shortGUID + " -> " + type);

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
            List<string> script = new List<string>();
            script.Add("Commands cmd = new Commands(\"COMMANDS.PAK\");");
            script.Add("cmd.Composites.Clear();");
            for (int i = 0; i < cmd.Composites.Count; i++)
            {
                string compositeName = "COMP_" + cmd.Composites[i].shortGUID.ToByteString().Replace('-', '_');
                script.Add("Composite " + compositeName + " = cmd.AddComposite(@\"" + cmd.Composites[i].name + "\");");

                for (int x = 0; x < cmd.Composites[i].functions.Count; x++)
                {
                    string entityName = "ENT_" + cmd.Composites[i].functions[x].shortGUID.ToByteString().Replace('-', '_');
                    script.Add("FunctionEntity " + entityName + " = " + compositeName + ".AddFunction(");
                    if (CommandsUtils.FunctionTypeExists(cmd.Composites[i].functions[x].function)) script[script.Count - 1] += "FunctionType." + CommandsUtils.GetFunctionType(cmd.Composites[i].functions[x].function) + ");";
                    else script[script.Count - 1] += "@\"" + cmd.GetComposite(cmd.Composites[i].functions[x].function).name + "\");";

                    for (int y = 0; y < cmd.Composites[i].functions[x].resources.Count; y++)
                    {
                        string resourceName = "RES_" + cmd.Composites[i].functions[x].resources[y].GetHashCode().ToString().Replace('-', '_');
                        switch (cmd.Composites[i].functions[x].resources[y].entryType)
                        {
                            case ResourceType.RENDERABLE_INSTANCE:
                                script.Add("ResourceReference " + resourceName + " = " + entityName + ".AddResource(ResourceType." + cmd.Composites[i].functions[x].resources[y].entryType + ");");
                                Vector3 pos = cmd.Composites[i].functions[x].resources[y].position;
                                script.Add(resourceName + ".position = new Vector3(" + pos.X + "f, " + pos.Y + "f, " + pos.Z + "f);");
                                Vector3 rot = cmd.Composites[i].functions[x].resources[y].rotation;
                                script.Add(resourceName + ".rotation = new Vector3(" + rot.X + "f, " + rot.Y + "f, " + rot.Z + "f);");
                                script.Add(resourceName + ".startIndex = " + cmd.Composites[i].functions[x].resources[y].startIndex + ";");
                                script.Add(resourceName + ".count = " + cmd.Composites[i].functions[x].resources[y].count + ";");
                                break;
                            default:
                                throw new Exception("Unhandled resource");
                        }
                    }
                }
                for (int x = 0; x < cmd.Composites[i].variables.Count; x++)
                {
                    string entityName = "ENT_" + cmd.Composites[i].variables[x].shortGUID.ToByteString().Replace('-', '_');
                    script.Add("VariableEntity " + entityName + " = " + compositeName + ".AddVariable(\"" + ShortGuidUtils.FindString(cmd.Composites[i].variables[x].name) + "\", DataType." + cmd.Composites[i].variables[x].type.ToString() + ");");
                }
                for (int x = 0; x < cmd.Composites[i].proxies.Count; x++)
                {
                    throw new Exception("Unhandled proxy");
                }
                for (int x = 0; x < cmd.Composites[i].overrides.Count; x++)
                {
                    throw new Exception("Unhandled override");
                }

                List<Entity> entities = cmd.Composites[i].GetEntities();
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
                        string connectedEntityName = "ENT_" + entities[x].childLinks[y].childID.ToByteString().Replace('-', '_');
                        script.Add(entityName + ".AddParameterLink(\"" + ShortGuidUtils.FindString(entities[x].childLinks[y].parentParamID) + "\", " + connectedEntityName + ", \"" + ShortGuidUtils.FindString(entities[x].childLinks[y].childParamID) + "\");");
                    }
                }
            }
            script.Add("cmd.Save();");
            return script;
        }
    }
}
