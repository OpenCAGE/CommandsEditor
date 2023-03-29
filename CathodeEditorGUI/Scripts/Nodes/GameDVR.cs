#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GameDVR : STNode
	{
		private int _m_start_time;
		[STNodeProperty("start_time", "start_time")]
		public int m_start_time
		{
			get { return _m_start_time; }
			set { _m_start_time = value; this.Invalidate(); }
		}
		
		private int _m_duration;
		[STNodeProperty("duration", "duration")]
		public int m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private string _m_moment_ID;
		[STNodeProperty("moment_ID", "moment_ID")]
		public string m_moment_ID
		{
			get { return _m_moment_ID; }
			set { _m_moment_ID = value; this.Invalidate(); }
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
			
			this.Title = "GameDVR";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
		}
	}
}
#endif
