#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MonitorActionMap : STNode
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
			
			this.Title = "MonitorActionMap";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_pressed_use", typeof(void), false);
			this.OutputOptions.Add("on_released_use", typeof(void), false);
			this.OutputOptions.Add("on_pressed_crouch", typeof(void), false);
			this.OutputOptions.Add("on_released_crouch", typeof(void), false);
			this.OutputOptions.Add("on_pressed_run", typeof(void), false);
			this.OutputOptions.Add("on_released_run", typeof(void), false);
			this.OutputOptions.Add("on_pressed_aim", typeof(void), false);
			this.OutputOptions.Add("on_released_aim", typeof(void), false);
			this.OutputOptions.Add("on_pressed_shoot", typeof(void), false);
			this.OutputOptions.Add("on_released_shoot", typeof(void), false);
			this.OutputOptions.Add("on_pressed_reload", typeof(void), false);
			this.OutputOptions.Add("on_released_reload", typeof(void), false);
			this.OutputOptions.Add("on_pressed_melee", typeof(void), false);
			this.OutputOptions.Add("on_released_melee", typeof(void), false);
			this.OutputOptions.Add("on_pressed_activate_item", typeof(void), false);
			this.OutputOptions.Add("on_released_activate_item", typeof(void), false);
			this.OutputOptions.Add("on_pressed_switch_weapon", typeof(void), false);
			this.OutputOptions.Add("on_released_switch_weapon", typeof(void), false);
			this.OutputOptions.Add("on_pressed_change_dof_focus", typeof(void), false);
			this.OutputOptions.Add("on_released_change_dof_focus", typeof(void), false);
			this.OutputOptions.Add("on_pressed_select_motion_tracker", typeof(void), false);
			this.OutputOptions.Add("on_released_select_motion_tracker", typeof(void), false);
			this.OutputOptions.Add("on_pressed_select_torch", typeof(void), false);
			this.OutputOptions.Add("on_released_select_torch", typeof(void), false);
			this.OutputOptions.Add("on_pressed_torch_beam", typeof(void), false);
			this.OutputOptions.Add("on_released_torch_beam", typeof(void), false);
			this.OutputOptions.Add("on_pressed_peek", typeof(void), false);
			this.OutputOptions.Add("on_released_peek", typeof(void), false);
			this.OutputOptions.Add("on_pressed_back_close", typeof(void), false);
			this.OutputOptions.Add("on_released_back_close", typeof(void), false);
			this.OutputOptions.Add("movement_stick_x", typeof(float), false);
			this.OutputOptions.Add("movement_stick_y", typeof(float), false);
			this.OutputOptions.Add("camera_stick_x", typeof(float), false);
			this.OutputOptions.Add("camera_stick_y", typeof(float), false);
			this.OutputOptions.Add("mouse_x", typeof(float), false);
			this.OutputOptions.Add("mouse_y", typeof(float), false);
			this.OutputOptions.Add("analog_aim", typeof(float), false);
			this.OutputOptions.Add("analog_shoot", typeof(float), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
