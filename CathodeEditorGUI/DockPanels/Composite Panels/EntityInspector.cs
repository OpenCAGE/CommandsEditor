using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.Properties;
using CommandsEditor.UserControls;
using OpenCAGE;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static CommandsEditor.SelectEnumString;

namespace CommandsEditor.DockPanels
{
    public partial class EntityInspector : DockContent
    {
        private CompositeDisplay _compositeDisplay;
        public CompositeDisplay CompositeDisplay => _compositeDisplay;

        private Entity _entity = null;
        private Composite _entityCompositePtr = null; //The composite that this entity points to, if it does.

        public bool Populated => _entity != null;

        public LevelContent Content => _compositeDisplay.Content;

        public Entity Entity => _entity;
        public Composite Composite => _compositeDisplay.Composite;

        private bool _displayingLinks = true;
        public bool DisplayingLinks => _displayingLinks;

        public EntityInspector(CompositeDisplay compositeDisplay)
        {
            _compositeDisplay = compositeDisplay;

            this.FormClosing += (s, e) => { DepopulateUI(); };
            this.FormClosed += EntityDisplay_FormClosed;

            InitializeComponent();

            Singleton.OnEntityAddPending += OnEntityAddPending;
            Singleton.OnEntityAdded += OnEntityAdded;
            Singleton.OnEntityRenamed += OnEntityRenamed;
            Singleton.OnCompositeRenamed += OnCompositeRenamed;

            Reload();

            this.DockStateChanged += EntityInspector_DockStateChanged;

            this.CloseButtonVisible = false;
        }

        private void EntityInspector_DockStateChanged(object sender, EventArgs e)
        {
            Console.WriteLine(DockState);
            if (DockState == DockState.Unknown || DockState == DockState.Hidden)
                return;

            if (DockState == _previousDockState) return;
            _previousDockState = DockState;

            SettingsManager.SetString(Singleton.Settings.EntityInspectorState, DockState.ToString());
        }

        private void OnEntityAddPending()
        {
            if (_prevTask != null && !_prevTask.IsCompleted && _prevTaskToken != null)
            {
                _prevTaskToken.Cancel();
            }
        }
        private void OnEntityAdded(Entity e)
        {
            if (_prevTask != null && !_prevTask.IsCompleted)
            {
                StartBackgroundEntityLoader();
            }
        }

        //TODO: this is not as efficient as it could be: really we should only reload if we know we're affected by the rename
        private void OnEntityRenamed(Entity entity, string name)
        {
            if (!Populated) return;
            Reload();
        }
        private void OnCompositeRenamed(Composite composite, string name)
        {
            if (!Populated) return;
            Reload();
        }

        public void PopulateUI(Entity entity, bool displayLinks)
        {
            if (!this.Visible)
                this.Show();
            
            _entity = entity;
            _entityCompositePtr = _entity.variant == EntityVariant.FUNCTION ? Content.commands.GetComposite(((FunctionEntity)_entity).function) : null;

            switch (_entity.variant)
            {
                case EntityVariant.VARIABLE:
                    this.Icon = Resources.AnimatorController_Icon;
                    break;
                case EntityVariant.FUNCTION:
                    if (Content.commands.GetComposite(((FunctionEntity)_entity).function) == null)
                        this.Icon = Resources.d_ScriptableObject_Icon_braces_only;
                    else
                        this.Icon = Resources.d_PrefabVariant_Icon;
                    break;
                case EntityVariant.PROXY:
                    this.Icon = Resources.d_ScriptableObject_Icon;
                    break;
                case EntityVariant.ALIAS:
                    this.Icon = Resources.AreaEffector2D_Icon;
                    break;
            }

            Reload(displayLinks);

            this.Activate();
        }

        public void DepopulateUI()
        {
            this.Hide();
            EntityDisplay_FormClosed(null, null);
        }

        private void EntityDisplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.FormClosed -= EntityDisplay_FormClosed;
            Singleton.OnEntityAddPending -= OnEntityAddPending;
            Singleton.OnEntityAdded -= OnEntityAdded;
            Singleton.OnEntityRenamed -= OnEntityRenamed;
            Singleton.OnCompositeRenamed -= OnCompositeRenamed;

