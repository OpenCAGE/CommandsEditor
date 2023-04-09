using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_WithdrawAlien : STNode
	{
		private bool _m_allow_any_searches_to_complete;
		[STNodeProperty("allow_any_searches_to_complete", "allow_any_searches_to_complete")]
		public bool m_allow_any_searches_to_complete
		{
			get { return _m_allow_any_searches_to_complete; }
			set { _m_allow_any_searches_to_complete = value; this.Invalidate(); }
		}
		
		private bool _m_permanent;
		[STNodeProperty("permanent", "permanent")]
		public bool m_permanent
		{
			get { return _m_permanent; }
			set { _m_permanent = value; this.Invalidate(); }
		}
		
		private bool _m_killtraps;
		[STNodeProperty("killtraps", "killtraps")]
		public bool m_killtraps
		{
			get { return _m_killtraps; }
			set { _m_killtraps = value; this.Invalidate(); }
		}
		
		private float _m_initial_radius;
		[STNodeProperty("initial_radius", "initial_radius")]
		public float m_initial_radius
		{
			get { return _m_initial_radius; }
			set { _m_initial_radius = value; this.Invalidate(); }
		}
		
		private float _m_timed_out_radius;
		[STNodeProperty("timed_out_radius", "timed_out_radius")]
		public float m_timed_out_radius
		{
			get { return _m_timed_out_radius; }
			set { _m_timed_out_radius = value; this.Invalidate(); }
		}
		
		private float _m_time_to_force;
		[STNodeProperty("time_to_force", "time_to_force")]
		public float m_time_to_force
		{
			get { return _m_time_to_force; }
			set { _m_time_to_force = value; this.Invalidate(); }
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
			
			this.Title = "NPC_WithdrawAlien";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("cancel", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("cancelled", typeof(void), false);
		}
	}
}
