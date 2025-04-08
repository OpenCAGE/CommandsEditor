using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.Popups;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CommandsEditor
{
    public static class Singleton
    {
        static Singleton()
        {
            //Populate localised text string databases (in English)
            List<string> textList = Directory.GetFiles(SharedData.pathToAI + "/DATA/TEXT/ENGLISH/", "*.TXT", SearchOption.AllDirectories).ToList<string>();
            {
                TextDB[] strings = new TextDB[textList.Count];
                Parallel.For(0, textList.Count, (i) =>
                {
                    strings[i] = new TextDB(textList[i]);
                });
                for (int i = 0; i < textList.Count; i++)
                    GlobalTextDBs.Add(Path.GetFileNameWithoutExtension(textList[i]), strings[i]);
            }

            //Load bulky global data
            Task.Factory.StartNew(() => LoadGlobalAssets());
            Task.Factory.StartNew(() => LoadAnimData());
        }

        public static CommandsEditor Editor;

        //Global localised string DBs for English
        public static Dictionary<string, TextDB> GlobalTextDBs = new Dictionary<string, TextDB>();

        //Animation content from ANIMATIONS.PAK
        public static List<string> AllSkeletons = new List<string>();
        public static List<string> AllAnimTrees = new List<string>();
        public static SortedDictionary<string, HashSet<string>> AllAnimations = new SortedDictionary<string, HashSet<string>>(); //Anim Set, Animations
        public static Dictionary<string, HashSet<string>> GenderedSkeletons = new Dictionary<string, HashSet<string>>(); //Gender, Skeletons

        //Global animation strings
        public static AnimationStrings AnimationStrings;
        public static AnimationStrings AnimationStrings_Debug;

        //Global assets
        public static Textures GlobalTextures;

        //Load events
        public static Action<LevelContent> OnLevelLoaded;
        public static Action<LevelContent> OnLevelAssetsLoaded;

        //Reload events
        public static Action<Entity> OnEntityReloaded;
        public static Action<Composite> OnCompositeReloaded;

        //Selection events
        public static Action<Entity> OnEntitySelected;
        public static Action<Composite> OnCompositeSelected;

        //Misc events
        public static Action OnCAGEAnimationEditorOpened;
        public static Action<Entity> OnEntityDeleted;
        public static Action<Entity, string> OnEntityRenamed;
        public static Action<Composite, string> OnCompositeRenamed;
        public static Action<cTransform, Entity> OnEntityMoved;
        public static Action<Entity> OnEntityDeleted;
        public static Action<Composite> OnCompositeDeleted;
        public static Action OnSaved;
        public static Action OnParameterModified;
        public static Action OnResourceModified;

        //Entity about to be / being added
        public static Action OnEntityAddPending;
        public static Action<Entity> OnEntityAdded;
        public static Action OnCompositeAddPending;
        public static Action<Composite> OnCompositeAdded;

        //Settings keys
        public static SettingsStrings Settings = new SettingsStrings();
        public class SettingsStrings
        {
            public readonly string ServerOpt = "CE_ConnectToUnity";
            public readonly string NodeOpt = "CS_NodeView";
            public readonly string EntIdOpt = "CS_ShowEntityIDs";
            public readonly string InstOpt = "CS_InstanceMode";
            public readonly string CompNameOnlyOpt = "CS_SearchOnlyCompName";
            public readonly string UseEntityTabs = "CS_UseEntityTabs";
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
            public readonly string NodegraphWidth = "CS_NodegraphWidth";
            public readonly string NodegraphHeight = "CS_NodegraphHeight";
            public readonly string SplitWidthMainRight = "CS_SplitWidthMainRight";
            public readonly string SplitWidthMainBottom = "CS_SplitWidthMainBottom";
            public readonly string PreviouslySelectedFunctionType = "CS_PreviouslySelectedFunctionType";
            public readonly string PreviouslySearchedFunctionType = "CS_PreviouslySearchedFunctionType";
            public readonly string PreviouslySearchedParamPopulation = "CS_PreviouslySearchedParamPopulation";
            public readonly string PreviouslySelectedCompInstType = "CS_PreviouslySelectedCompInstType";
            public readonly string PreviouslySearchedCompInstType = "CS_PreviouslySearchedCompInstType";
            public readonly string PreviouslySearchedParamPopulationComp = "CS_PreviouslySearchedParamPopulationComp";
            public readonly string ExperimentalResourceStuff = "CS_EnableExperimentalResourceSaving";
            public readonly string MakeNodeWhenMakeEntity = "CS_MakeNodeWhenMakeEntity";
            public readonly string PrevFuncUsesSearch = "CS_PrevFuncUsesSearch";
            public readonly string PrevVariableType = "CS_PrevVariableTypeNew";
            public readonly string PrevVariableType_Enum = "CS_PrevVariableTypeEnum";
            public readonly string PrevVariableType_EnumString = "CS_PrevVariableTypeEnumString";
            public readonly string CustomColours = "CS_CustomColours";
            public readonly string EntityListState = "CS_EntityListState";
            public readonly string EntityListWidth = "CS_EntityListWidth";
            public readonly string EntityInspectorState = "CS_EntityInspectorState";
            public readonly string EntityInspectorWidth = "CS_EntityInspectorWidth";
            public readonly string PreviouslySearchedParamPopulationProxyOrAlias = "CS_PreviouslySearchedParamPopulationProxyOrAlias";
        }

        public static Action OnAnimationsLoaded;
        public static bool LoadedAnimationContent => _loadedAnimationContent;
        private static bool _loadedAnimationContent = false;

        public static Action OnGlobalAssetsLoaded;
        public static bool LoadedGlobalAssets => _loadedGlobalAssets;
        private static bool _loadedGlobalAssets = false;

        /* Load anim data */
        private static void LoadAnimData()
        {
            //Load animation data
            PAK2 animPAK = new PAK2(SharedData.pathToAI + "/DATA/GLOBAL/ANIMATION.PAK");

            //Load all male/female skeletons
            List<PAK2.File> skeletonDefs = animPAK.Entries.FindAll(o => o.Filename.Length > 17 && o.Filename.Substring(0, 17) == "DATA\\SKELETONDEFS");
            for (int i = 0; i < skeletonDefs.Count; i++)
            {
                string skeleton = Path.GetFileNameWithoutExtension(skeletonDefs[i].Filename);
                File.WriteAllBytes(skeleton, skeletonDefs[i].Content);
                XmlNode skeletonType = new BML(skeleton).Content.SelectSingleNode("//SkeletonDef/LoResReferenceSkeleton");
                if (skeletonType?.InnerText == "MALE" || skeletonType?.InnerText == "FEMALENPC")
                {
                    if (!GenderedSkeletons.ContainsKey(skeletonType?.InnerText))
                        GenderedSkeletons.Add(skeletonType?.InnerText, new HashSet<string>());
                    GenderedSkeletons[skeletonType?.InnerText].Add(skeleton);
                }
                File.Delete(skeleton);
            }

            //Anim string db
            File.WriteAllBytes("ANIM_STRING_DB.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB.BIN")).Content);
            AnimationStrings = new AnimationStrings("ANIM_STRING_DB.BIN");
            File.Delete("ANIM_STRING_DB.BIN");

            //Debug anim string db
            File.WriteAllBytes("ANIM_STRING_DB_DEBUG.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB_DEBUG.BIN")).Content);
            AnimationStrings_Debug = new AnimationStrings("ANIM_STRING_DB_DEBUG.BIN");
            File.Delete("ANIM_STRING_DB_DEBUG.BIN");

            //Load all skeleton names
            List<PAK2.File> skeletonNames = animPAK.Entries.FindAll(o => o.Filename.Length > 24 && o.Filename.Substring(0, 24) == "DATA\\ANIM_SYS\\SKELE\\DEFS");
            for (int i = 0; i < skeletonNames.Count; i++)
                AllSkeletons.Add(AnimationStrings_Debug.Entries[Convert.ToUInt32(Path.GetFileNameWithoutExtension(skeletonNames[i].Filename))]);
            AllSkeletons.Sort();

            //Load all anim sets
            List<PAK2.File> animClipDbs = animPAK.Entries.FindAll(o => { string path = Path.GetFileName(o.Filename); if (path.Length < ("_ANIM_CLIP_DB.BIN").Length) return false; return path.Substring(path.Length - ("_ANIM_CLIP_DB.BIN").Length) == "_ANIM_CLIP_DB.BIN"; });
            for (int i = 0; i < animClipDbs.Count; i++)
            {
                uint animSetID = Convert.ToUInt32(Path.GetFileName(animClipDbs[i].Filename).Split('_')[0]);
                string animSet = AnimationStrings_Debug.Entries[animSetID];
                HashSet<string> animations = new HashSet<string>();
                using (BinaryReader reader = new BinaryReader(new MemoryStream(animClipDbs[i].Content)))
                {
                    //This fixes a weird thing where there's an unknown variable offset at the start
                    int offset = 4;
                    while (true)
                    {
                        reader.BaseStream.Position = offset;
                        if (reader.ReadUInt32() == animSetID)
                            break;
                        offset += 1;
                    }
                    reader.BaseStream.Position += 4;

                    int countAnimNames = reader.ReadInt32();
                    int countAnimFileNames = reader.ReadInt32();
                    for (int x = 0; x < countAnimNames; x++)
                    {
                        animations.Add(AnimationStrings_Debug.Entries[reader.ReadUInt32()]);
                        reader.BaseStream.Position += 4;
                    }

                    //TODO: There's more info here. Useful?
                }
                AllAnimations.Add(animSet, animations);
            }
            foreach (KeyValuePair<string, HashSet<string>> anims in AllAnimations)
            {
                List<string> animList = anims.Value.ToList();
                animList.Sort();
                anims.Value.Clear();
                foreach (string anim in animList)
                    anims.Value.Add(anim);
            }

            //Load all anim trees
            List<PAK2.File> animTreeDbs = animPAK.Entries.FindAll(o => { string path = Path.GetFileName(o.Filename); if (path.Length < ("_ANIM_TREE_DB.BIN").Length) return false; return path.Substring(path.Length - ("_ANIM_TREE_DB.BIN").Length) == "_ANIM_TREE_DB.BIN"; });
            for (int i = 0; i < animTreeDbs.Count; i++)
                AllAnimTrees.Add(AnimationStrings_Debug.Entries[Convert.ToUInt32(Path.GetFileName(animTreeDbs[i].Filename).Split('_')[0])]);
            AllAnimTrees.Sort();

            /*
            //Load all animations by anim set (NOTE: no longer using this as the ID gives the filename, not the anim name, but keeping it for future reference)
            List<PAK2.File> streamedAnims = animPAK.Entries.FindAll(o => o.Filename.Length > 24 && o.Filename.Substring(0, 24) == "DATA\\ANIM_SYS\\STREAMED64");
            for (int i = 0; i < streamedAnims.Count; i++)
            {
                string[] filepathParts = Path.GetFileNameWithoutExtension(streamedAnims[i].Filename).Split('_');
                string animationName = AnimationStrings_Debug.Entries[Convert.ToUInt32(filepathParts[filepathParts.Length - 1])];
                string animSetName = "";
                using (BinaryReader reader = new BinaryReader(new MemoryStream(streamedAnims[i].Content)))
                {
                    reader.BaseStream.Position = 4;
                    animSetName = AnimationStrings_Debug.Entries[reader.ReadUInt32()];
                }
                if (animSetName == "FLOATMAN") continue; //NOTE: Skipping "FLOATMAN" as it seems to be the dialogue animations, which are just auto applied by Speech.
                HashSet<string> anims;
                if (!AllAnimations.TryGetValue(animSetName, out anims))
                {
                    anims = new HashSet<string>();
                    AllAnimations.Add(animSetName, anims);
                }
                anims.Add(Path.GetFileName(animationName).ToLower());
            }
            */

            _loadedAnimationContent = true;
            OnAnimationsLoaded?.Invoke();
        }

        /* Load global assets */
        private static void LoadGlobalAssets()
        {
            GlobalTextures = new Textures(SharedData.pathToAI + "/DATA/ENV/GLOBAL/WORLD/GLOBAL_TEXTURES.ALL.PAK");
            _loadedGlobalAssets = true;
            OnGlobalAssetsLoaded?.Invoke();
        }
    }

    public static class EditorClipboard
    {
        public static Entity Entity = null;
    }
}