            for (int i = 0; i < entity_params.Controls.Count; i++)
            {
                if (entity_params.Controls[i] is GUI_Link)
                {
                    GUI_Link link = (GUI_Link)entity_params.Controls[i];
                    link.GoToEntity -= _compositeDisplay.LoadEntity;
                    link.OnLinkEdited -= OnLinkEdited;
                }
                entity_params.Controls[i].Dispose();
            }
            entity_params.Controls.Clear();

            _entity = null;
            _entityCompositePtr = null;

            imageList1.Images.Clear();
            imageList1.Dispose();

            if (add_parameter != null)
            {
                add_parameter.OnSaved -= Reload;
                add_parameter.Close();
            }
        }

        /* Reload this display */
        public void Reload() => Reload(_displayingLinks);
        public void Reload(bool displayLinks)
        {
#if DO_ENTITY_PERF_CHECK
            //TODO: The performance here is pretty poor. I should swap to using the PropertyGrid.
            Stopwatch timer = Stopwatch.StartNew();
            Console.WriteLine("[ENTITY RELOAD] ** START **");
#endif

            _displayingLinks = displayLinks;
            ModifyParameters.Visible = !_displayingLinks;

            //UI defaults - TODO: just set this in the designer.
            entityInfoGroup.Text = "Selected Entity Info";
            entityParamGroup.Text = "Selected Entity Parameters";
            selected_entity_type_description.Text = "";
            selected_entity_name.Text = "";
            for (int i = 0; i < entity_params.Controls.Count; i++)
            {
                if (entity_params.Controls[i] is ParameterUserControl)
                    ((ParameterUserControl)entity_params.Controls[i]).OnDeleted -= OnDeleteParam;

                entity_params.Controls[i].Dispose();
            }
            entity_params.Controls.Clear();
            jumpToComposite.Visible = false;
            editFunction.Enabled = false;
            editEntityResources.Enabled = false;
            editEntityMovers.Enabled = false;
            showOverridesAndProxies.Enabled = false;
            goToZone.Enabled = false;
            hierarchyDisplay.Visible = false;

            //NOTE: These visibility options should be mirrored in EntityListContextMenu_Opening in EntityList
            renameEntity.Enabled = _entity != null && _entity.variant != EntityVariant.ALIAS && _entity.variant != EntityVariant.VARIABLE; //TODO: we should support variable renaming, but doing that requires managing renaming all links/params (including node links)
            duplicateEntity.Enabled = _entity != null && _entity.variant != EntityVariant.ALIAS && _entity.variant != EntityVariant.VARIABLE; //This works, but why would you ever want to?
            deleteEntity.Enabled = _entity != null;

            ModifyParameters.Enabled = _entity != null;
            ModifyParameters_Link.Enabled = _entity != null;
            addLinkOut.Enabled = _entity != null;

            if (_entity == null)
            {
#if DO_ENTITY_PERF_CHECK
                timer.Stop();
#endif
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            StartBackgroundEntityLoader();
            List<Control> controls = new List<Control>();

            //populate info labels
            string entityVariantStr = "";
            switch (_entity.variant)
            {
                case EntityVariant.FUNCTION:
                    entityVariantStr = _entityCompositePtr != null ? "Composite Instance" : "Function";
                    break;
                case EntityVariant.VARIABLE:
                    //TODO: we should have a custom display for these. it's kinda weird to have parameters of parameters in this UI
                    entityVariantStr = "Composite Parameter";
                    break;
                case EntityVariant.PROXY:
                    entityVariantStr = "Proxy";
                    break;
                case EntityVariant.ALIAS:
                    entityVariantStr = "Alias";
                    break;
            }
            entityInfoGroup.Text = "Selected " + entityVariantStr + " Info";
            entityParamGroup.Text = "Selected " + entityVariantStr + " Parameters";

            //TODO: change this text contextually based on the linked editor - and hide the button when one isn't available.
            editFunction.Text = "Function";

            //TODO: we can correctly show the resources button now based on parameter info
            CompositePinInfoTable.PinInfo variableInfo = null;
            string description = "";
            switch (_entity.variant)
            {
                case EntityVariant.FUNCTION:
                    selected_entity_name.Text = EntityUtils.GetName(Composite.shortGUID, _entity.shortGUID);

                    //Composite Instance
                    if (_entityCompositePtr != null)
                    {
                        jumpToComposite.Visible = true;
                        editEntityResources.Enabled = false;
                        description = _entityCompositePtr.name;
                        //editFunction.Enabled = true;
                        //editFunction.Text = "Alias Overrides"; //TODO: show count?
                    }

                    //Function Entity
                    else
                    {
                        jumpToComposite.Visible = false;
                        editEntityResources.Enabled = (Content.resource.models != null); //TODO: we can hide this button completely outside of this state

                        FunctionType function = CommandsUtils.GetFunctionType(((FunctionEntity)_entity).function);
                        description = function.ToString();
                        editFunction.Enabled = function == FunctionType.CAGEAnimation || function == FunctionType.TriggerSequence || function == FunctionType.Character;
                    }
                    break;
                case EntityVariant.VARIABLE:
                    variableInfo = CompositeUtils.GetParameterInfo(Composite, (VariableEntity)Entity);
                    if (variableInfo == null)
                        Console.WriteLine("Warning: Could not get parameter pin info!");
                    description = (variableInfo != null ? variableInfo.PinTypeGUID.ToString() : ((VariableEntity)_entity).type.ToString());
                    selected_entity_name.Text = ShortGuidUtils.FindString(((VariableEntity)_entity).name);
                    break;
                case EntityVariant.PROXY:
                case EntityVariant.ALIAS:
                    hierarchyDisplay.Visible = true;
                    ShortGuid[] entityHierarchy = _entity.variant == EntityVariant.PROXY ? ((ProxyEntity)_entity).proxy.path : ((AliasEntity)_entity).alias.path;
                    Entity ent = CommandsUtils.ResolveHierarchy(Content.commands, Composite, entityHierarchy, out Composite comp, out string hierarchy, SettingsManager.GetBool("CS_ShowEntityIDs"));
                    hierarchyDisplay.Text = hierarchy;
                    jumpToComposite.Visible = true;
                    selected_entity_name.Text = (_entity.variant == EntityVariant.PROXY ? "Proxy to " : "Alias of ") + EntityUtils.GetName(comp, ent);
                    break;
                default:
                    selected_entity_name.Text = EntityUtils.GetName(Composite.shortGUID, _entity.shortGUID);
                    break;
            }
            selected_entity_type_description.Text = description;
            this.Text = selected_entity_name.Text;

            //show mvr editor button if this entity has a mvr link
            if (Content.mvr != null && Content.mvr.Entries.FindAll(o => o.entity.entity_id == this._entity.shortGUID).Count != 0)
                editEntityMovers.Enabled = true;

#if DO_ENTITY_PERF_CHECK
            Console.WriteLine($"[ENTITY RELOAD] METADATA UPDATE COMPLETED: {timer.Elapsed.TotalMilliseconds} ms");
#endif

            int current_ui_offset = 7;
            if (_displayingLinks)
            {
                //populate linked params IN
                List<Entity> ents = Composite.GetEntities();
                foreach (Entity ent in ents)
                {
                    foreach (EntityConnector link in ent.childLinks)
                    {
                        if (link.linkedEntityID != _entity.shortGUID) continue;
                        GUI_Link parameterGUI = new GUI_Link(this);
                        parameterGUI.PopulateUI(link, false, ent.shortGUID);
                        parameterGUI.TrackInstanceInfo(Composite.shortGUID, Entity.shortGUID, link.linkedParamID);
                        parameterGUI.HighlightAsModified(false); //For now, marking all links as "modified", given that they likely won't be default vals
                        parameterGUI.GoToEntity += _compositeDisplay.LoadEntity;
                        parameterGUI.OnLinkEdited += OnLinkEdited;
                        parameterGUI.Location = new Point(15, current_ui_offset);
                        parameterGUI.Width = entity_params.Width - 30;
                        parameterGUI.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                        current_ui_offset += parameterGUI.Height + 6;
                        controls.Add(parameterGUI);
                    }
                }
            }

#if DO_ENTITY_PERF_CHECK
            Console.WriteLine($"[ENTITY RELOAD] LINK IN CONTROLS COMPLETED: {timer.Elapsed.TotalMilliseconds} ms");
#endif

#if AUTO_POPULATE_PARAMS
            //make sure all defaults are applied to the entity so that we're showing everything
            //TODO: this should also factor in links in/out - if a link already exists then we shouldn't add it as a param (or it should add it and highlight it as such)
            if (!ParameterModificationTracker.IsDefaultsApplied(Composite.shortGUID, Entity.shortGUID))
            {
#if DEBUG
                int count_pre_add = _entity.parameters.Count;
#endif
                switch (_entity.variant)
                {
                    case EntityVariant.FUNCTION:
                        EntityUtils.ApplyDefaults((FunctionEntity)_entity, true, false);
                        break;
                    case EntityVariant.PROXY:
                        EntityUtils.ApplyDefaults((ProxyEntity)_entity, true, false);
                        break;
                }
                ParameterModificationTracker.SetDefaultsApplied(Composite.shortGUID, Entity.shortGUID);
#if DEBUG
                Console.WriteLine("Applied " + (_entity.parameters.Count - count_pre_add) + " defaults to entity.");
#endif
#if DO_ENTITY_PERF_CHECK
                Console.WriteLine($"[ENTITY RELOAD] DEFAULTS APPLIED: {timer.Elapsed.TotalMilliseconds} ms");
#endif
            }
#endif

            //TODO: this should be grouped by the functiontype they came from if that applies here. e.g. if it came from a base class, show it in another group.
            //TODO: if the type here is STRING, we should check to see if it's actually ENUM_STRING using ParameterUtils, then display the nice UI.

            //populate parameter inputs
            //NOTE: some pins are listed as params, because they specify the "delay" for the pin to be activated (both in and out) - i should display this info differently.

            _entity.parameters = _entity.parameters.OrderBy(o => o.name.ToString()).ToList();
            for (int i = 0; i < _entity.parameters.Count; i++)
            {
                //Use our metadata to update any wrongly typed cEnumStrings to get the nice UI
                if (_entity.parameters[i].content.dataType == DataType.STRING)
                {
                    ParameterData data = ParameterUtils.CreateDefaultParameterData(Entity, Composite, _entity.parameters[i].name);
                    if (data != null && data.dataType == DataType.ENUM_STRING)
                    {
                        ((cEnumString)data).value = ((cString)_entity.parameters[i].content).value;
                        _entity.parameters[i].content = data;
                    }
                }

                ParameterData this_param = _entity.parameters[i].content;
                ParameterUserControl parameterGUI = null;
                string paramName = _entity.parameters[i].name.ToString();
                switch (this_param.dataType)
                {
                    case DataType.TRANSFORM:
                        parameterGUI = new GUI_TransformDataType();
                        ((GUI_TransformDataType)parameterGUI).PopulateUI(_entity, (cTransform)this_param, paramName);
                        break;
                    case DataType.INTEGER:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Int((cInteger)this_param, paramName);
                        break;
                    case DataType.ENUM_STRING:
                        parameterGUI = new GUI_StringVariant_AssetDropdown();
                        ((GUI_StringVariant_AssetDropdown)parameterGUI).PopulateUI((cEnumString)this_param, paramName, false); //TODO: allow type selection?
                        break;
                    case DataType.STRING:
                        //TODO: Need an animation selector for the anim/skele pair
                        //TODO: There are some string types which should actually be selected via the EnumString UI like map_description on SetSubObjective, or unlocked_text on UI_Icon
                        parameterGUI = new GUI_StringDataType();
                        ((GUI_StringDataType)parameterGUI).PopulateUI((cString)this_param, paramName);
                        break;
                    case DataType.BOOL:
                        parameterGUI = new GUI_BoolDataType();
                        ((GUI_BoolDataType)parameterGUI).PopulateUI((cBool)this_param, paramName);
                        break;
                    case DataType.FLOAT:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Float((cFloat)this_param, paramName);
                        break;
                    case DataType.VECTOR:
                        //TODO: Should add a "colour" flag to handle this nicer.
                        switch (paramName)
                        {
                            case "AMBIENT_LIGHTING_COLOUR":
                            case "COLOUR_TINT_START":
                            case "COLOUR_TINT_MID":
                            case "COLOUR_TINT_END":
                            case "COLOUR_TINT":
                            case "COLOUR_TINT_OUTER":
                            case "DEPTH_INTERSECT_COLOUR_VALUE":
                            case "DEPTH_INTERSECT_INITIAL_COLOUR":
                            case "DEPTH_INTERSECT_MIDPOINT_COLOUR":
                            case "DEPTH_INTERSECT_END_COLOUR":
                            case "DEPTH_FOG_INITIAL_COLOUR":
                            case "DEPTH_FOG_MIDPOINT_COLOUR":
                            case "DEPTH_FOG_END_COLOUR":
                            case "ColourFactor":
                            case "lens_flare_colour":
                            case "light_shaft_colour":
                            case "initial_colour":
                            case "near_colour":
                            case "far_colour":
                            case "colour":
                            case "Colour":
                                parameterGUI = new GUI_VectorVariant_Colour();
                                ((GUI_VectorVariant_Colour)parameterGUI).PopulateUI((cVector3)this_param, paramName);
                                break;
                            default:
                                parameterGUI = new GUI_VectorDataType();
                                ((GUI_VectorDataType)parameterGUI).PopulateUI((cVector3)this_param, paramName);
                                break;
                        }
                        break;
                    case DataType.ENUM:
                        parameterGUI = new GUI_EnumDataType();
                        ParameterData defaultData = ParameterUtils.CreateDefaultParameterData(Entity, Composite, paramName);
                        ((GUI_EnumDataType)parameterGUI).PopulateUI((cEnum)this_param, paramName, defaultData == null || (defaultData.dataType == DataType.ENUM && ((cEnum)defaultData).enumID == ShortGuid.Invalid));
                        break;
                    case DataType.RESOURCE:
                        parameterGUI = new GUI_ResourceDataType();
                        ((GUI_ResourceDataType)parameterGUI).PopulateUI(this, (cResource)this_param, paramName);
                        break;
                    case DataType.SPLINE:
                        parameterGUI = new GUI_SplineDataType(_entity);
                        ((GUI_SplineDataType)parameterGUI).PopulateUI((cSpline)this_param, paramName);
                        break;
                }
                parameterGUI.Parameter = _entity.parameters[i];
                parameterGUI.OnDeleted += OnDeleteParam;
                parameterGUI.Location = new Point(15, current_ui_offset);
                parameterGUI.Width = entity_params.Width - 30;
                parameterGUI.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                current_ui_offset += parameterGUI.Height + 6;
                controls.Add(parameterGUI);

#if AUTO_POPULATE_PARAMS
                parameterGUI.TrackInstanceInfo(Composite.shortGUID, Entity.shortGUID, _entity.parameters[i].name);
                //Note: we always mark variable entity parameters as "modified", because they have no defaults - they're by definition variable.
                if (_entity.variant == EntityVariant.VARIABLE || ParameterModificationTracker.IsParameterModified(Composite.shortGUID, Entity.shortGUID, _entity.parameters[i].name))
                    parameterGUI.HighlightAsModified(false);
#endif
            }

            /*
            if (_entity.variant == EntityVariant.VARIABLE)
            {
                _entity.parameters = _entity.parameters.OrderBy(o => o.name.ToString()).ToList();

                for (int i = 0; i < _entity.parameters.Count; i++)
                {
                    ParameterUserControl parameterGUI = ParameterGroup.GenerateUserControl(_entity, _entity.parameters[i]);
                    parameterGUI.OnDeleted += OnDeleteParam;
                    parameterGUI.Location = new Point(15, current_ui_offset);
                    parameterGUI.Width = entity_params.Width - 30;
                    parameterGUI.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    current_ui_offset += parameterGUI.Height + 6;
                    controls.Add(parameterGUI);
                }
            }
            else
            {
                Dictionary<ShortGuid, List<Parameter>> parametersByImplementer = new Dictionary<ShortGuid, List<Parameter>>();
                for (int i = 0; i < _entity.parameters.Count; i++)
                {
                    (ParameterVariant?, DataType?, ShortGuid) metadata = ParameterUtils.GetParameterMetadata(_entity, _entity.parameters[i].name);

                    if (!parametersByImplementer.TryGetValue(metadata.Item3, out List<Parameter> parameters))
                    {
                        parameters = new List<Parameter>();
                        parametersByImplementer.Add(metadata.Item3, parameters);
                    }
                    parameters.Add(_entity.parameters[i]);
                }
                foreach (KeyValuePair<ShortGuid, List<Parameter>> implementedParams in parametersByImplementer)
                {
                    //NOTE: functiontype can be null if it's a composite instance: need to look up the composite to get name for group
                    ParameterGroup group = new ParameterGroup();
                    if (CommandsUtils.FunctionTypeExists(implementedParams.Key))
                    {
                        group.SetTitle(((FunctionType)implementedParams.Key.ToUInt32()).ToString());
                    }
                    else
                    {
                        Composite comp = Content.commands.GetComposite(implementedParams.Key);
                        if (comp != null)
                            group.SetTitle(Path.GetFileName(comp.name));
                    }
                    foreach (Parameter p in implementedParams.Value)
                    {
                        group.AddParameter(ParameterGroup.GenerateUserControl(_entity, p));
                    }
                }
            }
            */


#if DO_ENTITY_PERF_CHECK
            Console.WriteLine($"[ENTITY RELOAD] PARAMETER CONTROLS COMPLETED: {timer.Elapsed.TotalMilliseconds} ms");
#endif

            if (_displayingLinks)
            {
                //populate linked params OUT
                for (int i = 0; i < _entity.childLinks.Count; i++)
                {
                    GUI_Link parameterGUI = new GUI_Link(this);
                    parameterGUI.PopulateUI(_entity.childLinks[i], true);
                    parameterGUI.TrackInstanceInfo(Composite.shortGUID, Entity.shortGUID, _entity.childLinks[i].thisParamID);
                    parameterGUI.HighlightAsModified(false); //For now, marking all links as "modified", given that they likely won't be default vals
                    parameterGUI.GoToEntity += _compositeDisplay.LoadEntity;
                    parameterGUI.OnLinkEdited += OnLinkEdited;
                    parameterGUI.Location = new Point(15, current_ui_offset);
                    parameterGUI.Width = entity_params.Width - 30;
                    parameterGUI.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    current_ui_offset += parameterGUI.Height + 6;
                    controls.Add(parameterGUI);
                }
            }

#if DO_ENTITY_PERF_CHECK
            Console.WriteLine($"[ENTITY RELOAD] LINK OUT CONTROLS COMPLETED: {timer.Elapsed.TotalMilliseconds} ms");
#endif

            entity_params.SuspendLayout();
            entity_params.Controls.AddRange(controls.ToArray());
            entity_params.ResumeLayout();

#if DO_ENTITY_PERF_CHECK
            timer.Stop();
            Console.WriteLine($"[ENTITY RELOAD] ADDED CONTROLS TO WINDOW: {timer.Elapsed.TotalMilliseconds} ms");
#endif

            Singleton.OnEntityReloaded?.Invoke(_entity);
            Cursor.Current = Cursors.Default;
        }

        private void OnDeleteParam(Parameter param)
        {
            Singleton.OnParameterModified?.Invoke();
            _entity.parameters.Remove(param);
            _compositeDisplay.ReloadEntity(_entity);
        }

        private void OnLinkEdited(Entity orig, Entity linked)
        {
            Singleton.OnParameterModified?.Invoke();
            _compositeDisplay.ReloadEntity(orig);
            _compositeDisplay.ReloadEntity(linked);
        }

        private CancellationTokenSource _prevTaskToken = null;
        private Task _prevTask = null;
        private void StartBackgroundEntityLoader()
        {
            if (_prevTaskToken != null)
                _prevTaskToken.Cancel();

            _prevTaskToken = new CancellationTokenSource();
            _prevTask = Task.Run(() => BackgroundEntityLoader(_entity, this, _prevTaskToken.Token), _prevTaskToken.Token);
        }
        private void BackgroundEntityLoader(Entity ent, EntityInspector mainInst, CancellationToken ct)
        {
            bool isPointedTo = false;
            Composite zoneComp = null;
            FunctionEntity zoneEnt = null;
            Parallel.For(0, 2, (i) =>
            {
                switch (i)
                {
                    case 0:
                        isPointedTo = mainInst.Content.editor_utils.IsEntityReferencedExternally(ent, ct);
                        break;
                    case 1:
                        mainInst.Content.editor_utils.TryFindZoneForEntity(ent, mainInst.Composite, out zoneComp, out zoneEnt, ct);
                        break;
                }
            });
            mainInst.ThreadedEntityUIUpdate(ent, isPointedTo, zoneComp, zoneEnt);
        }
        private Composite zoneCompositeForSelectedEntity = null;
        private FunctionEntity zoneEntityForSelectedEntity = null;
        public void ThreadedEntityUIUpdate(Entity ent, bool isPointedTo, Composite zoneComp, FunctionEntity zoneEnt)
        {
            //TODO: we have an issue here where this can be called after the entitydisplay object has been disposed

            try
            {
                showOverridesAndProxies.Invoke(new Action(() => { showOverridesAndProxies.Enabled = isPointedTo; }));
                zoneCompositeForSelectedEntity = zoneComp;
                zoneEntityForSelectedEntity = zoneEnt;
                string zoneText = "Zone";
                if (zoneEnt != null)
                {
                    Parameter name = zoneEnt.GetParameter("name");
                    if (name != null) zoneText += " (" + ((cString)name.content).value + ")";
                }
                goToZone.Invoke(new Action(() => { goToZone.Enabled = zoneEnt != null; goToZone.Text = zoneText; }));
            }
            catch { }
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItem1.Enabled = _entity != null;
            createLinkToolStripMenuItem.Enabled = _entity != null;
            createLinkToolStripMenuItem.Visible = DisplayingLinks;
        }

        ModifyPinsOrParameters add_parameter;
        private void ModifyParameters_Click(object sender, EventArgs e)
        {
            if (add_parameter != null)
            {
                add_parameter.OnSaved -= Reload;
                add_parameter.Close();
            }
            
            add_parameter = new ModifyPinsOrParameters(this);
            add_parameter.Show();
            add_parameter.OnSaved += Reload;
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ModifyParameters_Click(null, null);
        }

        /* Add a new link out */
        private void addLinkOut_Click(object sender, EventArgs e)
        {
            AddOrEditLink add_link = new AddOrEditLink(this);
            add_link.Show();
            add_link.OnSaved += Reload;
        }
        private void createLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addLinkOut_Click(null, null);
        }

