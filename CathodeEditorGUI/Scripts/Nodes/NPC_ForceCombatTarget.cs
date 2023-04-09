using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_ForceCombatTarget : STNode
	{
		private bool _m_LockOtherAttackersOut;
		[STNodeProperty("LockOtherAttackersOut", "LockOtherAttackersOut")]
		public bool m_LockOtherAttackersOut
		{
			get { return _m_LockOtherAttackersOut; }
			set { _m_LockOtherAttackersOut = value; this.Invalidate(); }
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
			
			this.Title = "NPC_ForceCombatTarget";
			
			this.InputOptions.Add("Target", typeof(STNode), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
