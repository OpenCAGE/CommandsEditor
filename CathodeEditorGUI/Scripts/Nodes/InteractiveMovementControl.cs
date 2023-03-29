#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class InteractiveMovementControl : STNode
	{
		private bool _m_can_go_both_ways;
		[STNodeProperty("can_go_both_ways", "can_go_both_ways")]
		public bool m_can_go_both_ways
		{
			get { return _m_can_go_both_ways; }
			set { _m_can_go_both_ways = value; this.Invalidate(); }
		}
		
		private bool _m_use_left_input_stick;
		[STNodeProperty("use_left_input_stick", "use_left_input_stick")]
		public bool m_use_left_input_stick
		{
			get { return _m_use_left_input_stick; }
			set { _m_use_left_input_stick = value; this.Invalidate(); }
		}
		
		private float _m_base_progress_speed;
		[STNodeProperty("base_progress_speed", "base_progress_speed")]
		public float m_base_progress_speed
		{
			get { return _m_base_progress_speed; }
			set { _m_base_progress_speed = value; this.Invalidate(); }
		}
		
		private float _m_movement_threshold;
		[STNodeProperty("movement_threshold", "movement_threshold")]
		public float m_movement_threshold
		{
			get { return _m_movement_threshold; }
			set { _m_movement_threshold = value; this.Invalidate(); }
		}
		
		private float _m_momentum_damping;
		[STNodeProperty("momentum_damping", "momentum_damping")]
		public float m_momentum_damping
		{
			get { return _m_momentum_damping; }
			set { _m_momentum_damping = value; this.Invalidate(); }
		}
		
		private bool _m_track_bone_position;
		[STNodeProperty("track_bone_position", "track_bone_position")]
		public bool m_track_bone_position
		{
			get { return _m_track_bone_position; }
			set { _m_track_bone_position = value; this.Invalidate(); }
		}
		
		private string _m_character_node;
		[STNodeProperty("character_node", "character_node")]
		public string m_character_node
		{
			get { return _m_character_node; }
			set { _m_character_node = value; this.Invalidate(); }
		}
		
		private cTransform _m_track_position;
		[STNodeProperty("track_position", "track_position")]
		public cTransform m_track_position
		{
			get { return _m_track_position; }
			set { _m_track_position = value; this.Invalidate(); }
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
			
			this.Title = "InteractiveMovementControl";
			
			this.InputOptions.Add("duration", typeof(float), false);
			this.InputOptions.Add("start_time", typeof(float), false);
			this.InputOptions.Add("progress_path", typeof(string), false);
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("completed", typeof(void), false);
			this.OutputOptions.Add("result", typeof(float), false);
			this.OutputOptions.Add("speed", typeof(float), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
