#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Logic_Vent_Entrance : STNode
	{
		private bool _m_force_stand_on_exit;
		[STNodeProperty("force_stand_on_exit", "force_stand_on_exit")]
		public bool m_force_stand_on_exit
		{
			get { return _m_force_stand_on_exit; }
			set { _m_force_stand_on_exit = value; this.Invalidate(); }
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
			
			this.Title = "Logic_Vent_Entrance";
			
			this.InputOptions.Add("Hide_Pos", typeof(cTransform), false);
			this.InputOptions.Add("Emit_Pos", typeof(cTransform), false);
			this.InputOptions.Add("enter", typeof(void), false);
			this.InputOptions.Add("exit", typeof(void), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			this.InputOptions.Add("set_is_open", typeof(void), false);
			this.InputOptions.Add("set_is_closed", typeof(void), false);
			
			this.OutputOptions.Add("entered", typeof(void), false);
			this.OutputOptions.Add("exited", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("set_to_open", typeof(void), false);
			this.OutputOptions.Add("set_to_closed", typeof(void), false);
		}
	}
}
#endif
