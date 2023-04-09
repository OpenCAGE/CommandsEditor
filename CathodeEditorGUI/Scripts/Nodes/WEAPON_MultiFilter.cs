using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_MultiFilter : STNode
	{
		private bool _m_AttackerFilter;
		[STNodeProperty("AttackerFilter", "AttackerFilter")]
		public bool m_AttackerFilter
		{
			get { return _m_AttackerFilter; }
			set { _m_AttackerFilter = value; this.Invalidate(); }
		}
		
		private bool _m_TargetFilter;
		[STNodeProperty("TargetFilter", "TargetFilter")]
		public bool m_TargetFilter
		{
			get { return _m_TargetFilter; }
			set { _m_TargetFilter = value; this.Invalidate(); }
		}
		
		private int _m_DamageThreshold;
		[STNodeProperty("DamageThreshold", "DamageThreshold")]
		public int m_DamageThreshold
		{
			get { return _m_DamageThreshold; }
			set { _m_DamageThreshold = value; this.Invalidate(); }
		}
		
		private string _m_DamageType;
		[STNodeProperty("DamageType", "DamageType")]
		public string m_DamageType
		{
			get { return _m_DamageType; }
			set { _m_DamageType = value; this.Invalidate(); }
		}
		
		private bool _m_UseAmmoFilter;
		[STNodeProperty("UseAmmoFilter", "UseAmmoFilter")]
		public bool m_UseAmmoFilter
		{
			get { return _m_UseAmmoFilter; }
			set { _m_UseAmmoFilter = value; this.Invalidate(); }
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
			
			this.Title = "WEAPON_MultiFilter";
			
			this.InputOptions.Add("impact", typeof(void), false);
			
			this.OutputOptions.Add("passed", typeof(void), false);
			this.OutputOptions.Add("failed", typeof(void), false);
			this.OutputOptions.Add("impacted", typeof(void), false);
		}
	}
}
