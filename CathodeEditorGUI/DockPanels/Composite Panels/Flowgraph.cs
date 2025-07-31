using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.Popups.UserControls;
using OpenCAGE;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;
using static CathodeLib.CompositeFlowgraphTable;
using static CathodeLib.CompositePinInfoTable;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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

        //NOTE: This assumes you've already checked with FlowgraphLayoutManager that LinksMatch!
        public void ShowFlowgraph(Composite composite, FlowgraphMeta flowgraphMeta)
        {
            if (Commands.Utils.PurgeDeadLinks(composite))
                Commands.Utils.PurgedComposites.purged.Add(composite.shortGUID);

            _composite = composite;
            this.Text = flowgraphMeta.Name;
            _flowgraphName = flowgraphMeta.Name;

            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _spawnOffset = 0;

            //Populate nodes for entities
            List<Tuple<Entity, FlowgraphMeta.NodeMeta>> entities = new List<Tuple<Entity, FlowgraphMeta.NodeMeta>>();
            for (int i = 0; i < flowgraphMeta.Nodes.Count; i++)
            {
                Entity entity = composite.GetEntityByID(flowgraphMeta.Nodes[i].EntityGUID);
                if (entity == null)
                    continue; //If an entity doesn't exist, this should've already been deemed acceptable by FlowgraphLayoutManager.
                entities.Add(new Tuple<Entity, FlowgraphMeta.NodeMeta>(entity, flowgraphMeta.Nodes[i]));
            }
            STNode[] nodes = new STNode[entities.Count];
            for (int i = 0; i < entities.Count; i++)
            {
                nodes[i] = EntityToNode(entities[i].Item1);
                nodes[i].SetPosition(entities[i].Item2.Position);
                
                // !!TODO!!
                // This is a temporary solution to position the pins in the right place: add them all, link them up, then remove ones without links.
                // It's REALLY not ideal as it adds a lot of overhead adding and removing things for no reason, but it's the quickest hack fix for now.
                // I should instead check the type of pin it should be when linking, and add it then, like how I add the in/out links for dynamic things (TriggerSeq/CAGEAnim).
                AddAllPins(nodes[i]);

                foreach (ShortGuid pin in entities[i].Item2.PinsIn)
                    nodes[i].AddInputOption(pin);
                foreach (ShortGuid pin in entities[i].Item2.PinsOut)
                    nodes[i].AddOutputOption(pin);

                nodes[i].NodeID = entities[i].Item2.NodeID;
            }

            Console.WriteLine("Loading Flowgraph: " + flowgraphMeta.Name);

            //Populate connections
            for (int i = 0; i < entities.Count; i++)
            {
                foreach (FlowgraphMeta.NodeMeta.ConnectionMeta connectionMeta in entities[i].Item2.Connections)
                {
                    STNode connectedNode = nodes.FirstOrDefault(o => o.NodeID == connectionMeta.ConnectedNodeID && o.ShortGUID == connectionMeta.ConnectedEntityGUID);

                    EntityConnector connector = nodes[i].Entity.childLinks.FirstOrDefault(o => o.thisParamID == connectionMeta.ParameterGUID && o.linkedParamID == connectionMeta.ConnectedParameterGUID && o.linkedEntityID == connectedNode.ShortGUID);
                    if (!connector.ID.IsInvalid) //NOTE: This condition should never fail if the layout has been checked by FlowgraphLayoutManager!
                    {
                        STNodeOption pinOut = nodes[i].GetOption(connectionMeta.ParameterGUID);
                        if (pinOut == null) pinOut = nodes[i].AddOutputOption(connectionMeta.ParameterGUID);
                        STNodeOption pinIn = connectedNode.GetOption(connectionMeta.ConnectedParameterGUID);
                        if (pinIn == null) pinIn = connectedNode.AddInputOption(connectionMeta.ConnectedParameterGUID);

                        ConnectionStatus status = pinOut.ConnectOption(pinIn);
                        if (status != ConnectionStatus.Connected)
                        {
                            //NOTE: We hit this for some in the base game, but it SHOULDN'T be a problem -> links that can't connect won't logically work.
                            Console.WriteLine("WARNING: Could not create the following connection..."); 
                            Console.WriteLine("\t" + nodes[i].Title + " [" + pinOut.Text + "] " + pinOut.Location + " -> " + connectedNode.Title + " [" + pinIn.Text + "] " + pinIn.Location);
                        }
                    }
#if DEBUG
                    else
                    {
                        throw new Exception("Invalid flowgraph layout loaded!!");
                    }
#endif
                }
            }

            //TODO: This is the other half of the temporary hack found above, we now want to remove unconnected pins so our nodes aren't huge.
            for (int i = 0; i < flowgraphMeta.Nodes.Count; i++)
            {
                RemoveUnusedPins(nodes[i]);
            }

            //Correctly respect the scale/position of the saved flowgraph
            stNodeEditor1.ScaleCanvas(flowgraphMeta.CanvasScale, 0, 0);
            stNodeEditor1.MoveCanvas(flowgraphMeta.CanvasPosition.X, flowgraphMeta.CanvasPosition.Y, false, CanvasMoveArgs.All);

            //Recompute all nodes -> this is kinda expensive and not ideal, but I think it's needed to make sure everything draws nicely.
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

        private STNode EntityToNode(Entity entity)
        {
            if (entity == null)
                return null;

            STNode node = new STNode();
            node.Entity = entity;
            RegenerateNodeStyle(node);
            stNodeEditor1.Nodes.Add(node);
            node.SetPosition(new Point(0, _spawnOffset));
            _spawnOffset += node.Height + 10;

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
                    Entity ent = Commands.Utils.ResolveHierarchy(_composite, (node.Entity.variant == EntityVariant.PROXY) ? ((ProxyEntity)node.Entity).proxy.path : ((AliasEntity)node.Entity).alias.path, out Composite c, out string s);
                    node.SetColour(node.Entity.variant == EntityVariant.PROXY ? Color.LightGreen : Color.Orange, node.Entity.variant == EntityVariant.PROXY ? Color.Green : Color.OrangeRed, Color.Black);
                    switch (ent.variant)
                    {
                        case EntityVariant.FUNCTION:
                            FunctionEntity function = (FunctionEntity)ent;
                            if (function.function.IsFunctionType)
                            {
                                node.SetName(Commands.Utils.GetEntityName(c, ent), node.Entity.variant + " TO: " + function.function.AsFunctionType.ToString());
                            }
                            else
                                node.SetName(Commands.Utils.GetEntityName(c, ent), node.Entity.variant + " TO: " + Path.GetFileName(Commands.GetComposite(function.function).name));
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
                        node.SetColour(Color.DodgerBlue, Color.Blue, Color.White);
                        node.SetName(Commands.Utils.GetEntityName(_composite, node.Entity), funcEnt.function.AsFunctionType.ToString());
                    }
                    else
                    {
                        node.SetColour(Color.Blue, Color.DodgerBlue, Color.White);
                        node.SetName(Commands.Utils.GetEntityName(_composite, node.Entity), Path.GetFileName(Commands.GetComposite(funcEnt.function).name));
                    }
                    break;
                case EntityVariant.VARIABLE:
                    VariableEntity varEnt = (VariableEntity)node.Entity;
                    node.SetColour(Color.Red, Color.PaleVioletRed, Color.White);
                    node.SetName(varEnt.name.ToString());
                    AddAllPins(node);
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
                STNode node = EntityToNode(ent[i]);
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
            AddAllPins(node);
        }

        private void AddAllPins(STNode node)
        {
            Entity entity = _composite.GetEntityByID(node.ShortGUID);
            switch (entity.variant)
            {
                case EntityVariant.VARIABLE:
                    VariableEntity varEnt = (VariableEntity)entity;
                    PinInfo info = Commands.Utils.GetPinInfo(_composite, varEnt);
                    switch (info.PinTypeGUID.AsCompositePinType)
                    {
                        case CompositePinType.CompositeInputAnimationInfoVariablePin:
                        case CompositePinType.CompositeInputBoolVariablePin:
                        case CompositePinType.CompositeInputDirectionVariablePin:
                        case CompositePinType.CompositeInputFloatVariablePin:
                        case CompositePinType.CompositeInputIntVariablePin:
                        case CompositePinType.CompositeInputObjectVariablePin:
                        case CompositePinType.CompositeInputPositionVariablePin:
                        case CompositePinType.CompositeInputStringVariablePin:
                        case CompositePinType.CompositeInputVariablePin:
                        case CompositePinType.CompositeInputZoneLinkPtrVariablePin:
                        case CompositePinType.CompositeInputZonePtrVariablePin:
                        case CompositePinType.CompositeInputEnumVariablePin:
                        case CompositePinType.CompositeInputEnumStringVariablePin:
                        case CompositePinType.CompositeOutputAnimationInfoVariablePin:
                        case CompositePinType.CompositeOutputBoolVariablePin:
                        case CompositePinType.CompositeOutputDirectionVariablePin:
                        case CompositePinType.CompositeOutputFloatVariablePin:
                        case CompositePinType.CompositeOutputIntVariablePin:
                        case CompositePinType.CompositeOutputObjectVariablePin:
                        case CompositePinType.CompositeOutputPositionVariablePin:
                        case CompositePinType.CompositeOutputStringVariablePin:
                        case CompositePinType.CompositeOutputVariablePin:
                        case CompositePinType.CompositeOutputZoneLinkPtrVariablePin:
                        case CompositePinType.CompositeOutputZonePtrVariablePin:
                        case CompositePinType.CompositeOutputEnumVariablePin:
                        case CompositePinType.CompositeOutputEnumStringVariablePin:
                            node.AddBottomOption(varEnt.name);
                            break;
                        case CompositePinType.CompositeMethodPin:
                            node.AddOutputOption(varEnt.name);
                            break;
                        case CompositePinType.CompositeTargetPin:
                            node.AddInputOption(varEnt.name);
                            break;
                        case CompositePinType.CompositeReferencePin:
                            node.AddTopOption(varEnt.name, PinStyle.ArrowDown);
                            break;
                    }
                    break;
                default:
                    List<(ShortGuid, ParameterVariant, DataType)> allParameters = Commands.Utils.GetAllParameters(entity, _composite);
                    foreach ((ShortGuid, ParameterVariant, DataType) parameter in allParameters)
                    {
                        //string param = parameter.Item1.ToString();
                        //if (param == "delete_me" || param == "enable" || param == "disable" || param == "position") 
                        //    continue;

                        switch (parameter.Item2)
                        {
                            case ParameterVariant.INPUT_PIN:
                            case ParameterVariant.PARAMETER:
                            case ParameterVariant.STATE_PARAMETER:
                                node.AddTopOption(parameter.Item1, PinStyle.ArrowDown);
                                break;
                            //case ParameterVariant.METHOD_FUNCTION:
                            //    node.AddInputOption(parameter.Item1);
                            //    break;
                            case ParameterVariant.METHOD_PIN:
                                node.AddInputOption(parameter.Item1);
                                ShortGuid relay = Commands.Utils.GetRelay(parameter.Item1);
                                if (relay != ShortGuid.Invalid)
                                    node.AddOutputOption(relay);
                                break;
                            case ParameterVariant.OUTPUT_PIN:
                                node.AddTopOption(parameter.Item1, PinStyle.ArrowUp);
                                break;
                            case ParameterVariant.TARGET_PIN:
                                node.AddOutputOption(parameter.Item1);
                                break;
                            case ParameterVariant.REFERENCE_PIN:
                                node.AddBottomOption(parameter.Item1);
                                break;
                        }

                        //todo: need to include custom ones for triggerseq and cageanim
                    }
                    break;
            }
        }

        private void removeUnusedPinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STNode node = stNodeEditor1.GetHoveredNode();
            RemoveUnusedPins(node);
        }

        private static void RemoveUnusedPins(STNode node)
        {
            STNodeOption[] ins = node.GetInputOptions();
            for (int i = 0; i < ins.Length; i++)
                if (ins[i].ConnectionCount == 0)
                    node.RemoveInputOption(ins[i].ShortGUID);
            STNodeOption[] outs = node.GetOutputOptions();
            for (int i = 0; i < outs.Length; i++)
                if (outs[i].ConnectionCount == 0)
                    node.RemoveOutputOption(outs[i].ShortGUID);
            STNodeOption[] ups = node.GetTopOptions();
            for (int i = 0; i < ups.Length; i++)
                if (ups[i].ConnectionCount == 0)
                    node.RemoveTopOption(ups[i].ShortGUID);
            STNodeOption[] downs = node.GetBottomOptions();
            for (int i = 0; i < downs.Length; i++)
                if (downs[i].ConnectionCount == 0)
                    node.RemoveBottomOption(downs[i].ShortGUID);
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
            STNode duplicated = EntityToNode(node.Entity);

            STNodeOption[] ins = node.GetInputOptions();
            for (int i = 0; i < ins.Length; i++)
                duplicated.AddInputOption(ins[i].ShortGUID);
            STNodeOption[] outs = node.GetOutputOptions();
            for (int i = 0; i < outs.Length; i++)
                duplicated.AddOutputOption(outs[i].ShortGUID);
            STNodeOption[] ups = node.GetTopOptions();
            for (int i = 0; i < ups.Length; i++)
                duplicated.AddTopOption(ups[i].ShortGUID, ups[i].Style);
            STNodeOption[] downs = node.GetBottomOptions();
            for (int i = 0; i < downs.Length; i++)
                duplicated.AddBottomOption(downs[i].ShortGUID);

            duplicated.SetPosition(new Point(node.Location.X + 15, node.Location.Y + 15));

            return duplicated;
        }

        private void AddConnection(STNode node)
        {

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
