#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ToggleFunctionality : STNode
	{
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
			
			this.Title = "ToggleFunctionality";
			
			this.InputOptions.Add("disable_radial", typeof(void), false);
			this.InputOptions.Add("enable_radial", typeof(void), false);
			this.InputOptions.Add("disable_radial_hacking_info", typeof(void), false);
			this.InputOptions.Add("enable_radial_hacking_info", typeof(void), false);
			this.InputOptions.Add("disable_radial_cutting_info", typeof(void), false);
			this.InputOptions.Add("enable_radial_cutting_info", typeof(void), false);
			this.InputOptions.Add("disable_radial_battery_info", typeof(void), false);
			this.InputOptions.Add("enable_radial_battery_info", typeof(void), false);
			this.InputOptions.Add("disable_hud_battery_info", typeof(void), false);
			this.InputOptions.Add("enable_hud_battery_info", typeof(void), false);
			
			this.OutputOptions.Add("radial_disabled", typeof(void), false);
			this.OutputOptions.Add("radial_enabled", typeof(void), false);
			this.OutputOptions.Add("radial_hacking_info_disabled", typeof(void), false);
			this.OutputOptions.Add("radial_hacking_info_enabled", typeof(void), false);
			this.OutputOptions.Add("radial_cutting_info_disabled", typeof(void), false);
			this.OutputOptions.Add("radial_cutting_info_enabled", typeof(void), false);
			this.OutputOptions.Add("radial_battery_info_disabled", typeof(void), false);
			this.OutputOptions.Add("radial_battery_info_enabled", typeof(void), false);
			this.OutputOptions.Add("hud_battery_info_disabled", typeof(void), false);
			this.OutputOptions.Add("hud_battery_info_enabled", typeof(void), false);
		}
	}
}
#endif
