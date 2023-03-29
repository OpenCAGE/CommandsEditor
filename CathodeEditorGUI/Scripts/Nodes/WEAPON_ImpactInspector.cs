#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_ImpactInspector : STNode
	{
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
			
			this.Title = "WEAPON_ImpactInspector";
			
			this.InputOptions.Add("impact", typeof(void), false);
			
			this.OutputOptions.Add("damage", typeof(int), false);
			this.OutputOptions.Add("impact_position", typeof(cTransform), false);
			this.OutputOptions.Add("impact_target", typeof(STNode), false);
			this.OutputOptions.Add("impacted", typeof(void), false);
		}
	}
}
#endif
