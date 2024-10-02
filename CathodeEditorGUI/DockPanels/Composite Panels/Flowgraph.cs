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

        public Flowgraph()
        {
            InitializeComponent();
            this.FormClosed += EntityFlowgraph_FormClosed; ;

            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
            stNodeEditor1.AllowSameOwnerConnections = true;
            stNodeEditor1.SelectedChanged += Owner_SelectedChanged;

#if !DEBUG
            SaveFlowgraph.Visible = false;
            AutoCalc.Visible = false;
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

        //TODO: NEEDS REWORKING INTO "SHOW FLOWGRAPH" WHEN ALL LAYOUTS ARE DEFINED.
        //      THIS FUNCTION SHOULD BE PASSED THE FLOWGRAPHMETA OBJECT.
        public void ShowComposite(Composite composite)
        {
            Console.WriteLine("EntityFlowgraph::ShowComposite - " + composite.name);

            if (CommandsUtils.PurgeDeadLinks(Commands, composite))
                CommandsUtils.PurgedComposites.purged.Add(composite.shortGUID);

            _composite = composite;
            this.Text = "Flowgraph: " + _composite.name;

            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _spawnOffset = 0;

            //TODO: When I've fully populated the layout manager, this should be reworked. We should be passed the FlowgraphMeta object instead of the Composite.
            List<FlowgraphMeta> flowgraphMetas = FlowgraphLayoutManager.GetLayouts(composite);
            if (flowgraphMetas.Count > 1)
            {
                string breakhere = "";
                Console.WriteLine("ERROR! WE SHOULD NOT HAVE MORE THAN ONE LAYOUT DEFINITION YET!");
            }
            if (flowgraphMetas.Count > 0)
            {
                FlowgraphMeta flowgraphMeta = flowgraphMetas[0];
                this.Text = "Flowgraph: " + flowgraphMeta.Name;

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
                        ConnectionStatus status = pinOut.ConnectOption(pinIn);

                        if (status != ConnectionStatus.Connected)
                        {
                            Console.WriteLine("WARNING! Could not create connection!");
                        }

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
                            //throw new Exception("Mismatch!");
                        }
                    }

                    if (!populatedEntities.Contains(entity))
                    {
                        //Our composite mismatches the flowgraph layout, the user must have modified the content with an older version of the script editor.
                        //TODO: Do something here.
                        //throw new Exception("Mismatch!");
                        Console.WriteLine("missing entity with links");
                    }
                }

                //Correctly respect the scale/position of the saved flowgraph
                stNodeEditor1.ScaleCanvas(flowgraphMeta.CanvasScale, 0, 0);
                stNodeEditor1.MoveCanvas(flowgraphMeta.CanvasPosition.X, flowgraphMeta.CanvasPosition.Y, false, CanvasMoveArgs.All);
            }
            else
            {
                //This composite has no defined layouts. We won't get here in the final build. This function will be refactored after the layout db is populated.
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
                        ConnectionStatus status = linkIn.ConnectOption(linkOut);
                        if (status != ConnectionStatus.Connected)
                        {
                            Console.WriteLine("WARNING! Could not create connection!");
                        }
                    }
                }

                this.Text = "UNSAVED with " + stNodeEditor1.Nodes.Count + " nodes";
            }

            foreach (STNode node in stNodeEditor1.Nodes)
                node.Recompute();

            //stack nodes nicely when we don't have a layout
            if (flowgraphMetas.Count == 0)
            {
                int height = 10;
                foreach (STNode node in stNodeEditor1.Nodes)
                {
                    node.SetPosition(new Point(0, height));
                    height += node.Height + 10;
                }
            }

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

            //TODO: once the layout db is fully populated, this "allowDuplicate" thing can be removed as we will never not want duplicates enabled
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
                                    node.SetName(EntityUtils.GetName(c, ent), entity.variant + " TO: " + CommandsUtils.GetFunctionType(function.function).ToString());
                                }
                                else
                                    node.SetName(EntityUtils.GetName(c, ent) , entity.variant + " TO: " + Path.GetFileName(Commands.GetComposite(function.function).name));
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
                            node.SetName(EntityUtils.GetName(composite, entity), CommandsUtils.GetFunctionType(funcEnt.function).ToString());
                        }
                        else
                        {
                            node.SetColour(Color.Blue, Color.White);
                            node.SetName(EntityUtils.GetName(composite, entity), Path.GetFileName(Commands.GetComposite(funcEnt.function).name));
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

        //Saves the Flowgraph's layout, and compiles the links back to commands
        //NOTE: This assumes that you have already cleared all childLinks in the composite already. That can be done by using CompositeUtils.ClearAllLinks
        public void SaveAndCompile()
        {
            //TODO: when the layout db is completely populated, we should change this to reflect the name of the loaded flowgraph from the FlowgraphMeta object that got passed.
            FlowgraphMeta layout = FlowgraphLayoutManager.SaveLayout(stNodeEditor1, _composite, Path.GetFileName(_composite.name));
            Console.WriteLine("Saved flowgraph layout: " + layout.Name);

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

        #region Test Crap That Can Be Deleted Later

        private void SaveFlowgraph_Click(object sender, EventArgs e)
        {
            var layouts = FlowgraphLayoutManager.GetLayouts(_composite);
            if (layouts.Count > 1)
            {
                MessageBox.Show("NO! You cannot use this button here without being destructive. This button assumes there are zero or one flowgraphs, but this composite already has more than that.");
                string breakhere = "";
            }

            FlowgraphLayoutManager.DEBUG_UsePreDefinedTable = true;
            CompositeUtils.ClearAllLinks(_composite);
            SaveAndCompile();
            FlowgraphLayoutManager.DEBUG_UsePreDefinedTable = false;

            Singleton.Editor.CommandsDisplay.DEBUG_LoadNextToConstruct();
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

        public void DEBUG_LoadAll_Test(Commands commands)
        {
            _commands = commands;
            for (int i = 0; i < Commands.Entries.Count; i++)
            {
                ShowComposite(Commands.Entries[i]);
            }
        }
        #endregion

        //disable entity-related actions on the context menu if no entity is selected
        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            STNode node = stNodeEditor1.GetHoveredNode();

            addPinInToolStripMenuItem.Visible = node != null;
            addPinOutToolStripMenuItem.Visible = node != null;
            toolStripSeparator1.Visible = node != null;
            removePinInToolStripMenuItem.Visible = node != null;
            removePinOutToolStripMenuItem.Visible = node != null;
            modifyPinsIn.Visible = node != null;
            modifyPinsOut.Visible = node != null;
            toolStripSeparator2.Visible = node != null;
            deleteToolStripMenuItem.Visible = node != null;
            duplicateToolStripMenuItem.Visible = node != null;

            if (node != null)
            {
                addPinInToolStripMenuItem.Enabled = node.Entity.variant != EntityVariant.VARIABLE;
                addPinOutToolStripMenuItem.Enabled = node.Entity.variant != EntityVariant.VARIABLE;
                removePinInToolStripMenuItem.Enabled = node.Entity.variant != EntityVariant.VARIABLE;
                removePinOutToolStripMenuItem.Enabled = node.Entity.variant != EntityVariant.VARIABLE;
                modifyPinsIn.Enabled = node.Entity.variant != EntityVariant.VARIABLE;
                modifyPinsOut.Enabled = node.Entity.variant != EntityVariant.VARIABLE;

                addPinInToolStripMenuItem.Visible = SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator);
                addPinOutToolStripMenuItem.Visible = SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator);
                toolStripSeparator1.Visible = SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator);
                removePinInToolStripMenuItem.Visible = SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator);
                removePinOutToolStripMenuItem.Visible = SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator);
                modifyPinsIn.Visible = !SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator);
                modifyPinsOut.Visible = !SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator);
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
                ShowCheckboxes = false,
            }, false);
            selectEnt.OnFinalEntitySelected += AddNodeCallbackEntitySelected;
            selectEnt.Show();
            _nodeSpawnPosition = new Point((int)stNodeEditor1.MousePositionInCanvas.X, (int)stNodeEditor1.MousePositionInCanvas.Y);
        }
        private void AddNodeCallbackEntitySelected(Entity ent)
        {
            STNode node = EntityToNode(ent, _composite, true);
            node.SetPosition(_nodeSpawnPosition);
        }

        AddPin _pinManager = null;
        private void PinManager(AddPin.Mode mode)
        {
            STNode node = stNodeEditor1.GetHoveredNode();

            if (_pinManager != null)
            {
                //_pinManager.OnSaved -= Reload;
                _pinManager.Close();
            }

            if (SettingsManager.GetBool(Singleton.Settings.UseLegacyParamCreator))
                _pinManager = new AddPin_Custom(node, mode);
            else
                _pinManager = new AddPin_PreDefined(node, mode);

            _pinManager.Show();
            //_pinManager.OnSaved += Reload;
        }

        //Add pin to right clicked node
        private void addPinInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PinManager(AddPin.Mode.ADD_IN);
        }
        private void addPinOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PinManager(AddPin.Mode.ADD_OUT);
        }

        //Remove pin from right clicked node
        private void removePinInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PinManager(AddPin.Mode.REMOVE_IN);
        }
        private void removePinOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PinManager(AddPin.Mode.REMOVE_OUT);
        }

        //Add/remove pins in/out using new method
        private void modifyPinsIn_Click(object sender, EventArgs e)
        {
            PinManager(AddPin.Mode.ADD_IN);
        }
        private void modifyPinsOut_Click(object sender, EventArgs e)
        {
            PinManager(AddPin.Mode.ADD_OUT);
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

            STNode duplicated = EntityToNode(node.Entity, _composite, true);

            STNodeOption[] ins = node.GetInputOptions();
            for (int i = 0; i < ins.Length; i++)
                duplicated.AddInputOption(ins[i].ShortGUID);
            STNodeOption[] outs = node.GetOutputOptions();
            for (int i = 0; i < outs.Length; i++)
                duplicated.AddOutputOption(outs[i].ShortGUID);

            duplicated.SetPosition(new Point(node.Location.X + 15, node.Location.Y + 15));
        }
    }
}
