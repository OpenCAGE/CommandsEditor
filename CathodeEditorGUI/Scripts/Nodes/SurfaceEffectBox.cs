using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SurfaceEffectBox : STNode
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
		
		private cVector3 _m_COLOUR_TINT_OUTER;
		[STNodeProperty("COLOUR_TINT_OUTER", "COLOUR_TINT_OUTER")]
		public cVector3 m_COLOUR_TINT_OUTER
		{
			get { return _m_COLOUR_TINT_OUTER; }
			set { _m_COLOUR_TINT_OUTER = value; this.Invalidate(); }
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
		
		private float _m_FADE_OUT_TIME;
		[STNodeProperty("FADE_OUT_TIME", "FADE_OUT_TIME")]
		public float m_FADE_OUT_TIME
		{
			get { return _m_FADE_OUT_TIME; }
			set { _m_FADE_OUT_TIME = value; this.Invalidate(); }
		}
		
		private float _m_SURFACE_WRAP;
		[STNodeProperty("SURFACE_WRAP", "SURFACE_WRAP")]
		public float m_SURFACE_WRAP
		{
			get { return _m_SURFACE_WRAP; }
			set { _m_SURFACE_WRAP = value; this.Invalidate(); }
		}
		
		private float _m_ROUGHNESS_SCALE;
		[STNodeProperty("ROUGHNESS_SCALE", "ROUGHNESS_SCALE")]
		public float m_ROUGHNESS_SCALE
		{
			get { return _m_ROUGHNESS_SCALE; }
			set { _m_ROUGHNESS_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_SPARKLE_SCALE;
		[STNodeProperty("SPARKLE_SCALE", "SPARKLE_SCALE")]
		public float m_SPARKLE_SCALE
		{
			get { return _m_SPARKLE_SCALE; }
			set { _m_SPARKLE_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_METAL_STYLE_REFLECTIONS;
		[STNodeProperty("METAL_STYLE_REFLECTIONS", "METAL_STYLE_REFLECTIONS")]
		public float m_METAL_STYLE_REFLECTIONS
		{
			get { return _m_METAL_STYLE_REFLECTIONS; }
			set { _m_METAL_STYLE_REFLECTIONS = value; this.Invalidate(); }
		}
		
		private float _m_SHININESS_OPACITY;
		[STNodeProperty("SHININESS_OPACITY", "SHININESS_OPACITY")]
		public float m_SHININESS_OPACITY
		{
			get { return _m_SHININESS_OPACITY; }
			set { _m_SHININESS_OPACITY = value; this.Invalidate(); }
		}
		
		private float _m_TILING_ZY;
		[STNodeProperty("TILING_ZY", "TILING_ZY")]
		public float m_TILING_ZY
		{
			get { return _m_TILING_ZY; }
			set { _m_TILING_ZY = value; this.Invalidate(); }
		}
		
		private float _m_TILING_ZX;
		[STNodeProperty("TILING_ZX", "TILING_ZX")]
		public float m_TILING_ZX
		{
			get { return _m_TILING_ZX; }
			set { _m_TILING_ZX = value; this.Invalidate(); }
		}
		
		private float _m_TILING_XY;
		[STNodeProperty("TILING_XY", "TILING_XY")]
		public float m_TILING_XY
		{
			get { return _m_TILING_XY; }
			set { _m_TILING_XY = value; this.Invalidate(); }
		}
		
		private cVector3 _m_FALLOFF;
		[STNodeProperty("FALLOFF", "FALLOFF")]
		public cVector3 m_FALLOFF
		{
			get { return _m_FALLOFF; }
			set { _m_FALLOFF = value; this.Invalidate(); }
		}
		
		private bool _m_WS_LOCKED;
		[STNodeProperty("WS_LOCKED", "WS_LOCKED")]
		public bool m_WS_LOCKED
		{
			get { return _m_WS_LOCKED; }
			set { _m_WS_LOCKED = value; this.Invalidate(); }
		}
		
		private string _m_TEXTURE_MAP;
		[STNodeProperty("TEXTURE_MAP", "TEXTURE_MAP")]
		public string m_TEXTURE_MAP
		{
			get { return _m_TEXTURE_MAP; }
			set { _m_TEXTURE_MAP = value; this.Invalidate(); }
		}
		
		private string _m_SPARKLE_MAP;
		[STNodeProperty("SPARKLE_MAP", "SPARKLE_MAP")]
		public string m_SPARKLE_MAP
		{
			get { return _m_SPARKLE_MAP; }
			set { _m_SPARKLE_MAP = value; this.Invalidate(); }
		}
		
		private bool _m_ENVMAP;
		[STNodeProperty("ENVMAP", "ENVMAP")]
		public bool m_ENVMAP
		{
			get { return _m_ENVMAP; }
			set { _m_ENVMAP = value; this.Invalidate(); }
		}
		
		private string _m_ENVIRONMENT_MAP;
		[STNodeProperty("ENVIRONMENT_MAP", "ENVIRONMENT_MAP")]
		public string m_ENVIRONMENT_MAP
		{
			get { return _m_ENVIRONMENT_MAP; }
			set { _m_ENVIRONMENT_MAP = value; this.Invalidate(); }
		}
		
		private float _m_ENVMAP_PERCENT_EMISSIVE;
		[STNodeProperty("ENVMAP_PERCENT_EMISSIVE", "ENVMAP_PERCENT_EMISSIVE")]
		public float m_ENVMAP_PERCENT_EMISSIVE
		{
			get { return _m_ENVMAP_PERCENT_EMISSIVE; }
			set { _m_ENVMAP_PERCENT_EMISSIVE = value; this.Invalidate(); }
		}
		
		private bool _m_SPHERE;
		[STNodeProperty("SPHERE", "SPHERE")]
		public bool m_SPHERE
		{
			get { return _m_SPHERE; }
			set { _m_SPHERE = value; this.Invalidate(); }
		}
		
		private bool _m_BOX;
		[STNodeProperty("BOX", "BOX")]
		public bool m_BOX
		{
			get { return _m_BOX; }
			set { _m_BOX = value; this.Invalidate(); }
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
			
			this.Title = "SurfaceEffectBox";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("fade_out", typeof(void), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("faded_out", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("event", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
