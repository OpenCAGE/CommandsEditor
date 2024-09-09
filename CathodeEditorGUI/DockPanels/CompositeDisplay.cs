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
using System.Windows.Media.Animation;
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

        public bool Populated => _composite != null;

        private Composite _composite;
        public Composite Composite => _composite;

        private EntityList _entityList;
        private EntityFlowgraph _entityFlowgraph;

        private EntityDisplay _entityDisplay;
        public EntityDisplay EntityDisplay => _entityDisplay;

        private CompositePath _path = new CompositePath();
        public CompositePath Path => _path;

        private static Mutex _mut = new Mutex();
        private bool _canExportChildren = true;
        private bool _isSubbed = false;

        public CompositeDisplay(CommandsDisplay commandsDisplay)
        {
            _commandsDisplay = commandsDisplay;

            InitializeComponent();

            dockPanel.ShowDocumentIcon = true;

#if !DEBUG
            DEBUG_ShowAllInNodegraph.Visible = false;
#endif

            _entityList = new EntityList();
            _entityList.Show(dockPanel, DockState.DockLeft);
            //_entityList.DockState = DockState.DockLeftAutoHide; //<-- not using auto hide for now to get ui active for events

            _entityFlowgraph = new EntityFlowgraph();
            _entityFlowgraph.Show(dockPanel, DockState.Document);

            _entityDisplay = new EntityDisplay(this, false);
            _entityDisplay.Show(dockPanel, DockState.DockRight);
            _entityDisplay.FormClosing += OnEntityDisplayClosing;

            this.FormClosed += CompositeDisplay_FormClosed;
        }

        private void OnCompositeRenamed(Composite composite, string name)
        {
            if (!Populated || (!Path.AllComposites.Contains(composite) && composite != _composite)) return;
            this.Text = EditorUtils.GetCompositeName(_composite);
            pathDisplay.Text = _path.GetPath(_composite);
        }

        private void OnAddNewEntity(Entity entity)
        {
            ReloadUIForNewEntity(entity);
            LoadEntity(entity);
        }

        /* Call this to show the CompositeDisplay with the requested Composite content */
        public void PopulateUI(Composite composite)
        {
            if (!_isSubbed)
            {
                _entityList.List.SelectedEntityChanged += LoadEntity;
                Singleton.OnCompositeRenamed += OnCompositeRenamed;
                Singleton.OnEntityAdded += OnAddNewEntity;
                _isSubbed = true;
            }

            EditorUtils.CompositeType type = Content.editor_utils.GetCompositeType(composite);
            
            switch (type)
            {
                case EditorUtils.CompositeType.IS_ROOT:
                    this.Icon = Properties.Resources.globe;
                    break;
                case EditorUtils.CompositeType.IS_GLOBAL:
                case EditorUtils.CompositeType.IS_PAUSE_MENU:
                    this.Icon = Properties.Resources.cog;
                    break;
                case EditorUtils.CompositeType.IS_DISPLAY_MODEL:
                    this.Icon = Properties.Resources.Avatar_Icon;
                    break;
                case EditorUtils.CompositeType.IS_GENERIC_COMPOSITE:
                    this.Icon = Properties.Resources.d_Prefab_Icon;
                    break;
            }

            _entityFlowgraph.ShowComposite(composite);
            _entityList.List.Setup(composite, null, false);
            _path = new CompositePath();
            this.Text = EditorUtils.GetCompositeName(composite);

            Reload(composite);
            Singleton.OnCompositeSelected?.Invoke(_composite);
        }

        /* Call this to hide the CompositeDisplay */
        public void DepopulateUI()
        {
            this.Hide();
            CompositeDisplay_FormClosed(null, null);
        }

        private void CompositeDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            _entityList.List.SelectedEntityChanged -= LoadEntity;
            //this.FormClosed -= CompositeDisplay_FormClosed;
            Singleton.OnCompositeRenamed -= OnCompositeRenamed;
            Singleton.OnEntityAdded -= OnAddNewEntity;
            _isSubbed = false;

            if (dialog_var != null)
                dialog_var.Close();
            if (dialog_func != null)
                dialog_func.Close();
            if (dialog_compinst != null)
                dialog_compinst.Close();
            if (dialog_hierarchy != null)
                dialog_hierarchy.Close();

            if (_renameComposite != null)
                _renameComposite.FormClosed -= _renameComposite_FormClosed;
            if (_instanceInfoPopup != null)
                _instanceInfoPopup.FormClosed -= _instanceInfoPopup_FormClosed;
            if (_entityRenameDialog != null)
                _entityRenameDialog.FormClosed -= Rename_entity_FormClosed;

            _composite = null;
            _entityDisplay = null;

            CloseAllChildTabs();

            imageList.Images.Clear();
            imageList.Dispose();
            entityListIcons.Images.Clear();
            entityListIcons.Dispose();

            vS2015BlueTheme1.Dispose();
        }

        private void Reload(Composite composite)
        {
            Cursor.Current = Cursors.WaitCursor;

            findUses.Visible = Content.commands.EntryPoints[0] != composite;
            deleteComposite.Visible = !Content.commands.EntryPoints.Contains(composite);

            pathDisplay.Text = _path.GetPath(composite);
            _composite = composite;

            List<Entity> purged = CommandsUtils.PurgeDeadLinks(Content.commands, composite);
            for (int i = 0; i < purged.Count; i++)
                Singleton.OnEntityDeleted(purged[i]);
            CommandsUtils.PurgedComposites.purged.Add(composite.shortGUID);

            CloseAllChildTabs();
            Reload(false);
            this.Activate();

            _instanceInfoPopup?.Close();

#if DEBUG
            DEBUG_ShowAllInNodegraph.ForeColor = NodePositionDatabase.CanRestoreFlowgraph(_composite.name) ? Color.Green : Color.Red;
#endif

            Cursor.Current = Cursors.Default;
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
            _entityList.List.LoadComposite(Composite);
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
                    if (!allowedTypes.Contains(ent.resources[i].resource_type))
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
                        if (!allowedTypes.Contains(resourceRefs[i].resource_type))
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
            if (_entityDisplay.Populated)
                _entityDisplay.Reload();
        }

        /* Reload a specific entity's UI (if it is loaded) */
        public void ReloadEntity(Entity entity)
        {
            if (_entityDisplay != null && _entityDisplay.Entity == entity)
                _entityDisplay.Reload();
        }

        /* Perform a partial UI reload for a newly added entity */
        private void ReloadUIForNewEntity(Entity newEnt)
        {
            if (newEnt == null) return;
            _entityList.List.AddNewEntity(newEnt);
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

            //First, make sure the list has the right entity selected, then exit early to avoid loading twice.
            if (_entityList.List.SelectedEntity != entity)
            {
                _entityList.List.SelectEntity(entity);
                return;
            }

            _entityDisplay.PopulateUI(entity);

            _entityList.List.FocusOnList();
        }
        private void OnEntityDisplayClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            EntityDisplay display = (EntityDisplay)sender;
            display.DepopulateUI();
        }

        public void CloseAllChildTabsExcept(Entity entity)
        {
            if (_entityDisplay == null || _entityDisplay.Entity == entity)
                return;

            //Note: we don't actually close tabs here, we just hide them - they can be repurposed then instead of spawning new ones
            _entityDisplay.DepopulateUI();
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
                    if (entities[i].childLinks[x].linkedEntityID != entity.shortGUID) entLinks.Add(entities[i].childLinks[x]);
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

            CommandsUtils.PurgedComposites.purged.Clear(); //TODO: we should smartly remove from this list, rather than removing all

            if (_entityDisplay.Entity == entity && _entityDisplay.Populated)
                _entityDisplay.Close();

            if (reloadUI)
            {
                _entityList.List.LoadComposite(Composite);
                ReloadAllEntities();
            }

            Singleton.OnEntityDeleted?.Invoke(entity);
        }

        public void DuplicateEntity(Entity entity)
        {
            if (MessageBox.Show("Are you sure you want to duplicate this entity?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            AddCopyOfEntity(entity);
        }

        public void AddCopyOfEntity(Entity entity)
        {
            Singleton.OnEntityAddPending?.Invoke();
            Entity newEnt = MakeCopyOfEntity(entity);
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
            Content.editor_utils.GenerateCompositeInstances(Content.commands);
            Singleton.OnEntityAdded?.Invoke(newEnt);
        }

        private Entity MakeCopyOfEntity(Entity entity)
        {
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
            {
                EntityUtils.SetName(
                        Composite.shortGUID,
                        newEnt.shortGUID,
                        EntityUtils.GetName(Composite.shortGUID, entity.shortGUID) + "_clone");

                //TODO: not using the below, because really we should check every entity's name to get the index to append.
                /*
                string name = EntityUtils.GetName(Composite.shortGUID, entity.shortGUID);
                string[] vals = name.Split('_');
                if (vals.Length > 1 && int.TryParse(vals[vals.Length - 1], out int index))
                {
                    index += 1;
                    name = name.Substring(0, name.Length - vals[vals.Length - 1].Length) + index;
                }
                EntityUtils.SetName(
                    Composite.shortGUID,
                    newEnt.shortGUID,
                    name);
                */
            }

            //Add parent links in to this entity that linked in to the other entity
            List<Entity> ents = Composite.GetEntities();
            List<EntityConnector> newLinks = new List<EntityConnector>();
            int num_of_new_things = 1;
            foreach (Entity ent in ents)
            {
                newLinks.Clear();
                foreach (EntityConnector link in ent.childLinks)
                {
                    if (link.linkedEntityID == entity.shortGUID)
                    {
                        EntityConnector newLink = new EntityConnector();
                        newLink.ID = ShortGuidUtils.Generate(DateTime.Now.ToString("G") + num_of_new_things.ToString()); num_of_new_things++;
                        newLink.linkedEntityID = newEnt.shortGUID;
                        newLink.linkedParamID = link.linkedParamID;
                        newLink.thisParamID = link.thisParamID;
                        newLinks.Add(newLink);
                    }
                }
                if (newLinks.Count != 0) ent.childLinks.AddRange(newLinks);
            }

            //TODO should update entity ID on collision_map resource

#if DEBUG
            //If entity is a composite instance, check to see if it should make a new PHYSICS.MAP entry
            if (entity.variant == EntityVariant.FUNCTION)
            {
                Composite comp = Content.commands.GetComposite(((FunctionEntity)entity).function);

                //TODO: need to recurse into all child composite instances to find ALL contained PhysicsSystem functions, rather than just the layer below
                FunctionEntity phys = comp?.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.PhysicsSystem));
                if (phys != null)
                {
                    List<ShortGuid> instancesEnt = new List<ShortGuid>();
                    List<EntityPath> pathsEnt = Content.editor_utils.GetHierarchiesForEntity(Composite, entity);
                    List<ShortGuid> instancesPhys = new List<ShortGuid>();
                    List<EntityPath> pathsPhys = new List<EntityPath>();
                    pathsEnt.ForEach(path => {
                        instancesEnt.Add(path.GenerateInstance());

                        EntityPath pathPhys = path.Copy();
                        pathPhys.path.Insert(path.path.Count - 1, phys.shortGUID);
                        pathsPhys.Add(pathPhys);

                        instancesPhys.Add(pathPhys.GenerateInstance());
                    });

                    List<PhysicsMaps.Entry> physMaps = Content.resource.physics_maps.Entries.FindAll(physMap =>
                        instancesPhys.Contains(physMap.composite_instance_id) &&
                        physMap.entity.entity_id == entity.shortGUID &&
                        instancesEnt.Contains(physMap.entity.composite_instance_id)
                    );
                    physMaps.ForEach(physMap =>
                    {
                        PhysicsMaps.Entry newPhysMap = physMap.Copy();
                        newPhysMap.entity.entity_id = newEnt.shortGUID;

                        EntityPath pathPhys = pathsPhys.FirstOrDefault(x => x.GenerateInstance() == physMap.composite_instance_id);
                        EntityPath newPathPhys = pathPhys.Copy();
                        newPathPhys.path[newPathPhys.path.Count - 3] = newEnt.shortGUID;
                        newPhysMap.composite_instance_id = newPathPhys.GenerateInstance();
                        Content.resource.physics_maps.Entries.Add(newPhysMap);
                        //Content.resource.physics_maps.Entries[Content.resource.physics_maps.Entries.IndexOf(physMap)] = newPhysMap;

                        //TODO: need to set pos/rot properly

                        Resources.Resource physRes = Content.resource.resources.Entries.FirstOrDefault(res => res.composite_instance_id == physMap.composite_instance_id);
                        Resources.Resource newPhysRes = physRes.Copy();
                        newPhysRes.composite_instance_id = newPhysMap.composite_instance_id;
                        //newPhysRes.index = Content.resource.resources.Entries.Count;
                        Content.resource.resources.Entries.Add(newPhysRes);
                        //Content.resource.resources.Entries[Content.resource.resources.Entries.IndexOf(p)] = resPhys;
                    });
                }
            }
#endif

            return newEnt;
        }

        private void deleteCheckedEntities_Click(object sender, EventArgs e)
        {
            List<Entity> entities = _entityList.List.CheckedEntities;

            if (entities.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected entities?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (Entity entity in entities)
                DeleteEntity(entity, false, false);

            _entityList.List.LoadComposite(Composite);
            ReloadAllEntities();
        }

        private void exportComposite_Click(object sender, EventArgs e)
        {
            ExportComposite dialog = new ExportComposite(Composite, _canExportChildren);
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

        AddEntity_Variable dialog_var = null;
        AddEntity_Function dialog_func = null;
        AddEntity_CompositeInstance dialog_compinst = null;
        SelectHierarchy dialog_hierarchy = null; EntityVariant dialog_hierarchy_entvar;
        private void CreateEntity(EntityVariant variant = EntityVariant.FUNCTION, bool composite = false)
        {
            if (variant == EntityVariant.FUNCTION && !composite)
            {
                if (dialog_func != null)
                    dialog_func.Close();

                dialog_func = new AddEntity_Function(Composite);
                dialog_func.Show();
                dialog_func.Focus();
            }
            else if (variant == EntityVariant.FUNCTION && composite)
            {
                if (dialog_compinst != null)
                    dialog_compinst.Close();

                dialog_compinst = new AddEntity_CompositeInstance(Composite);
                dialog_compinst.Show();
                dialog_compinst.Focus();
            }
            else if (variant == EntityVariant.PROXY || variant == EntityVariant.ALIAS)
            {
                if (dialog_hierarchy != null)
                    dialog_hierarchy.Close();

                dialog_hierarchy_entvar = variant;
                switch (dialog_hierarchy_entvar)
                {
                    case EntityVariant.PROXY:
                        dialog_hierarchy = new SelectHierarchy(Content.commands.EntryPoints[0], new CompositeEntityList.DisplayOptions()
                        {
                            DisplayAliases = false,
                            DisplayFunctions = true,
                            DisplayProxies = false,
                            DisplayVariables = false,
                        });
                        dialog_hierarchy.Text = "Create Proxy";
                        break;
                    case EntityVariant.ALIAS:
                        dialog_hierarchy = new SelectHierarchy(_composite, new CompositeEntityList.DisplayOptions()
                        {
                            DisplayAliases = false,
                            DisplayFunctions = true,
                            DisplayProxies = true,
                            DisplayVariables = true,
                        });
                        dialog_hierarchy.Text = "Create Alias";
                        break;
                }
                dialog_hierarchy.OnHierarchyGenerated += OnNewEntityHierarchyGenerated;
                dialog_hierarchy.Show();
                dialog_hierarchy.Focus();
            }
            else if (variant == EntityVariant.VARIABLE)
            {
                if (dialog_var != null)
                    dialog_var.Close();

                dialog_var = new AddEntity_Variable(Composite);
                dialog_var.Show();
                dialog_var.Focus();
            }
        }
        private void OnNewEntityHierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            Singleton.OnEntityAddPending?.Invoke();

            Entity ent = null;
            switch (dialog_hierarchy_entvar)
            {
                case EntityVariant.PROXY:
                    List<ShortGuid> hierarchy = new List<ShortGuid>();
                    hierarchy.Add(Content.commands.EntryPoints[0].shortGUID);
                    hierarchy.AddRange(generatedHierarchy);
                    ent = _composite.AddProxy(Content.commands, hierarchy); //TODO: re-add "add default params"
                    Entity pointedEnt = ((ProxyEntity)ent).proxy.GetPointedEntity(Content.commands, out Composite pointedComp);
                    EntityUtils.SetName(_composite, ent, EntityUtils.GetName(pointedComp, pointedEnt) + " Proxy");
                    break;
                case EntityVariant.ALIAS:
                    hierarchy = generatedHierarchy;
                    ent = _composite.AddAlias(hierarchy); //TODO: re-add "add default params"?
                    break;
            }

            Singleton.OnEntityAdded?.Invoke(ent);
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
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteEntity(_entityList.List.SelectedEntity);
        }
        RenameEntity _entityRenameDialog = null;
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_entityRenameDialog != null)
                _entityRenameDialog.Close();

            _entityRenameDialog = new RenameEntity(_entityList.List.SelectedEntity, this.Composite);
            _entityRenameDialog.Show();
            _entityRenameDialog.FormClosed += Rename_entity_FormClosed;
        }
        private void Rename_entity_FormClosed(object sender, FormClosedEventArgs e)
        {
            _entityRenameDialog = null;
        }
        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DuplicateEntity(_entityList.List.SelectedEntity);
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

        RenameComposite _renameComposite;
        private void renameComposite_Click(object sender, EventArgs e)
        {
            if (_renameComposite != null)
                _renameComposite.Close();

            string name = EditorUtils.GetCompositeName(_composite);
            string path = (name == _composite.name) ? "" : _composite.name.Substring(0, _composite.name.Length - name.Length - 1);

            _renameComposite = new RenameComposite(_composite, path.Replace('\\', '/'));
            _renameComposite.Show();
            _renameComposite.FormClosed += _renameComposite_FormClosed;
        }
        private void _renameComposite_FormClosed(object sender, FormClosedEventArgs e)
        {
            _renameComposite = null;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorClipboard.Entity = _entityList.List.SelectedEntity;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCopyOfEntity(EditorClipboard.Entity);
        }

        private void DEBUG_ShowAllInNodegraph_Click(object sender, EventArgs e)
        {
            EntityFlowgraph form = new EntityFlowgraph();
            form.ShowComposite(Composite);
            form.Show();
#if DEBUG
            form.FormClosed += DEBUG_ShowAllInNodegraph_FormClosed;
        }
        private void DEBUG_ShowAllInNodegraph_FormClosed(object sender, FormClosedEventArgs e)
        {
            DEBUG_ShowAllInNodegraph.ForeColor = NodePositionDatabase.CanRestoreFlowgraph(_composite.name) ? Color.Green : Color.Red;
#endif
        }
    }
}
