using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AccessTerminal : STNode
	{
		private bool _m_light_on_reset;
		[STNodeProperty("light_on_reset", "light_on_reset")]
		public bool m_light_on_reset
		{
			get { return _m_light_on_reset; }
			set { _m_light_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_location;
		[STNodeProperty("location", "location")]
		public string m_location
		{
			get { return _m_location; }
			set { _m_location = value; this.Invalidate(); }
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
			
			this.Title = "AccessTerminal";
			
			this.InputOptions.Add("folder0", typeof(string), false);
			this.InputOptions.Add("folder1", typeof(string), false);
			this.InputOptions.Add("folder2", typeof(string), false);
			this.InputOptions.Add("folder3", typeof(string), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("cancel", typeof(void), false);
			this.InputOptions.Add("light_switch_on", typeof(void), false);
			this.InputOptions.Add("light_switch_off", typeof(void), false);
			
			this.OutputOptions.Add("closed", typeof(void), false);
			this.OutputOptions.Add("all_data_has_been_read", typeof(void), false);
			this.OutputOptions.Add("ui_breakout_triggered", typeof(void), false);
			this.OutputOptions.Add("all_data_read", typeof(bool), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("cancelled", typeof(void), false);
			this.OutputOptions.Add("light_switched_on", typeof(void), false);
			this.OutputOptions.Add("light_switched_off", typeof(void), false);
		}
	}
}
