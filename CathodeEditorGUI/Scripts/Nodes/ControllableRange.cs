#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ControllableRange : STNode
	{
		private float _m_min_range_x;
		[STNodeProperty("min_range_x", "min_range_x")]
		public float m_min_range_x
		{
			get { return _m_min_range_x; }
			set { _m_min_range_x = value; this.Invalidate(); }
		}
		
		private float _m_max_range_x;
		[STNodeProperty("max_range_x", "max_range_x")]
		public float m_max_range_x
		{
			get { return _m_max_range_x; }
			set { _m_max_range_x = value; this.Invalidate(); }
		}
		
		private float _m_min_range_y;
		[STNodeProperty("min_range_y", "min_range_y")]
		public float m_min_range_y
		{
			get { return _m_min_range_y; }
			set { _m_min_range_y = value; this.Invalidate(); }
		}
		
		private float _m_max_range_y;
		[STNodeProperty("max_range_y", "max_range_y")]
		public float m_max_range_y
		{
			get { return _m_max_range_y; }
			set { _m_max_range_y = value; this.Invalidate(); }
		}
		
		private float _m_min_feather_range_x;
		[STNodeProperty("min_feather_range_x", "min_feather_range_x")]
		public float m_min_feather_range_x
		{
			get { return _m_min_feather_range_x; }
			set { _m_min_feather_range_x = value; this.Invalidate(); }
		}
		
		private float _m_max_feather_range_x;
		[STNodeProperty("max_feather_range_x", "max_feather_range_x")]
		public float m_max_feather_range_x
		{
			get { return _m_max_feather_range_x; }
			set { _m_max_feather_range_x = value; this.Invalidate(); }
		}
		
		private float _m_min_feather_range_y;
		[STNodeProperty("min_feather_range_y", "min_feather_range_y")]
		public float m_min_feather_range_y
		{
			get { return _m_min_feather_range_y; }
			set { _m_min_feather_range_y = value; this.Invalidate(); }
		}
		
		private float _m_max_feather_range_y;
		[STNodeProperty("max_feather_range_y", "max_feather_range_y")]
		public float m_max_feather_range_y
		{
			get { return _m_max_feather_range_y; }
			set { _m_max_feather_range_y = value; this.Invalidate(); }
		}
		
		private float _m_speed_x;
		[STNodeProperty("speed_x", "speed_x")]
		public float m_speed_x
		{
			get { return _m_speed_x; }
			set { _m_speed_x = value; this.Invalidate(); }
		}
		
		private float _m_speed_y;
		[STNodeProperty("speed_y", "speed_y")]
		public float m_speed_y
		{
			get { return _m_speed_y; }
			set { _m_speed_y = value; this.Invalidate(); }
		}
		
		private float _m_damping_x;
		[STNodeProperty("damping_x", "damping_x")]
		public float m_damping_x
		{
			get { return _m_damping_x; }
			set { _m_damping_x = value; this.Invalidate(); }
		}
		
		private float _m_damping_y;
		[STNodeProperty("damping_y", "damping_y")]
		public float m_damping_y
		{
			get { return _m_damping_y; }
			set { _m_damping_y = value; this.Invalidate(); }
		}
		
		private float _m_mouse_speed_x;
		[STNodeProperty("mouse_speed_x", "mouse_speed_x")]
		public float m_mouse_speed_x
		{
			get { return _m_mouse_speed_x; }
			set { _m_mouse_speed_x = value; this.Invalidate(); }
		}
		
		private float _m_mouse_speed_y;
		[STNodeProperty("mouse_speed_y", "mouse_speed_y")]
		public float m_mouse_speed_y
		{
			get { return _m_mouse_speed_y; }
			set { _m_mouse_speed_y = value; this.Invalidate(); }
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
			
			this.Title = "ControllableRange";
			
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
#endif
