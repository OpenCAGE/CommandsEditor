using CATHODE.Scripting;
using System.Collections.Generic;
using CommandsEditor.Popups.Base;
using CommandsEditor.DockPanels;
using CathodeLib;
using CATHODE;
using System.Windows;
using System.Linq;
using WebSocketSharp;

namespace CommandsEditor
{
    public partial class ExportComposite : BaseWindow
    {
        CompositeDisplay _display;
        LevelContent _destinationContent;

        public ExportComposite(CompositeDisplay display, bool canExportChildren) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, display.Content)
        {
            _display = display;

            InitializeComponent();

            levelList.BeginUpdate();
            levelList.Items.AddRange(Level.GetLevels(SharedData.pathToAI, true).ToArray());
            levelList.Items.Remove(Content.level);
            levelList.EndUpdate();

            levelList.SelectedIndex = 0;

            this.Text = "Port '" + _display.Composite.name + "'";
            
            if (!canExportChildren)
            {
                recurse.Checked = false;
                recurse.Enabled = false;
            }
        }

        private void export_Click(object sender, System.EventArgs e)
        {
            _destinationContent = new LevelContent(levelList.SelectedItem.ToString());

            AddCompositesRecursively(_display.Composite, _destinationContent.commands);

            _destinationContent.commands.Save();
            //todo: save other things too

            MessageBox.Show("Finished porting '" + _display.Composite.name + "' to ' " + levelList.SelectedItem.ToString() + "!", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void AddCompositesRecursively(Composite composite, Commands commands)
        {
            Composite dest = commands.Entries.FirstOrDefault(o => o.shortGUID == composite.shortGUID);
            if (overwrite.Checked)
            {
                if (dest != null)
                    commands.Entries.Remove(dest);
                dest = null;
            }
            if (dest == null)
            {
                foreach (FunctionEntity ent in composite.functions)
                {
                    for (int i = 0; i < ent.resources.Count; i++)
                        ent.resources[i] = CopyResource(ent.resources[i]);

                    Parameter resources = ent.GetParameter("resource");
                    if (resources != null)
                        for (int i = 0; i < ((cResource)resources.content).value.Count; i++)
                            ((cResource)resources.content).value[i] = CopyResource(((cResource)resources.content).value[i]);
                }

                commands.Entries.Add(composite);
            }

            if (!recurse.Checked) return;

            foreach (FunctionEntity ent in composite.functions)
            {
                if (CommandsUtils.FunctionTypeExists(ent.function)) continue;

                Composite nestedComp = Content.commands.GetComposite(ent.function);
                if (nestedComp != null)
                    AddCompositesRecursively(nestedComp, commands);
            }
        }

        private ResourceReference CopyResource(ResourceReference resource)
        {
            switch (resource.entryType)
            {
                //TODO: THESE SHOULD BE HANDLED DIFFERENTLY ANYWAY. WE SHOULD HANDLE THEM WHEN WRITING THE COMMANDS.PAK, NOT HERE. WE NEED TO KNOW THE WHOLE CONTEXT.
                case ResourceType.DYNAMIC_PHYSICS_SYSTEM:
                    //Write to PHYSICS.MAP
                    break;
                case ResourceType.COLLISION_MAPPING:
                    //Write to COLLISION.MAP
                    break;
                //----

                case ResourceType.RENDERABLE_INSTANCE:
                    //Write to MODELS/TEXTURES/MATERIALS
                    break;
                case ResourceType.ANIMATED_MODEL:
                    //todo: need to verify we don't need to update any other stuff on resource ent here
                    //todo: also need to verify the char doesn't exist in file already? not the end of the world
                    _destinationContent.resource.env_animations.Entries.Add(Content.resource.env_animations.Entries[resource.index]);
                    resource.index = _destinationContent.resource.env_animations.Entries.Count - 1;
                    //Write to ENVIRONMENT_ANIMATION.DAT
                    break;
                default:
                    //TODO: we want to get to a point where we never encounter this before releasing to main
                    // - it might be a case that we don't need to copy every type of resource, and only RENDERABLE_INSTANCE and characters etc...
                    MessageBox.Show("Encountered an unexpected resource type!!!");
                    return null;
            }
            return null; //todo: return updated resoure ref pointing to copied one
        }
    }
}
