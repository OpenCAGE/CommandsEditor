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
using CATHODE.LEGACY;
using System.Xml.Linq;
using System.IO;

namespace CommandsEditor
{
    public class LevelContent
    {
        //Level descriptors & scripting
        public string level = "";
        public Commands commands = null;
        public Movers mvr = null;

        //Level-specific assets and various DBs
        public Resource resource = new Resource();
        public class Resource
        {
            public Models models = null;
            public Materials materials = null;
            public Textures textures = null;
            public Textures textures_global = Singleton.GlobalTextures;

            public ShadersPAK shaders_legacy = null; //LEGACY
            public Shaders shaders_new = null; 

            public RenderableElements reds = null;

            public Resources resources = null;
            public EnvironmentAnimations env_animations = null;
            public CollisionMaps collision_maps = null;
            public PhysicsMaps physics_maps = null;

            public SoundBankData sound_bankdata = null;
            public SoundDialogueLookups sound_dialoguelookups = null;
            public SoundEventData sound_eventdata = null;
            public SoundEnvironmentData sound_environmentdata = null;

            public CharacterAccessorySets character_accessories = null;
        }

        //UI stuff
        public Dictionary<Composite, Dictionary<Entity, ListViewItem>> composite_content_cache = new Dictionary<Composite, Dictionary<Entity, ListViewItem>>();

        //TODO: this should really be refactored. hacked in legacy stuff.
        public EditorUtils editor_utils = null;

        private string worldPath = "";
        private string renderablePath = "";