        private void editEntityMovers_Click(object sender, EventArgs e)
        {
            EditMVR moverEditor = new EditMVR(this);
            moverEditor.Show();
        }

        private void showOverridesAndProxies_Click(object sender, EventArgs e)
        {
            ShowCrossRefs crossRefs = new ShowCrossRefs(this);
            crossRefs.Show();
            crossRefs.OnEntitySelected += _compositeDisplay.CommandsDisplay.LoadCompositeAndEntity;
        }

        private void editEntityResources_Click(object sender, EventArgs e)
        {
            AddOrEditResource resourceEditor = new AddOrEditResource(this); 
            resourceEditor.Show();
            resourceEditor.OnSaved += OnResourceEditorSaved;
        }
        private void OnResourceEditorSaved(List<ResourceReference> resources)
        {
            ((FunctionEntity)Entity).resources = resources;
            Singleton.OnResourceModified?.Invoke();
        }

        private void goToZone_Click(object sender, EventArgs e)
        {
            CompositeDisplay display = _compositeDisplay;
            if (Composite != zoneCompositeForSelectedEntity)
                display = _compositeDisplay.CommandsDisplay.LoadComposite(zoneCompositeForSelectedEntity);

            display.LoadEntity(zoneEntityForSelectedEntity);
        }

        private void editFunction_Click(object sender, EventArgs e)
        {
            if (Entity.variant != EntityVariant.FUNCTION) return;
            if (_entityCompositePtr != null)
            {
                //Composite Instance
                ShowCompositeInstanceOverrides overrideDisplay = new ShowCompositeInstanceOverrides(this);
                overrideDisplay.Show();
            }
            else
            {
                //Function Entity
                FunctionType function = CommandsUtils.GetFunctionType(((FunctionEntity)Entity).function);
                switch (function)
                {
                    case FunctionType.CAGEAnimation:
                        Singleton.OnCAGEAnimationEditorOpened?.Invoke();
                        CAGEAnimationEditor cageAnimationEditor = new CAGEAnimationEditor(this);
                        cageAnimationEditor.Show();
                        cageAnimationEditor.OnSaved += CAGEAnimationEditor_OnSaved;
                        break;
                    case FunctionType.TriggerSequence:
                        TriggerSequenceEditor triggerSequenceEditor = new TriggerSequenceEditor(this);
                        triggerSequenceEditor.Show();
                        break;
                    case FunctionType.Character:
                        //TODO: I think this is only valid for entities with "custom_character_type" set - but working that out requires a complex parse of connected entities. So ignoring for now.
                        CharacterEditor characterEditor = new CharacterEditor(this);
                        characterEditor.Show();
                        break;
                }
            }
        }
        private void CAGEAnimationEditor_OnSaved(CAGEAnimation newEntity)
        {
            CAGEAnimation entity = (CAGEAnimation)Entity;
            entity.connections = newEntity.connections;
            entity.events = newEntity.events;
            entity.animations = newEntity.animations;
            entity.parameters = newEntity.parameters;
            Reload();
        }

