#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FogBox : STNode
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
		
		private string _m_GEOMETRY_TYPE;
		[STNodeProperty("GEOMETRY_TYPE", "GEOMETRY_TYPE")]
		public string m_GEOMETRY_TYPE
		{
			get { return _m_GEOMETRY_TYPE; }
			set { _m_GEOMETRY_TYPE = value; this.Invalidate(); }
		}
		
		private cVector3 _m_COLOUR_TINT;
		[STNodeProperty("COLOUR_TINT", "COLOUR_TINT")]
		public cVector3 m_COLOUR_TINT
		{
			get { return _m_COLOUR_TINT; }
			set { _m_COLOUR_TINT = value; this.Invalidate(); }
		}
		
		private float _m_DISTANCE_FADE;
		[STNodeProperty("DISTANCE_FADE", "DISTANCE_FADE")]
		public float m_DISTANCE_FADE
		{
			get { return _m_DISTANCE_FADE; }
			set { _m_DISTANCE_FADE = value; this.Invalidate(); }
		}
		
		private float _m_ANGLE_FADE;
		[STNodeProperty("ANGLE_FADE", "ANGLE_FADE")]
		public float m_ANGLE_FADE
		{
			get { return _m_ANGLE_FADE; }
			set { _m_ANGLE_FADE = value; this.Invalidate(); }
		}
		
		private bool _m_BILLBOARD;
		[STNodeProperty("BILLBOARD", "BILLBOARD")]
		public bool m_BILLBOARD
		{
			get { return _m_BILLBOARD; }
			set { _m_BILLBOARD = value; this.Invalidate(); }
		}
		
		private bool _m_EARLY_ALPHA;
		[STNodeProperty("EARLY_ALPHA", "EARLY_ALPHA")]
		public bool m_EARLY_ALPHA
		{
			get { return _m_EARLY_ALPHA; }
			set { _m_EARLY_ALPHA = value; this.Invalidate(); }
		}
		
		private bool _m_LOW_RES;
		[STNodeProperty("LOW_RES", "LOW_RES")]
		public bool m_LOW_RES
		{
			get { return _m_LOW_RES; }
			set { _m_LOW_RES = value; this.Invalidate(); }
		}
		
		private bool _m_CONVEX_GEOM;
		[STNodeProperty("CONVEX_GEOM", "CONVEX_GEOM")]
		public bool m_CONVEX_GEOM
		{
			get { return _m_CONVEX_GEOM; }
			set { _m_CONVEX_GEOM = value; this.Invalidate(); }
		}
		
		private float _m_THICKNESS;
		[STNodeProperty("THICKNESS", "THICKNESS")]
		public float m_THICKNESS
		{
			get { return _m_THICKNESS; }
			set { _m_THICKNESS = value; this.Invalidate(); }
		}
		
		private bool _m_START_DISTANT_CLIP;
		[STNodeProperty("START_DISTANT_CLIP", "START_DISTANT_CLIP")]
		public bool m_START_DISTANT_CLIP
		{
			get { return _m_START_DISTANT_CLIP; }
			set { _m_START_DISTANT_CLIP = value; this.Invalidate(); }
		}
		
		private float _m_START_DISTANCE_FADE;
		[STNodeProperty("START_DISTANCE_FADE", "START_DISTANCE_FADE")]
		public float m_START_DISTANCE_FADE
		{
			get { return _m_START_DISTANCE_FADE; }
			set { _m_START_DISTANCE_FADE = value; this.Invalidate(); }
		}
		
		private bool _m_SOFTNESS;
		[STNodeProperty("SOFTNESS", "SOFTNESS")]
		public bool m_SOFTNESS
		{
			get { return _m_SOFTNESS; }
			set { _m_SOFTNESS = value; this.Invalidate(); }
		}
		
		private float _m_SOFTNESS_EDGE;
		[STNodeProperty("SOFTNESS_EDGE", "SOFTNESS_EDGE")]
		public float m_SOFTNESS_EDGE
		{
			get { return _m_SOFTNESS_EDGE; }
			set { _m_SOFTNESS_EDGE = value; this.Invalidate(); }
		}
		
		private bool _m_LINEAR_HEIGHT_DENSITY;
		[STNodeProperty("LINEAR_HEIGHT_DENSITY", "LINEAR_HEIGHT_DENSITY")]
		public bool m_LINEAR_HEIGHT_DENSITY
		{
			get { return _m_LINEAR_HEIGHT_DENSITY; }
			set { _m_LINEAR_HEIGHT_DENSITY = value; this.Invalidate(); }
		}
		
		private bool _m_SMOOTH_HEIGHT_DENSITY;
		[STNodeProperty("SMOOTH_HEIGHT_DENSITY", "SMOOTH_HEIGHT_DENSITY")]
		public bool m_SMOOTH_HEIGHT_DENSITY
		{
			get { return _m_SMOOTH_HEIGHT_DENSITY; }
			set { _m_SMOOTH_HEIGHT_DENSITY = value; this.Invalidate(); }
		}
		
		private float _m_HEIGHT_MAX_DENSITY;
		[STNodeProperty("HEIGHT_MAX_DENSITY", "HEIGHT_MAX_DENSITY")]
		public float m_HEIGHT_MAX_DENSITY
		{
			get { return _m_HEIGHT_MAX_DENSITY; }
			set { _m_HEIGHT_MAX_DENSITY = value; this.Invalidate(); }
		}
		
		private bool _m_FRESNEL_FALLOFF;
		[STNodeProperty("FRESNEL_FALLOFF", "FRESNEL_FALLOFF")]
		public bool m_FRESNEL_FALLOFF
		{
			get { return _m_FRESNEL_FALLOFF; }
			set { _m_FRESNEL_FALLOFF = value; this.Invalidate(); }
		}
		
		private float _m_FRESNEL_POWER;
		[STNodeProperty("FRESNEL_POWER", "FRESNEL_POWER")]
		public float m_FRESNEL_POWER
		{
			get { return _m_FRESNEL_POWER; }
			set { _m_FRESNEL_POWER = value; this.Invalidate(); }
		}
		
		private bool _m_DEPTH_INTERSECT_COLOUR;
		[STNodeProperty("DEPTH_INTERSECT_COLOUR", "DEPTH_INTERSECT_COLOUR")]
		public bool m_DEPTH_INTERSECT_COLOUR
		{
			get { return _m_DEPTH_INTERSECT_COLOUR; }
			set { _m_DEPTH_INTERSECT_COLOUR = value; this.Invalidate(); }
		}
		
		private cVector3 _m_DEPTH_INTERSECT_INITIAL_COLOUR;
		[STNodeProperty("DEPTH_INTERSECT_INITIAL_COLOUR", "DEPTH_INTERSECT_INITIAL_COLOUR")]
		public cVector3 m_DEPTH_INTERSECT_INITIAL_COLOUR
		{
			get { return _m_DEPTH_INTERSECT_INITIAL_COLOUR; }
			set { _m_DEPTH_INTERSECT_INITIAL_COLOUR = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_INTERSECT_INITIAL_ALPHA;
		[STNodeProperty("DEPTH_INTERSECT_INITIAL_ALPHA", "DEPTH_INTERSECT_INITIAL_ALPHA")]
		public float m_DEPTH_INTERSECT_INITIAL_ALPHA
		{
			get { return _m_DEPTH_INTERSECT_INITIAL_ALPHA; }
			set { _m_DEPTH_INTERSECT_INITIAL_ALPHA = value; this.Invalidate(); }
		}
		
		private cVector3 _m_DEPTH_INTERSECT_MIDPOINT_COLOUR;
		[STNodeProperty("DEPTH_INTERSECT_MIDPOINT_COLOUR", "DEPTH_INTERSECT_MIDPOINT_COLOUR")]
		public cVector3 m_DEPTH_INTERSECT_MIDPOINT_COLOUR
		{
			get { return _m_DEPTH_INTERSECT_MIDPOINT_COLOUR; }
			set { _m_DEPTH_INTERSECT_MIDPOINT_COLOUR = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_INTERSECT_MIDPOINT_ALPHA;
		[STNodeProperty("DEPTH_INTERSECT_MIDPOINT_ALPHA", "DEPTH_INTERSECT_MIDPOINT_ALPHA")]
		public float m_DEPTH_INTERSECT_MIDPOINT_ALPHA
		{
			get { return _m_DEPTH_INTERSECT_MIDPOINT_ALPHA; }
			set { _m_DEPTH_INTERSECT_MIDPOINT_ALPHA = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_INTERSECT_MIDPOINT_DEPTH;
		[STNodeProperty("DEPTH_INTERSECT_MIDPOINT_DEPTH", "DEPTH_INTERSECT_MIDPOINT_DEPTH")]
		public float m_DEPTH_INTERSECT_MIDPOINT_DEPTH
		{
			get { return _m_DEPTH_INTERSECT_MIDPOINT_DEPTH; }
			set { _m_DEPTH_INTERSECT_MIDPOINT_DEPTH = value; this.Invalidate(); }
		}
		
		private cVector3 _m_DEPTH_INTERSECT_END_COLOUR;
		[STNodeProperty("DEPTH_INTERSECT_END_COLOUR", "DEPTH_INTERSECT_END_COLOUR")]
		public cVector3 m_DEPTH_INTERSECT_END_COLOUR
		{
			get { return _m_DEPTH_INTERSECT_END_COLOUR; }
			set { _m_DEPTH_INTERSECT_END_COLOUR = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_INTERSECT_END_ALPHA;
		[STNodeProperty("DEPTH_INTERSECT_END_ALPHA", "DEPTH_INTERSECT_END_ALPHA")]
		public float m_DEPTH_INTERSECT_END_ALPHA
		{
			get { return _m_DEPTH_INTERSECT_END_ALPHA; }
			set { _m_DEPTH_INTERSECT_END_ALPHA = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_INTERSECT_END_DEPTH;
		[STNodeProperty("DEPTH_INTERSECT_END_DEPTH", "DEPTH_INTERSECT_END_DEPTH")]
		public float m_DEPTH_INTERSECT_END_DEPTH
		{
			get { return _m_DEPTH_INTERSECT_END_DEPTH; }
			set { _m_DEPTH_INTERSECT_END_DEPTH = value; this.Invalidate(); }
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
			
			this.Title = "FogBox";
			
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
#endif
