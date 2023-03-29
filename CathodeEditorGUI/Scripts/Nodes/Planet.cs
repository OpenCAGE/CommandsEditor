#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Planet : STNode
	{
		private float _m_parallax_scale;
		[STNodeProperty("parallax_scale", "parallax_scale")]
		public float m_parallax_scale
		{
			get { return _m_parallax_scale; }
			set { _m_parallax_scale = value; this.Invalidate(); }
		}
		
		private int _m_planet_sort_key;
		[STNodeProperty("planet_sort_key", "planet_sort_key")]
		public int m_planet_sort_key
		{
			get { return _m_planet_sort_key; }
			set { _m_planet_sort_key = value; this.Invalidate(); }
		}
		
		private float _m_overbright_scalar;
		[STNodeProperty("overbright_scalar", "overbright_scalar")]
		public float m_overbright_scalar
		{
			get { return _m_overbright_scalar; }
			set { _m_overbright_scalar = value; this.Invalidate(); }
		}
		
		private float _m_light_wrap_angle_scalar;
		[STNodeProperty("light_wrap_angle_scalar", "light_wrap_angle_scalar")]
		public float m_light_wrap_angle_scalar
		{
			get { return _m_light_wrap_angle_scalar; }
			set { _m_light_wrap_angle_scalar = value; this.Invalidate(); }
		}
		
		private float _m_penumbra_falloff_power_scalar;
		[STNodeProperty("penumbra_falloff_power_scalar", "penumbra_falloff_power_scalar")]
		public float m_penumbra_falloff_power_scalar
		{
			get { return _m_penumbra_falloff_power_scalar; }
			set { _m_penumbra_falloff_power_scalar = value; this.Invalidate(); }
		}
		
		private float _m_lens_flare_brightness;
		[STNodeProperty("lens_flare_brightness", "lens_flare_brightness")]
		public float m_lens_flare_brightness
		{
			get { return _m_lens_flare_brightness; }
			set { _m_lens_flare_brightness = value; this.Invalidate(); }
		}
		
		private cVector3 _m_lens_flare_colour;
		[STNodeProperty("lens_flare_colour", "lens_flare_colour")]
		public cVector3 m_lens_flare_colour
		{
			get { return _m_lens_flare_colour; }
			set { _m_lens_flare_colour = value; this.Invalidate(); }
		}
		
		private float _m_atmosphere_edge_falloff_power;
		[STNodeProperty("atmosphere_edge_falloff_power", "atmosphere_edge_falloff_power")]
		public float m_atmosphere_edge_falloff_power
		{
			get { return _m_atmosphere_edge_falloff_power; }
			set { _m_atmosphere_edge_falloff_power = value; this.Invalidate(); }
		}
		
		private float _m_atmosphere_edge_transparency;
		[STNodeProperty("atmosphere_edge_transparency", "atmosphere_edge_transparency")]
		public float m_atmosphere_edge_transparency
		{
			get { return _m_atmosphere_edge_transparency; }
			set { _m_atmosphere_edge_transparency = value; this.Invalidate(); }
		}
		
		private float _m_atmosphere_scroll_speed;
		[STNodeProperty("atmosphere_scroll_speed", "atmosphere_scroll_speed")]
		public float m_atmosphere_scroll_speed
		{
			get { return _m_atmosphere_scroll_speed; }
			set { _m_atmosphere_scroll_speed = value; this.Invalidate(); }
		}
		
		private float _m_atmosphere_detail_scroll_speed;
		[STNodeProperty("atmosphere_detail_scroll_speed", "atmosphere_detail_scroll_speed")]
		public float m_atmosphere_detail_scroll_speed
		{
			get { return _m_atmosphere_detail_scroll_speed; }
			set { _m_atmosphere_detail_scroll_speed = value; this.Invalidate(); }
		}
		
		private float _m_override_global_tint;
		[STNodeProperty("override_global_tint", "override_global_tint")]
		public float m_override_global_tint
		{
			get { return _m_override_global_tint; }
			set { _m_override_global_tint = value; this.Invalidate(); }
		}
		
		private cVector3 _m_global_tint;
		[STNodeProperty("global_tint", "global_tint")]
		public cVector3 m_global_tint
		{
			get { return _m_global_tint; }
			set { _m_global_tint = value; this.Invalidate(); }
		}
		
		private float _m_flow_cycle_time;
		[STNodeProperty("flow_cycle_time", "flow_cycle_time")]
		public float m_flow_cycle_time
		{
			get { return _m_flow_cycle_time; }
			set { _m_flow_cycle_time = value; this.Invalidate(); }
		}
		
		private float _m_flow_speed;
		[STNodeProperty("flow_speed", "flow_speed")]
		public float m_flow_speed
		{
			get { return _m_flow_speed; }
			set { _m_flow_speed = value; this.Invalidate(); }
		}
		
		private float _m_flow_tex_scale;
		[STNodeProperty("flow_tex_scale", "flow_tex_scale")]
		public float m_flow_tex_scale
		{
			get { return _m_flow_tex_scale; }
			set { _m_flow_tex_scale = value; this.Invalidate(); }
		}
		
		private float _m_flow_warp_strength;
		[STNodeProperty("flow_warp_strength", "flow_warp_strength")]
		public float m_flow_warp_strength
		{
			get { return _m_flow_warp_strength; }
			set { _m_flow_warp_strength = value; this.Invalidate(); }
		}
		
		private float _m_detail_uv_scale;
		[STNodeProperty("detail_uv_scale", "detail_uv_scale")]
		public float m_detail_uv_scale
		{
			get { return _m_detail_uv_scale; }
			set { _m_detail_uv_scale = value; this.Invalidate(); }
		}
		
		private float _m_normal_uv_scale;
		[STNodeProperty("normal_uv_scale", "normal_uv_scale")]
		public float m_normal_uv_scale
		{
			get { return _m_normal_uv_scale; }
			set { _m_normal_uv_scale = value; this.Invalidate(); }
		}
		
		private float _m_terrain_uv_scale;
		[STNodeProperty("terrain_uv_scale", "terrain_uv_scale")]
		public float m_terrain_uv_scale
		{
			get { return _m_terrain_uv_scale; }
			set { _m_terrain_uv_scale = value; this.Invalidate(); }
		}
		
		private float _m_atmosphere_normal_strength;
		[STNodeProperty("atmosphere_normal_strength", "atmosphere_normal_strength")]
		public float m_atmosphere_normal_strength
		{
			get { return _m_atmosphere_normal_strength; }
			set { _m_atmosphere_normal_strength = value; this.Invalidate(); }
		}
		
		private float _m_terrain_normal_strength;
		[STNodeProperty("terrain_normal_strength", "terrain_normal_strength")]
		public float m_terrain_normal_strength
		{
			get { return _m_terrain_normal_strength; }
			set { _m_terrain_normal_strength = value; this.Invalidate(); }
		}
		
		private cVector3 _m_light_shaft_colour;
		[STNodeProperty("light_shaft_colour", "light_shaft_colour")]
		public cVector3 m_light_shaft_colour
		{
			get { return _m_light_shaft_colour; }
			set { _m_light_shaft_colour = value; this.Invalidate(); }
		}
		
		private float _m_light_shaft_range;
		[STNodeProperty("light_shaft_range", "light_shaft_range")]
		public float m_light_shaft_range
		{
			get { return _m_light_shaft_range; }
			set { _m_light_shaft_range = value; this.Invalidate(); }
		}
		
		private float _m_light_shaft_decay;
		[STNodeProperty("light_shaft_decay", "light_shaft_decay")]
		public float m_light_shaft_decay
		{
			get { return _m_light_shaft_decay; }
			set { _m_light_shaft_decay = value; this.Invalidate(); }
		}
		
		private float _m_light_shaft_min_occlusion_distance;
		[STNodeProperty("light_shaft_min_occlusion_distance", "light_shaft_min_occlusion_distance")]
		public float m_light_shaft_min_occlusion_distance
		{
			get { return _m_light_shaft_min_occlusion_distance; }
			set { _m_light_shaft_min_occlusion_distance = value; this.Invalidate(); }
		}
		
		private float _m_light_shaft_intensity;
		[STNodeProperty("light_shaft_intensity", "light_shaft_intensity")]
		public float m_light_shaft_intensity
		{
			get { return _m_light_shaft_intensity; }
			set { _m_light_shaft_intensity = value; this.Invalidate(); }
		}
		
		private float _m_light_shaft_density;
		[STNodeProperty("light_shaft_density", "light_shaft_density")]
		public float m_light_shaft_density
		{
			get { return _m_light_shaft_density; }
			set { _m_light_shaft_density = value; this.Invalidate(); }
		}
		
		private bool _m_light_shaft_source_occlusion;
		[STNodeProperty("light_shaft_source_occlusion", "light_shaft_source_occlusion")]
		public bool m_light_shaft_source_occlusion
		{
			get { return _m_light_shaft_source_occlusion; }
			set { _m_light_shaft_source_occlusion = value; this.Invalidate(); }
		}
		
		private bool _m_blocks_light_shafts;
		[STNodeProperty("blocks_light_shafts", "blocks_light_shafts")]
		public bool m_blocks_light_shafts
		{
			get { return _m_blocks_light_shafts; }
			set { _m_blocks_light_shafts = value; this.Invalidate(); }
		}
		
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_pause_on_reset;
		[STNodeProperty("pause_on_reset", "pause_on_reset")]
		public bool m_pause_on_reset
		{
			get { return _m_pause_on_reset; }
			set { _m_pause_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "Planet";
			
			this.InputOptions.Add("planet_resource", typeof(STNode), false);
			this.InputOptions.Add("parallax_position", typeof(cTransform), false);
			this.InputOptions.Add("sun_position", typeof(cTransform), false);
			this.InputOptions.Add("light_shaft_source_position", typeof(cTransform), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
