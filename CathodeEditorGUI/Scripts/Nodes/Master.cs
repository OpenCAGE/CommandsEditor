#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Master : STNode
	{
		private bool _m_suspend_on_reset;
		[STNodeProperty("suspend_on_reset", "suspend_on_reset")]
		public bool m_suspend_on_reset
		{
			get { return _m_suspend_on_reset; }
			set { _m_suspend_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_disable_display;
		[STNodeProperty("disable_display", "disable_display")]
		public bool m_disable_display
		{
			get { return _m_disable_display; }
			set { _m_disable_display = value; this.Invalidate(); }
		}
		
		private bool _m_disable_collision;
		[STNodeProperty("disable_collision", "disable_collision")]
		public bool m_disable_collision
		{
			get { return _m_disable_collision; }
			set { _m_disable_collision = value; this.Invalidate(); }
		}
		
		private bool _m_disable_simulation;
		[STNodeProperty("disable_simulation", "disable_simulation")]
		public bool m_disable_simulation
		{
			get { return _m_disable_simulation; }
			set { _m_disable_simulation = value; this.Invalidate(); }
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
			
			this.Title = "Master";
			
			this.InputOptions.Add("objects", typeof(STNode), false);
			this.InputOptions.Add("suspend", typeof(void), false);
			this.InputOptions.Add("allow", typeof(void), false);
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("simulate", typeof(void), false);
			this.InputOptions.Add("keyframe", typeof(void), false);
			
			this.OutputOptions.Add("suspended", typeof(void), false);
			this.OutputOptions.Add("allowed", typeof(void), false);
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("simulating", typeof(void), false);
			this.OutputOptions.Add("keyframed", typeof(void), false);
		}
	}
}
#endif
