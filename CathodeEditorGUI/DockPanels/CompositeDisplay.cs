using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.Popups.UserControls;
using ListViewGroupCollapse;
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
using System.Windows.Forms.Design;
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

        private List<EntityDisplay> _entityDisplays = new List<EntityDisplay>();

        private EntityDisplay _activeEntityDisplay = null;
        public EntityDisplay ActiveEntityDisplay => _activeEntityDisplay;

        private CompositePath _path = new CompositePath();
        public CompositePath Path => _path;

        private static Mutex _mut = new Mutex();
        private bool _canExportChildren = true;

        private const int _defaultSplitterDistance = 500;

        public CompositeDisplay(CommandsDisplay commandsDisplay)
        {
            _commandsDisplay = commandsDisplay;

            InitializeComponent();

            dockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;
            dockPanel.ShowDocumentIcon = true;

            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.SplitterDistance = SettingsManager.GetInteger(Singleton.Settings.EntitySplitWidth, _defaultSplitterDistance);

            compositeEntityList1.SelectedEntityChanged += LoadEntity;
            compositeEntityList1.ContextMenuStrip = EntityListContextMenu;

            this.FormClosed += CompositeDisplay_FormClosed;
        }

        /* Call this to show the CompositeDisplay with the requested Composite content */
        public void PopulateUI(Composite composite)
        {
            EditorUtils.CompositeType type = Content.editor_utils.GetCompositeType(composite);

            if (type == EditorUtils.CompositeType.IS_ROOT)
                this.Icon = Properties.Resources.globe;
            else if (type == EditorUtils.CompositeType.IS_GLOBAL || type == EditorUtils.CompositeType.IS_PAUSE_MENU)
                this.Icon = Properties.Resources.cog;
            else if (type == EditorUtils.CompositeType.IS_DISPLAY_MODEL)
                this.Icon = Properties.Resources.Avatar_Icon;

            compositeEntityList1.Setup(composite);

            Reload(composite);
        }

        /* Call this to hide the CompositeDisplay */
        public void DepopulateUI()
        {
            this.Hide();
            CompositeDisplay_FormClosed(null, null);
        }

        private void CompositeDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            _composite = null;
            _activeEntityDisplay = null;

            CloseAllChildTabs();

            if (sender != null && e != null)
                _entityDisplays.Clear();
        }

        private void Reload(Composite composite)
        {
            Cursor.Current = Cursors.WaitCursor;

            bool isCoreComposite = !Content.commands.EntryPoints.Contains(composite);
            findUses.Visible = isCoreComposite;
            deleteComposite.Visible = isCoreComposite;

            this.Text = EditorUtils.GetCompositeName(composite);
            pathDisplay.Text = _path.GetPath(composite);
            _composite = composite;

            CommandsUtils.PurgeDeadLinks(Content.commands, composite);
            CloseAllChildTabs();
            Reload(false);
            this.Activate();

            _instanceInfoPopup?.Close();

            Cursor.Current = Cursors.Default;
        }

        public void ResetSplitter()
        {
            splitContainer1.SplitterDistance = _defaultSplitterDistance;
        }

        /* Load a child composite within this composite */
        public void LoadChild(Composite composite, Entity entity)
        {
            _path.StepForwards(_composite, entity);
            Reload(composite);
        }

        /* Load the parent composite, one back from this composite */
        public void LoadParent()
        {
            if (_path.StepBackwards(out Composite composite, out Entity entity))
            {
                Reload(composite);
                LoadEntity(entity);
            }
        }

        /* Reload this display */
        public void Reload(bool alsoReloadEntities = true)
        {
            compositeEntityList1.LoadComposite(Composite);
            if (alsoReloadEntities) ReloadAllEntities();

            exportComposite.Enabled = false;
            Task.Factory.StartNew(() => UpdateExportCompositeVisibility());

            Singleton.OnCompositeReloaded?.Invoke(_composite);
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
            List<ResourceType> allowedTypes = new List<ResourceType>();
            allowedTypes.Add(ResourceType.ANIMATED_MODEL);
            allowedTypes.Add(ResourceType.RENDERABLE_INSTANCE);
            allowedTypes.Add(ResourceType.COLLISION_MAPPING);

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

                for (int i = 0; i < ent.resources.Count; i++)
                {
                    if (!allowedTypes.Contains(ent.resources[i].entryType))
                    {
                        found = true;
                        state.Stop();
                    }
                }

                Parameter resources = ent.GetParameter("resource");
                if (resources != null)
                {
                    List<ResourceReference> resourceRefs = ((cResource)resources.content).value;
                    for (int i = 0; i < resourceRefs.Count; i++)
                    {
                        if (!allowedTypes.Contains(resourceRefs[i].entryType))
                        {
                            found = true;
                            state.Stop();
                        }
                    }
                }
            });
            return found;
        }

        /* Reload all entities loaded in this display */
        public void ReloadAllEntities()
        {
            for (int i = 0; i < _entityDisplays.Count; i++)
            {
                if (!_entityDisplays[i].Visible) continue;
                _entityDisplays[i].Reload();
            }
        }

        /* Reload a specific entity's UI (if it is loaded) */
        public void ReloadEntity(Entity entity)
        {
            _entityDisplays.FirstOrDefault(o => o.Entity == entity && o.Visible)?.Reload();
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

        /* Perform a partial UI reload for a newly added entity */
        private void ReloadUIForNewEntity(Entity newEnt)
        {
            if (newEnt == null) return;
            compositeEntityList1.AddNewEntity(newEnt);
            LoadEntity(newEnt);
        }

        /* Load an entity into the composite tabs UI */
        public void LoadEntity(ShortGuid guid)
        {
            LoadEntity(Composite.GetEntityByID(guid));
        }
        public void LoadEntity(Entity entity)
        {
            if (entity == null) return;

            EntityDisplay display = null;
            if (SettingsManager.GetBool(Singleton.Settings.UseEntityTabs))
            {
                //If tabbing is enabled, try & find an unused background tab for us to repurpose
                for (int i = 0; i < _entityDisplays.Count; i++)
                {
                    if (_entityDisplays[i].Visible) continue;
                    display = _entityDisplays[i];
                    break;
                }
                //Otherwise, spawn a new tab
                if (display == null)
                {
                    display = new EntityDisplay(this);
                    display.Show(dockPanel, DockState.Document);
                    display.FormClosing += OnEntityDisplayClosing;
                    _entityDisplays.Add(display);
                }
            }
            else
            {
                //If tabbing is disabled, make the tab if it hasn't been made already
                if (_entityDisplays.Count == 0)
                {
                    display = new EntityDisplay(this);
                    display.Show(dockPanel, DockState.Document);
                    display.FormClosing += OnEntityDisplayClosing;
                    _entityDisplays.Add(display);
                }
                //Otherwise just repurpose the one that was already made
                else
                {
                    display = _entityDisplays[0];
                }
            }
            display.PopulateUI(entity);

            compositeEntityList1.FocusOnList();
        }
        private void OnEntityDisplayClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            EntityDisplay display = (EntityDisplay)sender;
            display.DepopulateUI();
        }

        public void CloseAllChildTabsExcept(Entity entity)
        {
            //Note: we don't actually close tabs here, we just hide them - they can be repurposed then instead of spawning new ones
            List<EntityDisplay> toClose = _entityDisplays.FindAll(o => o.Entity != entity);
            for (int i = 0; i < toClose.Count; i++)
                toClose[i].DepopulateUI();
        }
        public void CloseAllChildTabs()
        {
            CloseAllChildTabsExcept(null);
        }

        private void findUses_Click(object sender, EventArgs e)
        {
            ShowCompositeUses uses = new ShowCompositeUses(Composite);
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

            _entityDisplays.FirstOrDefault(o => o.Entity == entity && o.Visible)?.Close();

            if (reloadUI)
            {
                compositeEntityList1.LoadComposite(Composite);
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
            List<Entity> entities = compositeEntityList1.CheckedEntities;

            if (entities.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected entities?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (Entity entity in entities)
                DeleteEntity(entity, false, false);

            compositeEntityList1.LoadComposite(Composite);
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
        private void createAliasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.ALIAS);
        }

        AddEntity dialog = null;
        private void CreateEntity(EntityVariant variant = EntityVariant.FUNCTION, bool composite = false)
        {
            if (dialog != null && (dialog.Variant != variant || dialog.Composite != composite))
                dialog.Close();

            if (dialog == null)
            {
                dialog = new AddEntity(this, variant, composite);
                dialog.OnNewEntity += OnAddNewEntity;
                dialog.FormClosed += Dialog_FormClosed;
            }

            dialog.Show();
            dialog.Focus();
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

        ShowInstanceInfo _instanceInfoPopup = null;
        private void instanceInfo_Click(object sender, EventArgs e)
        {
            if (_instanceInfoPopup != null)
            {
                _instanceInfoPopup.BringToFront();
                _instanceInfoPopup.Focus();
                return;
            }

            _instanceInfoPopup = new ShowInstanceInfo(this);
            _instanceInfoPopup.Show();
            _instanceInfoPopup.FormClosed += _instanceInfoPopup_FormClosed;
        }
        private void _instanceInfoPopup_FormClosed(object sender, FormClosedEventArgs e)
        {
            _instanceInfoPopup = null;
        }

        /* Entity List Context Menu */
        private void FooListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var lv = sender as ListViewExtended;
                var item = lv.HitTest(e.Location).Item;

                deleteToolStripMenuItem.Enabled = item != null;
                renameToolStripMenuItem.Enabled = item != null;
                duplicateToolStripMenuItem.Enabled = item != null;

                if (item != null)
                    lv.FocusedItem = item;

                EntityListContextMenu.Show(lv, e.Location);
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteEntity(compositeEntityList1.SelectedEntity);
        }
        RenameEntity _entityRenameDialog = null;
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_entityRenameDialog != null)
                _entityRenameDialog.Close();

            _entityRenameDialog = new RenameEntity(compositeEntityList1.SelectedEntity, this.Composite);
            _entityRenameDialog.Show();
            _entityRenameDialog.OnRenamed += OnEntityRenamed;
            _entityRenameDialog.FormClosed += Rename_entity_FormClosed;
        }
        private void OnEntityRenamed(string name, Entity entity)
        {
            Content.composite_content_cache[Composite][entity].Text = name;
            CommandsDisplay.ReloadAllEntities();
            //TODO-URGENT: Also need to update Proxy/Alias hierarchies.
        }
        private void Rename_entity_FormClosed(object sender, FormClosedEventArgs e)
        {
            _entityRenameDialog = null;
        }
        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DuplicateEntity(compositeEntityList1.SelectedEntity);
        }
        private void createParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.VARIABLE);
        }
        private void createInstanceOfCompositeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.FUNCTION, true);
        }
        private void createFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.FUNCTION);
        }
        private void createProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.PROXY);
        }
        private void createAliasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CreateEntity(EntityVariant.ALIAS);
        }

        //disable entity-related actions on the context menu if no entity is selected
        private void EntityListContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasSelectedEntity = compositeEntityList1.SelectedEntity != null;

            deleteToolStripMenuItem.Enabled = hasSelectedEntity;
            renameToolStripMenuItem.Enabled = hasSelectedEntity;
            duplicateToolStripMenuItem.Enabled = hasSelectedEntity;
        }

        //UI: handle saving split container width between composites/runs 
        private void dockPanel_Resize(object sender, EventArgs e)
        {
            SettingsManager.SetInteger(Singleton.Settings.EntitySplitWidth, splitContainer1.SplitterDistance);
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

        public List<Composite> AllComposites
        {
            get
            {
                return _composites;
            }
        }

        public List<Entity> AllEntities
        {
            get
            {
                return _entities;
            }
        }

        public string GetPath(Composite currentComp)
        {
            string path = "";
            for (int i = 0; i < _composites.Count; i++)
            {
                path += EditorUtils.GetCompositeName(_composites[i]) + " > ";
            }
            path += EditorUtils.GetCompositeName(currentComp);
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
