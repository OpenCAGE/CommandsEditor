using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ExclusiveMaster : STNode
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
			
			this.Title = "ExclusiveMaster";
			
			this.InputOptions.Add("active_objects", typeof(STNode), false);
			this.InputOptions.Add("inactive_objects", typeof(STNode), false);
			this.InputOptions.Add("set_active", typeof(void), false);
			this.InputOptions.Add("set_inactive", typeof(void), false);
			
			this.OutputOptions.Add("activated", typeof(void), false);
			this.OutputOptions.Add("deactivated", typeof(void), false);
		}
	}
}
