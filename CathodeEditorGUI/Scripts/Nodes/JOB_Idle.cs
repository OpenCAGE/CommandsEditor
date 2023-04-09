using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class JOB_Idle : STNode
	{
		private string _m_task_operation_mode;
		[STNodeProperty("task_operation_mode", "task_operation_mode")]
		public string m_task_operation_mode
		{
			get { return _m_task_operation_mode; }
			set { _m_task_operation_mode = value; this.Invalidate(); }
		}
		
		private bool _m_should_perform_all_tasks;
		[STNodeProperty("should_perform_all_tasks", "should_perform_all_tasks")]
		public bool m_should_perform_all_tasks
		{
			get { return _m_should_perform_all_tasks; }
			set { _m_should_perform_all_tasks = value; this.Invalidate(); }
		}
		
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "JOB_Idle";
			
			
		}
	}
}
