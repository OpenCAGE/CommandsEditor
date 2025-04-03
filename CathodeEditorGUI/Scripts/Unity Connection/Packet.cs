using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CommandsEditor.UnityConnection
{
    public enum PacketEvent
    {
        LEVEL_LOADED,

        COMPOSITE_SELECTED,
        COMPOSITE_PATH_STEPPED,

        ENTITY_SELECTED,
        ENTITY_MOVED,
        ENTITY_DELETED,
        ENTITY_ADDED,

        GENERIC_DATA_SYNC,
    }

    public class Packet
    {
        public Packet(PacketEvent packet_event)
        {
            this.packet_event = packet_event;
        }

        //Packet metadata
        public PacketEvent packet_event;
        public const int version = 3;

        //Setup metadata
        public string level_name = "";
        public string system_folder = "";

        //Selection metadata
        public uint composite = 0;
        public List<uint> path = new List<uint>(); 
        public uint entity = 0;

        //Transform
        public Vector3 position = new Vector3();
        public Vector3 rotation = new Vector3();

        //Track if things have changed
        public bool dirty = false;
    }
}
