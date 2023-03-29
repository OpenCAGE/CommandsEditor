#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_FollowUsingJobs : STNode
	{
		private string _m_fastest_allowed_move_type;
		[STNodeProperty("fastest_allowed_move_type", "fastest_allowed_move_type")]
		public string m_fastest_allowed_move_type
		{
			get { return _m_fastest_allowed_move_type; }
			set { _m_fastest_allowed_move_type = value; this.Invalidate(); }
		}
		
		private string _m_slowest_allowed_move_type;
		[STNodeProperty("slowest_allowed_move_type", "slowest_allowed_move_type")]
		public string m_slowest_allowed_move_type
		{
			get { return _m_slowest_allowed_move_type; }
			set { _m_slowest_allowed_move_type = value; this.Invalidate(); }
		}
		
		private float _m_centre_job_restart_radius;
		[STNodeProperty("centre_job_restart_radius", "centre_job_restart_radius")]
		public float m_centre_job_restart_radius
		{
			get { return _m_centre_job_restart_radius; }
			set { _m_centre_job_restart_radius = value; this.Invalidate(); }
		}
		
		private float _m_inner_radius;
		[STNodeProperty("inner_radius", "inner_radius")]
		public float m_inner_radius
		{
			get { return _m_inner_radius; }
			set { _m_inner_radius = value; this.Invalidate(); }
		}
		
		private float _m_outer_radius;
		[STNodeProperty("outer_radius", "outer_radius")]
		public float m_outer_radius
		{
			get { return _m_outer_radius; }
			set { _m_outer_radius = value; this.Invalidate(); }
		}
		
		private float _m_job_select_radius;
		[STNodeProperty("job_select_radius", "job_select_radius")]
		public float m_job_select_radius
		{
			get { return _m_job_select_radius; }
			set { _m_job_select_radius = value; this.Invalidate(); }
		}
		
		private float _m_job_cancel_radius;
		[STNodeProperty("job_cancel_radius", "job_cancel_radius")]
		public float m_job_cancel_radius
		{
			get { return _m_job_cancel_radius; }
			set { _m_job_cancel_radius = value; this.Invalidate(); }
		}
		
		private float _m_teleport_required_range;
		[STNodeProperty("teleport_required_range", "teleport_required_range")]
		public float m_teleport_required_range
		{
			get { return _m_teleport_required_range; }
			set { _m_teleport_required_range = value; this.Invalidate(); }
		}
		
		private float _m_teleport_radius;
		[STNodeProperty("teleport_radius", "teleport_radius")]
		public float m_teleport_radius
		{
			get { return _m_teleport_radius; }
			set { _m_teleport_radius = value; this.Invalidate(); }
		}
		
		private bool _m_prefer_traversals;
		[STNodeProperty("prefer_traversals", "prefer_traversals")]
		public bool m_prefer_traversals
		{
			get { return _m_prefer_traversals; }
			set { _m_prefer_traversals = value; this.Invalidate(); }
		}
		
		private bool _m_avoid_player;
		[STNodeProperty("avoid_player", "avoid_player")]
		public bool m_avoid_player
		{
			get { return _m_avoid_player; }
			set { _m_avoid_player = value; this.Invalidate(); }
		}
		
		private bool _m_allow_teleports;
		[STNodeProperty("allow_teleports", "allow_teleports")]
		public bool m_allow_teleports
		{
			get { return _m_allow_teleports; }
			set { _m_allow_teleports = value; this.Invalidate(); }
		}
		
		private string _m_follow_type;
		[STNodeProperty("follow_type", "follow_type")]
		public string m_follow_type
		{
			get { return _m_follow_type; }
			set { _m_follow_type = value; this.Invalidate(); }
		}
		
		private bool _m_clamp_speed;
		[STNodeProperty("clamp_speed", "clamp_speed")]
		public bool m_clamp_speed
		{
			get { return _m_clamp_speed; }
			set { _m_clamp_speed = value; this.Invalidate(); }
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
			
			this.Title = "CMD_FollowUsingJobs";
			
			this.InputOptions.Add("target_to_follow", typeof(cTransform), false);
			this.InputOptions.Add("who_Im_leading", typeof(STNode), false);
			this.InputOptions.Add("seed", typeof(void), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("failed", typeof(void), false);
			this.OutputOptions.Add("seeded", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("command_started", typeof(void), false);
		}
	}
}
#endif
