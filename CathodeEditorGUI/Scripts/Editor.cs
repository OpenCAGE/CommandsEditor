using CATHODE;
using CATHODE.EXPERIMENTAL;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using System.Collections.Generic;

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

        //Level-specific assets and various DBs
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

            public SoundBankData sound_bankdata;
            public SoundDialogueLookups sound_dialoguelookups;
            public SoundEventData sound_eventdata;
        }

        //Global animation strings
        public static AnimationStrings animstrings;
        public static AnimationStrings animstrings_debug;

        //Global localised string DBs for English
        public static Dictionary<string, Strings> strings = new Dictionary<string, Strings>();
    }
}
