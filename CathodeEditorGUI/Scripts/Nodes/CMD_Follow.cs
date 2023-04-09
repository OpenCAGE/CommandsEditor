using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_Follow : STNode
	{
		private string _m_idle_stance;
		[STNodeProperty("idle_stance", "idle_stance")]
		public string m_idle_stance
		{
			get { return _m_idle_stance; }
			set { _m_idle_stance = value; this.Invalidate(); }
		}
		
		private string _m_move_type;
		[STNodeProperty("move_type", "move_type")]
		public string m_move_type
		{
			get { return _m_move_type; }
			set { _m_move_type = value; this.Invalidate(); }
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
		
		private bool _m_prefer_traversals;
		[STNodeProperty("prefer_traversals", "prefer_traversals")]
		public bool m_prefer_traversals
		{
			get { return _m_prefer_traversals; }
			set { _m_prefer_traversals = value; this.Invalidate(); }
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
			
			this.Title = "CMD_Follow";
			
			this.InputOptions.Add("Waypoint", typeof(cTransform), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("entered_inner_radius", typeof(void), false);
			this.OutputOptions.Add("exitted_outer_radius", typeof(void), false);
			this.OutputOptions.Add("failed", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("command_started", typeof(void), false);
		}
	}
}
