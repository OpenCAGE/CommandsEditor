using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;
using WebSocketSharp;
using WeifenLuo.WinFormsUI.Docking;
using Path = System.IO.Path;

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

        private CompositePath _path = new CompositePath();
        public CompositePath Path => _path;

        private static Mutex _mut = new Mutex();
        private bool _canExportChildren = true;

        public CompositeDisplay(CommandsDisplay commandsDisplay, Composite composite)
        {
            _commandsDisplay = commandsDisplay;

            InitializeComponent();
            dockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;

            Load(composite);
        }

        private void Load(Composite composite)
        {
            Cursor.Current = Cursors.WaitCursor;

            findUses.Visible = Content.commands.EntryPoints[0] != composite;
            deleteComposite.Visible = Content.commands.EntryPoints[0] != composite;

            this.Text = composite.name;
            pathDisplay.Text = _path.GetPath(composite);
            _composite = composite;

            CommandsUtils.PurgeDeadLinks(Content.commands, composite);
            CloseAllChildTabs();
            Reload(false);
            this.Activate();

            Cursor.Current = Cursors.Default;
        }

        /* Load a child composite within this composite */
        public void LoadChild(Composite composite, Entity entity)
        {
            _path.StepForwards(_composite, entity);
            Load(composite);
        }

        /* Load the parent composite, one back from this composite */
        public void LoadParent()
        {
            if (_path.StepBackwards(out Composite composite, out Entity entity))
            {
                Load(composite);
                LoadEntity(entity);
            }
        }

        /* Reload this display */
        public void Reload(bool alsoReloadEntities = true)
        {
            PopulateListView(_composite.GetEntities());
            if (alsoReloadEntities) ReloadAllEntities();

            exportComposite.Enabled = false;
            Task.Factory.StartNew(() => UpdateExportCompositeVisibility());
        }

        /* Work out if we can export this composite: for now, we can't export composites that contain any resources, as the resource pointers would be wrong */
        private void UpdateExportCompositeVisibility()
        {
            try
            {
                _canExportChildren = true;
                bool visible = !DoesCompositeContainResource(_composite);
                EnableDisableButtonRun(visible);
            }
            catch { }
        }
        delegate void EnableDisableButtonRunDeleg(bool value);
        private void EnableDisableButtonRun(bool value)
        {
            if (toolStrip1.InvokeRequired)
            {
                this.toolStrip1.Invoke(new EnableDisableButtonRunDeleg
                 (EnableDisableButtonRun), value);
            }
            else
            {
                exportComposite.Enabled = value;
            }
        }
        private bool DoesCompositeContainResource(Composite comp)
        {
            bool found = false;
            Parallel.ForEach(comp.functions, (ent, state) =>
            {
                if (_canExportChildren && !CommandsUtils.FunctionTypeExists(ent.function))
                {
                    Composite nestedComp = Content.commands.GetComposite(ent.function);
                    if (nestedComp != null)
                    {
                        if (DoesCompositeContainResource(nestedComp))
                        {
                            _mut.WaitOne();
                            _canExportChildren = false;
                            _mut.ReleaseMutex();
                        }
                    }
                }

                if (ent.resources.Count != 0)
                {
                    found = true;
                    state.Stop();
                }

                Parameter resources = ent.GetParameter("resource");
                if (resources != null && ((cResource)resources.content).value.Count != 0)
                {
                    found = true;
                    state.Stop();
                }
            });
            return found;
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
            bool showID = SettingsManager.GetBool(Singleton.Settings.EntIdOpt);
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
            if (entity == null) return;

            if (SettingsManager.GetBool(Singleton.Settings.UseEntTabsOpt) == false)
                CloseAllChildTabs();

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

            composite_content.Focus();
        }

        private void OnCompositePanelClosed(object sender, FormClosedEventArgs e)
        {
            _entityDisplays.Remove(((EntityDisplay)sender).Entity);
        }

        public void CloseAllChildTabsExcept(Entity entity)
        {
            List<EntityDisplay> displays = new List<EntityDisplay>();
            foreach (KeyValuePair<Entity, EntityDisplay> display in _entityDisplays)
            {
                if (display.Key == entity) continue;
                displays.Add(display.Value);
            }
            foreach (EntityDisplay display in displays)
                display.Close();
        }
        public void CloseAllChildTabs()
        {
            CloseAllChildTabsExcept(null);
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
            ShowCompositeUses uses = new ShowCompositeUses(Content, Composite);
            uses.Show();
            uses.OnEntitySelected += _commandsDisplay.LoadCompositeAndEntity;
        }

        private void deleteComposite_Click(object sender, EventArgs e)
        {
            _commandsDisplay.DeleteComposite(_composite);
        }

        public void DeleteEntity(Entity entity, bool ask = true, bool reloadUI = true)
        {
            if (ask && MessageBox.Show("Are you sure you want to remove this entity?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            switch (entity.variant)
            {
                case EntityVariant.VARIABLE:
                    Composite.variables.Remove((VariableEntity)entity);
                    break;
                case EntityVariant.FUNCTION:
                    Composite.functions.Remove((FunctionEntity)entity);
                    break;
                case EntityVariant.ALIAS:
                    Composite.aliases.Remove((AliasEntity)entity);
                    break;
                case EntityVariant.PROXY:
                    Composite.proxies.Remove((ProxyEntity)entity);
                    break;
            }

            List<Entity> entities = Composite.GetEntities();
            for (int i = 0; i < entities.Count; i++) //We should actually query every entity in the PAK, since we might be ref'd by a proxy or alias
            {
                List<EntityConnector> entLinks = new List<EntityConnector>();
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
                                if (triggerSequence.entities[x].connectedEntity.path.Count < 2 ||
                                    triggerSequence.entities[x].connectedEntity.path[triggerSequence.entities[x].connectedEntity.path.Count - 2] != entity.shortGUID)
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
                                if (cageAnim.connections[x].connectedEntity.path.Count < 2 ||
                                    cageAnim.connections[x].connectedEntity.path[cageAnim.connections[x].connectedEntity.path.Count - 2] != entity.shortGUID)
                                {
                                    headers.Add(cageAnim.connections[x]);
                                }
                            }
                            cageAnim.connections = headers;
                            break;
                    }
                }
            }

            if (_entityDisplays.ContainsKey(entity))
                _entityDisplays[entity].Close();

            if (reloadUI)
            {
                PopulateListView(_composite.GetEntities());
                ReloadAllEntities();
            }
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
                case EntityVariant.ALIAS:
                    newEnt = ((AliasEntity)entity).Copy();
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
            List<EntityConnector> newLinks = new List<EntityConnector>();
            int num_of_new_things = 1;
            foreach (Entity ent in ents)
            {
                newLinks.Clear();
                foreach (EntityConnector link in ent.childLinks)
                {
                    if (link.childID == entity.shortGUID)
                    {
                        EntityConnector newLink = new EntityConnector();
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
                case EntityVariant.ALIAS:
                    Composite.aliases.Add((AliasEntity)newEnt);
                    break;
            }

            //Load in to UI
            ReloadUIForNewEntity(newEnt);
            _commandsDisplay.CacheHierarchies();
        }

        private void deleteCheckedEntities_Click(object sender, EventArgs e)
        {
            if (composite_content.CheckedItems.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected entities?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (ListViewItem item in composite_content.CheckedItems)
                DeleteEntity((Entity)item.Tag, false, false);

            PopulateListView(_composite.GetEntities());
            ReloadAllEntities();
        }

        private void exportComposite_Click(object sender, EventArgs e)
        {
            ExportComposite dialog = new ExportComposite(this, _canExportChildren);
            dialog.Show();
        }

        /* Context menu composite close options */
        private void closeSelected_Click(object sender, EventArgs e)
        {
            CloseAllChildTabs();
            Close();
        }

        private void createEntity_Click(object sender, EventArgs e)
        {

        }
        private void createVariableEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.VARIABLE);
        }
        private void createFunctionEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.FUNCTION);
        }
        private void createCompositeEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.FUNCTION, true);
        }
        private void createProxyEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.PROXY);
        }
        private void createOverrideEntityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.ALIAS);
        }

        AddEntity dialog = null;
        private void CreateEntity(EntityVariant variant = EntityVariant.FUNCTION, bool composite = false)
        {
            if (dialog != null && (dialog.Variant != variant || dialog.Composite != composite))
                dialog.Close();

            if (dialog == null) 
                dialog = new AddEntity(this, variant, composite);

            dialog.Show();
            dialog.OnNewEntity += OnAddNewEntity;
            dialog.FormClosed += Dialog_FormClosed;
        }
        private void OnAddNewEntity(Entity entity)
        {
            ReloadUIForNewEntity(entity);
            LoadEntity(entity);
        }
        private void Dialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            dialog = null;
        }

        private void goBackOnPath_Click(object sender, EventArgs e)
        {
            LoadParent();
        }
    }

    public class CompositePath
    {
        private List<Composite> _composites = new List<Composite>();
        private List<Entity> _entities = new List<Entity>();

        public void StepForwards(Composite prevComp, Entity entityFollowed)
        {
            _composites.Add(prevComp);
            _entities.Add(entityFollowed);
        }

        public bool StepBackwards(out Composite prevComp, out Entity entityFollowed)
        {
            if (_composites.Count == 0 || _entities.Count == 0)
            {
                prevComp = null;
                entityFollowed = null;
                return false;
            }

            prevComp = _composites[_composites.Count - 1];
            entityFollowed = _entities[_entities.Count - 1];

            _composites.RemoveAt(_composites.Count - 1);
            _entities.RemoveAt(_entities.Count - 1);

            return true;
        }

        public Composite PreviousComposite
        {
            get 
            {
                if (_composites.Count == 0) return null;
                return _composites[_composites.Count - 1];
            }
        }

        public Entity PreviousEntity
        {
            get
            {
                if (_entities.Count == 0) return null;
                return _entities[_entities.Count - 1];
            }
        }

        public string GetPath(Composite currentComp)
        {
            string path = "";
            for (int i = 0; i < _composites.Count; i++)
            {
                path += Path.GetFileName(_composites[i].name) + " > ";
            }
            path += Path.GetFileName(currentComp.name);
            return path;
        }

        public List<CompAndEnt> GetPathRich(Composite currentComp)
        {
            List<CompAndEnt> rich = new List<CompAndEnt>();
            for (int i = 0; i < _composites.Count; i++)
            {
                rich.Add(new CompAndEnt() { Composite = _composites[i], Entity = _entities[i] });
            }
            rich.Add(new CompAndEnt() { Composite = currentComp, Entity = null });
            return rich;
        }

        public struct CompAndEnt
        {
            public Composite Composite;
            public Entity Entity;
        }
    }
}
