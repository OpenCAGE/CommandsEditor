using CATHODE.Assets;
using CATHODE.Commands;
using CATHODE.LEGACY;
using CATHODE.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CathodeEditorGUI
{
    public static class CurrentInstance
    {
        //Level descriptors & scripting
        public static CommandsPAK commandsPAK;
        public static MoverDatabase moverDB;

        //Helpers
        public static EntityNameLookup compositeLookup;

        //Scripting state info
        public static Composite selectedComposite;
        public static Entity selectedEntity;

        //Assets and various DBs
        public static CathodeModels modelDB;
        public static MaterialDatabase materialDB;
        public static Textures textureDB;
        public static Textures textureDB_Global;
        public static RenderableElementsDatabase redsDB;
    }
}
