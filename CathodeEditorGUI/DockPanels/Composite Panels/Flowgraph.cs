using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ST.Library.UI.NodeEditor;
using CATHODE.Scripting.Internal;
using CATHODE.Scripting;
using OpenCAGE;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml.Linq;
using CATHODE;
using static CathodeLib.CompositeFlowgraphsTable;
using CommandsEditor.Popups.UserControls;
using System.IO;
using CathodeLib;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.ComponentModel.Design;
using static CommandsEditor.ModifyPinsOrParameters;
using System.Diagnostics;

namespace CommandsEditor
{
    public partial class Flowgraph : DockContent
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
        private string _flowgraphName = "";

        public Flowgraph()
        {
            InitializeComponent();
            this.FormClosed += EntityFlowgraph_FormClosed;

            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
            stNodeEditor1.AllowSameOwnerConnections = true;
            stNodeEditor1.SelectedChanged += Owner_SelectedChanged;

#if !DEBUG
            SaveFlowgraph.Visible = false;
            AutoCalc.Visible = false;
            AutoCalcAdjacents.Visible = false;
            RemoveEmpties.Visible = false;
            ResetFG.Visible = false;
            SaveFlowgraphUnfinished.Visible = false;
            SplitConnected.Visible = false;
            SplitInHalf.Visible = false;
            DuplicateForAllConnections.Visible = false;
            AutoCalcAndSplit.Visible = false;
#endif

            DEBUG_UnfinishedWarning.Visible = false;

            //todo: i feel like these events should come from the compositedisplay?
            Singleton.OnEntitySelected += OnEntitySelectedGlobally;
            Singleton.OnEntityAdded += OnEntityAddedGlobally;
            Singleton.OnEntityDeleted += OnEntityDeletedGlobally;
            Singleton.OnEntityRenamed += OnEntityRenamedGlobally;
        }

        private void OnEntitySelectedGlobally(Entity entity)
        {
            if (entity == _previouslySelectedEntity)
                return;
            SelectAllNodesForEntity(entity);
        }

        private void OnEntityRenamedGlobally(Entity entity, string newNew)
        {
            foreach (STNode node in stNodeEditor1.Nodes)
            {
                if (node.Entity.shortGUID != entity.shortGUID)
                    continue;
                RegenerateNodeStyle(node);
            }
        }

        private void EntityFlowgraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            stNodeEditor1.SelectedChanged -= Owner_SelectedChanged;
            Singleton.OnEntitySelected -= OnEntitySelectedGlobally;
            Singleton.OnEntityAdded -= OnEntityAddedGlobally;
            Singleton.OnEntityDeleted -= OnEntityDeletedGlobally;
            Singleton.OnEntityRenamed -= OnEntityRenamedGlobally;

