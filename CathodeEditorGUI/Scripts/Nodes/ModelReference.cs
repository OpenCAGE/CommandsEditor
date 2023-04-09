using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ModelReference : STNode
	{
		private bool _m_show_on_reset;
		[STNodeProperty("show_on_reset", "show_on_reset")]
		public bool m_show_on_reset
		{
			get { return _m_show_on_reset; }
			set { _m_show_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_simulate_on_reset;
		[STNodeProperty("simulate_on_reset", "simulate_on_reset")]
		public bool m_simulate_on_reset
		{
			get { return _m_simulate_on_reset; }
			set { _m_simulate_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_light_on_reset;
		[STNodeProperty("light_on_reset", "light_on_reset")]
		public bool m_light_on_reset
		{
			get { return _m_light_on_reset; }
			set { _m_light_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_convert_to_physics;
		[STNodeProperty("convert_to_physics", "convert_to_physics")]
		public bool m_convert_to_physics
		{
			get { return _m_convert_to_physics; }
			set { _m_convert_to_physics = value; this.Invalidate(); }
		}
		
		private string _m_material;
		[STNodeProperty("material", "material")]
		public string m_material
		{
			get { return _m_material; }
			set { _m_material = value; this.Invalidate(); }
		}
		
		private bool _m_occludes_atmosphere;
		[STNodeProperty("occludes_atmosphere", "occludes_atmosphere")]
		public bool m_occludes_atmosphere
		{
			get { return _m_occludes_atmosphere; }
			set { _m_occludes_atmosphere = value; this.Invalidate(); }
		}
		
		private bool _m_include_in_planar_reflections;
		[STNodeProperty("include_in_planar_reflections", "include_in_planar_reflections")]
		public bool m_include_in_planar_reflections
		{
			get { return _m_include_in_planar_reflections; }
			set { _m_include_in_planar_reflections = value; this.Invalidate(); }
		}
		
		private string _m_lod_ranges;
		[STNodeProperty("lod_ranges", "lod_ranges")]
		public string m_lod_ranges
		{
			get { return _m_lod_ranges; }
			set { _m_lod_ranges = value; this.Invalidate(); }
		}
		
		private float _m_intensity_multiplier;
		[STNodeProperty("intensity_multiplier", "intensity_multiplier")]
		public float m_intensity_multiplier
		{
			get { return _m_intensity_multiplier; }
			set { _m_intensity_multiplier = value; this.Invalidate(); }
		}
		
		private float _m_radiosity_multiplier;
		[STNodeProperty("radiosity_multiplier", "radiosity_multiplier")]
		public float m_radiosity_multiplier
		{
			get { return _m_radiosity_multiplier; }
			set { _m_radiosity_multiplier = value; this.Invalidate(); }
		}
		
		private cVector3 _m_emissive_tint;
		[STNodeProperty("emissive_tint", "emissive_tint")]
		public cVector3 m_emissive_tint
		{
			get { return _m_emissive_tint; }
			set { _m_emissive_tint = value; this.Invalidate(); }
		}
		
		private bool _m_replace_intensity;
		[STNodeProperty("replace_intensity", "replace_intensity")]
		public bool m_replace_intensity
		{
			get { return _m_replace_intensity; }
			set { _m_replace_intensity = value; this.Invalidate(); }
		}
		
		private bool _m_replace_tint;
		[STNodeProperty("replace_tint", "replace_tint")]
		public bool m_replace_tint
		{
			get { return _m_replace_tint; }
			set { _m_replace_tint = value; this.Invalidate(); }
		}
		
		private cVector3 _m_decal_scale;
		[STNodeProperty("decal_scale", "decal_scale")]
		public cVector3 m_decal_scale
		{
			get { return _m_decal_scale; }
			set { _m_decal_scale = value; this.Invalidate(); }
		}
		
		private cVector3 _m_lightdecal_tint;
		[STNodeProperty("lightdecal_tint", "lightdecal_tint")]
		public cVector3 m_lightdecal_tint
		{
			get { return _m_lightdecal_tint; }
			set { _m_lightdecal_tint = value; this.Invalidate(); }
		}
		
		private float _m_lightdecal_intensity;
		[STNodeProperty("lightdecal_intensity", "lightdecal_intensity")]
		public float m_lightdecal_intensity
		{
			get { return _m_lightdecal_intensity; }
			set { _m_lightdecal_intensity = value; this.Invalidate(); }
		}
		
		private cVector3 _m_diffuse_colour_scale;
		[STNodeProperty("diffuse_colour_scale", "diffuse_colour_scale")]
		public cVector3 m_diffuse_colour_scale
		{
			get { return _m_diffuse_colour_scale; }
			set { _m_diffuse_colour_scale = value; this.Invalidate(); }
		}
		
		private float _m_diffuse_opacity_scale;
		[STNodeProperty("diffuse_opacity_scale", "diffuse_opacity_scale")]
		public float m_diffuse_opacity_scale
		{
			get { return _m_diffuse_opacity_scale; }
			set { _m_diffuse_opacity_scale = value; this.Invalidate(); }
		}
		
		private cVector3 _m_vertex_colour_scale;
		[STNodeProperty("vertex_colour_scale", "vertex_colour_scale")]
		public cVector3 m_vertex_colour_scale
		{
			get { return _m_vertex_colour_scale; }
			set { _m_vertex_colour_scale = value; this.Invalidate(); }
		}
		
		private float _m_vertex_opacity_scale;
		[STNodeProperty("vertex_opacity_scale", "vertex_opacity_scale")]
		public float m_vertex_opacity_scale
		{
			get { return _m_vertex_opacity_scale; }
			set { _m_vertex_opacity_scale = value; this.Invalidate(); }
		}
		
		private float _m_uv_scroll_speed_x;
		[STNodeProperty("uv_scroll_speed_x", "uv_scroll_speed_x")]
		public float m_uv_scroll_speed_x
		{
			get { return _m_uv_scroll_speed_x; }
			set { _m_uv_scroll_speed_x = value; this.Invalidate(); }
		}
		
		private float _m_uv_scroll_speed_y;
		[STNodeProperty("uv_scroll_speed_y", "uv_scroll_speed_y")]
		public float m_uv_scroll_speed_y
		{
			get { return _m_uv_scroll_speed_y; }
			set { _m_uv_scroll_speed_y = value; this.Invalidate(); }
		}
		
		private float _m_alpha_blend_noise_power_scale;
		[STNodeProperty("alpha_blend_noise_power_scale", "alpha_blend_noise_power_scale")]
		public float m_alpha_blend_noise_power_scale
		{
			get { return _m_alpha_blend_noise_power_scale; }
			set { _m_alpha_blend_noise_power_scale = value; this.Invalidate(); }
		}
		
		private float _m_alpha_blend_noise_uv_scale;
		[STNodeProperty("alpha_blend_noise_uv_scale", "alpha_blend_noise_uv_scale")]
		public float m_alpha_blend_noise_uv_scale
		{
			get { return _m_alpha_blend_noise_uv_scale; }
			set { _m_alpha_blend_noise_uv_scale = value; this.Invalidate(); }
		}
		
		private float _m_alpha_blend_noise_uv_offset_X;
		[STNodeProperty("alpha_blend_noise_uv_offset_X", "alpha_blend_noise_uv_offset_X")]
		public float m_alpha_blend_noise_uv_offset_X
		{
			get { return _m_alpha_blend_noise_uv_offset_X; }
			set { _m_alpha_blend_noise_uv_offset_X = value; this.Invalidate(); }
		}
		
		private float _m_alpha_blend_noise_uv_offset_Y;
		[STNodeProperty("alpha_blend_noise_uv_offset_Y", "alpha_blend_noise_uv_offset_Y")]
		public float m_alpha_blend_noise_uv_offset_Y
		{
			get { return _m_alpha_blend_noise_uv_offset_Y; }
			set { _m_alpha_blend_noise_uv_offset_Y = value; this.Invalidate(); }
		}
		
		private float _m_dirt_multiply_blend_spec_power_scale;
		[STNodeProperty("dirt_multiply_blend_spec_power_scale", "dirt_multiply_blend_spec_power_scale")]
		public float m_dirt_multiply_blend_spec_power_scale
		{
			get { return _m_dirt_multiply_blend_spec_power_scale; }
			set { _m_dirt_multiply_blend_spec_power_scale = value; this.Invalidate(); }
		}
		
		private float _m_dirt_map_uv_scale;
		[STNodeProperty("dirt_map_uv_scale", "dirt_map_uv_scale")]
		public float m_dirt_map_uv_scale
		{
			get { return _m_dirt_map_uv_scale; }
			set { _m_dirt_map_uv_scale = value; this.Invalidate(); }
		}
		
		private bool _m_remove_on_damaged;
		[STNodeProperty("remove_on_damaged", "remove_on_damaged")]
		public bool m_remove_on_damaged
		{
			get { return _m_remove_on_damaged; }
			set { _m_remove_on_damaged = value; this.Invalidate(); }
		}
		
		private int _m_damage_threshold;
		[STNodeProperty("damage_threshold", "damage_threshold")]
		public int m_damage_threshold
		{
			get { return _m_damage_threshold; }
			set { _m_damage_threshold = value; this.Invalidate(); }
		}
		
		private bool _m_is_debris;
		[STNodeProperty("is_debris", "is_debris")]
		public bool m_is_debris
		{
			get { return _m_is_debris; }
			set { _m_is_debris = value; this.Invalidate(); }
		}
		
		private bool _m_is_prop;
		[STNodeProperty("is_prop", "is_prop")]
		public bool m_is_prop
		{
			get { return _m_is_prop; }
			set { _m_is_prop = value; this.Invalidate(); }
		}
		
		private bool _m_is_thrown;
		[STNodeProperty("is_thrown", "is_thrown")]
		public bool m_is_thrown
		{
			get { return _m_is_thrown; }
			set { _m_is_thrown = value; this.Invalidate(); }
		}
		
		private bool _m_report_sliding;
		[STNodeProperty("report_sliding", "report_sliding")]
		public bool m_report_sliding
		{
			get { return _m_report_sliding; }
			set { _m_report_sliding = value; this.Invalidate(); }
		}
		
		private bool _m_force_keyframed;
		[STNodeProperty("force_keyframed", "force_keyframed")]
		public bool m_force_keyframed
		{
			get { return _m_force_keyframed; }
			set { _m_force_keyframed = value; this.Invalidate(); }
		}
		
		private bool _m_force_transparent;
		[STNodeProperty("force_transparent", "force_transparent")]
		public bool m_force_transparent
		{
			get { return _m_force_transparent; }
			set { _m_force_transparent = value; this.Invalidate(); }
		}
		
		private bool _m_soft_collision;
		[STNodeProperty("soft_collision", "soft_collision")]
		public bool m_soft_collision
		{
			get { return _m_soft_collision; }
			set { _m_soft_collision = value; this.Invalidate(); }
		}
		
		private bool _m_allow_reposition_of_physics;
		[STNodeProperty("allow_reposition_of_physics", "allow_reposition_of_physics")]
		public bool m_allow_reposition_of_physics
		{
			get { return _m_allow_reposition_of_physics; }
			set { _m_allow_reposition_of_physics = value; this.Invalidate(); }
		}
		
		private bool _m_disable_size_culling;
		[STNodeProperty("disable_size_culling", "disable_size_culling")]
		public bool m_disable_size_culling
		{
			get { return _m_disable_size_culling; }
			set { _m_disable_size_culling = value; this.Invalidate(); }
		}
		
		private bool _m_cast_shadows;
		[STNodeProperty("cast_shadows", "cast_shadows")]
		public bool m_cast_shadows
		{
			get { return _m_cast_shadows; }
			set { _m_cast_shadows = value; this.Invalidate(); }
		}
		
		private bool _m_cast_shadows_in_torch;
		[STNodeProperty("cast_shadows_in_torch", "cast_shadows_in_torch")]
		public bool m_cast_shadows_in_torch
		{
			get { return _m_cast_shadows_in_torch; }
			set { _m_cast_shadows_in_torch = value; this.Invalidate(); }
		}
		
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
		}
		
		private bool _m_delete_me;
		[STNodeProperty("delete_me", "delete_me")]
		public bool m_delete_me
		{
			get { return _m_delete_me; }
			set { _m_delete_me = value; this.Invalidate(); }
		}
		
		private string _m_name;
		[STNodeProperty("name", "name")]
		public string m_name
		{
			get { return _m_name; }
			set { _m_name = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "ModelReference";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("simulate", typeof(void), false);
			this.InputOptions.Add("keyframe", typeof(void), false);
			this.InputOptions.Add("light_switch_on", typeof(void), false);
			this.InputOptions.Add("light_switch_off", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("on_damaged", typeof(void), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("simulating", typeof(void), false);
			this.OutputOptions.Add("keyframed", typeof(void), false);
			this.OutputOptions.Add("light_switched_on", typeof(void), false);
			this.OutputOptions.Add("light_switched_off", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
