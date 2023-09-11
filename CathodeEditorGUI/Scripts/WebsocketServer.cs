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

public class WebsocketServer : WebSocketBehavior
{
    public Action OnClientConnect;

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageType type = (MessageType)Convert.ToInt32(e.Data.Substring(0, 1));
        switch (type)
        {
            default:
                Console.WriteLine(e.Data.Substring(1));
                break;
        }
    }

    protected override void OnOpen()
    {
        SendMessage(new WSPacket { type = MessageType.SYNC_VERSION, version = VERSION });
        OnClientConnect?.Invoke();
        base.OnOpen();
    }

    public void SendMessage(WSPacket content)
    {
        base.Send(JsonConvert.SerializeObject(content));
    }

    //TODO: Keep this in sync with clients
    public const int VERSION = 2;
    public enum MessageType
    {
        SYNC_VERSION,

        LOAD_LEVEL,
        LOAD_COMPOSITE,

        GO_TO_POSITION,
        SHOW_ENTITY_NAME,
    }
    public class WSPacket
    {
        public MessageType type;

        public int version;

        public string level_name;
        public string alien_path;

        public System.Numerics.Vector3 position;
        public System.Numerics.Vector3 rotation;

        public string entity_name;

        public string composite_name;
    }
}