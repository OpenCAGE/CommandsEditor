using CATHODE.Scripting;
using System.Collections.Generic;
using CommandsEditor.Popups.Base;
using CommandsEditor.DockPanels;
using CathodeLib;
using CATHODE;
using System.Windows;
using System.Linq;

namespace CommandsEditor
{
    public partial class ExportComposite : BaseWindow
    {
        CompositeDisplay _display;

        public ExportComposite(CompositeDisplay display) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, display.Content)
        {
            _display = display;

            InitializeComponent();

            levelList.BeginUpdate();
            levelList.Items.AddRange(Level.GetLevels(SharedData.pathToAI, true).ToArray());
            levelList.Items.Remove(Content.level);
            levelList.EndUpdate();

            levelList.SelectedIndex = 0;
        }

        private void export_Click(object sender, System.EventArgs e)
        {
            Commands commands = new Commands(SharedData.pathToAI + "/DATA/ENV/PRODUCTION/" + levelList.SelectedItem.ToString() + "/WORLD/COMMANDS.PAK");
            AddCompositesRecursively(_display.Composite, commands);
            commands.Save();

            MessageBox.Show("Finished exporting '" + _display.Composite.name + "' to ' " + levelList.SelectedItem.ToString() + "!", "Complete", MessageBoxButton.OK, MessageBoxImage.Information);

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
                commands.Entries.Add(composite);

            if (!recurse.Checked) return;

            foreach (FunctionEntity ent in composite.functions)
            {
                if (CommandsUtils.FunctionTypeExists(ent.function)) continue;

                Composite nestedComp = Content.commands.GetComposite(ent.function);
                if (nestedComp != null)
                    AddCompositesRecursively(nestedComp, commands);
            }
        }
    }
}
