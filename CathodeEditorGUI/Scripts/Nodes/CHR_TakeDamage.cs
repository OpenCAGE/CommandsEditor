using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_TakeDamage : STNode
	{
		private int _m_Damage;
		[STNodeProperty("Damage", "Damage")]
		public int m_Damage
		{
			get { return _m_Damage; }
			set { _m_Damage = value; this.Invalidate(); }
		}
		
		private bool _m_DamageIsAPercentage;
		[STNodeProperty("DamageIsAPercentage", "DamageIsAPercentage")]
		public bool m_DamageIsAPercentage
		{
			get { return _m_DamageIsAPercentage; }
			set { _m_DamageIsAPercentage = value; this.Invalidate(); }
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
			
			this.Title = "CHR_TakeDamage";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
