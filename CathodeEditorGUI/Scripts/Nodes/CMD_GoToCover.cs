using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_GoToCover : STNode
	{
		private string _m_move_type;
		[STNodeProperty("move_type", "move_type")]
		public string m_move_type
		{
			get { return _m_move_type; }
			set { _m_move_type = value; this.Invalidate(); }
		}
		
		private float _m_SearchRadius;
		[STNodeProperty("SearchRadius", "SearchRadius")]
		public float m_SearchRadius
		{
			get { return _m_SearchRadius; }
			set { _m_SearchRadius = value; this.Invalidate(); }
		}
		
		private bool _m_enable_lookaround;
		[STNodeProperty("enable_lookaround", "enable_lookaround")]
		public bool m_enable_lookaround
		{
			get { return _m_enable_lookaround; }
			set { _m_enable_lookaround = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private bool _m_continue_from_previous_move;
		[STNodeProperty("continue_from_previous_move", "continue_from_previous_move")]
		public bool m_continue_from_previous_move
		{
			get { return _m_continue_from_previous_move; }
			set { _m_continue_from_previous_move = value; this.Invalidate(); }
		}
		
		private bool _m_disallow_traversal;
		[STNodeProperty("disallow_traversal", "disallow_traversal")]
		public bool m_disallow_traversal
		{
			get { return _m_disallow_traversal; }
			set { _m_disallow_traversal = value; this.Invalidate(); }
		}
		
		private bool _m_should_be_aiming;
		[STNodeProperty("should_be_aiming", "should_be_aiming")]
		public bool m_should_be_aiming
		{
			get { return _m_should_be_aiming; }
			set { _m_should_be_aiming = value; this.Invalidate(); }
		}
		
		private bool _m_use_current_target_as_aim;
		[STNodeProperty("use_current_target_as_aim", "use_current_target_as_aim")]
		public bool m_use_current_target_as_aim
		{
			get { return _m_use_current_target_as_aim; }
			set { _m_use_current_target_as_aim = value; this.Invalidate(); }
		}
		
		private bool _m_override_all_ai;
		[STNodeProperty("override_all_ai", "override_all_ai")]
		public bool m_override_all_ai
		{
			get { return _m_override_all_ai; }
			set { _m_override_all_ai = value; this.Invalidate(); }
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
			
			this.Title = "CMD_GoToCover";
			
			this.InputOptions.Add("CoverPoint", typeof(cTransform), false);
			this.InputOptions.Add("AimTarget", typeof(STNode), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("succeeded", typeof(void), false);
			this.OutputOptions.Add("failed", typeof(void), false);
			this.OutputOptions.Add("entered_cover", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("command_started", typeof(void), false);
		}
	}
}
