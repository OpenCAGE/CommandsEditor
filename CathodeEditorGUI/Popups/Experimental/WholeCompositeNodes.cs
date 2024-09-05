using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ST.Library.UI.NodeEditor;
using CATHODE.Scripting.Internal;
using CATHODE.Scripting;
using CommandsEditor.UserControls;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CommandsEditor.Popups.Base;
using WebSocketSharp;
using System.Security.Cryptography;
using CommandsEditor.Nodes;
using CommandsEditor.DockPanels;
using OpenCAGE;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using static System.Windows.Forms.LinkLabel;
using System.Xml.Linq;
using CathodeLib;

namespace CommandsEditor
{
    public partial class WholeCompositeNodes : DockContent
    {
        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        private Composite _composite;
        private List<CustomNode> _nodes = new List<CustomNode>();

        private NodePositionDatabase _positionDB;

        //note something weird on systemic processing composit

        public WholeCompositeNodes()
        {
            InitializeComponent();
            _positionDB = new NodePositionDatabase();
        }

        public void ShowComposite(Composite composite)
        {
            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _nodes.Clear();

            _composite = composite;
            List<Entity> entities = _composite.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                CustomNode mainNode = EntityToNode(entities[i], _composite);

                for (int x = 0;x < entities[i].childLinks.Count; x++)
                {
                    Entity childEnt = composite.GetEntityByID(entities[i].childLinks[x].linkedEntityID);
                    if (childEnt == null)
                        continue;

                    CustomNode childNode = EntityToNode(childEnt, _composite);
                    STNodeOption linkIn = childNode.AddInputOption(entities[i].childLinks[x].linkedParamID.ToString());
                    STNodeOption linkOut = mainNode.AddOutputOption(entities[i].childLinks[x].thisParamID.ToString());
                    linkIn.ConnectOption(linkOut);
                }
            }

            this.Text = _composite.name;
            _positionDB.TryRestoreFlowgraph(_composite.name, stNodeEditor1);

            //Lock options for now
            foreach (STNode node in stNodeEditor1.Nodes)
                node.LockOption = true;

            stNodeEditor1.ResumeLayout();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
        }

        private CustomNode EntityToNode(Entity entity, Composite composite)
        {
            if (entity == null)
                return null;

            CustomNode node = _nodes.FirstOrDefault(o => o.ID == entity.shortGUID);

            if (node == null)
            {
                node = new CustomNode();
                node.ID = entity.shortGUID;
                switch (entity.variant)
                {
                    case EntityVariant.PROXY:
                    case EntityVariant.ALIAS:
                        Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, composite, (entity.variant == EntityVariant.PROXY) ? ((ProxyEntity)entity).proxy.path : ((AliasEntity)entity).alias.path, out Composite c, out string s);
                        node.SetColour(entity.variant == EntityVariant.PROXY ? Color.LightGreen : Color.Orange, Color.Black);
                        switch (ent.variant)
                        {
                            case EntityVariant.FUNCTION:
                                FunctionEntity function = (FunctionEntity)ent;
                                if (CommandsUtils.FunctionTypeExists(function.function))
                                {
                                    node.SetName(entity.variant + " TO: " + function.function.ToString() + "\n" + EntityUtils.GetName(c, ent), 35);
                                }
                                else
                                    node.SetName(entity.variant + " TO: " + Content.commands.GetComposite(function.function).name + "\n" + EntityUtils.GetName(c, ent), 35);
                                break;
                            case EntityVariant.VARIABLE:
                                node.SetName(entity.variant + " TO: " + ((VariableEntity)ent).name.ToString());
                                break;
                        }
                        break;
                    case EntityVariant.FUNCTION:
                        FunctionEntity funcEnt = (FunctionEntity)entity;
                        if (CommandsUtils.FunctionTypeExists(funcEnt.function))
                        {
                            node.SetName(funcEnt.function.ToString() + "\n" + EntityUtils.GetName(composite, entity), 35);
                        }
                        else
                        {
                            node.SetColour(Color.Blue, Color.White);
                            node.SetName(Content.commands.GetComposite(funcEnt.function).name + "\n" + EntityUtils.GetName(composite, entity), 35);
                        }
                        break;
                    case EntityVariant.VARIABLE:
                        node.SetColour(Color.Red, Color.White);
                        node.SetName(((VariableEntity)entity).name.ToString());
                        break;
                }
                node.Recompute();
                _nodes.Add(node);
                stNodeEditor1.Nodes.Add(node);
            }

            return node;
        }

        private void SaveFlowgraph_Click(object sender, EventArgs e)
        {
            _positionDB.SaveFlowgraph(_composite.name, stNodeEditor1);
        }
    }

    //todo: if this ever makes it to production, will need to make sure this is done safely to not overwrite stuff. at the mo its fine for testing locally with single instances.
    public class NodePositionDatabase
    {
        private string _filepath = "node_positions.bin";
        private List<FlowgraphMeta> _flowgraphMetas;

        public NodePositionDatabase()
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

        public bool TryRestoreFlowgraph(string compositeName, STNodeEditor editor)
        {
            FlowgraphMeta flowgraphMeta = _flowgraphMetas.FirstOrDefault(o => o.Name == compositeName);
            if (flowgraphMeta == null)
                return false;

            for (int i = 0; i < editor.Nodes.Count; i++)
            {
                if (!(editor.Nodes[i] is CustomNode))
                    continue;

                CustomNode node = (CustomNode)editor.Nodes[i];
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

        public void SaveFlowgraph(string compositeName, STNodeEditor editor)
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
                if (!(editor.Nodes[i] is CustomNode))
                    continue;

                CustomNode node = (CustomNode)editor.Nodes[i];
                FlowgraphMeta.NodeMeta nodeMeta = new FlowgraphMeta.NodeMeta();
                nodeMeta.ID = node.ID;
                nodeMeta.Position = node.Location;
                flowgraphMeta.Nodes.Add(nodeMeta);
            }

            SaveFile();
        }

        private void SaveFile()
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
