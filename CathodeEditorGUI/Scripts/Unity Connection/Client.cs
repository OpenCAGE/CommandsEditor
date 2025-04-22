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
    public class Client : WebSocketBehavior
    {
        public Action OnConnect;
        public Action OnDisconnect;

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
            Console.WriteLine("[WEBSOCKET] Client connected");
            SendMessage(new Packet(PacketEvent.GENERIC_DATA_SYNC));
            OnConnect?.Invoke();
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("[WEBSOCKET] Client disconnected");
            OnDisconnect?.Invoke();
            base.OnClose(e);
        }

        public void SendMessage(Packet content)
        {
#if DEBUG
            Console.WriteLine("[WEBSOCKET] Sending " + content.packet_event + " data");
#endif
            base.Send(JsonConvert.SerializeObject(content));
        }
    }
}