using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SimpleRefraction : STNode
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
		
		private float _m_DISTANCEFACTOR;
		[STNodeProperty("DISTANCEFACTOR", "DISTANCEFACTOR")]
		public float m_DISTANCEFACTOR
		{
			get { return _m_DISTANCEFACTOR; }
			set { _m_DISTANCEFACTOR = value; this.Invalidate(); }
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
		
		private float _m_REFRACTFACTOR;
		[STNodeProperty("REFRACTFACTOR", "REFRACTFACTOR")]
		public float m_REFRACTFACTOR
		{
			get { return _m_REFRACTFACTOR; }
			set { _m_REFRACTFACTOR = value; this.Invalidate(); }
		}
		
		private bool _m_SECONDARY_NORMAL_MAPPING;
		[STNodeProperty("SECONDARY_NORMAL_MAPPING", "SECONDARY_NORMAL_MAPPING")]
		public bool m_SECONDARY_NORMAL_MAPPING
		{
			get { return _m_SECONDARY_NORMAL_MAPPING; }
			set { _m_SECONDARY_NORMAL_MAPPING = value; this.Invalidate(); }
		}
		
		private string _m_SECONDARY_NORMAL_MAP;
		[STNodeProperty("SECONDARY_NORMAL_MAP", "SECONDARY_NORMAL_MAP")]
		public string m_SECONDARY_NORMAL_MAP
		{
			get { return _m_SECONDARY_NORMAL_MAP; }
			set { _m_SECONDARY_NORMAL_MAP = value; this.Invalidate(); }
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
		
		private float _m_SECONDARY_REFRACTFACTOR;
		[STNodeProperty("SECONDARY_REFRACTFACTOR", "SECONDARY_REFRACTFACTOR")]
		public float m_SECONDARY_REFRACTFACTOR
		{
			get { return _m_SECONDARY_REFRACTFACTOR; }
			set { _m_SECONDARY_REFRACTFACTOR = value; this.Invalidate(); }
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
		
		private bool _m_DISTORTION_OCCLUSION;
		[STNodeProperty("DISTORTION_OCCLUSION", "DISTORTION_OCCLUSION")]
		public bool m_DISTORTION_OCCLUSION
		{
			get { return _m_DISTORTION_OCCLUSION; }
			set { _m_DISTORTION_OCCLUSION = value; this.Invalidate(); }
		}
		
		private float _m_MIN_OCCLUSION_DISTANCE;
		[STNodeProperty("MIN_OCCLUSION_DISTANCE", "MIN_OCCLUSION_DISTANCE")]
		public float m_MIN_OCCLUSION_DISTANCE
		{
			get { return _m_MIN_OCCLUSION_DISTANCE; }
			set { _m_MIN_OCCLUSION_DISTANCE = value; this.Invalidate(); }
		}
		
		private bool _m_FLOW_UV_ANIMATION;
		[STNodeProperty("FLOW_UV_ANIMATION", "FLOW_UV_ANIMATION")]
		public bool m_FLOW_UV_ANIMATION
		{
			get { return _m_FLOW_UV_ANIMATION; }
			set { _m_FLOW_UV_ANIMATION = value; this.Invalidate(); }
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
			
			this.Title = "SimpleRefraction";
			
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
