//#define DO_PRE_FLIGHT_CHECKS
// ^ Enable this define to sanity check the vanilla node DB for any dodgy entries.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using ST.Library.UI.NodeEditor;
using static CathodeLib.CompositeFlowgraphsTable;

namespace CommandsEditor
{
    //Handles loading vanilla/custom flowgraph layouts, and saving custom layouts
    public class FlowgraphLayoutManager
    {
        private static CompositeFlowgraphsTable _preDefinedLayouts;
        private static CompositeFlowgraphsTable _userDefinedLayouts;

        private static CompositeFlowgraphCompatibilityTable _compatibility;

        //TODO: remove this once i'm done populating the layout database!!
        public static bool DEBUG_IsUnfinished = false;
        public static bool DEBUG_UsePreDefinedTable = false;
        private static CompositeFlowgraphsTable Table
        {
            get
            {
#if DEBUG
                if (DEBUG_UsePreDefinedTable)
                {
                    return _preDefinedLayouts;
                }
#endif
                return _userDefinedLayouts;
            }
        }

        public static Commands LinkedCommands => _commands;
        private static Commands _commands;

        static FlowgraphLayoutManager()
        {
            _preDefinedLayouts = new CompositeFlowgraphsTable();
            _userDefinedLayouts = new CompositeFlowgraphsTable();

            using (BinaryReader reader = new BinaryReader(new MemoryStream(Properties.Resources.flowgraphs)))
            {
                _preDefinedLayouts.Read(reader);
            }
#if DEBUG && DO_PRE_FLIGHT_CHECKS
            //For sanity: make sure the vanilla db doesn't contain any empty flowgraphs
            List<FlowgraphMeta> trimmed = new List<FlowgraphMeta>();
            for (int i = 0; i < _preDefinedLayouts.flowgraphs.Count; i++)
            {
                if (_preDefinedLayouts.flowgraphs[i].Nodes.Count != 0)
                    trimmed.Add(_preDefinedLayouts.flowgraphs[i]);
            }
            List<FlowgraphMeta> trimmed2 = new List<FlowgraphMeta>();
            for (int i = 0; i < trimmed.Count; i++)
            {
                int connections = 0;
                for (int x = 0; x < trimmed[i].Nodes.Count; x++)
                {
                    connections += trimmed[i].Nodes[x].Connections.Count;
                }
                if (connections != 0)
                    trimmed2.Add(trimmed[i]);
            }
            //TODO: should run through all the ones flagged as pre node title shortening and make sure they look ok
            Console.WriteLine("FlowgraphLayoutManager found " + (_preDefinedLayouts.flowgraphs.Count - trimmed2.Count) + " invalid predefined flowgraph definitions");
            _preDefinedLayouts.flowgraphs = trimmed2;
            SaveVanillaDB();
#endif

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

        //Checks to see if the given flowgraph is compatible with the Flowgraph system: Composites saved with earlier versions of OpenCAGE are unsupported.
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
            return Table.flowgraphs.FirstOrDefault(o => o.CompositeGUID == composite.shortGUID && !o.IsUnfinished) != null;
        }

        //Gets all flowgraph layouts for the given composite
        public static List<FlowgraphMeta> GetLayouts(Composite composite)
        {
            return Table.flowgraphs.FindAll(o => o.CompositeGUID == composite.shortGUID);
        }

        //Save/add layout to db
        public static FlowgraphMeta SaveLayout(STNodeEditor editor, Composite composite, string name) //NOTE: passing no editor here will produce an empty layout, which could be destructive!
        {
            FlowgraphMeta flowgraphMeta = editor == null ? new FlowgraphMeta() { Name = name, CompositeGUID = composite.shortGUID } : editor.AsFlowgraphMeta(composite, name);
            FlowgraphMeta existingFGM = Table.flowgraphs.FirstOrDefault(o => o.Name == flowgraphMeta.Name && o.CompositeGUID == flowgraphMeta.CompositeGUID);
            if (existingFGM != null)
                Table.flowgraphs[Table.flowgraphs.IndexOf(existingFGM)] = flowgraphMeta;
            else
                Table.flowgraphs.Add(flowgraphMeta);

#if DEBUG
            SaveVanillaDB();
#endif

            return flowgraphMeta;
        }
#if DEBUG
        private static void SaveVanillaDB()
        {
            string vanillaFlowgraphDBPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            vanillaFlowgraphDBPath = vanillaFlowgraphDBPath.Substring(0, vanillaFlowgraphDBPath.Length - Path.GetFileName(vanillaFlowgraphDBPath).Length);
            vanillaFlowgraphDBPath += "../CathodeEditorGUI/Resources/flowgraphs.bin";
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(vanillaFlowgraphDBPath)))
            {
                writer.BaseStream.SetLength(0);
                _preDefinedLayouts.Write(writer);
                writer.Close();
            }
        }
