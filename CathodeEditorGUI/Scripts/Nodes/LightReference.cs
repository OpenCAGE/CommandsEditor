#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class LightReference : STNode
	{
		private bool _m_deleted;
		[STNodeProperty("deleted", "deleted")]
		public bool m_deleted
		{
			get { return _m_deleted; }
			set { _m_deleted = value; this.Invalidate(); }
		}
		
		private bool _m_show_on_reset;
		[STNodeProperty("show_on_reset", "show_on_reset")]
		public bool m_show_on_reset
		{
			get { return _m_show_on_reset; }
			set { _m_show_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_light_on_reset;
		[STNodeProperty("light_on_reset", "light_on_reset")]
		public bool m_light_on_reset
		{
			get { return _m_light_on_reset; }
			set { _m_light_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_type;
		[STNodeProperty("type", "type")]
		public string m_type
		{
			get { return _m_type; }
			set { _m_type = value; this.Invalidate(); }
		}
		
		private float _m_defocus_attenuation;
		[STNodeProperty("defocus_attenuation", "defocus_attenuation")]
		public float m_defocus_attenuation
		{
			get { return _m_defocus_attenuation; }
			set { _m_defocus_attenuation = value; this.Invalidate(); }
		}
		
		private float _m_start_attenuation;
		[STNodeProperty("start_attenuation", "start_attenuation")]
		public float m_start_attenuation
		{
			get { return _m_start_attenuation; }
			set { _m_start_attenuation = value; this.Invalidate(); }
		}
		
		private float _m_end_attenuation;
		[STNodeProperty("end_attenuation", "end_attenuation")]
		public float m_end_attenuation
		{
			get { return _m_end_attenuation; }
			set { _m_end_attenuation = value; this.Invalidate(); }
		}
		
		private bool _m_physical_attenuation;
		[STNodeProperty("physical_attenuation", "physical_attenuation")]
		public bool m_physical_attenuation
		{
			get { return _m_physical_attenuation; }
			set { _m_physical_attenuation = value; this.Invalidate(); }
		}
		
		private float _m_near_dist;
		[STNodeProperty("near_dist", "near_dist")]
		public float m_near_dist
		{
			get { return _m_near_dist; }
			set { _m_near_dist = value; this.Invalidate(); }
		}
		
		private float _m_near_dist_shadow_offset;
		[STNodeProperty("near_dist_shadow_offset", "near_dist_shadow_offset")]
		public float m_near_dist_shadow_offset
		{
			get { return _m_near_dist_shadow_offset; }
			set { _m_near_dist_shadow_offset = value; this.Invalidate(); }
		}
		
		private float _m_inner_cone_angle;
		[STNodeProperty("inner_cone_angle", "inner_cone_angle")]
		public float m_inner_cone_angle
		{
			get { return _m_inner_cone_angle; }
			set { _m_inner_cone_angle = value; this.Invalidate(); }
		}
		
		private float _m_outer_cone_angle;
		[STNodeProperty("outer_cone_angle", "outer_cone_angle")]
		public float m_outer_cone_angle
		{
			get { return _m_outer_cone_angle; }
			set { _m_outer_cone_angle = value; this.Invalidate(); }
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
		
		private float _m_area_light_radius;
		[STNodeProperty("area_light_radius", "area_light_radius")]
		public float m_area_light_radius
		{
			get { return _m_area_light_radius; }
			set { _m_area_light_radius = value; this.Invalidate(); }
		}
		
		private float _m_diffuse_softness;
		[STNodeProperty("diffuse_softness", "diffuse_softness")]
		public float m_diffuse_softness
		{
			get { return _m_diffuse_softness; }
			set { _m_diffuse_softness = value; this.Invalidate(); }
		}
		
		private float _m_diffuse_bias;
		[STNodeProperty("diffuse_bias", "diffuse_bias")]
		public float m_diffuse_bias
		{
			get { return _m_diffuse_bias; }
			set { _m_diffuse_bias = value; this.Invalidate(); }
		}
		
		private float _m_glossiness_scale;
		[STNodeProperty("glossiness_scale", "glossiness_scale")]
		public float m_glossiness_scale
		{
			get { return _m_glossiness_scale; }
			set { _m_glossiness_scale = value; this.Invalidate(); }
		}
		
		private float _m_flare_occluder_radius;
		[STNodeProperty("flare_occluder_radius", "flare_occluder_radius")]
		public float m_flare_occluder_radius
		{
			get { return _m_flare_occluder_radius; }
			set { _m_flare_occluder_radius = value; this.Invalidate(); }
		}
		
		private float _m_flare_spot_offset;
		[STNodeProperty("flare_spot_offset", "flare_spot_offset")]
		public float m_flare_spot_offset
		{
			get { return _m_flare_spot_offset; }
			set { _m_flare_spot_offset = value; this.Invalidate(); }
		}
		
		private float _m_flare_intensity_scale;
		[STNodeProperty("flare_intensity_scale", "flare_intensity_scale")]
		public float m_flare_intensity_scale
		{
			get { return _m_flare_intensity_scale; }
			set { _m_flare_intensity_scale = value; this.Invalidate(); }
		}
		
		private bool _m_cast_shadow;
		[STNodeProperty("cast_shadow", "cast_shadow")]
		public bool m_cast_shadow
		{
			get { return _m_cast_shadow; }
			set { _m_cast_shadow = value; this.Invalidate(); }
		}
		
		private string _m_fade_type;
		[STNodeProperty("fade_type", "fade_type")]
		public string m_fade_type
		{
			get { return _m_fade_type; }
			set { _m_fade_type = value; this.Invalidate(); }
		}
		
		private bool _m_is_specular;
		[STNodeProperty("is_specular", "is_specular")]
		public bool m_is_specular
		{
			get { return _m_is_specular; }
			set { _m_is_specular = value; this.Invalidate(); }
		}
		
		private bool _m_has_lens_flare;
		[STNodeProperty("has_lens_flare", "has_lens_flare")]
		public bool m_has_lens_flare
		{
			get { return _m_has_lens_flare; }
			set { _m_has_lens_flare = value; this.Invalidate(); }
		}
		
		private bool _m_has_noclip;
		[STNodeProperty("has_noclip", "has_noclip")]
		public bool m_has_noclip
		{
			get { return _m_has_noclip; }
			set { _m_has_noclip = value; this.Invalidate(); }
		}
		
		private bool _m_is_square_light;
		[STNodeProperty("is_square_light", "is_square_light")]
		public bool m_is_square_light
		{
			get { return _m_is_square_light; }
			set { _m_is_square_light = value; this.Invalidate(); }
		}
		
		private bool _m_is_flash_light;
		[STNodeProperty("is_flash_light", "is_flash_light")]
		public bool m_is_flash_light
		{
			get { return _m_is_flash_light; }
			set { _m_is_flash_light = value; this.Invalidate(); }
		}
		
		private bool _m_no_alphalight;
		[STNodeProperty("no_alphalight", "no_alphalight")]
		public bool m_no_alphalight
		{
			get { return _m_no_alphalight; }
			set { _m_no_alphalight = value; this.Invalidate(); }
		}
		
		private bool _m_include_in_planar_reflections;
		[STNodeProperty("include_in_planar_reflections", "include_in_planar_reflections")]
		public bool m_include_in_planar_reflections
		{
			get { return _m_include_in_planar_reflections; }
			set { _m_include_in_planar_reflections = value; this.Invalidate(); }
		}
		
		private int _m_shadow_priority;
		[STNodeProperty("shadow_priority", "shadow_priority")]
		public int m_shadow_priority
		{
			get { return _m_shadow_priority; }
			set { _m_shadow_priority = value; this.Invalidate(); }
		}
		
		private float _m_aspect_ratio;
		[STNodeProperty("aspect_ratio", "aspect_ratio")]
		public float m_aspect_ratio
		{
			get { return _m_aspect_ratio; }
			set { _m_aspect_ratio = value; this.Invalidate(); }
		}
		
		private string _m_gobo_texture;
		[STNodeProperty("gobo_texture", "gobo_texture")]
		public string m_gobo_texture
		{
			get { return _m_gobo_texture; }
			set { _m_gobo_texture = value; this.Invalidate(); }
		}
		
		private bool _m_horizontal_gobo_flip;
		[STNodeProperty("horizontal_gobo_flip", "horizontal_gobo_flip")]
		public bool m_horizontal_gobo_flip
		{
			get { return _m_horizontal_gobo_flip; }
			set { _m_horizontal_gobo_flip = value; this.Invalidate(); }
		}
		
		private cVector3 _m_colour;
		[STNodeProperty("colour", "colour")]
		public cVector3 m_colour
		{
			get { return _m_colour; }
			set { _m_colour = value; this.Invalidate(); }
		}
		
		private float _m_strip_length;
		[STNodeProperty("strip_length", "strip_length")]
		public float m_strip_length
		{
			get { return _m_strip_length; }
			set { _m_strip_length = value; this.Invalidate(); }
		}
		
		private bool _m_distance_mip_selection_gobo;
		[STNodeProperty("distance_mip_selection_gobo", "distance_mip_selection_gobo")]
		public bool m_distance_mip_selection_gobo
		{
			get { return _m_distance_mip_selection_gobo; }
			set { _m_distance_mip_selection_gobo = value; this.Invalidate(); }
		}
		
		private bool _m_volume;
		[STNodeProperty("volume", "volume")]
		public bool m_volume
		{
			get { return _m_volume; }
			set { _m_volume = value; this.Invalidate(); }
		}
		
		private float _m_volume_end_attenuation;
		[STNodeProperty("volume_end_attenuation", "volume_end_attenuation")]
		public float m_volume_end_attenuation
		{
			get { return _m_volume_end_attenuation; }
			set { _m_volume_end_attenuation = value; this.Invalidate(); }
		}
		
		private cVector3 _m_volume_colour_factor;
		[STNodeProperty("volume_colour_factor", "volume_colour_factor")]
		public cVector3 m_volume_colour_factor
		{
			get { return _m_volume_colour_factor; }
			set { _m_volume_colour_factor = value; this.Invalidate(); }
		}
		
		private float _m_volume_density;
		[STNodeProperty("volume_density", "volume_density")]
		public float m_volume_density
		{
			get { return _m_volume_density; }
			set { _m_volume_density = value; this.Invalidate(); }
		}
		
		private float _m_depth_bias;
		[STNodeProperty("depth_bias", "depth_bias")]
		public float m_depth_bias
		{
			get { return _m_depth_bias; }
			set { _m_depth_bias = value; this.Invalidate(); }
		}
		
		private int _m_slope_scale_depth_bias;
		[STNodeProperty("slope_scale_depth_bias", "slope_scale_depth_bias")]
		public int m_slope_scale_depth_bias
		{
			get { return _m_slope_scale_depth_bias; }
			set { _m_slope_scale_depth_bias = value; this.Invalidate(); }
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
			
			this.Title = "LightReference";
			
			this.InputOptions.Add("occlusion_geometry", typeof(string), false);
			this.InputOptions.Add("mastered_by_visibility", typeof(STNode), false);
			this.InputOptions.Add("exclude_shadow_entities", typeof(STNode), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			this.InputOptions.Add("light_switch_on", typeof(void), false);
			this.InputOptions.Add("light_switch_off", typeof(void), false);
			this.InputOptions.Add("purge", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
			this.OutputOptions.Add("light_switched_on", typeof(void), false);
			this.OutputOptions.Add("light_switched_off", typeof(void), false);
			this.OutputOptions.Add("purged", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
