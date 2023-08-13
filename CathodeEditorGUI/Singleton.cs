using CATHODE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsEditor
{
    public static class Singleton
    {
        public static CommandsEditor Editor;

        //Global localised string DBs for English
        public static Dictionary<string, Strings> Strings = new Dictionary<string, Strings>();

        //Skeletons from ANIMATIONS.PAK
        public static Dictionary<string, List<string>> Skeletons = new Dictionary<string, List<string>>();

        //Global animation strings
        public static AnimationStrings AnimationStrings;
        public static AnimationStrings AnimationStrings_Debug;
    }
}