            if (_renameFlowgraphPopup != null)
                _renameFlowgraphPopup.FormClosed -= _renameFlowgraphPopup_FormClosed;
        }

        private Entity _previouslySelectedEntity = null;
        private void Owner_SelectedChanged(object sender, EventArgs e)
        {
            if (!SettingsManager.GetBool(Singleton.Settings.OpenEntityFromNode))
                return;

            STNode[] nodes = stNodeEditor1.GetSelectedNode();
            if (nodes.Length != 1) return;

            Entity ent = _composite.GetEntityByID(nodes[0].ShortGUID);
            if (ent == _previouslySelectedEntity) return;
            _previouslySelectedEntity = ent;

            Singleton.Editor.CommandsDisplay?.CompositeDisplay?.LoadEntity(ent);
            Singleton.OnEntitySelected?.Invoke(ent); //need to call this again b/c the activation event doesn't fire here
        }

        private void SelectAllNodesForEntity(Entity entity)
        {
            DeselectAllNodes();

            if (entity == null)
                return;

            STNode[] nodes = stNodeEditor1.Nodes.ToArray();
            foreach (STNode node in nodes)
            {
                if (node.ShortGUID != entity.shortGUID)
                    continue;
                SelectNode(node);
            }
        }

        private void SelectNode(STNode node)
        {
            _previouslySelectedEntity = node.Entity;
            Console.WriteLine("SelectNode: " + node.Title + " - " + node.Guid);

            stNodeEditor1.AddSelectedNode(node);
            node.SetSelected(true, true);
            stNodeEditor1.SetActiveNode(node);
        }

        private void DeselectAllNodes()
        {
            STNode[] nodes = stNodeEditor1.Nodes.ToArray();
            foreach (STNode node in nodes)
            {
                if (!node.IsSelected)
                    continue;
                node.SetSelected(false, true);
            }
            stNodeEditor1.SetActiveNode(null);
            stNodeEditor1.RemoveAllSelectedNodes();
        }

        private void OnEntityAddedGlobally(Entity entity)
        {
            if (SettingsManager.GetBool(Singleton.Settings.MakeNodeWhenMakeEntity))
            {
                STNode node = EntityToNode(entity); 
                SelectNode(node);
            }
        }

        private void OnEntityDeletedGlobally(Entity entity)
        {
            List<STNode> nodes = new List<STNode>();

            STNode[] allNodes = stNodeEditor1.Nodes.ToArray();
            foreach (STNode node in allNodes)
            {
                if (node.ShortGUID != entity.shortGUID)
                    continue;

                nodes.Add(node);
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                stNodeEditor1.Nodes.Remove(nodes[i]);
            }
        }

        public bool ShowFlowgraph(Composite composite, FlowgraphMeta flowgraphMeta)
        {
            if (CommandsUtils.PurgeDeadLinks(Commands, composite))
                CommandsUtils.PurgedComposites.purged.Add(composite.shortGUID);

            _composite = composite;
            this.Text = flowgraphMeta.Name;
            _flowgraphName = flowgraphMeta.Name;

            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _spawnOffset = 0;

            //Populate nodes for entities
            List<Entity> populatedEntities = new List<Entity>();
            STNode[] nodes = new STNode[flowgraphMeta.Nodes.Count];
            for (int i = 0; i < flowgraphMeta.Nodes.Count; i++)
            {
                Entity entity = composite.GetEntityByID(flowgraphMeta.Nodes[i].EntityGUID);
                if (entity == null)
                {
                    //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                    return false;
                }
                populatedEntities.Add(entity);

                nodes[i] = EntityToNode(entity, true);
                nodes[i].SetPosition(flowgraphMeta.Nodes[i].Position);

                foreach (ShortGuid pin in flowgraphMeta.Nodes[i].PinsIn)
                    nodes[i].AddInputOption(pin);
                foreach (ShortGuid pin in flowgraphMeta.Nodes[i].PinsOut)
                    nodes[i].AddOutputOption(pin);

                nodes[i].NodeID = flowgraphMeta.Nodes[i].NodeID;
            }

            //Populate connections
            List<EntityConnector> populatedConnections = new List<EntityConnector>();
            for (int i = 0; i < flowgraphMeta.Nodes.Count; i++)
            {
                foreach (FlowgraphMeta.NodeMeta.ConnectionMeta connectionMeta in flowgraphMeta.Nodes[i].Connections)
                {
                    STNode connectedNode = nodes.FirstOrDefault(o => o.NodeID == connectionMeta.ConnectedNodeID && o.ShortGUID == connectionMeta.ConnectedEntityGUID);

                    STNodeOption pinOut = nodes[i].AddOutputOption(connectionMeta.ParameterGUID);
                    STNodeOption pinIn = connectedNode.AddInputOption(connectionMeta.ConnectedParameterGUID);
                    ConnectionStatus status = pinOut.ConnectOption(pinIn);

                    if (status != ConnectionStatus.Connected)
                    {
                        Console.WriteLine("WARNING! Could not create connection!");
                        return false;
                    }

                    EntityConnector connector = nodes[i].Entity.childLinks.FirstOrDefault(o => o.thisParamID == connectionMeta.ParameterGUID && o.linkedParamID == connectionMeta.ConnectedParameterGUID && o.linkedEntityID == connectedNode.ShortGUID);
                    if (connector.ID.IsInvalid)
                    {
                        //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                        Console.WriteLine("Unexpected extra connection that isn't in PAK!!");
                        return false;
                    }
                    populatedConnections.Add(connector);
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
                    if (!populatedConnections.Contains(connection))
                    {
                        Console.WriteLine("Failed to find connection from " + entity.shortGUID + " to " + connection.linkedEntityID.ToByteString() + ": '" + connection.thisParamID + "' [" + connection.thisParamID.ToByteString() + "] -> '" + connection.linkedParamID + "' [" + connection.linkedParamID.ToByteString() + "]");
                        //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                        return false;
                    }
                }

                if (!populatedEntities.Contains(entity))
                {
                    //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                    Console.WriteLine("missing entity with links");
                    return false;
                }
            }

            //Correctly respect the scale/position of the saved flowgraph
            stNodeEditor1.ScaleCanvas(flowgraphMeta.CanvasScale, 0, 0);
            stNodeEditor1.MoveCanvas(flowgraphMeta.CanvasPosition.X, flowgraphMeta.CanvasPosition.Y, false, CanvasMoveArgs.All);

            foreach (STNode node in stNodeEditor1.Nodes)
                node.Recompute();

            stNodeEditor1.ResumeLayout();
            stNodeEditor1.Invalidate();

#if DEBUG
            DEBUG_UnfinishedWarning.Visible = flowgraphMeta.IsUnfinished;
#endif

            return true;
        }

