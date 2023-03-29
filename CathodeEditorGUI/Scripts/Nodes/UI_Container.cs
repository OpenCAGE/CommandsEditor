#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class UI_Container : STNode
	{
		private bool _m_is_persistent;
		[STNodeProperty("is_persistent", "is_persistent")]
		public bool m_is_persistent
		{
			get { return _m_is_persistent; }
			set { _m_is_persistent = value; this.Invalidate(); }
		}
		
		private bool _m_is_temporary;
		[STNodeProperty("is_temporary", "is_temporary")]
		public bool m_is_temporary
		{
			get { return _m_is_temporary; }
			set { _m_is_temporary = value; this.Invalidate(); }
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
			
			this.Title = "UI_Container";
			
			this.InputOptions.Add("contents", typeof(string), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("ui_icon", typeof(int), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("take_slot", typeof(void), false);
			this.OutputOptions.Add("emptied", typeof(void), false);
			this.OutputOptions.Add("has_been_used", typeof(bool), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("closed", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