#endif

        //Remove a layout from the DB
        public static void RemoveLayout(Composite composite, string name)
        {
            Table.flowgraphs.RemoveAll(o => o.CompositeGUID == composite.shortGUID && o.Name == name);
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
            _userDefinedLayouts = (CompositeFlowgraphsTable)CustomTable.ReadTable(filepath, CustomEndTables.COMPOSITE_FLOWGRAPHS);
            if (_userDefinedLayouts == null) _userDefinedLayouts = new CompositeFlowgraphsTable();
            Console.WriteLine("Loaded " + _userDefinedLayouts.flowgraphs.Count + " custom flowgraph layouts!");
            
            _compatibility = (CompositeFlowgraphCompatibilityTable)CustomTable.ReadTable(filepath, CustomEndTables.COMPOSITE_FLOWGRAPH_COMPATIBILITY_INFO);
            if (_compatibility == null) _compatibility = new CompositeFlowgraphCompatibilityTable();
            Console.WriteLine("Loaded " + _compatibility.compatibility_info.Count + " flowgraph compatibility definitions!");

            //If there are no defined layouts, then this Commands is being opened for the first time since the flowgraph update: handle compatibility checks
            if (_userDefinedLayouts.flowgraphs.Count == 0)
            {
                //Copy the default layouts over
                for (int i = 0; i < _preDefinedLayouts.flowgraphs.Count; i++)
                    _userDefinedLayouts.flowgraphs.Add(_preDefinedLayouts.flowgraphs[i].Copy());

                //Run through every composite and purge CAGE's lingering links (do it twice to be sure)
                for (int i = 0; i < 2; i++)
                    CommandsUtils.PurgeDeadLinks(_commands, _commands.Entries[i]);

                for (int i = 0; i < _commands.Entries.Count; i++)
                {
                    CommandsUtils.PurgedComposites.purged.Add(_commands.Entries[i].shortGUID);

                    var compatibilityInfo = _compatibility.compatibility_info.FirstOrDefault(o => o.composite_id == _commands.Entries[i].shortGUID);
                    if (compatibilityInfo == null)
                    {
                        compatibilityInfo = new CompositeFlowgraphCompatibilityTable.CompatibilityInfo() { composite_id = _commands.Entries[i].shortGUID };
                        _compatibility.compatibility_info.Add(compatibilityInfo);
                    }

                    //Clear out any aliases with no parameters/links
                    List<AliasEntity> aliasPurged = new List<AliasEntity>();
                    for (int x = 0; x < _commands.Entries[i].aliases.Count; x++)
                    {
                        if (_commands.Entries[i].aliases[x].childLinks.Count == 0 &&
                            _commands.Entries[i].aliases[x].parameters.Count == 0 &&
                            _commands.Entries[i].aliases[x].GetParentLinks(_commands.Entries[i]).Count == 0)
                            continue;
                        aliasPurged.Add(_commands.Entries[i].aliases[x]);
                    }
                    _commands.Entries[i].aliases = aliasPurged;

#if DEBUG
                    compatibilityInfo.flowgraphs_supported = true;
                    continue;
#endif

                    int links = CompositeUtils.CountLinks(_commands.Entries[i]);
                    if (links == 0)
                    {
                        //If the composite has no links, regardless of its vanilla-ness, allow it
                        _userDefinedLayouts.flowgraphs.RemoveAll(o => o.CompositeGUID == _commands.Entries[i].shortGUID);
                        SaveLayout(null, _commands.Entries[i], Path.GetFileName(_commands.Entries[i].name));
                        compatibilityInfo.flowgraphs_supported = true;
                    }
                    else
                    {
                        //If there are links, make sure they match up with the stored layout

                        //TODO: need to validate that the content of the composite hasn't diverged from the layout
                        compatibilityInfo.flowgraphs_supported = HasLayout(_commands.Entries[i]);

                        //if they don't - do something about it 
                    }
                }
            }
        }

        private static void SaveCustomFlowgraphs(string filepath)
        {
            CustomTable.WriteTable(filepath, CustomEndTables.COMPOSITE_FLOWGRAPHS, _userDefinedLayouts);
            Console.WriteLine("Saved " + _userDefinedLayouts.flowgraphs.Count + " custom flowgraph layouts!");

            CustomTable.WriteTable(filepath, CustomEndTables.COMPOSITE_FLOWGRAPH_COMPATIBILITY_INFO, _compatibility);
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
            flowgraphMeta.IsUnfinished = FlowgraphLayoutManager.DEBUG_IsUnfinished;

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
    }
}
