
//#define USE_LEGACY_NODEDB

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
using CommandsEditor.DockPanels;
using OpenCAGE;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using static System.Windows.Forms.LinkLabel;
using System.Xml.Linq;
using CathodeLib;
using System.Windows.Input;
using CATHODE;
using static CathodeLib.CompositeFlowgraphsTable;
using static CathodeLib.CompositeFlowgraphsTable.FlowgraphMeta;

namespace CommandsEditor
{
    public partial class EntityFlowgraph : DockContent
    {
        //protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        private Commands _commands; //only for testing
        private Commands Commands
        {
            get
            {
                if (_commands == null)
                    return Singleton.Editor?.CommandsDisplay?.Content.commands;
                else
                    return _commands;
            }
        }


        private Composite _composite;
        private int _spawnOffset = 0;

        public EntityFlowgraph()
        {
            InitializeComponent();
            this.FormClosed += EntityFlowgraph_FormClosed; ;

            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
            stNodeEditor1.AllowSameOwnerConnections = true;
            stNodeEditor1.SelectedChanged += Owner_SelectedChanged;

#if !DEBUG
            DEBUG_CalcPositions.Visible = false;
            DEBUG_DumpUnfinished.Visible = false;
            DEBUG_Duplicate.Visible = false;
            DEBUG_NextAndSave.Visible = false;
            DEBUG_NextUnfinished.Visible = false;
            DEBUG_SaveAllNoLinks.Visible = false;
            DEBUG_Next1Link.Visible = false;
            DEBUG_LoadAll.Visible = false;
#endif

            //todo: i feel like these events should come from the compositedisplay?
            Singleton.OnEntitySelected += OnEntitySelectedGlobally;
            Singleton.OnEntityAdded += OnEntityAddedGlobally;
            Singleton.OnEntityDeleted += OnEntityDeletedGlobally;
        }

        private void EntityFlowgraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            stNodeEditor1.SelectedChanged -= Owner_SelectedChanged;
            Singleton.OnEntitySelected -= OnEntitySelectedGlobally;
            Singleton.OnEntityAdded -= OnEntityAddedGlobally;
            Singleton.OnEntityDeleted -= OnEntityDeletedGlobally;
        }

        private Entity _previouslySelectedEntity = null;
        private void Owner_SelectedChanged(object sender, EventArgs e)
        {
            if (!SettingsManager.GetBool(Singleton.Settings.OpenEntityFromNode))
                return;

            STNode[] nodes = stNodeEditor1.GetSelectedNode();
            if (nodes.Length != 1) return;

            Entity ent = _composite.GetEntityByID(((CathodeNode)nodes[0]).ShortGUID);
            if (ent == _previouslySelectedEntity) return;
            _previouslySelectedEntity = ent;

            Singleton.Editor.CommandsDisplay?.CompositeDisplay?.LoadEntity(ent);
            Singleton.OnEntitySelected?.Invoke(ent); //need to call this again b/c the activation event doesn't fire here
        }

        private void OnEntitySelectedGlobally(Entity entity)
        {
            for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
            {
                stNodeEditor1.Nodes[i].IsSelected = false;
                stNodeEditor1.Nodes[i].SetSelected(false, true);
            }
            stNodeEditor1.SetActiveNode(null);

            if (entity == null)
                return;

            for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
            {
                if (stNodeEditor1.Nodes[i].ShortGUID == entity.shortGUID)
                {
                    stNodeEditor1.Nodes[i].IsSelected = true;
                    stNodeEditor1.Nodes[i].SetSelected(true, true);
                    stNodeEditor1.SetActiveNode(stNodeEditor1.Nodes[i]);
                    return;
                }
            }
        }

        private void OnEntityAddedGlobally(Entity entity)
        {
            EntityToNode(entity, _composite);
        }

        private void OnEntityDeletedGlobally(Entity entity)
        {
            List<STNode> nodes = new List<STNode>();
            for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
            {
                if (stNodeEditor1.Nodes[i].ShortGUID != entity.shortGUID)
                    continue;

                nodes.Add(stNodeEditor1.Nodes[i]);
                break;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                stNodeEditor1.Nodes.Remove(nodes[i]);
            }
        }

