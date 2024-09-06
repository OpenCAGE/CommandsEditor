using CATHODE.Scripting;
using CathodeLib;
using CommandsEditor.Nodes;
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
                        flowgraphMeta.Nodes = new List<FlowgraphMeta.NodeMeta>();
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

            for (int i = 0; i < editor.Nodes.Count; i++)
            {
                if (!(editor.Nodes[i] is CathodeNode))
                    continue;

                CathodeNode node = (CathodeNode)editor.Nodes[i];
                FlowgraphMeta.NodeMeta nodeMeta = flowgraphMeta.Nodes.FirstOrDefault(o => o.ID == node.ID);
                if (nodeMeta == null)
                    continue;

                node.SetPosition(nodeMeta.Position);
            }

            editor.ScaleCanvas(flowgraphMeta.CanvasScale, 0, 0);
            editor.MoveCanvas(flowgraphMeta.CanvasPosition.X, flowgraphMeta.CanvasPosition.Y, false, CanvasMoveArgs.All);

            //TODO: maybe console.writeline some info about number of nodes missing positions, or not included in the list when expected? implies user has modified the composite
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
            flowgraphMeta.Nodes = new List<FlowgraphMeta.NodeMeta>();
            flowgraphMeta.CanvasPosition = editor.CanvasOffset;
            flowgraphMeta.CanvasScale = editor.CanvasScale;

            for (int i = 0; i < editor.Nodes.Count; i++)
            {
                if (!(editor.Nodes[i] is CathodeNode))
                    continue;

                CathodeNode node = (CathodeNode)editor.Nodes[i];
                FlowgraphMeta.NodeMeta nodeMeta = new FlowgraphMeta.NodeMeta();
                nodeMeta.ID = node.ID;
                nodeMeta.Position = node.Location;
                flowgraphMeta.Nodes.Add(nodeMeta);
            }

            SaveFile();
        }

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
                }
                Console.WriteLine("NodePositionDatabase SAVED " + _flowgraphMetas.Count + " flowgraphs");
            }
        }

        private class FlowgraphMeta
        {
            public string Name;
            public List<NodeMeta> Nodes;

            public PointF CanvasPosition;
            public float CanvasScale;

            public class NodeMeta
            {
                public ShortGuid ID;
                public Point Position;
            }
        }
    }
}
