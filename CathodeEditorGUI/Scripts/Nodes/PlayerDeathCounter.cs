#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayerDeathCounter : STNode
	{
		private int _m_Limit;
		[STNodeProperty("Limit", "Limit")]
		public int m_Limit
		{
			get { return _m_Limit; }
			set { _m_Limit = value; this.Invalidate(); }
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
			
			this.Title = "PlayerDeathCounter";
			
			this.InputOptions.Add("filter", typeof(bool), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("on_limit", typeof(void), false);
			this.OutputOptions.Add("above_limit", typeof(void), false);
			this.OutputOptions.Add("count", typeof(int), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
