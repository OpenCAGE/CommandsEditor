using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class UI_ReactionGame : STNode
	{
		private bool _m_exit_on_fail;
		[STNodeProperty("exit_on_fail", "exit_on_fail")]
		public bool m_exit_on_fail
		{
			get { return _m_exit_on_fail; }
			set { _m_exit_on_fail = value; this.Invalidate(); }
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
			
			this.Title = "UI_ReactionGame";
			
			this.InputOptions.Add("enter", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("ui_icon", typeof(int), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("success", typeof(void), false);
			this.OutputOptions.Add("fail", typeof(void), false);
			this.OutputOptions.Add("stage0_success", typeof(void), false);
			this.OutputOptions.Add("stage0_fail", typeof(void), false);
			this.OutputOptions.Add("stage1_success", typeof(void), false);
			this.OutputOptions.Add("stage1_fail", typeof(void), false);
			this.OutputOptions.Add("stage2_success", typeof(void), false);
			this.OutputOptions.Add("stage2_fail", typeof(void), false);
			this.OutputOptions.Add("ui_breakout_triggered", typeof(void), false);
			this.OutputOptions.Add("resources_finished_unloading", typeof(void), false);
			this.OutputOptions.Add("resources_finished_loading", typeof(void), false);
			this.OutputOptions.Add("completion_percentage", typeof(float), false);
			this.OutputOptions.Add("entered", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("closed", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
