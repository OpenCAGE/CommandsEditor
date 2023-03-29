#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_Sleeping_Android_Monitor : STNode
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
			
			this.Title = "NPC_Sleeping_Android_Monitor";
			
			this.InputOptions.Add("Android_NPC", typeof(STNode), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			this.InputOptions.Add("task_end", typeof(void), false);
			
			this.OutputOptions.Add("Twitch", typeof(void), false);
			this.OutputOptions.Add("SitUp_Start", typeof(void), false);
			this.OutputOptions.Add("SitUp_End", typeof(void), false);
			this.OutputOptions.Add("Sleeping_GetUp", typeof(void), false);
			this.OutputOptions.Add("Sitting_GetUp", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("task_ended", typeof(void), false);
		}
	}
}
#endif
