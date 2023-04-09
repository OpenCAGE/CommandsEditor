using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerTouch : STNode
	{
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "TriggerTouch";
			
			this.InputOptions.Add("physics_object", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			
			this.OutputOptions.Add("touch_event", typeof(void), false);
			this.OutputOptions.Add("impact_normal", typeof(cVector3), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
		}
	}
}
