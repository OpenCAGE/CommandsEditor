using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.DockPanels
{
    public partial class CompositeDisplay : DockContent
    {
        private CommandsDisplay _commandsDisplay;
        public CommandsDisplay CommandsDisplay => _commandsDisplay;
        public LevelContent Content => _commandsDisplay.Content;

        private Composite _composite;
        public Composite Composite => _composite;

        List<ListViewItem> composite_content_RAW = new List<ListViewItem>();
        private string currentSearch = "";

        private Dictionary<Entity, EntityDisplay> _entityDisplays = new Dictionary<Entity, EntityDisplay>();

        public CompositeDisplay(CommandsDisplay commandsDisplay, Composite composite)
        {
            _commandsDisplay = commandsDisplay;
            _composite = composite;

            InitializeComponent();
            this.Text = composite.name;

            commandsDisplay.Content.OnCompositeSelected?.Invoke(composite);

            Cursor.Current = Cursors.WaitCursor;
            CommandsUtils.PurgeDeadLinks(commandsDisplay.Content.commands, composite);
            PopulateListView();
            Cursor.Current = Cursors.Default;
        }

        private void PopulateListView()
        {
            composite_content.BeginUpdate();
            composite_content.Items.Clear();
            List<Entity> entities = _composite.GetEntities();
            for (int i = 0; i < entities.Count; i++)
                AddEntityToListView(entities[i]);
            composite_content.EndUpdate();
        }

        private void AddEntityToListView(Entity entity)
        {
            ListViewItem item = Content.GenerateListViewItem(entity, _composite);
            composite_content.Items.Add(item);
            composite_content_RAW.Add(item);
        }

        private void createEntity_Click(object sender, EventArgs e)
        {
            AddEntity add_parameter = new AddEntity(this);
            add_parameter.Show();
            add_parameter.OnNewEntity += OnAddNewEntity;
        }
        private void OnAddNewEntity(Entity entity)
        {
            ReloadUIForNewEntity(entity);
            LoadEntity(entity);
        }

        /* Perform a partial UI reload for a newly added entity */
        private void ReloadUIForNewEntity(Entity newEnt)
        {
            if (newEnt == null) return;
            if (currentSearch == "")
            {
                AddEntityToListView(newEnt);
            }
            else
            {
                PopulateListView();
            }
            LoadEntity(newEnt);
        }

        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (composite_content.SelectedItems.Count == 0) return;

            Entity entity = (Entity)composite_content.SelectedItems[0].Tag;
            LoadEntity(entity);
        }

        public void LoadEntity(Entity entity)
        {
            if (_entityDisplays.ContainsKey(entity))
            {
                _entityDisplays[entity].Activate();
            }
            else
            {
                EntityDisplay panel = new EntityDisplay(this, entity);
                panel.Show(Singleton.Editor.DockPanel, DockState.Document);
                panel.FormClosed += OnCompositePanelClosed;
                _entityDisplays.Add(entity, panel);
            }
        }

        private void OnCompositePanelClosed(object sender, FormClosedEventArgs e)
        {
            _entityDisplays.Remove(((EntityDisplay)sender).Entity);
        }

        public void CloseAllChildTabs()
        {
            List<EntityDisplay> displays = new List<EntityDisplay>();
            foreach (KeyValuePair<Entity, EntityDisplay> display in _entityDisplays)
                displays.Add(display.Value);
            foreach (EntityDisplay display in displays)
                display.Close();
        }

        private void entity_search_btn_Click(object sender, EventArgs e)
        {
            DoSearch();
        }
        private void DoSearch()
        {
            if (entity_search_box.Text == currentSearch) return;
            List<ListViewItem> matched = new List<ListViewItem>();
            foreach (ListViewItem item in composite_content_RAW) if (item.Text.ToUpper().Contains(entity_search_box.Text.ToUpper())) matched.Add(item);
            composite_content.BeginUpdate();
            composite_content.Items.Clear();
            for (int i = 0; i < matched.Count; i++) composite_content.Items.Add(matched[i]);
            composite_content.EndUpdate();
            currentSearch = entity_search_box.Text;
        }

        private void findUses_Click(object sender, EventArgs e)
        {
            ShowCompositeUses uses = new ShowCompositeUses(this);
            uses.Show();
            uses.OnEntitySelected += _commandsDisplay.LoadCompositeAndEntity;
        }

        private void deleteComposite_Click(object sender, EventArgs e)
        {
            _commandsDisplay.DeleteComposite(_composite);
        }
    }
}
