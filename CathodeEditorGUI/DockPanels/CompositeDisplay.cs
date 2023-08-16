using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using OpenCAGE;
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

        private List<ListViewItem> composite_content_RAW = new List<ListViewItem>();
        private string currentSearch = "";

        private Dictionary<Entity, EntityDisplay> _entityDisplays = new Dictionary<Entity, EntityDisplay>();

        private EntityDisplay _activeEntityDisplay = null;
        public EntityDisplay ActiveEntityDisplay => _activeEntityDisplay;

        public CompositeDisplay(CommandsDisplay commandsDisplay, Composite composite)
        {
            _commandsDisplay = commandsDisplay;
            _composite = composite;

            InitializeComponent();
            this.Text = composite.name;
            dockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;

            this.Activate();

            Cursor.Current = Cursors.WaitCursor;
            CommandsUtils.PurgeDeadLinks(commandsDisplay.Content.commands, composite);
            PopulateListView(_composite.GetEntities());
            Cursor.Current = Cursors.Default;
        }

        /* Reload this display */
        public void Reload(bool alsoReloadEntities = true)
        {
            PopulateListView(_composite.GetEntities());
            if (alsoReloadEntities) ReloadAllEntities();
        }
        
        /* Reload all entities loaded in this display */
        public void ReloadAllEntities()
        {
            foreach (KeyValuePair<Entity, EntityDisplay> display in _entityDisplays)
            {
                display.Value.Reload();
            }
        }

        /* Reload a specific entity's UI (if it is loaded) */
        public void ReloadEntity(Entity entity)
        {
            if (!_entityDisplays.ContainsKey(entity)) return;
            _entityDisplays[entity].Reload();
        }

        /* Monitor the currently active entity tab */
        private void DockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            EntityDisplay prevActiveEntityDisplay = _activeEntityDisplay;
            object content = ((DockPanel)sender).ActiveContent;

            if (content is EntityDisplay) 
                _activeEntityDisplay = (EntityDisplay)content;
            else 
                return;

            if (prevActiveEntityDisplay == _activeEntityDisplay) return;

            if (prevActiveEntityDisplay != null)
                prevActiveEntityDisplay.FormClosing -= OnActiveContentClosing;
            _activeEntityDisplay.FormClosing += OnActiveContentClosing;

            Singleton.OnEntitySelected?.Invoke(_activeEntityDisplay.Entity);
        }
        private void OnActiveContentClosing(object sender, FormClosingEventArgs e)
        {
            EntityDisplay prevActiveEntityDisplay = _activeEntityDisplay;
            _activeEntityDisplay = null;

            if (prevActiveEntityDisplay == _activeEntityDisplay) return;

            Singleton.OnEntitySelected?.Invoke(null);
        }

        private void PopulateListView(List<Entity> entities)
        {
            composite_content.BeginUpdate();
            composite_content.Items.Clear();

            bool hasID = composite_content.Columns.ContainsKey("ID");
            bool showID = SettingsManager.GetBool("CS_ShowEntityIDs");
            if (showID && !hasID)
                composite_content.Columns.Add(new ColumnHeader() { Name = "ID", Text = "ID", Width = 100 });
            else if (!showID && hasID)
                composite_content.Columns.RemoveByKey("ID");

            for (int i = 0; i < entities.Count; i++)
                AddEntityToListView(entities[i]);

            composite_content.EndUpdate();
        }

        private void AddEntityToListView(Entity entity)
        {
            ListViewItem item = Content.GenerateListViewItem(entity, _composite);
            item.Group = composite_content.Groups[(int)entity.variant];
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
                PopulateListView(_composite.GetEntities());
            }
            LoadEntity(newEnt);
        }

        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (composite_content.SelectedItems.Count == 0) return;

            Entity entity = (Entity)composite_content.SelectedItems[0].Tag;
            LoadEntity(entity);
        }

        /* Load an entity into the composite tabs UI */
        public void LoadEntity(ShortGuid guid)
        {
            LoadEntity(Composite.GetEntityByID(guid));
        }
        public void LoadEntity(Entity entity)
        {
            if (_entityDisplays.ContainsKey(entity))
            {
                _entityDisplays[entity].Reload();
                _entityDisplays[entity].Activate();
            }
            else
            {
                EntityDisplay panel = new EntityDisplay(this, entity);
                panel.Show(dockPanel, DockState.Document);
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

            List<Entity> allEntities = _composite.GetEntities();
            List<Entity> filteredEntities = new List<Entity>();

            foreach (Entity entity in allEntities)
            {
                foreach (ListViewItem.ListViewSubItem subitem in Content.composite_content_cache[Composite][entity].SubItems)
                {
                    if (!subitem.Text.ToUpper().Contains(entity_search_box.Text.ToUpper())) continue;

                    filteredEntities.Add(entity);
                    break;
                }
            }

            PopulateListView(filteredEntities);
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

        public void DeleteEntity(Entity entity)
        {
            if (MessageBox.Show("Are you sure you want to remove this entity?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            switch (entity.variant)
            {
                case EntityVariant.VARIABLE:
                    Composite.variables.Remove((VariableEntity)entity);
                    break;
                case EntityVariant.FUNCTION:
                    Composite.functions.Remove((FunctionEntity)entity);
                    break;
                case EntityVariant.OVERRIDE:
                    Composite.overrides.Remove((OverrideEntity)entity);
                    break;
                case EntityVariant.PROXY:
                    Composite.proxies.Remove((ProxyEntity)entity);
                    break;
            }

            List<Entity> entities = Composite.GetEntities();
            for (int i = 0; i < entities.Count; i++) //We should actually query every entity in the PAK, since we might be ref'd by a proxy or override
            {
                List<EntityLink> entLinks = new List<EntityLink>();
                for (int x = 0; x < entities[i].childLinks.Count; x++)
                {
                    if (entities[i].childLinks[x].childID != entity.shortGUID) entLinks.Add(entities[i].childLinks[x]);
                }
                entities[i].childLinks = entLinks;

                if (entities[i].variant == EntityVariant.FUNCTION)
                {
                    string entType = ShortGuidUtils.FindString(((FunctionEntity)entities[i]).function);
                    switch (entType)
                    {
                        case "TriggerSequence":
                            TriggerSequence triggerSequence = (TriggerSequence)entities[i];
                            List<TriggerSequence.Entity> triggers = new List<TriggerSequence.Entity>();
                            for (int x = 0; x < triggerSequence.entities.Count; x++)
                            {
                                if (triggerSequence.entities[x].connectedEntity.hierarchy.Count < 2 ||
                                    triggerSequence.entities[x].connectedEntity.hierarchy[triggerSequence.entities[x].connectedEntity.hierarchy.Count - 2] != entity.shortGUID)
                                {
                                    triggers.Add(triggerSequence.entities[x]);
                                }
                            }
                            triggerSequence.entities = triggers;
                            break;
                        case "CAGEAnimation":
                            CAGEAnimation cageAnim = (CAGEAnimation)entities[i];
                            List<CAGEAnimation.Connection> headers = new List<CAGEAnimation.Connection>();
                            for (int x = 0; x < cageAnim.connections.Count; x++)
                            {
                                if (cageAnim.connections[x].connectedEntity.hierarchy.Count < 2 ||
                                    cageAnim.connections[x].connectedEntity.hierarchy[cageAnim.connections[x].connectedEntity.hierarchy.Count - 2] != entity.shortGUID)
                                {
                                    headers.Add(cageAnim.connections[x]);
                                }
                            }
                            cageAnim.connections = headers;
                            break;
                    }
                }
            }

            PopulateListView(_composite.GetEntities());
            if (_entityDisplays.ContainsKey(entity))
                _entityDisplays[entity].Close();

            ReloadAllEntities();
        }

        public void DuplicateEntity(Entity entity)
        {
            if (MessageBox.Show("Are you sure you want to duplicate this entity?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            //Generate new entity ID and name
            Entity newEnt = null;
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    newEnt = ((FunctionEntity)entity).Copy();
                    break;
                case EntityVariant.VARIABLE:
                    newEnt = ((VariableEntity)entity).Copy();
                    break;
                case EntityVariant.OVERRIDE:
                    newEnt = ((OverrideEntity)entity).Copy();
                    break;
                case EntityVariant.PROXY:
                    newEnt = ((ProxyEntity)entity).Copy();
                    break;
            }
            newEnt.shortGUID = ShortGuidUtils.GenerateRandom();
            if (newEnt.variant != EntityVariant.VARIABLE)
                EntityUtils.SetName(
                    Composite.shortGUID,
                    newEnt.shortGUID,
                    EntityUtils.GetName(Composite.shortGUID, entity.shortGUID) + "_clone");

            //Add parent links in to this entity that linked in to the other entity
            List<Entity> ents = Composite.GetEntities();
            List<EntityLink> newLinks = new List<EntityLink>();
            int num_of_new_things = 1;
            foreach (Entity ent in ents)
            {
                newLinks.Clear();
                foreach (EntityLink link in ent.childLinks)
                {
                    if (link.childID == entity.shortGUID)
                    {
                        EntityLink newLink = new EntityLink();
                        newLink.connectionID = ShortGuidUtils.Generate(DateTime.Now.ToString("G") + num_of_new_things.ToString()); num_of_new_things++;
                        newLink.childID = newEnt.shortGUID;
                        newLink.childParamID = link.childParamID;
                        newLink.parentParamID = link.parentParamID;
                        newLinks.Add(newLink);
                    }
                }
                if (newLinks.Count != 0) ent.childLinks.AddRange(newLinks);
            }

            //Save back to composite
            switch (newEnt.variant)
            {
                case EntityVariant.FUNCTION:
                    Composite.functions.Add((FunctionEntity)newEnt);
                    break;
                case EntityVariant.VARIABLE:
                    Composite.variables.Add((VariableEntity)newEnt);
                    break;
                case EntityVariant.PROXY:
                    Composite.proxies.Add((ProxyEntity)newEnt);
                    break;
                case EntityVariant.OVERRIDE:
                    Composite.overrides.Add((OverrideEntity)newEnt);
                    break;
            }

            //Load in to UI
            ReloadUIForNewEntity(newEnt);
            _commandsDisplay.CacheHierarchies();
        }
    }
}
