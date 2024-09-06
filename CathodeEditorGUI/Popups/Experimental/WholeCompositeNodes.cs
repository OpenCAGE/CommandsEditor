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
using System.Windows.Input;

namespace CommandsEditor
{
    public partial class WholeCompositeNodes : DockContent
    {
        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        private Composite _composite;
        private int _spawnOffset = 0;

        public WholeCompositeNodes()
        {
            InitializeComponent();
        }

        public void ShowComposite(Composite composite)
        {
            CommandsUtils.PurgeDeadLinks(Content.commands, composite);

            stNodeEditor1.SuspendLayout();
            stNodeEditor1.Nodes.Clear();
            _spawnOffset = 0;

            _composite = composite;
            List<Entity> entities = _composite.GetEntities();
            for (int i = 0; i < entities.Count; i++)
            {
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
                    if (!(stNodeEditor1.Nodes[i] is CathodeNode))
                        continue;

                    CathodeNode thisNode = (CathodeNode)stNodeEditor1.Nodes[i];
                    if (thisNode.ShortGUID != entity.shortGUID)
                        continue;

                    node = thisNode;
                    break;
                }
            }

            if (node == null)
            {
                node = new CathodeNode();
                node.ShortGUID = entity.shortGUID;
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
                        node.SetColour(Color.Red, Color.White);
                        node.SetName(((VariableEntity)entity).name.ToString());
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
            for (int i = 0; i < Content.commands.Entries.Count; i++)
            {
                if (!NodePositionDatabase.CanRestoreFlowgraph(Content.commands.Entries[i].name))
                {
                    ShowComposite(Content.commands.Entries[i]);
                    return;
                }    
            }
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
    }
}
