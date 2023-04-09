using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GetClosestPoint : STNode
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
			
			this.Title = "GetClosestPoint";
			
			this.InputOptions.Add("Positions", typeof(STNode), false);
			this.InputOptions.Add("pos_to_be_near", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("bound_to_closest", typeof(void), false);
			this.OutputOptions.Add("position_of_closest", typeof(cTransform), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
