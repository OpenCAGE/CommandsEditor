using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.Popups;
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

        //Global textures
        public static Textures GlobalTextures;
        
        //State tracking for paths from root composite
        public static CompositeInstance InstanceInfo = new CompositeInstance();
        public class CompositeInstance
        {
            public Composite InitialComposite;
            public List<CompEnt> PathFromInitial;

            //Get the currently loaded thing in the path
            public Composite CurrentComposite
            {
                get
                {
                    if (PathFromInitial.Count == 0)
                        return InitialComposite;
                    return PathFromInitial[PathFromInitial.Count - 1].Composite;
                }
            }

            //Get the previously loaded composite in the path
            public Composite PreviousComposite
            {
                get
                {
                    if (PathFromInitial.Count == 1)
                        return InitialComposite;
                    if (PathFromInitial.Count == 0)
                        return null; // Can't go back from the initial composite lol - maybe we should handle this differently?

                    return PathFromInitial[PathFromInitial.Count - 2].Composite;
                }
            }



            public List<string> PathAsString;

            public class CompEnt 
            {
                public Entity Entity;
                public Composite Composite;
            }
        }


        public static Action<LevelContent> OnLevelLoaded;
        public static Action<LevelContent> OnAssetsLoaded;

        public static Action<Entity> OnEntitySelected;
        public static Action<Composite> OnCompositeSelected;

        public static Action OnCAGEAnimationEditorOpened;



        public static SettingsStrings Settings = new SettingsStrings();
        public class SettingsStrings
        {
            public readonly string ServerOpt = "CE_ConnectToUnity";
            public readonly string BackupsOpt = "CS_Autosave";
            public readonly string NodeOpt = "CS_NodeView";
            public readonly string EntIdOpt = "CS_ShowEntityIDs";
            public readonly string InstOpt = "CS_InstanceMode";
            public readonly string CompNameOnlyOpt = "CS_SearchOnlyCompName";
            public readonly string UseCompTabsOpt = "CS_UseCompositeTabs";
            public readonly string UseEntTabsOpt = "CS_UseEntityTabs";
            public readonly string ShowSavedMsgOpt = "CS_ShowSavedNotif";
            public readonly string ShowTexOpt = "CS_ShowTextures";
        }



    }
}
