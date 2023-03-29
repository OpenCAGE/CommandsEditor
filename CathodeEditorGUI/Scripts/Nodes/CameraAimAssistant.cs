#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CameraAimAssistant : STNode
	{
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private float _m_activation_radius;
		[STNodeProperty("activation_radius", "activation_radius")]
		public float m_activation_radius
		{
			get { return _m_activation_radius; }
			set { _m_activation_radius = value; this.Invalidate(); }
		}
		
		private float _m_inner_radius;
		[STNodeProperty("inner_radius", "inner_radius")]
		public float m_inner_radius
		{
			get { return _m_inner_radius; }
			set { _m_inner_radius = value; this.Invalidate(); }
		}
		
		private float _m_camera_speed_attenuation;
		[STNodeProperty("camera_speed_attenuation", "camera_speed_attenuation")]
		public float m_camera_speed_attenuation
		{
			get { return _m_camera_speed_attenuation; }
			set { _m_camera_speed_attenuation = value; this.Invalidate(); }
		}
		
		private float _m_min_activation_distance;
		[STNodeProperty("min_activation_distance", "min_activation_distance")]
		public float m_min_activation_distance
		{
			get { return _m_min_activation_distance; }
			set { _m_min_activation_distance = value; this.Invalidate(); }
		}
		
		private float _m_fading_range;
		[STNodeProperty("fading_range", "fading_range")]
		public float m_fading_range
		{
			get { return _m_fading_range; }
			set { _m_fading_range = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "CameraAimAssistant";
			
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
		}
	}
}
#endif
