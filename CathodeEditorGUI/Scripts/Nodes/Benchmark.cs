using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Benchmark : STNode
	{
		private string _m_benchmark_name;
		[STNodeProperty("benchmark_name", "benchmark_name")]
		public string m_benchmark_name
		{
			get { return _m_benchmark_name; }
			set { _m_benchmark_name = value; this.Invalidate(); }
		}
		
		private bool _m_save_stats;
		[STNodeProperty("save_stats", "save_stats")]
		public bool m_save_stats
		{
			get { return _m_save_stats; }
			set { _m_save_stats = value; this.Invalidate(); }
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
			
			this.Title = "Benchmark";
			
			this.InputOptions.Add("start_benchmark", typeof(void), false);
			this.InputOptions.Add("stop_benchmark", typeof(void), false);
			
			this.OutputOptions.Add("started_benchmark", typeof(void), false);
			this.OutputOptions.Add("stopped_benchmark", typeof(void), false);
		}
	}
}
