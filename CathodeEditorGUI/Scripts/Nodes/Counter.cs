using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Counter : STNode
	{
		private bool _m_is_limitless;
		[STNodeProperty("is_limitless", "is_limitless")]
		public bool m_is_limitless
		{
			get { return _m_is_limitless; }
			set { _m_is_limitless = value; this.Invalidate(); }
		}
		
		private int _m_trigger_limit;
		[STNodeProperty("trigger_limit", "trigger_limit")]
		public int m_trigger_limit
		{
			get { return _m_trigger_limit; }
			set { _m_trigger_limit = value; this.Invalidate(); }
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
			
			this.Title = "Counter";
			
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_under_limit", typeof(void), false);
			this.OutputOptions.Add("on_limit", typeof(void), false);
			this.OutputOptions.Add("on_over_limit", typeof(void), false);
			this.OutputOptions.Add("Count", typeof(int), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
