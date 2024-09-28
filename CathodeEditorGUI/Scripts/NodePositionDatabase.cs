using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsEditor
{
    //todo: if this ever makes it to production, will need to make sure this is done safely to not overwrite stuff with multiple instances. at the mo its fine for testing locally with single instances.
    public static class NodePositionDatabase
    {
        private static string _filepath = "node_positions.bin";
        private static List<FlowgraphMeta> _flowgraphMetas;

        static NodePositionDatabase()
        {
            _flowgraphMetas = new List<FlowgraphMeta>();

            if (File.Exists(_filepath))
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(_filepath)))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        FlowgraphMeta flowgraphMeta = new FlowgraphMeta();
                        flowgraphMeta.Name = reader.ReadString();
                        flowgraphMeta.CanvasPosition = new PointF(reader.ReadSingle(), reader.ReadSingle());
                        flowgraphMeta.CanvasScale = reader.ReadSingle();
                        reader.BaseStream.Position += 10; //reserved
                        int nodeMetaCount = reader.ReadInt32();
                        for (int x = 0; x < nodeMetaCount; x++)
                        {
                            FlowgraphMeta.NodeMeta nodeMeta = new FlowgraphMeta.NodeMeta();
                            nodeMeta.ID = Utilities.Consume<ShortGuid>(reader);
                            nodeMeta.Position = new Point(reader.ReadInt32(), reader.ReadInt32());
                            flowgraphMeta.Nodes.Add(nodeMeta);
                        }
                        int variableNodeMetaCount = reader.ReadInt32();
                        for (int x = 0; x < variableNodeMetaCount; x++)
                        {
                            FlowgraphMeta.VariableNodeMeta nodeMeta = new FlowgraphMeta.VariableNodeMeta();
                            nodeMeta.ID = Utilities.Consume<ShortGuid>(reader);
                            int instanceCount = reader.ReadInt32();
                            for (int y = 0; y < instanceCount; y++)
                            {
                                FlowgraphMeta.VariableNodeMeta.Instance instance = new FlowgraphMeta.VariableNodeMeta.Instance();
                                instance.Position = new Point(reader.ReadInt32(), reader.ReadInt32());
                                int inCount = reader.ReadInt32();
                                for (int z = 0; z < inCount; z++)
                                {
                                    FlowgraphMeta.VariableNodeMeta.Instance.Connection connection = new FlowgraphMeta.VariableNodeMeta.Instance.Connection();
                                    connection.EntityID = Utilities.Consume<ShortGuid>(reader);
                                    connection.ParameterID = Utilities.Consume<ShortGuid>(reader);
                                    instance.ConnectionsIn.Add(connection);
                                }
                                int outCount = reader.ReadInt32();
                                for (int z = 0; z < outCount; z++)
                                {
                                    FlowgraphMeta.VariableNodeMeta.Instance.Connection connection = new FlowgraphMeta.VariableNodeMeta.Instance.Connection();
                                    connection.EntityID = Utilities.Consume<ShortGuid>(reader);
                                    connection.ParameterID = Utilities.Consume<ShortGuid>(reader);
                                    instance.ConnectionsOut.Add(connection);
                                }
                                nodeMeta.Instances.Add(instance);
                            }
                            flowgraphMeta.VariableNodes.Add(nodeMeta);
                        }
                        _flowgraphMetas.Add(flowgraphMeta);
                    }
                    Console.WriteLine("NodePositionDatabase LOADED " + _flowgraphMetas.Count + " flowgraphs");
                }
            }
        }

        public static bool CanRestoreFlowgraph(string compositeName)
        {
            return _flowgraphMetas.FirstOrDefault(o => o.Name == compositeName) != null;
        }

        public static bool TryRestoreFlowgraph(string compositeName, STNodeEditor editor)
        {
            FlowgraphMeta flowgraphMeta = _flowgraphMetas.FirstOrDefault(o => o.Name == compositeName);
            if (flowgraphMeta == null)
                return false;

            List<STNode> nodesToAdd = new List<STNode>();
            for (int i = 0; i < editor.Nodes.Count; i++)
            {
                STNode node = editor.Nodes[i];
                FlowgraphMeta.NodeMeta nodeMeta = flowgraphMeta.Nodes.FirstOrDefault(o => o.ID == node.ShortGUID);
                if (nodeMeta != null)
                {
                    node.SetPosition(nodeMeta.Position);
                }
                FlowgraphMeta.VariableNodeMeta variableNodeMeta = flowgraphMeta.VariableNodes.FirstOrDefault(o => o.ID == node.ShortGUID);
                if (variableNodeMeta != null)
                {
                    int connectionsIn = node.GetInputOptions().Length;
                    int connectionsOut = node.GetOutputOptions().Length;

                    if (variableNodeMeta.Instances.Count > 1)
                    {
                        //note to self: there is currently a problem where it all freezes if you spawn two of the same variable entity. need to find out why.
                        // the whole implementation of it is kinda jank so would be good to re-evaluate. maybe just leave it and revisit after the editing support is working.
                        string gsdff = ";";
                    }

                    for (int x = 0; x < variableNodeMeta.Instances.Count; x++)
                    {
                        STNode instanceNode = node;
                        if (x > 0)
                        {
                            instanceNode = new STNode();
                            instanceNode.Entity = node.Entity;
                            instanceNode.SetName(node.Title, node.SubTitle);
                            instanceNode.SetColour(node.TitleColor, node.ForeColor);
                            instanceNode.AddInputOption(((VariableEntity)node.Entity).name);
                            instanceNode.AddOutputOption(((VariableEntity)node.Entity).name);
                            nodesToAdd.Add(instanceNode);
                        }
                        instanceNode.SetPosition(variableNodeMeta.Instances[x].Position);

                        //We want to disconnect this variable node instance from any connections that the instance doesn't draw between
                        STNodeOption[] ins = instanceNode.GetInputOptions();
                        int insCount = ins.Length;
                        for (int y = 0; y < ins.Length; y++)
                        {
                            List<STNodeOption> connections = ins[y].GetConnectedOption();
                            for (int z = 0; z < connections.Count; z++)
                            {
                                STNode connectedNode = connections[z].Owner;
                                var connectionMeta = variableNodeMeta.Instances[x].ConnectionsIn.FirstOrDefault(o => o.ParameterID == connections[z].ShortGUID && o.EntityID == connectedNode.ShortGUID);
                                if (connectionMeta == null)
                                {
                                    connections[z].DisconnectOption(ins[y]);
                                    insCount--;
                                    continue;
                                }
                            }
                        }
                        connectionsIn -= insCount;
                        STNodeOption[] outs = instanceNode.GetOutputOptions();
                        int outCount = outs.Length;
                        for (int y = 0; y < outs.Length; y++)
                        {
                            List<STNodeOption> connections = outs[y].GetConnectedOption();
                            for (int z = 0; z < connections.Count; z++)
                            {
                                STNode connectedNode = connections[z].Owner;
                                var connectionMeta = variableNodeMeta.Instances[x].ConnectionsOut.FirstOrDefault(o => o.ParameterID == connections[z].ShortGUID && o.EntityID == connectedNode.ShortGUID);
                                if (connectionMeta == null)
                                {
                                    connections[z].DisconnectOption(outs[y]);
                                    outCount--;
                                    continue;
                                }
                            }
                        }
                        connectionsOut -= outCount;

                        instanceNode.Recompute();
                    }

                    //there is a problem with multi nodes

                    if (connectionsIn > 0)
                    {
                        string sdfsdf = "";
                    }
                    if (connectionsOut > 0)
                    {
                        string fsdfdf = "";
                    }
                }
                editor.Nodes.AddRange(nodesToAdd.ToArray());
            }

            editor.ScaleCanvas(flowgraphMeta.CanvasScale, 0, 0);
            editor.MoveCanvas(flowgraphMeta.CanvasPosition.X, flowgraphMeta.CanvasPosition.Y, false, CanvasMoveArgs.All);

            //TODO: maybe console.writeline some info about number of nodes missing positions, or not included in the list when expected? implies user has modified the composite
            //todo: we should store modified node positions direct to the commands.pak
            return true;
        }

        public static void SaveFlowgraph(string compositeName, STNodeEditor editor)
        {
            FlowgraphMeta flowgraphMeta = _flowgraphMetas.FirstOrDefault(o => o.Name == compositeName);
            if (flowgraphMeta == null)
            {
                flowgraphMeta = new FlowgraphMeta();
                flowgraphMeta.Name = compositeName;
                _flowgraphMetas.Add(flowgraphMeta);
            }
            flowgraphMeta.CanvasPosition = editor.CanvasOffset;
            flowgraphMeta.CanvasScale = editor.CanvasScale;
            flowgraphMeta.Nodes.Clear();
            for (int i = 0; i < editor.Nodes.Count; i++)
            {
                STNode node = editor.Nodes[i];
                FlowgraphMeta.NodeMeta nodeMeta = new FlowgraphMeta.NodeMeta();
                nodeMeta.ID = node.ShortGUID;
                nodeMeta.Position = node.Location;
                flowgraphMeta.Nodes.Add(nodeMeta);
            }
            flowgraphMeta.VariableNodes.Clear();
            for (int i = 0; i < editor.Nodes.Count; i++)
            {
                STNode node = editor.Nodes[i];
                FlowgraphMeta.VariableNodeMeta variableNodeMeta = flowgraphMeta.VariableNodes.FirstOrDefault(o => o.ID == node.ShortGUID);
                if (variableNodeMeta == null)
                {
                    variableNodeMeta = new FlowgraphMeta.VariableNodeMeta();
                    variableNodeMeta.ID = node.ShortGUID;
                    flowgraphMeta.VariableNodes.Add(variableNodeMeta);
                }
                FlowgraphMeta.VariableNodeMeta.Instance instance = new FlowgraphMeta.VariableNodeMeta.Instance();
                instance.Position = node.Location;

                STNodeOption[] ins = node.GetInputOptions();
                for (int y = 0; y < ins.Length; y++)
                {
                    List<STNodeOption> connections = ins[y].GetConnectedOption();
                    for (int z = 0; z < connections.Count; z++)
                    {
                        STNode connectedNode = connections[z].Owner;
                        instance.ConnectionsIn.Add(new FlowgraphMeta.VariableNodeMeta.Instance.Connection()
                        {
                            ParameterID = connections[z].ShortGUID,
                            EntityID = connectedNode.ShortGUID
                        });
                    }
                }
                STNodeOption[] outs = node.GetOutputOptions();
                for (int y = 0; y < outs.Length; y++)
                {
                    List<STNodeOption> connections = outs[y].GetConnectedOption();
                    for (int z = 0; z < connections.Count; z++)
                    {
                        STNode connectedNode = connections[z].Owner;
                        instance.ConnectionsOut.Add(new FlowgraphMeta.VariableNodeMeta.Instance.Connection()
                        {
                            ParameterID = connections[z].ShortGUID,
                            EntityID = connectedNode.ShortGUID
                        });
                    }
                }

                variableNodeMeta.Instances.Add(instance);
            }
            SaveFile();
        }

