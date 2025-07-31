//#define DO_PRE_FLIGHT_CHECKS
// ^ Enable this define to sanity check the vanilla node DB for any dodgy entries.

using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using Newtonsoft.Json;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CathodeLib.CompositeFlowgraphTable;

namespace CommandsEditor
{
    //NOTE: There seem to be instances where this is not saving: jumping into child composites, and enabling/disabling GUIDs

    //Handles loading vanilla/custom flowgraph layouts, and saving custom layouts
    public class FlowgraphLayoutManager
    {
        private static CompositeFlowgraphTable _preDefinedLayouts = new CompositeFlowgraphTable();
        private static CompositeFlowgraphTable _userDefinedLayouts = new CompositeFlowgraphTable();
        private static CompositeFlowgraphCompatibilityTable _compatibility = new CompositeFlowgraphCompatibilityTable();

        public static Commands LinkedCommands => _commands;
        private static Commands _commands;

        static FlowgraphLayoutManager()
        {
            byte[] contentCompressed = Properties.Resources.flowgraphs;
            if (File.Exists("LocalDB\\flowgraphs.dat"))
                contentCompressed = File.ReadAllBytes("LocalDB\\flowgraphs.dat");
            byte[] content = null;

            using (MemoryStream stream = new MemoryStream())
            using (GZipStream compressedStream = new GZipStream(new MemoryStream(contentCompressed), CompressionMode.Decompress))
            {
                compressedStream.CopyTo(stream);
                content = stream.ToArray();
            }
            _preDefinedLayouts = (CompositeFlowgraphTable)CustomTable.ReadTable(content, CustomTableType.COMPOSITE_FLOWGRAPHS);

            //Always add new composites into the compatibility table
            Singleton.OnCompositeAdded += AddToCompatibilityTable;
        }
        private static void AddToCompatibilityTable(Composite composite)
        {
            SaveLayout(null, composite, Path.GetFileName(composite.name)); //Add in a default empty flowgraph
            _compatibility.compatibility_info.Add(new CompositeFlowgraphCompatibilityTable.CompatibilityInfo()
            {
                composite_id = composite.shortGUID,
                flowgraphs_supported = true
            });
        }

        //Sets if the given composite supports flowgraphs: a composite wouldn't support flowgraphs if it diverges from the saved layout, or has no layout defined
        private static void SetCompatibilityInfo(Composite composite, bool flowgraphs_supported)
        {
            var compatibilityInfo = _compatibility.compatibility_info.FirstOrDefault(o => o.composite_id == composite.shortGUID);
            if (compatibilityInfo == null)
            {
                compatibilityInfo = new CompositeFlowgraphCompatibilityTable.CompatibilityInfo() { composite_id = composite.shortGUID };
                _compatibility.compatibility_info.Add(compatibilityInfo);
            }
            compatibilityInfo.flowgraphs_supported = flowgraphs_supported;
        }

        //Checks to see if a composite has associated flowgraph compatibility info: a composite wouldn't have this if it is being opened for the first time since script editor v3
        public static bool HasCompatibilityInfo(Composite composite)
        {
            return _compatibility.compatibility_info.FirstOrDefault(o => o.composite_id == composite.shortGUID) != null;
        }

        //Checks the given composite against the layout DB to see if the links/entities match
        public static void EvaluateCompatibility(Composite composite)
        {
            int links = _commands.Utils.CountLinks(composite);
            if (links == 0)
            {
                //If the composite has no links, regardless of if it diverges from the saved layouts, allow it
                RemoveAllLayouts(composite);
                SaveLayout(null, composite, Path.GetFileName(composite.name));
                SetCompatibilityInfo(composite, true);
            }
            else
            {
                //If there are links, make sure they match up with the stored layout (if there is one)
                bool hasLayout = HasLayout(composite);
                if (hasLayout)
                {
                    SetCompatibilityInfo(composite, GetLayouts(composite).LinksMatch(composite));
                }
                else
                {
                    //NOTE: No longer writing compatibility info for Composites with no layouts defined, as it means it'll work nicer with future updates.
                    //Console.WriteLine("Flowgraphs are not supported as no layout has been defined yet!");
                    //SetCompatibilityInfo(composite, false);
                }
            }
        }

        //Checks to see if the given flowgraph is compatible with the Flowgraph system (make sure this has been evaluated first using the method above)
        public static bool IsCompatible(Composite composite)
        {
            if (composite == null)
                return false;

            var info = _compatibility.compatibility_info.FirstOrDefault(o => o.composite_id == composite.shortGUID);
            if (info == null)
                return false;
            return info.flowgraphs_supported;
        }

