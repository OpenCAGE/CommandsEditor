using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Explosion_AINotifier : STNode
	{
		private cVector3 _m_ExplosionPos;
		[STNodeProperty("ExplosionPos", "ExplosionPos")]
		public cVector3 m_ExplosionPos
		{
			get { return _m_ExplosionPos; }
			set { _m_ExplosionPos = value; this.Invalidate(); }
		}
		
		private string _m_AmmoType;
		[STNodeProperty("AmmoType", "AmmoType")]
		public string m_AmmoType
		{
			get { return _m_AmmoType; }
			set { _m_AmmoType = value; this.Invalidate(); }
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
			
			this.Title = "Explosion_AINotifier";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_character_damage_fx", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