#if DEBUG
        //This is for local use only: populates all nodes from a composite without a layout
        public void PopulateDefaultEntities(Composite composite)
        {
            if (CommandsUtils.PurgeDeadLinks(Commands, composite))
                CommandsUtils.PurgedComposites.purged.Add(composite.shortGUID);

            _composite = composite;
            this.Text = _composite.name;

            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _spawnOffset = 0;

            List<Entity> entities = _composite.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].HasLinks(_composite))
                    continue;

                STNode mainNode = EntityToNode(entities[i]);

                for (int x = 0; x < entities[i].childLinks.Count; x++)
                {
                    Entity childEnt = composite.GetEntityByID(entities[i].childLinks[x].linkedEntityID);
                    if (childEnt == null)
                        continue;

                    STNode childNode = EntityToNode(childEnt);
                    STNodeOption linkIn = childNode.AddInputOption(entities[i].childLinks[x].linkedParamID);
                    STNodeOption linkOut = mainNode.AddOutputOption(entities[i].childLinks[x].thisParamID);
                    ConnectionStatus status = linkIn.ConnectOption(linkOut);
                    if (status != ConnectionStatus.Connected)
                    {
                        Console.WriteLine("WARNING! Could not create connection!");
                    }
                }
            }

            foreach (STNode node in stNodeEditor1.Nodes)
                node.Recompute();

            stNodeEditor1.ResumeLayout();
            stNodeEditor1.Invalidate();

            int height = 10;
            foreach (STNode node in stNodeEditor1.Nodes)
            {
                node.SetPosition(new Point(0, height));
                height += node.Height + 10;
            }
            
            DEBUG_UnfinishedWarning.Visible = false;

            this.Text = "UNSAVED with " + stNodeEditor1.Nodes.Count + " nodes";
            _flowgraphName = Path.GetFileName(_composite.name);
        }
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
        }

        private STNode EntityToNode(Entity entity, bool allowDuplicate = false)
        {
            if (entity == null)
                return null;

            //TODO: once the layout db is fully populated, this "allowDuplicate" thing can be removed as we will never not want duplicates enabled
            STNode node = null;
            if (!allowDuplicate)
            {
                foreach (STNode n in stNodeEditor1.Nodes)
                {
                    if (n.ShortGUID != entity.shortGUID)
                        continue;

                    node = n;
                    break;
                }
            }

            if (node == null)
            {
                node = new STNode();
                node.Entity = entity;
                RegenerateNodeStyle(node);
                stNodeEditor1.Nodes.Add(node);

                node.SetPosition(new Point(0, _spawnOffset));
                _spawnOffset += node.Height + 10;
            }

            return node;
        }

        //Regenerate the node's visual for the associated entity (sets name, colour, redraws)
        private void RegenerateNodeStyle(STNode node)
        {
            if (node == null)
                return;

            switch (node.Entity.variant)
            {
                case EntityVariant.PROXY:
                case EntityVariant.ALIAS:
                    Entity ent = CommandsUtils.ResolveHierarchy(Commands, _composite, (node.Entity.variant == EntityVariant.PROXY) ? ((ProxyEntity)node.Entity).proxy.path : ((AliasEntity)node.Entity).alias.path, out Composite c, out string s);
                    node.SetColour(node.Entity.variant == EntityVariant.PROXY ? Color.LightGreen : Color.Orange, Color.Black);
                    switch (ent.variant)
                    {
                        case EntityVariant.FUNCTION:
                            FunctionEntity function = (FunctionEntity)ent;
                            if (function.function.IsFunctionType)
                            {
                                node.SetName(EntityUtils.GetName(c, ent), node.Entity.variant + " TO: " + function.function.AsFunctionType.ToString());
                            }
                            else
                                node.SetName(EntityUtils.GetName(c, ent), node.Entity.variant + " TO: " + Path.GetFileName(Commands.GetComposite(function.function).name));
                            break;
                        case EntityVariant.VARIABLE:
                            node.SetName(node.Entity.variant + " TO: " + ((VariableEntity)ent).name.ToString());
                            break;
                    }
                    break;
                case EntityVariant.FUNCTION:
                    FunctionEntity funcEnt = (FunctionEntity)node.Entity;
                    if (funcEnt.function.IsFunctionType)
                    {
                        node.SetName(EntityUtils.GetName(_composite, node.Entity), funcEnt.function.AsFunctionType.ToString());
                    }
                    else
                    {
                        node.SetColour(Color.Blue, Color.White);
                        node.SetName(EntityUtils.GetName(_composite, node.Entity), Path.GetFileName(Commands.GetComposite(funcEnt.function).name));
                    }
                    break;
                case EntityVariant.VARIABLE:
                    VariableEntity varEnt = (VariableEntity)node.Entity;
                    node.SetColour(Color.Red, Color.White);
                    node.SetName(varEnt.name.ToString());
                    node.AddInputOption(varEnt.name);
                    node.AddOutputOption(varEnt.name);
                    break;
            }
            node.Recompute();
        }

        //Saves the Flowgraph's layout, and compiles the links back to commands
        //NOTE: This assumes that you have already cleared all childLinks in the composite already. That can be done by using CompositeUtils.ClearAllLinks
        public void SaveAndCompile()
        {
            FlowgraphMeta layout = FlowgraphLayoutManager.SaveLayout(stNodeEditor1, _composite, _flowgraphName);
            //Console.WriteLine("Stored flowgraph layout: " + layout.Name);

            //Re-generate connections using the content in the nodegraph
            foreach (STNode node in stNodeEditor1.Nodes)
            {
                STNodeOption[] options = node.GetOutputOptions();
                for (int y = 0; y < options.Length; y++)
                {
                    List<STNodeOption> connections = options[y].GetConnectedOption();
                    for (int z = 0; z < connections.Count; z++)
                    {
                        STNode connectedNode = connections[z].Owner;
                        node.Entity.AddParameterLink(options[y].ShortGUID, connectedNode.ShortGUID, connections[z].ShortGUID);
                    }
                }
            }
        }

        private void SaveFlowgraph_Click(object sender, EventArgs e)
        {
#if DEBUG
            FlowgraphLayoutManager.DEBUG_UsePreDefinedTable = true; 
            FlowgraphMeta layout = FlowgraphLayoutManager.SaveLayout(stNodeEditor1, _composite, _flowgraphName);
            Console.WriteLine("Saved predefined flowgraph layout: " + layout.Name);
            FlowgraphLayoutManager.DEBUG_UsePreDefinedTable = false;

            //TODO: should validate that the connections havent changed really before loading another to make sure we're not saving a dodgy layout

            Singleton.Editor.CommandsDisplay.DEBUG_LoadNextToConstruct();
#endif
        }

        private void SaveFlowgraphUnfinished_Click(object sender, EventArgs e)
        {
            FlowgraphLayoutManager.DEBUG_IsUnfinished = true;
            SaveFlowgraph_Click(null, null);
            FlowgraphLayoutManager.DEBUG_IsUnfinished = false;
        }

        private void ResetFG_Click(object sender, EventArgs e)
        {
#if DEBUG
            PopulateDefaultEntities(_composite);
#endif
        }

        private void AutoCalc_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
            {
                Console.WriteLine("SELECT ONE NODE");
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
                    //connectedNode.SetPosition(new Point(10, height));
                    connectedNode.SetPosition(new Point(250 - connectedNode.Width, height));
                    height += connectedNode.Height + 10;
                }
            }
            height = (this.Size.Height / 2) - (outputStackedHeight / 2) - 20;
            foreach (STNodeOption output in origNode.GetOutputOptions()) //outputs on this node
            {
                foreach (STNodeOption input in output.GetConnectedOption()) //connected to input on other node
                {
                    STNode connectedNode = input.Owner;
                    //connectedNode.SetPosition(new Point(this.Size.Width - connectedNode.Width - 50, height));
                    connectedNode.SetPosition(new Point(this.Size.Width - 250, height));
                    height += connectedNode.Height + 10;
                }
            }
            origNode.SetPosition(new Point((this.Size.Width / 2) - (origNode.Width / 2) - 10, (this.Size.Height / 2) - (((outputStackedHeight > inputStackedHeight) ? outputStackedHeight : inputStackedHeight) / 2) - 20));
        }

        List<STNode> positionedNodes = new List<STNode>();
        private void AutoCalcAdjacents_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
                return;

            positionedNodes.Clear();
            int currentY = stNodeEditor1.GetSelectedNode()[0].Location.Y; 
            PositionNode(stNodeEditor1.GetSelectedNode()[0], ref currentY, false);
        }
        private int PositionNode(STNode node, ref int currentY, bool doSplits)
        {
            if (positionedNodes.Contains(node))
                return 0;

            positionedNodes.Add(node);

            STNodeOption[] outputs = node.GetOutputOptions();
            STNodeOption[] inputs = node.GetInputOptions();
            int nodeWidthSpacing = 60;
            int nodeHeightSpacing = 20;

            int subtreeHeight = 0;
            int initialY = currentY;
            int stackedHeight = 0;

            for (int x = 0; x < outputs.Length; x++)
            {
                var connectedOptions = outputs[x].GetConnectedOption();
                for (int i = 0; i < connectedOptions.Count; i++)
                {
                    STNode connectedNode = connectedOptions[i].Owner;

                    if (doSplits)
                    {
                        //If the node's connected option is "reference" and it meets criteria, duplicate it
                        if (connectedOptions[i].ShortGUID == ShortGuidUtils.Generate("reference") && connectedOptions[i].ConnectionCount > 1)
                        {
                            //if (connectedNode.GetInputOptions().Length > 1 || connectedNode.GetOutputOptions().Length != 0 || (connectedNode.GetInputOptions().Length == 1 && connectedNode.GetInputOptions()[0].GetConnectedOption().Count != 1 && connectedNode.GetOutputOptions().Length == 0))
                            {
                                connectedNode = DuplicateNode(connectedNode);
                                outputs[x].DisconnectOption(connectedOptions[i]);
                                outputs[x].ConnectOption(connectedNode.GetInputOptions().FirstOrDefault(o => o.ShortGUID == connectedOptions[i].ShortGUID));
                            }
                        }

                        //If the connected node is a variable and the connection has multiple links, duplicate it
                        if (connectedNode.Entity.variant == EntityVariant.VARIABLE && connectedOptions[i].ConnectionCount > 1)
                        {
                            connectedNode = DuplicateNode(connectedNode);
                            outputs[x].DisconnectOption(connectedOptions[i]);
                            outputs[x].ConnectOption(connectedNode.GetInputOptions().FirstOrDefault(o => o.ShortGUID == connectedOptions[i].ShortGUID));
                        }

                        RemoveEmpties_Click(null, null);

                        //This is the end of the chain
                        /*
                        if (connectedNode.GetOutputOptions().Length == 0)
                        {
                            //If the node's connected option is "reference" and it has more than one other input, 
                            if (connectedOptions[i].ShortGUID == ShortGuidUtils.Generate("reference"))
                            {
                                if (connectedNode.GetInputOptions().Length > 1)
                                {
                                    connectedNode = DuplicateNode(connectedNode);
                                }
                            }
                        }
                        */
                    }

                    if (positionedNodes.Contains(connectedNode))
                        continue;

                    //if only one input, or if input is reference, and has multiple connections, and no outputs?
                    //duplicate node and use that as connected

                    int targetY = initialY + stackedHeight;
                    connectedNode.SetPosition(new Point(node.Location.X + node.Width + nodeWidthSpacing, targetY));

                    int thisHeightSpacing = nodeHeightSpacing;
                    if (i + 1 < connectedOptions.Count && connectedOptions[i + 1].Owner.Entity.variant == EntityVariant.VARIABLE)
                        thisHeightSpacing = 5;
                    if (x + 1 < outputs.Length && outputs[x + 1].GetConnectedOption().Count != 0 && outputs[x + 1].GetConnectedOption()[0].Owner.Entity.variant == EntityVariant.VARIABLE && connectedNode.Entity.variant == EntityVariant.VARIABLE)
                        thisHeightSpacing = 5;

                    int childSubtreeHeight = PositionNode(connectedNode, ref targetY, doSplits);
                    stackedHeight += childSubtreeHeight + thisHeightSpacing;

                    subtreeHeight = Math.Max(subtreeHeight, stackedHeight);
                }
            }

            if (outputs.Length > 0)
                node.SetPosition(new Point(node.Location.X, initialY)); 

            return Math.Max(subtreeHeight, node.Height);
        }

        private void AutoCalcAndSplit_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
                return;

            positionedNodes.Clear();
            int currentY = stNodeEditor1.GetSelectedNode()[0].Location.Y;
            PositionNode(stNodeEditor1.GetSelectedNode()[0], ref currentY, true);
        }

        private void SplitConnected_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
                return;

            STNode node = stNodeEditor1.GetSelectedNode()[0];
            STNodeOption[] inputs = node.GetInputOptions();
            STNodeOption[] outputs = node.GetOutputOptions();

            if (outputs.Length != 1)
                return;

            if (outputs[0].GetConnectedOption().Count != 1)
                return;

            STNodeOption connectedOption = outputs[0].GetConnectedOption()[0];
            STNode duplicated = DuplicateNode(connectedOption.Owner);

            connectedOption.DisconnectOption(outputs[0]);
            duplicated.GetInputOptions().FirstOrDefault(o => o.ShortGUID == connectedOption.ShortGUID).ConnectOption(outputs[0]);

            duplicated.SetPosition(new Point(node.Location.X + node.Width + 60, node.Location.Y));
        }

        private void SplitInHalf_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
                return;

            STNode node = stNodeEditor1.GetSelectedNode()[0];
            STNodeOption[] outputs = node.GetOutputOptions();

            STNode duplicated = DuplicateNode(node);
            duplicated.SetPosition(new Point(node.Location.X + 20, node.Location.Y + 20));

            for (int i = 0; i < outputs.Length; i++)
            {
                List<STNodeOption> connections = outputs[i].GetConnectedOption();
                for (int x = 0; x < connections.Count; x++)
                {
                    STNodeOption connectedOption = connections[x];
                    connectedOption.DisconnectOption(outputs[i]);
                    duplicated.GetOutputOptions().FirstOrDefault(o => o.ShortGUID == outputs[i].ShortGUID).ConnectOption(connectedOption);
                }
            }
        }

        private void RemoveEmpties_Click(object sender, EventArgs e)
        {
            foreach (STNode node in stNodeEditor1.Nodes)
            {
                List<ShortGuid> toRemove = new List<ShortGuid>();
                foreach (STNodeOption option in node.GetInputOptions()) 
                {
                    if (option.GetConnectedOption().Count == 0)
                        toRemove.Add(option.ShortGUID);
                }
                for (int x = 0; x < toRemove.Count; x++)
                    node.RemoveInputOption(toRemove[x]);
                toRemove.Clear();
                foreach (STNodeOption option in node.GetOutputOptions())
                {
                    if (option.GetConnectedOption().Count == 0)
                        toRemove.Add(option.ShortGUID);
                }
                for (int x = 0; x < toRemove.Count; x++)
                    node.RemoveOutputOption(toRemove[x]);
            }
        }

        private void DuplicateForAllConnections_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
                return;

            STNode node = stNodeEditor1.GetSelectedNode()[0];

            int duplicateCount = 0;

            STNodeOption[] inputs = node.GetInputOptions();
            for (int i = 0; i < inputs.Length; i++)
            {
                var connectedOptions = inputs[i].GetConnectedOption();
                for (int x = 0; x < connectedOptions.Count; x++)
                {
                    STNode newNode = DuplicateNode(node);
                    inputs[i].DisconnectOption(connectedOptions[x]);
                    newNode.GetInputOptions().FirstOrDefault(o => o.ShortGUID == inputs[i].ShortGUID).ConnectOption(connectedOptions[x]);

                    duplicateCount++;
                    newNode.SetPosition(new Point(node.Location.X + (duplicateCount * 10), node.Location.Y + (duplicateCount * 10)));
                }
            }

            STNodeOption[] outputs = node.GetOutputOptions();
            for (int i = 0; i < outputs.Length; i++)
            {
                var connectedOptions = outputs[i].GetConnectedOption();
                for (int x = 0; x < connectedOptions.Count; x++)
                {
                    STNode newNode = DuplicateNode(node);
                    outputs[i].DisconnectOption(connectedOptions[x]);
                    newNode.GetOutputOptions().FirstOrDefault(o => o.ShortGUID == outputs[i].ShortGUID).ConnectOption(connectedOptions[x]);

                    duplicateCount++;
                    newNode.SetPosition(new Point(node.Location.X + (duplicateCount * 10), node.Location.Y + (duplicateCount * 10)));
                }
            }

            stNodeEditor1.Nodes.Remove(node);
        }

        //disable entity-related actions on the context menu if no entity is selected
        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            STNode node = stNodeEditor1.GetHoveredNode();

            modifyPinsIn.Visible = node != null;
            modifyPinsOut.Visible = node != null;
            toolStripSeparator1.Visible = node != null;
            deleteToolStripMenuItem.Visible = node != null;
            duplicateToolStripMenuItem.Visible = node != null;
            toolStripSeparator2.Visible = node != null;
            addAllPinsToolStripMenuItem.Visible = node != null;
            removeUnusedPinsToolStripMenuItem.Visible = node != null;

            if (node != null)
            {
                modifyPinsIn.Enabled = node.Entity.variant != EntityVariant.VARIABLE;
                modifyPinsOut.Enabled = node.Entity.variant != EntityVariant.VARIABLE;
            }

            addNodeToolStripMenuItem.Visible = node == null;
        }

        //Add new node (via context menu strip)
        Point _nodeSpawnPosition = new Point();
        private void addNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectHierarchy selectEnt = new SelectHierarchy(_composite, new Popups.UserControls.CompositeEntityList.DisplayOptions()
            {
                DisplayAliases = true,
                DisplayFunctions = true,
                DisplayProxies = true,
                DisplayVariables = true,
                ShowCheckboxes = true,
            }, false);
            selectEnt.OnFinalEntitiesSelected += AddNodeCallbackEntitySelected;
            selectEnt.Show();
            _nodeSpawnPosition = new Point((int)stNodeEditor1.MousePositionInCanvas.X, (int)stNodeEditor1.MousePositionInCanvas.Y);
        }
        private void AddNodeCallbackEntitySelected(List<Entity> ent)
        {
            for (int i = 0; i < ent.Count; i++)
            {
                STNode node = EntityToNode(ent[i], true);
                Point offsetSpawnPos = new Point(_nodeSpawnPosition.X + (i * 20), _nodeSpawnPosition.Y + (i * 20));
                node.SetPosition(offsetSpawnPos);
            }
        }

        ModifyPinsOrParameters _pinManager = null;
        private void PinManager(ModifyPinsOrParameters.Mode mode)
        {
            STNode node = stNodeEditor1.GetHoveredNode();

            if (_pinManager != null)
                _pinManager.Close();

            _pinManager = new ModifyPinsOrParameters(node, mode);
            _pinManager.Show();
        }

        //Add/remove pins in/out 
        private void modifyPinsIn_Click(object sender, EventArgs e)
        {
            PinManager(ModifyPinsOrParameters.Mode.LINK_IN);
        }
        private void modifyPinsOut_Click(object sender, EventArgs e)
        {
            PinManager(ModifyPinsOrParameters.Mode.LINK_OUT);
        }

        //Add/remove batch pins in/out
        private void addAllPinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STNode node = stNodeEditor1.GetHoveredNode();
            List<(ShortGuid, ParameterVariant, DataType)> allParameters = ParameterUtils.GetAllParameters(_composite.GetEntityByID(node.ShortGUID), _composite);
            foreach ((ShortGuid, ParameterVariant, DataType) parameter in allParameters)
            {
                switch (parameter.Item2)
                {
                    case ParameterVariant.STATE_PARAMETER:
                    case ParameterVariant.INPUT_PIN:
                    case ParameterVariant.PARAMETER:
                        //case ParameterVariant.METHOD_FUNCTION:
                        node.AddInputOption(parameter.Item1);
                        break;
                    case ParameterVariant.METHOD_PIN:
                        node.AddInputOption(parameter.Item1);
                        ShortGuid relay = ParameterUtils.GetRelay(parameter.Item1);
                        if (relay != ShortGuid.Invalid)
                            node.AddOutputOption(relay);
                        break;
                    case ParameterVariant.OUTPUT_PIN:
                    case ParameterVariant.TARGET_PIN:
                    case ParameterVariant.REFERENCE_PIN:
                        node.AddOutputOption(parameter.Item1);
                        break;
                }
            }
        }
        private void removeUnusedPinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STNode node = stNodeEditor1.GetHoveredNode();
            STNodeOption[] ins = node.GetInputOptions();
            for (int i = 0; i < ins.Length; i++)
                if (ins[i].ConnectionCount == 0)
                    node.RemoveInputOption(ins[i].ShortGUID);
            STNodeOption[] outs = node.GetOutputOptions();
            for (int i = 0; i < outs.Length; i++)
                if (outs[i].ConnectionCount == 0)
                    node.RemoveOutputOption(outs[i].ShortGUID);
        }

        //Delete right clicked node
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: confirmation?
            STNode node = stNodeEditor1.GetHoveredNode();
            stNodeEditor1.Nodes.Remove(node);
        }

        //Duplicate the right clicked node
        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STNode node = stNodeEditor1.GetHoveredNode();
            DuplicateNode(node);
        }
        private STNode DuplicateNode(STNode node)
        {
            STNode duplicated = EntityToNode(node.Entity, true);

            STNodeOption[] ins = node.GetInputOptions();
            for (int i = 0; i < ins.Length; i++)
                duplicated.AddInputOption(ins[i].ShortGUID);
            STNodeOption[] outs = node.GetOutputOptions();
            for (int i = 0; i < outs.Length; i++)
                duplicated.AddOutputOption(outs[i].ShortGUID);

            duplicated.SetPosition(new Point(node.Location.X + 15, node.Location.Y + 15));

            return duplicated;
        }

        private void TabStripContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            deleteFGToolstripMenuItem.Text = "Delete flowgraph '" + _flowgraphName + "'";
            renameFGToolStripMenuItem.Text = "Rename flowgraph '" + _flowgraphName + "'";
        }

        private void deleteFGToolstripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the flowgraph '" + _flowgraphName + "'?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            FlowgraphLayoutManager.RemoveLayout(_composite, _flowgraphName);
            this.Close();
        }
        RenameGeneric _renameFlowgraphPopup;
        private void renameFGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_renameFlowgraphPopup != null)
                _renameFlowgraphPopup.Close();

            _renameFlowgraphPopup = new RenameGeneric(_flowgraphName, new RenameGeneric.RenameGenericContent()
            {
                Title = "Rename flowgraph for " + _composite.name,
                Description = "New Flowgraph Name",
                ButtonText = "Rename Flowgraph"
            });
            _renameFlowgraphPopup.Show();
            _renameFlowgraphPopup.OnRenamed += OnRenameFlowgraph;
            _renameFlowgraphPopup.FormClosed += _renameFlowgraphPopup_FormClosed;
        }
        private void OnRenameFlowgraph(string name)
        {
            List<FlowgraphMeta> layouts = FlowgraphLayoutManager.GetLayouts(_composite);
            for (int i = 0; i < layouts.Count; i++)
            {
                if (layouts[i].Name == name)
                {
                    MessageBox.Show("There's already a flowgraph named '" + name + "' in this Composite! Please pick a unique name.", "Name taken!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            layouts.FirstOrDefault(o => o.Name == _flowgraphName).Name = name;
            this.Text = name;
            _flowgraphName = name;
        }
        private void _renameFlowgraphPopup_FormClosed(object sender, FormClosedEventArgs e)
        {
            _renameFlowgraphPopup = null;
        }
        private void createNewFlowgraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Editor.CommandsDisplay.CompositeDisplay.CreateFlowgraph();
        }
    }
}
