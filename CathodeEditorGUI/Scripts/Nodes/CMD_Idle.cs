#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_Idle : STNode
	{
		private bool _m_should_face_target;
		[STNodeProperty("should_face_target", "should_face_target")]
		public bool m_should_face_target
		{
			get { return _m_should_face_target; }
			set { _m_should_face_target = value; this.Invalidate(); }
		}
		
		private bool _m_should_raise_gun_while_turning;
		[STNodeProperty("should_raise_gun_while_turning", "should_raise_gun_while_turning")]
		public bool m_should_raise_gun_while_turning
		{
			get { return _m_should_raise_gun_while_turning; }
			set { _m_should_raise_gun_while_turning = value; this.Invalidate(); }
		}
		
		private string _m_desired_stance;
		[STNodeProperty("desired_stance", "desired_stance")]
		public string m_desired_stance
		{
			get { return _m_desired_stance; }
			set { _m_desired_stance = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private string _m_idle_style;
		[STNodeProperty("idle_style", "idle_style")]
		public string m_idle_style
		{
			get { return _m_idle_style; }
			set { _m_idle_style = value; this.Invalidate(); }
		}
		
		private bool _m_lock_cameras;
		[STNodeProperty("lock_cameras", "lock_cameras")]
		public bool m_lock_cameras
		{
			get { return _m_lock_cameras; }
			set { _m_lock_cameras = value; this.Invalidate(); }
		}
		
		private bool _m_anchor;
		[STNodeProperty("anchor", "anchor")]
		public bool m_anchor
		{
			get { return _m_anchor; }
			set { _m_anchor = value; this.Invalidate(); }
		}
		
		private bool _m_start_instantly;
		[STNodeProperty("start_instantly", "start_instantly")]
		public bool m_start_instantly
		{
			get { return _m_start_instantly; }
			set { _m_start_instantly = value; this.Invalidate(); }
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
			
			this.Title = "CMD_Idle";
			
			this.InputOptions.Add("target_to_face", typeof(cTransform), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("finished", typeof(void), false);
			this.OutputOptions.Add("interrupted", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("command_started", typeof(void), false);
		}
	}
}
#endif