        private void jumpToComposite_Click(object sender, EventArgs e)
        {
            switch (Entity.variant)
            {
                case EntityVariant.PROXY:
                    //Proxies forward directly to the entity they point to, breaking us out of the hierarchy.
                    Entity entity = CommandsUtils.ResolveHierarchy(Content.commands, Composite, ((ProxyEntity)Entity).proxy.path, out Composite flow, out string hierarchy);
                    if (MessageBox.Show("Jumping to a proxy will break you out of your composite.\nAre you sure?", "About to follow proxy...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        _compositeDisplay.CommandsDisplay.LoadCompositeAndEntity(flow, entity);
                    break;
                case EntityVariant.FUNCTION:
                    //Composite instances take us a step down the hierarchy.
                    _compositeDisplay.LoadChild(Content.commands.GetComposite(selected_entity_type_description.Text), Entity);
                    return;
                case EntityVariant.ALIAS:
                    //Aliases take us (potentially) multiple steps down the hierarchy.
                    ShortGuid[] aliasPath = ((AliasEntity)Entity).alias.path;
                    for (int i = 0; i < aliasPath.Length - 2; i++)
                        _compositeDisplay.LoadChild(Content.commands.GetComposite(((FunctionEntity)Composite.GetEntityByID(aliasPath[i])).function), Composite.GetEntityByID(aliasPath[i]));
                    _compositeDisplay.LoadEntity(Composite.GetEntityByID(aliasPath[aliasPath.Length - 2]));
                    return;
            }

        }

        private void deleteEntity_Click(object sender, EventArgs e)
        {
            _compositeDisplay.DeleteEntity(Entity);
        }

        private void duplicateEntity_Click(object sender, EventArgs e)
        {
            _compositeDisplay.DuplicateEntity(Entity);
        }

        private void renameEntity_Click(object sender, EventArgs e)
        {
            RenameEntity rename_entity = new RenameEntity(this.Entity, this.Composite);
            rename_entity.Show();
        }

        /* Context menu close entity */
        private void closeAll_Click(object sender, EventArgs e)
        {
            _compositeDisplay.CloseAllChildTabs();
        }
        private void closeSelected_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void closeAllBut_Click(object sender, EventArgs e)
        {
            _compositeDisplay.CloseAllChildTabsExcept(Entity);
        }
    }
}
