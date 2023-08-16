using CATHODE.Scripting;
using CATHODE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Scripting.Internal;
using OpenCAGE;

namespace CommandsEditor
{
    public class LevelContent
    {
        //Level descriptors & scripting
        public string level;
        public Commands commands;
        public Movers mvr;

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
            public PhysicsMaps physics_maps;

            public SoundBankData sound_bankdata;
            public SoundDialogueLookups sound_dialoguelookups;
            public SoundEventData sound_eventdata;
            public SoundEnvironmentData sound_environmentdata;

            public CharacterAccessorySets character_accessories;
        }

        //Skeletons from ANIMATIONS.PAK
        public Dictionary<string, List<string>> skeletons = new Dictionary<string, List<string>>();

        //Global animation strings
        public AnimationStrings animstrings;
        public AnimationStrings animstrings_debug;

        //Global localised string DBs for English
        public Dictionary<string, Strings> strings = new Dictionary<string, Strings>();

        //UI stuff
        public Dictionary<Composite, Dictionary<Entity, ListViewItem>> composite_content_cache = new Dictionary<Composite, Dictionary<Entity, ListViewItem>>();

        //TODO: this should really be refactored. hacked in legacy stuff.
        public EditorUtils editor_utils;

        public LevelContent(string levelName)
        {
            level = levelName;

            //Load everything
            if (!Load(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + level + "/"))
            {
                MessageBox.Show("Failed to load level data.\nPlease reset your game files.", "Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Link up commands to utils and cache some things (TODO: this is the only thing holding us back from being able to have multiple levels open at once)
            EntityUtils.LinkCommands(commands);
            ShortGuidUtils.LinkCommands(commands);

            //Cache entity list view items
            ListViewItem[][] compositeItems = new ListViewItem[commands.Entries.Count][];
            Parallel.For(0, commands.Entries.Count, (i) =>
            {
                List<Entity> entities = commands.Entries[i].GetEntities();
                compositeItems[i] = new ListViewItem[entities.Count];
                Parallel.For(0, entities.Count, (x) =>
                {
                    compositeItems[i][x] = GenerateListViewItem(entities[x], commands.Entries[i], false);
                });
            });
            for (int i = 0; i < commands.Entries.Count; i++)
            {
                List<Entity> entities = commands.Entries[i].GetEntities();
                Dictionary<Entity, ListViewItem> listViewItems = new Dictionary<Entity, ListViewItem>();
                for (int x = 0; x < compositeItems[i].Length; x++)
                {
                    listViewItems.Add(entities[x], compositeItems[i][x]);
                }
                composite_content_cache.Add(commands.Entries[i], listViewItems);
            }

            //Force collect
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            //TEMP: Testing out new brute-forced ShortGuids
            ShortGuidUtils.Generate("Win");
            ShortGuidUtils.Generate("End");
            ShortGuidUtils.Generate("Cut");
            for (int i = 0; i < 26; i++)
                ShortGuidUtils.Generate("cut" + i);
            //ShortGuidUtils.Generate("temp");
            //ShortGuidUtils.Generate("test");
            //ShortGuidUtils.Generate("M01");
            //ShortGuidUtils.Generate("M02");
            //ShortGuidUtils.Generate("M03");
            //ShortGuidUtils.Generate("TEST");
            //ShortGuidUtils.Generate("CORE");
            //ShortGuidUtils.Generate("left");
            //ShortGuidUtils.Generate("back");
            //ShortGuidUtils.Generate("bind");
            //ShortGuidUtils.Generate("cam");
        }

        private bool Load(string path)
        {
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                string worldPath = path + "WORLD/";
                string renderablePath = path + "RENDERABLE/";

                //The game has two hard-coded _PATCH overrides. We should use RENDERABLE from the non-patched folder.
                switch (level)
                {
                    case "DLC/BSPNOSTROMO_RIPLEY_PATCH":
                    case "DLC/BSPNOSTROMO_TWOTEAMS_PATCH":
                        renderablePath = renderablePath.Replace(level, level.Substring(0, level.Length - ("_PATCH").Length)) + "RENDERABLE/";
                        break;
                }

                //Somewhat hacky way of loading everything at once
                Parallel.For(0, 15, (i) =>
                {
                    switch (i)
                    {
                        case 0:
                            resource.models = new Models(renderablePath + "LEVEL_MODELS.PAK");
                            break;
                        case 1:
                            resource.materials = new Materials(renderablePath + "LEVEL_MODELS.MTL");
                            break;
                        case 2:
                            //resource.textures = new Textures(renderablePath + "LEVEL_TEXTURES.ALL.PAK");
                            break;
                        case 3:
                            //resource.textures_Global = new Textures(SharedData.pathToAI + "/DATA/ENV/GLOBAL/WORLD/GLOBAL_TEXTURES.ALL.PAK");
                            break;
                        case 4:
                            resource.reds = new RenderableElements(worldPath + "REDS.BIN");
                            break;
                        case 5:
                            resource.env_animations = new EnvironmentAnimations(worldPath + "ENVIRONMENT_ANIMATION.DAT");
                            break;
                        case 6:
                            resource.collision_maps = new CollisionMaps(worldPath + "COLLISION.MAP");
                            break;
                        case 7:
                            resource.physics_maps = new PhysicsMaps(worldPath + "PHYSICS.MAP");
                            break;
                        case 8:
                            resource.sound_bankdata = new SoundBankData(worldPath + "SOUNDBANKDATA.DAT");
                            break;
                        case 9:
                            resource.sound_dialoguelookups = new SoundDialogueLookups(worldPath + "SOUNDDIALOGUELOOKUPS.DAT");
                            break;
                        case 10:
                            resource.sound_eventdata = new SoundEventData(worldPath + "SOUNDEVENTDATA.DAT");
                            break;
                        case 11:
                            resource.sound_environmentdata = new SoundEnvironmentData(worldPath + "SOUNDENVIRONMENTDATA.DAT");
                            break;
                        case 12:
                            resource.character_accessories = new CharacterAccessorySets(worldPath + "CHARACTERACCESSORYSETS.BIN");
                            break;
                        case 13:
                            mvr = new Movers(worldPath + "MODELS.MVR");
                            break;
                        case 14:
                            commands = new Commands(worldPath + "COMMANDS.PAK");
                            break;
                    }
                });

                if (!commands.Loaded || commands.EntryPoints == null || commands.EntryPoints[0] == null)
                    return false;

                Singleton.OnLevelLoaded?.Invoke(this);
                return true;
#if !CATHODE_FAIL_HARD
            }
            catch
            {
                return false;
            }
#endif
        }

        public ListViewItem GenerateListViewItem(Entity entity, Composite composite, bool checkCache = true)
        {
            if (checkCache)
            {
                if (composite_content_cache.ContainsKey(composite))
                    if (composite_content_cache[composite].ContainsKey(entity))
                        return composite_content_cache[composite][entity];
            }

            ListViewItem item = new ListViewItem()
            {
                Tag = entity
            };
            switch (entity.variant)
            {
                case EntityVariant.VARIABLE:
                    item.Text = ShortGuidUtils.FindString(((VariableEntity)entity).name);
                    item.SubItems.Add(((VariableEntity)entity).type.ToString());
                    break;
                case EntityVariant.FUNCTION:
                    item.Text = EntityUtils.GetName(composite.shortGUID, entity.shortGUID);
                    Composite funcComposite = commands.GetComposite(((FunctionEntity)entity).function);
                    if (funcComposite != null) item.SubItems.Add(funcComposite.name);
                    else item.SubItems.Add(CathodeEntityDatabase.GetEntity(((FunctionEntity)entity).function).className);
                    break;
                case EntityVariant.OVERRIDE:
                    CommandsUtils.ResolveHierarchy(commands, composite, ((OverrideEntity)entity).connectedEntity.hierarchy, out Composite c, out string s, false);
                    item.Text = s;
                    item.SubItems.Add("");
                    break;
                case EntityVariant.PROXY:
                    CommandsUtils.ResolveHierarchy(commands, composite, ((ProxyEntity)entity).connectedEntity.hierarchy, out Composite c2, out string s2, false);
                    item.Text = EntityUtils.GetName(composite.shortGUID, entity.shortGUID);
                    item.SubItems.Add(s2);
                    break;
            }
            item.SubItems.Add(entity.shortGUID.ToByteString());

            //we wanted to check the cache and it wasn't there, so lets add it
            if (checkCache)
            {
                if (!composite_content_cache.ContainsKey(composite))
                {
                    composite_content_cache.Add(composite, new Dictionary<Entity, ListViewItem>());
                }
                if (!composite_content_cache[composite].ContainsKey(entity))
                {
                    composite_content_cache[composite].Add(entity, item);
                }
            }

            return item;
        }
    }
}
