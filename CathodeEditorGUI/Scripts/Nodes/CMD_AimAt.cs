#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_AimAt : STNode
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
			
			this.Title = "CMD_AimAt";
			
			this.InputOptions.Add("AimTarget", typeof(STNode), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("finished", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("command_started", typeof(void), false);
		}
	}
}
#endif
