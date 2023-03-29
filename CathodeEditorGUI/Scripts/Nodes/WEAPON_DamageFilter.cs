#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_DamageFilter : STNode
	{
		private int _m_damage_threshold;
		[STNodeProperty("damage_threshold", "damage_threshold")]
		public int m_damage_threshold
		{
			get { return _m_damage_threshold; }
			set { _m_damage_threshold = value; this.Invalidate(); }
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
			
			this.Title = "WEAPON_DamageFilter";
			
			this.InputOptions.Add("impact", typeof(void), false);
			
			this.OutputOptions.Add("passed", typeof(void), false);
			this.OutputOptions.Add("failed", typeof(void), false);
			this.OutputOptions.Add("impacted", typeof(void), false);
		}
	}
}
#endif
