﻿using System;
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
#if DEBUG
using CommandsEditor.Nodes;
#endif

namespace CommandsEditor
{
    public partial class NodeEditorTest : BaseWindow
    {
        public NodeEditorTest() : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            stNodePropertyGrid1.Text = "Node_Property";
            stNodeTreeView1.LoadAssembly(Application.ExecutablePath);
            stNodeEditor1.LoadAssembly(Application.ExecutablePath);

            stNodeEditor1.ActiveChanged += (s, ea) => stNodePropertyGrid1.SetNode(stNodeEditor1.ActiveNode);
            //stNodeEditor1.SelectedChanged += (s, ea) => stNodePropertyGrid1.SetSTNode(stNodeEditor1.ActiveNode);
            stNodeEditor1.OptionConnected += (s, ea) => stNodeEditor1.ShowAlert(ea.Status.ToString(), Color.White, ea.Status == ConnectionStatus.Connected ? Color.FromArgb(125, Color.Green) : Color.FromArgb(125, Color.Red));
            stNodeEditor1.CanvasZoomed += (s, ea) => stNodeEditor1.ShowAlert(stNodeEditor1.CanvasScale.ToString("F2"), Color.White, Color.FromArgb(125, Color.Yellow));
            stNodeEditor1.NodeAdded += (s, ea) => ea.Node.ContextMenuStrip = contextMenuStrip1;

            stNodePropertyGrid1.SetInfoKey("Author", "Mail", "Link", "Show Help");
            stNodeTreeView1.PropertyGrid.SetInfoKey("Author", "Mail", "Link", "Show Help");

            stNodeEditor1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            stNodeTreeView1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
            stNodePropertyGrid1.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.Renderer = new ToolStripRendererEx();
        }

        private void Form1_Load(object sender, EventArgs e) {
            //int nLines = 0;
            //foreach (var v in Directory.GetFiles("../../../", "*.cs", SearchOption.AllDirectories)) {
            //    nLines += File.ReadAllLines(v).Length;
            //}
            //MessageBox.Show(nLines.ToString());
            //this.Resize += (s, ea) => this.Text = this.Size.ToString();
            //this.BeginInvoke(new MethodInvoker(() => {
            //    //this.Size = new Size(488, 306);
            //    this.Size = new Size(488, 246);
            //    stNodeTreeView1.Visible = false;
            //    stNodePropertyGrid1.Top = stNodeEditor1.Top;
            //    stNodePropertyGrid1.Height = stNodeEditor1.Height;
            //    stNodeTreeView1.Height = stNodeEditor1.Height;
            //}));


//TODO: loop child & parent entities, and spawn with links

#if DEBUG
            stNodeEditor1.Nodes.Add(new TerminalContent());
            stNodeEditor1.Nodes.Add(new AchievementMonitor());

            CompositeInterface compNode = new CompositeInterface();
            compNode.AddOptions(new string[] { "test1", "test2" }, new string[] { "test1o", "test2o" });
            stNodeEditor1.Nodes.Add(compNode);

            STNodeCollection nodes = stNodeEditor1.Nodes;

            STNode node1 = nodes[0];
            STNodeOption[] node1Options = node1.GetOutputOptions();
            STNodeOption node1Option = node1Options.FirstOrDefault(o => o.Text == "selected");

            STNode node2 = nodes[1];
            STNodeOption[] node2Options = node2.GetInputOptions();
            STNodeOption node2Option = node2Options.FirstOrDefault(o => o.Text == "apply_start");

            node1Option.ConnectOption(node2Option);
#endif
        }

        private void btn_open_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.stn|*.stn";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            stNodeEditor1.Nodes.Clear();
            stNodeEditor1.LoadCanvas(ofd.FileName);
        }

        private void btn_save_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.stn|*.stn";
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            stNodeEditor1.SaveCanvas(sfd.FileName);
        }

        private void lockConnectionToolStripMenuItem_Click(object sender, EventArgs e) {
            stNodeEditor1.ActiveNode.LockOption = !stNodeEditor1.ActiveNode.LockOption;
        }

        private void lockLocationToolStripMenuItem_Click(object sender, EventArgs e) {
            if (stNodeEditor1.ActiveNode == null) return;
            stNodeEditor1.ActiveNode.LockLocation = !stNodeEditor1.ActiveNode.LockLocation;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            if (stNodeEditor1.ActiveNode == null) return;
            stNodeEditor1.Nodes.Remove(stNodeEditor1.ActiveNode);
        }
    }
}
