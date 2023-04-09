using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_ModifyCombatBehaviour : STNode
	{
		private string _m_behaviour_type;
		[STNodeProperty("behaviour_type", "behaviour_type")]
		public string m_behaviour_type
		{
			get { return _m_behaviour_type; }
			set { _m_behaviour_type = value; this.Invalidate(); }
		}
		
		private bool _m_status;
		[STNodeProperty("status", "status")]
		public bool m_status
		{
			get { return _m_status; }
			set { _m_status = value; this.Invalidate(); }
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
			
			this.Title = "CMD_ModifyCombatBehaviour";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			
			this.OutputOptions.Add("start_applied", typeof(void), false);
		}
	}
}