        public void ShowComposite(Composite composite)
        {
            Console.WriteLine("EntityFlowgraph::ShowComposite - " + composite.name);

            List<Entity> purged = CommandsUtils.PurgeDeadLinks(Commands, composite);
            for (int i = 0; i < purged.Count; i++)
                Singleton.OnEntityDeleted(purged[i]);

            _composite = composite;
            this.Text = _composite.name;

            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _spawnOffset = 0;

            FlowgraphMeta flowgraphMeta = FlowgraphManager.GetLayout(composite);
#if USE_LEGACY_NODEDB
            flowgraphMeta = null;
            Console.WriteLine("Using Legacy NodePositionDatabase");
#endif
            if (flowgraphMeta != null)
            {
                //Populate nodes for entities
                List<Entity> populatedEntities = new List<Entity>();
                CathodeNode[] nodes = new CathodeNode[flowgraphMeta.Nodes.Count];
                for (int i = 0; i < flowgraphMeta.Nodes.Count; i++)
                {
                    Entity entity = composite.GetEntityByID(flowgraphMeta.Nodes[i].EntityGUID);
                    if (entity == null)
                    {
                        //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                        //TODO: Do something here.
                        throw new Exception("Mismatch!");
                    }
                    populatedEntities.Add(entity);

                    nodes[i] = EntityToNode(entity, composite, true);
                    nodes[i].SetPosition(flowgraphMeta.Nodes[i].Position);
                }

                //Populate connections
                List<ShortGuid> populatedConnections = new List<ShortGuid>();
                for (int i = 0; i < flowgraphMeta.Nodes.Count; i++)
                {
                    //NOTE: IMPORTANT! this exposes a dumb oversight in that i populate connections twice (facepalm)
                    //The way this should be stored: save all pins in/out on 
                    foreach (FlowgraphMeta.NodeMeta.ConnectionMeta connectionMeta in flowgraphMeta.Nodes[i].ConnectionsIn)
                    {
                        //NOTE: IMPORTANT! i'm now realising that this won't work for "multiple nodes per entity" -> each node needs to have its own unique identifier to be able to connect reliably. this will need adding into the population/saving logic for nodes.
                        CathodeNode connectedNode = nodes.FirstOrDefault(o => o.ShortGUID == connectionMeta.ConnectedEntityGUID);

                        STNodeOption pinIn = nodes[i].AddInputOption(connectionMeta.ParameterGUID);
                        STNodeOption pinOut = connectedNode.AddOutputOption(connectionMeta.ConnectedParameterGUID);
                        pinIn.ConnectOption(pinOut);

                        EntityConnector connector = connectedNode.Entity.childLinks.FirstOrDefault(o => o.thisParamID == connectionMeta.ConnectedParameterGUID && o.linkedParamID == connectionMeta.ParameterGUID);
                        if (connector.ID.IsInvalid)
                        {
                            //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                            //TODO: Do something here.
                            throw new Exception("Mismatch!");
                        }
                        populatedConnections.Add(connector.ID);
                    }
                    foreach (FlowgraphMeta.NodeMeta.ConnectionMeta connectionMeta in flowgraphMeta.Nodes[i].ConnectionsOut)
                    {
                        CathodeNode connectedNode = nodes.FirstOrDefault(o => o.ShortGUID == connectionMeta.ConnectedEntityGUID);

                        STNodeOption pinOut = nodes[i].AddOutputOption(connectionMeta.ParameterGUID);
                        STNodeOption pinIn = connectedNode.AddInputOption(connectionMeta.ConnectedParameterGUID);
                        pinOut.ConnectOption(pinIn);

                        EntityConnector connector = nodes[i].Entity.childLinks.FirstOrDefault(o => o.thisParamID == connectionMeta.ParameterGUID && o.linkedParamID == connectionMeta.ConnectedParameterGUID);
                        if (connector.ID.IsInvalid)
                        {
                            //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                            //TODO: Do something here.
                            throw new Exception("Mismatch!");
                        }
                        populatedConnections.Add(connector.ID);
                    }
                }

                //Sanity check that our Composite doesn't contain any additional links/entities that we didn't populate in the flowgraph but should've
                List<Entity> entities = composite.GetEntities();
                foreach (Entity entity in entities)
                {
                    if (entity.childLinks.Count == 0)
                        continue;

                    foreach (EntityConnector connection in entity.childLinks)
                    {
                        if (!populatedConnections.Contains(connection.ID))
                        {
                            //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                            //TODO: Do something here.
                            Console.WriteLine("Failed to find connection from " + entity.shortGUID + " to " + connection.linkedEntityID.ToByteString() + ": '" + connection.thisParamID + "' [" + connection.thisParamID.ToByteString() + "] -> '" + connection.linkedParamID + "' [" + connection.linkedParamID.ToByteString() + "]");
                            //throw new Exception("Mismatch!");
                        }
                    }

                    if (!populatedEntities.Contains(entity))
                    {
                        //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                        //TODO: Do something here.
                        throw new Exception("Mismatch!");
                    }
                }

                //Correctly respect the scale/position of the saved flowgraph
                stNodeEditor1.ScaleCanvas(flowgraphMeta.CanvasScale, 0, 0);
                stNodeEditor1.MoveCanvas(flowgraphMeta.CanvasPosition.X, flowgraphMeta.CanvasPosition.Y, false, CanvasMoveArgs.All);
            }
            else
            {
#if !USE_LEGACY_NODEDB
                //This composite has no defined layouts
                Console.WriteLine("NO DEFINED FLOWGRAPH LAYOUT FOUND! We should never reach this in production code.");
#endif

                List<Entity> entities = _composite.GetEntities();
                for (int i = 0; i < entities.Count; i++)
                {
                    if (!entities[i].HasLinks(_composite))
                        continue;

                    CathodeNode mainNode = EntityToNode(entities[i], _composite);

                    for (int x = 0; x < entities[i].childLinks.Count; x++)
                    {
                        Entity childEnt = composite.GetEntityByID(entities[i].childLinks[x].linkedEntityID);
                        if (childEnt == null)
                            continue;

                        CathodeNode childNode = EntityToNode(childEnt, _composite);
                        STNodeOption linkIn = childNode.AddInputOption(entities[i].childLinks[x].linkedParamID);
                        STNodeOption linkOut = mainNode.AddOutputOption(entities[i].childLinks[x].thisParamID);
                        linkIn.ConnectOption(linkOut);
                    }

                    //we should ensure variable entities always have a pin in/out for easier editing
                    if (entities[i].variant == EntityVariant.VARIABLE)
                    {
                        VariableEntity varEnt = (VariableEntity)entities[i];
                        mainNode.AddInputOption(varEnt.name);
                        mainNode.AddOutputOption(varEnt.name);
                    }
                    //NOTE: WE should limit link creation on variable entities to JUST the above. 
                }

#if USE_LEGACY_NODEDB
                NodePositionDatabase.TryRestoreFlowgraph(_composite.name, stNodeEditor1);
#endif
            }

            foreach (STNode node in stNodeEditor1.Nodes)
                ((CathodeNode)node).Recompute();

            stNodeEditor1.ResumeLayout();
            stNodeEditor1.Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
        }

