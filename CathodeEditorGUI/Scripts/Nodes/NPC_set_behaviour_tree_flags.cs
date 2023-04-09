using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_set_behaviour_tree_flags : STNode
	{
		private string _m_BehaviourTreeFlag;
		[STNodeProperty("BehaviourTreeFlag", "BehaviourTreeFlag")]
		public string m_BehaviourTreeFlag
		{
			get { return _m_BehaviourTreeFlag; }
			set { _m_BehaviourTreeFlag = value; this.Invalidate(); }
		}
		
		private bool _m_FlagSetting;
		[STNodeProperty("FlagSetting", "FlagSetting")]
		public bool m_FlagSetting
		{
			get { return _m_FlagSetting; }
			set { _m_FlagSetting = value; this.Invalidate(); }
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
			
			this.Title = "NPC_set_behaviour_tree_flags";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