        //Checks to see if there is at least one finished flowgraph for the given composite
        public static bool HasLayout(Composite composite)
        {
            return _userDefinedLayouts.flowgraphs.FirstOrDefault(o => o.CompositeGUID == composite.shortGUID && !o.IsUnfinished) != null;
        }

        //Gets all flowgraph layouts for the given composite
        public static List<FlowgraphMeta> GetLayouts(Composite composite)
        {
            return _userDefinedLayouts.flowgraphs.FindAll(o => o.CompositeGUID == composite.shortGUID);
        }

        //Save/add layout to db
        public static FlowgraphMeta SaveLayout(STNodeEditor editor, Composite composite, string name) //NOTE: passing no editor here will produce an empty layout, which could be destructive!
        {
            FlowgraphMeta flowgraphMeta = editor == null ? new FlowgraphMeta() { Name = name, CompositeGUID = composite.shortGUID } : editor.AsFlowgraphMeta(composite, name);
            FlowgraphMeta existingFGM = _userDefinedLayouts.flowgraphs.FirstOrDefault(o => o.Name == flowgraphMeta.Name && o.CompositeGUID == flowgraphMeta.CompositeGUID);
            if (existingFGM != null)
                _userDefinedLayouts.flowgraphs[_userDefinedLayouts.flowgraphs.IndexOf(existingFGM)] = flowgraphMeta;
            else
                _userDefinedLayouts.flowgraphs.Add(flowgraphMeta);
            return flowgraphMeta;
        }

        //Remove a layout from the DB
        public static void RemoveLayout(Composite composite, string name)
        {
            _userDefinedLayouts.flowgraphs.RemoveAll(o => o.CompositeGUID == composite.shortGUID && o.Name == name);
        }
        public static void RemoveAllLayouts(Composite composite)
        {
            _userDefinedLayouts.flowgraphs.RemoveAll(o => o.CompositeGUID == composite.shortGUID);
        }

        public static void LinkCommands(Commands commands)
        {
            if (_commands != null)
            {
                _commands.OnLoadSuccess -= LoadCustomFlowgraphs;
                _commands.OnSaveSuccess -= SaveCustomFlowgraphs;
            }

            _commands = commands;
            if (_commands == null) return;

            _commands.OnLoadSuccess += LoadCustomFlowgraphs;
            _commands.OnSaveSuccess += SaveCustomFlowgraphs;

            LoadCustomFlowgraphs(_commands.Filepath);
        }

        private static void LoadCustomFlowgraphs(string filepath)
        {
            _userDefinedLayouts = (CompositeFlowgraphTable)CustomTable.ReadTable(filepath, CustomTableType.COMPOSITE_FLOWGRAPHS);
            if (_userDefinedLayouts == null) _userDefinedLayouts = new CompositeFlowgraphTable();
            Console.WriteLine("Loaded " + _userDefinedLayouts.flowgraphs.Count + " custom flowgraph layouts!");
            
            _compatibility = (CompositeFlowgraphCompatibilityTable)CustomTable.ReadTable(filepath, CustomTableType.COMPOSITE_FLOWGRAPH_COMPATIBILITY_INFO);
            if (_compatibility == null) _compatibility = new CompositeFlowgraphCompatibilityTable();
            Console.WriteLine("Loaded " + _compatibility.compatibility_info.Count + " flowgraph compatibility definitions!");

            //Copy the default layouts over for composites in this Commands if they don't already exist
            FlowgraphMeta.SupportedLevel level = (FlowgraphMeta.SupportedLevel)Enum.Parse(typeof(FlowgraphMeta.SupportedLevel), Path.GetFileName(_commands.EntryPoints[0].name).ToUpper()); //TODO: should really have a global level name based on what's loading, rather than this.
            if (_userDefinedLayouts.flowgraphs.Count == 0)
            {
                for (int i = 0; i < _preDefinedLayouts.flowgraphs.Count; i++)
                {
                    if (!_preDefinedLayouts.flowgraphs[i].SupportedLevels.HasFlag(level))
                        continue;
                    if (_commands.Entries.FirstOrDefault(o => o.shortGUID == _preDefinedLayouts.flowgraphs[i].CompositeGUID) == null)
                        continue;
                    _userDefinedLayouts.flowgraphs.Add(_preDefinedLayouts.flowgraphs[i].Copy());
                }
                Console.WriteLine("Applied " + _userDefinedLayouts.flowgraphs.Count + " suitable flowgraph layouts, of the " + _preDefinedLayouts.flowgraphs.Count + " available!");
            }
        }

