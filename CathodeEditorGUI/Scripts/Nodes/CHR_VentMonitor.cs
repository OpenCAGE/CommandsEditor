using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_VentMonitor : STNode
	{
		private bool _m_trigger_on_start;
		[STNodeProperty("trigger_on_start", "trigger_on_start")]
		public bool m_trigger_on_start
		{
			get { return _m_trigger_on_start; }
			set { _m_trigger_on_start = value; this.Invalidate(); }
		}
		
		private bool _m_trigger_on_checkpoint_restart;
		[STNodeProperty("trigger_on_checkpoint_restart", "trigger_on_checkpoint_restart")]
		public bool m_trigger_on_checkpoint_restart
		{
			get { return _m_trigger_on_checkpoint_restart; }
			set { _m_trigger_on_checkpoint_restart = value; this.Invalidate(); }
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
			
			this.Title = "CHR_VentMonitor";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("entered_vent", typeof(void), false);
			this.OutputOptions.Add("exited_vent", typeof(void), false);
			this.OutputOptions.Add("IsInVent", typeof(bool), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
