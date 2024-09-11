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

namespace CommandsEditor
{
    public partial class EntityFlowgraph : DockContent
    {
        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        private Composite _composite;
        private int _spawnOffset = 0;

        public EntityFlowgraph()
        {
            InitializeComponent();
            this.FormClosed += EntityFlowgraph_FormClosed; ;

            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
            stNodeEditor1.SelectedChanged += Owner_SelectedChanged;

#if !DEBUG
            DEBUG_CalcPositions.Visible = false;
            DEBUG_DumpUnfinished.Visible = false;
            DEBUG_Duplicate.Visible = false;
            DEBUG_NextAndSave.Visible = false;
            DEBUG_NextUnfinished.Visible = false;
            DEBUG_SaveAllNoLinks.Visible = false;
            DEBUG_Next1Link.Visible = false;
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

            Entity ent = Singleton.Editor.CommandsDisplay?.CompositeDisplay?.Composite?.GetEntityByID(((CathodeNode)nodes[0]).ShortGUID);
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

            List<Entity> purged = CommandsUtils.PurgeDeadLinks(Content.commands, composite);
            for (int i = 0; i < purged.Count; i++)
                Singleton.OnEntityDeleted(purged[i]);

            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _spawnOffset = 0;

            _composite = composite;
            List<Entity> entities = _composite.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                //TODO: do we want to only draw nodes with child connections? if not, we want to add a UI for adding entity nodes as well as the connections (drag from entity list?)

                CathodeNode mainNode = EntityToNode(entities[i], _composite);

                for (int x = 0;x < entities[i].childLinks.Count; x++)
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
            }

            this.Text = _composite.name;
            NodePositionDatabase.TryRestoreFlowgraph(_composite.name, stNodeEditor1);

            foreach (STNode node in stNodeEditor1.Nodes)
                ((CathodeNode)node).Recompute();

            //Lock options for now
            //foreach (STNode node in stNodeEditor1.Nodes)
            //    node.LockOption = true;

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
                                    node.SetName(EntityUtils.GetName(c, ent) , entity.variant + " TO: " + Content.commands.GetComposite(function.function).name);
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
            int index = Content.commands.Entries.IndexOf(_composite) + 1;
            if (index >= Content.commands.Entries.Count)
                index = 0;
            
            while (NodePositionDatabase.CanRestoreFlowgraph(Content.commands.Entries[index].name))
            {
                if (index + 1 >= Content.commands.Entries.Count)
                    index = 0;
                else
                    index += 1;
            }
            ShowComposite(Content.commands.Entries[index]);
        }

        private void DEBUG_DumpUnfinished_Click(object sender, EventArgs e)
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("Incomplete Flowgraphs:");
            int count = 0;
            for (int i = 0; i < Content.commands.Entries.Count; i++)
            {
                if (NodePositionDatabase.CanRestoreFlowgraph(Content.commands.Entries[i].name))
                    continue;

                Console.WriteLine(" - " + Content.commands.Entries[i].name);
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
            for (int i = 0; i < Content.commands.Entries.Count; i++)
            {
                if (NodePositionDatabase.CanRestoreFlowgraph(Content.commands.Entries[i].name))
                    continue;

                bool noLinks = true;
                List<Entity> entities = Content.commands.Entries[i].GetEntities();
                for (int x = 0; x < entities.Count; x++)
                {
                    List<EntityConnector> linksIn = entities[x].GetParentLinks(Content.commands.Entries[i]);
                    List<EntityConnector> linksOut = entities[x].childLinks;

                    if (linksIn.Count != 0 || linksOut.Count != 0)
                    {
                        noLinks = false;
                        break;
                    }
                }

                if (!noLinks)
                    continue;

                ShowComposite(Content.commands.Entries[i]);
                SaveFlowgraph_Click(null, null);
            }
        }

        private void DEBUG_Next1Link_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Content.commands.Entries.Count; i++)
            {
                if (NodePositionDatabase.CanRestoreFlowgraph(Content.commands.Entries[i].name))
                    continue;

                bool shouldGenerate = false;
                Entity entWithLinksInAndOut = null;
                List<Entity> entities = Content.commands.Entries[i].GetEntities();
                for (int x = 0; x < entities.Count; x++)
                {
                    List<EntityConnector> linksIn = entities[x].GetParentLinks(Content.commands.Entries[i]);
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
                    ShowComposite(Content.commands.Entries[i]);
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
    }
}
