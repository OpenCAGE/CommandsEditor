using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SimpleWater : STNode
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
		
		private float _m_SHININESS;
		[STNodeProperty("SHININESS", "SHININESS")]
		public float m_SHININESS
		{
			get { return _m_SHININESS; }
			set { _m_SHININESS = value; this.Invalidate(); }
		}
		
		private float _m_softness_edge;
		[STNodeProperty("softness_edge", "softness_edge")]
		public float m_softness_edge
		{
			get { return _m_softness_edge; }
			set { _m_softness_edge = value; this.Invalidate(); }
		}
		
		private float _m_FRESNEL_POWER;
		[STNodeProperty("FRESNEL_POWER", "FRESNEL_POWER")]
		public float m_FRESNEL_POWER
		{
			get { return _m_FRESNEL_POWER; }
			set { _m_FRESNEL_POWER = value; this.Invalidate(); }
		}
		
		private float _m_MIN_FRESNEL;
		[STNodeProperty("MIN_FRESNEL", "MIN_FRESNEL")]
		public float m_MIN_FRESNEL
		{
			get { return _m_MIN_FRESNEL; }
			set { _m_MIN_FRESNEL = value; this.Invalidate(); }
		}
		
		private float _m_MAX_FRESNEL;
		[STNodeProperty("MAX_FRESNEL", "MAX_FRESNEL")]
		public float m_MAX_FRESNEL
		{
			get { return _m_MAX_FRESNEL; }
			set { _m_MAX_FRESNEL = value; this.Invalidate(); }
		}
		
		private bool _m_LOW_RES_ALPHA_PASS;
		[STNodeProperty("LOW_RES_ALPHA_PASS", "LOW_RES_ALPHA_PASS")]
		public bool m_LOW_RES_ALPHA_PASS
		{
			get { return _m_LOW_RES_ALPHA_PASS; }
			set { _m_LOW_RES_ALPHA_PASS = value; this.Invalidate(); }
		}
		
		private bool _m_ATMOSPHERIC_FOGGING;
		[STNodeProperty("ATMOSPHERIC_FOGGING", "ATMOSPHERIC_FOGGING")]
		public bool m_ATMOSPHERIC_FOGGING
		{
			get { return _m_ATMOSPHERIC_FOGGING; }
			set { _m_ATMOSPHERIC_FOGGING = value; this.Invalidate(); }
		}
		
		private string _m_NORMAL_MAP;
		[STNodeProperty("NORMAL_MAP", "NORMAL_MAP")]
		public string m_NORMAL_MAP
		{
			get { return _m_NORMAL_MAP; }
			set { _m_NORMAL_MAP = value; this.Invalidate(); }
		}
		
		private float _m_SPEED;
		[STNodeProperty("SPEED", "SPEED")]
		public float m_SPEED
		{
			get { return _m_SPEED; }
			set { _m_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_SCALE;
		[STNodeProperty("SCALE", "SCALE")]
		public float m_SCALE
		{
			get { return _m_SCALE; }
			set { _m_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_NORMAL_MAP_STRENGTH;
		[STNodeProperty("NORMAL_MAP_STRENGTH", "NORMAL_MAP_STRENGTH")]
		public float m_NORMAL_MAP_STRENGTH
		{
			get { return _m_NORMAL_MAP_STRENGTH; }
			set { _m_NORMAL_MAP_STRENGTH = value; this.Invalidate(); }
		}
		
		private bool _m_SECONDARY_NORMAL_MAPPING;
		[STNodeProperty("SECONDARY_NORMAL_MAPPING", "SECONDARY_NORMAL_MAPPING")]
		public bool m_SECONDARY_NORMAL_MAPPING
		{
			get { return _m_SECONDARY_NORMAL_MAPPING; }
			set { _m_SECONDARY_NORMAL_MAPPING = value; this.Invalidate(); }
		}
		
		private float _m_SECONDARY_SPEED;
		[STNodeProperty("SECONDARY_SPEED", "SECONDARY_SPEED")]
		public float m_SECONDARY_SPEED
		{
			get { return _m_SECONDARY_SPEED; }
			set { _m_SECONDARY_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_SECONDARY_SCALE;
		[STNodeProperty("SECONDARY_SCALE", "SECONDARY_SCALE")]
		public float m_SECONDARY_SCALE
		{
			get { return _m_SECONDARY_SCALE; }
			set { _m_SECONDARY_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_SECONDARY_NORMAL_MAP_STRENGTH;
		[STNodeProperty("SECONDARY_NORMAL_MAP_STRENGTH", "SECONDARY_NORMAL_MAP_STRENGTH")]
		public float m_SECONDARY_NORMAL_MAP_STRENGTH
		{
			get { return _m_SECONDARY_NORMAL_MAP_STRENGTH; }
			set { _m_SECONDARY_NORMAL_MAP_STRENGTH = value; this.Invalidate(); }
		}
		
		private bool _m_ALPHA_MASKING;
		[STNodeProperty("ALPHA_MASKING", "ALPHA_MASKING")]
		public bool m_ALPHA_MASKING
		{
			get { return _m_ALPHA_MASKING; }
			set { _m_ALPHA_MASKING = value; this.Invalidate(); }
		}
		
		private string _m_ALPHA_MASK;
		[STNodeProperty("ALPHA_MASK", "ALPHA_MASK")]
		public string m_ALPHA_MASK
		{
			get { return _m_ALPHA_MASK; }
			set { _m_ALPHA_MASK = value; this.Invalidate(); }
		}
		
		private bool _m_FLOW_MAPPING;
		[STNodeProperty("FLOW_MAPPING", "FLOW_MAPPING")]
		public bool m_FLOW_MAPPING
		{
			get { return _m_FLOW_MAPPING; }
			set { _m_FLOW_MAPPING = value; this.Invalidate(); }
		}
		
		private string _m_FLOW_MAP;
		[STNodeProperty("FLOW_MAP", "FLOW_MAP")]
		public string m_FLOW_MAP
		{
			get { return _m_FLOW_MAP; }
			set { _m_FLOW_MAP = value; this.Invalidate(); }
		}
		
		private float _m_CYCLE_TIME;
		[STNodeProperty("CYCLE_TIME", "CYCLE_TIME")]
		public float m_CYCLE_TIME
		{
			get { return _m_CYCLE_TIME; }
			set { _m_CYCLE_TIME = value; this.Invalidate(); }
		}
		
		private float _m_FLOW_SPEED;
		[STNodeProperty("FLOW_SPEED", "FLOW_SPEED")]
		public float m_FLOW_SPEED
		{
			get { return _m_FLOW_SPEED; }
			set { _m_FLOW_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_FLOW_TEX_SCALE;
		[STNodeProperty("FLOW_TEX_SCALE", "FLOW_TEX_SCALE")]
		public float m_FLOW_TEX_SCALE
		{
			get { return _m_FLOW_TEX_SCALE; }
			set { _m_FLOW_TEX_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_FLOW_WARP_STRENGTH;
		[STNodeProperty("FLOW_WARP_STRENGTH", "FLOW_WARP_STRENGTH")]
		public float m_FLOW_WARP_STRENGTH
		{
			get { return _m_FLOW_WARP_STRENGTH; }
			set { _m_FLOW_WARP_STRENGTH = value; this.Invalidate(); }
		}
		
		private bool _m_ENVIRONMENT_MAPPING;
		[STNodeProperty("ENVIRONMENT_MAPPING", "ENVIRONMENT_MAPPING")]
		public bool m_ENVIRONMENT_MAPPING
		{
			get { return _m_ENVIRONMENT_MAPPING; }
			set { _m_ENVIRONMENT_MAPPING = value; this.Invalidate(); }
		}
		
		private string _m_ENVIRONMENT_MAP;
		[STNodeProperty("ENVIRONMENT_MAP", "ENVIRONMENT_MAP")]
		public string m_ENVIRONMENT_MAP
		{
			get { return _m_ENVIRONMENT_MAP; }
			set { _m_ENVIRONMENT_MAP = value; this.Invalidate(); }
		}
		
		private float _m_ENVIRONMENT_MAP_MULT;
		[STNodeProperty("ENVIRONMENT_MAP_MULT", "ENVIRONMENT_MAP_MULT")]
		public float m_ENVIRONMENT_MAP_MULT
		{
			get { return _m_ENVIRONMENT_MAP_MULT; }
			set { _m_ENVIRONMENT_MAP_MULT = value; this.Invalidate(); }
		}
		
		private bool _m_LOCALISED_ENVIRONMENT_MAPPING;
		[STNodeProperty("LOCALISED_ENVIRONMENT_MAPPING", "LOCALISED_ENVIRONMENT_MAPPING")]
		public bool m_LOCALISED_ENVIRONMENT_MAPPING
		{
			get { return _m_LOCALISED_ENVIRONMENT_MAPPING; }
			set { _m_LOCALISED_ENVIRONMENT_MAPPING = value; this.Invalidate(); }
		}
		
		private float _m_ENVMAP_SIZE;
		[STNodeProperty("ENVMAP_SIZE", "ENVMAP_SIZE")]
		public float m_ENVMAP_SIZE
		{
			get { return _m_ENVMAP_SIZE; }
			set { _m_ENVMAP_SIZE = value; this.Invalidate(); }
		}
		
		private bool _m_LOCALISED_ENVMAP_BOX_PROJECTION;
		[STNodeProperty("LOCALISED_ENVMAP_BOX_PROJECTION", "LOCALISED_ENVMAP_BOX_PROJECTION")]
		public bool m_LOCALISED_ENVMAP_BOX_PROJECTION
		{
			get { return _m_LOCALISED_ENVMAP_BOX_PROJECTION; }
			set { _m_LOCALISED_ENVMAP_BOX_PROJECTION = value; this.Invalidate(); }
		}
		
		private cVector3 _m_ENVMAP_BOXPROJ_BB_SCALE;
		[STNodeProperty("ENVMAP_BOXPROJ_BB_SCALE", "ENVMAP_BOXPROJ_BB_SCALE")]
		public cVector3 m_ENVMAP_BOXPROJ_BB_SCALE
		{
			get { return _m_ENVMAP_BOXPROJ_BB_SCALE; }
			set { _m_ENVMAP_BOXPROJ_BB_SCALE = value; this.Invalidate(); }
		}
		
		private bool _m_REFLECTIVE_MAPPING;
		[STNodeProperty("REFLECTIVE_MAPPING", "REFLECTIVE_MAPPING")]
		public bool m_REFLECTIVE_MAPPING
		{
			get { return _m_REFLECTIVE_MAPPING; }
			set { _m_REFLECTIVE_MAPPING = value; this.Invalidate(); }
		}
		
		private float _m_REFLECTION_PERTURBATION_STRENGTH;
		[STNodeProperty("REFLECTION_PERTURBATION_STRENGTH", "REFLECTION_PERTURBATION_STRENGTH")]
		public float m_REFLECTION_PERTURBATION_STRENGTH
		{
			get { return _m_REFLECTION_PERTURBATION_STRENGTH; }
			set { _m_REFLECTION_PERTURBATION_STRENGTH = value; this.Invalidate(); }
		}
		
		private cVector3 _m_DEPTH_FOG_INITIAL_COLOUR;
		[STNodeProperty("DEPTH_FOG_INITIAL_COLOUR", "DEPTH_FOG_INITIAL_COLOUR")]
		public cVector3 m_DEPTH_FOG_INITIAL_COLOUR
		{
			get { return _m_DEPTH_FOG_INITIAL_COLOUR; }
			set { _m_DEPTH_FOG_INITIAL_COLOUR = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_FOG_INITIAL_ALPHA;
		[STNodeProperty("DEPTH_FOG_INITIAL_ALPHA", "DEPTH_FOG_INITIAL_ALPHA")]
		public float m_DEPTH_FOG_INITIAL_ALPHA
		{
			get { return _m_DEPTH_FOG_INITIAL_ALPHA; }
			set { _m_DEPTH_FOG_INITIAL_ALPHA = value; this.Invalidate(); }
		}
		
		private cVector3 _m_DEPTH_FOG_MIDPOINT_COLOUR;
		[STNodeProperty("DEPTH_FOG_MIDPOINT_COLOUR", "DEPTH_FOG_MIDPOINT_COLOUR")]
		public cVector3 m_DEPTH_FOG_MIDPOINT_COLOUR
		{
			get { return _m_DEPTH_FOG_MIDPOINT_COLOUR; }
			set { _m_DEPTH_FOG_MIDPOINT_COLOUR = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_FOG_MIDPOINT_ALPHA;
		[STNodeProperty("DEPTH_FOG_MIDPOINT_ALPHA", "DEPTH_FOG_MIDPOINT_ALPHA")]
		public float m_DEPTH_FOG_MIDPOINT_ALPHA
		{
			get { return _m_DEPTH_FOG_MIDPOINT_ALPHA; }
			set { _m_DEPTH_FOG_MIDPOINT_ALPHA = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_FOG_MIDPOINT_DEPTH;
		[STNodeProperty("DEPTH_FOG_MIDPOINT_DEPTH", "DEPTH_FOG_MIDPOINT_DEPTH")]
		public float m_DEPTH_FOG_MIDPOINT_DEPTH
		{
			get { return _m_DEPTH_FOG_MIDPOINT_DEPTH; }
			set { _m_DEPTH_FOG_MIDPOINT_DEPTH = value; this.Invalidate(); }
		}
		
		private cVector3 _m_DEPTH_FOG_END_COLOUR;
		[STNodeProperty("DEPTH_FOG_END_COLOUR", "DEPTH_FOG_END_COLOUR")]
		public cVector3 m_DEPTH_FOG_END_COLOUR
		{
			get { return _m_DEPTH_FOG_END_COLOUR; }
			set { _m_DEPTH_FOG_END_COLOUR = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_FOG_END_ALPHA;
		[STNodeProperty("DEPTH_FOG_END_ALPHA", "DEPTH_FOG_END_ALPHA")]
		public float m_DEPTH_FOG_END_ALPHA
		{
			get { return _m_DEPTH_FOG_END_ALPHA; }
			set { _m_DEPTH_FOG_END_ALPHA = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_FOG_END_DEPTH;
		[STNodeProperty("DEPTH_FOG_END_DEPTH", "DEPTH_FOG_END_DEPTH")]
		public float m_DEPTH_FOG_END_DEPTH
		{
			get { return _m_DEPTH_FOG_END_DEPTH; }
			set { _m_DEPTH_FOG_END_DEPTH = value; this.Invalidate(); }
		}
		
		private string _m_CAUSTIC_TEXTURE;
		[STNodeProperty("CAUSTIC_TEXTURE", "CAUSTIC_TEXTURE")]
		public string m_CAUSTIC_TEXTURE
		{
			get { return _m_CAUSTIC_TEXTURE; }
			set { _m_CAUSTIC_TEXTURE = value; this.Invalidate(); }
		}
		
		private float _m_CAUSTIC_TEXTURE_SCALE;
		[STNodeProperty("CAUSTIC_TEXTURE_SCALE", "CAUSTIC_TEXTURE_SCALE")]
		public float m_CAUSTIC_TEXTURE_SCALE
		{
			get { return _m_CAUSTIC_TEXTURE_SCALE; }
			set { _m_CAUSTIC_TEXTURE_SCALE = value; this.Invalidate(); }
		}
		
		private bool _m_CAUSTIC_REFRACTIONS;
		[STNodeProperty("CAUSTIC_REFRACTIONS", "CAUSTIC_REFRACTIONS")]
		public bool m_CAUSTIC_REFRACTIONS
		{
			get { return _m_CAUSTIC_REFRACTIONS; }
			set { _m_CAUSTIC_REFRACTIONS = value; this.Invalidate(); }
		}
		
		private bool _m_CAUSTIC_REFLECTIONS;
		[STNodeProperty("CAUSTIC_REFLECTIONS", "CAUSTIC_REFLECTIONS")]
		public bool m_CAUSTIC_REFLECTIONS
		{
			get { return _m_CAUSTIC_REFLECTIONS; }
			set { _m_CAUSTIC_REFLECTIONS = value; this.Invalidate(); }
		}
		
		private float _m_CAUSTIC_SPEED_SCALAR;
		[STNodeProperty("CAUSTIC_SPEED_SCALAR", "CAUSTIC_SPEED_SCALAR")]
		public float m_CAUSTIC_SPEED_SCALAR
		{
			get { return _m_CAUSTIC_SPEED_SCALAR; }
			set { _m_CAUSTIC_SPEED_SCALAR = value; this.Invalidate(); }
		}
		
		private float _m_CAUSTIC_INTENSITY;
		[STNodeProperty("CAUSTIC_INTENSITY", "CAUSTIC_INTENSITY")]
		public float m_CAUSTIC_INTENSITY
		{
			get { return _m_CAUSTIC_INTENSITY; }
			set { _m_CAUSTIC_INTENSITY = value; this.Invalidate(); }
		}
		
		private float _m_CAUSTIC_SURFACE_WRAP;
		[STNodeProperty("CAUSTIC_SURFACE_WRAP", "CAUSTIC_SURFACE_WRAP")]
		public float m_CAUSTIC_SURFACE_WRAP
		{
			get { return _m_CAUSTIC_SURFACE_WRAP; }
			set { _m_CAUSTIC_SURFACE_WRAP = value; this.Invalidate(); }
		}
		
		private float _m_CAUSTIC_HEIGHT;
		[STNodeProperty("CAUSTIC_HEIGHT", "CAUSTIC_HEIGHT")]
		public float m_CAUSTIC_HEIGHT
		{
			get { return _m_CAUSTIC_HEIGHT; }
			set { _m_CAUSTIC_HEIGHT = value; this.Invalidate(); }
		}
		
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private cVector3 _m_half_dimensions;
		[STNodeProperty("half_dimensions", "half_dimensions")]
		public cVector3 m_half_dimensions
		{
			get { return _m_half_dimensions; }
			set { _m_half_dimensions = value; this.Invalidate(); }
		}
		
		private bool _m_include_physics;
		[STNodeProperty("include_physics", "include_physics")]
		public bool m_include_physics
		{
			get { return _m_include_physics; }
			set { _m_include_physics = value; this.Invalidate(); }
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
			
			this.Title = "SimpleWater";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("event", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