        public LevelContent(string levelName)
        {
            level = levelName;

            string path = SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + level + "/";
            worldPath = path + "WORLD/";

            //The game has two hard-coded _PATCH overrides. We should use RENDERABLE from the non-patched folder.
            switch (level)
            {
                case "DLC/BSPNOSTROMO_RIPLEY_PATCH":
                case "DLC/BSPNOSTROMO_TWOTEAMS_PATCH":
                    renderablePath = path.Replace(level, level.Substring(0, level.Length - ("_PATCH").Length)) + "RENDERABLE/";
                    break;
                default:
                    renderablePath = path + "RENDERABLE/";
                    break;
            }

            //Load
            Task.Factory.StartNew(() => LoadAssets());
            if (!LoadCommands())
            {
                MessageBox.Show("Failed to load Commands data.\nPlease reset your game files.", "Load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Link up commands to utils and cache some things
            EntityUtils.LinkCommands(commands);
            ShortGuidUtils.LinkCommands(commands);

            //Cache entity list view items (TODO: do this on a thread and handle conflicts nicely)
            /*
            Dictionary<Entity, ListViewItem>[] listViewItems = new Dictionary<Entity, ListViewItem>[commands.Entries.Count];
            Parallel.For(0, commands.Entries.Count, (i) =>
            {
                List<Entity> entities = commands.Entries[(int)i].GetEntities();
                ListViewItem[] compositeItems = new ListViewItem[entities.Count];
                Parallel.For(0, entities.Count, (x) =>
                {
                    compositeItems[x] = GenerateListViewItem(entities[(int)x], commands.Entries[(int)i], CacheMethod.IGNORE_CACHE);
                });
                listViewItems[i] = new Dictionary<Entity, ListViewItem>();
                for (int x = 0; x < compositeItems.Length; x++)
                {
                    listViewItems[i].Add(entities[x], compositeItems[x]);
                }
            });
            for (int i = 0; i < commands.Entries.Count; i++)
                composite_content_cache.Add(commands.Entries[i], listViewItems[i]);
            */

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

        ~LevelContent()
        {
            if (EntityUtils.LinkedCommands == commands)
            {
                EntityUtils.LinkCommands(null);
            }
            if (ShortGuidUtils.LinkedCommands == commands)
            {
                ShortGuidUtils.LinkCommands(null);
            }

            resource = null;
            commands = null;
            mvr = null;
            editor_utils = null;

            composite_content_cache.Clear();
        }

        private bool LoadCommands()
        {
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                Parallel.For(0, 3, (i) =>
                {
                    switch (i)
                    {
                        case 1:
                            mvr = new Movers(worldPath + "MODELS.MVR");
                            break;
                        case 2:
                            commands = new Commands(worldPath + "COMMANDS.PAK");
                            commands.Entries = commands.Entries.OrderBy(o => o.name).ToList();
                            commands.EntryPoints[0].name = EditorUtils.GetCompositeName(commands.EntryPoints[0]);
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

        private void LoadAssets()
        {
#if !CATHODE_FAIL_HARD
            try
            {
#endif
                Parallel.For(0, 14, (i) =>
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
                            resource.textures = new Textures(renderablePath + "LEVEL_TEXTURES.ALL.PAK");
                            break;
                        case 3:
                            resource.shaders_legacy = new ShadersPAK(renderablePath + "LEVEL_SHADERS_DX11.PAK"); 
                            resource.shaders_new = new Shaders(renderablePath + "LEVEL_SHADERS_DX11.PAK"); 
                            break;
                        case 4:
                            resource.reds = new RenderableElements(worldPath + "REDS.BIN");
                            break;
                        case 5:
                            resource.env_animations = new EnvironmentAnimations(worldPath + "ENVIRONMENT_ANIMATION.DAT", Singleton.AnimationStrings_Debug);
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
                            resource.resources = new Resources(worldPath + "RESOURCES.BIN");
                            break;
                    }
                });
                Singleton.OnAssetsLoaded?.Invoke(this);
#if !CATHODE_FAIL_HARD
            }
            catch { }
#endif
        }

        public enum CacheMethod
        {
            CHECK_OR_POPULATE_CACHE,
            IGNORE_AND_OVERWRITE_CACHE,
            IGNORE_CACHE,
        }

        public ListViewItem GenerateListViewItem(Entity entity, Composite composite, CacheMethod cacheMethod = CacheMethod.CHECK_OR_POPULATE_CACHE)
        {
            if (cacheMethod == CacheMethod.CHECK_OR_POPULATE_CACHE)
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
                    if (funcComposite != null) item.SubItems.Add(EditorUtils.GetCompositeName(funcComposite));
                    else item.SubItems.Add(CathodeEntityDatabase.GetEntity(((FunctionEntity)entity).function).className);
                    break;
                case EntityVariant.ALIAS:
                    CommandsUtils.ResolveHierarchy(commands, composite, ((AliasEntity)entity).alias.path, out Composite c, out string s, false);
                    item.Text = s;
                    item.SubItems.Add("");
                    break;
                case EntityVariant.PROXY:
                    CommandsUtils.ResolveHierarchy(commands, composite, ((ProxyEntity)entity).proxy.path, out Composite c2, out string s2, false);
                    item.Text = EntityUtils.GetName(composite.shortGUID, entity.shortGUID);
                    item.SubItems.Add(s2);
                    break;
            }
            item.SubItems.Add(entity.shortGUID.ToByteString());

            switch (cacheMethod)
            {
                case CacheMethod.CHECK_OR_POPULATE_CACHE:
                    //we wanted to check the cache and it wasn't there, so lets add it
                    if (!composite_content_cache.ContainsKey(composite))
                    {
                        composite_content_cache.Add(composite, new Dictionary<Entity, ListViewItem>());
                    }
                    if (!composite_content_cache[composite].ContainsKey(entity))
                    {
                        composite_content_cache[composite].Add(entity, item);
                    }
                    break;

                case CacheMethod.IGNORE_AND_OVERWRITE_CACHE:
                    //we want to write (or overwrite) the cache, so lets do that
                    if (composite_content_cache.ContainsKey(composite))
                    {
                        if (composite_content_cache[composite].ContainsKey(entity))
                        {
                            composite_content_cache[composite][entity] = item;
                        }
                        else
                        {
                            composite_content_cache[composite].Add(entity, item);
                        }
                    }
                    else
                    {
                        composite_content_cache.Add(composite, new Dictionary<Entity, ListViewItem>());
                        composite_content_cache[composite].Add(entity, item);
                    }
                    break;
            }

            return item;
        }
    }
}
