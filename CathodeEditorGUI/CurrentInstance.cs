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
        public static CommandsPAK commandsPAK;

        public static EntityNameLookup compositeLookup;

        public static CathodeComposite selectedComposite;
        public static CathodeEntity selectedEntity;

        //Chonky assets - don't keep these loaded all the time
        public static CathodeModels modelDB;
        public static MaterialDatabase materialDB;
        public static CathodeTextures textureDB;
    }
}