        private static void SaveCustomFlowgraphs(string filepath)
        {
            CustomTable.WriteTable(filepath, CustomTableType.COMPOSITE_FLOWGRAPHS, _userDefinedLayouts);
            Console.WriteLine("Saved " + _userDefinedLayouts.flowgraphs.Count + " custom flowgraph layouts!");

            CustomTable.WriteTable(filepath, CustomTableType.COMPOSITE_FLOWGRAPH_COMPATIBILITY_INFO, _compatibility);
            Console.WriteLine("Saved " + _compatibility.compatibility_info.Count + " flowgraph compatibility definitions!");
        }
    }

    public static class FlowgraphManagerUtils
    {
        /* Convert a STNodeEditor graph to a FlowgraphMeta object for saving */
        public static FlowgraphMeta AsFlowgraphMeta(this STNodeEditor editor, Composite composite, string name)
        {
            // default name = Path.GetFileName(composite.name)

            FlowgraphMeta flowgraphMeta = new FlowgraphMeta();
            flowgraphMeta.CompositeGUID = composite.shortGUID;
            flowgraphMeta.Name = name;

            flowgraphMeta.UsesShortenedNames = true;
            flowgraphMeta.IsUnfinished = false; //now unused

            flowgraphMeta.CanvasPosition = editor.CanvasOffset;
            flowgraphMeta.CanvasScale = editor.CanvasScale;
            flowgraphMeta.Nodes = new List<FlowgraphMeta.NodeMeta>();
            for (int i = 0; i < editor.Nodes.Count; i++)
            {
                STNode node = editor.Nodes[i];
                FlowgraphMeta.NodeMeta nodeMeta = new FlowgraphMeta.NodeMeta();
                nodeMeta.EntityGUID = node.ShortGUID;
                nodeMeta.NodeID = i;

                nodeMeta.Position = node.Location;

                STNodeOption[] ins = node.GetInputOptions();
                STNodeOption[] outs = node.GetOutputOptions();

                for (int y = 0; y < ins.Length; y++)
                    nodeMeta.PinsIn.Add(ins[y].ShortGUID);
                for (int y = 0; y < outs.Length; y++)
                    nodeMeta.PinsOut.Add(outs[y].ShortGUID);

                for (int y = 0; y < outs.Length; y++)
                {
                    List<STNodeOption> connections = outs[y].GetConnectedOption();
                    for (int z = 0; z < connections.Count; z++)
                    {
                        STNode connectedNode = connections[z].Owner;
                        nodeMeta.Connections.Add(new FlowgraphMeta.NodeMeta.ConnectionMeta()
                        {
                            ParameterGUID = outs[y].ShortGUID,
                            ConnectedEntityGUID = connectedNode.ShortGUID,
                            ConnectedNodeID = editor.Nodes.IndexOf(connectedNode),
                            ConnectedParameterGUID = connections[z].ShortGUID,
                        });
                    }
                }

                flowgraphMeta.Nodes.Add(nodeMeta);
            }

            return flowgraphMeta;
        }

