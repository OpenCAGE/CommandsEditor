#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_Group_Death_Monitor : STNode
	{
		private STNode _m_squad_coordinator;
		[STNodeProperty("squad_coordinator", "squad_coordinator")]
		public STNode m_squad_coordinator
		{
			get { return _m_squad_coordinator; }
			set { _m_squad_coordinator = value; this.Invalidate(); }
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
			
			this.Title = "NPC_Group_Death_Monitor";
			
			this.InputOptions.Add("start_monitor", typeof(void), false);
			this.InputOptions.Add("stop_monitor", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("last_man_dying", typeof(void), false);
			this.OutputOptions.Add("all_killed", typeof(void), false);
			this.OutputOptions.Add("started_monitor", typeof(void), false);
			this.OutputOptions.Add("stopped_monitor", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
