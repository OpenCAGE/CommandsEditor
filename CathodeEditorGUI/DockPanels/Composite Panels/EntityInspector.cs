using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
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
using static CommandsEditor.SelectSpecialString;

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
#if DEBUG
            Stopwatch timer = Stopwatch.StartNew();
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

            renameEntity.Visible = _entity != null && _entity.variant != EntityVariant.ALIAS && _entity.variant != EntityVariant.VARIABLE; //TODO: we should support variable renaming, but doing that requires managing renaming all links/params (including node links)
            deleteEntity.Visible = _entity != null;
            duplicateEntity.Visible = _entity != null;

            ModifyParameters.Enabled = _entity != null;
            ModifyParameters_Link.Enabled = _entity != null;
            addLinkOut.Enabled = _entity != null;

            if (_entity == null)
            {
#if DEBUG
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

                        ShortGuid thisFunction = ((FunctionEntity)_entity).function;
                        description = CathodeEntityDatabase.GetEntity(thisFunction).className;

                        FunctionType function = CommandsUtils.GetFunctionType(thisFunction);
                        editFunction.Enabled = function == FunctionType.CAGEAnimation || function == FunctionType.TriggerSequence || function == FunctionType.Character;
                    }
                    break;
                case EntityVariant.VARIABLE:
                    description = "DataType " + ((VariableEntity)_entity).type.ToString();
                    selected_entity_name.Text = ShortGuidUtils.FindString(((VariableEntity)_entity).name);
                    //renameSelectedNode.Enabled = false;
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

            //populate parameter inputs
            _entity.parameters = _entity.parameters.OrderBy(o => o.name.ToString()).ToList();
            for (int i = 0; i < _entity.parameters.Count; i++)
            {
                ParameterData this_param = _entity.parameters[i].content;
                ParameterUserControl parameterGUI = null;
                string paramName = _entity.parameters[i].name.ToString();
                switch (this_param.dataType)
                {
                    case DataType.TRANSFORM:
                        parameterGUI = new GUI_TransformDataType();
                        ((GUI_TransformDataType)parameterGUI).PopulateUI((cTransform)this_param, paramName);
                        break;
                    case DataType.INTEGER:
                        parameterGUI = new GUI_NumericDataType();
                        ((GUI_NumericDataType)parameterGUI).PopulateUI_Int((cInteger)this_param, paramName);
                        break;
                    case DataType.STRING:
                        /*
                        //TODO: handle this for proxies/aliases too...
                        if (entity.variant == EntityVariant.FUNCTION)
                        {
                            CathodeEntityDatabase.ParameterDefinition? info = CathodeEntityDatabase.GetParametersFromEntity(((FunctionEntity)entity).function).FirstOrDefault(o => o.name == paramName);
                            if (info != null)
                            {
                                switch (info.Value.datatype) //TODO: can we cast this to resource type enum?
                                {
                                    //TODO: use this instead of the hardcoded definitions below...
                                    case "SOUND_REVERB":

                                        break;
                                }
                            }
                        }
                        */

                        AssetList.Type asset = AssetList.Type.NONE;
                        string asset_arg = "";
                        //TODO: We can figure out a lot of these from the iOS dump.
                        //      For example - SoundEnvironmentMarker shows reverb_name as DataType SOUND_REVERB!
                        switch (paramName)
                        {
                            //case "Animation":
                            //    asset = AssetList.Type.ANIMATION;
                            //    break;
                            case "material":
                                asset = AssetList.Type.MATERIAL;
                                break;
                            case "title":
                            case "presence_id":
                            case "map_description":
                            case "content_title":
                            case "folder_title":
                            case "additional_info": //TODO: this is a good example of why we should handle this per-entity
                                asset = AssetList.Type.LOCALISED_STRING;
                                if (_entity.variant == EntityVariant.FUNCTION && CommandsUtils.GetFunctionType(((FunctionEntity)_entity).function).ToString().Contains("Objective"))
                                    asset_arg = "OBJECTIVES";
                                else if (_entity.variant == EntityVariant.FUNCTION && CommandsUtils.GetFunctionType(((FunctionEntity)_entity).function).ToString().Contains("Terminal"))
                                    asset_arg = "T0001/UI"; //TODO: we should also support TEXT dbs in the level folder for DLC stuff
                                else
                                    asset_arg = "UI";
                                break;
                            case "title_id":
                            case "message_id":
                            case "unlocked_text":
                            case "locked_text":
                            case "action_text":
                                asset = AssetList.Type.LOCALISED_STRING;
                                asset_arg = "UI";
                                break;
                            case "sound_event":
                            case "stop_sound_event":
                            case "music_event":
                            case "stop_event":
                            case "line_01_event":
                            case "line_02_event":
                            case "line_03_event":
                            case "line_04_event":
                            case "line_05_event":
                            case "line_06_event":
                            case "line_07_event":
                            case "line_08_event":
                            case "line_09_event":
                            case "line_10_event":
                            case "on_enter_event":
                            case "on_exit_event":
                            case "music_start_event":
                            case "music_end_event":
                            case "music_restart_event":
                                asset = AssetList.Type.SOUND_EVENT;
                                break;
                            case "reverb_name":
                                asset = AssetList.Type.SOUND_REVERB;
                                break;
                            case "sound_bank":
                                asset = AssetList.Type.SOUND_BANK;
                                break;
                        }
                        if (asset != AssetList.Type.NONE)
                        {
                            parameterGUI = new GUI_StringVariant_AssetDropdown();
                            ((GUI_StringVariant_AssetDropdown)parameterGUI).PopulateUI((cString)this_param, paramName, asset, asset_arg);
                        }
                        else
                        {
                            parameterGUI = new GUI_StringDataType();
                            ((GUI_StringDataType)parameterGUI).PopulateUI((cString)this_param, paramName);
                        }
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
                        ((GUI_EnumDataType)parameterGUI).PopulateUI((cEnum)this_param, paramName);
                        break;
                    case DataType.RESOURCE:
                        parameterGUI = new GUI_ResourceDataType();
                        ((GUI_ResourceDataType)parameterGUI).PopulateUI(this, (cResource)this_param, paramName);
                        break;
                    case DataType.SPLINE:
                        parameterGUI = new GUI_SplineDataType(this);
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
            }

            if (_displayingLinks)
            {
                //populate linked params OUT
                for (int i = 0; i < _entity.childLinks.Count; i++)
                {
                    GUI_Link parameterGUI = new GUI_Link(this);
                    parameterGUI.PopulateUI(_entity.childLinks[i], true);
                    parameterGUI.GoToEntity += _compositeDisplay.LoadEntity;
                    parameterGUI.OnLinkEdited += OnLinkEdited;
                    parameterGUI.Location = new Point(15, current_ui_offset);
                    parameterGUI.Width = entity_params.Width - 30;
                    parameterGUI.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                    current_ui_offset += parameterGUI.Height + 6;
                    controls.Add(parameterGUI);
                }
            }

            entity_params.SuspendLayout();
            entity_params.Controls.AddRange(controls.ToArray());
            entity_params.ResumeLayout();

#if DEBUG
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine($"Entity reload taken: {timeTaken.TotalMilliseconds} ms");
#endif

            Singleton.OnEntityReloaded?.Invoke(_entity);
            Cursor.Current = Cursors.Default;
        }

        private void OnDeleteParam(Parameter param)
        {
            _entity.parameters.Remove(param);
            _compositeDisplay.ReloadEntity(_entity);
        }

        private void OnLinkEdited(Entity orig, Entity linked)
        {
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
