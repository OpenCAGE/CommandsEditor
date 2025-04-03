using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace CommandsEditor.UnityConnection
{
    public class Recieve : WebSocketBehavior
    {
        public Action OnClientConnect;

        protected override void OnMessage(MessageEventArgs e)
        {
            PacketEvent type = (PacketEvent)Convert.ToInt32(e.Data.Substring(0, 1));
            switch (type)
            {
                default:
                    Console.WriteLine(e.Data.Substring(1));
                    break;
            }
        }

        protected override void OnOpen()
        {
            SendMessage(new Packet(PacketEvent.GENERIC_DATA_SYNC));
            OnClientConnect?.Invoke();
            base.OnOpen();
        }

        public void SendMessage(Packet content)
        {
            base.Send(JsonConvert.SerializeObject(content));
        }
    }
}