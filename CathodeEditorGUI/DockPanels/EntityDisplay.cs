using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.DockPanels
{
    public partial class EntityDisplay : DockContent
    {
        private CompositeDisplay _compositeDisplay;
        private Entity _entity;

        public LevelContent Content => _compositeDisplay.Content;

        public Entity Entity => _entity;
        public Composite Composite => _compositeDisplay.Composite;

        private List<Entity> parentEntities = new List<Entity>();
        private List<Entity> childEntities = new List<Entity>();

        public EntityDisplay(CompositeDisplay compositeDisplay, Entity entity)
        {
            if (entity == null)
            {
                this.Close();
                return;
            }

            _entity = entity;
            _compositeDisplay = compositeDisplay;

            InitializeComponent();

            Content.OnEntitySelected?.Invoke(entity);

            Cursor.Current = Cursors.WaitCursor;
            entity_params.SuspendLayout();
            Task.Factory.StartNew(() => BackgroundEntityLoader(entity, this));

            //populate info labels
            entityInfoGroup.Text = "Selected " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.variant.ToString().ToLower().Replace('_', ' ')) + " Entity Info";
            entityParamGroup.Text = "Selected " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.variant.ToString().ToLower().Replace('_', ' ')) + " Entity Parameters";
            string description = "";
            switch (entity.variant)
            {
                case EntityVariant.FUNCTION:
                    ShortGuid thisFunction = ((FunctionEntity)entity).function;
                    Composite funcComposite = Content.commands.GetComposite(thisFunction);
                    jumpToComposite.Visible = funcComposite != null;
                    if (funcComposite != null)
                        description = funcComposite.name;
                    else
                        description = CathodeEntityDatabase.GetEntity(thisFunction).className;
                    selected_entity_name.Text = EntityUtils.GetName(Composite.shortGUID, entity.shortGUID);
                    if (funcComposite == null)
                    {
                        FunctionType function = CommandsUtils.GetFunctionType(thisFunction);
                        editFunction.Enabled = function == FunctionType.CAGEAnimation || function == FunctionType.TriggerSequence || function == FunctionType.Character;
                    }
                    editEntityResources.Enabled = (Content.resource.models != null);
                    break;
                case EntityVariant.VARIABLE:
                    description = "DataType " + ((VariableEntity)entity).type.ToString();
                    selected_entity_name.Text = ShortGuidUtils.FindString(((VariableEntity)entity).name);
                    //renameSelectedNode.Enabled = false;
                    break;
                case EntityVariant.PROXY:
                case EntityVariant.OVERRIDE:
                    hierarchyDisplay.Visible = true;
                    string hierarchy = "";
                    if (entity.variant == EntityVariant.PROXY) CommandsUtils.ResolveHierarchy(Content.commands, Composite, ((ProxyEntity)entity).connectedEntity.hierarchy, out Composite comp, out hierarchy);
                    else CommandsUtils.ResolveHierarchy(Content.commands, Composite, ((OverrideEntity)entity).connectedEntity.hierarchy, out Composite comp, out hierarchy);
                    hierarchyDisplay.Text = hierarchy;
                    jumpToComposite.Visible = true;
                    selected_entity_name.Text = EntityUtils.GetName(Composite.shortGUID, entity.shortGUID);
                    break;
                default:
                    selected_entity_name.Text = EntityUtils.GetName(Composite.shortGUID, entity.shortGUID);
                    break;
            }
            selected_entity_type_description.Text = description;
            this.Text = selected_entity_name.Text;

            //show mvr editor button if this entity has a mvr link
            if (Content.mvr != null && Content.mvr.Entries.FindAll(o => o.entity.entity_id == _entity.shortGUID).Count != 0)
                editEntityMovers.Enabled = true;

            //populate linked params IN
            parentEntities.Clear();
            int current_ui_offset = 7;
            List<Entity> ents = Composite.GetEntities();
            foreach (Entity ent in ents)
            {
                foreach (EntityLink link in ent.childLinks)
                {
                    if (link.childID != entity.shortGUID) continue;
                    GUI_Link parameterGUI = new GUI_Link(this);
                    parameterGUI.PopulateUI(link, false, ent.shortGUID);
                    parameterGUI.GoToEntity += compositeDisplay.LoadEntity;
                    parameterGUI.Location = new Point(15, current_ui_offset);
                    current_ui_offset += parameterGUI.Height + 6;
                    entity_params.Controls.Add(parameterGUI);
                    parentEntities.Add(ent);
                }
            }

            //populate parameter inputs
            entity.parameters = entity.parameters.OrderBy(o => o.name.ToString()).ToList();
            for (int i = 0; i < entity.parameters.Count; i++)
            {
                ParameterData this_param = entity.parameters[i].content;
                UserControl parameterGUI = null;
                string paramName = entity.parameters[i].name.ToString();
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
                        //TODO: handle this for proxies/overrides too...
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
                                if (entity.variant == EntityVariant.FUNCTION && CommandsUtils.GetFunctionType(((FunctionEntity)entity).function).ToString().Contains("Objective"))
                                    asset_arg = "OBJECTIVES";
                                else if (entity.variant == EntityVariant.FUNCTION && CommandsUtils.GetFunctionType(((FunctionEntity)entity).function).ToString().Contains("Terminal"))
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
                            parameterGUI = new GUI_StringVariant_AssetDropdown(Content);
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
                        parameterGUI = new GUI_ResourceDataType(Content);
                        ((GUI_ResourceDataType)parameterGUI).PopulateUI((cResource)this_param, paramName);
                        break;
                    case DataType.SPLINE:
                        parameterGUI = new GUI_SplineDataType(this);
                        ((GUI_SplineDataType)parameterGUI).PopulateUI((cSpline)this_param, paramName);
                        break;
                }
                parameterGUI.Location = new Point(15, current_ui_offset);
                current_ui_offset += parameterGUI.Height + 6;
                entity_params.Controls.Add(parameterGUI);
            }

            //populate linked params OUT
            childEntities.Clear();
            for (int i = 0; i < entity.childLinks.Count; i++)
            {
                GUI_Link parameterGUI = new GUI_Link(this);
                parameterGUI.PopulateUI(entity.childLinks[i], true);
                parameterGUI.GoToEntity += compositeDisplay.LoadEntity;
                parameterGUI.Location = new Point(15, current_ui_offset);
                current_ui_offset += parameterGUI.Height + 6;
                entity_params.Controls.Add(parameterGUI);
                childEntities.Add(Composite.GetEntityByID(entity.childLinks[i].childID));
            }

            //TODO: REIMPLEMENT THIS
            //Update node viewer if it's open
            //if (nodeViewer != null)
            //{
            //    nodeViewer.AddEntities(Editor.selected.composite, Editor.selected.entity);
            //}

            entity_params.ResumeLayout();
            Cursor.Current = Cursors.Default;
        }
        private void BackgroundEntityLoader(Entity ent, EntityDisplay mainInst)
        {
            bool isPointedTo = mainInst.Content.editor_utils.IsEntityReferencedExternally(ent);
            mainInst.Content.editor_utils.TryFindZoneForEntity(ent, mainInst.Composite, out Composite zoneComp, out FunctionEntity zoneEnt);
            mainInst.ThreadedEntityUIUpdate(ent, isPointedTo, zoneComp, zoneEnt);
        }
        private Composite zoneCompositeForSelectedEntity = null;
        private FunctionEntity zoneEntityForSelectedEntity = null;
        public void ThreadedEntityUIUpdate(Entity ent, bool isPointedTo, Composite zoneComp, FunctionEntity zoneEnt)
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

        /* Add a new parameter */
        private void addNewParameter_Click(object sender, EventArgs e)
        {/*
            AddParameter add_parameter = new AddParameter(this, _entity);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }
        private void refresh_entity_event(Object sender, FormClosedEventArgs e)
        {
            LoadEntity(Editor.selected.entity);
            this.BringToFront();
            this.Focus();*/
        }

        /* Add a new link out */
        private void addLinkOut_Click(object sender, EventArgs e)
        {
            /*
            if (Editor.selected.entity == null) return;
            AddOrEditLink add_link = new AddOrEditLink(this, Editor.selected.composite, Editor.selected.entity);
            add_link.Show();
            add_link.FormClosed += new FormClosedEventHandler(refresh_entity_event);*/
        }

        /* Remove a parameter */
        private void removeParameter_Click(object sender, EventArgs e)
        {/*
            if (Editor.selected.entity == null) return;
            if (entity_params.Controls.Count == 0) return;
            if (Editor.selected.entity.childLinks.Count + Editor.selected.entity.parameters.Count == 0) return;
            RemoveParameter remove_parameter = new RemoveParameter(this, Editor.selected.entity);
            remove_parameter.Show();
            remove_parameter.FormClosed += new FormClosedEventHandler(refresh_entity_event);*/
        }
    }
}
