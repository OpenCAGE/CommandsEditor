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

namespace CommandsEditor
{
    public static class LocalDebug
    {
        public static void TestAllCmds()
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
                        File.WriteAllText("out.json", JsonConvert.SerializeObject(anim, Formatting.Indented));

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
//#if DEBUG
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
//#endif
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
            //Models mdls = new Models("G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\BSP_TORRENS\\RENDERABLE\\LEVEL_MODELS.PAK");
            //mdls.Save();
            /*
            CathodeModels mdls_old = new CathodeModels(
                "G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\BSP_TORRENS\\RENDERABLE\\MODELS_LEVEL.BIN",
                "G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\BSP_TORRENS\\RENDERABLE\\LEVEL_MODELS.PAK");

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

            //Models mdls = new Models("G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\ENG_ALIEN_NEST\\RENDERABLE\\LEVEL_MODELS.PAK");
            //mdls.Save();
            ////Textures tex34 = new Textures("G:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\DATA\\ENV\\PRODUCTION\\BSP_LV426_PT01\\RENDERABLE\\LEVEL_TEXTURES.ALL.PAK");
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
                for (int i = 0; i < Editor.commands.Entries.Count; i++)
                {
                    for (int x = 0; x < Editor.commands.Entries[i].functions.Count; x++)
                    {
                        if (!CommandsUtils.FunctionTypeExists(Editor.commands.Entries[i].functions[x].function)) continue;
                        FunctionType type = CommandsUtils.GetFunctionType(Editor.commands.Entries[i].functions[x].function);
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


                                Console.WriteLine(Editor.commands.Entries[i].name + " -> " + Editor.commands.Entries[i].functions[x].shortGUID + " -> " + type);

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
                        switch (cmd.Entries[i].functions[x].resources[y].entryType)
                        {
                            case ResourceType.RENDERABLE_INSTANCE:
                                script.Add("ResourceReference " + resourceName + " = " + entityName + ".AddResource(ResourceType." + cmd.Entries[i].functions[x].resources[y].entryType + ");");
                                Vector3 pos = cmd.Entries[i].functions[x].resources[y].position;
                                script.Add(resourceName + ".position = new Vector3(" + pos.X + "f, " + pos.Y + "f, " + pos.Z + "f);");
                                Vector3 rot = cmd.Entries[i].functions[x].resources[y].rotation;
                                script.Add(resourceName + ".rotation = new Vector3(" + rot.X + "f, " + rot.Y + "f, " + rot.Z + "f);");
                                script.Add(resourceName + ".startIndex = " + cmd.Entries[i].functions[x].resources[y].startIndex + ";");
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
                for (int x = 0; x < cmd.Entries[i].overrides.Count; x++)
                {
                    throw new Exception("Unhandled override");
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
