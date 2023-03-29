#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FilterIsEnemyOfCharacter : STNode
	{
		private bool _m_use_alliance_at_death;
		[STNodeProperty("use_alliance_at_death", "use_alliance_at_death")]
		public bool m_use_alliance_at_death
		{
			get { return _m_use_alliance_at_death; }
			set { _m_use_alliance_at_death = value; this.Invalidate(); }
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
			
			this.Title = "FilterIsEnemyOfCharacter";
			
			this.InputOptions.Add("Character", typeof(STNode), false);
			
		}
	}
}
#endif
