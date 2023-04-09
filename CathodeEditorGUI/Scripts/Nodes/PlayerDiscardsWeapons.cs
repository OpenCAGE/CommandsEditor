using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayerDiscardsWeapons : STNode
	{
		private bool _m_discard_pistol;
		[STNodeProperty("discard_pistol", "discard_pistol")]
		public bool m_discard_pistol
		{
			get { return _m_discard_pistol; }
			set { _m_discard_pistol = value; this.Invalidate(); }
		}
		
		private bool _m_discard_shotgun;
		[STNodeProperty("discard_shotgun", "discard_shotgun")]
		public bool m_discard_shotgun
		{
			get { return _m_discard_shotgun; }
			set { _m_discard_shotgun = value; this.Invalidate(); }
		}
		
		private bool _m_discard_flamethrower;
		[STNodeProperty("discard_flamethrower", "discard_flamethrower")]
		public bool m_discard_flamethrower
		{
			get { return _m_discard_flamethrower; }
			set { _m_discard_flamethrower = value; this.Invalidate(); }
		}
		
		private bool _m_discard_boltgun;
		[STNodeProperty("discard_boltgun", "discard_boltgun")]
		public bool m_discard_boltgun
		{
			get { return _m_discard_boltgun; }
			set { _m_discard_boltgun = value; this.Invalidate(); }
		}
		
		private bool _m_discard_cattleprod;
		[STNodeProperty("discard_cattleprod", "discard_cattleprod")]
		public bool m_discard_cattleprod
		{
			get { return _m_discard_cattleprod; }
			set { _m_discard_cattleprod = value; this.Invalidate(); }
		}
		
		private bool _m_discard_melee;
		[STNodeProperty("discard_melee", "discard_melee")]
		public bool m_discard_melee
		{
			get { return _m_discard_melee; }
			set { _m_discard_melee = value; this.Invalidate(); }
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
			
			this.Title = "PlayerDiscardsWeapons";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
