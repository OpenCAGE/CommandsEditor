using CATHODE;
using CATHODE.Commands;
using CathodeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddEntity : Form
    {
        public Action<Entity> OnNewEntity;

        Composite composite = null;
        List<Composite> composites = null;
        List<CathodeEntityDatabase.EntityDefinition> availableEntities = null;
        List<ShortGuid> hierarchy = null;

        public CathodeEditorGUI_AddEntity(Composite _comp, List<Composite> _comps)
        {
            composite = _comp;
            composites = _comps.OrderBy(o => o.name).ToList();
            availableEntities = CathodeEntityDatabase.GetEntities();
            InitializeComponent();

            //quick hack to reload dropdown
            createDatatypeEntity.Checked = true;
            createFunctionEntity.Checked = true;
        }

        //Repopulate UI
        private void selectedDatatypeEntity(object sender, EventArgs e)
        {
            //Datatype
            entityVariant.Visible = true;
            label2.Visible = true;
            generateHierarchy.Visible = false;
            createNewEntity.Enabled = true;
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            entityVariant.Items.AddRange(new object[] {
                                    "STRING",
                                    "FLOAT",
                                    "INTEGER",
                                    "BOOL",
                                    "VECTOR",
                                    "TRANSFORM",
                                    "ENUM",
                                    "SPLINE"
                                    // TODO: we should support other types here, such as ZONE_LINK_PTR used in doors
            });
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void selectedFunctionEntity(object sender, EventArgs e)
        {
            //Function
            entityVariant.Visible = true;
            label2.Visible = true;
            generateHierarchy.Visible = false;
            createNewEntity.Enabled = true;
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < availableEntities.Count; i++) entityVariant.Items.Add(availableEntities[i].className);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDown;
        }
        private void selectedCompositeEntity(object sender, EventArgs e)
        {
            //Composite
            entityVariant.Visible = true;
            label2.Visible = true;
            generateHierarchy.Visible = false;
            createNewEntity.Enabled = true;
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < composites.Count; i++) entityVariant.Items.Add(composites[i].name);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void selectedProxyEntity(object sender, EventArgs e)
        {
            //Proxy
            entityVariant.Visible = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            createNewEntity.Enabled = false;
            hierarchy = null;
        }
        private void selectedOverrideEntity(object sender, EventArgs e)
        {
            //Override
            entityVariant.Visible = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            createNewEntity.Enabled = false;
            hierarchy = null;

            //TODO: Remove this warning when the checksum is figured out :)
            if (createOverrideEntity.Checked)
                MessageBox.Show("Please be aware that overrides are currently non-functional in-game due to an extra checksum used by Cathode to verify their existence, which has not yet been reverse engineered.\n\nIf you think you can help, feel free to submit a PR to CathodeLib on GitHub with the fix!", "Wait a minute!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /* Generate hierarchy for proxy/override */
        private void generateHierarchy_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_EditHierarchy hierarchyEditor = null;
            if (createProxyEntity.Checked)
            {
                hierarchyEditor = new CathodeEditorGUI_EditHierarchy(CurrentInstance.commandsPAK.EntryPoints[0]);
            }
            else if (createOverrideEntity.Checked)
            {
                hierarchyEditor = new CathodeEditorGUI_EditHierarchy(CurrentInstance.selectedComposite);
            }
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            if (createProxyEntity.Checked)
            {
                hierarchy = new List<ShortGuid>();
                hierarchy.Add(CurrentInstance.commandsPAK.EntryPoints[0].shortGUID);
                hierarchy.AddRange(generatedHierarchy);
                createNewEntity.Enabled = true;
            }
            else if(createOverrideEntity.Checked)
            {
                hierarchy = generatedHierarchy;
                createNewEntity.Enabled = true;
            }
        }

        private void createEntity(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter an entity name!", "No name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShortGuid thisID = ShortGuidUtils.Generate(DateTime.Now.ToString("G"));

            if (createDatatypeEntity.Checked)
            {
                //Make the DatatypeEntity
                DatatypeEntity newEntity = new DatatypeEntity(thisID);
                newEntity.type = (DataType)entityVariant.SelectedIndex;
                newEntity.parameter = ShortGuidUtils.Generate(textBox1.Text);

                //Make the parameter to give this DatatypeEntity a value (the only time you WOULDN'T want this is if the val is coming from a linked entity)
                ParameterData thisParam = null;
                switch (newEntity.type)
                {
                    case DataType.STRING:
                        thisParam = new cString("");
                        break;
                    case DataType.FLOAT:
                        thisParam = new cFloat(0.0f);
                        break;
                    case DataType.INTEGER:
                        thisParam = new cInteger(0);
                        break;
                    case DataType.BOOL:
                        thisParam = new cBool(true);
                        break;
                    case DataType.VECTOR:
                        thisParam = new cVector3(new Vector3(0, 0, 0));
                        break;
                    case DataType.TRANSFORM:
                        thisParam = new cTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
                        break;
                    case DataType.ENUM:
                        thisParam = new cEnum("ALERTNESS_STATE", 0); //ALERTNESS_STATE is the first alphabetically
                        break;
                    case DataType.SPLINE:
                        thisParam = new cSpline();
                        break;
                }
                newEntity.parameters.Add(new Parameter(newEntity.parameter, thisParam));

                //Add to composite & save name
                composite.datatypes.Add(newEntity);
                ShortGuidUtils.Generate(textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createFunctionEntity.Checked)
            {
                //Create FunctionEntity
                FunctionEntity newEntity = new FunctionEntity(thisID);
                ShortGuid function = CathodeEntityDatabase.GetEntity(entityVariant.Text).guid;
                if (function.val == null)
                {
                    MessageBox.Show("Please make sure you have typed or selected a valid entity class to create.", "Invalid entity class", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                switch (CommandsUtils.GetFunctionType(function))
                {
                    #region AUTOGENERATED ENTITY CONTENT
                    case FunctionType.ScriptInterface:
                        newEntity.parameters.Add(new Parameter("delete_me", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("name", new cString())); //String
                        break;
                    case FunctionType.ProxyInterface:
                        newEntity.parameters.Add(new Parameter("proxy_filter_targets", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("proxy_enable_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.ScriptVariable:
                        newEntity.parameters.Add(new Parameter("on_changed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_restored", new ParameterData())); //
                        break;
                    case FunctionType.SensorInterface:
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("pause_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.CloseableInterface:
                        newEntity.parameters.Add(new Parameter("open_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.GateInterface:
                        newEntity.parameters.Add(new Parameter("open_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("lock_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.ZoneInterface:
                        newEntity.parameters.Add(new Parameter("on_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_unloaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_streaming", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("force_visible_on_load", new cBool())); //bool
                        break;
                    case FunctionType.AttachmentInterface:
                        newEntity.parameters.Add(new Parameter("attach_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("attachment", new ParameterData())); //ReferenceFramePtr
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        break;
                    case FunctionType.SensorAttachmentInterface:
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("pause_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.CompositeInterface:
                        newEntity.parameters.Add(new Parameter("is_template", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("local_only", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("suspend_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_shared", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("requires_script_for_current_gen", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("requires_script_for_next_gen", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("convert_to_physics", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("delete_standard_collision", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("delete_ballistic_collision", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disable_display", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disable_collision", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disable_simulation", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("mapping", new cString())); //FilePath
                        newEntity.parameters.Add(new Parameter("include_in_planar_reflections", new cBool())); //bool
                        break;
                    case FunctionType.EnvironmentModelReference:
                        cResource resourceData2 = new cResource(newEntity.shortGUID);
                        resourceData2.AddResource(ResourceType.ANIMATED_MODEL); //TODO: need to figure out what startIndex links to, so we can set that!
                        newEntity.parameters.Add(new Parameter("resource", resourceData2));
                        break;
                    case FunctionType.SplinePath:
                        newEntity.parameters.Add(new Parameter("loop", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("orientated", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("points", new cSpline())); //SplineData
                        break;
                    case FunctionType.Box:
                        newEntity.parameters.Add(new Parameter("event", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("include_physics", new cBool())); //bool
                        break;
                    case FunctionType.HasAccessAtDifficulty:
                        newEntity.parameters.Add(new Parameter("difficulty", new cInteger())); //int
                        break;
                    case FunctionType.UpdateLeaderBoardDisplay:
                        newEntity.parameters.Add(new Parameter("time", new cFloat())); //float
                        break;
                    case FunctionType.SetNextLoadingMovie:
                        newEntity.parameters.Add(new Parameter("playlist_to_load", new cString())); //String
                        break;
                    case FunctionType.ButtonMashPrompt:
                        newEntity.parameters.Add(new Parameter("on_back_to_zero", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_degrade", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_mashed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("mashes_to_completion", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("time_between_degrades", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("use_degrade", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("hold_to_charge", new cBool())); //bool
                        break;
                    case FunctionType.GetFlashIntValue:
                        newEntity.parameters.Add(new Parameter("callback", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("int_value", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("callback_name", new cString())); //String
                        break;
                    case FunctionType.GetFlashFloatValue:
                        newEntity.parameters.Add(new Parameter("callback", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("float_value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("callback_name", new cString())); //String
                        break;
                    case FunctionType.Sphere:
                        newEntity.parameters.Add(new Parameter("event", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("include_physics", new cBool())); //bool
                        break;
                    case FunctionType.ImpactSphere:
                        newEntity.parameters.Add(new Parameter("event", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("include_physics", new cBool())); //bool
                        break;
                    case FunctionType.UiSelectionBox:
                        newEntity.parameters.Add(new Parameter("is_priority", new cBool())); //bool
                        break;
                    case FunctionType.UiSelectionSphere:
                        newEntity.parameters.Add(new Parameter("is_priority", new cBool())); //bool
                        break;
                    case FunctionType.CollisionBarrier:
                        newEntity.parameters.Add(new Parameter("on_damaged", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("collision_type", new cEnum("COLLISION_TYPE", 0))); //COLLISION_TYPE
                        newEntity.parameters.Add(new Parameter("static_collision", new cBool())); //bool
                        break;
                    case FunctionType.PlayerTriggerBox:
                        newEntity.parameters.Add(new Parameter("on_entered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_exited", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        break;
                    case FunctionType.PlayerUseTriggerBox:
                        newEntity.parameters.Add(new Parameter("on_entered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_exited", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_use", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("text", new cString())); //String
                        break;
                    case FunctionType.ModelReference:
                        newEntity.parameters.Add(new Parameter("on_damaged", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("simulate_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("light_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("convert_to_physics", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("material", new cString())); //String
                        newEntity.parameters.Add(new Parameter("occludes_atmosphere", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("include_in_planar_reflections", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("lod_ranges", new cString())); //String
                        newEntity.parameters.Add(new Parameter("intensity_multiplier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("radiosity_multiplier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("emissive_tint", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("replace_intensity", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("replace_tint", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("decal_scale", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("lightdecal_tint", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("lightdecal_intensity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("diffuse_colour_scale", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("diffuse_opacity_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("vertex_colour_scale", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("vertex_opacity_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("uv_scroll_speed_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("uv_scroll_speed_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_blend_noise_power_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_blend_noise_uv_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_blend_noise_uv_offset_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_blend_noise_uv_offset_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("dirt_multiply_blend_spec_power_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("dirt_map_uv_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("remove_on_damaged", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("damage_threshold", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("is_debris", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_prop", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_thrown", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("report_sliding", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("force_keyframed", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("force_transparent", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("soft_collision", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("allow_reposition_of_physics", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disable_size_culling", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("cast_shadows", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("cast_shadows_in_torch", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("alpha_light_offset_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_light_offset_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_light_scale_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_light_scale_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_light_average_normal", new cVector3())); //Direction
                        cResource resourceData = new cResource(newEntity.shortGUID);
                        resourceData.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        newEntity.parameters.Add(new Parameter("resource", resourceData));
                        break;
                    case FunctionType.LightReference:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("light_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("occlusion_geometry", new cEnum("RENDERABLE_INSTANCE", 0))); //RENDERABLE_INSTANCE
                        newEntity.parameters.Add(new Parameter("mastered_by_visibility", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("exclude_shadow_entities", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("type", new cEnum("LIGHT_TYPE", 0))); //LIGHT_TYPE
                        newEntity.parameters.Add(new Parameter("defocus_attenuation", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("start_attenuation", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("end_attenuation", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("physical_attenuation", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("near_dist", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("near_dist_shadow_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("inner_cone_angle", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("outer_cone_angle", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("intensity_multiplier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("radiosity_multiplier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("area_light_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("diffuse_softness", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("diffuse_bias", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("glossiness_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flare_occluder_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flare_spot_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flare_intensity_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("cast_shadow", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("fade_type", new cEnum("LIGHT_FADE_TYPE", 0))); //LIGHT_FADE_TYPE
                        newEntity.parameters.Add(new Parameter("is_specular", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("has_lens_flare", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("has_noclip", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_square_light", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_flash_light", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("no_alphalight", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("include_in_planar_reflections", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("shadow_priority", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("aspect_ratio", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("gobo_texture", new cString())); //String
                        newEntity.parameters.Add(new Parameter("horizontal_gobo_flip", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("strip_length", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("distance_mip_selection_gobo", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("volume", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("volume_end_attenuation", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("volume_colour_factor", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("volume_density", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("depth_bias", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("slope_scale_depth_bias", new cInteger())); //int
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.ParticleEmitterReference:
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("mastered_by_visibility", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("use_local_rotation", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("include_in_planar_reflections", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("material", new cString())); //String
                        newEntity.parameters.Add(new Parameter("unique_material", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("quality_level", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("bounds_max", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("bounds_min", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("TEXTURE_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("DRAW_PASS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ASPECT_RATIO", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FADE_AT_DISTANCE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PARTICLE_COUNT", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SYSTEM_EXPIRY_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SIZE_START_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SIZE_START_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SIZE_END_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SIZE_END_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHA_IN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHA_OUT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MASK_AMOUNT_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MASK_AMOUNT_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MASK_AMOUNT_MIDPOINT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PARTICLE_EXPIRY_TIME_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PARTICLE_EXPIRY_TIME_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("COLOUR_SCALE_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("COLOUR_SCALE_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIND_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIND_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIND_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHA_REF_VALUE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_LS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_NONE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_ON_AXIS_X", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_ON_AXIS_Y", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_ON_AXIS_Z", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_VELOCITY_ALIGNED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_VELOCITY_STRETCHED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_SPHERE_PROJECTION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_STANDARD", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_ALPHA_REF", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_ADDITIVE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_PREMULTIPLIED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_DISTORTION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("LOW_RES", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("EARLY_ALPHA", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("LOOPING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ANIMATED_ALPHA", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("NONE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("LIGHTING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PER_PARTICLE_LIGHTING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("X_AXIS_FLIP", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Y_AXIS_FLIP", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARD_FACING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_ON_AXIS_FADEOUT", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BILLBOARDING_CAMERA_LOCKED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("CAMERA_RELATIVE_POS_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CAMERA_RELATIVE_POS_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CAMERA_RELATIVE_POS_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPHERE_PROJECTION_RADIUS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DISTORTION_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SCALE_MODIFIER", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CPU", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SPAWN_RATE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPAWN_RATE_VAR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPAWN_NUMBER", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("LIFETIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LIFETIME_VAR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WORLD_TO_LOCAL_BLEND_START", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WORLD_TO_LOCAL_BLEND_END", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WORLD_TO_LOCAL_MAX_DIST", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CELL_EMISSION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("CELL_MAX_DIST", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CUSTOM_SEED_CPU", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SEED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ALPHA_TEST", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ZTEST", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("START_MID_END_SPEED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SPEED_START_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED_START_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED_MID_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED_MID_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED_END_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED_END_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LAUNCH_DECELERATE_SPEED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("LAUNCH_DECELERATE_SPEED_START_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LAUNCH_DECELERATE_SPEED_START_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LAUNCH_DECELERATE_DEC_RATE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EMISSION_AREA", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("EMISSION_AREA_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EMISSION_AREA_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EMISSION_AREA_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EMISSION_SURFACE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("EMISSION_DIRECTION_SURFACE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("AREA_CUBOID", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("AREA_SPHEROID", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("AREA_CYLINDER", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PIVOT_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PIVOT_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("GRAVITY", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("GRAVITY_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("GRAVITY_MAX_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT_START", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT_END", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("COLOUR_USE_MID", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT_MID", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("COLOUR_MIDPOINT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPREAD_FEATURE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SPREAD_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPREAD", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROTATION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ROTATION_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROTATION_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROTATION_RANDOM_START", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ROTATION_BASE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROTATION_VAR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROTATION_RAMP", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ROTATION_IN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROTATION_OUT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROTATION_DAMP", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FADE_NEAR_CAMERA", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("FADE_NEAR_CAMERA_MAX_DIST", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FADE_NEAR_CAMERA_THRESHOLD", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TEXTURE_ANIMATION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("TEXTURE_ANIMATION_FRAMES", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("NUM_ROWS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("TEXTURE_ANIMATION_LOOP_COUNT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RANDOM_START_FRAME", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("WRAP_FRAMES", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("NO_ANIM", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SUB_FRAME_BLEND", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SOFTNESS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SOFTNESS_EDGE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SOFTNESS_ALPHA_THICKNESS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SOFTNESS_ALPHA_DEPTH_MODIFIER", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("REVERSE_SOFTNESS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("REVERSE_SOFTNESS_EDGE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PIVOT_AND_TURBULENCE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PIVOT_OFFSET_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PIVOT_OFFSET_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TURBULENCE_FREQUENCY_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TURBULENCE_FREQUENCY_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TURBULENCE_AMOUNT_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TURBULENCE_AMOUNT_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHATHRESHOLD", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ALPHATHRESHOLD_TOTALTIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHATHRESHOLD_RANGE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHATHRESHOLD_BEGINSTART", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHATHRESHOLD_BEGINSTOP", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHATHRESHOLD_ENDSTART", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHATHRESHOLD_ENDSTOP", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("COLOUR_RAMP", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("COLOUR_RAMP_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("COLOUR_RAMP_ALPHA", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("DEPTH_FADE_AXIS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("DEPTH_FADE_AXIS_DIST", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FADE_AXIS_PERCENT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_UV_ANIMATION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("FLOW_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("FLOW_TEXTURE_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("CYCLE_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_TEX_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_WARP_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("INFINITE_PROJECTION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PARALLAX_POSITION", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DISTORTION_OCCLUSION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("AMBIENT_LIGHTING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("AMBIENT_LIGHTING_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("NO_CLIP", new cInteger())); //int
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.RibbonEmitterReference:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("mastered_by_visibility", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("use_local_rotation", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("include_in_planar_reflections", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("material", new cString())); //String
                        newEntity.parameters.Add(new Parameter("unique_material", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("quality_level", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_STANDARD", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_ALPHA_REF", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_ADDITIVE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_PREMULTIPLIED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BLENDING_DISTORTION", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("NO_MIPS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("UV_SQUARED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("LOW_RES", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("LIGHTING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("MASK_AMOUNT_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MASK_AMOUNT_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MASK_AMOUNT_MIDPOINT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DRAW_PASS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SYSTEM_EXPIRY_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LIFETIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SMOOTHED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("WORLD_TO_LOCAL_BLEND_START", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WORLD_TO_LOCAL_BLEND_END", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WORLD_TO_LOCAL_MAX_DIST", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TEXTURE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("TEXTURE_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("UV_REPEAT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("UV_SCROLLSPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MULTI_TEXTURE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("U2_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("V2_REPEAT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("V2_SCROLLSPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MULTI_TEXTURE_BLEND", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("MULTI_TEXTURE_ADD", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("MULTI_TEXTURE_MULT", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("MULTI_TEXTURE_MAX", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("MULTI_TEXTURE_MIN", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SECOND_TEXTURE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("TEXTURE_MAP2", new cString())); //String
                        newEntity.parameters.Add(new Parameter("CONTINUOUS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("BASE_LOCKED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SPAWN_RATE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TRAILING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("INSTANT", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("RATE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("TRAIL_SPAWN_RATE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TRAIL_DELAY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MAX_TRAILS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("POINT_TO_POINT", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("TARGET_POINT_POSITION", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ABS_FADE_IN_0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ABS_FADE_IN_1", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FORCES", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("GRAVITY_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("GRAVITY_MAX_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DRAG_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIND_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIND_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIND_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("START_MID_END_SPEED", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SPEED_START_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED_START_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIDTH", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("WIDTH_START", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIDTH_MID", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIDTH_END", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIDTH_IN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WIDTH_OUT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("COLOUR_SCALE_START", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("COLOUR_SCALE_MID", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("COLOUR_SCALE_END", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT_START", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT_MID", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT_END", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("ALPHA_FADE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("FADE_IN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FADE_OUT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EDGE_FADE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ALPHA_ERODE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SIDE_ON_FADE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SIDE_FADE_START", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SIDE_FADE_END", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DISTANCE_SCALING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("DIST_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPREAD_FEATURE", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SPREAD_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPREAD", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EMISSION_AREA", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("EMISSION_AREA_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EMISSION_AREA_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EMISSION_AREA_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("AREA_CUBOID", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("AREA_SPHEROID", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("AREA_CYLINDER", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("COLOUR_RAMP", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("COLOUR_RAMP_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("SOFTNESS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SOFTNESS_EDGE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SOFTNESS_ALPHA_THICKNESS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SOFTNESS_ALPHA_DEPTH_MODIFIER", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("AMBIENT_LIGHTING", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("AMBIENT_LIGHTING_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("NO_CLIP", new cInteger())); //int
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.GPU_PFXEmitterReference:
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("mastered_by_visibility", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("EFFECT_NAME", new cString())); //String
                        newEntity.parameters.Add(new Parameter("SPAWN_NUMBER", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SPAWN_RATE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPREAD_MIN", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPREAD_MAX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EMITTER_SIZE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED_VAR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LIFETIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LIFETIME_VAR", new cFloat())); //float
                        break;
                    case FunctionType.FogSphere:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("INTENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("OPACITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EARLY_ALPHA", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LOW_RES_ALPHA", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("CONVEX_GEOM", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DISABLE_SIZE_CULLING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("NO_CLIP", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ALPHA_LIGHTING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DYNAMIC_ALPHA_LIGHTING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("EXPONENTIAL_DENSITY", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SCENE_DEPENDANT_DENSITY", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("FRESNEL_TERM", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("FRESNEL_POWER", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SOFTNESS", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SOFTNESS_EDGE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("BLEND_ALPHA_OVER_DISTANCE", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("FAR_BLEND_DISTANCE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("NEAR_BLEND_DISTANCE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_BLEND_ALPHA_OVER_DISTANCE", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SECONDARY_FAR_BLEND_DISTANCE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_NEAR_BLEND_DISTANCE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_COLOUR", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_COLOUR_VALUE", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_ALPHA_VALUE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_RANGE", new cFloat())); //float
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.FogBox:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("GEOMETRY_TYPE", new cEnum("FOG_BOX_TYPE", 0))); //FOG_BOX_TYPE
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DISTANCE_FADE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ANGLE_FADE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("BILLBOARD", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("EARLY_ALPHA", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LOW_RES", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("CONVEX_GEOM", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("THICKNESS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("START_DISTANT_CLIP", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("START_DISTANCE_FADE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SOFTNESS", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SOFTNESS_EDGE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LINEAR_HEIGHT_DENSITY", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SMOOTH_HEIGHT_DENSITY", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("HEIGHT_MAX_DENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FRESNEL_FALLOFF", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("FRESNEL_POWER", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_COLOUR", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_INITIAL_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_INITIAL_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_MIDPOINT_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_MIDPOINT_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_MIDPOINT_DEPTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_END_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_END_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_INTERSECT_END_DEPTH", new cFloat())); //float
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.SurfaceEffectSphere:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT_OUTER", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("INTENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("OPACITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FADE_OUT_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SURFACE_WRAP", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROUGHNESS_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPARKLE_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("METAL_STYLE_REFLECTIONS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SHININESS_OPACITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TILING_ZY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TILING_ZX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TILING_XY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("WS_LOCKED", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("TEXTURE_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("SPARKLE_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("ENVMAP", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ENVIRONMENT_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("ENVMAP_PERCENT_EMISSIVE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPHERE", new cBool())); //bool
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.SurfaceEffectBox:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("COLOUR_TINT_OUTER", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("INTENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("OPACITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FADE_OUT_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SURFACE_WRAP", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ROUGHNESS_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPARKLE_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("METAL_STYLE_REFLECTIONS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SHININESS_OPACITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TILING_ZY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TILING_ZX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("TILING_XY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FALLOFF", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("WS_LOCKED", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("TEXTURE_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("SPARKLE_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("ENVMAP", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ENVIRONMENT_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("ENVMAP_PERCENT_EMISSIVE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPHERE", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("BOX", new cBool())); //bool
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.SimpleWater:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SHININESS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("softness_edge", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FRESNEL_POWER", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MIN_FRESNEL", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MAX_FRESNEL", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LOW_RES_ALPHA_PASS", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ATMOSPHERIC_FOGGING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("NORMAL_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("NORMAL_MAP_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_NORMAL_MAPPING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SECONDARY_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_NORMAL_MAP_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHA_MASKING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ALPHA_MASK", new cString())); //String
                        newEntity.parameters.Add(new Parameter("FLOW_MAPPING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("FLOW_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("CYCLE_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_TEX_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_WARP_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ENVIRONMENT_MAPPING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ENVIRONMENT_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("ENVIRONMENT_MAP_MULT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LOCALISED_ENVIRONMENT_MAPPING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ENVMAP_SIZE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LOCALISED_ENVMAP_BOX_PROJECTION", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ENVMAP_BOXPROJ_BB_SCALE", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("REFLECTIVE_MAPPING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("REFLECTION_PERTURBATION_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_INITIAL_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_INITIAL_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_MIDPOINT_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_MIDPOINT_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_MIDPOINT_DEPTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_END_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_END_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_END_DEPTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CAUSTIC_TEXTURE", new cString())); //String
                        newEntity.parameters.Add(new Parameter("CAUSTIC_TEXTURE_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CAUSTIC_REFRACTIONS", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("CAUSTIC_REFLECTIONS", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("CAUSTIC_SPEED_SCALAR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CAUSTIC_INTENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CAUSTIC_SURFACE_WRAP", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CAUSTIC_HEIGHT", new cFloat())); //float
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        newEntity.parameters.Add(new Parameter("CAUSTIC_TEXTURE_INDEX", new cInteger())); //int
                        break;
                    case FunctionType.SimpleRefraction:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DISTANCEFACTOR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("NORMAL_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("REFRACTFACTOR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_NORMAL_MAPPING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SECONDARY_NORMAL_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("SECONDARY_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_REFRACTFACTOR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHA_MASKING", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ALPHA_MASK", new cString())); //String
                        newEntity.parameters.Add(new Parameter("DISTORTION_OCCLUSION", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("MIN_OCCLUSION_DISTANCE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_UV_ANIMATION", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("FLOW_MAP", new cString())); //String
                        newEntity.parameters.Add(new Parameter("CYCLE_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_TEX_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_WARP_STRENGTH", new cFloat())); //float
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.ProjectiveDecal:
                        newEntity.parameters.Add(new Parameter("deleted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("include_in_planar_reflections", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("material", new cString())); //String
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.LODControls:
                        newEntity.parameters.Add(new Parameter("lod_range_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("disable_lods", new cBool())); //bool
                        break;
                    case FunctionType.LightingMaster:
                        newEntity.parameters.Add(new Parameter("light_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        break;
                    case FunctionType.DebugCamera:
                        newEntity.parameters.Add(new Parameter("linked_cameras", new ParameterData())); //Object
                        break;
                    case FunctionType.CameraResource:
                        newEntity.parameters.Add(new Parameter("on_enter_transition_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_exit_transition_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("camera_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("is_camera_transformation_local", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("camera_transformation", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("fov", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("clipping_planes_preset", new cEnum("CLIPPING_PLANES_PRESETS", 0))); //CLIPPING_PLANES_PRESETS
                        newEntity.parameters.Add(new Parameter("is_ghost", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("converge_to_player_camera", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("reset_player_camera_on_exit", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("enable_enter_transition", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("transition_curve_direction", new cEnum("TRANSITION_DIRECTION", 0))); //TRANSITION_DIRECTION
                        newEntity.parameters.Add(new Parameter("transition_curve_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("transition_duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("transition_ease_in", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("transition_ease_out", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("enable_exit_transition", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("exit_transition_curve_direction", new cEnum("TRANSITION_DIRECTION", 0))); //TRANSITION_DIRECTION
                        newEntity.parameters.Add(new Parameter("exit_transition_curve_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("exit_transition_duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("exit_transition_ease_in", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("exit_transition_ease_out", new cFloat())); //float
                        break;
                    case FunctionType.CameraFinder:
                        newEntity.parameters.Add(new Parameter("camera_name", new cString())); //String
                        break;
                    case FunctionType.CameraBehaviorInterface:
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("pause_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("linked_cameras", new cEnum("CAMERA_INSTANCE", 0))); //CAMERA_INSTANCE
                        newEntity.parameters.Add(new Parameter("behavior_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("priority", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("threshold", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("blend_in", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("blend_out", new cFloat())); //float
                        break;
                    case FunctionType.HandCamera:
                        newEntity.parameters.Add(new Parameter("noise_type", new cEnum("NOISE_TYPE", 0))); //NOISE_TYPE
                        newEntity.parameters.Add(new Parameter("frequency", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("damping", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("rotation_intensity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("min_fov_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_fov_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_noise", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_noise", new cFloat())); //float
                        break;
                    case FunctionType.CameraShake:
                        newEntity.parameters.Add(new Parameter("relative_transformation", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("impulse_intensity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("impulse_position", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("shake_type", new cEnum("SHAKE_TYPE", 0))); //SHAKE_TYPE
                        newEntity.parameters.Add(new Parameter("shake_frequency", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("max_rotation_angles", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("max_position_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("shake_rotation", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("shake_position", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("bone_shaking", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("override_weapon_swing", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("internal_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("external_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("strength_damping", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("explosion_push_back", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("spring_constant", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("spring_damping", new cFloat())); //float
                        break;
                    case FunctionType.CameraPathDriven:
                        newEntity.parameters.Add(new Parameter("position_path", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("target_path", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("reference_path", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("position_path_transform", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target_path_transform", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("reference_path_transform", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("point_to_project", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("path_driven_type", new cEnum("PATH_DRIVEN_TYPE", 0))); //PATH_DRIVEN_TYPE
                        newEntity.parameters.Add(new Parameter("invert_progression", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position_path_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("target_path_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("animation_duration", new cFloat())); //float
                        break;
                    case FunctionType.FixedCamera:
                        newEntity.parameters.Add(new Parameter("use_transform_position", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("transform_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("camera_position", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("camera_target", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("camera_position_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("camera_target_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("apply_target", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("apply_position", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_target_offset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_position_offset", new cBool())); //bool
                        break;
                    case FunctionType.BoneAttachedCamera:
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("position_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("rotation_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("movement_damping", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("bone_name", new cString())); //String
                        break;
                    case FunctionType.ControllableRange:
                        newEntity.parameters.Add(new Parameter("min_range_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_range_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_range_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_range_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_feather_range_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_feather_range_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_feather_range_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_feather_range_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("speed_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("speed_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("damping_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("damping_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("mouse_speed_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("mouse_speed_y", new cFloat())); //float
                        break;
                    case FunctionType.StealCamera:
                        newEntity.parameters.Add(new Parameter("on_converged", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("focus_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("steal_type", new cEnum("STEAL_CAMERA_TYPE", 0))); //STEAL_CAMERA_TYPE
                        newEntity.parameters.Add(new Parameter("check_line_of_sight", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("blend_in_duration", new cFloat())); //float
                        break;
                    case FunctionType.FollowCameraModifier:
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position_curve", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("target_curve", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("modifier_type", new cEnum("FOLLOW_CAMERA_MODIFIERS", 0))); //FOLLOW_CAMERA_MODIFIERS
                        newEntity.parameters.Add(new Parameter("position_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("target_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("field_of_view", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("force_state", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("force_state_initial_value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("can_mirror", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_first_person", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("bone_blending_ratio", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("movement_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("movement_speed_vertical", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("movement_damping", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("horizontal_limit_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("horizontal_limit_max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("vertical_limit_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("vertical_limit_max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("mouse_speed_hori", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("mouse_speed_vert", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("acceleration_duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("acceleration_ease_in", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("acceleration_ease_out", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("transition_duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("transition_ease_in", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("transition_ease_out", new cFloat())); //float
                        break;
                    case FunctionType.CameraPath:
                        newEntity.parameters.Add(new Parameter("linked_splines", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("path_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("path_type", new cEnum("CAMERA_PATH_TYPE", 0))); //CAMERA_PATH_TYPE
                        newEntity.parameters.Add(new Parameter("path_class", new cEnum("CAMERA_PATH_CLASS", 0))); //CAMERA_PATH_CLASS
                        newEntity.parameters.Add(new Parameter("is_local", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("relative_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("is_loop", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        break;
                    case FunctionType.CameraAimAssistant:
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("activation_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("inner_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("camera_speed_attenuation", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_activation_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("fading_range", new cFloat())); //float
                        break;
                    case FunctionType.CameraPlayAnimation:
                        newEntity.parameters.Add(new Parameter("on_animation_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("animated_camera", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("position_marker", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("character_to_focus", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("focal_length_mm", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("focal_plane_m", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("fnum", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("focal_point", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("animation_length", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("frames_count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("result_transformation", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("data_file", new cString())); //String
                        newEntity.parameters.Add(new Parameter("start_frame", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("end_frame", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("play_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("loop_play", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("clipping_planes_preset", new cEnum("CLIPPING_PLANES_PRESETS", 0))); //CLIPPING_PLANES_PRESETS
                        newEntity.parameters.Add(new Parameter("is_cinematic", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("dof_key", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("shot_number", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("override_dof", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("focal_point_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("bone_to_focus", new cString())); //String
                        break;
                    case FunctionType.CamPeek:
                        newEntity.parameters.Add(new Parameter("pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("x_ratio", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("y_ratio", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("range_left", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("range_right", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("range_up", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("range_down", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("range_forward", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("range_backward", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("speed_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("speed_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("damping_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("damping_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("focal_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("focal_distance_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("roll_factor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("use_ik_solver", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_horizontal_plane", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("stick", new cEnum("SIDE", 0))); //SIDE
                        newEntity.parameters.Add(new Parameter("disable_collision_test", new cBool())); //bool
                        break;
                    case FunctionType.CameraDofController:
                        newEntity.parameters.Add(new Parameter("character_to_focus", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("focal_length_mm", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("focal_plane_m", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("fnum", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("focal_point", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("focal_point_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("bone_to_focus", new cString())); //String
                        break;
                    case FunctionType.ClipPlanesController:
                        newEntity.parameters.Add(new Parameter("near_plane", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("far_plane", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("update_near", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("update_far", new cBool())); //bool
                        break;
                    case FunctionType.GetCurrentCameraTarget:
                        newEntity.parameters.Add(new Parameter("target", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("distance", new cFloat())); //float
                        break;
                    case FunctionType.Logic_Vent_Entrance:
                        newEntity.parameters.Add(new Parameter("Hide_Pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Emit_Pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("force_stand_on_exit", new cBool())); //bool
                        break;
                    case FunctionType.Logic_Vent_System:
                        newEntity.parameters.Add(new Parameter("Vent_Entrances", new cEnum("VENT_ENTRANCE", 0))); //VENT_ENTRANCE
                        break;
                    case FunctionType.CharacterCommand:
                        newEntity.parameters.Add(new Parameter("command_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("override_all_ai", new cBool())); //bool
                        break;
                    case FunctionType.CMD_Follow:
                        newEntity.parameters.Add(new Parameter("entered_inner_radius", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exitted_outer_radius", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Waypoint", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("idle_stance", new cEnum("IDLE", 0))); //IDLE
                        newEntity.parameters.Add(new Parameter("move_type", new cEnum("MOVE", 0))); //MOVE
                        newEntity.parameters.Add(new Parameter("inner_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("outer_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("prefer_traversals", new cBool())); //bool
                        break;
                    case FunctionType.CMD_FollowUsingJobs:
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("target_to_follow", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("who_Im_leading", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("fastest_allowed_move_type", new cEnum("MOVE", 0))); //MOVE
                        newEntity.parameters.Add(new Parameter("slowest_allowed_move_type", new cEnum("MOVE", 0))); //MOVE
                        newEntity.parameters.Add(new Parameter("centre_job_restart_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("inner_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("outer_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("job_select_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("job_cancel_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("teleport_required_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("teleport_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("prefer_traversals", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("avoid_player", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("allow_teleports", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("follow_type", new cEnum("FOLLOW_TYPE", 0))); //FOLLOW_TYPE
                        newEntity.parameters.Add(new Parameter("clamp_speed", new cBool())); //bool
                        break;
                    case FunctionType.NPC_FollowOffset:
                        newEntity.parameters.Add(new Parameter("offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("target_to_follow", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cTransform())); //Position
                        break;
                    case FunctionType.AnimationMask:
                        newEntity.parameters.Add(new Parameter("maskHips", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskTorso", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskNeck", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskHead", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskFace", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskLeftLeg", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskRightLeg", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskLeftArm", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskRightArm", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskLeftHand", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskRightHand", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskLeftFingers", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskRightFingers", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskTail", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskLips", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskEyes", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskLeftShoulder", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskRightShoulder", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskRoot", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskPrecedingLayers", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskSelf", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maskFollowingLayers", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("weight", new cFloat())); //float
                        newEntity.AddResource(ResourceType.ANIMATION_MASK_RESOURCE);
                        break;
                    case FunctionType.CMD_PlayAnimation:
                        newEntity.parameters.Add(new Parameter("Interrupted", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("badInterrupted", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SafePos", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Marker", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("ExitPosition", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("ExternalStartTime", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("ExternalTime", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("OverrideCharacter", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("OptionalMask", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("animationLength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("AnimationSet", new cString())); //String
                        newEntity.parameters.Add(new Parameter("Animation", new cString())); //String
                        newEntity.parameters.Add(new Parameter("StartFrame", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("EndFrame", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PlayCount", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PlaySpeed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("AllowGravity", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("AllowCollision", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Start_Instantly", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("AllowInterruption", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("RemoveMotion", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DisableGunLayer", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("BlendInTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("GaitSyncStart", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ConvergenceTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LocationConvergence", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("OrientationConvergence", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("UseExitConvergence", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ExitConvergenceTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Mirror", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("FullCinematic", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("RagdollEnabled", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("NoIK", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("NoFootIK", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("NoLayers", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("PlayerAnimDrivenView", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ExertionFactor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("AutomaticZoning", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ManualLoading", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("IsCrouchedAnim", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("InitiallyBackstage", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Death_by_ragdoll_only", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("dof_key", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("shot_number", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("UseShivaArms", new cBool())); //bool
                        newEntity.AddResource(ResourceType.PLAY_ANIMATION_DATA_RESOURCE);
                        break;
                    case FunctionType.CMD_Idle:
                        newEntity.parameters.Add(new Parameter("finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("interrupted", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("target_to_face", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("should_face_target", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("should_raise_gun_while_turning", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("desired_stance", new cEnum("CHARACTER_STANCE", 0))); //CHARACTER_STANCE
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("idle_style", new cEnum("IDLE_STYLE", 0))); //IDLE_STYLE
                        newEntity.parameters.Add(new Parameter("lock_cameras", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("anchor", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("start_instantly", new cBool())); //bool
                        break;
                    case FunctionType.CMD_GoTo:
                        newEntity.parameters.Add(new Parameter("succeeded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Waypoint", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("AimTarget", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("move_type", new cEnum("MOVE", 0))); //MOVE
                        newEntity.parameters.Add(new Parameter("enable_lookaround", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_stopping_anim", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("always_stop_at_radius", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("stop_at_radius_if_lined_up", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("continue_from_previous_move", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disallow_traversal", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("arrived_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("should_be_aiming", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_current_target_as_aim", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("allow_to_use_vents", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DestinationIsBackstage", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("maintain_current_facing", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("start_instantly", new cBool())); //bool
                        break;
                    case FunctionType.CMD_GoToCover:
                        newEntity.parameters.Add(new Parameter("succeeded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("entered_cover", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("CoverPoint", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("AimTarget", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("move_type", new cEnum("MOVE", 0))); //MOVE
                        newEntity.parameters.Add(new Parameter("SearchRadius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("enable_lookaround", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("continue_from_previous_move", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disallow_traversal", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("should_be_aiming", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_current_target_as_aim", new cBool())); //bool
                        break;
                    case FunctionType.CMD_MoveTowards:
                        newEntity.parameters.Add(new Parameter("succeeded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("MoveTarget", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("AimTarget", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("move_type", new cEnum("MOVE", 0))); //MOVE
                        newEntity.parameters.Add(new Parameter("disallow_traversal", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("should_be_aiming", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_current_target_as_aim", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("never_succeed", new cBool())); //bool
                        break;
                    case FunctionType.CMD_Die:
                        newEntity.parameters.Add(new Parameter("Killer", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("death_style", new cEnum("DEATH_STYLE", 0))); //DEATH_STYLE
                        break;
                    case FunctionType.CMD_LaunchMeleeAttack:
                        newEntity.parameters.Add(new Parameter("finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("melee_attack_type", new cEnum("MELEE_ATTACK_TYPE", 0))); //MELEE_ATTACK_TYPE
                        newEntity.parameters.Add(new Parameter("enemy_type", new cEnum("ENEMY_TYPE", 0))); //ENEMY_TYPE
                        newEntity.parameters.Add(new Parameter("melee_attack_index", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("skip_convergence", new cBool())); //bool
                        break;
                    case FunctionType.CMD_ModifyCombatBehaviour:
                        newEntity.parameters.Add(new Parameter("behaviour_type", new cEnum("COMBAT_BEHAVIOUR", 0))); //COMBAT_BEHAVIOUR
                        newEntity.parameters.Add(new Parameter("status", new cBool())); //bool
                        break;
                    case FunctionType.CMD_HolsterWeapon:
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("should_holster", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("skip_anims", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("equipment_slot", new cEnum("EQUIPMENT_SLOT", 0))); //EQUIPMENT_SLOT
                        newEntity.parameters.Add(new Parameter("force_player_unarmed_on_holster", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("force_drop_held_item", new cBool())); //bool
                        break;
                    case FunctionType.CMD_ForceReloadWeapon:
                        newEntity.parameters.Add(new Parameter("success", new ParameterData())); //
                        break;
                    case FunctionType.CMD_ForceMeleeAttack:
                        newEntity.parameters.Add(new Parameter("melee_attack_type", new cEnum("MELEE_ATTACK_TYPE", 0))); //MELEE_ATTACK_TYPE
                        newEntity.parameters.Add(new Parameter("enemy_type", new cEnum("ENEMY_TYPE", 0))); //ENEMY_TYPE
                        newEntity.parameters.Add(new Parameter("melee_attack_index", new cInteger())); //int
                        break;
                    case FunctionType.CHR_ModifyBreathing:
                        newEntity.parameters.Add(new Parameter("Exhaustion", new cFloat())); //float
                        break;
                    case FunctionType.CHR_HoldBreath:
                        newEntity.parameters.Add(new Parameter("ExhaustionOnStop", new cFloat())); //float
                        break;
                    case FunctionType.CHR_DeepCrouch:
                        newEntity.parameters.Add(new Parameter("crouch_amount", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("smooth_damping", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("allow_stand_up", new cBool())); //bool
                        break;
                    case FunctionType.CHR_PlaySecondaryAnimation:
                        newEntity.parameters.Add(new Parameter("Interrupted", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Marker", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("OptionalMask", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("ExternalStartTime", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("ExternalTime", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("animationLength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("AnimationSet", new cString())); //String
                        newEntity.parameters.Add(new Parameter("Animation", new cString())); //String
                        newEntity.parameters.Add(new Parameter("StartFrame", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("EndFrame", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PlayCount", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PlaySpeed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("StartInstantly", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("AllowInterruption", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("BlendInTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("GaitSyncStart", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Mirror", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("AnimationLayer", new cEnum("SECONDARY_ANIMATION_LAYER", 0))); //SECONDARY_ANIMATION_LAYER
                        newEntity.parameters.Add(new Parameter("AutomaticZoning", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ManualLoading", new cBool())); //bool
                        break;
                    case FunctionType.CHR_LocomotionModifier:
                        newEntity.parameters.Add(new Parameter("Can_Run", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Can_Crouch", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Can_Aim", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Can_Injured", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Must_Walk", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Must_Run", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Must_Crouch", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Must_Aim", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Must_Injured", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Is_In_Spacesuit", new cBool())); //bool
                        break;
                    case FunctionType.CHR_SetMood:
                        newEntity.parameters.Add(new Parameter("mood", new cEnum("MOOD", 0))); //MOOD
                        newEntity.parameters.Add(new Parameter("moodIntensity", new cEnum("MOOD_INTENSITY", 0))); //MOOD_INTENSITY
                        newEntity.parameters.Add(new Parameter("timeOut", new cFloat())); //float
                        break;
                    case FunctionType.CHR_LocomotionEffect:
                        newEntity.parameters.Add(new Parameter("Effect", new cEnum("ANIMATION_EFFECT_TYPE", 0))); //ANIMATION_EFFECT_TYPE
                        break;
                    case FunctionType.CHR_LocomotionDuck:
                        newEntity.parameters.Add(new Parameter("Height", new cEnum("DUCK_HEIGHT", 0))); //DUCK_HEIGHT
                        break;
                    case FunctionType.CMD_ShootAt:
                        newEntity.parameters.Add(new Parameter("succeeded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Target", new ParameterData())); //Object
                        break;
                    case FunctionType.CMD_AimAtCurrentTarget:
                        newEntity.parameters.Add(new Parameter("succeeded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Raise_gun", new cBool())); //bool
                        break;
                    case FunctionType.CMD_AimAt:
                        newEntity.parameters.Add(new Parameter("finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AimTarget", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Raise_gun", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_current_target", new cBool())); //bool
                        break;
                    case FunctionType.Player_Sensor:
                        newEntity.parameters.Add(new Parameter("Standard", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Running", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Aiming", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Vent", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Grapple", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Death", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Cover", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Motion_Tracked", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Motion_Tracked_Vent", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Leaning", new ParameterData())); //
                        break;
                    case FunctionType.CMD_Ragdoll:
                        newEntity.parameters.Add(new Parameter("finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("actor", new ParameterData())); //CHARACTER
                        newEntity.parameters.Add(new Parameter("impact_velocity", new cVector3())); //Direction
                        break;
                    case FunctionType.CHR_SetTacticalPosition:
                        newEntity.parameters.Add(new Parameter("tactical_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("sweep_type", new cEnum("AREA_SWEEP_TYPE", 0))); //AREA_SWEEP_TYPE
                        newEntity.parameters.Add(new Parameter("fixed_sweep_radius", new cFloat())); //float
                        break;
                    case FunctionType.CHR_SetFocalPoint:
                        newEntity.parameters.Add(new Parameter("focal_point", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("priority", new cEnum("PRIORITY", 0))); //PRIORITY
                        newEntity.parameters.Add(new Parameter("speed", new cEnum("LOOK_SPEED", 0))); //LOOK_SPEED
                        newEntity.parameters.Add(new Parameter("steal_camera", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("line_of_sight_test", new cBool())); //bool
                        break;
                    case FunctionType.CHR_SetAndroidThrowTarget:
                        newEntity.parameters.Add(new Parameter("thrown", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("throw_position", new cTransform())); //Position
                        break;
                    case FunctionType.CHR_SetAlliance:
                        newEntity.parameters.Add(new Parameter("Alliance", new cEnum("ALLIANCE_GROUP", 0))); //ALLIANCE_GROUP
                        break;
                    case FunctionType.CHR_GetAlliance:
                        newEntity.parameters.Add(new Parameter("Alliance", new cEnum())); //Enum
                        break;
                    case FunctionType.ALLIANCE_SetDisposition:
                        newEntity.parameters.Add(new Parameter("A", new cEnum("ALLIANCE_GROUP", 0))); //ALLIANCE_GROUP
                        newEntity.parameters.Add(new Parameter("B", new cEnum("ALLIANCE_GROUP", 0))); //ALLIANCE_GROUP
                        newEntity.parameters.Add(new Parameter("Disposition", new cEnum("ALLIANCE_STANCE", 0))); //ALLIANCE_STANCE
                        break;
                    case FunctionType.CHR_SetInvincibility:
                        newEntity.parameters.Add(new Parameter("damage_mode", new cEnum("DAMAGE_MODE", 0))); //DAMAGE_MODE
                        break;
                    case FunctionType.CHR_SetHealth:
                        newEntity.parameters.Add(new Parameter("HealthPercentage", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("UsePercentageOfCurrentHeath", new cBool())); //bool
                        break;
                    case FunctionType.CHR_GetHealth:
                        newEntity.parameters.Add(new Parameter("Health", new cInteger())); //int
                        break;
                    case FunctionType.CHR_SetDebugDisplayName:
                        newEntity.parameters.Add(new Parameter("DebugName", new cString())); //String
                        break;
                    case FunctionType.CHR_TakeDamage:
                        newEntity.parameters.Add(new Parameter("Damage", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("DamageIsAPercentage", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("AmmoType", new cEnum("AMMO_TYPE", 0))); //AMMO_TYPE
                        break;
                    case FunctionType.CHR_SetSubModelVisibility:
                        newEntity.parameters.Add(new Parameter("is_visible", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("matching", new cString())); //String
                        break;
                    case FunctionType.CHR_SetHeadVisibility:
                        newEntity.parameters.Add(new Parameter("is_visible", new cBool())); //bool
                        break;
                    case FunctionType.CHR_SetFacehuggerAggroRadius:
                        newEntity.parameters.Add(new Parameter("radius", new cFloat())); //float
                        break;
                    case FunctionType.CHR_DamageMonitor:
                        newEntity.parameters.Add(new Parameter("damaged", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("InstigatorFilter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DamageDone", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Instigator", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("DamageType", new cEnum("DAMAGE_EFFECTS", 0))); //DAMAGE_EFFECTS
                        break;
                    case FunctionType.CHR_KnockedOutMonitor:
                        newEntity.parameters.Add(new Parameter("on_knocked_out", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_recovered", new ParameterData())); //
                        break;
                    case FunctionType.CHR_DeathMonitor:
                        newEntity.parameters.Add(new Parameter("dying", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("killed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("KillerFilter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Killer", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("DamageType", new cEnum("DAMAGE_EFFECTS", 0))); //DAMAGE_EFFECTS
                        break;
                    case FunctionType.CHR_RetreatMonitor:
                        newEntity.parameters.Add(new Parameter("reached_retreat", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("started_retreating", new ParameterData())); //
                        break;
                    case FunctionType.CHR_WeaponFireMonitor:
                        newEntity.parameters.Add(new Parameter("fired", new ParameterData())); //
                        break;
                    case FunctionType.CHR_TorchMonitor:
                        newEntity.parameters.Add(new Parameter("torch_on", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("torch_off", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("TorchOn", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_checkpoint_restart", new cBool())); //bool
                        break;
                    case FunctionType.CHR_VentMonitor:
                        newEntity.parameters.Add(new Parameter("entered_vent", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exited_vent", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("IsInVent", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_checkpoint_restart", new cBool())); //bool
                        break;
                    case FunctionType.CharacterTypeMonitor:
                        newEntity.parameters.Add(new Parameter("spawned", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("despawned", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("all_despawned", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AreAny", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_class", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.parameters.Add(new Parameter("trigger_on_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_checkpoint_restart", new cBool())); //bool
                        break;
                    case FunctionType.Convo:
                        newEntity.parameters.Add(new Parameter("everyoneArrived", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("playerJoined", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("playerLeft", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("npcJoined", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("members", new cEnum("LOGIC_CHARACTER", 0))); //LOGIC_CHARACTER
                        newEntity.parameters.Add(new Parameter("speaker", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("alwaysTalkToPlayerIfPresent", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("playerCanJoin", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("playerCanLeave", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("positionNPCs", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("circularShape", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("convoPosition", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("personalSpaceRadius", new cFloat())); //float
                        break;
                    case FunctionType.NPC_NotifyDynamicDialogueEvent:
                        newEntity.parameters.Add(new Parameter("DialogueEvent", new cEnum("DIALOGUE_NPC_EVENT", 0))); //DIALOGUE_NPC_EVENT
                        break;
                    case FunctionType.NPC_Squad_DialogueMonitor:
                        newEntity.parameters.Add(new Parameter("Suspicious_Item_Initial", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspicious_Item_Close", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspicious_Warning", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspicious_Warning_Fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Missing_Buddy", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Search_Started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Search_Loop", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Search_Complete", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Detected_Enemy", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Alien_Heard_Backstage", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Interrogative", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Warning", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Last_Chance", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Stand_Down", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Attack", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Advance", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Melee", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Hit_By_Weapon", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Go_to_Cover", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("No_Cover", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Shoot_From_Cover", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Cover_Broken", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Retreat", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Panic", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Final_Hit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Ally_Death", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Incoming_IED", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Alert_Squad", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("My_Death", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Idle_Passive", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Idle_Aggressive", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Block", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Enter_Grapple", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Grapple_From_Cover", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Player_Observed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("squad_coordinator", new ParameterData())); //Object
                        break;
                    case FunctionType.NPC_Group_DeathCounter:
                        newEntity.parameters.Add(new Parameter("on_threshold", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("TriggerThreshold", new cInteger())); //int
                        break;
                    case FunctionType.NPC_Group_Death_Monitor:
                        newEntity.parameters.Add(new Parameter("last_man_dying", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("all_killed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("squad_coordinator", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("CheckAllNPCs", new cBool())); //bool
                        break;
                    case FunctionType.NPC_SenseLimiter:
                        newEntity.parameters.Add(new Parameter("Sense", new cEnum("SENSORY_TYPE", 0))); //SENSORY_TYPE
                        break;
                    case FunctionType.NPC_ResetSensesAndMemory:
                        newEntity.parameters.Add(new Parameter("ResetMenaceToFull", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ResetSensesLimiters", new cBool())); //bool
                        break;
                    case FunctionType.NPC_SetupMenaceManager:
                        newEntity.parameters.Add(new Parameter("AgressiveMenace", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ProgressionFraction", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ResetMenaceMeter", new cBool())); //bool
                        break;
                    case FunctionType.NPC_AlienConfig:
                        newEntity.parameters.Add(new Parameter("AlienConfigString", new cString())); //String
                        break;
                    case FunctionType.NPC_SetSenseSet:
                        newEntity.parameters.Add(new Parameter("SenseSet", new cEnum("SENSE_SET", 0))); //SENSE_SET
                        break;
                    case FunctionType.NPC_GetLastSensedPositionOfTarget:
                        newEntity.parameters.Add(new Parameter("NoRecentSense", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SensedOnLeft", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SensedOnRight", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SensedInFront", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SensedBehind", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OptionalTarget", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("LastSensedPosition", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("MaxTimeSince", new cFloat())); //float
                        break;
                    case FunctionType.HeldItem_AINotifier:
                        newEntity.parameters.Add(new Parameter("Item", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Duration", new cFloat())); //float
                        break;
                    case FunctionType.NPC_Gain_Aggression_In_Radius:
                        newEntity.parameters.Add(new Parameter("Position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("AggressionGain", new cEnum("AGGRESSION_GAIN", 0))); //AGGRESSION_GAIN
                        break;
                    case FunctionType.NPC_Aggression_Monitor:
                        newEntity.parameters.Add(new Parameter("on_interrogative", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_warning", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_last_chance", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_stand_down", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_idle", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_aggressive", new ParameterData())); //
                        break;
                    case FunctionType.Explosion_AINotifier:
                        newEntity.parameters.Add(new Parameter("on_character_damage_fx", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ExplosionPos", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("AmmoType", new cEnum("AMMO_TYPE", 0))); //AMMO_TYPE
                        break;
                    case FunctionType.NPC_Sleeping_Android_Monitor:
                        newEntity.parameters.Add(new Parameter("Twitch", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SitUp_Start", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SitUp_End", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Sleeping_GetUp", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Sitting_GetUp", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Android_NPC", new ParameterData())); //Object
                        break;
                    case FunctionType.NPC_Highest_Awareness_Monitor:
                        newEntity.parameters.Add(new Parameter("All_Dead", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Stunned", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Unaware", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspicious", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SearchingArea", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SearchingLastSensed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Aware", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_changed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("NPC_Coordinator", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Target", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("trigger_on_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("CheckAllNPCs", new cBool())); //bool
                        break;
                    case FunctionType.NPC_Squad_GetAwarenessState:
                        newEntity.parameters.Add(new Parameter("All_Dead", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Stunned", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Unaware", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspicious", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SearchingArea", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SearchingLastSensed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Aware", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("NPC_Coordinator", new ParameterData())); //Object
                        break;
                    case FunctionType.NPC_Squad_GetAwarenessWatermark:
                        newEntity.parameters.Add(new Parameter("All_Dead", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Stunned", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Unaware", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspicious", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SearchingArea", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SearchingLastSensed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Aware", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("NPC_Coordinator", new ParameterData())); //Object
                        break;
                    case FunctionType.PlayerCameraMonitor:
                        newEntity.parameters.Add(new Parameter("AndroidNeckSnap", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AlienKill", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AlienKillBroken", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AlienKillInVent", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StandardAnimDrivenView", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StopNonStandardCameras", new ParameterData())); //
                        break;
                    case FunctionType.ScreenEffectEventMonitor:
                        newEntity.parameters.Add(new Parameter("MeleeHit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("BulletHit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("MedkitHeal", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StartStrangle", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StopStrangle", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StartLowHealth", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StopLowHealth", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StartDeath", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StopDeath", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AcidHit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("FlashbangHit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("HitAndRun", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("CancelHitAndRun", new ParameterData())); //
                        break;
                    case FunctionType.DEBUG_SenseLevels:
                        newEntity.parameters.Add(new Parameter("no_activation", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trace_activation", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("lower_activation", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("normal_activation", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("upper_activation", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Sense", new cEnum("SENSORY_TYPE", 0))); //SENSORY_TYPE
                        break;
                    case FunctionType.NPC_FakeSense:
                        newEntity.parameters.Add(new Parameter("SensedObject", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("FakePosition", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Sense", new cEnum("SENSORY_TYPE", 0))); //SENSORY_TYPE
                        newEntity.parameters.Add(new Parameter("ForceThreshold", new cEnum("THRESHOLD_QUALIFIER", 0))); //THRESHOLD_QUALIFIER
                        break;
                    case FunctionType.NPC_SuspiciousItem:
                        newEntity.parameters.Add(new Parameter("ItemPosition", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Item", new cEnum("SUSPICIOUS_ITEM", 0))); //SUSPICIOUS_ITEM
                        newEntity.parameters.Add(new Parameter("InitialReactionValidStartDuration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FurtherReactionValidStartDuration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RetriggerDelay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Trigger", new cEnum("SUSPICIOUS_ITEM_TRIGGER", 0))); //SUSPICIOUS_ITEM_TRIGGER
                        newEntity.parameters.Add(new Parameter("ShouldMakeAggressive", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("MaxGroupMembersInteract", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SystematicSearchRadius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("AllowSamePriorityToOveride", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("UseSamePriorityCloserDistanceConstraint", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SamePriorityCloserDistanceConstraint", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("UseSamePriorityRecentTimeConstraint", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("SamePriorityRecentTimeConstraint", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("BehaviourTreePriority", new cEnum("SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY", 0))); //SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY
                        newEntity.parameters.Add(new Parameter("InteruptSubPriority", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("DetectableByBackstageAlien", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DoIntialReaction", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("MoveCloseToSuspectPosition", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DoCloseToReaction", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DoCloseToWaitForGroupMembers", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DoSystematicSearch", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("GroupNotify", new cEnum("SUSPICIOUS_ITEM_STAGE", 0))); //SUSPICIOUS_ITEM_STAGE
                        newEntity.parameters.Add(new Parameter("DoIntialReactionSubsequentGroupMember", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("MoveCloseToSuspectPositionSubsequentGroupMember", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DoCloseToReactionSubsequentGroupMember", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DoCloseToWaitForGroupMembersSubsequentGroupMember", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DoSystematicSearchSubsequentGroupMember", new cBool())); //bool
                        break;
                    case FunctionType.NPC_SetAlienDevelopmentStage:
                        newEntity.parameters.Add(new Parameter("AlienStage", new cEnum("ALIEN_DEVELOPMENT_MANAGER_STAGES", 0))); //ALIEN_DEVELOPMENT_MANAGER_STAGES
                        newEntity.parameters.Add(new Parameter("Reset", new cBool())); //bool
                        break;
                    case FunctionType.NPC_TargetAcquire:
                        newEntity.parameters.Add(new Parameter("no_targets", new ParameterData())); //
                        break;
                    case FunctionType.CHR_IsWithinRange:
                        newEntity.parameters.Add(new Parameter("In_range", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Out_of_range", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Height", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Range_test_shape", new cEnum("RANGE_TEST_SHAPE", 0))); //RANGE_TEST_SHAPE
                        break;
                    case FunctionType.NPC_ForceCombatTarget:
                        newEntity.parameters.Add(new Parameter("Target", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("LockOtherAttackersOut", new cBool())); //bool
                        break;
                    case FunctionType.NPC_SetAimTarget:
                        newEntity.parameters.Add(new Parameter("Target", new ParameterData())); //Object
                        break;
                    case FunctionType.CHR_SetTorch:
                        newEntity.parameters.Add(new Parameter("TorchOn", new cBool())); //bool
                        break;
                    case FunctionType.CHR_GetTorch:
                        newEntity.parameters.Add(new Parameter("torch_on", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("torch_off", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("TorchOn", new cBool())); //bool
                        break;
                    case FunctionType.NPC_SetAutoTorchMode:
                        newEntity.parameters.Add(new Parameter("AutoUseTorchInDark", new cBool())); //bool
                        break;
                    case FunctionType.NPC_GetCombatTarget:
                        newEntity.parameters.Add(new Parameter("bound_trigger", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("target", new ParameterData())); //Object
                        break;
                    case FunctionType.NPC_AreaBox:
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        break;
                    case FunctionType.NPC_MeleeContext:
                        newEntity.parameters.Add(new Parameter("ConvergePos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Context_Type", new cEnum("MELEE_CONTEXT_TYPE", 0))); //MELEE_CONTEXT_TYPE
                        break;
                    case FunctionType.NPC_SetSafePoint:
                        newEntity.parameters.Add(new Parameter("SafePositions", new cTransform())); //Position
                        break;
                    case FunctionType.Player_ExploitableArea:
                        newEntity.parameters.Add(new Parameter("NpcSafePositions", new cTransform())); //Position
                        break;
                    case FunctionType.NPC_SetDefendArea:
                        newEntity.parameters.Add(new Parameter("AreaObjects", new cEnum("NPC_AREA_RESOURCE", 0))); //NPC_AREA_RESOURCE
                        break;
                    case FunctionType.NPC_SetPursuitArea:
                        newEntity.parameters.Add(new Parameter("AreaObjects", new cEnum("NPC_AREA_RESOURCE", 0))); //NPC_AREA_RESOURCE
                        break;
                    case FunctionType.NPC_ForceRetreat:
                        newEntity.parameters.Add(new Parameter("AreaObjects", new cEnum("NPC_AREA_RESOURCE", 0))); //NPC_AREA_RESOURCE
                        break;
                    case FunctionType.NPC_DefineBackstageAvoidanceArea:
                        newEntity.parameters.Add(new Parameter("AreaObjects", new cEnum("NPC_AREA_RESOURCE", 0))); //NPC_AREA_RESOURCE
                        break;
                    case FunctionType.NPC_SetAlertness:
                        newEntity.parameters.Add(new Parameter("AlertState", new cEnum("ALERTNESS_STATE", 0))); //ALERTNESS_STATE
                        break;
                    case FunctionType.NPC_SetStartPos:
                        newEntity.parameters.Add(new Parameter("StartPos", new cTransform())); //Position
                        break;
                    case FunctionType.NPC_SetAgressionProgression:
                        newEntity.parameters.Add(new Parameter("allow_progression", new cBool())); //bool
                        break;
                    case FunctionType.NPC_SetLocomotionTargetSpeed:
                        newEntity.parameters.Add(new Parameter("Speed", new cEnum("LOCOMOTION_TARGET_SPEED", 0))); //LOCOMOTION_TARGET_SPEED
                        break;
                    case FunctionType.NPC_SetGunAimMode:
                        newEntity.parameters.Add(new Parameter("AimingMode", new cEnum("NPC_GUN_AIM_MODE", 0))); //NPC_GUN_AIM_MODE
                        break;
                    case FunctionType.NPC_set_behaviour_tree_flags:
                        newEntity.parameters.Add(new Parameter("BehaviourTreeFlag", new cEnum("BEHAVIOUR_TREE_FLAGS", 0))); //BEHAVIOUR_TREE_FLAGS
                        newEntity.parameters.Add(new Parameter("FlagSetting", new cBool())); //bool
                        break;
                    case FunctionType.NPC_SetHidingSearchRadius:
                        newEntity.parameters.Add(new Parameter("Radius", new cFloat())); //float
                        break;
                    case FunctionType.NPC_SetHidingNearestLocation:
                        newEntity.parameters.Add(new Parameter("hiding_pos", new cTransform())); //Position
                        break;
                    case FunctionType.NPC_WithdrawAlien:
                        newEntity.parameters.Add(new Parameter("allow_any_searches_to_complete", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("permanent", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("killtraps", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("initial_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("timed_out_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("time_to_force", new cFloat())); //float
                        break;
                    case FunctionType.NPC_behaviour_monitor:
                        newEntity.parameters.Add(new Parameter("state_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("state_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("behaviour", new cEnum("BEHAVIOR_TREE_BRANCH_TYPE", 0))); //BEHAVIOR_TREE_BRANCH_TYPE
                        newEntity.parameters.Add(new Parameter("trigger_on_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_checkpoint_restart", new cBool())); //bool
                        break;
                    case FunctionType.NPC_multi_behaviour_monitor:
                        newEntity.parameters.Add(new Parameter("Cinematic_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Cinematic_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Damage_Response_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Damage_Response_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Target_Is_NPC_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Target_Is_NPC_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Breakout_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Breakout_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Attack_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Attack_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Stunned_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Stunned_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Backstage_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Backstage_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("In_Vent_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("In_Vent_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Killtrap_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Killtrap_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Threat_Aware_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Threat_Aware_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspect_Target_Response_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspect_Target_Response_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Player_Hiding_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Player_Hiding_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspicious_Item_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Suspicious_Item_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Search_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Search_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Area_Sweep_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Area_Sweep_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trigger_on_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_checkpoint_restart", new cBool())); //bool
                        break;
                    case FunctionType.NPC_ambush_monitor:
                        newEntity.parameters.Add(new Parameter("setup", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("abandoned", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trap_sprung", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ambush_type", new cEnum("AMBUSH_TYPE", 0))); //AMBUSH_TYPE
                        newEntity.parameters.Add(new Parameter("trigger_on_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_checkpoint_restart", new cBool())); //bool
                        break;
                    case FunctionType.NPC_navmesh_type_monitor:
                        newEntity.parameters.Add(new Parameter("state_set", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("state_unset", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("nav_mesh_type", new cEnum("NAV_MESH_AREA_TYPE", 0))); //NAV_MESH_AREA_TYPE
                        newEntity.parameters.Add(new Parameter("trigger_on_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_checkpoint_restart", new cBool())); //bool
                        break;
                    case FunctionType.CHR_HasWeaponOfType:
                        newEntity.parameters.Add(new Parameter("on_true", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_false", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Result", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("weapon_type", new cEnum("WEAPON_TYPE", 0))); //WEAPON_TYPE
                        newEntity.parameters.Add(new Parameter("check_if_weapon_draw", new cBool())); //bool
                        break;
                    case FunctionType.NPC_TriggerAimRequest:
                        newEntity.parameters.Add(new Parameter("started_aiming", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished_aiming", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("interrupted", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AimTarget", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Raise_gun", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_current_target", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("clamp_angle", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("clear_current_requests", new cBool())); //bool
                        break;
                    case FunctionType.NPC_TriggerShootRequest:
                        newEntity.parameters.Add(new Parameter("started_shooting", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished_shooting", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("interrupted", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("empty_current_clip", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("shot_count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("clear_current_requests", new cBool())); //bool
                        break;
                    case FunctionType.Squad_SetMaxEscalationLevel:
                        newEntity.parameters.Add(new Parameter("max_level", new cEnum("NPC_AGGRO_LEVEL", 0))); //NPC_AGGRO_LEVEL
                        newEntity.parameters.Add(new Parameter("squad_coordinator", new ParameterData())); //Object
                        break;
                    case FunctionType.Chr_PlayerCrouch:
                        newEntity.parameters.Add(new Parameter("crouch", new cBool())); //bool
                        break;
                    case FunctionType.NPC_Once:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        break;
                    case FunctionType.Custom_Hiding_Vignette_controller:
                        newEntity.parameters.Add(new Parameter("StartFade", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("StopFade", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Breath", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Blackout_start_time", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("run_out_time", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Vignette", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FadeValue", new cFloat())); //float
                        break;
                    case FunctionType.Custom_Hiding_Controller:
                        newEntity.parameters.Add(new Parameter("Started_Idle", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Started_Exit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Got_Out", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Prompt_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Prompt_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Start_choking", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Start_oxygen_starvation", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Show_MT", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Hide_MT", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Spawn_MT", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Despawn_MT", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Start_Busted_By_Alien", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Start_Busted_By_Android", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("End_Busted_By_Android", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Start_Busted_By_Human", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("End_Busted_By_Human", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Enter_Anim", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("Idle_Anim", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("Exit_Anim", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("has_MT", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_high", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("AlienBusted_Player_1", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AlienBusted_Alien_1", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AlienBusted_Player_2", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AlienBusted_Alien_2", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AlienBusted_Player_3", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AlienBusted_Alien_3", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AlienBusted_Player_4", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AlienBusted_Alien_4", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AndroidBusted_Player_1", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AndroidBusted_Android_1", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AndroidBusted_Player_2", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("AndroidBusted_Android_2", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("MT_pos", new cTransform())); //Position
                        break;
                    case FunctionType.TorchDynamicMovement:
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("torch", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("max_spatial_velocity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_angular_velocity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_position_displacement", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_target_displacement", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("position_damping", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("target_damping", new cFloat())); //float
                        break;
                    case FunctionType.EQUIPPABLE_ITEM:
                        newEntity.parameters.Add(new Parameter("finished_spawning", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("equipped", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("unequipped", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pickup", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_discard", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_melee_impact", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_used_basic_function", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("spawn_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("item_animated_asset", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("owner", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("has_owner", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_animation_context", new cString())); //String
                        newEntity.parameters.Add(new Parameter("character_activate_animation_context", new cString())); //String
                        newEntity.parameters.Add(new Parameter("left_handed", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("inventory_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("equipment_slot", new cEnum("EQUIPMENT_SLOT", 0))); //EQUIPMENT_SLOT
                        newEntity.parameters.Add(new Parameter("holsters_on_owner", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("holster_node", new cString())); //String
                        newEntity.parameters.Add(new Parameter("holster_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("weapon_handedness", new cEnum("WEAPON_HANDEDNESS", 0))); //WEAPON_HANDEDNESS
                        break;
                    case FunctionType.AIMED_ITEM:
                        newEntity.parameters.Add(new Parameter("on_started_aiming", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_stopped_aiming", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_display_on", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_display_off", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_effect_on", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_effect_off", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("target_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("average_target_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_target_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("fixed_target_distance_for_local_player", new cFloat())); //float
                        break;
                    case FunctionType.MELEE_WEAPON:
                        newEntity.parameters.Add(new Parameter("item_animated_model_and_collision", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("normal_attack_damage", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("power_attack_damage", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("position_input", new cTransform())); //Position
                        break;
                    case FunctionType.AIMED_WEAPON:
                        newEntity.parameters.Add(new Parameter("on_fired_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_fired_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_fired_fail_single", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_impact", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_reload_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_reload_another", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_reload_empty_clip", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_reload_canceled", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_reload_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_reload_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_shooting_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_shooting_wind_down", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_shooting_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_overheated", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_cooled_down", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_charge_complete", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_charge_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_charge_stopped", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_turned_on", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_turned_off", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_torch_on_requested", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_torch_off_requested", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ammoRemainingInClip", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ammoToFillClip", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ammoThatWasInClip", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("charge_percentage", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("charge_noise_percentage", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("weapon_type", new cEnum("WEAPON_TYPE", 0))); //WEAPON_TYPE
                        newEntity.parameters.Add(new Parameter("requires_turning_on", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ejectsShellsOnFiring", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("aim_assist_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("default_ammo_type", new cEnum("AMMO_TYPE", 0))); //AMMO_TYPE
                        newEntity.parameters.Add(new Parameter("starting_ammo", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("clip_size", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("consume_ammo_over_time_when_turned_on", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_auto_shots_per_second", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_manual_shots_per_second", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("wind_down_time_in_seconds", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("maximum_continous_fire_time_in_seconds", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("overheat_recharge_time_in_seconds", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("automatic_firing", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("overheats", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("charged_firing", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("charging_duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_charge_to_fire", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("overcharge_timer", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("charge_noise_start_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("reloadIndividualAmmo", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("alwaysDoFullReloadOfClips", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("movement_accuracy_penalty_per_second", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("aim_rotation_accuracy_penalty_per_second", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("accuracy_penalty_per_shot", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("accuracy_accumulated_per_second", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("player_exposed_accuracy_penalty_per_shot", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("player_exposed_accuracy_accumulated_per_second", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("recoils_on_fire", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("alien_threat_aware", new cBool())); //bool
                        break;
                    case FunctionType.PlayerWeaponMonitor:
                        newEntity.parameters.Add(new Parameter("on_clip_above_percentage", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_clip_below_percentage", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_clip_empty", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_clip_full", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("weapon_type", new cEnum("WEAPON_TYPE", 0))); //WEAPON_TYPE
                        newEntity.parameters.Add(new Parameter("ammo_percentage_in_clip", new cFloat())); //float
                        break;
                    case FunctionType.PlayerDiscardsWeapons:
                        newEntity.parameters.Add(new Parameter("discard_pistol", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_shotgun", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_flamethrower", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_boltgun", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_cattleprod", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_melee", new cBool())); //bool
                        break;
                    case FunctionType.PlayerDiscardsItems:
                        newEntity.parameters.Add(new Parameter("discard_ieds", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_medikits", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_ammo", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_flares_and_lights", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_materials", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_batteries", new cBool())); //bool
                        break;
                    case FunctionType.PlayerDiscardsTools:
                        newEntity.parameters.Add(new Parameter("discard_motion_tracker", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_cutting_torch", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_hacking_tool", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("discard_keycard", new cBool())); //bool
                        break;
                    case FunctionType.WEAPON_GiveToCharacter:
                        newEntity.parameters.Add(new Parameter("Character", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Weapon", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("is_holstered", new cBool())); //bool
                        break;
                    case FunctionType.WEAPON_GiveToPlayer:
                        newEntity.parameters.Add(new Parameter("weapon", new cEnum("EQUIPMENT_SLOT", 0))); //EQUIPMENT_SLOT
                        newEntity.parameters.Add(new Parameter("holster", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("starting_ammo", new cInteger())); //int
                        break;
                    case FunctionType.WEAPON_ImpactEffect:
                        newEntity.parameters.Add(new Parameter("StaticEffects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("DynamicEffects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("DynamicAttachedEffects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Type", new cEnum("WEAPON_IMPACT_EFFECT_TYPE", 0))); //WEAPON_IMPACT_EFFECT_TYPE
                        newEntity.parameters.Add(new Parameter("Orientation", new cEnum("WEAPON_IMPACT_EFFECT_ORIENTATION", 0))); //WEAPON_IMPACT_EFFECT_ORIENTATION
                        newEntity.parameters.Add(new Parameter("Priority", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("SafeDistant", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("LifeTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("character_damage_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RandomRotation", new cBool())); //bool
                        break;
                    case FunctionType.WEAPON_ImpactFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("PhysicMaterial", new cString())); //String
                        break;
                    case FunctionType.WEAPON_AttackerFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.WEAPON_TargetObjectFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.WEAPON_ImpactInspector:
                        newEntity.parameters.Add(new Parameter("damage", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("impact_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("impact_target", new ParameterData())); //Object
                        break;
                    case FunctionType.WEAPON_DamageFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("damage_threshold", new cInteger())); //int
                        break;
                    case FunctionType.WEAPON_DidHitSomethingFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        break;
                    case FunctionType.WEAPON_MultiFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AttackerFilter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("TargetFilter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("DamageThreshold", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("DamageType", new cEnum("DAMAGE_EFFECTS", 0))); //DAMAGE_EFFECTS
                        newEntity.parameters.Add(new Parameter("UseAmmoFilter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("AmmoType", new cEnum("AMMO_TYPE", 0))); //AMMO_TYPE
                        break;
                    case FunctionType.WEAPON_ImpactCharacterFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.parameters.Add(new Parameter("character_body_location", new cEnum("IMPACT_CHARACTER_BODY_LOCATION_TYPE", 0))); //IMPACT_CHARACTER_BODY_LOCATION_TYPE
                        break;
                    case FunctionType.WEAPON_Effect:
                        newEntity.parameters.Add(new Parameter("WorldPos", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("AttachedEffects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("UnattachedEffects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("LifeTime", new cFloat())); //float
                        break;
                    case FunctionType.WEAPON_AmmoTypeFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("AmmoType", new cEnum("DAMAGE_EFFECTS", 0))); //DAMAGE_EFFECTS
                        break;
                    case FunctionType.WEAPON_ImpactAngleFilter:
                        newEntity.parameters.Add(new Parameter("greater", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("less", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ReferenceAngle", new cFloat())); //float
                        break;
                    case FunctionType.WEAPON_ImpactOrientationFilter:
                        newEntity.parameters.Add(new Parameter("passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ThresholdAngle", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Orientation", new cEnum("WEAPON_IMPACT_FILTER_ORIENTATION", 0))); //WEAPON_IMPACT_FILTER_ORIENTATION
                        break;
                    case FunctionType.EFFECT_ImpactGenerator:
                        newEntity.parameters.Add(new Parameter("on_impact", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trigger_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("min_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("spread", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("skip_characters", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_local_rotation", new cBool())); //bool
                        break;
                    case FunctionType.EFFECT_EntityGenerator:
                        newEntity.parameters.Add(new Parameter("entities", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("trigger_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("spread", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("force_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("force_max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("force_offset_XY_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("force_offset_XY_max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("force_offset_Z_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("force_offset_Z_max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lifetime_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lifetime_max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("use_local_rotation", new cBool())); //bool
                        break;
                    case FunctionType.EFFECT_DirectionalPhysics:
                        newEntity.parameters.Add(new Parameter("relative_direction", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("effect_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("angular_falloff", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_force", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_force", new cFloat())); //float
                        break;
                    case FunctionType.PlatformConstantBool:
                        newEntity.parameters.Add(new Parameter("NextGen", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("X360", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("PS3", new cBool())); //bool
                        break;
                    case FunctionType.PlatformConstantInt:
                        newEntity.parameters.Add(new Parameter("NextGen", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("X360", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("PS3", new cInteger())); //int
                        break;
                    case FunctionType.PlatformConstantFloat:
                        newEntity.parameters.Add(new Parameter("NextGen", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("X360", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PS3", new cFloat())); //float
                        break;
                    case FunctionType.VariableBool:
                        newEntity.parameters.Add(new Parameter("initial_value", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_persistent", new cBool())); //bool
                        break;
                    case FunctionType.VariableInt:
                        newEntity.parameters.Add(new Parameter("initial_value", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("is_persistent", new cBool())); //bool
                        break;
                    case FunctionType.VariableFloat:
                        newEntity.parameters.Add(new Parameter("initial_value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("is_persistent", new cBool())); //bool
                        break;
                    case FunctionType.VariableString:
                        newEntity.parameters.Add(new Parameter("initial_value", new cString())); //String
                        newEntity.parameters.Add(new Parameter("is_persistent", new cBool())); //bool
                        break;
                    case FunctionType.VariableVector:
                        newEntity.parameters.Add(new Parameter("initial_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("initial_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("initial_z", new cFloat())); //float
                        break;
                    case FunctionType.VariableVector2:
                        newEntity.parameters.Add(new Parameter("initial_value", new cVector3())); //Direction
                        break;
                    case FunctionType.VariableColour:
                        newEntity.parameters.Add(new Parameter("initial_colour", new cVector3())); //Direction
                        break;
                    case FunctionType.VariableFlashScreenColour:
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("pause_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("initial_colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("flash_layer_name", new cString())); //String
                        break;
                    case FunctionType.VariableHackingConfig:
                        newEntity.parameters.Add(new Parameter("nodes", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("sensors", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("victory_nodes", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("victory_sensors", new cInteger())); //int
                        break;
                    case FunctionType.VariableEnum:
                        newEntity.parameters.Add(new Parameter("initial_value", new cEnum())); //Enum
                        newEntity.parameters.Add(new Parameter("is_persistent", new cBool())); //bool
                        break;
                    case FunctionType.VariableObject:
                        newEntity.parameters.Add(new Parameter("initial", new ParameterData())); //Object
                        break;
                    case FunctionType.VariableAnimationInfo:
                        newEntity.parameters.Add(new Parameter("AnimationSet", new cString())); //String
                        newEntity.parameters.Add(new Parameter("Animation", new cString())); //String
                        break;
                    case FunctionType.ExternalVariableBool:
                        newEntity.parameters.Add(new Parameter("game_variable", new cEnum("GAME_VARIABLE", 0))); //GAME_VARIABLE
                        break;
                    case FunctionType.NonPersistentBool:
                        newEntity.parameters.Add(new Parameter("initial_value", new cBool())); //bool
                        break;
                    case FunctionType.NonPersistentInt:
                        newEntity.parameters.Add(new Parameter("initial_value", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("is_persistent", new cBool())); //bool
                        break;
                    case FunctionType.GameDVR:
                        newEntity.parameters.Add(new Parameter("start_time", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("duration", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("moment_ID", new cEnum("GAME_CLIP", 0))); //GAME_CLIP
                        break;
                    case FunctionType.Zone:
                        newEntity.parameters.Add(new Parameter("composites", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("suspend_on_unload", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("space_visible", new cBool())); //bool
                        break;
                    case FunctionType.ZoneLink:
                        newEntity.parameters.Add(new Parameter("ZoneA", new ParameterData())); //ZonePtr
                        newEntity.parameters.Add(new Parameter("ZoneB", new ParameterData())); //ZonePtr
                        newEntity.parameters.Add(new Parameter("cost", new cInteger())); //int
                        break;
                    case FunctionType.ZoneExclusionLink:
                        newEntity.parameters.Add(new Parameter("ZoneA", new ParameterData())); //ZonePtr
                        newEntity.parameters.Add(new Parameter("ZoneB", new ParameterData())); //ZonePtr
                        newEntity.parameters.Add(new Parameter("exclude_streaming", new cBool())); //bool
                        break;
                    case FunctionType.ZoneLoaded:
                        newEntity.parameters.Add(new Parameter("on_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_unloaded", new ParameterData())); //
                        break;
                    case FunctionType.FlushZoneCache:
                        newEntity.parameters.Add(new Parameter("CurrentGen", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("NextGen", new cBool())); //bool
                        break;
                    case FunctionType.StateQuery:
                        newEntity.parameters.Add(new Parameter("on_true", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_false", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Input", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Result", new cBool())); //bool
                        break;
                    case FunctionType.BooleanLogicInterface:
                        newEntity.parameters.Add(new Parameter("on_true", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_false", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("LHS", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("RHS", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Result", new cBool())); //bool
                        break;
                    case FunctionType.LogicOnce:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        break;
                    case FunctionType.LogicDelay:
                        newEntity.parameters.Add(new Parameter("on_delay_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("can_suspend", new cBool())); //bool
                        break;
                    case FunctionType.LogicSwitch:
                        newEntity.parameters.Add(new Parameter("true_now_false", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("false_now_true", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_true", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_false", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_restored_true", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_restored_false", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("initial_value", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_persistent", new cBool())); //bool
                        break;
                    case FunctionType.LogicGate:
                        newEntity.parameters.Add(new Parameter("on_allowed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_disallowed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("allow", new cBool())); //bool
                        break;
                    case FunctionType.BooleanLogicOperation:
                        newEntity.parameters.Add(new Parameter("Input", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Result", new cBool())); //bool
                        break;
                    case FunctionType.FloatMath_All:
                        newEntity.parameters.Add(new Parameter("Numbers", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.FloatMultiply_All:
                        newEntity.parameters.Add(new Parameter("Invert", new cBool())); //bool
                        break;
                    case FunctionType.FloatMath:
                        newEntity.parameters.Add(new Parameter("LHS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RHS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.FloatMultiplyClamp:
                        newEntity.parameters.Add(new Parameter("Min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Max", new cFloat())); //float
                        break;
                    case FunctionType.FloatClampMultiply:
                        newEntity.parameters.Add(new Parameter("Min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Max", new cFloat())); //float
                        break;
                    case FunctionType.FloatOperation:
                        newEntity.parameters.Add(new Parameter("Input", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.FloatCompare:
                        newEntity.parameters.Add(new Parameter("on_true", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_false", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("LHS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RHS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Threshold", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cBool())); //bool
                        break;
                    case FunctionType.FloatModulate:
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("wave_shape", new cEnum("WAVE_SHAPE", 0))); //WAVE_SHAPE
                        newEntity.parameters.Add(new Parameter("frequency", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("phase", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("amplitude", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("bias", new cFloat())); //float
                        break;
                    case FunctionType.FloatModulateRandom:
                        newEntity.parameters.Add(new Parameter("on_full_switched_on", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_full_switched_off", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("switch_on_anim", new cEnum("LIGHT_TRANSITION", 0))); //LIGHT_TRANSITION
                        newEntity.parameters.Add(new Parameter("switch_on_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("switch_on_custom_frequency", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("switch_on_duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("switch_off_anim", new cEnum("LIGHT_TRANSITION", 0))); //LIGHT_TRANSITION
                        newEntity.parameters.Add(new Parameter("switch_off_custom_frequency", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("switch_off_duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("behaviour_anim", new cEnum("LIGHT_ANIM", 0))); //LIGHT_ANIM
                        newEntity.parameters.Add(new Parameter("behaviour_frequency", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("behaviour_frequency_variance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("behaviour_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("pulse_modulation", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("oscillate_range_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("sparking_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("blink_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("blink_range_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flicker_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flicker_off_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flicker_range_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flicker_off_range_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("disable_behaviour", new cBool())); //bool
                        break;
                    case FunctionType.FloatLinearProportion:
                        newEntity.parameters.Add(new Parameter("Initial_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Target_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Proportion", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.FloatGetLinearProportion:
                        newEntity.parameters.Add(new Parameter("Min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Input", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Proportion", new cFloat())); //float
                        break;
                    case FunctionType.FloatLinearInterpolateTimed:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Initial_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Target_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PingPong", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Loop", new cBool())); //bool
                        break;
                    case FunctionType.FloatLinearInterpolateSpeed:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Initial_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Target_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PingPong", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Loop", new cBool())); //bool
                        break;
                    case FunctionType.FloatLinearInterpolateSpeedAdvanced:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trigger_on_min", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trigger_on_max", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trigger_on_loop", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Initial_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Min_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Max_Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PingPong", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Loop", new cBool())); //bool
                        break;
                    case FunctionType.FloatSmoothStep:
                        newEntity.parameters.Add(new Parameter("Low_Edge", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("High_Edge", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.FloatClamp:
                        newEntity.parameters.Add(new Parameter("Min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.FilterAbsorber:
                        newEntity.parameters.Add(new Parameter("output", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("factor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("start_value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("input", new cFloat())); //float
                        break;
                    case FunctionType.IntegerMath_All:
                        newEntity.parameters.Add(new Parameter("Numbers", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Result", new cInteger())); //int
                        break;
                    case FunctionType.IntegerMath:
                        newEntity.parameters.Add(new Parameter("LHS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("RHS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Result", new cInteger())); //int
                        break;
                    case FunctionType.IntegerOperation:
                        newEntity.parameters.Add(new Parameter("Input", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Result", new cInteger())); //int
                        break;
                    case FunctionType.IntegerCompare:
                        newEntity.parameters.Add(new Parameter("on_true", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_false", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("LHS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("RHS", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Result", new cBool())); //bool
                        break;
                    case FunctionType.IntegerAnalyse:
                        newEntity.parameters.Add(new Parameter("Input", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Result", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val0", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val1", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val2", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val3", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val4", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val5", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val6", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val7", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val8", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Val9", new cInteger())); //int
                        break;
                    case FunctionType.SetEnum:
                        newEntity.parameters.Add(new Parameter("Output", new cEnum())); //Enum
                        newEntity.parameters.Add(new Parameter("initial_value", new cEnum())); //Enum
                        break;
                    case FunctionType.SetString:
                        newEntity.parameters.Add(new Parameter("Output", new cString())); //String
                        newEntity.parameters.Add(new Parameter("initial_value", new cString())); //String
                        break;
                    case FunctionType.VectorMath:
                        newEntity.parameters.Add(new Parameter("LHS", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("RHS", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.VectorScale:
                        newEntity.parameters.Add(new Parameter("LHS", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("RHS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.VectorNormalise:
                        newEntity.parameters.Add(new Parameter("Input", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.VectorModulus:
                        newEntity.parameters.Add(new Parameter("Input", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.ScalarProduct:
                        newEntity.parameters.Add(new Parameter("LHS", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("RHS", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.VectorDirection:
                        newEntity.parameters.Add(new Parameter("From", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("To", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.VectorYaw:
                        newEntity.parameters.Add(new Parameter("Vector", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.VectorRotateYaw:
                        newEntity.parameters.Add(new Parameter("Vector", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Yaw", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.VectorRotateRoll:
                        newEntity.parameters.Add(new Parameter("Vector", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Roll", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.VectorRotatePitch:
                        newEntity.parameters.Add(new Parameter("Vector", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Pitch", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.VectorRotateByPos:
                        newEntity.parameters.Add(new Parameter("Vector", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("WorldPos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.VectorMultiplyByPos:
                        newEntity.parameters.Add(new Parameter("Vector", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("WorldPos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cTransform())); //Position
                        break;
                    case FunctionType.VectorDistance:
                        newEntity.parameters.Add(new Parameter("LHS", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("RHS", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.VectorReflect:
                        newEntity.parameters.Add(new Parameter("Input", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Normal", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.SetVector:
                        newEntity.parameters.Add(new Parameter("x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.SetVector2:
                        newEntity.parameters.Add(new Parameter("Input", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.SetColour:
                        newEntity.parameters.Add(new Parameter("Colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.GetTranslation:
                        newEntity.parameters.Add(new Parameter("Input", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.GetRotation:
                        newEntity.parameters.Add(new Parameter("Input", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.GetComponentInterface:
                        newEntity.parameters.Add(new Parameter("Input", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.SetPosition:
                        newEntity.parameters.Add(new Parameter("Translation", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Rotation", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Input", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("set_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.PositionDistance:
                        newEntity.parameters.Add(new Parameter("LHS", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("RHS", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.VectorLinearProportion:
                        newEntity.parameters.Add(new Parameter("Initial_Value", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Target_Value", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Proportion", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        break;
                    case FunctionType.VectorLinearInterpolateTimed:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Initial_Value", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Target_Value", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Reverse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PingPong", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Loop", new cBool())); //bool
                        break;
                    case FunctionType.VectorLinearInterpolateSpeed:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Initial_Value", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Target_Value", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Reverse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("PingPong", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Loop", new cBool())); //bool
                        break;
                    case FunctionType.MoveInTime:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("end_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("result", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        break;
                    case FunctionType.SmoothMove:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("timer", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("start_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("end_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("start_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("end_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("result", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        break;
                    case FunctionType.RotateInTime:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("origin", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("timer", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("time_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("time_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("time_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("loop", new cBool())); //bool
                        break;
                    case FunctionType.RotateAtSpeed:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("origin", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("timer", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("speed_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("speed_Y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("speed_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("loop", new cBool())); //bool
                        break;
                    case FunctionType.PointAt:
                        newEntity.parameters.Add(new Parameter("origin", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("target", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Result", new cTransform())); //Position
                        break;
                    case FunctionType.SetLocationAndOrientation:
                        newEntity.parameters.Add(new Parameter("location", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("axis", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("local_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("result", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("axis_is", new cEnum("ORIENTATION_AXIS", 0))); //ORIENTATION_AXIS
                        break;
                    case FunctionType.ApplyRelativeTransform:
                        newEntity.parameters.Add(new Parameter("origin", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("destination", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("input", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("output", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("use_trigger_entity", new cBool())); //bool
                        break;
                    case FunctionType.RandomFloat:
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Max", new cFloat())); //float
                        break;
                    case FunctionType.RandomInt:
                        newEntity.parameters.Add(new Parameter("Result", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Min", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Max", new cInteger())); //int
                        break;
                    case FunctionType.RandomBool:
                        newEntity.parameters.Add(new Parameter("Result", new cBool())); //bool
                        break;
                    case FunctionType.RandomVector:
                        newEntity.parameters.Add(new Parameter("Result", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("MinX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MaxX", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MinY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MaxY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MinZ", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MaxZ", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Normalised", new cBool())); //bool
                        break;
                    case FunctionType.RandomSelect:
                        newEntity.parameters.Add(new Parameter("Input", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Result", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Seed", new cFloat())); //float
                        break;
                    case FunctionType.TriggerRandom:
                        newEntity.parameters.Add(new Parameter("Random_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_11", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_12", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Num", new cInteger())); //int
                        break;
                    case FunctionType.TriggerRandomSequence:
                        newEntity.parameters.Add(new Parameter("Random_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("All_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("current", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("num", new cInteger())); //int
                        break;
                    case FunctionType.Persistent_TriggerRandomSequence:
                        newEntity.parameters.Add(new Parameter("Random_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("All_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("current", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("num", new cInteger())); //int
                        break;
                    case FunctionType.TriggerWeightedRandom:
                        newEntity.parameters.Add(new Parameter("Random_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Random_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("current", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Weighting_01", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_02", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_03", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_04", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_05", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_06", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_07", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_08", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_09", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Weighting_10", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("allow_same_pin_in_succession", new cBool())); //bool
                        break;
                    case FunctionType.PlayEnvironmentAnimation:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_finished_streaming", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("play_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("jump_to_the_end_on_play", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("geometry", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("marker", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("external_start_time", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("external_time", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("animation_length", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("animation_info", new cEnum("AnimationInfoPtr", 0))); //AnimationInfoPtr
                        newEntity.parameters.Add(new Parameter("AnimationSet", new cString())); //String
                        newEntity.parameters.Add(new Parameter("Animation", new cString())); //String
                        newEntity.parameters.Add(new Parameter("start_frame", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("end_frame", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("play_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("loop", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_cinematic", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("shot_number", new cInteger())); //int
                        break;
                    case FunctionType.CAGEAnimation:
                        newEntity = new CAGEAnimation(thisID);
                        newEntity.parameters.Add(new Parameter("animation_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("animation_interrupted", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("animation_changed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("cinematic_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("cinematic_unloaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("external_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("current_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("use_external_time", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("rewind_on_stop", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("jump_to_the_end", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("playspeed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("anim_length", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("is_cinematic", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_cinematic_skippable", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("skippable_timer", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("capture_video", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("capture_clip_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("playback", new cFloat())); //float
                        break;
                    case FunctionType.MultitrackLoop:
                        newEntity.parameters.Add(new Parameter("current_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("loop_condition", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("start_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("end_time", new cFloat())); //float
                        break;
                    case FunctionType.ReTransformer:
                        newEntity.parameters.Add(new Parameter("new_transform", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("result", new cTransform())); //Position
                        break;
                    case FunctionType.TriggerSequence:
                        newEntity = new TriggerSequence(thisID);
                        newEntity.parameters.Add(new Parameter("proxy_enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("attach_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("trigger_mode", new cEnum("ANIM_MODE", 0))); //ANIM_MODE
                        newEntity.parameters.Add(new Parameter("random_seed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("use_random_intervals", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("no_duplicates", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("interval_multiplier", new cFloat())); //float
                        break;
                    case FunctionType.Checkpoint:
                        newEntity.parameters.Add(new Parameter("on_checkpoint", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_captured", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_saved", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished_saving", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished_loading", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("cancelled_saving", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished_saving_to_hdd", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("player_spawn_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("is_first_checkpoint", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_first_autorun_checkpoint", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("section", new cString())); //String
                        newEntity.parameters.Add(new Parameter("mission_number", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("checkpoint_type", new cEnum("CHECKPOINT_TYPE", 0))); //CHECKPOINT_TYPE
                        break;
                    case FunctionType.MissionNumber:
                        newEntity.parameters.Add(new Parameter("on_changed", new ParameterData())); //
                        break;
                    case FunctionType.SetAsActiveMissionLevel:
                        newEntity.parameters.Add(new Parameter("clear_level", new cBool())); //bool
                        break;
                    case FunctionType.CheckpointRestoredNotify:
                        newEntity.parameters.Add(new Parameter("restored", new ParameterData())); //
                        break;
                    case FunctionType.DebugLoadCheckpoint:
                        newEntity.parameters.Add(new Parameter("previous_checkpoint", new cBool())); //bool
                        break;
                    case FunctionType.GameStateChanged:
                        newEntity.parameters.Add(new Parameter("mission_number", new cFloat())); //float
                        break;
                    case FunctionType.DisplayMessage:
                        newEntity.parameters.Add(new Parameter("title_id", new cString())); //String
                        newEntity.parameters.Add(new Parameter("message_id", new cString())); //String
                        break;
                    case FunctionType.DisplayMessageWithCallbacks:
                        newEntity.parameters.Add(new Parameter("on_yes", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_no", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_cancel", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("title_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("message_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("yes_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("no_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("cancel_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("yes_button", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("no_button", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("cancel_button", new cBool())); //bool
                        break;
                    case FunctionType.LevelInfo:
                        newEntity.parameters.Add(new Parameter("save_level_name_id", new cString())); //String
                        break;
                    case FunctionType.DebugCheckpoint:
                        newEntity.parameters.Add(new Parameter("on_checkpoint", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("section", new cString())); //String
                        newEntity.parameters.Add(new Parameter("level_reset", new cBool())); //bool
                        break;
                    case FunctionType.Benchmark:
                        newEntity.parameters.Add(new Parameter("benchmark_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("save_stats", new cBool())); //bool
                        break;
                    case FunctionType.EndGame:
                        newEntity.parameters.Add(new Parameter("on_game_end_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_game_ended", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("success", new cBool())); //bool
                        break;
                    case FunctionType.LeaveGame:
                        newEntity.parameters.Add(new Parameter("disconnect_from_session", new cBool())); //bool
                        break;
                    case FunctionType.DebugTextStacking:
                        newEntity.parameters.Add(new Parameter("float_input", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("int_input", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("bool_input", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("vector_input", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("enum_input", new cEnum())); //Enum
                        newEntity.parameters.Add(new Parameter("text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("namespace", new cString())); //String
                        newEntity.parameters.Add(new Parameter("size", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("ci_type", new cEnum("CI_MESSAGE_TYPE", 0))); //CI_MESSAGE_TYPE
                        newEntity.parameters.Add(new Parameter("needs_debug_opt_to_render", new cBool())); //bool
                        break;
                    case FunctionType.DebugText:
                        newEntity.parameters.Add(new Parameter("duration_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("float_input", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("int_input", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("bool_input", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("vector_input", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("enum_input", new cEnum())); //Enum
                        newEntity.parameters.Add(new Parameter("text_input", new cString())); //String
                        newEntity.parameters.Add(new Parameter("text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("namespace", new cString())); //String
                        newEntity.parameters.Add(new Parameter("size", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("alignment", new cEnum("TEXT_ALIGNMENT", 0))); //TEXT_ALIGNMENT
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("pause_game", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("cancel_pause_with_button_press", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("priority", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ci_type", new cEnum("CI_MESSAGE_TYPE", 0))); //CI_MESSAGE_TYPE
                        break;
                    case FunctionType.TutorialMessage:
                        newEntity.parameters.Add(new Parameter("text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("text_list", new cEnum("TUTORIAL_ENTRY_ID", 0))); //TUTORIAL_ENTRY_ID
                        newEntity.parameters.Add(new Parameter("show_animation", new cBool())); //bool
                        break;
                    case FunctionType.DebugEnvironmentMarker:
                        newEntity.parameters.Add(new Parameter("target", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("float_input", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("int_input", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("bool_input", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("vector_input", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("enum_input", new cEnum())); //Enum
                        newEntity.parameters.Add(new Parameter("text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("namespace", new cString())); //String
                        newEntity.parameters.Add(new Parameter("size", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("world_pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("scale_with_distance", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("max_string_length", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("scroll_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("show_distance_from_target", new cBool())); //bool
                        break;
                    case FunctionType.DebugPositionMarker:
                        newEntity.parameters.Add(new Parameter("world_pos", new cTransform())); //Position
                        break;
                    case FunctionType.DebugCaptureScreenShot:
                        newEntity.parameters.Add(new Parameter("finished_capturing", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("wait_for_streamer", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("capture_filename", new cString())); //String
                        newEntity.parameters.Add(new Parameter("fov", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("near", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("far", new cFloat())); //float
                        break;
                    case FunctionType.DebugCaptureCorpse:
                        newEntity.parameters.Add(new Parameter("finished_capturing", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("corpse_name", new cString())); //String
                        break;
                    case FunctionType.DebugMenuToggle:
                        newEntity.parameters.Add(new Parameter("debug_variable", new cString())); //String
                        newEntity.parameters.Add(new Parameter("value", new cBool())); //bool
                        break;
                    case FunctionType.PlayerTorch:
                        newEntity.parameters.Add(new Parameter("requested_torch_holster", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("requested_torch_draw", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("power_in_current_battery", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("battery_count", new cInteger())); //int
                        break;
                    case FunctionType.Master:
                        newEntity.parameters.Add(new Parameter("suspend_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("disable_display", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disable_collision", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disable_simulation", new cBool())); //bool
                        break;
                    case FunctionType.ExclusiveMaster:
                        newEntity.parameters.Add(new Parameter("active_objects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("inactive_objects", new ParameterData())); //Object
                        newEntity.AddResource(ResourceType.EXCLUSIVE_MASTER_STATE_RESOURCE);
                        break;
                    case FunctionType.ThinkOnce:
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_random_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("random_start_delay", new cFloat())); //float
                        break;
                    case FunctionType.Thinker:
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("delay_between_triggers", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("is_continuous", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_random_start", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("random_start_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("total_thinking_time", new cFloat())); //float
                        break;
                    case FunctionType.AllPlayersReady:
                        newEntity.parameters.Add(new Parameter("on_all_players_ready", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("pause_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("activation_delay", new cFloat())); //float
                        break;
                    case FunctionType.SyncOnAllPlayers:
                        newEntity.parameters.Add(new Parameter("on_synchronized", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_synchronized_host", new ParameterData())); //
                        break;
                    case FunctionType.SyncOnFirstPlayer:
                        newEntity.parameters.Add(new Parameter("on_synchronized", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_synchronized_host", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_synchronized_local", new ParameterData())); //
                        break;
                    case FunctionType.NetPlayerCounter:
                        newEntity.parameters.Add(new Parameter("on_full", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_empty", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_intermediate", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("is_full", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_empty", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("contains_local_player", new cBool())); //bool
                        break;
                    case FunctionType.BroadcastTrigger:
                        newEntity.parameters.Add(new Parameter("on_triggered", new ParameterData())); //
                        break;
                    case FunctionType.HostOnlyTrigger:
                        newEntity.parameters.Add(new Parameter("on_triggered", new ParameterData())); //
                        break;
                    case FunctionType.SpawnGroup:
                        newEntity.parameters.Add(new Parameter("on_spawn_request", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("default_group", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.RespawnExcluder:
                        newEntity.parameters.Add(new Parameter("excluded_points", new ParameterData())); //Object
                        break;
                    case FunctionType.RespawnConfig:
                        newEntity.parameters.Add(new Parameter("min_dist", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("preferred_dist", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_dist", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("respawn_mode", new cEnum("RESPAWN_MODE", 0))); //RESPAWN_MODE
                        newEntity.parameters.Add(new Parameter("respawn_wait_time", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("uncollidable_time", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("is_default", new cBool())); //bool
                        break;
                    case FunctionType.NumConnectedPlayers:
                        newEntity.parameters.Add(new Parameter("on_count_changed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("count", new cInteger())); //int
                        break;
                    case FunctionType.NumPlayersOnStart:
                        newEntity.parameters.Add(new Parameter("count", new cInteger())); //int
                        break;
                    case FunctionType.NetworkedTimer:
                        newEntity.parameters.Add(new Parameter("on_second_changed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_started_counting", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_finished_counting", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("time_elapsed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("time_left", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("time_elapsed_sec", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("time_left_sec", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        break;
                    case FunctionType.DebugObjectMarker:
                        newEntity.parameters.Add(new Parameter("marked_object", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("marked_name", new cString())); //String
                        break;
                    case FunctionType.EggSpawner:
                        newEntity.parameters.Add(new Parameter("egg_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("hostile_egg", new cBool())); //bool
                        break;
                    case FunctionType.RandomObjectSelector:
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("chosen_object", new ParameterData())); //Object
                        break;
                    case FunctionType.CompoundVolume:
                        newEntity.parameters.Add(new Parameter("event", new ParameterData())); //
                        break;
                    case FunctionType.TriggerVolumeFilter:
                        newEntity.parameters.Add(new Parameter("on_event_entered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_event_exited", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.TriggerVolumeFilter_Monitored:
                        newEntity.parameters.Add(new Parameter("on_event_entered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_event_exited", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.TriggerFilter:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.TriggerObjectsFilter:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        break;
                    case FunctionType.BindObjectsMultiplexer:
                        newEntity.parameters.Add(new Parameter("Pin1_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin2_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin3_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin4_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin5_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin6_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin7_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin8_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin9_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin10_Bound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        break;
                    case FunctionType.TriggerObjectsFilterCounter:
                        newEntity.parameters.Add(new Parameter("none_passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("some_passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("all_passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.TriggerContainerObjectsFilterCounter:
                        newEntity.parameters.Add(new Parameter("none_passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("some_passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("all_passed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("container", new ParameterData())); //Object
                        break;
                    case FunctionType.TriggerTouch:
                        newEntity.parameters.Add(new Parameter("touch_event", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("physics_object", new cEnum("COLLISION_MAPPING", 0))); //COLLISION_MAPPING
                        newEntity.parameters.Add(new Parameter("impact_normal", new cVector3())); //Direction
                        break;
                    case FunctionType.TriggerDamaged:
                        newEntity.parameters.Add(new Parameter("on_damaged", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("physics_object", new cEnum("COLLISION_MAPPING", 0))); //COLLISION_MAPPING
                        newEntity.parameters.Add(new Parameter("impact_normal", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("threshold", new cFloat())); //float
                        break;
                    case FunctionType.TriggerBindCharacter:
                        newEntity.parameters.Add(new Parameter("bound_trigger", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("characters", new ParameterData())); //Object
                        break;
                    case FunctionType.TriggerBindAllCharactersOfType:
                        newEntity.parameters.Add(new Parameter("bound_trigger", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character_class", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TriggerBindCharactersInSquad:
                        newEntity.parameters.Add(new Parameter("bound_trigger", new ParameterData())); //
                        break;
                    case FunctionType.TriggerUnbindCharacter:
                        newEntity.parameters.Add(new Parameter("unbound_trigger", new ParameterData())); //
                        break;
                    case FunctionType.TriggerExtractBoundObject:
                        newEntity.parameters.Add(new Parameter("unbound_trigger", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("bound_object", new ParameterData())); //Object
                        break;
                    case FunctionType.TriggerExtractBoundCharacter:
                        newEntity.parameters.Add(new Parameter("unbound_trigger", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("bound_character", new ParameterData())); //Object
                        break;
                    case FunctionType.TriggerDelay:
                        newEntity.parameters.Add(new Parameter("delayed_trigger", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("purged_trigger", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("time_left", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Hrs", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Sec", new cFloat())); //float
                        break;
                    case FunctionType.TriggerSwitch:
                        newEntity.parameters.Add(new Parameter("Pin_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("current", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("num", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("loop", new cBool())); //bool
                        break;
                    case FunctionType.TriggerSelect:
                        newEntity.parameters.Add(new Parameter("Pin_0", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_11", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_12", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_13", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_14", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_15", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_16", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Object_0", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_1", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_2", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_3", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_4", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_5", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_6", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_7", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_8", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_9", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_10", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_11", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_12", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_13", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_14", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_15", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_16", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Result", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("index", new cInteger())); //int
                        break;
                    case FunctionType.TriggerSelect_Direct:
                        newEntity.parameters.Add(new Parameter("Changed_to_0", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_11", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_12", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_13", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_14", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_15", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Changed_to_16", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Object_0", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_1", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_2", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_3", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_4", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_5", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_6", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_7", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_8", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_9", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_10", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_11", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_12", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_13", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_14", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_15", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Object_16", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Result", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("TriggeredIndex", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Changes_only", new cBool())); //bool
                        break;
                    case FunctionType.TriggerCheckDifficulty:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("DifficultyLevel", new cEnum("DIFFICULTY_SETTING_TYPE", 0))); //DIFFICULTY_SETTING_TYPE
                        break;
                    case FunctionType.TriggerSync:
                        newEntity.parameters.Add(new Parameter("Pin1_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin2_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin3_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin4_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin5_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin6_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin7_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin8_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin9_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin10_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("reset_on_trigger", new cBool())); //bool
                        break;
                    case FunctionType.LogicAll:
                        newEntity.parameters.Add(new Parameter("Pin1_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin2_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin3_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin4_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin5_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin6_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin7_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin8_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin9_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin10_Synced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("num", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("reset_on_trigger", new cBool())); //bool
                        break;
                    case FunctionType.Logic_MultiGate:
                        newEntity.parameters.Add(new Parameter("Underflow", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_11", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_12", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_13", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_14", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_15", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_16", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_17", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_18", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_19", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pin_20", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Overflow", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trigger_pin", new cInteger())); //int
                        break;
                    case FunctionType.Counter:
                        newEntity.parameters.Add(new Parameter("on_under_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_over_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("is_limitless", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_limit", new cInteger())); //int
                        break;
                    case FunctionType.LogicCounter:
                        newEntity.parameters.Add(new Parameter("on_under_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_over_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("restored_on_under_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("restored_on_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("restored_on_over_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("is_limitless", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_limit", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("non_persistent", new cBool())); //bool
                        break;
                    case FunctionType.LogicPressurePad:
                        newEntity.parameters.Add(new Parameter("Pad_Activated", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pad_Deactivated", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("bound_characters", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Limit", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Count", new cInteger())); //int
                        break;
                    case FunctionType.SetObject:
                        newEntity.parameters.Add(new Parameter("Input", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Output", new ParameterData())); //Object
                        break;
                    case FunctionType.GateResourceInterface:
                        newEntity.parameters.Add(new Parameter("gate_status_changed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("request_open_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("request_lock_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("force_open_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("force_close_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_auto", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("auto_close_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("is_open", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_locked", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("gate_status", new cInteger())); //int
                        break;
                    case FunctionType.Door:
                        newEntity.parameters.Add(new Parameter("started_opening", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("started_closing", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished_opening", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished_closing", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("used_locked", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("used_unlocked", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("used_forced_open", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("used_forced_closed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("waiting_to_open", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("highlight", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("unhighlight", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("zone_link", new ParameterData())); //ZoneLinkPtr
                        newEntity.parameters.Add(new Parameter("animation", new cEnum("ANIMATED_MODEL", 0))); //ANIMATED_MODEL
                        newEntity.parameters.Add(new Parameter("trigger_filter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("icon_pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("icon_usable_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("show_icon_when_locked", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("nav_mesh", new cEnum("NAV_MESH_BARRIER_RESOURCE", 0))); //NAV_MESH_BARRIER_RESOURCE
                        newEntity.parameters.Add(new Parameter("wait_point_1", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("wait_point_2", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("geometry", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("is_scripted", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("wait_to_open", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_waiting", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("unlocked_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("locked_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("icon_keyframe", new cEnum("UI_ICON_ICON", 0))); //UI_ICON_ICON
                        newEntity.parameters.Add(new Parameter("detach_anim", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("invert_nav_mesh_barrier", new cBool())); //bool
                        break;
                    case FunctionType.MonitorPadInput:
                        newEntity.parameters.Add(new Parameter("on_pressed_A", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_A", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_B", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_B", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_X", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_X", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_Y", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_Y", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_L1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_L1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_R1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_R1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_L2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_L2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_R2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_R2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_L3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_L3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_R3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_R3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_dpad_left", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_dpad_left", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_dpad_right", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_dpad_right", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_dpad_up", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_dpad_up", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_dpad_down", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_dpad_down", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("left_stick_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("left_stick_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("right_stick_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("right_stick_y", new cFloat())); //float
                        break;
                    case FunctionType.MonitorActionMap:
                        newEntity.parameters.Add(new Parameter("on_pressed_use", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_use", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_crouch", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_crouch", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_run", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_run", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_aim", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_aim", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_shoot", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_shoot", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_reload", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_reload", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_melee", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_melee", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_activate_item", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_activate_item", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_switch_weapon", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_switch_weapon", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_change_dof_focus", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_change_dof_focus", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_select_motion_tracker", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_select_motion_tracker", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_select_torch", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_select_torch", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_torch_beam", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_torch_beam", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_peek", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_peek", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pressed_back_close", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_released_back_close", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("movement_stick_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("movement_stick_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("camera_stick_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("camera_stick_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("mouse_x", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("mouse_y", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("analog_aim", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("analog_shoot", new cFloat())); //float
                        break;
                    case FunctionType.PadLightBar:
                        newEntity.parameters.Add(new Parameter("colour", new cVector3())); //Direction
                        break;
                    case FunctionType.PadRumbleImpulse:
                        newEntity.parameters.Add(new Parameter("low_frequency_rumble", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("high_frequency_rumble", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("left_trigger_impulse", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("right_trigger_impulse", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("aim_trigger_impulse", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("shoot_trigger_impulse", new cFloat())); //float
                        break;
                    case FunctionType.TriggerViewCone:
                        newEntity.parameters.Add(new Parameter("enter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("target_is_visible", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("no_target_visible", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("target", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("fov", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("aspect_ratio", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("source_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("intersect_with_geometry", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("visible_target", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("target_offset", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("visible_area_type", new cEnum("VIEWCONE_TYPE", 0))); //VIEWCONE_TYPE
                        newEntity.parameters.Add(new Parameter("visible_area_horizontal", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("visible_area_vertical", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("raycast_grace", new cFloat())); //float
                        break;
                    case FunctionType.TriggerCameraViewCone:
                        newEntity.parameters.Add(new Parameter("enter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("target", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("fov", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("aspect_ratio", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("intersect_with_geometry", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_camera_fov", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("target_offset", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("visible_area_type", new cEnum("VIEWCONE_TYPE", 0))); //VIEWCONE_TYPE
                        newEntity.parameters.Add(new Parameter("visible_area_horizontal", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("visible_area_vertical", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("raycast_grace", new cFloat())); //float
                        break;
                    case FunctionType.TriggerCameraViewConeMulti:
                        newEntity.parameters.Add(new Parameter("enter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("target", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target1", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target2", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target3", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target4", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target5", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target6", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target7", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target8", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target9", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("fov", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("aspect_ratio", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("intersect_with_geometry", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("number_of_inputs", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("use_camera_fov", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("visible_area_type", new cEnum("VIEWCONE_TYPE", 0))); //VIEWCONE_TYPE
                        newEntity.parameters.Add(new Parameter("visible_area_horizontal", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("visible_area_vertical", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("raycast_grace", new cFloat())); //float
                        break;
                    case FunctionType.TriggerCameraVolume:
                        newEntity.parameters.Add(new Parameter("inside", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("exit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("inside_factor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lookat_factor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lookat_X_position", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lookat_Y_position", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("start_radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("radius", new cFloat())); //float
                        break;
                    case FunctionType.NPC_Debug_Menu_Item:
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //Object
                        break;
                    case FunctionType.Character:
                        newEntity.parameters.Add(new Parameter("finished_spawning", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished_respawning", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("dead_container_take_slot", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("dead_container_emptied", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_ragdoll_impact", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_footstep", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_despawn_requested", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("spawn_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("contents_of_dead_container", new cEnum("INVENTORY_ITEM_QUANTITY", 0))); //INVENTORY_ITEM_QUANTITY
                        newEntity.parameters.Add(new Parameter("PopToNavMesh", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_cinematic", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("disable_dead_container", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("allow_container_without_death", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("container_interaction_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("anim_set", new cEnum("ANIM_SET", 0))); //ANIM_SET
                        newEntity.parameters.Add(new Parameter("anim_tree_set", new cEnum("ANIM_TREE_SET", 0))); //ANIM_TREE_SET
                        newEntity.parameters.Add(new Parameter("attribute_set", new cEnum("ATTRIBUTE_SET", 0))); //ATTRIBUTE_SET
                        newEntity.parameters.Add(new Parameter("is_player", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_backstage", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("force_backstage_on_respawn", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_class", new cEnum("CHARACTER_CLASS", 0))); //CHARACTER_CLASS
                        newEntity.parameters.Add(new Parameter("alliance_group", new cEnum("ALLIANCE_GROUP", 0))); //ALLIANCE_GROUP
                        newEntity.parameters.Add(new Parameter("dialogue_voice", new cEnum("DIALOGUE_VOICE_ACTOR", 0))); //DIALOGUE_VOICE_ACTOR
                        newEntity.parameters.Add(new Parameter("spawn_id", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("display_model", new cString())); //String
                        newEntity.parameters.Add(new Parameter("reference_skeleton", new cEnum("CHR_SKELETON_SET", 0))); //CHR_SKELETON_SET
                        newEntity.parameters.Add(new Parameter("torso_sound", new cEnum("SOUND_TORSO_GROUP", 0))); //SOUND_TORSO_GROUP
                        newEntity.parameters.Add(new Parameter("leg_sound", new cEnum("SOUND_LEG_GROUP", 0))); //SOUND_LEG_GROUP
                        newEntity.parameters.Add(new Parameter("footwear_sound", new cEnum("SOUND_FOOTWEAR_GROUP", 0))); //SOUND_FOOTWEAR_GROUP
                        newEntity.parameters.Add(new Parameter("custom_character_type", new cEnum("CUSTOM_CHARACTER_TYPE", 0))); //CUSTOM_CHARACTER_TYPE
                        newEntity.parameters.Add(new Parameter("custom_character_accessory_override", new cEnum("CUSTOM_CHARACTER_ACCESSORY_OVERRIDE", 0))); //CUSTOM_CHARACTER_ACCESSORY_OVERRIDE
                        newEntity.parameters.Add(new Parameter("custom_character_population_type", new cEnum("CUSTOM_CHARACTER_POPULATION", 0))); //CUSTOM_CHARACTER_POPULATION
                        newEntity.parameters.Add(new Parameter("named_custom_character", new cString())); //String
                        newEntity.parameters.Add(new Parameter("named_custom_character_assets_set", new cEnum("CUSTOM_CHARACTER_ASSETS", 0))); //CUSTOM_CHARACTER_ASSETS
                        newEntity.parameters.Add(new Parameter("gcip_distribution_bias", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("inventory_set", new cEnum("PLAYER_INVENTORY_SET", 0))); //PLAYER_INVENTORY_SET
                        break;
                    case FunctionType.RegisterCharacterModel:
                        newEntity.parameters.Add(new Parameter("display_model", new cString())); //String
                        newEntity.parameters.Add(new Parameter("reference_skeleton", new cEnum("CHR_SKELETON_SET", 0))); //CHR_SKELETON_SET
                        break;
                    case FunctionType.DespawnPlayer:
                        newEntity.parameters.Add(new Parameter("despawned", new ParameterData())); //
                        break;
                    case FunctionType.DespawnCharacter:
                        newEntity.parameters.Add(new Parameter("despawned", new ParameterData())); //
                        break;
                    case FunctionType.FilterAnd:
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.FilterOr:
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.FilterNot:
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        break;
                    case FunctionType.FilterIsEnemyOfCharacter:
                        newEntity.parameters.Add(new Parameter("Character", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("use_alliance_at_death", new cBool())); //bool
                        break;
                    case FunctionType.FilterIsEnemyOfAllianceGroup:
                        newEntity.parameters.Add(new Parameter("alliance_group", new cEnum("ALLIANCE_GROUP", 0))); //ALLIANCE_GROUP
                        break;
                    case FunctionType.FilterIsPhysicsObject:
                        newEntity.parameters.Add(new Parameter("object", new ParameterData())); //Object
                        break;
                    case FunctionType.FilterIsObject:
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        break;
                    case FunctionType.FilterIsCharacter:
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //Object
                        break;
                    case FunctionType.FilterIsFacingTarget:
                        newEntity.parameters.Add(new Parameter("target", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("tolerance", new cFloat())); //float
                        break;
                    case FunctionType.FilterBelongsToAlliance:
                        newEntity.parameters.Add(new Parameter("alliance_group", new cEnum("ALLIANCE_GROUP", 0))); //ALLIANCE_GROUP
                        break;
                    case FunctionType.FilterHasWeaponOfType:
                        newEntity.parameters.Add(new Parameter("weapon_type", new cEnum("WEAPON_TYPE", 0))); //WEAPON_TYPE
                        break;
                    case FunctionType.FilterHasWeaponEquipped:
                        newEntity.parameters.Add(new Parameter("weapon_type", new cEnum("WEAPON_TYPE", 0))); //WEAPON_TYPE
                        break;
                    case FunctionType.FilterIsinInventory:
                        newEntity.parameters.Add(new Parameter("ItemName", new cString())); //String
                        break;
                    case FunctionType.FilterIsCharacterClass:
                        newEntity.parameters.Add(new Parameter("character_class", new cEnum("CHARACTER_CLASS", 0))); //CHARACTER_CLASS
                        break;
                    case FunctionType.FilterIsCharacterClassCombo:
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.FilterIsInAlertnessState:
                        newEntity.parameters.Add(new Parameter("AlertState", new cEnum("ALERTNESS_STATE", 0))); //ALERTNESS_STATE
                        break;
                    case FunctionType.FilterIsInLocomotionState:
                        newEntity.parameters.Add(new Parameter("State", new cEnum("LOCOMOTION_STATE", 0))); //LOCOMOTION_STATE
                        break;
                    case FunctionType.FilterCanSeeTarget:
                        newEntity.parameters.Add(new Parameter("Target", new ParameterData())); //Object
                        break;
                    case FunctionType.FilterIsAgressing:
                        newEntity.parameters.Add(new Parameter("Target", new ParameterData())); //Object
                        break;
                    case FunctionType.FilterIsValidInventoryItem:
                        newEntity.parameters.Add(new Parameter("item", new cEnum("INVENTORY_ITEM_QUANTITY", 0))); //INVENTORY_ITEM_QUANTITY
                        break;
                    case FunctionType.FilterIsInWeaponRange:
                        newEntity.parameters.Add(new Parameter("weapon_owner", new ParameterData())); //Object
                        break;
                    case FunctionType.TriggerWhenSeeTarget:
                        newEntity.parameters.Add(new Parameter("seen", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Target", new ParameterData())); //Object
                        break;
                    case FunctionType.FilterIsPlatform:
                        newEntity.parameters.Add(new Parameter("Platform", new cEnum("PLATFORM_TYPE", 0))); //PLATFORM_TYPE
                        break;
                    case FunctionType.FilterIsUsingDevice:
                        newEntity.parameters.Add(new Parameter("Device", new cEnum("INPUT_DEVICE_TYPE", 0))); //INPUT_DEVICE_TYPE
                        break;
                    case FunctionType.FilterSmallestUsedDifficulty:
                        newEntity.parameters.Add(new Parameter("difficulty", new cEnum("DIFFICULTY_SETTING_TYPE", 0))); //DIFFICULTY_SETTING_TYPE
                        break;
                    case FunctionType.FilterHasPlayerCollectedIdTag:
                        newEntity.parameters.Add(new Parameter("tag_id", new cEnum("IDTAG_ID", 0))); //IDTAG_ID
                        break;
                    case FunctionType.FilterHasBehaviourTreeFlagSet:
                        newEntity.parameters.Add(new Parameter("BehaviourTreeFlag", new cEnum("BEHAVIOUR_TREE_FLAGS", 0))); //BEHAVIOUR_TREE_FLAGS
                        break;
                    case FunctionType.Job:
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.JOB_Idle:
                        newEntity.parameters.Add(new Parameter("task_operation_mode", new cEnum("TASK_OPERATION_MODE", 0))); //TASK_OPERATION_MODE
                        newEntity.parameters.Add(new Parameter("should_perform_all_tasks", new cBool())); //bool
                        break;
                    case FunctionType.JOB_SpottingPosition:
                        newEntity.parameters.Add(new Parameter("SpottingPosition", new cTransform())); //Position
                        break;
                    case FunctionType.Task:
                        newEntity.parameters.Add(new Parameter("start_command", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("selected_by_npc", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("clean_up", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Job", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("TaskPosition", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("should_stop_moving_when_reached", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("should_orientate_when_reached", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("reached_distance_threshold", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("selection_priority", new cEnum("TASK_PRIORITY", 0))); //TASK_PRIORITY
                        newEntity.parameters.Add(new Parameter("timeout", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("always_on_tracker", new cBool())); //bool
                        break;
                    case FunctionType.FlareTask:
                        newEntity.parameters.Add(new Parameter("specific_character", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("filter_options", new cEnum("TASK_CHARACTER_CLASS_FILTER", 0))); //TASK_CHARACTER_CLASS_FILTER
                        break;
                    case FunctionType.IdleTask:
                        newEntity.parameters.Add(new Parameter("start_pre_move", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_interrupt", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("interrupted_while_moving", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("specific_character", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("should_auto_move_to_position", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("ignored_for_auto_selection", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("has_pre_move_script", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("has_interrupt_script", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("filter_options", new cEnum("TASK_CHARACTER_CLASS_FILTER", 0))); //TASK_CHARACTER_CLASS_FILTER
                        break;
                    case FunctionType.FollowTask:
                        newEntity.parameters.Add(new Parameter("can_initially_end_early", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("stop_radius", new cFloat())); //float
                        break;
                    case FunctionType.NPC_ForceNextJob:
                        newEntity.parameters.Add(new Parameter("job_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("job_completed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("job_interrupted", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ShouldInterruptCurrentTask", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Job", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("InitialTask", new ParameterData())); //Object
                        break;
                    case FunctionType.NPC_SetRateOfFire:
                        newEntity.parameters.Add(new Parameter("MinTimeBetweenShots", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RandomRange", new cFloat())); //float
                        break;
                    case FunctionType.NPC_SetFiringRhythm:
                        newEntity.parameters.Add(new Parameter("MinShootingTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RandomRangeShootingTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MinNonShootingTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RandomRangeNonShootingTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MinCoverNonShootingTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("RandomRangeCoverNonShootingTime", new cFloat())); //float
                        break;
                    case FunctionType.NPC_SetFiringAccuracy:
                        newEntity.parameters.Add(new Parameter("Accuracy", new cFloat())); //float
                        break;
                    case FunctionType.TriggerBindAllNPCs:
                        newEntity.parameters.Add(new Parameter("npc_inside", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("npc_outside", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("centre", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("radius", new cFloat())); //float
                        break;
                    case FunctionType.Trigger_AudioOccluded:
                        newEntity.parameters.Add(new Parameter("NotOccluded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Occluded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Range", new cFloat())); //float
                        break;
                    case FunctionType.SwitchLevel:
                        newEntity.parameters.Add(new Parameter("level_name", new cString())); //String
                        break;
                    case FunctionType.SoundPlaybackBaseClass:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("attached_sound_object", new cEnum("SOUND_OBJECT", 0))); //SOUND_OBJECT
                        newEntity.parameters.Add(new Parameter("sound_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("is_occludable", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("argument_1", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        newEntity.parameters.Add(new Parameter("argument_2", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        newEntity.parameters.Add(new Parameter("argument_3", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        newEntity.parameters.Add(new Parameter("argument_4", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        newEntity.parameters.Add(new Parameter("argument_5", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        newEntity.parameters.Add(new Parameter("namespace", new cString())); //String
                        newEntity.parameters.Add(new Parameter("object_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("restore_on_checkpoint", new cBool())); //bool
                        break;
                    case FunctionType.Sound:
                        newEntity.parameters.Add(new Parameter("stop_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("is_static_ambience", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("start_on", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("multi_trigger", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_multi_emitter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("create_sound_object", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("switch_name", new cEnum("SOUND_SWITCH", 0))); //SOUND_SWITCH
                        newEntity.parameters.Add(new Parameter("switch_value", new cString())); //String
                        newEntity.parameters.Add(new Parameter("last_gen_enabled", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("resume_after_suspended", new cBool())); //bool
                        break;
                    case FunctionType.Speech:
                        newEntity.parameters.Add(new Parameter("on_speech_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("alt_character", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("speech_priority", new cEnum("SPEECH_PRIORITY", 0))); //SPEECH_PRIORITY
                        newEntity.parameters.Add(new Parameter("queue_time", new cFloat())); //float
                        break;
                    case FunctionType.NPC_DynamicDialogueGlobalRange:
                        newEntity.parameters.Add(new Parameter("dialogue_range", new cFloat())); //float
                        break;
                    case FunctionType.CHR_PlayNPCBark:
                        newEntity.parameters.Add(new Parameter("on_speech_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_speech_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("queue_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("sound_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("speech_priority", new cEnum("SPEECH_PRIORITY", 0))); //SPEECH_PRIORITY
                        newEntity.parameters.Add(new Parameter("dialogue_mode", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        newEntity.parameters.Add(new Parameter("action", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        break;
                    case FunctionType.SpeechScript:
                        newEntity.parameters.Add(new Parameter("on_script_ended", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character_01", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("character_02", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("character_03", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("character_04", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("character_05", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("alt_character_01", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("alt_character_02", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("alt_character_03", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("alt_character_04", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("alt_character_05", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("speech_priority", new cEnum("SPEECH_PRIORITY", 0))); //SPEECH_PRIORITY
                        newEntity.parameters.Add(new Parameter("is_occludable", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("line_01_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_01_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_02_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_02_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_02_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_03_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_03_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_03_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_04_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_04_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_04_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_05_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_05_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_05_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_06_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_06_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_06_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_07_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_07_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_07_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_08_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_08_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_08_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_09_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_09_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_09_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("line_10_delay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("line_10_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("line_10_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("restore_on_checkpoint", new cBool())); //bool
                        break;
                    case FunctionType.SoundNetworkNode:
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        break;
                    case FunctionType.SoundEnvironmentMarker:
                        newEntity.parameters.Add(new Parameter("reverb_name", new cEnum("SOUND_REVERB", 0))); //SOUND_REVERB
                        newEntity.parameters.Add(new Parameter("on_enter_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("on_exit_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("linked_network_occlusion_scaler", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("room_size", new cEnum("SOUND_STATE", 0))); //SOUND_STATE
                        newEntity.parameters.Add(new Parameter("disable_network_creation", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        break;
                    case FunctionType.SoundEnvironmentZone:
                        newEntity.parameters.Add(new Parameter("reverb_name", new cEnum("SOUND_REVERB", 0))); //SOUND_REVERB
                        newEntity.parameters.Add(new Parameter("priority", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        break;
                    case FunctionType.SoundLoadBank:
                        newEntity.parameters.Add(new Parameter("bank_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("sound_bank", new cString())); //String
                        newEntity.parameters.Add(new Parameter("trigger_via_pin", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("memory_pool", new cEnum("SOUND_POOL", 0))); //SOUND_POOL
                        break;
                    case FunctionType.SoundLoadSlot:
                        newEntity.parameters.Add(new Parameter("bank_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("sound_bank", new cString())); //String
                        newEntity.parameters.Add(new Parameter("memory_pool", new cEnum("SOUND_POOL", 0))); //SOUND_POOL
                        break;
                    case FunctionType.SoundSetRTPC:
                        newEntity.parameters.Add(new Parameter("rtpc_value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("sound_object", new cEnum("SOUND_OBJECT", 0))); //SOUND_OBJECT
                        newEntity.parameters.Add(new Parameter("rtpc_name", new cEnum("SOUND_RTPC", 0))); //SOUND_RTPC
                        newEntity.parameters.Add(new Parameter("smooth_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("start_on", new cBool())); //bool
                        break;
                    case FunctionType.SoundSetState:
                        newEntity.parameters.Add(new Parameter("state_name", new cEnum("SOUND_STATE", 0))); //SOUND_STATE
                        newEntity.parameters.Add(new Parameter("state_value", new cString())); //String
                        break;
                    case FunctionType.SoundSetSwitch:
                        newEntity.parameters.Add(new Parameter("sound_object", new cEnum("SOUND_OBJECT", 0))); //SOUND_OBJECT
                        newEntity.parameters.Add(new Parameter("switch_name", new cEnum("SOUND_SWITCH", 0))); //SOUND_SWITCH
                        newEntity.parameters.Add(new Parameter("switch_value", new cString())); //String
                        break;
                    case FunctionType.SoundImpact:
                        newEntity.parameters.Add(new Parameter("sound_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("is_occludable", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("argument_1", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        newEntity.parameters.Add(new Parameter("argument_2", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        newEntity.parameters.Add(new Parameter("argument_3", new cEnum("SOUND_ARGUMENT", 0))); //SOUND_ARGUMENT
                        break;
                    case FunctionType.SoundBarrier:
                        newEntity.parameters.Add(new Parameter("default_open", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("band_aid", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("override_value", new cFloat())); //float
                        newEntity.AddResource(ResourceType.COLLISION_MAPPING);
                        break;
                    case FunctionType.MusicController:
                        newEntity.parameters.Add(new Parameter("music_start_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("music_end_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("music_restart_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("layer_control_rtpc", new cEnum("SOUND_PARAMETER", 0))); //SOUND_PARAMETER
                        newEntity.parameters.Add(new Parameter("smooth_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alien_max_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("object_max_distance", new cFloat())); //float
                        break;
                    case FunctionType.MusicTrigger:
                        newEntity.parameters.Add(new Parameter("on_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("connected_object", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("music_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("smooth_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("queue_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("interrupt_all", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("trigger_once", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("rtpc_set_mode", new cEnum("MUSIC_RTPC_MODE", 0))); //MUSIC_RTPC_MODE
                        newEntity.parameters.Add(new Parameter("rtpc_target_value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("rtpc_duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("rtpc_set_return_mode", new cEnum("MUSIC_RTPC_MODE", 0))); //MUSIC_RTPC_MODE
                        newEntity.parameters.Add(new Parameter("rtpc_return_value", new cFloat())); //float
                        break;
                    case FunctionType.SoundLevelInitialiser:
                        newEntity.parameters.Add(new Parameter("auto_generate_networks", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("network_node_min_spacing", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("network_node_max_visibility", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("network_node_ceiling_height", new cFloat())); //float
                        break;
                    case FunctionType.SoundMissionInitialiser:
                        newEntity.parameters.Add(new Parameter("human_max_threat", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("android_max_threat", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alien_max_threat", new cFloat())); //float
                        break;
                    case FunctionType.SoundRTPCController:
                        newEntity.parameters.Add(new Parameter("stealth_default_on", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("threat_default_on", new cBool())); //bool
                        break;
                    case FunctionType.SoundTimelineTrigger:
                        newEntity.parameters.Add(new Parameter("sound_event", new cEnum("SOUND_EVENT", 0))); //SOUND_EVENT
                        newEntity.parameters.Add(new Parameter("trigger_time", new cFloat())); //float
                        break;
                    case FunctionType.SoundPhysicsInitialiser:
                        newEntity.parameters.Add(new Parameter("contact_max_timeout", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("contact_smoothing_attack_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("contact_smoothing_decay_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("contact_min_magnitude", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("contact_max_trigger_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("impact_min_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("impact_max_trigger_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ragdoll_min_timeout", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ragdoll_min_speed", new cFloat())); //float
                        break;
                    case FunctionType.SoundPlayerFootwearOverride:
                        newEntity.parameters.Add(new Parameter("footwear_sound", new cEnum("SOUND_FOOTWEAR_GROUP", 0))); //SOUND_FOOTWEAR_GROUP
                        break;
                    case FunctionType.AddToInventory:
                        newEntity.parameters.Add(new Parameter("success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("items", new ParameterData())); //Object
                        break;
                    case FunctionType.RemoveFromInventory:
                        newEntity.parameters.Add(new Parameter("success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("items", new ParameterData())); //Object
                        break;
                    case FunctionType.LimitItemUse:
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("items", new ParameterData())); //Object
                        break;
                    case FunctionType.PlayerHasItem:
                        newEntity.parameters.Add(new Parameter("items", new ParameterData())); //Object
                        break;
                    case FunctionType.PlayerHasItemWithName:
                        newEntity.parameters.Add(new Parameter("item_name", new cString())); //String
                        break;
                    case FunctionType.PlayerHasItemEntity:
                        newEntity.parameters.Add(new Parameter("success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("items", new ParameterData())); //Object
                        break;
                    case FunctionType.PlayerHasEnoughItems:
                        newEntity.parameters.Add(new Parameter("items", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("quantity", new cInteger())); //int
                        break;
                    case FunctionType.PlayerHasSpaceForItem:
                        newEntity.parameters.Add(new Parameter("items", new ParameterData())); //Object
                        break;
                    case FunctionType.InventoryItem:
                        newEntity.parameters.Add(new Parameter("collect", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("itemName", new cString())); //String
                        newEntity.parameters.Add(new Parameter("out_itemName", new cString())); //String
                        newEntity.parameters.Add(new Parameter("out_quantity", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("item", new cString())); //String
                        newEntity.parameters.Add(new Parameter("quantity", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("clear_on_collect", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("gcip_instances_count", new cInteger())); //int
                        break;
                    case FunctionType.GetInventoryItemName:
                        newEntity.parameters.Add(new Parameter("item", new cEnum("INVENTORY_ITEM_QUANTITY", 0))); //INVENTORY_ITEM_QUANTITY
                        newEntity.parameters.Add(new Parameter("equippable_item", new cEnum("EQUIPPABLE_ITEM_INSTANCE", 0))); //EQUIPPABLE_ITEM_INSTANCE
                        break;
                    case FunctionType.PickupSpawner:
                        newEntity.parameters.Add(new Parameter("collect", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("spawn_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("item_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("item_quantity", new cInteger())); //int
                        break;
                    case FunctionType.MultiplePickupSpawner:
                        newEntity.parameters.Add(new Parameter("pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("item_name", new cString())); //String
                        break;
                    case FunctionType.AddItemsToGCPool:
                        newEntity.parameters.Add(new Parameter("items", new cEnum("INVENTORY_ITEM_QUANTITY", 0))); //INVENTORY_ITEM_QUANTITY
                        break;
                    case FunctionType.SetupGCDistribution:
                        newEntity.parameters.Add(new Parameter("c00", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c01", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c02", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c03", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c04", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c05", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c06", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c07", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c08", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c09", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("c10", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("minimum_multiplier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("divisor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lookup_decrease_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lookup_point_increase", new cInteger())); //int
                        break;
                    case FunctionType.AllocateGCItemsFromPool:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("items", new cEnum("INVENTORY_ITEM_QUANTITY", 0))); //INVENTORY_ITEM_QUANTITY
                        newEntity.parameters.Add(new Parameter("force_usage_count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("distribution_bias", new cFloat())); //float
                        break;
                    case FunctionType.AllocateGCItemFromPoolBySubset:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("selectable_items", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("item_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("item_quantity", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("force_usage", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("distribution_bias", new cFloat())); //float
                        break;
                    case FunctionType.QueryGCItemPool:
                        newEntity.parameters.Add(new Parameter("count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("item_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("item_quantity", new cInteger())); //int
                        break;
                    case FunctionType.RemoveFromGCItemPool:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("item_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("item_quantity", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("gcip_instances_to_remove", new cInteger())); //int
                        break;
                    case FunctionType.FlashScript:
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("filename", new cString())); //String
                        newEntity.parameters.Add(new Parameter("layer_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("target_texture_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("type", new cEnum("FLASH_SCRIPT_RENDER_TYPE", 0))); //FLASH_SCRIPT_RENDER_TYPE
                        break;
                    case FunctionType.UI_KeyGate:
                        newEntity.parameters.Add(new Parameter("keycard_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("keycode_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("keycard_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("keycode_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("keycard_cancelled", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("keycode_cancelled", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ui_breakout_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("lock_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("light_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("code", new cString())); //String
                        newEntity.parameters.Add(new Parameter("carduid", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("key_type", new cEnum("UI_KEYGATE_TYPE", 0))); //UI_KEYGATE_TYPE
                        break;
                    case FunctionType.RTT_MoviePlayer:
                        newEntity.parameters.Add(new Parameter("start", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("end", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("filename", new cString())); //String
                        newEntity.parameters.Add(new Parameter("layer_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("target_texture_name", new cString())); //String
                        break;
                    case FunctionType.MoviePlayer:
                        newEntity.parameters.Add(new Parameter("start", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("end", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("skipped", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("trigger_end_on_skipped", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("filename", new cString())); //String
                        newEntity.parameters.Add(new Parameter("skippable", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("enable_debug_skip", new cBool())); //bool
                        break;
                    case FunctionType.DurangoVideoCapture:
                        newEntity.parameters.Add(new Parameter("clip_name", new cString())); //String
                        break;
                    case FunctionType.VideoCapture:
                        newEntity.parameters.Add(new Parameter("clip_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("only_in_capture_mode", new cBool())); //bool
                        break;
                    case FunctionType.FlashInvoke:
                        newEntity.parameters.Add(new Parameter("layer_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("mrtt_texture", new cString())); //String
                        newEntity.parameters.Add(new Parameter("method", new cString())); //String
                        newEntity.parameters.Add(new Parameter("invoke_type", new cEnum("FLASH_INVOKE_TYPE", 0))); //FLASH_INVOKE_TYPE
                        newEntity.parameters.Add(new Parameter("int_argument_0", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("int_argument_1", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("int_argument_2", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("int_argument_3", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("float_argument_0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("float_argument_1", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("float_argument_2", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("float_argument_3", new cFloat())); //float
                        break;
                    case FunctionType.MotionTrackerPing:
                        newEntity.parameters.Add(new Parameter("FakePosition", new cTransform())); //Position
                        break;
                    case FunctionType.FlashCallback:
                        newEntity.parameters.Add(new Parameter("callback", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("callback_name", new cString())); //String
                        break;
                    case FunctionType.PopupMessage:
                        newEntity.parameters.Add(new Parameter("display", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("header_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("main_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("sound_event", new cEnum("POPUP_MESSAGE_SOUND", 0))); //POPUP_MESSAGE_SOUND
                        newEntity.parameters.Add(new Parameter("icon_keyframe", new cEnum("POPUP_MESSAGE_ICON", 0))); //POPUP_MESSAGE_ICON
                        break;
                    case FunctionType.UIBreathingGameIcon:
                        newEntity.parameters.Add(new Parameter("fill_percentage", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("prompt_text", new cString())); //String
                        break;
                    case FunctionType.GenericHighlightEntity:
                        newEntity.parameters.Add(new Parameter("highlight_geometry", new cEnum("RENDERABLE_INSTANCE", 0))); //RENDERABLE_INSTANCE
                        break;
                    case FunctionType.UI_Icon:
                        newEntity.parameters.Add(new Parameter("start", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("button_released", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("broadcasted_start", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("highlight", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("unhighlight", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("lock_looked_at", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("lock_interaction", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("lock_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("show_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("geometry", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("highlight_geometry", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("target_pickup_item", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("highlight_distance_threshold", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("interaction_distance_threshold", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("icon_user", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("unlocked_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("locked_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("action_text", new cString())); //String
                        newEntity.parameters.Add(new Parameter("icon_keyframe", new cEnum("UI_ICON_ICON", 0))); //UI_ICON_ICON
                        newEntity.parameters.Add(new Parameter("can_be_used", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("category", new cEnum("PICKUP_CATEGORY", 0))); //PICKUP_CATEGORY
                        newEntity.parameters.Add(new Parameter("push_hold_time", new cFloat())); //float
                        break;
                    case FunctionType.UI_Attached:
                        newEntity.parameters.Add(new Parameter("closed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ui_icon", new cInteger())); //int
                        break;
                    case FunctionType.UI_Container:
                        newEntity.parameters.Add(new Parameter("take_slot", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("emptied", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("contents", new cEnum("INVENTORY_ITEM_QUANTITY", 0))); //INVENTORY_ITEM_QUANTITY
                        newEntity.parameters.Add(new Parameter("has_been_used", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_persistent", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_temporary", new cBool())); //bool
                        break;
                    case FunctionType.UI_ReactionGame:
                        newEntity.parameters.Add(new Parameter("success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stage0_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stage0_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stage1_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stage1_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stage2_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stage2_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ui_breakout_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("resources_finished_unloading", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("resources_finished_loading", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("completion_percentage", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("exit_on_fail", new cBool())); //bool
                        break;
                    case FunctionType.UI_Keypad:
                        newEntity.parameters.Add(new Parameter("success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("code", new cString())); //String
                        newEntity.parameters.Add(new Parameter("exit_on_fail", new cBool())); //bool
                        break;
                    case FunctionType.HackingGame:
                        newEntity.parameters.Add(new Parameter("win", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("alarm_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("loaded_idle", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("loaded_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ui_breakout_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("resources_finished_unloading", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("resources_finished_loading", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("lock_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("light_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("completion_percentage", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("hacking_difficulty", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("auto_exit", new cBool())); //bool
                        break;
                    case FunctionType.SetHackingToolLevel:
                        newEntity.parameters.Add(new Parameter("level", new cInteger())); //int
                        break;
                    case FunctionType.TerminalContent:
                        newEntity.parameters.Add(new Parameter("selected", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("content_title", new cString())); //String
                        newEntity.parameters.Add(new Parameter("content_decoration_title", new cString())); //String
                        newEntity.parameters.Add(new Parameter("additional_info", new cString())); //String
                        newEntity.parameters.Add(new Parameter("is_connected_to_audio_log", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_triggerable", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_single_use", new cBool())); //bool
                        break;
                    case FunctionType.TerminalFolder:
                        newEntity.parameters.Add(new Parameter("code_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("code_fail", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("selected", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("lock_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("content0", new cEnum("TERMINAL_CONTENT_DETAILS", 0))); //TERMINAL_CONTENT_DETAILS
                        newEntity.parameters.Add(new Parameter("content1", new cEnum("TERMINAL_CONTENT_DETAILS", 0))); //TERMINAL_CONTENT_DETAILS
                        newEntity.parameters.Add(new Parameter("code", new cString())); //String
                        newEntity.parameters.Add(new Parameter("folder_title", new cString())); //String
                        newEntity.parameters.Add(new Parameter("folder_lock_type", new cEnum("FOLDER_LOCK_TYPE", 0))); //FOLDER_LOCK_TYPE
                        break;
                    case FunctionType.AccessTerminal:
                        newEntity.parameters.Add(new Parameter("closed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("all_data_has_been_read", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ui_breakout_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("light_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("folder0", new cEnum("TERMINAL_FOLDER_DETAILS", 0))); //TERMINAL_FOLDER_DETAILS
                        newEntity.parameters.Add(new Parameter("folder1", new cEnum("TERMINAL_FOLDER_DETAILS", 0))); //TERMINAL_FOLDER_DETAILS
                        newEntity.parameters.Add(new Parameter("folder2", new cEnum("TERMINAL_FOLDER_DETAILS", 0))); //TERMINAL_FOLDER_DETAILS
                        newEntity.parameters.Add(new Parameter("folder3", new cEnum("TERMINAL_FOLDER_DETAILS", 0))); //TERMINAL_FOLDER_DETAILS
                        newEntity.parameters.Add(new Parameter("all_data_read", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("location", new cEnum("TERMINAL_LOCATION", 0))); //TERMINAL_LOCATION
                        break;
                    case FunctionType.SetGatingToolLevel:
                        newEntity.parameters.Add(new Parameter("level", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("tool_type", new cEnum("GATING_TOOL_TYPE", 0))); //GATING_TOOL_TYPE
                        break;
                    case FunctionType.GetGatingToolLevel:
                        newEntity.parameters.Add(new Parameter("level", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("tool_type", new cEnum("GATING_TOOL_TYPE", 0))); //GATING_TOOL_TYPE
                        break;
                    case FunctionType.GetPlayerHasGatingTool:
                        newEntity.parameters.Add(new Parameter("has_tool", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("doesnt_have_tool", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("tool_type", new cEnum("GATING_TOOL_TYPE", 0))); //GATING_TOOL_TYPE
                        break;
                    case FunctionType.GetPlayerHasKeycard:
                        newEntity.parameters.Add(new Parameter("has_card", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("doesnt_have_card", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("card_uid", new cInteger())); //int
                        break;
                    case FunctionType.SetPlayerHasKeycard:
                        newEntity.parameters.Add(new Parameter("card_uid", new cInteger())); //int
                        break;
                    case FunctionType.SetPlayerHasGatingTool:
                        newEntity.parameters.Add(new Parameter("tool_type", new cEnum("GATING_TOOL_TYPE", 0))); //GATING_TOOL_TYPE
                        break;
                    case FunctionType.CollectSevastopolLog:
                        newEntity.parameters.Add(new Parameter("log_id", new cEnum("SEVASTOPOL_LOG_ID", 0))); //SEVASTOPOL_LOG_ID
                        break;
                    case FunctionType.CollectNostromoLog:
                        newEntity.parameters.Add(new Parameter("log_id", new cEnum("NOSTROMO_LOG_ID", 0))); //NOSTROMO_LOG_ID
                        break;
                    case FunctionType.CollectIDTag:
                        newEntity.parameters.Add(new Parameter("tag_id", new cEnum("IDTAG_ID", 0))); //IDTAG_ID
                        break;
                    case FunctionType.StartNewChapter:
                        newEntity.parameters.Add(new Parameter("chapter", new cInteger())); //int
                        break;
                    case FunctionType.UnlockLogEntry:
                        newEntity.parameters.Add(new Parameter("entry", new cInteger())); //int
                        break;
                    case FunctionType.MapAnchor:
                        newEntity.parameters.Add(new Parameter("map_north", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("map_pos", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("map_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("keyframe", new cString())); //String
                        newEntity.parameters.Add(new Parameter("keyframe1", new cString())); //String
                        newEntity.parameters.Add(new Parameter("keyframe2", new cString())); //String
                        newEntity.parameters.Add(new Parameter("keyframe3", new cString())); //String
                        newEntity.parameters.Add(new Parameter("keyframe4", new cString())); //String
                        newEntity.parameters.Add(new Parameter("keyframe5", new cString())); //String
                        newEntity.parameters.Add(new Parameter("world_pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("is_default_for_items", new cBool())); //bool
                        break;
                    case FunctionType.MapItem:
                        newEntity.parameters.Add(new Parameter("show_ui_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("item_type", new cEnum("MAP_ICON_TYPE", 0))); //MAP_ICON_TYPE
                        newEntity.parameters.Add(new Parameter("map_keyframe", new cEnum("MAP_KEYFRAME_ID", 0))); //MAP_KEYFRAME_ID
                        break;
                    case FunctionType.UnlockMapDetail:
                        newEntity.parameters.Add(new Parameter("map_keyframe", new cString())); //String
                        newEntity.parameters.Add(new Parameter("details", new cString())); //String
                        break;
                    case FunctionType.RewireSystem:
                        newEntity.parameters.Add(new Parameter("on", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("off", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("world_pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("display_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("display_name_enum", new cEnum("REWIRE_SYSTEM_NAME", 0))); //REWIRE_SYSTEM_NAME
                        newEntity.parameters.Add(new Parameter("on_by_default", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("running_cost", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("system_type", new cEnum("REWIRE_SYSTEM_TYPE", 0))); //REWIRE_SYSTEM_TYPE
                        newEntity.parameters.Add(new Parameter("map_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("element_name", new cString())); //String
                        break;
                    case FunctionType.RewireLocation:
                        newEntity.parameters.Add(new Parameter("power_draw_increased", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("power_draw_reduced", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("systems", new cEnum("REWIRE_SYSTEM", 0))); //REWIRE_SYSTEM
                        newEntity.parameters.Add(new Parameter("element_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("display_name", new cString())); //String
                        break;
                    case FunctionType.RewireAccess_Point:
                        newEntity.parameters.Add(new Parameter("closed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("ui_breakout_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("interactive_locations", new cEnum("REWIRE_LOCATION", 0))); //REWIRE_LOCATION
                        newEntity.parameters.Add(new Parameter("visible_locations", new cEnum("REWIRE_LOCATION", 0))); //REWIRE_LOCATION
                        newEntity.parameters.Add(new Parameter("additional_power", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("display_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("map_element_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("map_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("map_x_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("map_y_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("map_zoom", new cFloat())); //float
                        break;
                    case FunctionType.RewireTotalPowerResource:
                        newEntity.parameters.Add(new Parameter("total_power", new cInteger())); //int
                        break;
                    case FunctionType.Rewire:
                        newEntity.parameters.Add(new Parameter("closed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("locations", new cEnum("REWIRE_LOCATION", 0))); //REWIRE_LOCATION
                        newEntity.parameters.Add(new Parameter("access_points", new cEnum("REWIRE_ACCESS_POINT", 0))); //REWIRE_ACCESS_POINT
                        newEntity.parameters.Add(new Parameter("map_keyframe", new cString())); //String
                        newEntity.parameters.Add(new Parameter("total_power", new cInteger())); //int
                        break;
                    case FunctionType.SetMotionTrackerRange:
                        newEntity.parameters.Add(new Parameter("range", new cFloat())); //float
                        break;
                    case FunctionType.SetGamepadAxes:
                        newEntity.parameters.Add(new Parameter("invert_x", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("invert_y", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("save_settings", new cBool())); //bool
                        break;
                    case FunctionType.SetGameplayTips:
                        newEntity.parameters.Add(new Parameter("tip_string_id", new cString())); //String
                        break;
                    case FunctionType.GameOver:
                        newEntity.parameters.Add(new Parameter("tip_string_id", new cString())); //String
                        newEntity.parameters.Add(new Parameter("default_tips_enabled", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("level_tips_enabled", new cBool())); //bool
                        break;
                    case FunctionType.GameplayTip:
                        newEntity.parameters.Add(new Parameter("string_id", new cEnum("GAMEPLAY_TIP_STRING_ID", 0))); //GAMEPLAY_TIP_STRING_ID
                        break;
                    case FunctionType.Minigames:
                        newEntity.parameters.Add(new Parameter("on_success", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_failure", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("game_inertial_damping_active", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("game_green_text_active", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("game_yellow_chart_active", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("game_overloc_fail_active", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("game_docking_active", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("game_environ_ctr_active", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("config_pass_number", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("config_fail_limit", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("config_difficulty", new cInteger())); //int
                        break;
                    case FunctionType.SetBlueprintInfo:
                        newEntity.parameters.Add(new Parameter("type", new cEnum("BLUEPRINT_TYPE", 0))); //BLUEPRINT_TYPE
                        newEntity.parameters.Add(new Parameter("level", new cEnum("BLUEPRINT_LEVEL", 0))); //BLUEPRINT_LEVEL
                        newEntity.parameters.Add(new Parameter("available", new cBool())); //bool
                        break;
                    case FunctionType.GetBlueprintLevel:
                        newEntity.parameters.Add(new Parameter("level", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("type", new cEnum("BLUEPRINT_TYPE", 0))); //BLUEPRINT_TYPE
                        break;
                    case FunctionType.GetBlueprintAvailable:
                        newEntity.parameters.Add(new Parameter("available", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("type", new cEnum("BLUEPRINT_TYPE", 0))); //BLUEPRINT_TYPE
                        break;
                    case FunctionType.GetSelectedCharacterId:
                        newEntity.parameters.Add(new Parameter("character_id", new cInteger())); //int
                        break;
                    case FunctionType.GetNextPlaylistLevelName:
                        newEntity.parameters.Add(new Parameter("level_name", new cString())); //String
                        break;
                    case FunctionType.IsPlaylistTypeSingle:
                        newEntity.parameters.Add(new Parameter("single", new cBool())); //bool
                        break;
                    case FunctionType.IsPlaylistTypeAll:
                        newEntity.parameters.Add(new Parameter("all", new cBool())); //bool
                        break;
                    case FunctionType.IsPlaylistTypeMarathon:
                        newEntity.parameters.Add(new Parameter("marathon", new cBool())); //bool
                        break;
                    case FunctionType.IsCurrentLevelAChallengeMap:
                        newEntity.parameters.Add(new Parameter("challenge_map", new cBool())); //bool
                        break;
                    case FunctionType.IsCurrentLevelAPreorderMap:
                        newEntity.parameters.Add(new Parameter("preorder_map", new cBool())); //bool
                        break;
                    case FunctionType.GetCurrentPlaylistLevelIndex:
                        newEntity.parameters.Add(new Parameter("index", new cInteger())); //int
                        break;
                    case FunctionType.SetObjectiveCompleted:
                        newEntity.parameters.Add(new Parameter("objective_id", new cInteger())); //int
                        break;
                    case FunctionType.GoToFrontend:
                        newEntity.parameters.Add(new Parameter("frontend_state", new cEnum("FRONTEND_STATE", 0))); //FRONTEND_STATE
                        break;
                    case FunctionType.TriggerLooper:
                        newEntity.parameters.Add(new Parameter("target", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("delay", new cFloat())); //float
                        break;
                    case FunctionType.CoverLine:
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("low", new cBool())); //bool
                        newEntity.AddResource(ResourceType.CATHODE_COVER_SEGMENT);
                        newEntity.parameters.Add(new Parameter("LinePathPosition", new cTransform())); //Position
                        break;
                    case FunctionType.TRAV_ContinuousLadder:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("RungSpacing", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_ContinuousPipe:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_ContinuousLedge:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Dangling", new cEnum("AUTODETECT", 0))); //AUTODETECT
                        newEntity.parameters.Add(new Parameter("Sidling", new cEnum("AUTODETECT", 0))); //AUTODETECT
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_ContinuousClimbingWall:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Dangling", new cEnum("AUTODETECT", 0))); //AUTODETECT
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_ContinuousCinematicSidle:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_ContinuousBalanceBeam:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_ContinuousTightGap:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_1ShotVentEntrance:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Completed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.AddResource(ResourceType.TRAVERSAL_SEGMENT);
                        break;
                    case FunctionType.TRAV_1ShotVentExit:
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Completed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.AddResource(ResourceType.TRAVERSAL_SEGMENT);
                        break;
                    case FunctionType.TRAV_1ShotFloorVentEntrance:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Completed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.AddResource(ResourceType.TRAVERSAL_SEGMENT);
                        break;
                    case FunctionType.TRAV_1ShotFloorVentExit:
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Completed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.AddResource(ResourceType.TRAVERSAL_SEGMENT);
                        break;
                    case FunctionType.TRAV_1ShotClimbUnder:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("LinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_1ShotLeap:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnSuccess", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnFailure", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("StartEdgeLinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("EndEdgeLinePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("InUse", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("MissDistance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("NearMissDistance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.TRAV_1ShotSpline:
                        newEntity.parameters.Add(new Parameter("OnEnter", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OnExit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("enable_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("open_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("EntrancePath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("ExitPath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("MinimumPath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("MaximumPath", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("MinimumSupport", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("MaximumSupport", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("template", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("headroom", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("extra_cost", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("fit_end_to_edge", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("min_speed", new cEnum("LOCOMOTION_TARGET_SPEED", 0))); //LOCOMOTION_TARGET_SPEED
                        newEntity.parameters.Add(new Parameter("max_speed", new cEnum("LOCOMOTION_TARGET_SPEED", 0))); //LOCOMOTION_TARGET_SPEED
                        newEntity.parameters.Add(new Parameter("animationTree", new cString())); //String
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.AddResource(ResourceType.TRAVERSAL_SEGMENT);
                        break;
                    case FunctionType.NavMeshBarrier:
                        newEntity.parameters.Add(new Parameter("open_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("opaque", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("allowed_character_classes_when_open", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.parameters.Add(new Parameter("allowed_character_classes_when_closed", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        newEntity.AddResource(ResourceType.NAV_MESH_BARRIER_RESOURCE);
                        break;
                    case FunctionType.NavMeshWalkablePlatform:
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        break;
                    case FunctionType.NavMeshExclusionArea:
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        break;
                    case FunctionType.NavMeshArea:
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("area_type", new cEnum("NAV_MESH_AREA_TYPE", 0))); //NAV_MESH_AREA_TYPE
                        break;
                    case FunctionType.NavMeshReachabilitySeedPoint:
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        break;
                    case FunctionType.CoverExclusionArea:
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("exclude_cover", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("exclude_vaults", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("exclude_mantles", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("exclude_jump_downs", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("exclude_crawl_space_spotting_positions", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("exclude_spotting_positions", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("exclude_assault_positions", new cBool())); //bool
                        break;
                    case FunctionType.SpottingExclusionArea:
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("half_dimensions", new cVector3())); //Direction
                        break;
                    case FunctionType.PathfindingTeleportNode:
                        newEntity.parameters.Add(new Parameter("started_teleporting", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stopped_teleporting", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("destination", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("build_into_navmesh", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("extra_cost", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.PathfindingWaitNode:
                        newEntity.parameters.Add(new Parameter("character_getting_near", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character_arriving", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character_stopped", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("started_waiting", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stopped_waiting", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("destination", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("build_into_navmesh", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("extra_cost", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.PathfindingManualNode:
                        newEntity.parameters.Add(new Parameter("character_arriving", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character_stopped", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("started_animating", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stopped_animating", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("PlayAnimData", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("destination", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("build_into_navmesh", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("extra_cost", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("character_classes", new cEnum("CHARACTER_CLASS_COMBINATION", 0))); //CHARACTER_CLASS_COMBINATION
                        break;
                    case FunctionType.PathfindingAlienBackstageNode:
                        newEntity.parameters.Add(new Parameter("started_animating_Entry", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stopped_animating_Entry", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("started_animating_Exit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("stopped_animating_Exit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("killtrap_anim_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("killtrap_anim_stopped", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("killtrap_fx_start", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("killtrap_fx_stop", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_loaded", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("open_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("PlayAnimData_Entry", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("PlayAnimData_Exit", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("Killtrap_alien", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("Killtrap_victim", new cEnum("PLAY_ANIMATION_DATA_RESOURCE", 0))); //PLAY_ANIMATION_DATA_RESOURCE
                        newEntity.parameters.Add(new Parameter("build_into_navmesh", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("top", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("extra_cost", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("network_id", new cInteger())); //int
                        break;
                    case FunctionType.ChokePoint:
                        newEntity.AddResource(ResourceType.CHOKE_POINT_RESOURCE);
                        break;
                    case FunctionType.NPC_SetChokePoint:
                        newEntity.parameters.Add(new Parameter("chokepoints", new cEnum("CHOKE_POINT_RESOURCE", 0))); //CHOKE_POINT_RESOURCE
                        break;
                    case FunctionType.Planet:
                        newEntity.parameters.Add(new Parameter("planet_resource", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("parallax_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("sun_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("light_shaft_source_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("parallax_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("planet_sort_key", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("overbright_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("light_wrap_angle_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("penumbra_falloff_power_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lens_flare_brightness", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lens_flare_colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("atmosphere_edge_falloff_power", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("atmosphere_edge_transparency", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("atmosphere_scroll_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("atmosphere_detail_scroll_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("override_global_tint", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("global_tint", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("flow_cycle_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flow_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flow_tex_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flow_warp_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("detail_uv_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("normal_uv_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("terrain_uv_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("atmosphere_normal_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("terrain_normal_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("light_shaft_colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("light_shaft_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("light_shaft_decay", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("light_shaft_min_occlusion_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("light_shaft_intensity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("light_shaft_density", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("light_shaft_source_occlusion", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("blocks_light_shafts", new cBool())); //bool
                        break;
                    case FunctionType.SpaceTransform:
                        newEntity.parameters.Add(new Parameter("affected_geometry", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("yaw_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("pitch_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("roll_speed", new cFloat())); //float
                        break;
                    case FunctionType.SpaceSuitVisor:
                        newEntity.parameters.Add(new Parameter("breath_level", new cFloat())); //float
                        break;
                    case FunctionType.NonInteractiveWater:
                        newEntity.parameters.Add(new Parameter("water_resource", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("SCALE_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SCALE_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SHININESS", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("NORMAL_MAP_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_NORMAL_MAP_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CYCLE_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_TEX_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_WARP_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FRESNEL_POWER", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MIN_FRESNEL", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MAX_FRESNEL", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ENVIRONMENT_MAP_MULT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ENVMAP_SIZE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ENVMAP_BOXPROJ_BB_SCALE", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("REFLECTION_PERTURBATION_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHA_PERTURBATION_STRENGTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ALPHALIGHT_MULT", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("softness_edge", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_INITIAL_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_INITIAL_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_MIDPOINT_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_MIDPOINT_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_MIDPOINT_DEPTH", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_END_COLOUR", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_END_ALPHA", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DEPTH_FOG_END_DEPTH", new cFloat())); //float
                        break;
                    case FunctionType.Refraction:
                        newEntity.parameters.Add(new Parameter("refraction_resource", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("SCALE_X", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SCALE_Z", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DISTANCEFACTOR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("REFRACTFACTOR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_REFRACTFACTOR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("SECONDARY_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("MIN_OCCLUSION_DISTANCE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("CYCLE_TIME", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_SPEED", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_TEX_SCALE", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("FLOW_WARP_STRENGTH", new cFloat())); //float
                        break;
                    case FunctionType.FogPlane:
                        newEntity.parameters.Add(new Parameter("fog_plane_resource", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("start_distance_fade_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("distance_fade_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("angle_fade_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("linear_height_density_fresnel_power_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("linear_heigth_density_max_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("tint", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("thickness_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("edge_softness_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("diffuse_0_uv_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("diffuse_0_speed_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("diffuse_1_uv_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("diffuse_1_speed_scalar", new cFloat())); //float
                        break;
                    case FunctionType.PostprocessingSettings:
                        newEntity.parameters.Add(new Parameter("intensity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("priority", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("blend_mode", new cEnum("BLEND_MODE", 0))); //BLEND_MODE
                        break;
                    case FunctionType.BloomSettings:
                        newEntity.parameters.Add(new Parameter("frame_buffer_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("frame_buffer_offset", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("bloom_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("bloom_gather_exponent", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("bloom_gather_scale", new cFloat())); //float
                        break;
                    case FunctionType.ColourSettings:
                        newEntity.parameters.Add(new Parameter("brightness", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("contrast", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("saturation", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("red_tint", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("green_tint", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("blue_tint", new cFloat())); //float
                        break;
                    case FunctionType.FlareSettings:
                        newEntity.parameters.Add(new Parameter("flareOffset0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareIntensity0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareAttenuation0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareOffset1", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareIntensity1", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareAttenuation1", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareOffset2", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareIntensity2", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareAttenuation2", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareOffset3", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareIntensity3", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("flareAttenuation3", new cFloat())); //float
                        break;
                    case FunctionType.HighSpecMotionBlurSettings:
                        newEntity.parameters.Add(new Parameter("contribution", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("camera_velocity_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("camera_velocity_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("camera_velocity_max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("object_velocity_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("object_velocity_min", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("object_velocity_max", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("blur_range", new cFloat())); //float
                        break;
                    case FunctionType.FilmGrainSettings:
                        newEntity.parameters.Add(new Parameter("low_lum_amplifier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("mid_lum_amplifier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("high_lum_amplifier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("low_lum_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("mid_lum_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("high_lum_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("noise_texture_scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("adaptive", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("adaptation_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("adaptation_time_scalar", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("unadapted_low_lum_amplifier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("unadapted_mid_lum_amplifier", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("unadapted_high_lum_amplifier", new cFloat())); //float
                        break;
                    case FunctionType.VignetteSettings:
                        newEntity.parameters.Add(new Parameter("vignette_factor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("vignette_chromatic_aberration_scale", new cFloat())); //float
                        break;
                    case FunctionType.DistortionSettings:
                        newEntity.parameters.Add(new Parameter("radial_distort_factor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("radial_distort_constraint", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("radial_distort_scalar", new cFloat())); //float
                        break;
                    case FunctionType.SharpnessSettings:
                        newEntity.parameters.Add(new Parameter("local_contrast_factor", new cFloat())); //float
                        break;
                    case FunctionType.LensDustSettings:
                        newEntity.parameters.Add(new Parameter("DUST_MAX_REFLECTED_BLOOM_INTENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DUST_REFLECTED_BLOOM_INTENSITY_SCALAR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DUST_MAX_BLOOM_INTENSITY", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DUST_BLOOM_INTENSITY_SCALAR", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("DUST_THRESHOLD", new cFloat())); //float
                        break;
                    case FunctionType.IrawanToneMappingSettings:
                        newEntity.parameters.Add(new Parameter("target_device_luminance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("target_device_adaptation", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("saccadic_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("superbright_adaptation", new cFloat())); //float
                        break;
                    case FunctionType.HableToneMappingSettings:
                        newEntity.parameters.Add(new Parameter("shoulder_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("linear_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("linear_angle", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("toe_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("toe_numerator", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("toe_denominator", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("linear_white_point", new cFloat())); //float
                        break;
                    case FunctionType.DayToneMappingSettings:
                        newEntity.parameters.Add(new Parameter("black_point", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("cross_over_point", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("white_point", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("shoulder_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("toe_strength", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("luminance_scale", new cFloat())); //float
                        break;
                    case FunctionType.LightAdaptationSettings:
                        newEntity.parameters.Add(new Parameter("fast_neural_t0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("slow_neural_t0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("pigment_bleaching_t0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("fb_luminance_to_candelas_per_m2", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_adaptation_lum", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("min_adaptation_lum", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("adaptation_percentile", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("low_bracket", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("high_bracket", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("adaptation_mechanism", new cEnum("LIGHT_ADAPTATION_MECHANISM", 0))); //LIGHT_ADAPTATION_MECHANISM
                        break;
                    case FunctionType.ColourCorrectionTransition:
                        newEntity.parameters.Add(new Parameter("interpolate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("colour_lut_a", new cString())); //String
                        newEntity.parameters.Add(new Parameter("colour_lut_b", new cString())); //String
                        newEntity.parameters.Add(new Parameter("lut_a_contribution", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("lut_b_contribution", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("colour_lut_a_index", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("colour_lut_b_index", new cInteger())); //int
                        break;
                    case FunctionType.ProjectileMotion:
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_pos", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("start_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Current_Position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Current_Velocity", new cVector3())); //Direction
                        break;
                    case FunctionType.ProjectileMotionComplex:
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("start_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("start_angular_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("flight_time_in_seconds", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("current_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("current_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("current_angular_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("current_flight_time_in_seconds", new cFloat())); //float
                        break;
                    case FunctionType.SplineDistanceLerp:
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("spline", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("lerp_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.MoveAlongSpline:
                        newEntity.parameters.Add(new Parameter("on_think", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("spline", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cTransform())); //Position
                        break;
                    case FunctionType.GetSplineLength:
                        newEntity.parameters.Add(new Parameter("spline", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        break;
                    case FunctionType.GetPointOnSpline:
                        newEntity.parameters.Add(new Parameter("spline", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("percentage_of_spline", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Result", new cTransform())); //Position
                        break;
                    case FunctionType.GetClosestPercentOnSpline:
                        newEntity.parameters.Add(new Parameter("spline", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("pos_to_be_near", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("position_on_spline", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Result", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("bidirectional", new cBool())); //bool
                        break;
                    case FunctionType.GetClosestPointOnSpline:
                        newEntity.parameters.Add(new Parameter("spline", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("pos_to_be_near", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("position_on_spline", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("look_ahead_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("unidirectional", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("directional_damping_threshold", new cFloat())); //float
                        break;
                    case FunctionType.GetClosestPoint:
                        newEntity.parameters.Add(new Parameter("bound_to_closest", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Positions", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("pos_to_be_near", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("position_of_closest", new cTransform())); //Position
                        break;
                    case FunctionType.GetClosestPointFromSet:
                        newEntity.parameters.Add(new Parameter("closest_is_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("closest_is_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Position_1", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_2", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_3", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_4", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_5", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_6", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_7", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_8", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_9", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Position_10", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("pos_to_be_near", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("position_of_closest", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("index_of_closest", new cInteger())); //int
                        break;
                    case FunctionType.GetCentrePoint:
                        newEntity.parameters.Add(new Parameter("Positions", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("position_of_centre", new cTransform())); //Position
                        break;
                    case FunctionType.FogSetting:
                        newEntity.parameters.Add(new Parameter("linear_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("linear_density", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("exponential_density", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("near_colour", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("far_colour", new cVector3())); //Direction
                        break;
                    case FunctionType.FullScreenBlurSettings:
                        newEntity.parameters.Add(new Parameter("contribution", new cFloat())); //float
                        break;
                    case FunctionType.DistortionOverlay:
                        newEntity.parameters.Add(new Parameter("intensity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("distortion_texture", new cString())); //String
                        newEntity.parameters.Add(new Parameter("alpha_threshold_enabled", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("threshold_texture", new cString())); //String
                        newEntity.parameters.Add(new Parameter("range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("begin_start_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("begin_stop_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("end_start_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("end_stop_time", new cFloat())); //float
                        break;
                    case FunctionType.FullScreenOverlay:
                        newEntity.parameters.Add(new Parameter("overlay_texture", new cString())); //String
                        newEntity.parameters.Add(new Parameter("threshold_value", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("threshold_start", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("threshold_stop", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("threshold_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("alpha_scalar", new cFloat())); //float
                        break;
                    case FunctionType.DepthOfFieldSettings:
                        newEntity.parameters.Add(new Parameter("focal_length_mm", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("focal_plane_m", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("fnum", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("focal_point", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("use_camera_target", new cBool())); //bool
                        break;
                    case FunctionType.ChromaticAberrations:
                        newEntity.parameters.Add(new Parameter("aberration_scalar", new cFloat())); //float
                        break;
                    case FunctionType.ScreenFadeOutToBlack:
                        newEntity.parameters.Add(new Parameter("fade_value", new cFloat())); //float
                        break;
                    case FunctionType.ScreenFadeOutToBlackTimed:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("time", new cFloat())); //float
                        break;
                    case FunctionType.ScreenFadeOutToWhite:
                        newEntity.parameters.Add(new Parameter("fade_value", new cFloat())); //float
                        break;
                    case FunctionType.ScreenFadeOutToWhiteTimed:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("time", new cFloat())); //float
                        break;
                    case FunctionType.ScreenFadeIn:
                        newEntity.parameters.Add(new Parameter("fade_value", new cFloat())); //float
                        break;
                    case FunctionType.ScreenFadeInTimed:
                        newEntity.parameters.Add(new Parameter("on_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("time", new cFloat())); //float
                        break;
                    case FunctionType.BlendLowResFrame:
                        newEntity.parameters.Add(new Parameter("blend_value", new cFloat())); //float
                        break;
                    case FunctionType.CharacterMonitor:
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //ResourceID
                        break;
                    case FunctionType.AreaHitMonitor:
                        newEntity.parameters.Add(new Parameter("on_flamer_hit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_shotgun_hit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_pistol_hit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("SpherePos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("SphereRadius", new cFloat())); //float
                        break;
                    case FunctionType.ENT_Debug_Exit_Game:
                        newEntity.parameters.Add(new Parameter("FailureText", new cString())); //String
                        newEntity.parameters.Add(new Parameter("FailureCode", new cInteger())); //int
                        break;
                    case FunctionType.StreamingMonitor:
                        newEntity.parameters.Add(new Parameter("on_loaded", new ParameterData())); //
                        break;
                    case FunctionType.Raycast:
                        newEntity.parameters.Add(new Parameter("Obstructed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Unobstructed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("OutOfRange", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("source_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("target_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("max_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("hit_object", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("hit_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("hit_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("priority", new cEnum("RAYCAST_PRIORITY", 0))); //RAYCAST_PRIORITY
                        break;
                    case FunctionType.PhysicsApplyImpulse:
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("direction", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("force", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("can_damage", new cBool())); //bool
                        break;
                    case FunctionType.PhysicsApplyVelocity:
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("angular_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("linear_velocity", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("propulsion_velocity", new cFloat())); //float
                        break;
                    case FunctionType.PhysicsModifyGravity:
                        newEntity.parameters.Add(new Parameter("float_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        break;
                    case FunctionType.PhysicsApplyBuoyancy:
                        newEntity.parameters.Add(new Parameter("objects", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("water_height", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("water_density", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("water_viscosity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("water_choppiness", new cFloat())); //float
                        break;
                    case FunctionType.AssetSpawner:
                        newEntity.parameters.Add(new Parameter("finished_spawning", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("callback_triggered", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("forced_despawn", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("spawn_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("asset", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("spawn_on_load", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("allow_forced_despawn", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("persist_on_callback", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("allow_physics", new cBool())); //bool
                        break;
                    case FunctionType.ProximityTrigger:
                        newEntity.parameters.Add(new Parameter("ignited", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("electrified", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("drenched", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("poisoned", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("fire_spread_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("water_permeate_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("electrical_conduction_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("gas_diffusion_rate", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("ignition_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("electrical_arc_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("water_flow_range", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("gas_dispersion_range", new cFloat())); //float
                        break;
                    case FunctionType.CharacterAttachmentNode:
                        newEntity.parameters.Add(new Parameter("attach_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //CHARACTER
                        newEntity.parameters.Add(new Parameter("attachment", new ParameterData())); //ReferenceFramePtr
                        newEntity.parameters.Add(new Parameter("Node", new cEnum("CHARACTER_NODE", 0))); //CHARACTER_NODE
                        newEntity.parameters.Add(new Parameter("AdditiveNode", new cEnum("CHARACTER_NODE", 0))); //CHARACTER_NODE
                        newEntity.parameters.Add(new Parameter("AdditiveNodeIntensity", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("UseOffset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Translation", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("Rotation", new cVector3())); //Direction
                        break;
                    case FunctionType.MultipleCharacterAttachmentNode:
                        newEntity.parameters.Add(new Parameter("attach_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_01", new ParameterData())); //CHARACTER
                        newEntity.parameters.Add(new Parameter("attachment_01", new ParameterData())); //ReferenceFramePtr
                        newEntity.parameters.Add(new Parameter("character_02", new ParameterData())); //CHARACTER
                        newEntity.parameters.Add(new Parameter("attachment_02", new ParameterData())); //ReferenceFramePtr
                        newEntity.parameters.Add(new Parameter("character_03", new ParameterData())); //CHARACTER
                        newEntity.parameters.Add(new Parameter("attachment_03", new ParameterData())); //ReferenceFramePtr
                        newEntity.parameters.Add(new Parameter("character_04", new ParameterData())); //CHARACTER
                        newEntity.parameters.Add(new Parameter("attachment_04", new ParameterData())); //ReferenceFramePtr
                        newEntity.parameters.Add(new Parameter("character_05", new ParameterData())); //CHARACTER
                        newEntity.parameters.Add(new Parameter("attachment_05", new ParameterData())); //ReferenceFramePtr
                        newEntity.parameters.Add(new Parameter("node", new cEnum("CHARACTER_NODE", 0))); //CHARACTER_NODE
                        newEntity.parameters.Add(new Parameter("use_offset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("translation", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("rotation", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("is_cinematic", new cBool())); //bool
                        break;
                    case FunctionType.AnimatedModelAttachmentNode:
                        newEntity.parameters.Add(new Parameter("attach_on_reset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("animated_model", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("attachment", new ParameterData())); //ReferenceFramePtr
                        newEntity.parameters.Add(new Parameter("bone_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("use_offset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("offset", new cTransform())); //Position
                        break;
                    case FunctionType.GetCharacterRotationSpeed:
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //CHARACTER
                        newEntity.parameters.Add(new Parameter("speed", new cFloat())); //float
                        break;
                    case FunctionType.LevelCompletionTargets:
                        newEntity.parameters.Add(new Parameter("TargetTime", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("NumDeaths", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("TeamRespawnBonus", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("NoLocalRespawnBonus", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("NoRespawnBonus", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("GrappleBreakBonus", new cInteger())); //int
                        break;
                    case FunctionType.EnvironmentMap:
                        newEntity.parameters.Add(new Parameter("Entities", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Priority", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("ColourFactor", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("EmissiveFactor", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("Texture", new cString())); //String
                        newEntity.parameters.Add(new Parameter("Texture_Index", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("environmentmap_index", new cInteger())); //int
                        break;
                    case FunctionType.Display_Element_On_Map:
                        newEntity.parameters.Add(new Parameter("map_name", new cString())); //String
                        newEntity.parameters.Add(new Parameter("element_name", new cString())); //String
                        break;
                    case FunctionType.Map_Floor_Change:
                        newEntity.parameters.Add(new Parameter("floor_name", new cString())); //String
                        break;
                    case FunctionType.Force_UI_Visibility:
                        newEntity.parameters.Add(new Parameter("also_disable_interactions", new cBool())); //bool
                        break;
                    case FunctionType.AddExitObjective:
                        newEntity.parameters.Add(new Parameter("marker", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("level_name", new cEnum("EXIT_WAYPOINT", 0))); //EXIT_WAYPOINT
                        break;
                    case FunctionType.SetPrimaryObjective:
                        newEntity.parameters.Add(new Parameter("title", new cString())); //String
                        newEntity.parameters.Add(new Parameter("additional_info", new cString())); //String
                        newEntity.parameters.Add(new Parameter("title_list", new cEnum("OBJECTIVE_ENTRY_ID", 0))); //OBJECTIVE_ENTRY_ID
                        newEntity.parameters.Add(new Parameter("additional_info_list", new cEnum("OBJECTIVE_ENTRY_ID", 0))); //OBJECTIVE_ENTRY_ID
                        newEntity.parameters.Add(new Parameter("show_message", new cBool())); //bool
                        break;
                    case FunctionType.SetSubObjective:
                        newEntity.parameters.Add(new Parameter("target_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("title", new cString())); //String
                        newEntity.parameters.Add(new Parameter("map_description", new cString())); //String
                        newEntity.parameters.Add(new Parameter("title_list", new cEnum("OBJECTIVE_ENTRY_ID", 0))); //OBJECTIVE_ENTRY_ID
                        newEntity.parameters.Add(new Parameter("map_description_list", new cEnum("OBJECTIVE_ENTRY_ID", 0))); //OBJECTIVE_ENTRY_ID
                        newEntity.parameters.Add(new Parameter("slot_number", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("objective_type", new cEnum("SUB_OBJECTIVE_TYPE", 0))); //SUB_OBJECTIVE_TYPE
                        newEntity.parameters.Add(new Parameter("show_message", new cBool())); //bool
                        break;
                    case FunctionType.ClearPrimaryObjective:
                        newEntity.parameters.Add(new Parameter("clear_all_sub_objectives", new cBool())); //bool
                        break;
                    case FunctionType.ClearSubObjective:
                        newEntity.parameters.Add(new Parameter("slot_number", new cInteger())); //int
                        break;
                    case FunctionType.UpdatePrimaryObjective:
                        newEntity.parameters.Add(new Parameter("show_message", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("clear_objective", new cBool())); //bool
                        break;
                    case FunctionType.UpdateSubObjective:
                        newEntity.parameters.Add(new Parameter("slot_number", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("show_message", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("clear_objective", new cBool())); //bool
                        break;
                    case FunctionType.DebugGraph:
                        newEntity.parameters.Add(new Parameter("Inputs", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("scale", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("samples_per_second", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("auto_scale", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("auto_scroll", new cBool())); //bool
                        break;
                    case FunctionType.UnlockAchievement:
                        newEntity.parameters.Add(new Parameter("achievement_id", new cEnum("ACHIEVEMENT_ID", 0))); //ACHIEVEMENT_ID
                        break;
                    case FunctionType.AchievementMonitor:
                        newEntity.parameters.Add(new Parameter("achievement_id", new cEnum("ACHIEVEMENT_ID", 0))); //ACHIEVEMENT_ID
                        break;
                    case FunctionType.AchievementStat:
                        newEntity.parameters.Add(new Parameter("achievement_id", new cEnum("ACHIEVEMENT_STAT_ID", 0))); //ACHIEVEMENT_STAT_ID
                        break;
                    case FunctionType.AchievementUniqueCounter:
                        newEntity.parameters.Add(new Parameter("achievement_id", new cEnum("ACHIEVEMENT_STAT_ID", 0))); //ACHIEVEMENT_STAT_ID
                        newEntity.parameters.Add(new Parameter("unique_object", new ParameterData())); //Object
                        break;
                    case FunctionType.SetRichPresence:
                        newEntity.parameters.Add(new Parameter("presence_id", new cEnum("PRESENCE_ID", 0))); //PRESENCE_ID
                        newEntity.parameters.Add(new Parameter("mission_number", new cFloat())); //float
                        break;
                    case FunctionType.SmokeCylinder:
                        newEntity.parameters.Add(new Parameter("pos", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("height", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        break;
                    case FunctionType.SmokeCylinderAttachmentInterface:
                        newEntity.parameters.Add(new Parameter("radius", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("height", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        break;
                    case FunctionType.PointTracker:
                        newEntity.parameters.Add(new Parameter("origin", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("target", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("target_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("result", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("origin_offset", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("max_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("damping_factor", new cFloat())); //float
                        break;
                    case FunctionType.ThrowingPointOfImpact:
                        newEntity.parameters.Add(new Parameter("show_point_of_impact", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("hide_point_of_impact", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Location", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("Visible", new cBool())); //bool
                        break;
                    case FunctionType.VisibilityMaster:
                        newEntity.parameters.Add(new Parameter("renderable", new cEnum("RENDERABLE_INSTANCE", 0))); //RENDERABLE_INSTANCE
                        newEntity.parameters.Add(new Parameter("mastered_by_visibility", new ParameterData())); //Object
                        break;
                    case FunctionType.MotionTrackerMonitor:
                        newEntity.parameters.Add(new Parameter("on_motion_sound", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_enter_range_sound", new ParameterData())); //
                        break;
                    case FunctionType.GlobalEvent:
                        newEntity.parameters.Add(new Parameter("EventValue", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("EventName", new cString())); //String
                        break;
                    case FunctionType.GlobalEventMonitor:
                        newEntity.parameters.Add(new Parameter("Event_1", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_2", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_3", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_4", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_5", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_6", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_7", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_8", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_9", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_10", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_11", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_12", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_13", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_14", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_15", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_16", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_17", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_18", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_19", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Event_20", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("EventName", new cString())); //String
                        break;
                    case FunctionType.GlobalPosition:
                        newEntity.parameters.Add(new Parameter("PositionName", new cString())); //String
                        break;
                    case FunctionType.UpdateGlobalPosition:
                        newEntity.parameters.Add(new Parameter("PositionName", new cString())); //String
                        break;
                    case FunctionType.PlayerLightProbe:
                        newEntity.parameters.Add(new Parameter("output", new cVector3())); //Direction
                        newEntity.parameters.Add(new Parameter("light_level_for_ai", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("dark_threshold", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("fully_lit_threshold", new cFloat())); //float
                        break;
                    case FunctionType.PlayerKilledAllyMonitor:
                        newEntity.parameters.Add(new Parameter("ally_killed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("start_on_reset", new cBool())); //bool
                        break;
                    case FunctionType.AILightCurveSettings:
                        newEntity.parameters.Add(new Parameter("y0", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("x1", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("y1", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("x2", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("y2", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("x3", new cFloat())); //float
                        break;
                    case FunctionType.InteractiveMovementControl:
                        newEntity.parameters.Add(new Parameter("completed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("duration", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("start_time", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("progress_path", new cSpline())); //SPLINE
                        newEntity.parameters.Add(new Parameter("result", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("can_go_both_ways", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("use_left_input_stick", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("base_progress_speed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("movement_threshold", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("momentum_damping", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("track_bone_position", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("character_node", new cEnum("CHARACTER_NODE", 0))); //CHARACTER_NODE
                        newEntity.parameters.Add(new Parameter("track_position", new cTransform())); //Position
                        break;
                    case FunctionType.PlayForMinDuration:
                        newEntity.parameters.Add(new Parameter("timer_expired", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("first_animation_started", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("next_animation", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("all_animations_finished", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("MinDuration", new cFloat())); //float
                        break;
                    case FunctionType.GCIP_WorldPickup:
                        newEntity.parameters.Add(new Parameter("spawn_completed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("pickup_collected", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("Pipe", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Gasoline", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Explosive", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Battery", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Blade", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Gel", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Adhesive", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("BoltGun Ammo", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Revolver Ammo", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Shotgun Ammo", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("BoltGun", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Revolver", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Shotgun", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Flare", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Flamer Fuel", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Flamer", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Scrap", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Torch Battery", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Torch", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Cattleprod Ammo", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("Cattleprod", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("StartOnReset", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("MissionNumber", new cFloat())); //float
                        break;
                    case FunctionType.Torch_Control:
                        newEntity.parameters.Add(new Parameter("torch_switched_off", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("torch_switched_on", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("character", new ParameterData())); //CHARACTER
                        break;
                    case FunctionType.DoorStatus:
                        newEntity.parameters.Add(new Parameter("hacking_difficulty", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        newEntity.parameters.Add(new Parameter("gate_type", new cEnum("UI_KEYGATE_TYPE", 0))); //UI_KEYGATE_TYPE
                        newEntity.parameters.Add(new Parameter("has_correct_keycard", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("cutting_tool_level", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("is_locked", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_powered", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("is_cutting_complete", new cBool())); //bool
                        break;
                    case FunctionType.DeleteHacking:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        break;
                    case FunctionType.DeleteKeypad:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        break;
                    case FunctionType.DeleteCuttingPanel:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        break;
                    case FunctionType.DeleteBlankPanel:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        break;
                    case FunctionType.DeleteHousing:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        newEntity.parameters.Add(new Parameter("is_door", new cBool())); //bool
                        break;
                    case FunctionType.DeletePullLever:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        newEntity.parameters.Add(new Parameter("lever_type", new cEnum("LEVER_TYPE", 0))); //LEVER_TYPE
                        break;
                    case FunctionType.DeleteRotateLever:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        newEntity.parameters.Add(new Parameter("lever_type", new cEnum("LEVER_TYPE", 0))); //LEVER_TYPE
                        break;
                    case FunctionType.DeleteButtonDisk:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        newEntity.parameters.Add(new Parameter("button_type", new cEnum("BUTTON_TYPE", 0))); //BUTTON_TYPE
                        break;
                    case FunctionType.DeleteButtonKeys:
                        newEntity.parameters.Add(new Parameter("door_mechanism", new cEnum("DOOR_MECHANISM", 0))); //DOOR_MECHANISM
                        newEntity.parameters.Add(new Parameter("button_type", new cEnum("BUTTON_TYPE", 0))); //BUTTON_TYPE
                        break;
                    case FunctionType.Interaction:
                        newEntity.parameters.Add(new Parameter("on_damaged", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_interrupt", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("on_killed", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("interruptible_on_start", new cBool())); //bool
                        break;
                    case FunctionType.PhysicsSystem:
                        newEntity.parameters.Add(new Parameter("system_index", new cInteger())); //int
                        newEntity.AddResource(ResourceType.DYNAMIC_PHYSICS_SYSTEM).startIndex = 0;
                        break;
                    case FunctionType.BulletChamber:
                        newEntity.parameters.Add(new Parameter("Slot1", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Slot2", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Slot3", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Slot4", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Slot5", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Slot6", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Weapon", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("Geometry", new ParameterData())); //Object
                        break;
                    case FunctionType.PlayerDeathCounter:
                        newEntity.parameters.Add(new Parameter("on_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("above_limit", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("count", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("Limit", new cInteger())); //int
                        break;
                    case FunctionType.RadiosityIsland:
                        newEntity.parameters.Add(new Parameter("composites", new ParameterData())); //Object
                        newEntity.parameters.Add(new Parameter("exclusions", new ParameterData())); //Object
                        break;
                    case FunctionType.RadiosityProxy:
                        newEntity.parameters.Add(new Parameter("position", new cTransform())); //Position
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                    case FunctionType.LeaderboardWriter:
                        newEntity.parameters.Add(new Parameter("time_elapsed", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("score", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("level_number", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("grade", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("player_character", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("combat", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("stealth", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("improv", new cInteger())); //int
                        newEntity.parameters.Add(new Parameter("star1", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("star2", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("star3", new cBool())); //bool
                        break;
                    case FunctionType.ProximityDetector:
                        newEntity.parameters.Add(new Parameter("in_proximity", new ParameterData())); //
                        newEntity.parameters.Add(new Parameter("filter", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("detector_position", new cTransform())); //Position
                        newEntity.parameters.Add(new Parameter("min_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("max_distance", new cFloat())); //float
                        newEntity.parameters.Add(new Parameter("requires_line_of_sight", new cBool())); //bool
                        newEntity.parameters.Add(new Parameter("proximity_duration", new cFloat())); //float
                        break;
                    case FunctionType.FakeAILightSourceInPlayersHand:
                        newEntity.parameters.Add(new Parameter("radius", new cFloat())); //float
                        break;
#endregion
                }
                //TODO: figure out the above NONE types (I think I need to implement link-only params)
                newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.NONE); 
                //TODO: adding position here as it should come up from base class i think, but i don't know base classes yet
                if (newEntity.parameters.FirstOrDefault(o => o.shortGUID == ShortGuidUtils.Generate("position")) == null)
                    newEntity.parameters.Add(new Parameter("position", new cTransform()));

                newEntity.function = function;

                //Add to composite & save name
                composite.functions.Add(newEntity);
                CurrentInstance.compositeLookup.SetName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createCompositeEntity.Checked)
            {
                //Create FunctionEntity
                FunctionEntity newEntity = new FunctionEntity(thisID);
                Composite composite = composites.FirstOrDefault(o => o.name == entityVariant.Text);
                if (composite == null)
                { 
                    MessageBox.Show("Failed to look up composite!\nPlease report this issue on GitHub.\n\n" + entityVariant.Text, "Could not find composite!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newEntity.parameters.Add(new Parameter("position", new cTransform()));
                newEntity.function = composite.shortGUID;

                //Add to composite & save name
                this.composite.functions.Add(newEntity);
                CurrentInstance.compositeLookup.SetName(this.composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createProxyEntity.Checked)
            {
                //Create ProxyEntity
                ProxyEntity newEntity = new ProxyEntity(thisID);
                newEntity.hierarchy = hierarchy;
                newEntity.extraId = ShortGuidUtils.Generate(DateTime.Now.ToString("G") + "temp");
                newEntity.parameters.Add(new Parameter("proxy_filter_targets", new cBool(false)));
                newEntity.parameters.Add(new Parameter("proxy_enable_on_reset", new cBool(false)));
                newEntity.parameters.Add(new Parameter("proxy_enable", new cFloat(0.0f)));
                newEntity.parameters.Add(new Parameter("proxy_enabled", new cFloat(0.0f)));
                newEntity.parameters.Add(new Parameter("proxy_disable", new cFloat(0.0f)));
                newEntity.parameters.Add(new Parameter("proxy_disabled", new cFloat(0.0f)));
                newEntity.parameters.Add(new Parameter("reference", new cString("")));
                newEntity.parameters.Add(new Parameter("trigger", new cFloat(0.0f)));

                //Add to composite & save name
                composite.proxies.Add(newEntity);
                CurrentInstance.compositeLookup.SetName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createOverrideEntity.Checked)
            {
                //Create OverrideEntity
                OverrideEntity newEntity = new OverrideEntity(thisID);
                newEntity.hierarchy = hierarchy;
                newEntity.checksum = ShortGuidUtils.Generate("temp"); //TODO: how do we generate this? without it, i think overrides won't work.

                //Add to composite & save name
                composite.overrides.Add(newEntity);
                CurrentInstance.compositeLookup.SetName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            this.Close();
        }
    }
}
