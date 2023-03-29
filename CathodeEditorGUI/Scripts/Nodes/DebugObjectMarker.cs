#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugObjectMarker : STNode
	{
		private STNode _m_marked_object;
		[STNodeProperty("marked_object", "marked_object")]
		public STNode m_marked_object
		{
			get { return _m_marked_object; }
			set { _m_marked_object = value; this.Invalidate(); }
		}
		
		private string _m_marked_name;
		[STNodeProperty("marked_name", "marked_name")]
		public string m_marked_name
		{
			get { return _m_marked_name; }
			set { _m_marked_name = value; this.Invalidate(); }
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
			
			this.Title = "DebugObjectMarker";
			
			
		}
	}
}
#endif
