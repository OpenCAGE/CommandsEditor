using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MultitrackLoop : STNode
	{
		private float _m_start_time;
		[STNodeProperty("start_time", "start_time")]
		public float m_start_time
		{
			get { return _m_start_time; }
			set { _m_start_time = value; this.Invalidate(); }
		}
		
		private float _m_end_time;
		[STNodeProperty("end_time", "end_time")]
		public float m_end_time
		{
			get { return _m_end_time; }
			set { _m_end_time = value; this.Invalidate(); }
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
			
			this.Title = "MultitrackLoop";
			
			this.InputOptions.Add("current_time", typeof(float), false);
			this.InputOptions.Add("loop_condition", typeof(bool), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
		}
	}
}
