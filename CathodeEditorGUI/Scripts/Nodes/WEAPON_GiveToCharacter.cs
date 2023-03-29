#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_GiveToCharacter : STNode
	{
		private bool _m_is_holstered;
		[STNodeProperty("is_holstered", "is_holstered")]
		public bool m_is_holstered
		{
			get { return _m_is_holstered; }
			set { _m_is_holstered = value; this.Invalidate(); }
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
			
			this.Title = "WEAPON_GiveToCharacter";
			
			this.InputOptions.Add("Character", typeof(STNode), false);
			this.InputOptions.Add("Weapon", typeof(STNode), false);
			this.InputOptions.Add("set", typeof(void), false);
			
			this.OutputOptions.Add("been_set", typeof(void), false);
		}
	}
}
#endif
