using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MonitorPadInput : STNode
	{
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
			
			this.Title = "MonitorPadInput";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_pressed_A", typeof(void), false);
			this.OutputOptions.Add("on_released_A", typeof(void), false);
			this.OutputOptions.Add("on_pressed_B", typeof(void), false);
			this.OutputOptions.Add("on_released_B", typeof(void), false);
			this.OutputOptions.Add("on_pressed_X", typeof(void), false);
			this.OutputOptions.Add("on_released_X", typeof(void), false);
			this.OutputOptions.Add("on_pressed_Y", typeof(void), false);
			this.OutputOptions.Add("on_released_Y", typeof(void), false);
			this.OutputOptions.Add("on_pressed_L1", typeof(void), false);
			this.OutputOptions.Add("on_released_L1", typeof(void), false);
			this.OutputOptions.Add("on_pressed_R1", typeof(void), false);
			this.OutputOptions.Add("on_released_R1", typeof(void), false);
			this.OutputOptions.Add("on_pressed_L2", typeof(void), false);
			this.OutputOptions.Add("on_released_L2", typeof(void), false);
			this.OutputOptions.Add("on_pressed_R2", typeof(void), false);
			this.OutputOptions.Add("on_released_R2", typeof(void), false);
			this.OutputOptions.Add("on_pressed_L3", typeof(void), false);
			this.OutputOptions.Add("on_released_L3", typeof(void), false);
			this.OutputOptions.Add("on_pressed_R3", typeof(void), false);
			this.OutputOptions.Add("on_released_R3", typeof(void), false);
			this.OutputOptions.Add("on_dpad_left", typeof(void), false);
			this.OutputOptions.Add("on_released_dpad_left", typeof(void), false);
			this.OutputOptions.Add("on_dpad_right", typeof(void), false);
			this.OutputOptions.Add("on_released_dpad_right", typeof(void), false);
			this.OutputOptions.Add("on_dpad_up", typeof(void), false);
			this.OutputOptions.Add("on_released_dpad_up", typeof(void), false);
			this.OutputOptions.Add("on_dpad_down", typeof(void), false);
			this.OutputOptions.Add("on_released_dpad_down", typeof(void), false);
			this.OutputOptions.Add("left_stick_x", typeof(float), false);
			this.OutputOptions.Add("left_stick_y", typeof(float), false);
			this.OutputOptions.Add("right_stick_x", typeof(float), false);
			this.OutputOptions.Add("right_stick_y", typeof(float), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
