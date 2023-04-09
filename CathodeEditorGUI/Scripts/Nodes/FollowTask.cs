using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FollowTask : STNode
	{
		private bool _m_can_initially_end_early;
		[STNodeProperty("can_initially_end_early", "can_initially_end_early")]
		public bool m_can_initially_end_early
		{
			get { return _m_can_initially_end_early; }
			set { _m_can_initially_end_early = value; this.Invalidate(); }
		}
		
		private float _m_stop_radius;
		[STNodeProperty("stop_radius", "stop_radius")]
		public float m_stop_radius
		{
			get { return _m_stop_radius; }
			set { _m_stop_radius = value; this.Invalidate(); }
		}
		
		private bool _m_should_auto_move_to_position;
		[STNodeProperty("should_auto_move_to_position", "should_auto_move_to_position")]
		public bool m_should_auto_move_to_position
		{
			get { return _m_should_auto_move_to_position; }
			set { _m_should_auto_move_to_position = value; this.Invalidate(); }
		}
		
		private bool _m_ignored_for_auto_selection;
		[STNodeProperty("ignored_for_auto_selection", "ignored_for_auto_selection")]
		public bool m_ignored_for_auto_selection
		{
			get { return _m_ignored_for_auto_selection; }
			set { _m_ignored_for_auto_selection = value; this.Invalidate(); }
		}
		
		private bool _m_has_pre_move_script;
		[STNodeProperty("has_pre_move_script", "has_pre_move_script")]
		public bool m_has_pre_move_script
		{
			get { return _m_has_pre_move_script; }
			set { _m_has_pre_move_script = value; this.Invalidate(); }
		}
		
		private bool _m_has_interrupt_script;
		[STNodeProperty("has_interrupt_script", "has_interrupt_script")]
		public bool m_has_interrupt_script
		{
			get { return _m_has_interrupt_script; }
			set { _m_has_interrupt_script = value; this.Invalidate(); }
		}
		
		private string _m_filter_options;
		[STNodeProperty("filter_options", "filter_options")]
		public string m_filter_options
		{
			get { return _m_filter_options; }
			set { _m_filter_options = value; this.Invalidate(); }
		}
		
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_should_stop_moving_when_reached;
		[STNodeProperty("should_stop_moving_when_reached", "should_stop_moving_when_reached")]
		public bool m_should_stop_moving_when_reached
		{
			get { return _m_should_stop_moving_when_reached; }
			set { _m_should_stop_moving_when_reached = value; this.Invalidate(); }
		}
		
		private bool _m_should_orientate_when_reached;
		[STNodeProperty("should_orientate_when_reached", "should_orientate_when_reached")]
		public bool m_should_orientate_when_reached
		{
			get { return _m_should_orientate_when_reached; }
			set { _m_should_orientate_when_reached = value; this.Invalidate(); }
		}
		
		private float _m_reached_distance_threshold;
		[STNodeProperty("reached_distance_threshold", "reached_distance_threshold")]
		public float m_reached_distance_threshold
		{
			get { return _m_reached_distance_threshold; }
			set { _m_reached_distance_threshold = value; this.Invalidate(); }
		}
		
		private string _m_selection_priority;
		[STNodeProperty("selection_priority", "selection_priority")]
		public string m_selection_priority
		{
			get { return _m_selection_priority; }
			set { _m_selection_priority = value; this.Invalidate(); }
		}
		
		private float _m_timeout;
		[STNodeProperty("timeout", "timeout")]
		public float m_timeout
		{
			get { return _m_timeout; }
			set { _m_timeout = value; this.Invalidate(); }
		}
		
		private bool _m_always_on_tracker;
		[STNodeProperty("always_on_tracker", "always_on_tracker")]
		public bool m_always_on_tracker
		{
			get { return _m_always_on_tracker; }
			set { _m_always_on_tracker = value; this.Invalidate(); }
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
			
			this.Title = "FollowTask";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("allow_early_end", typeof(void), false);
			this.InputOptions.Add("set_as_next_task", typeof(void), false);
			this.InputOptions.Add("completed_pre_move", typeof(void), false);
			this.InputOptions.Add("completed_interrupt", typeof(void), false);
			this.InputOptions.Add("task_end", typeof(void), false);
			this.InputOptions.Add("specific_character", typeof(STNode), false);
			this.InputOptions.Add("Job", typeof(STNode), false);
			this.InputOptions.Add("TaskPosition", typeof(cTransform), false);
			this.InputOptions.Add("filter", typeof(bool), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("on_early_end_allowed", typeof(void), false);
			this.OutputOptions.Add("task_set_as_next", typeof(void), false);
			this.OutputOptions.Add("on_pre_move_completed", typeof(void), false);
			this.OutputOptions.Add("on_completed_interrupt", typeof(void), false);
			this.OutputOptions.Add("task_ended", typeof(void), false);
			this.OutputOptions.Add("start_pre_move", typeof(void), false);
			this.OutputOptions.Add("start_interrupt", typeof(void), false);
			this.OutputOptions.Add("interrupted_while_moving", typeof(void), false);
			this.OutputOptions.Add("start_command", typeof(void), false);
			this.OutputOptions.Add("selected_by_npc", typeof(void), false);
			this.OutputOptions.Add("clean_up", typeof(void), false);
		}
	}
}
