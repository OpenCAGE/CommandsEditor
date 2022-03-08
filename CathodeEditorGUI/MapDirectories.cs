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

            all_env_dirs.Add("BSP_LV426_Pt01");
            all_env_dirs.Add("BSP_LV426_Pt02");
            all_env_dirs.Add("BSP_Torrens");
            AddIfHasDLC("DLC/BSPNostromo_Ripley_Patch"); //We use _PATCH as that's the COMMANDS.PAK that is loaded
            AddIfHasDLC("DLC/BSPNostromo_TwoTeams_Patch"); //We use _PATCH as that's the COMMANDS.PAK that is loaded
            AddIfHasDLC("DLC/ChallengeMap1");
            AddIfHasDLC("DLC/ChallengeMap3");
            AddIfHasDLC("DLC/ChallengeMap4");
            AddIfHasDLC("DLC/ChallengeMap5");
            AddIfHasDLC("DLC/ChallengeMap7");
            AddIfHasDLC("DLC/ChallengeMap9");
            AddIfHasDLC("DLC/ChallengeMap11");
            AddIfHasDLC("DLC/ChallengeMap12");
            AddIfHasDLC("DLC/ChallengeMap14");
            AddIfHasDLC("DLC/ChallengeMap16");
            AddIfHasDLC("DLC/SalvageMode1");
            AddIfHasDLC("DLC/SalvageMode2");
            all_env_dirs.Add("ENG_Alien_Nest");
            all_env_dirs.Add("ENG_ReactorCore");
            all_env_dirs.Add("ENG_TowPlatform");
            all_env_dirs.Add("Frontend");
            all_env_dirs.Add("HAB_Airport");
            all_env_dirs.Add("HAB_CorporatePent");
            all_env_dirs.Add("HAB_ShoppingCentre");
            all_env_dirs.Add("SCI_AndroidLab");
            all_env_dirs.Add("SCI_HospitalLower");
            all_env_dirs.Add("SCI_HospitalUpper");
            all_env_dirs.Add("SCI_Hub");
            all_env_dirs.Add("Solace");
            all_env_dirs.Add("Tech_Comms");
            all_env_dirs.Add("Tech_Hub");
            all_env_dirs.Add("Tech_MuthrCore");
            all_env_dirs.Add("Tech_RnD");
            all_env_dirs.Add("Tech_RnD_HzdLab");
        }
        private static void AddIfHasDLC(string MapName)
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
