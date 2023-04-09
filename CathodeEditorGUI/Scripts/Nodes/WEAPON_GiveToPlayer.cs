using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_GiveToPlayer : STNode
	{
		private string _m_weapon;
		[STNodeProperty("weapon", "weapon")]
		public string m_weapon
		{
			get { return _m_weapon; }
			set { _m_weapon = value; this.Invalidate(); }
		}
		
		private bool _m_holster;
		[STNodeProperty("holster", "holster")]
		public bool m_holster
		{
			get { return _m_holster; }
			set { _m_holster = value; this.Invalidate(); }
		}
		
		private int _m_starting_ammo;
		[STNodeProperty("starting_ammo", "starting_ammo")]
		public int m_starting_ammo
		{
			get { return _m_starting_ammo; }
			set { _m_starting_ammo = value; this.Invalidate(); }
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
			
			this.Title = "WEAPON_GiveToPlayer";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
