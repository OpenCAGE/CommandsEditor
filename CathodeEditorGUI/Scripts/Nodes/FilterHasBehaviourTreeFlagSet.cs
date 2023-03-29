#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FilterHasBehaviourTreeFlagSet : STNode
	{
		private string _m_BehaviourTreeFlag;
		[STNodeProperty("BehaviourTreeFlag", "BehaviourTreeFlag")]
		public string m_BehaviourTreeFlag
		{
			get { return _m_BehaviourTreeFlag; }
			set { _m_BehaviourTreeFlag = value; this.Invalidate(); }
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
			
			this.Title = "FilterHasBehaviourTreeFlagSet";
			
			
		}
	}
}
#endif
