using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class HandCamera : STNode
	{
		private string _m_noise_type;
		[STNodeProperty("noise_type", "noise_type")]
		public string m_noise_type
		{
			get { return _m_noise_type; }
			set { _m_noise_type = value; this.Invalidate(); }
		}
		
		private cVector3 _m_frequency;
		[STNodeProperty("frequency", "frequency")]
		public cVector3 m_frequency
		{
			get { return _m_frequency; }
			set { _m_frequency = value; this.Invalidate(); }
		}
		
		private cVector3 _m_damping;
		[STNodeProperty("damping", "damping")]
		public cVector3 m_damping
		{
			get { return _m_damping; }
			set { _m_damping = value; this.Invalidate(); }
		}
		
		private cVector3 _m_rotation_intensity;
		[STNodeProperty("rotation_intensity", "rotation_intensity")]
		public cVector3 m_rotation_intensity
		{
			get { return _m_rotation_intensity; }
			set { _m_rotation_intensity = value; this.Invalidate(); }
		}
		
		private float _m_min_fov_range;
		[STNodeProperty("min_fov_range", "min_fov_range")]
		public float m_min_fov_range
		{
			get { return _m_min_fov_range; }
			set { _m_min_fov_range = value; this.Invalidate(); }
		}
		
		private float _m_max_fov_range;
		[STNodeProperty("max_fov_range", "max_fov_range")]
		public float m_max_fov_range
		{
			get { return _m_max_fov_range; }
			set { _m_max_fov_range = value; this.Invalidate(); }
		}
		
		private float _m_min_noise;
		[STNodeProperty("min_noise", "min_noise")]
		public float m_min_noise
		{
			get { return _m_min_noise; }
			set { _m_min_noise = value; this.Invalidate(); }
		}
		
		private float _m_max_noise;
		[STNodeProperty("max_noise", "max_noise")]
		public float m_max_noise
		{
			get { return _m_max_noise; }
			set { _m_max_noise = value; this.Invalidate(); }
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
		
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_behavior_name;
		[STNodeProperty("behavior_name", "behavior_name")]
		public string m_behavior_name
		{
			get { return _m_behavior_name; }
			set { _m_behavior_name = value; this.Invalidate(); }
		}
		
		private int _m_priority;
		[STNodeProperty("priority", "priority")]
		public int m_priority
		{
			get { return _m_priority; }
			set { _m_priority = value; this.Invalidate(); }
		}
		
		private float _m_threshold;
		[STNodeProperty("threshold", "threshold")]
		public float m_threshold
		{
			get { return _m_threshold; }
			set { _m_threshold = value; this.Invalidate(); }
		}
		
		private float _m_blend_in;
		[STNodeProperty("blend_in", "blend_in")]
		public float m_blend_in
		{
			get { return _m_blend_in; }
			set { _m_blend_in = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private float _m_blend_out;
		[STNodeProperty("blend_out", "blend_out")]
		public float m_blend_out
		{
			get { return _m_blend_out; }
			set { _m_blend_out = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "HandCamera";
			
			this.InputOptions.Add("linked_cameras", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("activate_behavior", typeof(void), false);
			this.InputOptions.Add("deactivate_behavior", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("behavior_activated", typeof(void), false);
			this.OutputOptions.Add("behavior_deactivated", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
