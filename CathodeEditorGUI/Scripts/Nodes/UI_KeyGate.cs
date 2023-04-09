using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class UI_KeyGate : STNode
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
		
		private string _m_code;
		[STNodeProperty("code", "code")]
		public string m_code
		{
			get { return _m_code; }
			set { _m_code = value; this.Invalidate(); }
		}
		
		private int _m_carduid;
		[STNodeProperty("carduid", "carduid")]
		public int m_carduid
		{
			get { return _m_carduid; }
			set { _m_carduid = value; this.Invalidate(); }
		}
		
		private string _m_key_type;
		[STNodeProperty("key_type", "key_type")]
		public string m_key_type
		{
			get { return _m_key_type; }
			set { _m_key_type = value; this.Invalidate(); }
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
			
			this.Title = "UI_KeyGate";
			
			this.InputOptions.Add("enter", typeof(void), false);
			this.InputOptions.Add("exit", typeof(void), false);
			this.InputOptions.Add("lock", typeof(void), false);
			this.InputOptions.Add("unlock", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("light_switch_on", typeof(void), false);
			this.InputOptions.Add("light_switch_off", typeof(void), false);
			
			this.OutputOptions.Add("keycard_success", typeof(void), false);
			this.OutputOptions.Add("keycode_success", typeof(void), false);
			this.OutputOptions.Add("keycard_fail", typeof(void), false);
			this.OutputOptions.Add("keycode_fail", typeof(void), false);
			this.OutputOptions.Add("keycard_cancelled", typeof(void), false);
			this.OutputOptions.Add("keycode_cancelled", typeof(void), false);
			this.OutputOptions.Add("ui_breakout_triggered", typeof(void), false);
			this.OutputOptions.Add("entered", typeof(void), false);
			this.OutputOptions.Add("exited", typeof(void), false);
			this.OutputOptions.Add("locked", typeof(void), false);
			this.OutputOptions.Add("unlocked", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("light_switched_on", typeof(void), false);
			this.OutputOptions.Add("light_switched_off", typeof(void), false);
		}
	}
}
