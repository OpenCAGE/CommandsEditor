using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace CathodeEditorGUI
{
    public partial class client_test : Form
    {
        private WebSocket client;

        public client_test()
        {
            InitializeComponent();
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            this.Invoke((MethodInvoker)(() => listBox1.Items.Add(e.Data)));
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            this.Invoke((MethodInvoker)(() => listBox1.Items.Add("LOG: CONNECTION CLOSE")));
        }

        private void Client_OnOpen(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)(() => listBox1.Items.Add("LOG: CONNECTION OPEN")));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }

            client = new WebSocket("ws://localhost:1702/commands_editor");
            client.OnMessage += OnMessage;
            client.OnOpen += Client_OnOpen;
            client.OnClose += OnClose;
            client.Connect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Send(textBox1.Text);
        }
    }
}
