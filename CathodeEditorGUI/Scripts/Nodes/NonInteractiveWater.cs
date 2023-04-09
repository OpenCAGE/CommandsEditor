using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NonInteractiveWater : STNode
	{
		private float _m_SCALE_X;
		[STNodeProperty("SCALE_X", "SCALE_X")]
		public float m_SCALE_X
		{
			get { return _m_SCALE_X; }
			set { _m_SCALE_X = value; this.Invalidate(); }
		}
		
		private float _m_SCALE_Z;
		[STNodeProperty("SCALE_Z", "SCALE_Z")]
		public float m_SCALE_Z
		{
			get { return _m_SCALE_Z; }
			set { _m_SCALE_Z = value; this.Invalidate(); }
		}
		
		private float _m_SHININESS;
		[STNodeProperty("SHININESS", "SHININESS")]
		public float m_SHININESS
		{
			get { return _m_SHININESS; }
			set { _m_SHININESS = value; this.Invalidate(); }
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
		
		private float _m_ENVIRONMENT_MAP_MULT;
		[STNodeProperty("ENVIRONMENT_MAP_MULT", "ENVIRONMENT_MAP_MULT")]
		public float m_ENVIRONMENT_MAP_MULT
		{
			get { return _m_ENVIRONMENT_MAP_MULT; }
			set { _m_ENVIRONMENT_MAP_MULT = value; this.Invalidate(); }
		}
		
		private float _m_ENVMAP_SIZE;
		[STNodeProperty("ENVMAP_SIZE", "ENVMAP_SIZE")]
		public float m_ENVMAP_SIZE
		{
			get { return _m_ENVMAP_SIZE; }
			set { _m_ENVMAP_SIZE = value; this.Invalidate(); }
		}
		
		private cVector3 _m_ENVMAP_BOXPROJ_BB_SCALE;
		[STNodeProperty("ENVMAP_BOXPROJ_BB_SCALE", "ENVMAP_BOXPROJ_BB_SCALE")]
		public cVector3 m_ENVMAP_BOXPROJ_BB_SCALE
		{
			get { return _m_ENVMAP_BOXPROJ_BB_SCALE; }
			set { _m_ENVMAP_BOXPROJ_BB_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_REFLECTION_PERTURBATION_STRENGTH;
		[STNodeProperty("REFLECTION_PERTURBATION_STRENGTH", "REFLECTION_PERTURBATION_STRENGTH")]
		public float m_REFLECTION_PERTURBATION_STRENGTH
		{
			get { return _m_REFLECTION_PERTURBATION_STRENGTH; }
			set { _m_REFLECTION_PERTURBATION_STRENGTH = value; this.Invalidate(); }
		}
		
		private float _m_ALPHA_PERTURBATION_STRENGTH;
		[STNodeProperty("ALPHA_PERTURBATION_STRENGTH", "ALPHA_PERTURBATION_STRENGTH")]
		public float m_ALPHA_PERTURBATION_STRENGTH
		{
			get { return _m_ALPHA_PERTURBATION_STRENGTH; }
			set { _m_ALPHA_PERTURBATION_STRENGTH = value; this.Invalidate(); }
		}
		
		private float _m_ALPHALIGHT_MULT;
		[STNodeProperty("ALPHALIGHT_MULT", "ALPHALIGHT_MULT")]
		public float m_ALPHALIGHT_MULT
		{
			get { return _m_ALPHALIGHT_MULT; }
			set { _m_ALPHALIGHT_MULT = value; this.Invalidate(); }
		}
		
		private float _m_softness_edge;
		[STNodeProperty("softness_edge", "softness_edge")]
		public float m_softness_edge
		{
			get { return _m_softness_edge; }
			set { _m_softness_edge = value; this.Invalidate(); }
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
			
			this.Title = "NonInteractiveWater";
			
			this.InputOptions.Add("water_resource", typeof(STNode), false);
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
