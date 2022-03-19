using System.Collections.Generic;
using System.IO;

namespace CathodeEditorGUI
{
    class MapDirectories
    {
        private static List<string> all_env_dirs = new List<string>();
        static MapDirectories()
        {
            all_env_dirs.Clear();

            AddIfAvailable("BSP_LV426_Pt01");
            AddIfAvailable("BSP_LV426_Pt02");
            AddIfAvailable("BSP_Torrens");
            AddIfAvailable("DLC/BSPNostromo_Ripley_Patch"); //We use _PATCH as that's the COMMANDS.PAK that is loaded
            AddIfAvailable("DLC/BSPNostromo_TwoTeams_Patch"); //We use _PATCH as that's the COMMANDS.PAK that is loaded
            AddIfAvailable("DLC/ChallengeMap1");
            AddIfAvailable("DLC/ChallengeMap3");
            AddIfAvailable("DLC/ChallengeMap4");
            AddIfAvailable("DLC/ChallengeMap5");
            AddIfAvailable("DLC/ChallengeMap7");
            AddIfAvailable("DLC/ChallengeMap9");
            AddIfAvailable("DLC/ChallengeMap11");
            AddIfAvailable("DLC/ChallengeMap12");
            AddIfAvailable("DLC/ChallengeMap14");
            AddIfAvailable("DLC/ChallengeMap16");
            AddIfAvailable("DLC/SalvageMode1");
            AddIfAvailable("DLC/SalvageMode2");
            AddIfAvailable("ENG_Alien_Nest");
            AddIfAvailable("ENG_ReactorCore");
            AddIfAvailable("ENG_TowPlatform");
            AddIfAvailable("Frontend");
            AddIfAvailable("HAB_Airport");
            AddIfAvailable("HAB_CorporatePent");
            AddIfAvailable("HAB_ShoppingCentre");
            AddIfAvailable("SCI_AndroidLab");
            AddIfAvailable("SCI_HospitalLower");
            AddIfAvailable("SCI_HospitalUpper");
            AddIfAvailable("SCI_Hub");
            AddIfAvailable("Solace");
            AddIfAvailable("Tech_Comms");
            AddIfAvailable("Tech_Hub");
            AddIfAvailable("Tech_MuthrCore");
            AddIfAvailable("Tech_RnD");
            AddIfAvailable("Tech_RnD_HzdLab");
        }
        private static void AddIfAvailable(string MapName)
        {
            if (!File.Exists(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + MapName + "/WORLD/COMMANDS.PAK")) return;
            all_env_dirs.Add(MapName);
        }

        public static List<string> GetAvailable()
        {
            return all_env_dirs;
        }
    }
}
