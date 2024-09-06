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
        private List<CathodeNode> _nodes = new List<CathodeNode>();

        //note: there's a render error until you grab all nodes and move them u get a point in space sometimes. fixed by dumb auto pos?

        public WholeCompositeNodes()
        {
            InitializeComponent();
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
                CathodeNode mainNode = EntityToNode(entities[i], _composite);

                for (int x = 0;x < entities[i].childLinks.Count; x++)
                {
                    Entity childEnt = composite.GetEntityByID(entities[i].childLinks[x].linkedEntityID);
                    if (childEnt == null)
                        continue;

                    CathodeNode childNode = EntityToNode(childEnt, _composite);
                    STNodeOption linkIn = childNode.AddInputOption(entities[i].childLinks[x].linkedParamID.ToString());
                    STNodeOption linkOut = mainNode.AddOutputOption(entities[i].childLinks[x].thisParamID.ToString());
                    linkIn.ConnectOption(linkOut);
                }
            }

            // TODO: on each entity do dumb attempt at positioning to make it quicker to manualyl position.

            this.Text = _composite.name;
            NodePositionDatabase.TryRestoreFlowgraph(_composite.name, stNodeEditor1);

            //Lock options for now
            foreach (STNode node in stNodeEditor1.Nodes)
                node.LockOption = true;

            stNodeEditor1.ResumeLayout();
            stNodeEditor1.Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            stNodeEditor1.LoadAssembly(Application.ExecutablePath);
        }

        private CathodeNode EntityToNode(Entity entity, Composite composite)
        {
            if (entity == null)
                return null;

            CathodeNode node = _nodes.FirstOrDefault(o => o.ID == entity.shortGUID);

            if (node == null)
            {
                node = new CathodeNode();
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
                _nodes.Add(node);
                stNodeEditor1.Nodes.Add(node);
            }

            return node;
        }

        private void SaveFlowgraph_Click(object sender, EventArgs e)
        {
            NodePositionDatabase.SaveFlowgraph(_composite.name, stNodeEditor1);
        }
    }
}
