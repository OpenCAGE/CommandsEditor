#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FixedCamera : STNode
	{
		private bool _m_use_transform_position;
		[STNodeProperty("use_transform_position", "use_transform_position")]
		public bool m_use_transform_position
		{
			get { return _m_use_transform_position; }
			set { _m_use_transform_position = value; this.Invalidate(); }
		}
		
		private cTransform _m_transform_position;
		[STNodeProperty("transform_position", "transform_position")]
		public cTransform m_transform_position
		{
			get { return _m_transform_position; }
			set { _m_transform_position = value; this.Invalidate(); }
		}
		
		private cVector3 _m_camera_position;
		[STNodeProperty("camera_position", "camera_position")]
		public cVector3 m_camera_position
		{
			get { return _m_camera_position; }
			set { _m_camera_position = value; this.Invalidate(); }
		}
		
		private cVector3 _m_camera_target;
		[STNodeProperty("camera_target", "camera_target")]
		public cVector3 m_camera_target
		{
			get { return _m_camera_target; }
			set { _m_camera_target = value; this.Invalidate(); }
		}
		
		private cVector3 _m_camera_position_offset;
		[STNodeProperty("camera_position_offset", "camera_position_offset")]
		public cVector3 m_camera_position_offset
		{
			get { return _m_camera_position_offset; }
			set { _m_camera_position_offset = value; this.Invalidate(); }
		}
		
		private cVector3 _m_camera_target_offset;
		[STNodeProperty("camera_target_offset", "camera_target_offset")]
		public cVector3 m_camera_target_offset
		{
			get { return _m_camera_target_offset; }
			set { _m_camera_target_offset = value; this.Invalidate(); }
		}
		
		private bool _m_apply_target;
		[STNodeProperty("apply_target", "apply_target")]
		public bool m_apply_target
		{
			get { return _m_apply_target; }
			set { _m_apply_target = value; this.Invalidate(); }
		}
		
		private bool _m_apply_position;
		[STNodeProperty("apply_position", "apply_position")]
		public bool m_apply_position
		{
			get { return _m_apply_position; }
			set { _m_apply_position = value; this.Invalidate(); }
		}
		
		private bool _m_use_target_offset;
		[STNodeProperty("use_target_offset", "use_target_offset")]
		public bool m_use_target_offset
		{
			get { return _m_use_target_offset; }
			set { _m_use_target_offset = value; this.Invalidate(); }
		}
		
		private bool _m_use_position_offset;
		[STNodeProperty("use_position_offset", "use_position_offset")]
		public bool m_use_position_offset
		{
			get { return _m_use_position_offset; }
			set { _m_use_position_offset = value; this.Invalidate(); }
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
			
			this.Title = "FixedCamera";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("linked_cameras", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("activate_behavior", typeof(void), false);
			this.InputOptions.Add("deactivate_behavior", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("behavior_activated", typeof(void), false);
			this.OutputOptions.Add("behavior_deactivated", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
