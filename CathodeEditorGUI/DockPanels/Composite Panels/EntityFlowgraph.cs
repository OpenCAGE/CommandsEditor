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
            DEBUG_AddPinIn.Visible = false;
            DEBUG_AddPinOut.Visible = false;
            DEBUG_Compile.Visible = false;
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

            Entity ent = _composite.GetEntityByID(nodes[0].ShortGUID);
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

                    //TODO: I'd really like to focus the canvas on the selected node, but I can't think how to get it to work. Either that, or the highlight should be more obvious
                    //stNodeEditor1.MoveCanvas(stNodeEditor1.Nodes[i].Location.X - (stNodeEditor1.Width / 2), stNodeEditor1.Nodes[i].Location.Y - (stNodeEditor1.Height / 2), false, CanvasMoveArgs.All);
                    return;
                }
            }
        }

        private void OnEntityAddedGlobally(Entity entity)
        {
            if (SettingsManager.GetBool(Singleton.Settings.MakeNodeWhenMakeEntity))
            {
                EntityToNode(entity, _composite); //TODO: should really put it in a nice spot and/or maybe jump to it 
            }
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

            CommandsUtils.PurgeDeadLinks(Commands, composite);
            CommandsUtils.PurgedComposites.purged.Add(composite.shortGUID);

            _composite = composite;
            this.Text = _composite.name;

            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _spawnOffset = 0;

            FlowgraphMeta flowgraphMeta = FlowgraphManager.GetLayout(composite);
            if (flowgraphMeta != null)
            {
                //Populate nodes for entities
                List<Entity> populatedEntities = new List<Entity>();
                STNode[] nodes = new STNode[flowgraphMeta.Nodes.Count];
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
                        pinOut.ConnectOption(pinIn);

                        EntityConnector connector = nodes[i].Entity.childLinks.FirstOrDefault(o => o.thisParamID == connectionMeta.ParameterGUID && o.linkedParamID == connectionMeta.ConnectedParameterGUID && o.linkedEntityID == connectedNode.ShortGUID);
                        if (connector.ID.IsInvalid)
                        {
                            //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                            //TODO: Do something here.
                            //throw new Exception("Mismatch!");
                            Console.WriteLine("Unexpected extra connection that isn't in PAK!!");
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
                            //TODO: Do something here.
                            throw new Exception("Mismatch!");
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
                //This composite has no defined layouts
                Console.WriteLine("NO DEFINED FLOWGRAPH LAYOUT FOUND! We should never reach this in production code.");

                List<Entity> entities = _composite.GetEntities();
                for (int i = 0; i < entities.Count; i++)
                {
                    if (!entities[i].HasLinks(_composite))
                        continue;

                    STNode mainNode = EntityToNode(entities[i], _composite);

                    for (int x = 0; x < entities[i].childLinks.Count; x++)
                    {
                        Entity childEnt = composite.GetEntityByID(entities[i].childLinks[x].linkedEntityID);
                        if (childEnt == null)
                            continue;

                        STNode childNode = EntityToNode(childEnt, _composite);
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
            }

            foreach (STNode node in stNodeEditor1.Nodes)
                node.Recompute();

            stNodeEditor1.ResumeLayout();
            stNodeEditor1.Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
        }

        private STNode EntityToNode(Entity entity, Composite composite, bool allowDuplicate = false)
        {
            if (entity == null)
                return null;

            STNode node = null;
            if (!allowDuplicate)
            {
                for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
                {
                    if (stNodeEditor1.Nodes[i].ShortGUID != entity.shortGUID)
                        continue;

                    node = stNodeEditor1.Nodes[i];
                    break;
                }
            }

            if (node == null)
            {
                node = new STNode();
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

                node.SetPosition(new Point(0, _spawnOffset));
                _spawnOffset += node.Height + 10;
            }

            return node;
        }

        private void SaveFlowgraph_Click(object sender, EventArgs e)
        {
            FlowgraphManager.SaveLayout(stNodeEditor1, _composite);
        }

        private void DEBUG_CalcPositions_Click(object sender, EventArgs e)
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
                    connectedNode.SetPosition(new Point(10, height));
                    height += connectedNode.Height + 10;
                }
            }
            height = (this.Size.Height / 2) - (outputStackedHeight / 2) - 20;
            foreach (STNodeOption output in origNode.GetOutputOptions()) //outputs on this node
            {
                foreach (STNodeOption input in output.GetConnectedOption()) //connected to input on other node
                {
                    STNode connectedNode = input.Owner;
                    connectedNode.SetPosition(new Point(this.Size.Width - connectedNode.Width - 50, height));
                    height += connectedNode.Height + 10;
                }
            }
            origNode.SetPosition(new Point((this.Size.Width / 2) - (origNode.Width / 2) - 10, (this.Size.Height / 2) - (((outputStackedHeight > inputStackedHeight) ? outputStackedHeight : inputStackedHeight) / 2) - 20));
        }

        private void DEBUG_NextUnfinished_Click(object sender, EventArgs e)
        {
            int index = Commands.Entries.IndexOf(_composite) + 1;
            if (index >= Commands.Entries.Count)
                index = 0;

            while (FlowgraphManager.HasDefinedLayout(Commands.Entries[index]))
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
                if (FlowgraphManager.HasDefinedLayout(Commands.Entries[i]))
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

            STNode node = stNodeEditor1.GetSelectedNode()[0];
            STNode nodeNew = EntityToNode(_composite.GetEntityByID(node.ShortGUID), _composite, true);
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
                if (FlowgraphManager.HasDefinedLayout(Commands.Entries[i]))
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
                if (FlowgraphManager.HasDefinedLayout(Commands.Entries[i]))
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
            DEBUG_LoadAll_Test(_commands);
        }
        public void DEBUG_LoadAll_Test(Commands commands)
        {
            _commands = commands;
            for (int i = 0; i < Commands.Entries.Count; i++)
            {
                ShowComposite(Commands.Entries[i]);
            }
        }

        private void DEBUG_AddPinIn_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
            {
                Console.WriteLine("SELECT ONE NODE");
                return;
            }

            STNode node = stNodeEditor1.GetSelectedNode()[0];

            AddPin pin = new AddPin(node, AddPin.LinkType.IN);
            pin.Show();
        }
        private void DEBUG_AddPinOut_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.GetSelectedNode().Length != 1)
            {
                Console.WriteLine("SELECT ONE NODE");
                return;
            }

            STNode node = stNodeEditor1.GetSelectedNode()[0];

            AddPin pin = new AddPin(node, AddPin.LinkType.OUT);
            pin.Show();
        }

        private void DEBUG_AddNode_Click(object sender, EventArgs e)
        {
            SelectHierarchy selectEnt = new SelectHierarchy(_composite, new Popups.UserControls.CompositeEntityList.DisplayOptions()
            {
                DisplayAliases = true,
                DisplayFunctions = true,
                DisplayProxies = true,
                DisplayVariables = true,
                ShowCheckboxes = false,
            }, false);
            selectEnt.OnFinalEntitySelected += AddNodeCallbackEntitySelected;
            selectEnt.Show();
            //this is where the right click would've happened - store the location
        }
        private void AddNodeCallbackEntitySelected(Entity ent)
        {
            STNode node = EntityToNode(ent, _composite, true);
            //todo: set position where right click happened above
        }

        private void DEBUG_Compile_Click(object sender, EventArgs e)
        {
            //Clear all currently saved connections
            _composite.GetEntities().ForEach(o => o.childLinks.Clear());

            //Re-generate connections using the content in the nodegraph
            for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
            {
                STNode node = stNodeEditor1.Nodes[i];
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
    }
}
