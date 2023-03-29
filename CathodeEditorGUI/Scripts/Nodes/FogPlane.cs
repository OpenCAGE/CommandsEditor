#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FogPlane : STNode
	{
		private float _m_start_distance_fade_scalar;
		[STNodeProperty("start_distance_fade_scalar", "start_distance_fade_scalar")]
		public float m_start_distance_fade_scalar
		{
			get { return _m_start_distance_fade_scalar; }
			set { _m_start_distance_fade_scalar = value; this.Invalidate(); }
		}
		
		private float _m_distance_fade_scalar;
		[STNodeProperty("distance_fade_scalar", "distance_fade_scalar")]
		public float m_distance_fade_scalar
		{
			get { return _m_distance_fade_scalar; }
			set { _m_distance_fade_scalar = value; this.Invalidate(); }
		}
		
		private float _m_angle_fade_scalar;
		[STNodeProperty("angle_fade_scalar", "angle_fade_scalar")]
		public float m_angle_fade_scalar
		{
			get { return _m_angle_fade_scalar; }
			set { _m_angle_fade_scalar = value; this.Invalidate(); }
		}
		
		private float _m_linear_height_density_fresnel_power_scalar;
		[STNodeProperty("linear_height_density_fresnel_power_scalar", "linear_height_density_fresnel_power_scalar")]
		public float m_linear_height_density_fresnel_power_scalar
		{
			get { return _m_linear_height_density_fresnel_power_scalar; }
			set { _m_linear_height_density_fresnel_power_scalar = value; this.Invalidate(); }
		}
		
		private float _m_linear_heigth_density_max_scalar;
		[STNodeProperty("linear_heigth_density_max_scalar", "linear_heigth_density_max_scalar")]
		public float m_linear_heigth_density_max_scalar
		{
			get { return _m_linear_heigth_density_max_scalar; }
			set { _m_linear_heigth_density_max_scalar = value; this.Invalidate(); }
		}
		
		private cVector3 _m_tint;
		[STNodeProperty("tint", "tint")]
		public cVector3 m_tint
		{
			get { return _m_tint; }
			set { _m_tint = value; this.Invalidate(); }
		}
		
		private float _m_thickness_scalar;
		[STNodeProperty("thickness_scalar", "thickness_scalar")]
		public float m_thickness_scalar
		{
			get { return _m_thickness_scalar; }
			set { _m_thickness_scalar = value; this.Invalidate(); }
		}
		
		private float _m_edge_softness_scalar;
		[STNodeProperty("edge_softness_scalar", "edge_softness_scalar")]
		public float m_edge_softness_scalar
		{
			get { return _m_edge_softness_scalar; }
			set { _m_edge_softness_scalar = value; this.Invalidate(); }
		}
		
		private float _m_diffuse_0_uv_scalar;
		[STNodeProperty("diffuse_0_uv_scalar", "diffuse_0_uv_scalar")]
		public float m_diffuse_0_uv_scalar
		{
			get { return _m_diffuse_0_uv_scalar; }
			set { _m_diffuse_0_uv_scalar = value; this.Invalidate(); }
		}
		
		private float _m_diffuse_0_speed_scalar;
		[STNodeProperty("diffuse_0_speed_scalar", "diffuse_0_speed_scalar")]
		public float m_diffuse_0_speed_scalar
		{
			get { return _m_diffuse_0_speed_scalar; }
			set { _m_diffuse_0_speed_scalar = value; this.Invalidate(); }
		}
		
		private float _m_diffuse_1_uv_scalar;
		[STNodeProperty("diffuse_1_uv_scalar", "diffuse_1_uv_scalar")]
		public float m_diffuse_1_uv_scalar
		{
			get { return _m_diffuse_1_uv_scalar; }
			set { _m_diffuse_1_uv_scalar = value; this.Invalidate(); }
		}
		
		private float _m_diffuse_1_speed_scalar;
		[STNodeProperty("diffuse_1_speed_scalar", "diffuse_1_speed_scalar")]
		public float m_diffuse_1_speed_scalar
		{
			get { return _m_diffuse_1_speed_scalar; }
			set { _m_diffuse_1_speed_scalar = value; this.Invalidate(); }
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
			
			this.Title = "FogPlane";
			
			this.InputOptions.Add("fog_plane_resource", typeof(STNode), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
