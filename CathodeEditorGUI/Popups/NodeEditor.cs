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

namespace CommandsEditor
{
    public partial class NodeEditor : DockContent
    {
        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        private Entity ActiveEntity => Singleton.Editor.ActiveCompositeDisplay?.ActiveEntityDisplay?.Entity;
        private Composite ActiveComposite => Singleton.Editor.ActiveCompositeDisplay?.ActiveEntityDisplay?.Composite;

        private bool IsAutoHide => DockState == DockState.DockBottomAutoHide || DockState == DockState.DockLeftAutoHide || DockState == DockState.DockRightAutoHide || DockState == DockState.DockTopAutoHide;

        private bool _wasVisibleLastTime = false;
        private DockState _previousDockState = DockState.Unknown;

        private const int _defaultWidth = 600;
        private const int _defaultHeight = 500;

        public NodeEditor()
        {
            InitializeComponent();

            this.FormClosed += NodeEditor_FormClosed;

            this.Shown += NodeEditor_Shown;
            this.DockStateChanged += OnDockStateChanged;
            this.ResizeEnd += OnResized;

            Singleton.OnEntitySelected += UpdateEntities;
            Singleton.OnEntityReloaded += UpdateEntities;
            Singleton.OnLevelLoaded += delegate (LevelContent c) { UpdateEntities(); };
        }

        public void ResetLayout()
        {
            Width = _defaultWidth;
            Height = _defaultHeight;
        }

        private void NodeEditor_Shown(object sender, EventArgs e)
        {
            DockPanel.ActiveAutoHideContentChanged += OnDockActivenessChanged;
            DockPanel.ActiveContentChanged += OnDockActivenessChanged;

            if (DockState == DockState.Float)
            {
                Width = SettingsManager.GetInteger(Singleton.Settings.NodegraphWidth, _defaultWidth);
                Height = SettingsManager.GetInteger(Singleton.Settings.NodegraphHeight, _defaultHeight);
            }

            UpdateEntities();
        }

        private void NodeEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            DockPanel.ActiveAutoHideContentChanged -= OnDockActivenessChanged;
            DockPanel.ActiveContentChanged -= OnDockActivenessChanged;

            Singleton.OnEntitySelected -= UpdateEntities;
            Singleton.OnEntityReloaded -= UpdateEntities;

            if (DockState == DockState.Float)
            {
                SettingsManager.SetInteger(Singleton.Settings.NodegraphWidth, Width);
                SettingsManager.SetInteger(Singleton.Settings.NodegraphHeight, Height);
            }
        }

        private void OnDockStateChanged(object sender, EventArgs e)
        {
            if (DockState == DockState.Unknown || DockState == DockState.Hidden)
                return;

            if (DockState == _previousDockState) return;
            _previousDockState = DockState;

            SettingsManager.SetString(Singleton.Settings.NodegraphState, DockState.ToString());

            UpdateEntities();
        }

        private void OnResized(object sender, EventArgs e)
        {
            UpdateEntities();
        }

        private void OnDockActivenessChanged(object sender, EventArgs e)
        {
            //If we're invisible - log that & exit early
            if (IsAutoHide && !this.IsActivated)
            {
                _wasVisibleLastTime = false;
                return;
            }

            //If we're visible & were visible last time, also exit early
            if (_wasVisibleLastTime)
                return;

            //In this case, we should update our entities
            _wasVisibleLastTime = true;
            UpdateEntities();
        }

