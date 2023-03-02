﻿using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;

namespace CommandsEditor
{
    public static class Editor
    {
        //Level descriptors & scripting
        public static Commands commands;
        public static Movers mvr;

        //Scripting state info
        public static Selected selected;
        public struct Selected
        {
            public Composite composite;
            public Entity entity;
        }

        //Assets and various DBs
        public static Resource resource;
        public struct Resource
        {
            public Models models;
            public Materials materials;
            public Textures textures;
            public Textures textures_global;

            public RenderableElements reds;

            public EnvironmentAnimations env_animations;
            public CollisionMaps collision_maps;
        }
    }
}