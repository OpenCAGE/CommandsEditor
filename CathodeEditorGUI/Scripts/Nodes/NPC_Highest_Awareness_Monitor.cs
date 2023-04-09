using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_Highest_Awareness_Monitor : STNode
	{
		private bool _m_trigger_on_start;
		[STNodeProperty("trigger_on_start", "trigger_on_start")]
		public bool m_trigger_on_start
		{
			get { return _m_trigger_on_start; }
			set { _m_trigger_on_start = value; this.Invalidate(); }
		}
		
		private bool _m_CheckAllNPCs;
		[STNodeProperty("CheckAllNPCs", "CheckAllNPCs")]
		public bool m_CheckAllNPCs
		{
			get { return _m_CheckAllNPCs; }
			set { _m_CheckAllNPCs = value; this.Invalidate(); }
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
			
			this.Title = "NPC_Highest_Awareness_Monitor";
			
			this.InputOptions.Add("NPC_Coordinator", typeof(STNode), false);
			this.InputOptions.Add("Target", typeof(STNode), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("All_Dead", typeof(void), false);
			this.OutputOptions.Add("Stunned", typeof(void), false);
			this.OutputOptions.Add("Unaware", typeof(void), false);
			this.OutputOptions.Add("Suspicious", typeof(void), false);
			this.OutputOptions.Add("SearchingArea", typeof(void), false);
			this.OutputOptions.Add("SearchingLastSensed", typeof(void), false);
			this.OutputOptions.Add("Aware", typeof(void), false);
			this.OutputOptions.Add("on_changed", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
