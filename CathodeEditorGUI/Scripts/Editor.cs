using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using System;
using System.Collections.Generic;

namespace CommandsEditor
{
    public class Editor
    {
        //Level descriptors & scripting
        public Commands commands;
        public Movers mvr;

        //Scripting state info
        public Selected selected;
        public struct Selected
        {
            public Composite composite;
            public Entity entity;
        }

        //Level-specific assets and various DBs
        public Resource resource;
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
            public SoundEnvironmentData sound_environmentdata;
        }

        //Global animation strings
        public AnimationStrings animstrings;
        public AnimationStrings animstrings_debug;

        //Global localised string DBs for English
        public Dictionary<string, Strings> strings = new Dictionary<string, Strings>();

        //Events for new commands/entity/composite being selected
        public Action<Commands> OnCommandsSelected;
        public Action<Entity> OnEntitySelected;
        public Action<Composite> OnCompositeSelected;
        public Action OnCAGEAnimationEditorOpened;
    }
}
