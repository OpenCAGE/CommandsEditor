using CATHODE;
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

        private static bool _isDirty = false;

        static Send()
        {
            Singleton.OnLevelLoaded += LevelLoaded;
            Singleton.OnSaved += LevelSaved;
            Singleton.OnCompositeSelected += CompositeSelected;
            Singleton.OnCompositeDeleted += CompositeDeleted;
            Singleton.OnEntitySelected += EntitySelected;
            Singleton.OnEntityMoved += EntityMoved;
            Singleton.OnEntityAdded += EntityAdded;
            Singleton.OnEntityDeleted += EntityDeleted;
            Singleton.OnResourceModified += ResourceModified;
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
            _isDirty = false;
            SendData(GeneratePacket(PacketEvent.LEVEL_LOADED));
        }

        /* The level has been saved -> clear our dirty flag */
        private static void LevelSaved()
        {
            _isDirty = false;
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

        /* Composite lifetime events -> sync them to Unity */
        private static void CompositeDeleted(Composite composite)
        {
            SendData(GeneratePacket(PacketEvent.COMPOSITE_DELETED));
        }

        /* Entity lifetime events -> sync them to Unity */
        private static void EntitySelected(Entity entity)
        {
            SendData(GeneratePacket(PacketEvent.ENTITY_SELECTED));
        }
        private static void EntityMoved(cTransform transform, Entity entity)
        {
            _isDirty = true;

            Packet p = GeneratePacket(PacketEvent.ENTITY_MOVED);
            p.position = transform.position;
            p.rotation = transform.rotation;
            SendData(p);
        }
        private static void EntityAdded(Entity entity)
        {
            _isDirty = true;
            SendData(GeneratePacket(PacketEvent.ENTITY_ADDED));
        }
        private static void EntityDeleted(Entity entity)
        {
            _isDirty = true;
            SendData(GeneratePacket(PacketEvent.ENTITY_DELETED));
        }
        private static void ResourceModified()
        {
            _isDirty = true;
            Packet p = GeneratePacket(PacketEvent.ENTITY_RESOURCE_MODIFIED);
            if (Singleton.Editor?.CommandsDisplay?.CompositeDisplay?.EntityDisplay?.Entity != null)
            {
                //NOTE: Only caring about the "resource" parameter for now, as we're not rendering particles in the editor (which use the resource on entity)
                Entity e = Singleton.Editor.CommandsDisplay.CompositeDisplay.EntityDisplay.Entity;
                Parameter resource = e.GetParameter("resource");
                if (resource?.content != null && resource.content.dataType == DataType.RESOURCE)
                {
                    ResourceReference resourceRef = ((cResource)resource.content).GetResource(ResourceType.RENDERABLE_INSTANCE);
                    if (resourceRef != null)
                    {
                        for (int i = 0; i < resourceRef.count; i++)
                        {
                            RenderableElements.Element element = Singleton.Editor.CommandsDisplay.Content.resource.reds.Entries[resourceRef.index];
                            p.renderable.Add(new Tuple<int, int>(element.ModelIndex, element.MaterialIndex));
                        }
                    }
                }
            }
            SendData(p);
        }

        /* Re-sync a new client with all current info */
        private static void SyncClient()
        {
            if (_isDirty)
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
            p.dirty = _isDirty; //NOTE: Not using the DirtyTracker here as we only care about changes that will visually affect the Unity editor.
            return p;
        }

        /* Send data to all connected Unity sessions */
        private static void SendData(Packet content)
        {
            _server?.WebSocketServices["/commands_editor"].Sessions.Broadcast(JsonConvert.SerializeObject(content));
        }
    }
}
