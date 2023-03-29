#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_AmmoTypeFilter : STNode
	{
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
			
			this.Title = "WEAPON_AmmoTypeFilter";
			
			this.InputOptions.Add("impact", typeof(void), false);
			
			this.OutputOptions.Add("passed", typeof(void), false);
			this.OutputOptions.Add("failed", typeof(void), false);
			this.OutputOptions.Add("impacted", typeof(void), false);
		}
	}
}
#endif
