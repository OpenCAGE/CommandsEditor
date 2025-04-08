using System;
using System.Collections.Generic;

namespace CommandsEditor.UnityConnection
{
    public enum PacketEvent
    {
        LEVEL_LOADED,

        COMPOSITE_SELECTED,
        COMPOSITE_PATH_STEPPED,
        COMPOSITE_DELETED,

        ENTITY_SELECTED,
        ENTITY_MOVED,
        ENTITY_DELETED,
        ENTITY_ADDED,
        ENTITY_RESOURCE_MODIFIED,

        GENERIC_DATA_SYNC,
    }

    public class Packet
    {
        public Packet(PacketEvent packet_event = PacketEvent.GENERIC_DATA_SYNC)
        {
            this.packet_event = packet_event;
        }

        //Packet metadata
        public PacketEvent packet_event;
        public int version = 3;

        //Setup metadata
        public string level_name = "";
        public string system_folder = "";

        //Selection metadata
        public uint composite = 0;
        public List<uint> path = new List<uint>(); 
        public uint entity = 0;

        //Transform
        public System.Numerics.Vector3 position = new System.Numerics.Vector3();
        public System.Numerics.Vector3 rotation = new System.Numerics.Vector3();

        //Renderable resource
        public List<Tuple<int, int>> renderable = new List<Tuple<int, int>>(); //Model Index, Material Index

        //Track if things have changed
        public bool dirty = false;
    }
}
