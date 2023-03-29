#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Minigames : STNode
	{
		private bool _m_game_inertial_damping_active;
		[STNodeProperty("game_inertial_damping_active", "game_inertial_damping_active")]
		public bool m_game_inertial_damping_active
		{
			get { return _m_game_inertial_damping_active; }
			set { _m_game_inertial_damping_active = value; this.Invalidate(); }
		}
		
		private bool _m_game_green_text_active;
		[STNodeProperty("game_green_text_active", "game_green_text_active")]
		public bool m_game_green_text_active
		{
			get { return _m_game_green_text_active; }
			set { _m_game_green_text_active = value; this.Invalidate(); }
		}
		
		private bool _m_game_yellow_chart_active;
		[STNodeProperty("game_yellow_chart_active", "game_yellow_chart_active")]
		public bool m_game_yellow_chart_active
		{
			get { return _m_game_yellow_chart_active; }
			set { _m_game_yellow_chart_active = value; this.Invalidate(); }
		}
		
		private bool _m_game_overloc_fail_active;
		[STNodeProperty("game_overloc_fail_active", "game_overloc_fail_active")]
		public bool m_game_overloc_fail_active
		{
			get { return _m_game_overloc_fail_active; }
			set { _m_game_overloc_fail_active = value; this.Invalidate(); }
		}
		
		private bool _m_game_docking_active;
		[STNodeProperty("game_docking_active", "game_docking_active")]
		public bool m_game_docking_active
		{
			get { return _m_game_docking_active; }
			set { _m_game_docking_active = value; this.Invalidate(); }
		}
		
		private bool _m_game_environ_ctr_active;
		[STNodeProperty("game_environ_ctr_active", "game_environ_ctr_active")]
		public bool m_game_environ_ctr_active
		{
			get { return _m_game_environ_ctr_active; }
			set { _m_game_environ_ctr_active = value; this.Invalidate(); }
		}
		
		private int _m_config_pass_number;
		[STNodeProperty("config_pass_number", "config_pass_number")]
		public int m_config_pass_number
		{
			get { return _m_config_pass_number; }
			set { _m_config_pass_number = value; this.Invalidate(); }
		}
		
		private int _m_config_fail_limit;
		[STNodeProperty("config_fail_limit", "config_fail_limit")]
		public int m_config_fail_limit
		{
			get { return _m_config_fail_limit; }
			set { _m_config_fail_limit = value; this.Invalidate(); }
		}
		
		private int _m_config_difficulty;
		[STNodeProperty("config_difficulty", "config_difficulty")]
		public int m_config_difficulty
		{
			get { return _m_config_difficulty; }
			set { _m_config_difficulty = value; this.Invalidate(); }
		}
		
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_pause_on_reset;
		[STNodeProperty("pause_on_reset", "pause_on_reset")]
		public bool m_pause_on_reset
		{
			get { return _m_pause_on_reset; }
			set { _m_pause_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "Minigames";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_success", typeof(void), false);
			this.OutputOptions.Add("on_failure", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
