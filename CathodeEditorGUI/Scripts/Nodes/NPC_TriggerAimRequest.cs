using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_TriggerAimRequest : STNode
	{
		private bool _m_Raise_gun;
		[STNodeProperty("Raise_gun", "Raise_gun")]
		public bool m_Raise_gun
		{
			get { return _m_Raise_gun; }
			set { _m_Raise_gun = value; this.Invalidate(); }
		}
		
		private bool _m_use_current_target;
		[STNodeProperty("use_current_target", "use_current_target")]
		public bool m_use_current_target
		{
			get { return _m_use_current_target; }
			set { _m_use_current_target = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private float _m_clamp_angle;
		[STNodeProperty("clamp_angle", "clamp_angle")]
		public float m_clamp_angle
		{
			get { return _m_clamp_angle; }
			set { _m_clamp_angle = value; this.Invalidate(); }
		}
		
		private bool _m_clear_current_requests;
		[STNodeProperty("clear_current_requests", "clear_current_requests")]
		public bool m_clear_current_requests
		{
			get { return _m_clear_current_requests; }
			set { _m_clear_current_requests = value; this.Invalidate(); }
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
			
			this.Title = "NPC_TriggerAimRequest";
			
			this.InputOptions.Add("AimTarget", typeof(STNode), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("started_aiming", typeof(void), false);
			this.OutputOptions.Add("finished_aiming", typeof(void), false);
			this.OutputOptions.Add("interrupted", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
