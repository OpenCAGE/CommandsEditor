using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using Newtonsoft.Json;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp.Server;

namespace CommandsEditor.UnityConnection
{
    public static class Send
    {
        private static WebSocketServer _server;
        private static Recieve _serverLogic;

        public static bool Started => _server != null;
        public static bool Connected => _server != null && _server.WebSocketServices["/commands_editor"].Sessions.Count != 0;

        static Send()
        {
            //TODO: This "dirty" logic should be split out to another class, and the editor UI should reflect if changes are made but it hasn't been saved.

            Singleton.OnLevelLoaded += LevelLoaded;
            Singleton.OnCompositeSelected += CompositeSelected;
            //Singleton.OnCompositeDeleted; <- todo
            Singleton.OnEntitySelected += EntitySelected;
            Singleton.OnEntityMoved += EntityMoved;
            Singleton.OnEntityAdded += EntityAdded;
            Singleton.OnEntityDeleted += EntityDeleted;
        }

        public static bool Start()
        {
            Stop();

            try
            {
                _server = new WebSocketServer("ws://localhost:1702");
                _server.AddWebSocketService<Recieve>("/commands_editor", (server) =>
                {
                    _serverLogic = server;
                    _serverLogic.OnClientConnect += SyncClient;
                });
                _server.Start();
                return true;
            }
            catch
            {
                _server = null;
                return false;
            }
        }

        public static void Stop()
        {
            if (_server != null)
                _server.Stop();
            _server = null;
        }
        
        /* A level has just been loaded -> load its data in Unity */
        private static void LevelLoaded(LevelContent content)
        {
            SendData(GeneratePacket(PacketEvent.LEVEL_LOADED));
        }

        /* A composite has been loaded -> open it in the Unity scene */
        private static void CompositeSelected(Composite composite)
        {
            SendData(GeneratePacket(PacketEvent.COMPOSITE_SELECTED));

            //todo: need to unsub from old path when previous ui is closed
            Singleton.Editor.CommandsDisplay.CompositeDisplay.Path.OnSteppedForwards += CompositePathStepped;
            Singleton.Editor.CommandsDisplay.CompositeDisplay.Path.OnSteppedBackwards += CompositePathStepped;
        }
        private static void CompositePathStepped()
        {
            //todo: contextual entities should highlight
            SendData(GeneratePacket(PacketEvent.COMPOSITE_PATH_STEPPED));
        }

        /* Entity lifetime events -> sync them to Unity */
        private static void EntitySelected(Entity entity)
        {
            SendData(GeneratePacket(PacketEvent.ENTITY_SELECTED));
        }
        private static void EntityMoved(cTransform transform, Entity entity)
        {
            Packet p = GeneratePacket(PacketEvent.ENTITY_MOVED);
            p.position = transform.position;
            p.rotation = transform.rotation;
            SendData(p);
        }
        private static void EntityAdded(Entity entity)
        {
            SendData(GeneratePacket(PacketEvent.ENTITY_ADDED));
        }
        private static void EntityDeleted(Entity entity)
        {
            SendData(GeneratePacket(PacketEvent.ENTITY_DELETED));
        }

        /* Re-sync a new client with all current info */
        private static void SyncClient()
        {
            if (DirtyTracker.IsDirty)
            {
                //TODO: Warn that there's likely going to be a mismatch between client and server.
            }

            SendData(GeneratePacket());
        }

        /* Create a Packet object containing useful metadata */
        private static Packet GeneratePacket(PacketEvent packet_event = PacketEvent.GENERIC_DATA_SYNC)
        {
            Packet p = new Packet(packet_event);
            p.level_name = Singleton.Editor?.CommandsDisplay?.Content?.level;
            p.system_folder = SharedData.pathToAI;
            if (Singleton.Editor?.CommandsDisplay?.CompositeDisplay?.Composite != null)
            {
                p.composite = Singleton.Editor.CommandsDisplay.CompositeDisplay.Composite.shortGUID.ToUInt32();
            }
            if (Singleton.Editor?.CommandsDisplay?.CompositeDisplay?.EntityDisplay?.Entity != null)
            {
                p.entity = Singleton.Editor.CommandsDisplay.CompositeDisplay.EntityDisplay.Entity.shortGUID.ToUInt32();
            }
            if (Singleton.Editor?.CommandsDisplay?.CompositeDisplay != null)
            {
                List<CompositePath.CompAndEnt> richPath = Singleton.Editor.CommandsDisplay.CompositeDisplay.Path.GetPathRich(Singleton.Editor.CommandsDisplay.CompositeDisplay.Composite);
                foreach (CompositePath.CompAndEnt entry in richPath)
                    if (entry.Entity != null)
                        p.path.Add(entry.Entity.shortGUID.ToUInt32());
            }
            p.dirty = DirtyTracker.IsDirty;
            return p;
        }

        /* Send data to all connected Unity sessions */
        private static void SendData(Packet content)
        {
            _server?.WebSocketServices["/commands_editor"].Sessions.Broadcast(JsonConvert.SerializeObject(content));
        }
    }
}
