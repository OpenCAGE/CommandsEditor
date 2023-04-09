using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DeleteButtonKeys : STNode
	{
		private string _m_door_mechanism;
		[STNodeProperty("door_mechanism", "door_mechanism")]
		public string m_door_mechanism
		{
			get { return _m_door_mechanism; }
			set { _m_door_mechanism = value; this.Invalidate(); }
		}
		
		private string _m_button_type;
		[STNodeProperty("button_type", "button_type")]
		public string m_button_type
		{
			get { return _m_button_type; }
			set { _m_button_type = value; this.Invalidate(); }
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
			
			this.Title = "DeleteButtonKeys";
			
			
		}
	}
}
