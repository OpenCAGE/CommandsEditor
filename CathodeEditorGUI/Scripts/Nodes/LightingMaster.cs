using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class LightingMaster : STNode
	{
		private bool _m_light_on_reset;
		[STNodeProperty("light_on_reset", "light_on_reset")]
		public bool m_light_on_reset
		{
			get { return _m_light_on_reset; }
			set { _m_light_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "LightingMaster";
			
			this.InputOptions.Add("objects", typeof(STNode), false);
			this.InputOptions.Add("light_switch_on", typeof(void), false);
			this.InputOptions.Add("light_switch_off", typeof(void), false);
			
			this.OutputOptions.Add("light_switched_on", typeof(void), false);
			this.OutputOptions.Add("light_switched_off", typeof(void), false);
		}
	}
}