#if DEBUG
        private static void UPDATE_TO_NEW_VARIABLE_STUFF(Composite composite)
        {
            _flowgraphMetas.RemoveAll(o => o.Name.Length > ("DisplayModel:").Length && o.Name.Substring(0, ("DisplayModel:").Length) == "DisplayModel:");

            for (int i = 0; i < _flowgraphMetas.Count; i++)
            {
                if (_flowgraphMetas[i].Name != composite.name)
                    continue;

                if (_flowgraphMetas[i].VariableNodes.Count != 0)
                    continue;

                List<FlowgraphMeta.NodeMeta> Nodes = new List<FlowgraphMeta.NodeMeta>();
                for (int x=  0; x < _flowgraphMetas[i].Nodes.Count; x++)
                {
                    Entity ent = composite.GetEntityByID(_flowgraphMetas[i].Nodes[x].ID);
                    if (ent.variant != EntityVariant.VARIABLE)
                    {
                        Nodes.Add(_flowgraphMetas[i].Nodes[x]);
                        continue;
                    }

                    VariableEntity varEnt = (VariableEntity)ent;
                    FlowgraphMeta.VariableNodeMeta node = new FlowgraphMeta.VariableNodeMeta();
                    node.ID = varEnt.shortGUID;
                    node.Instances.Add(new FlowgraphMeta.VariableNodeMeta.Instance()
                    {
                        Position = _flowgraphMetas[i].Nodes[x].Position
                    });

                    var parentLinks = varEnt.GetParentLinks(composite);
                    for (int y = 0; y < parentLinks.Count; y++)
                    {
                        node.Instances[0].ConnectionsIn.Add(new FlowgraphMeta.VariableNodeMeta.Instance.Connection()
                        {
                             EntityID = parentLinks[y].linkedEntityID,
                             ParameterID = parentLinks[y].linkedParamID
                        });
                    }
                    for (int y = 0; y < varEnt.childLinks.Count; y++)
                    {
                        node.Instances[0].ConnectionsOut.Add(new FlowgraphMeta.VariableNodeMeta.Instance.Connection()
                        {
                            EntityID = varEnt.childLinks[y].linkedEntityID,
                            ParameterID = varEnt.childLinks[y].linkedParamID
                        });
                    }

                    _flowgraphMetas[i].VariableNodes.Add(node);
                }
                _flowgraphMetas[i].Nodes = Nodes;
            }
        }

        public static void DO_UPDATE()
        {
            return;

            Console.WriteLine("Doing Update...");

            List<string> files = Directory.GetFiles("F:\\SteamLibrary\\steamapps\\common\\Alien Isolation\\data_orig\\ENV\\Production", "COMMANDS.PAK", SearchOption.AllDirectories).ToList<string>();
            foreach (string file in files)
            {
                Commands commands = new Commands(file);
                foreach (Composite comp in commands.Entries)
                {
                    CommandsUtils.PurgeDeadLinks(commands, comp, true);
                    UPDATE_TO_NEW_VARIABLE_STUFF(comp);
                }
            }
            SaveFile();
        }
