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
        public static List<string> AllSkeletons = new List<string>();
        public static Dictionary<string, List<string>> GenderedSkeletons = new Dictionary<string, List<string>>();
        public static SkeleDB SkeletonDB;

        //Global animation strings
        public static AnimationStrings AnimationStrings;
        public static AnimationStrings AnimationStrings_Debug;

        //Global textures
        public static Textures GlobalTextures;
        
        //Load events
        public static Action<LevelContent> OnLevelLoaded;
        public static Action<LevelContent> OnAssetsLoaded;

        //Reload events
        public static Action<Entity> OnEntityReloaded;
        public static Action<Composite> OnCompositeReloaded;

        //Selection events
        public static Action<Entity> OnEntitySelected;
        public static Action<Composite> OnCompositeSelected;

        //Misc events
        public static Action OnCAGEAnimationEditorOpened;

        //Settings keys
        public static SettingsStrings Settings = new SettingsStrings();
        public class SettingsStrings
        {
            public readonly string ServerOpt = "CE_ConnectToUnity";
            public readonly string NodeOpt = "CS_NodeView";
            public readonly string EntIdOpt = "CS_ShowEntityIDs";
            public readonly string InstOpt = "CS_InstanceMode";
            public readonly string CompNameOnlyOpt = "CS_SearchOnlyCompName";
            public readonly string UseEntTabsOpt = "CS_UseEntityTabs";
            public readonly string ShowSavedMsgOpt = "CS_ShowSavedNotif";
            public readonly string ShowTexOpt = "CS_ShowTextures";
            public readonly string FileBrowserViewOpt = "CS_FileBrowserView";
            public readonly string EnableFileBrowser = "CS_FileBrowserEnabled";
            public readonly string AutoHideCompositeDisplay = "CS_FileBrowserAutoHide";
            public readonly string KeepUsesWindowOpen = "CS_KeepUsesWindowOpen";
            public readonly string OpenEntityFromNode = "CS_OpenEntityFromNode";
            public readonly string EntitySplitWidth = "CS_EntitySplitWidth";
            public readonly string CompositeSplitWidth = "CS_CompositeSplitWidth";
            public readonly string CommandsSplitWidth = "CS_CommandsSplitWidth";
            public readonly string WindowWidth = "CS_WindowWidth";
            public readonly string WindowHeight = "CS_WindowHeight";
            public readonly string WindowState = "CS_WindowState";
            public readonly string NodegraphState = "CS_NodegraphState";
            public readonly string SplitWidthMainRight = "CS_SplitWidthMainRight";
            public readonly string SplitWidthMainBottom = "CS_SplitWidthMainBottom";
        }
    }
}
