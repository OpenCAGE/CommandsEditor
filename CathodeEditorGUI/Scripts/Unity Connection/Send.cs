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
using System.Windows.Media.Animation;
using WebSocketSharp.Server;

namespace CommandsEditor.UnityConnection
{
    public static class Send
    {
        private static WebSocketServer _server;
        private static Client _serverLogic;

        public static bool Started => _server != null;
        public static bool Connected => _server != null && _server.WebSocketServices["/commands_editor"].Sessions.Count != 0;

        private static bool _isDirty = false;

        static Send()
        {
            Singleton.OnLevelLoaded += LevelLoaded;
            Singleton.OnSaved += LevelSaved;
            Singleton.OnCompositeAdded += CompositeAdded;
            Singleton.OnCompositeReloaded += CompositeReloaded;
            Singleton.OnCompositeSelected += CompositeSelected;
            Singleton.OnCompositeDeleted += CompositeDeleted;
            Singleton.OnEntityReloaded += EntitySelected;
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
                _server.AddWebSocketService<Client>("/commands_editor", (server) =>
                {
                    _serverLogic = server;
                    _serverLogic.OnConnect += SyncClient;
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
            Packet p = GeneratePacket(PacketEvent.COMPOSITE_SELECTED);
            p.composite = composite.shortGUID.ToUInt32();
            SendData(p);
        }
        private static void CompositeReloaded(Composite composite)
        {
            Packet p = GeneratePacket(PacketEvent.COMPOSITE_RELOADED);
            p.composite = composite.shortGUID.ToUInt32();
            SendData(p);
        }

        /* Composite lifetime events -> sync them to Unity */
        private static void CompositeAdded(Composite composite)
        {
            Packet p = GeneratePacket(PacketEvent.COMPOSITE_ADDED);
            p.composite = composite.shortGUID.ToUInt32();
            SendData(p);
        }
        private static void CompositeDeleted(Composite composite)
        {
            Packet p = GeneratePacket(PacketEvent.COMPOSITE_DELETED);
            p.composite = composite.shortGUID.ToUInt32();
            SendData(p);
        }

        /* Entity lifetime events -> sync them to Unity */
        private static void EntitySelected(Entity entity)
        {
            Packet p = GeneratePacket(PacketEvent.ENTITY_SELECTED);
            p.entity_variant = entity.variant;
            p.entity = entity.shortGUID.ToUInt32();
            SendData(p);
        }
        private static void EntityMoved(cTransform transform, Entity entity)
        {
            _isDirty = true;

            Packet p = GeneratePacket(PacketEvent.ENTITY_MOVED);
            p.position = transform.position;
            p.rotation = transform.rotation;
            p.entity_variant = entity.variant;
            p.entity = entity.shortGUID.ToUInt32();
            SendData(p);
        }
        private static void EntityDeleted(Entity entity)
        {
            _isDirty = true;

            Packet p = GeneratePacket(PacketEvent.ENTITY_DELETED);
            p.entity_variant = entity.variant;
            p.entity = entity.shortGUID.ToUInt32();
            SendData(p);
        }
        private static void EntityAdded(Entity entity)
        {
            _isDirty = true;

            Packet p = GeneratePacket(PacketEvent.ENTITY_ADDED);
            p.entity_variant = entity.variant;
            p.entity = entity.shortGUID.ToUInt32();
            SendData(p);
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
                            RenderableElements.Element element = Singleton.Editor.CommandsDisplay.Content.resource.reds.Entries[resourceRef.index + i];
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
            Console.WriteLine("[WEBSOCKET] " + _server?.WebSocketServices["/commands_editor"].Sessions.Count + " clients connected!");

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
            if (Singleton.Editor?.CommandsDisplay?.CompositeDisplay != null)
            {
                List<CompositePath.CompAndEnt> richPath = Singleton.Editor.CommandsDisplay.CompositeDisplay.Path.GetPathRich(Singleton.Editor.CommandsDisplay.CompositeDisplay.Composite);
                foreach (CompositePath.CompAndEnt entry in richPath)
                {
                    if (entry.Entity != null)
                    {
                        p.path_entities.Add(entry.Entity.shortGUID.ToUInt32());
                        p.path_composites.Add(entry.Composite.shortGUID.ToUInt32());
                    }
                }
            }
            if (Singleton.Editor?.CommandsDisplay?.CompositeDisplay?.EntityDisplay?.Entity != null)
            {
                Entity entity = Singleton.Editor.CommandsDisplay.CompositeDisplay.EntityDisplay.Entity;
                p.path_entities.Add(entity.shortGUID.ToUInt32());
                p.entity = entity.shortGUID.ToUInt32();
                p.entity_variant = entity.variant;
            }
            if (Singleton.Editor?.CommandsDisplay?.CompositeDisplay?.Composite != null)
            {
                Composite composite = Singleton.Editor.CommandsDisplay.CompositeDisplay.Composite;
                p.path_composites.Add(composite.shortGUID.ToUInt32());
                p.composite = composite.shortGUID.ToUInt32();
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
