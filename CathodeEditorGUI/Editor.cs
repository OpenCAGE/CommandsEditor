using CATHODE;
using CATHODE.Assets;
using CATHODE.Commands;
using CATHODE.LEGACY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CathodeEditorGUI
{
    public static class Editor
    {
        //Level descriptors & scripting
        public static CommandsPAK commands;
        public static MoverDatabase mvr;

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
            public CathodeModels models;
            public MaterialDatabase materials;
            public Textures textures;
            public Textures textures_Global;
            public RenderableElementsDatabase reds;
            public EnvironmentAnimationDatabase env_animations;
        }

        //Helpers
        public static Util util;
        public struct Util
        {
            public EntityUtils entity;
        }
    }
}
