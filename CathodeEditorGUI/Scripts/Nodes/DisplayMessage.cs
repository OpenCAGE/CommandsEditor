#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DisplayMessage : STNode
	{
		private string _m_title_id;
		[STNodeProperty("title_id", "title_id")]
		public string m_title_id
		{
			get { return _m_title_id; }
			set { _m_title_id = value; this.Invalidate(); }
		}
		
		private string _m_message_id;
		[STNodeProperty("message_id", "message_id")]
		public string m_message_id
		{
			get { return _m_message_id; }
			set { _m_message_id = value; this.Invalidate(); }
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
			
			this.Title = "DisplayMessage";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
