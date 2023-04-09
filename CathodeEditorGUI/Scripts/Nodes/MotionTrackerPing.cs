using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MotionTrackerPing : STNode
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
			
			this.Title = "MotionTrackerPing";
			
			this.InputOptions.Add("FakePosition", typeof(cTransform), false);
			this.InputOptions.Add("start_ping", typeof(void), false);
			this.InputOptions.Add("stop_ping", typeof(void), false);
			
			this.OutputOptions.Add("started_ping", typeof(void), false);
			this.OutputOptions.Add("stopped_ping", typeof(void), false);
		}
	}
}
