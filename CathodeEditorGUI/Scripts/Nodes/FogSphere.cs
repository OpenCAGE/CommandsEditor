#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FogSphere : STNode
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
		
		private cVector3 _m_COLOUR_TINT;
		[STNodeProperty("COLOUR_TINT", "COLOUR_TINT")]
		public cVector3 m_COLOUR_TINT
		{
			get { return _m_COLOUR_TINT; }
			set { _m_COLOUR_TINT = value; this.Invalidate(); }
		}
		
		private float _m_INTENSITY;
		[STNodeProperty("INTENSITY", "INTENSITY")]
		public float m_INTENSITY
		{
			get { return _m_INTENSITY; }
			set { _m_INTENSITY = value; this.Invalidate(); }
		}
		
		private float _m_OPACITY;
		[STNodeProperty("OPACITY", "OPACITY")]
		public float m_OPACITY
		{
			get { return _m_OPACITY; }
			set { _m_OPACITY = value; this.Invalidate(); }
		}
		
		private bool _m_EARLY_ALPHA;
		[STNodeProperty("EARLY_ALPHA", "EARLY_ALPHA")]
		public bool m_EARLY_ALPHA
		{
			get { return _m_EARLY_ALPHA; }
			set { _m_EARLY_ALPHA = value; this.Invalidate(); }
		}
		
		private bool _m_LOW_RES_ALPHA;
		[STNodeProperty("LOW_RES_ALPHA", "LOW_RES_ALPHA")]
		public bool m_LOW_RES_ALPHA
		{
			get { return _m_LOW_RES_ALPHA; }
			set { _m_LOW_RES_ALPHA = value; this.Invalidate(); }
		}
		
		private bool _m_CONVEX_GEOM;
		[STNodeProperty("CONVEX_GEOM", "CONVEX_GEOM")]
		public bool m_CONVEX_GEOM
		{
			get { return _m_CONVEX_GEOM; }
			set { _m_CONVEX_GEOM = value; this.Invalidate(); }
		}
		
		private bool _m_DISABLE_SIZE_CULLING;
		[STNodeProperty("DISABLE_SIZE_CULLING", "DISABLE_SIZE_CULLING")]
		public bool m_DISABLE_SIZE_CULLING
		{
			get { return _m_DISABLE_SIZE_CULLING; }
			set { _m_DISABLE_SIZE_CULLING = value; this.Invalidate(); }
		}
		
		private bool _m_NO_CLIP;
		[STNodeProperty("NO_CLIP", "NO_CLIP")]
		public bool m_NO_CLIP
		{
			get { return _m_NO_CLIP; }
			set { _m_NO_CLIP = value; this.Invalidate(); }
		}
		
		private bool _m_ALPHA_LIGHTING;
		[STNodeProperty("ALPHA_LIGHTING", "ALPHA_LIGHTING")]
		public bool m_ALPHA_LIGHTING
		{
			get { return _m_ALPHA_LIGHTING; }
			set { _m_ALPHA_LIGHTING = value; this.Invalidate(); }
		}
		
		private bool _m_DYNAMIC_ALPHA_LIGHTING;
		[STNodeProperty("DYNAMIC_ALPHA_LIGHTING", "DYNAMIC_ALPHA_LIGHTING")]
		public bool m_DYNAMIC_ALPHA_LIGHTING
		{
			get { return _m_DYNAMIC_ALPHA_LIGHTING; }
			set { _m_DYNAMIC_ALPHA_LIGHTING = value; this.Invalidate(); }
		}
		
		private float _m_DENSITY;
		[STNodeProperty("DENSITY", "DENSITY")]
		public float m_DENSITY
		{
			get { return _m_DENSITY; }
			set { _m_DENSITY = value; this.Invalidate(); }
		}
		
		private bool _m_EXPONENTIAL_DENSITY;
		[STNodeProperty("EXPONENTIAL_DENSITY", "EXPONENTIAL_DENSITY")]
		public bool m_EXPONENTIAL_DENSITY
		{
			get { return _m_EXPONENTIAL_DENSITY; }
			set { _m_EXPONENTIAL_DENSITY = value; this.Invalidate(); }
		}
		
		private bool _m_SCENE_DEPENDANT_DENSITY;
		[STNodeProperty("SCENE_DEPENDANT_DENSITY", "SCENE_DEPENDANT_DENSITY")]
		public bool m_SCENE_DEPENDANT_DENSITY
		{
			get { return _m_SCENE_DEPENDANT_DENSITY; }
			set { _m_SCENE_DEPENDANT_DENSITY = value; this.Invalidate(); }
		}
		
		private bool _m_FRESNEL_TERM;
		[STNodeProperty("FRESNEL_TERM", "FRESNEL_TERM")]
		public bool m_FRESNEL_TERM
		{
			get { return _m_FRESNEL_TERM; }
			set { _m_FRESNEL_TERM = value; this.Invalidate(); }
		}
		
		private float _m_FRESNEL_POWER;
		[STNodeProperty("FRESNEL_POWER", "FRESNEL_POWER")]
		public float m_FRESNEL_POWER
		{
			get { return _m_FRESNEL_POWER; }
			set { _m_FRESNEL_POWER = value; this.Invalidate(); }
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
		
		private bool _m_BLEND_ALPHA_OVER_DISTANCE;
		[STNodeProperty("BLEND_ALPHA_OVER_DISTANCE", "BLEND_ALPHA_OVER_DISTANCE")]
		public bool m_BLEND_ALPHA_OVER_DISTANCE
		{
			get { return _m_BLEND_ALPHA_OVER_DISTANCE; }
			set { _m_BLEND_ALPHA_OVER_DISTANCE = value; this.Invalidate(); }
		}
		
		private float _m_FAR_BLEND_DISTANCE;
		[STNodeProperty("FAR_BLEND_DISTANCE", "FAR_BLEND_DISTANCE")]
		public float m_FAR_BLEND_DISTANCE
		{
			get { return _m_FAR_BLEND_DISTANCE; }
			set { _m_FAR_BLEND_DISTANCE = value; this.Invalidate(); }
		}
		
		private float _m_NEAR_BLEND_DISTANCE;
		[STNodeProperty("NEAR_BLEND_DISTANCE", "NEAR_BLEND_DISTANCE")]
		public float m_NEAR_BLEND_DISTANCE
		{
			get { return _m_NEAR_BLEND_DISTANCE; }
			set { _m_NEAR_BLEND_DISTANCE = value; this.Invalidate(); }
		}
		
		private bool _m_SECONDARY_BLEND_ALPHA_OVER_DISTANCE;
		[STNodeProperty("SECONDARY_BLEND_ALPHA_OVER_DISTANCE", "SECONDARY_BLEND_ALPHA_OVER_DISTANCE")]
		public bool m_SECONDARY_BLEND_ALPHA_OVER_DISTANCE
		{
			get { return _m_SECONDARY_BLEND_ALPHA_OVER_DISTANCE; }
			set { _m_SECONDARY_BLEND_ALPHA_OVER_DISTANCE = value; this.Invalidate(); }
		}
		
		private float _m_SECONDARY_FAR_BLEND_DISTANCE;
		[STNodeProperty("SECONDARY_FAR_BLEND_DISTANCE", "SECONDARY_FAR_BLEND_DISTANCE")]
		public float m_SECONDARY_FAR_BLEND_DISTANCE
		{
			get { return _m_SECONDARY_FAR_BLEND_DISTANCE; }
			set { _m_SECONDARY_FAR_BLEND_DISTANCE = value; this.Invalidate(); }
		}
		
		private float _m_SECONDARY_NEAR_BLEND_DISTANCE;
		[STNodeProperty("SECONDARY_NEAR_BLEND_DISTANCE", "SECONDARY_NEAR_BLEND_DISTANCE")]
		public float m_SECONDARY_NEAR_BLEND_DISTANCE
		{
			get { return _m_SECONDARY_NEAR_BLEND_DISTANCE; }
			set { _m_SECONDARY_NEAR_BLEND_DISTANCE = value; this.Invalidate(); }
		}
		
		private bool _m_DEPTH_INTERSECT_COLOUR;
		[STNodeProperty("DEPTH_INTERSECT_COLOUR", "DEPTH_INTERSECT_COLOUR")]
		public bool m_DEPTH_INTERSECT_COLOUR
		{
			get { return _m_DEPTH_INTERSECT_COLOUR; }
			set { _m_DEPTH_INTERSECT_COLOUR = value; this.Invalidate(); }
		}
		
		private cVector3 _m_DEPTH_INTERSECT_COLOUR_VALUE;
		[STNodeProperty("DEPTH_INTERSECT_COLOUR_VALUE", "DEPTH_INTERSECT_COLOUR_VALUE")]
		public cVector3 m_DEPTH_INTERSECT_COLOUR_VALUE
		{
			get { return _m_DEPTH_INTERSECT_COLOUR_VALUE; }
			set { _m_DEPTH_INTERSECT_COLOUR_VALUE = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_INTERSECT_ALPHA_VALUE;
		[STNodeProperty("DEPTH_INTERSECT_ALPHA_VALUE", "DEPTH_INTERSECT_ALPHA_VALUE")]
		public float m_DEPTH_INTERSECT_ALPHA_VALUE
		{
			get { return _m_DEPTH_INTERSECT_ALPHA_VALUE; }
			set { _m_DEPTH_INTERSECT_ALPHA_VALUE = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_INTERSECT_RANGE;
		[STNodeProperty("DEPTH_INTERSECT_RANGE", "DEPTH_INTERSECT_RANGE")]
		public float m_DEPTH_INTERSECT_RANGE
		{
			get { return _m_DEPTH_INTERSECT_RANGE; }
			set { _m_DEPTH_INTERSECT_RANGE = value; this.Invalidate(); }
		}
		
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private float _m_radius;
		[STNodeProperty("radius", "radius")]
		public float m_radius
		{
			get { return _m_radius; }
			set { _m_radius = value; this.Invalidate(); }
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
			
			this.Title = "FogSphere";
			
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
