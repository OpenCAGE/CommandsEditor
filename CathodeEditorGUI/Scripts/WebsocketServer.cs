using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebsocketServer : WebSocketBehavior
{
    public Action OnLevelLoaded;
    public Action OnClientConnect;

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageType type = (MessageType)Convert.ToInt32(e.Data.Substring(0, 1));
        switch (type)
        {
            case MessageType.REPORT_LOADED_LEVEL:
                OnLevelLoaded?.Invoke();
                break;
            default:
                Console.WriteLine(e.Data.Substring(1));
                break;
        }
    }

    protected override void OnOpen()
    {
        SendMessage(MessageType.SYNC_VERSION, VERSION.ToString());
        OnClientConnect?.Invoke();
        base.OnOpen();
    }

    public void SendMessage(MessageType type, string content)
    {
        Send(((int)type).ToString() + content);
    }

    //TODO: Keep this in sync with clients
    public const int VERSION = 1;
    public enum MessageType
    {
        SYNC_VERSION,

        LOAD_LEVEL,
        LOAD_LEVEL_AT_POSITION,

        GO_TO_POSITION,
        GO_TO_REDS,

        SHOW_ENTITY_NAME,

        REPORT_LOADED_LEVEL,
        REPORTING_LOADED_LEVEL,
    }
}