        private CathodeNode EntityToNode(Entity entity, Composite composite, bool allowDuplicate = false)
        {
            if (entity == null)
                return null;

            CathodeNode node = null;
            if (!allowDuplicate)
            {
                for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
                {
                    if (stNodeEditor1.Nodes[i].ShortGUID != entity.shortGUID)
                        continue;

                    node = (CathodeNode)stNodeEditor1.Nodes[i]; //todo: should remove need to cast
                    break;
                }
            }

            if (node == null)
            {
                node = new CathodeNode();
                node.Entity = entity;
                switch (entity.variant)
                {
                    case EntityVariant.PROXY:
                    case EntityVariant.ALIAS:
                        Entity ent = CommandsUtils.ResolveHierarchy(Commands, composite, (entity.variant == EntityVariant.PROXY) ? ((ProxyEntity)entity).proxy.path : ((AliasEntity)entity).alias.path, out Composite c, out string s);
                        node.SetColour(entity.variant == EntityVariant.PROXY ? Color.LightGreen : Color.Orange, Color.Black);
                        switch (ent.variant)
                        {
                            case EntityVariant.FUNCTION:
                                FunctionEntity function = (FunctionEntity)ent;
                                if (CommandsUtils.FunctionTypeExists(function.function))
                                {
                                    node.SetName(EntityUtils.GetName(c, ent), entity.variant + " TO: " + function.function.ToString());
                                }
                                else
                                    node.SetName(EntityUtils.GetName(c, ent) , entity.variant + " TO: " + Commands.GetComposite(function.function).name);
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
                            node.SetName(EntityUtils.GetName(composite, entity), funcEnt.function.ToString());
                        }
                        else
                        {
                            node.SetColour(Color.Blue, Color.White);
                            node.SetName(EntityUtils.GetName(composite, entity), Commands.GetComposite(funcEnt.function).name);
                        }
                        break;
                    case EntityVariant.VARIABLE:
                        VariableEntity varEnt = (VariableEntity)entity;
                        node.SetColour(Color.Red, Color.White);
                        node.SetName(varEnt.name.ToString());
                        node.AddInputOption(varEnt.name);
                        node.AddOutputOption(varEnt.name);
                        break;
                }
                node.Recompute();
                stNodeEditor1.Nodes.Add(node);

                ((CathodeNode)node).SetPosition(new Point(0, _spawnOffset));
                _spawnOffset += node.Height + 10;
            }

            return node;
        }

