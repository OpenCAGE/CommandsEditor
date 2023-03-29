#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class LevelInfo : STNode
	{
		private string _m_save_level_name_id;
		[STNodeProperty("save_level_name_id", "save_level_name_id")]
		public string m_save_level_name_id
		{
			get { return _m_save_level_name_id; }
			set { _m_save_level_name_id = value; this.Invalidate(); }
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
			
			this.Title = "LevelInfo";
			
			
		}
	}
}
#endif