        /* Check a Composite against a set of FlowgraphMetas to see if the links are the same */
        public static bool LinksMatch(this List<FlowgraphMeta> metas, Composite composite)
        {
            List<LinkData> flowgraphLinks = new List<LinkData>();
            for (int i = 0; i < metas.Count; i++)
            {
                for (int x = 0; x < metas[i].Nodes.Count; x++)
                {
                    for (int y = 0; y < metas[i].Nodes[x].Connections.Count; y++)
                    {
                        flowgraphLinks.Add(new LinkData(
                            metas[i].Nodes[x].EntityGUID,
                            metas[i].Nodes[x].Connections[y].ParameterGUID,
                            metas[i].Nodes[x].Connections[y].ConnectedEntityGUID,
                            metas[i].Nodes[x].Connections[y].ConnectedParameterGUID)
                        );
                    }
                }
            }

            List<Entity> entities = composite.GetEntities();
            List<LinkData> compositeLinks = new List<LinkData>();
            for (int i = 0; i < entities.Count; i++)
            {
                //Ignore any links that point to missing entities!
                List<EntityConnector> trimmedChildren = new List<EntityConnector>();
                foreach (EntityConnector connector in entities[i].childLinks)
                {
                    if (composite.GetEntityByID(connector.linkedEntityID) == null)
                        continue;
                    trimmedChildren.Add(connector);
                }

                for (int x = 0; x < trimmedChildren.Count; x++)
                {
                    compositeLinks.Add(new LinkData(
                        entities[i].shortGUID,
                        trimmedChildren[x].thisParamID,
                        trimmedChildren[x].linkedEntityID,
                        trimmedChildren[x].linkedParamID)
                    );
                }
            }

            //Do we have the same number of links?
            if (flowgraphLinks.Count != compositeLinks.Count)
            {
                return false;
            }

            //Now we know we have the same number of links, do the links match? 
            flowgraphLinks = flowgraphLinks.OrderBy(o => o.In.ParameterID.ToString()).ThenBy(o => o.Out.ParameterID.ToString()).ThenBy(o => o.In.EntityID.ToByteString()).ThenBy(o => o.Out.EntityID.ToByteString()).ToList();
            compositeLinks = compositeLinks.OrderBy(o => o.In.ParameterID.ToString()).ThenBy(o => o.Out.ParameterID.ToString()).ThenBy(o => o.In.EntityID.ToByteString()).ThenBy(o => o.Out.EntityID.ToByteString()).ToList();
            for (int i = 0; i < flowgraphLinks.Count; i++)
            {
                if (flowgraphLinks[i] != compositeLinks[i])
                {
#if DEBUG
                    // If in debug mode, output both lists of links so I can easily diff them if needed.
                    string dirName = "FGLayoutCheck/" + Path.GetFileName(composite.name.Replace(":", "_"));
                    Directory.CreateDirectory(dirName);
                    File.WriteAllText(dirName + "/FLOWGRAPH LINKS.json", JsonConvert.SerializeObject(flowgraphLinks, Newtonsoft.Json.Formatting.Indented, new ShortGuidConverter()));
                    File.WriteAllText(dirName + "/COMPOSITE LINKS.json", JsonConvert.SerializeObject(compositeLinks, Newtonsoft.Json.Formatting.Indented, new ShortGuidConverter()));
#endif
                    return false;
                }
            }

            //Finally, double check that there aren't any entities missing from the composite.
            for (int i = 0; i < metas.Count; i++)
            {
                for (int x = 0; x < metas[i].Nodes.Count; x++)
                {
                    if (composite.GetEntityByID(metas[i].Nodes[x].EntityGUID) == null)
                    {
                        //If one is missing, check to see if it has any links in/out -> if it doesn't, it's fine, we're not losing anything important.
                        //Just be aware, the Flowgraph UI will need to be able to handle null entities safely.

                        if (metas[i].Nodes[x].Connections.Count != 0)
                            return false;

                        //This may seem like a ridiculous level of loops, but really, we should RARELY (or ideally never) get here. 
                        for (int p = 0; p < metas.Count; p++)
                        {
                            for (int c = 0; c < metas[p].Nodes.Count; c++)
                            {
                                for (int y = 0; y < metas[p].Nodes[c].Connections.Count; y++)
                                {
                                    if (metas[p].Nodes[c].Connections[y].ConnectedEntityGUID == metas[i].Nodes[x].EntityGUID)
                                        return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        struct LinkData
        {
            public LinkData(ShortGuid EntityID, ShortGuid ParameterID, ShortGuid LinkedEntityID, ShortGuid LinkedParameterID)
            {
                Out = new Parameter() { EntityID = EntityID, ParameterID = ParameterID };
                In = new Parameter() { EntityID = LinkedEntityID, ParameterID = LinkedParameterID };
            }

            public Parameter Out;
            public Parameter In;

            public struct Parameter
            {
                public ShortGuid EntityID;
                public ShortGuid ParameterID;

                public static bool operator ==(Parameter left, Parameter right)
                {
                    return left.Equals(right);
                }

                public static bool operator !=(Parameter left, Parameter right)
                {
                    return !(left == right);
                }

                public override bool Equals(object obj)
                {
                    if (obj is Parameter other)
                    {
                        return EntityID.Equals(other.EntityID) && ParameterID.Equals(other.ParameterID);
                    }
                    return false;
                }

                public override int GetHashCode()
                {
                    int hashCode = -1506387652;
                    hashCode = hashCode * -1521134295 + EntityID.GetHashCode();
                    hashCode = hashCode * -1521134295 + ParameterID.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(LinkData left, LinkData right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(LinkData left, LinkData right)
            {
                return !(left == right);
            }

            public override bool Equals(object obj)
            {
                if (obj is LinkData other)
                {
                    return Out == other.Out && In == other.In;
                }
                return false;
            }

            public override int GetHashCode()
            {
                int hashCode = 1047395477;
                hashCode = hashCode * -1521134295 + Out.GetHashCode();
                hashCode = hashCode * -1521134295 + In.GetHashCode();
                return hashCode;
            }
        }
    }
}
