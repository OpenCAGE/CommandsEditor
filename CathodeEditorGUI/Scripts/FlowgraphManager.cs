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
    public class FlowgraphManager
    {
        private static CompositeFlowgraphsTable _vanilla;
        private static CompositeFlowgraphsTable _custom;

        public static Commands LinkedCommands => _commands;
        private static Commands _commands;

        static FlowgraphManager()
        {
            _vanilla = new CompositeFlowgraphsTable();
            _custom = new CompositeFlowgraphsTable();

            //TODO: need to populate flowgraphs.bin by converting the content in NodePositionDatabase

            using (BinaryReader reader = new BinaryReader(new MemoryStream(Properties.Resources.flowgraphs)))
            {
                _vanilla.Read(reader);
            }
        }

        //util to see if there is at least one vanilla or custom defined flowgraph layout for the given composite
        public static bool HasDefinedLayout(Composite composite)
        {
            return _vanilla.flowgraphs.FirstOrDefault(o => o.CompositeGUID == composite.shortGUID) != null ||
                _custom.flowgraphs.FirstOrDefault(o => o.CompositeGUID == composite.shortGUID) != null;
        }

        //gets the first flowgraph layout metadata for the given composite - todo: eventually we want to support multiple flowgraphs per composite
        //Prioritises the "custom" table as custom user-defined layouts should always overrule the vanilla ones
        public static FlowgraphMeta GetLayout(Composite composite)
        {
            FlowgraphMeta toReturn = _custom.flowgraphs.FirstOrDefault(o => o.CompositeGUID == composite.shortGUID);
            if (toReturn != null)
                return toReturn;

            return _vanilla.flowgraphs.FirstOrDefault(o => o.CompositeGUID == composite.shortGUID);
        }

#if DEBUG
        //This is for populating the vanilla flowgraph layout table locally - it's for development use only, so should not be included in non-debug builds
        public static void AddVanillaFlowgraph(STNodeEditor editor, Composite composite)
        {
            FlowgraphMeta flowgraphMeta = editor.AsFlowgraphMeta(composite, Path.GetFileName(composite.name)); //TODO: when we start supporting multiple flowgraphs we should pass new names in here
            _vanilla.flowgraphs.RemoveAll(o => o.Name == flowgraphMeta.Name && o.CompositeGUID == flowgraphMeta.CompositeGUID);
            _vanilla.flowgraphs.Add(flowgraphMeta);

            string vanillaFlowgraphDBPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            vanillaFlowgraphDBPath = vanillaFlowgraphDBPath.Substring(0, vanillaFlowgraphDBPath.Length - Path.GetFileName(vanillaFlowgraphDBPath).Length);
            vanillaFlowgraphDBPath += "../CathodeEditorGUI/Resources/flowgraphs.bin";
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(vanillaFlowgraphDBPath)))
                _vanilla.Write(writer);
        }
#endif

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
            _custom = (CompositeFlowgraphsTable)CustomTable.ReadTable(filepath, CustomEndTables.COMPOSITE_FLOWGRAPHS);
            if (_custom == null) _custom = new CompositeFlowgraphsTable();
            Console.WriteLine("Loaded " + _custom.flowgraphs.Count + " custom flowgraph layouts!");
        }

        private static void SaveCustomFlowgraphs(string filepath)
        {
            CustomTable.WriteTable(filepath, CustomEndTables.COMPOSITE_FLOWGRAPHS, _custom);
            Console.WriteLine("Saved " + _custom.flowgraphs.Count + " custom flowgraph layouts!");
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

            flowgraphMeta.CanvasPosition = editor.CanvasOffset;
            flowgraphMeta.CanvasScale = editor.CanvasScale;
            flowgraphMeta.Nodes = new List<FlowgraphMeta.NodeMeta>();
            for (int i = 0; i < editor.Nodes.Count; i++)
            {
                if (!(editor.Nodes[i] is CathodeNode))
                    continue;

                CathodeNode node = (CathodeNode)editor.Nodes[i];
                FlowgraphMeta.NodeMeta nodeMeta = new FlowgraphMeta.NodeMeta();
                nodeMeta.EntityGUID = node.ShortGUID;
                nodeMeta.Position = node.Location;

                STNodeOption[] ins = node.GetInputOptions();
                for (int y = 0; y < ins.Length; y++)
                {
                    List<STNodeOption> connections = ins[y].GetConnectedOption();
                    for (int z = 0; z < connections.Count; z++)
                    {
                        if (!(connections[z].Owner is CathodeNode))
                            continue;

                        CathodeNode connectedNode = (CathodeNode)connections[z].Owner;
                        nodeMeta.ConnectionsIn.Add(new FlowgraphMeta.NodeMeta.ConnectionMeta()
                        {
                            ParameterGUID = ins[y].ShortGUID,
                            ConnectedEntityGUID = connectedNode.ShortGUID,
                            ConnectedParameterGUID = connections[z].ShortGUID,
                        });
                    }
                }
                STNodeOption[] outs = node.GetOutputOptions();
                for (int y = 0; y < outs.Length; y++)
                {
                    List<STNodeOption> connections = outs[y].GetConnectedOption();
                    for (int z = 0; z < connections.Count; z++)
                    {
                        if (!(connections[z].Owner is CathodeNode))
                            continue;

                        CathodeNode connectedNode = (CathodeNode)connections[z].Owner;
                        nodeMeta.ConnectionsOut.Add(new FlowgraphMeta.NodeMeta.ConnectionMeta()
                        {
                            ParameterGUID = outs[y].ShortGUID,
                            ConnectedEntityGUID = connectedNode.ShortGUID,
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
