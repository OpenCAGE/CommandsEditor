using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ZoneLink : STNode
	{
		private int _m_cost;
		[STNodeProperty("cost", "cost")]
		public int m_cost
		{
			get { return _m_cost; }
			set { _m_cost = value; this.Invalidate(); }
		}
		
		private bool _m_open_on_reset;
		[STNodeProperty("open_on_reset", "open_on_reset")]
		public bool m_open_on_reset
		{
			get { return _m_open_on_reset; }
			set { _m_open_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_lock_on_reset;
		[STNodeProperty("lock_on_reset", "lock_on_reset")]
		public bool m_lock_on_reset
		{
			get { return _m_lock_on_reset; }
			set { _m_lock_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "ZoneLink";
			
			this.InputOptions.Add("ZoneA", typeof(string), false);
			this.InputOptions.Add("ZoneB", typeof(string), false);
			this.InputOptions.Add("open", typeof(void), false);
			this.InputOptions.Add("close", typeof(void), false);
			this.InputOptions.Add("lock", typeof(void), false);
			this.InputOptions.Add("unlock", typeof(void), false);
			
			this.OutputOptions.Add("opened", typeof(void), false);
			this.OutputOptions.Add("closed", typeof(void), false);
			this.OutputOptions.Add("locked", typeof(void), false);
			this.OutputOptions.Add("unlocked", typeof(void), false);
		}
	}
}