#endif

        private static void SaveFile()
        {
            if (_flowgraphMetas == null)
                return;

            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(_filepath)))
            {
                writer.BaseStream.SetLength(0);
                writer.Write(_flowgraphMetas.Count);
                for (int i = 0; i < _flowgraphMetas.Count; i++)
                {
                    writer.Write(_flowgraphMetas[i].Name);
                    writer.Write(_flowgraphMetas[i].CanvasPosition.X);
                    writer.Write(_flowgraphMetas[i].CanvasPosition.Y);
                    writer.Write(_flowgraphMetas[i].CanvasScale);
                    writer.Write(new byte[10]); //reserved
                    writer.Write(_flowgraphMetas[i].Nodes.Count);
                    for (int x = 0; x < _flowgraphMetas[i].Nodes.Count; x++)
                    {
                        Utilities.Write<ShortGuid>(writer, _flowgraphMetas[i].Nodes[x].ID);
                        writer.Write(_flowgraphMetas[i].Nodes[x].Position.X);
                        writer.Write(_flowgraphMetas[i].Nodes[x].Position.Y);
                    }
                    writer.Write(_flowgraphMetas[i].VariableNodes.Count);
                    for (int x = 0; x < _flowgraphMetas[i].VariableNodes.Count; x++)
                    {
                        Utilities.Write<ShortGuid>(writer, _flowgraphMetas[i].VariableNodes[x].ID);
                        writer.Write(_flowgraphMetas[i].VariableNodes[x].Instances.Count);
                        for (int y = 0; y < _flowgraphMetas[i].VariableNodes[x].Instances.Count; y++)
                        {
                            writer.Write(_flowgraphMetas[i].VariableNodes[x].Instances[y].Position.X);
                            writer.Write(_flowgraphMetas[i].VariableNodes[x].Instances[y].Position.Y);
                            writer.Write(_flowgraphMetas[i].VariableNodes[x].Instances[y].ConnectionsIn.Count);
                            for (int z = 0; z < _flowgraphMetas[i].VariableNodes[x].Instances[y].ConnectionsIn.Count; z++)
                            {
                                Utilities.Write<ShortGuid>(writer, _flowgraphMetas[i].VariableNodes[x].Instances[y].ConnectionsIn[z].EntityID);
                                Utilities.Write<ShortGuid>(writer, _flowgraphMetas[i].VariableNodes[x].Instances[y].ConnectionsIn[z].ParameterID);
                            }
                            writer.Write(_flowgraphMetas[i].VariableNodes[x].Instances[y].ConnectionsOut.Count);
                            for (int z = 0; z < _flowgraphMetas[i].VariableNodes[x].Instances[y].ConnectionsOut.Count; z++)
                            {
                                Utilities.Write<ShortGuid>(writer, _flowgraphMetas[i].VariableNodes[x].Instances[y].ConnectionsOut[z].EntityID);
                                Utilities.Write<ShortGuid>(writer, _flowgraphMetas[i].VariableNodes[x].Instances[y].ConnectionsOut[z].ParameterID);
                            }
                        }
                    }
                }
                Console.WriteLine("NodePositionDatabase SAVED " + _flowgraphMetas.Count + " flowgraphs");
            }
        }

        private class FlowgraphMeta
        {
            public string Name;
            public List<NodeMeta> Nodes = new List<NodeMeta>();
            public List<VariableNodeMeta> VariableNodes = new List<VariableNodeMeta>();

            public PointF CanvasPosition;
            public float CanvasScale;

            public class NodeMeta
            {
                public ShortGuid ID;
                public Point Position;
            }

            //Variable nodes are handled diff to other nodes since can have multiple instances of one
            public class VariableNodeMeta
            {
                public ShortGuid ID;
                public List<Instance> Instances = new List<Instance>();

                public class Instance
                {
                    public Point Position;
                    public List<Connection> ConnectionsIn = new List<Connection>();
                    public List<Connection> ConnectionsOut = new List<Connection>();

                    public class Connection
                    {
                        public ShortGuid ParameterID;
                        public ShortGuid EntityID;
                    }
                }
            }
        }
    }
}
