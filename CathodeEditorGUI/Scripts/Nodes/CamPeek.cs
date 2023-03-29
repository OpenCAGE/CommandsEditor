#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CamPeek : STNode
	{
		private float _m_range_left;
		[STNodeProperty("range_left", "range_left")]
		public float m_range_left
		{
			get { return _m_range_left; }
			set { _m_range_left = value; this.Invalidate(); }
		}
		
		private float _m_range_right;
		[STNodeProperty("range_right", "range_right")]
		public float m_range_right
		{
			get { return _m_range_right; }
			set { _m_range_right = value; this.Invalidate(); }
		}
		
		private float _m_range_up;
		[STNodeProperty("range_up", "range_up")]
		public float m_range_up
		{
			get { return _m_range_up; }
			set { _m_range_up = value; this.Invalidate(); }
		}
		
		private float _m_range_down;
		[STNodeProperty("range_down", "range_down")]
		public float m_range_down
		{
			get { return _m_range_down; }
			set { _m_range_down = value; this.Invalidate(); }
		}
		
		private float _m_range_forward;
		[STNodeProperty("range_forward", "range_forward")]
		public float m_range_forward
		{
			get { return _m_range_forward; }
			set { _m_range_forward = value; this.Invalidate(); }
		}
		
		private float _m_range_backward;
		[STNodeProperty("range_backward", "range_backward")]
		public float m_range_backward
		{
			get { return _m_range_backward; }
			set { _m_range_backward = value; this.Invalidate(); }
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
		
		private float _m_focal_distance;
		[STNodeProperty("focal_distance", "focal_distance")]
		public float m_focal_distance
		{
			get { return _m_focal_distance; }
			set { _m_focal_distance = value; this.Invalidate(); }
		}
		
		private float _m_focal_distance_y;
		[STNodeProperty("focal_distance_y", "focal_distance_y")]
		public float m_focal_distance_y
		{
			get { return _m_focal_distance_y; }
			set { _m_focal_distance_y = value; this.Invalidate(); }
		}
		
		private float _m_roll_factor;
		[STNodeProperty("roll_factor", "roll_factor")]
		public float m_roll_factor
		{
			get { return _m_roll_factor; }
			set { _m_roll_factor = value; this.Invalidate(); }
		}
		
		private bool _m_use_ik_solver;
		[STNodeProperty("use_ik_solver", "use_ik_solver")]
		public bool m_use_ik_solver
		{
			get { return _m_use_ik_solver; }
			set { _m_use_ik_solver = value; this.Invalidate(); }
		}
		
		private bool _m_use_horizontal_plane;
		[STNodeProperty("use_horizontal_plane", "use_horizontal_plane")]
		public bool m_use_horizontal_plane
		{
			get { return _m_use_horizontal_plane; }
			set { _m_use_horizontal_plane = value; this.Invalidate(); }
		}
		
		private string _m_stick;
		[STNodeProperty("stick", "stick")]
		public string m_stick
		{
			get { return _m_stick; }
			set { _m_stick = value; this.Invalidate(); }
		}
		
		private bool _m_disable_collision_test;
		[STNodeProperty("disable_collision_test", "disable_collision_test")]
		public bool m_disable_collision_test
		{
			get { return _m_disable_collision_test; }
			set { _m_disable_collision_test = value; this.Invalidate(); }
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
			
			this.Title = "CamPeek";
			
			this.InputOptions.Add("linked_cameras", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("activate_behavior", typeof(void), false);
			this.InputOptions.Add("deactivate_behavior", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("pos", typeof(cTransform), false);
			this.OutputOptions.Add("x_ratio", typeof(float), false);
			this.OutputOptions.Add("y_ratio", typeof(float), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("behavior_activated", typeof(void), false);
			this.OutputOptions.Add("behavior_deactivated", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
