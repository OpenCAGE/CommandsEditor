#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class HackingGame : STNode
	{
		private bool _m_lock_on_reset;
		[STNodeProperty("lock_on_reset", "lock_on_reset")]
		public bool m_lock_on_reset
		{
			get { return _m_lock_on_reset; }
			set { _m_lock_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_light_on_reset;
		[STNodeProperty("light_on_reset", "light_on_reset")]
		public bool m_light_on_reset
		{
			get { return _m_light_on_reset; }
			set { _m_light_on_reset = value; this.Invalidate(); }
		}
		
		private int _m_hacking_difficulty;
		[STNodeProperty("hacking_difficulty", "hacking_difficulty")]
		public int m_hacking_difficulty
		{
			get { return _m_hacking_difficulty; }
			set { _m_hacking_difficulty = value; this.Invalidate(); }
		}
		
		private bool _m_auto_exit;
		[STNodeProperty("auto_exit", "auto_exit")]
		public bool m_auto_exit
		{
			get { return _m_auto_exit; }
			set { _m_auto_exit = value; this.Invalidate(); }
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
			
			this.Title = "HackingGame";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("cancel", typeof(void), false);
			this.InputOptions.Add("enter", typeof(void), false);
			this.InputOptions.Add("exit", typeof(void), false);
			this.InputOptions.Add("lock", typeof(void), false);
			this.InputOptions.Add("unlock", typeof(void), false);
			this.InputOptions.Add("light_switch_on", typeof(void), false);
			this.InputOptions.Add("light_switch_off", typeof(void), false);
			this.InputOptions.Add("display_tutorial", typeof(void), false);
			this.InputOptions.Add("transition_completed", typeof(void), false);
			this.InputOptions.Add("reset_hacking_success_flag", typeof(void), false);
			this.InputOptions.Add("display_hacking_upgrade", typeof(void), false);
			this.InputOptions.Add("hide_hacking_upgrade", typeof(void), false);
			
			this.OutputOptions.Add("win", typeof(void), false);
			this.OutputOptions.Add("fail", typeof(void), false);
			this.OutputOptions.Add("alarm_triggered", typeof(void), false);
			this.OutputOptions.Add("closed", typeof(void), false);
			this.OutputOptions.Add("loaded_idle", typeof(void), false);
			this.OutputOptions.Add("loaded_success", typeof(void), false);
			this.OutputOptions.Add("ui_breakout_triggered", typeof(void), false);
			this.OutputOptions.Add("resources_finished_unloading", typeof(void), false);
			this.OutputOptions.Add("resources_finished_loading", typeof(void), false);
			this.OutputOptions.Add("completion_percentage", typeof(float), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("cancelled", typeof(void), false);
			this.OutputOptions.Add("entered", typeof(void), false);
			this.OutputOptions.Add("exited", typeof(void), false);
			this.OutputOptions.Add("locked", typeof(void), false);
			this.OutputOptions.Add("unlocked", typeof(void), false);
			this.OutputOptions.Add("light_switched_on", typeof(void), false);
			this.OutputOptions.Add("light_switched_off", typeof(void), false);
			this.OutputOptions.Add("hacking_success_flag_reset", typeof(void), false);
			this.OutputOptions.Add("hacking_upgrade_displayed", typeof(void), false);
			this.OutputOptions.Add("hacking_upgrade_hidden", typeof(void), false);
		}
	}
}
#endif
