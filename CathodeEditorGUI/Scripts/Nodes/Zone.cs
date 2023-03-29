#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Zone : STNode
	{
		private bool _m_suspend_on_unload;
		[STNodeProperty("suspend_on_unload", "suspend_on_unload")]
		public bool m_suspend_on_unload
		{
			get { return _m_suspend_on_unload; }
			set { _m_suspend_on_unload = value; this.Invalidate(); }
		}
		
		private bool _m_space_visible;
		[STNodeProperty("space_visible", "space_visible")]
		public bool m_space_visible
		{
			get { return _m_space_visible; }
			set { _m_space_visible = value; this.Invalidate(); }
		}
		
		private bool _m_force_visible_on_load;
		[STNodeProperty("force_visible_on_load", "force_visible_on_load")]
		public bool m_force_visible_on_load
		{
			get { return _m_force_visible_on_load; }
			set { _m_force_visible_on_load = value; this.Invalidate(); }
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
			
			this.Title = "Zone";
			
			this.InputOptions.Add("composites", typeof(STNode), false);
			this.InputOptions.Add("request_load", typeof(void), false);
			this.InputOptions.Add("cancel_load", typeof(void), false);
			this.InputOptions.Add("request_unload", typeof(void), false);
			this.InputOptions.Add("cancel_unload", typeof(void), false);
			
			this.OutputOptions.Add("load_requested", typeof(void), false);
			this.OutputOptions.Add("load_cancelled", typeof(void), false);
			this.OutputOptions.Add("unload_requested", typeof(void), false);
			this.OutputOptions.Add("unload_cancelled", typeof(void), false);
			this.OutputOptions.Add("on_loaded", typeof(void), false);
			this.OutputOptions.Add("on_unloaded", typeof(void), false);
			this.OutputOptions.Add("on_streaming", typeof(void), false);
		}
	}
}
#endif