        private void SaveFlowgraph_Click(object sender, EventArgs e)
        {
            NodePositionDatabase.SaveFlowgraph(_composite.name, stNodeEditor1);
        }

        private void DEBUG_CalcPositions_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
            {
                Console.WriteLine("SELECT ONE NODE");
                return;
            }
            if (!(stNodeEditor1.GetSelectedNode()[0] is CathodeNode))
            {
                Console.WriteLine("SELECT CathodeNode");
                return;
            }

            TestAlignNode(stNodeEditor1.GetSelectedNode()[0]);
        }
        private void TestAlignNode(STNode origNode)
        {
            //Compute node sizes
            int inputStackedHeight = 0;
            foreach (STNodeOption input in origNode.GetInputOptions()) //inputs on this node
            {
                foreach (STNodeOption output in input.GetConnectedOption()) //connected to output on other node
                {
                    STNode connectedNode = output.Owner;
                    inputStackedHeight += connectedNode.Height + 10;
                }
            }
            int outputStackedHeight = 0;
            foreach (STNodeOption output in origNode.GetOutputOptions()) //outputs on this node
            {
                foreach (STNodeOption input in output.GetConnectedOption()) //connected to input on other node
                {
                    STNode connectedNode = input.Owner;
                    outputStackedHeight += connectedNode.Height + 10;
                }
            }

            //Set node positions
            int height = (this.Size.Height / 2) - (inputStackedHeight / 2) - 20;
            foreach (STNodeOption input in origNode.GetInputOptions()) //inputs on this node
            {
                foreach (STNodeOption output in input.GetConnectedOption()) //connected to output on other node
                {
                    STNode connectedNode = output.Owner;
                    ((CathodeNode)connectedNode).SetPosition(new Point(10, height));
                    height += connectedNode.Height + 10;
                }
            }
            height = (this.Size.Height / 2) - (outputStackedHeight / 2) - 20;
            foreach (STNodeOption output in origNode.GetOutputOptions()) //outputs on this node
            {
                foreach (STNodeOption input in output.GetConnectedOption()) //connected to input on other node
                {
                    STNode connectedNode = input.Owner;
                    ((CathodeNode)connectedNode).SetPosition(new Point(this.Size.Width - connectedNode.Width - 50, height));
                    height += connectedNode.Height + 10;
                }
            }
            ((CathodeNode)origNode).SetPosition(new Point((this.Size.Width / 2) - (origNode.Width / 2) - 10, (this.Size.Height / 2) - (((outputStackedHeight > inputStackedHeight) ? outputStackedHeight : inputStackedHeight) / 2) - 20));
        }

        private void DEBUG_NextUnfinished_Click(object sender, EventArgs e)
        {
            int index = Commands.Entries.IndexOf(_composite) + 1;
            if (index >= Commands.Entries.Count)
                index = 0;
            
            while (NodePositionDatabase.CanRestoreFlowgraph(Commands.Entries[index].name))
            {
                if (index + 1 >= Commands.Entries.Count)
                    index = 0;
                else
                    index += 1;
            }
            ShowComposite(Commands.Entries[index]);
        }

        private void DEBUG_DumpUnfinished_Click(object sender, EventArgs e)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("Incomplete Flowgraphs:");
            int count = 0;
            for (int i = 0; i < Commands.Entries.Count; i++)
            {
                if (NodePositionDatabase.CanRestoreFlowgraph(Commands.Entries[i].name))
                    continue;

                Console.WriteLine(" - " + Commands.Entries[i].name);
                count++;
            }
            Console.WriteLine("(There are " + count + ")");
            Console.WriteLine("----------------------");
        }

        private void DEBUG_NextAndSave_Click(object sender, EventArgs e)
        {
            SaveFlowgraph_Click(null, null);
            DEBUG_NextUnfinished_Click(null, null);
        }

        private void DEBUG_Duplicate_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
            {
                Console.WriteLine("SELECT ONE NODE");
                return;
            }
            if (!(stNodeEditor1.GetSelectedNode()[0] is CathodeNode))
            {
                Console.WriteLine("SELECT CathodeNode");
                return;
            }

            CathodeNode node = (CathodeNode)stNodeEditor1.GetSelectedNode()[0];
            CathodeNode nodeNew = EntityToNode(_composite.GetEntityByID(node.ShortGUID), _composite, true);
            foreach (STNodeOption input in node.GetInputOptions()) 
                nodeNew.AddInputOption(input.ShortGUID);
            foreach (STNodeOption output in node.GetOutputOptions())
                nodeNew.AddOutputOption(output.ShortGUID);

            nodeNew.SetPosition(new Point(node.Location.X + 10, node.Location.Y + 10));
        }

        private void DEBUG_SaveAllNoLinks_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Commands.Entries.Count; i++)
            {
                if (NodePositionDatabase.CanRestoreFlowgraph(Commands.Entries[i].name))
                    continue;

                bool noLinks = true;
                List<Entity> entities = Commands.Entries[i].GetEntities();
                for (int x = 0; x < entities.Count; x++)
                {
                    List<EntityConnector> linksIn = entities[x].GetParentLinks(Commands.Entries[i]);
                    List<EntityConnector> linksOut = entities[x].childLinks;

                    if (linksIn.Count != 0 || linksOut.Count != 0)
                    {
                        noLinks = false;
                        break;
                    }
                }

                if (!noLinks)
                    continue;

                ShowComposite(Commands.Entries[i]);
                SaveFlowgraph_Click(null, null);
            }
        }

        private void DEBUG_Next1Link_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Commands.Entries.Count; i++)
            {
                if (NodePositionDatabase.CanRestoreFlowgraph(Commands.Entries[i].name))
                    continue;

                bool shouldGenerate = false;
                Entity entWithLinksInAndOut = null;
                List<Entity> entities = Commands.Entries[i].GetEntities();
                for (int x = 0; x < entities.Count; x++)
                {
                    List<EntityConnector> linksIn = entities[x].GetParentLinks(Commands.Entries[i]);
                    List<EntityConnector> linksOut = entities[x].childLinks;

                    if (linksIn.Count != 0 && linksOut.Count != 0)
                    {
                        if (entWithLinksInAndOut == null)
                        {
                            entWithLinksInAndOut = entities[x];
                            shouldGenerate = true;
                        }
                        else
                        {
                            shouldGenerate = false;
                        }
                    }
                }

                if (shouldGenerate)
                {
                    ShowComposite(Commands.Entries[i]);
                    STNode node = null;
                    for (int x = 0; x < stNodeEditor1.Nodes.Count; x++)
                    {
                        if (stNodeEditor1.Nodes[x].ShortGUID != entWithLinksInAndOut.shortGUID)
                            continue;

                        node = stNodeEditor1.Nodes[x]; 
                        break;
                    }
                    TestAlignNode(node);
                    return;
                }
            }
        }

        private void DEBUG_LoadAll_Click(object sender, EventArgs e)
        {
            DEBUG_LoadAll_Test(_commands, false);
        }
        public void DEBUG_LoadAll_Test(Commands commands, bool doingConversion)
        {
            _commands = commands;
            for (int i = 0; i < Commands.Entries.Count; i++)
            {
                ShowComposite(Commands.Entries[i]);

                if (doingConversion && NodePositionDatabase.CanRestoreFlowgraph(Commands.Entries[i].name))
                    FlowgraphManager.AddVanillaFlowgraph(stNodeEditor1, Commands.Entries[i]);
            }
        }
    }
}
