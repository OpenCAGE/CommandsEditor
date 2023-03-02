﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebsocketServer : WebSocketBehavior
{
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

    public void SendMessage(MessageType type, string content)
    {
        Send(((int)type).ToString() + content);
    }
}

//TODO: Keep this in sync with clients
public enum MessageType
{
    TEST,

    LOAD_LEVEL,
    LOAD_LEVEL_AT_POSITION,

    GO_TO_POSITION,
    GO_TO_REDS,

    REPORT_LOADED_LEVEL,
    REPORTING_LOADED_LEVEL,
}