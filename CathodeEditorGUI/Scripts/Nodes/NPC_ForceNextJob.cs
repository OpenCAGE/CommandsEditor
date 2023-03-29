#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_ForceNextJob : STNode
	{
		private bool _m_ShouldInterruptCurrentTask;
		[STNodeProperty("ShouldInterruptCurrentTask", "ShouldInterruptCurrentTask")]
		public bool m_ShouldInterruptCurrentTask
		{
			get { return _m_ShouldInterruptCurrentTask; }
			set { _m_ShouldInterruptCurrentTask = value; this.Invalidate(); }
		}
		
		private STNode _m_Job;
		[STNodeProperty("Job", "Job")]
		public STNode m_Job
		{
			get { return _m_Job; }
			set { _m_Job = value; this.Invalidate(); }
		}
		
		private STNode _m_InitialTask;
		[STNodeProperty("InitialTask", "InitialTask")]
		public STNode m_InitialTask
		{
			get { return _m_InitialTask; }
			set { _m_InitialTask = value; this.Invalidate(); }
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
			
			this.Title = "NPC_ForceNextJob";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("job_started", typeof(void), false);
			this.OutputOptions.Add("job_completed", typeof(void), false);
			this.OutputOptions.Add("job_interrupted", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
#endif