        private void UpdateEntities(Entity e = null)
        {
            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();

            if (ActiveComposite == null || ActiveEntity == null)
                return;

            Console.WriteLine("NODEGRAPH: Loading entities...");
            _previouslySelectedEntity = ActiveEntity;

            CathodeNode mainNode = EntityToNode(ActiveEntity, ActiveComposite);
            stNodeEditor1.Nodes.Add(mainNode);

            //Generate input nodes
            List<Entity> ents = ActiveComposite.GetEntities();
            List<CathodeNode> inputNodes = new List<CathodeNode>();
            foreach (Entity ent in ents)
            {
                foreach (EntityConnector link in ent.childLinks)
                {
                    if (link.linkedEntityID != ActiveEntity.shortGUID) continue;
                    CathodeNode node = null;
                    for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
                    {
                        if (((CathodeNode)stNodeEditor1.Nodes[i]).ID == ent.shortGUID)
                        {
                            node = (CathodeNode)stNodeEditor1.Nodes[i];
                            break;
                        }
                    }
                    if (node == null)
                    {
                        node = EntityToNode(ent, ActiveComposite);
                        inputNodes.Add(node);
                        stNodeEditor1.Nodes.Add(node);
                    }
                    STNodeOption opt1 = node.AddOutputOption(link.thisParamID.ToString());
                    STNodeOption opt2 = mainNode.AddInputOption(link.linkedParamID.ToString());
                    opt1.ConnectOption(opt2);
                }
            }

            //Generate output nodes
            List<CathodeNode> outputNodes = new List<CathodeNode>();
            foreach (EntityConnector link in ActiveEntity.childLinks)
            {
                CathodeNode node = null;
                for (int i = 0; i < stNodeEditor1.Nodes.Count; i++)
                {
                    if (((CathodeNode)stNodeEditor1.Nodes[i]).ID == link.linkedEntityID)
                    {
                        node = (CathodeNode)stNodeEditor1.Nodes[i];
                        break;
                    }
                }
                if (node == null)
                {
                    node = EntityToNode(ActiveComposite.GetEntityByID(link.linkedEntityID), ActiveComposite);
                    if (node != null)
                    {
                        outputNodes.Add(node);
                        stNodeEditor1.Nodes.Add(node);
                    }
                }
                STNodeOption opt1 = node?.AddInputOption(link.linkedParamID.ToString());
                STNodeOption opt2 = mainNode.AddOutputOption(link.thisParamID.ToString());
                opt1.ConnectOption(opt2);
            }

            //Compute node sizes
            foreach (STNode node in stNodeEditor1.Nodes)
                ((CathodeNode)node).Recompute();
            int inputStackedHeight = 0;
            foreach (STNode node in inputNodes)
                inputStackedHeight += node.Height + 10;
            int outputStackedHeight = 0;
            foreach (STNode node in outputNodes)
                outputStackedHeight += node.Height + 10;

            //Set node positions
            int height = (this.Size.Height / 2) - (inputStackedHeight / 2) - 20;
            foreach (STNode node in inputNodes)
            {
                ((CathodeNode)node).SetPosition(new Point(10, height));
                height += node.Height + 10;
            }
            height = (this.Size.Height / 2) - (outputStackedHeight / 2) - 20;
            foreach (STNode node in outputNodes)
            {
                ((CathodeNode)node).SetPosition(new Point(this.Size.Width - node.Width - 50, height));
                height += node.Height + 10;
            }
            mainNode.SetPosition(new Point((this.Size.Width / 2) - (mainNode.Width / 2) - 10, (this.Size.Height / 2) - (((outputStackedHeight > inputStackedHeight) ? outputStackedHeight : inputStackedHeight) / 2) - 20));

            //Lock options for now
            foreach (STNode node in stNodeEditor1.Nodes)
                node.LockOption = true;

            stNodeEditor1.ResumeLayout();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
            stNodeEditor1.SelectedChanged += Owner_SelectedChanged;

            //stNodeEditor1.OptionConnected += (s, ea) => stNodeEditor1.ShowAlert(ea.Status.ToString(), Color.White, ea.Status == ConnectionStatus.Connected ? Color.FromArgb(125, Color.Green) : Color.FromArgb(125, Color.Red));
            //stNodeEditor1.CanvasScaled += (s, ea) => stNodeEditor1.ShowAlert(stNodeEditor1.CanvasScale.ToString("F2"), Color.White, Color.FromArgb(125, Color.Yellow));
            //stNodeEditor1.NodeAdded += (s, ea) => ea.Node.ContextMenuStrip = contextMenuStrip1;

            //contextMenuStrip1.ShowImageMargin = false;
            //contextMenuStrip1.Renderer = new ToolStripRendererEx();
        }

        private Entity _previouslySelectedEntity = null;
        private void Owner_SelectedChanged(object sender, EventArgs e)
        {
            if (!SettingsManager.GetBool(Singleton.Settings.OpenEntityFromNode)) 
                return;

            //when a node is selected, load it in the commands editor
            STNode[] nodes = stNodeEditor1.GetSelectedNode();
            if (nodes.Length != 1) return;

            Entity ent = Singleton.Editor.ActiveCompositeDisplay?.Composite?.GetEntityByID(((CathodeNode)nodes[0]).ID);
            if (ent == _previouslySelectedEntity) return;
            _previouslySelectedEntity = ent;

            Singleton.Editor.ActiveCompositeDisplay?.LoadEntity(ent);
            Singleton.OnEntitySelected?.Invoke(ent); //need to call this again b/c the activation event doesn't fire here
        }

        private CathodeNode EntityToNode(Entity entity, Composite composite)
        {
            if (entity == null)
                return null;

            CathodeNode node = new CathodeNode();
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
                                node.SetName(EntityUtils.GetName(c, ent), entity.variant + " TO: " + function.function.ToString());
                            }
                            else
                                node.SetName(EntityUtils.GetName(c, ent), entity.variant + " TO: " + Content.commands.GetComposite(function.function).name);
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
                        node.SetName(EntityUtils.GetName(composite, entity), Content.commands.GetComposite(funcEnt.function).name);
                    }
                    break;
                case EntityVariant.VARIABLE:
                    node.SetColour(Color.Red, Color.White);
                    node.SetName(((VariableEntity)entity).name.ToString());
                    break;
            }
            node.Recompute();
            return node;
        }
    }
}
