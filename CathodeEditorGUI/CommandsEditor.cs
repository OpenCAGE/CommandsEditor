using CATHODE;
using CATHODE.Scripting;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor
{
    public partial class CommandsEditor : Form
    {
        public DockPanel DockPanel => dockPanel;

        CommandsDisplay _commandsDisplay = null;

        public CommandsEditor()
        {
            Singleton.Editor = this;

            InitializeComponent();

            //Set title
            this.Text = "OpenCAGE Commands Editor";
            if (OpenCAGE.SettingsManager.GetBool("CONFIG_ShowPlatform") &&
                OpenCAGE.SettingsManager.GetString("META_GameVersion") != "")
            {
                switch (OpenCAGE.SettingsManager.GetString("META_GameVersion"))
                {
                    case "STEAM":
                        this.Text += " - Steam";
                        break;
                    case "EPIC_GAMES_STORE":
                        this.Text += " - Epic Games Store";
                        break;
                    case "GOG":
                        this.Text += " - GoG";
                        break;
                }
            }

            //Populate localised text string databases (in English)
            List<string> textList = Directory.GetFiles(SharedData.pathToAI + "/DATA/TEXT/ENGLISH/", "*.TXT", SearchOption.AllDirectories).ToList<string>();
            foreach (string text in textList)
                Singleton.Strings.Add(Path.GetFileNameWithoutExtension(text), new Strings(text));

            //Load animation data - this should be quick enough to not worry about waiting for the thread
            Task.Factory.StartNew(() => LoadAnimData());
        }

        /* Load anim data */
        public static void LoadAnimData()
        {
            //Load animation data
            PAK2 animPAK = new PAK2(SharedData.pathToAI + "/DATA/GLOBAL/ANIMATION.PAK");

            //Load all male/female skeletons
            List<PAK2.File> skeletons = animPAK.Entries.FindAll(o => o.Filename.Length > 17 && o.Filename.Substring(0, 17) == "DATA\\SKELETONDEFS");
            for (int i = 0; i < skeletons.Count; i++)
            {
                string skeleton = Path.GetFileNameWithoutExtension(skeletons[i].Filename);
                File.WriteAllBytes(skeleton, skeletons[i].Content);
                XmlNode skeletonType = new BML(skeleton).Content.SelectSingleNode("//SkeletonDef/LoResReferenceSkeleton");
                if (skeletonType?.InnerText == "MALE" || skeletonType?.InnerText == "FEMALENPC")
                {
                    if (!Singleton.Skeletons.ContainsKey(skeletonType?.InnerText))
                        Singleton.Skeletons.Add(skeletonType?.InnerText, new List<string>());
                    Singleton.Skeletons[skeletonType?.InnerText].Add(skeleton);
                }
                File.Delete(skeleton);
            }

            //Anim string db
            File.WriteAllBytes("ANIM_STRING_DB.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB.BIN")).Content);
            Singleton.AnimationStrings = new AnimationStrings("ANIM_STRING_DB.BIN");
            File.Delete("ANIM_STRING_DB.BIN");

            //Debug anim string db
            File.WriteAllBytes("ANIM_STRING_DB_DEBUG.BIN", animPAK.Entries.FirstOrDefault(o => o.Filename.Contains("ANIM_STRING_DB_DEBUG.BIN")).Content);
            Singleton.AnimationStrings_Debug = new AnimationStrings("ANIM_STRING_DB_DEBUG.BIN");
            File.Delete("ANIM_STRING_DB_DEBUG.BIN");

            animPAK = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void OnFormClose(object sender, CancelEventArgs e)
        {

        }

        private void loadLevel_Click(object sender, EventArgs e)
        {
            SelectLevel dialog = new SelectLevel();
            dialog.Show();
            dialog.OnLevelSelected += OnLevelSelected;
        }
        private void OnLevelSelected(string level)
        {
            //Close all existing
            if (_commandsDisplay != null)
            {
                _commandsDisplay.CloseAllChildTabs();
                _commandsDisplay.Close();
            }

            //Load new
            _commandsDisplay = new CommandsDisplay(level);
            _commandsDisplay.Show(Singleton.Editor.DockPanel, DockState.DockLeft);
            _commandsDisplay.CloseButtonVisible = false;
        }

        private void saveLevel_Click(object sender, EventArgs e)
        {
            if (_commandsDisplay == null) return;
            Cursor.Current = Cursors.WaitCursor;

#if !CATHODE_FAIL_HARD
            byte[] backup = null;
            try
            {
                backup = File.ReadAllBytes(Editor.commands.Filepath);
#endif
                _commandsDisplay.Content.commands.Save();
#if !CATHODE_FAIL_HARD
            }
            catch (Exception ex)
            {
                try
                {
                    if (backup != null)
                        File.WriteAllBytes(Editor.commands.Filepath, backup);
                }
                catch { }
            
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Failed to save changes!\n" + ex.Message, "Failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#endif

            //Calculate instance specific stuff
            _commandsDisplay.Content.editor_utils.GenerateCompositeInstances(_commandsDisplay.Content.commands); //TODO: Do we need to do this? I don't think we should.
            CreateDataForInstance(_commandsDisplay.Content.commands.EntryPoints[0]);

            Cursor.Current = Cursors.Default;
            MessageBox.Show("Saved changes!", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void CreateDataForInstance(Composite composite)
        {
            for (int i = 0; i < composite.functions.Count; i++) //todo: can we do this in parallel?
            {
                if (CommandsUtils.FunctionTypeExists(composite.functions[i].function))
                {
                    //This is a FunctionEntity which we may need to create data for the instance of

                    //Writing this code, I'm now realising that we would need to write these extra files to know the indexes to then write in Commands.
                    //It must all be done at the same time...
                    //Hmm...
                    for (int x = 0; x < composite.functions[i].resources.Count; x++)
                    {
                        ResourceReference resource = composite.functions[i].resources[x];
                        switch (resource.entryType)
                        {
                            case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                                //Write to PHYSICS.MAP
                                break;
                            case ResourceType.COLLISION_MAPPING:
                                //Write to COLLISION.MAP
                                break;
                            case ResourceType.RENDERABLE_INSTANCE:
                                //Write to MODELS.MVR
                                break;
                            case ResourceType.ANIMATED_MODEL:
                                //Write to ENVIRONMENT_ANIMATION.DAT
                                break;
                        }
                    }
                }
                else
                {
                    //This is a FunctionEntity which instances a child composite, so we should follow it through
                    Composite child = _commandsDisplay.Content.commands.GetComposite(composite.functions[i].function);
                    if (child != null) CreateDataForInstance(child);
                }
            }

            _commandsDisplay.Content.resource.physics_maps.Save();
            _commandsDisplay.Content.resource.collision_maps.Save();
            _commandsDisplay.Content.resource.character_accessories.Save();
            _commandsDisplay.Content.resource.reds.Save();
            _commandsDisplay.Content.mvr.Save();
        }
    }
}
