using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TerminalContent : STNode
	{
		private string _m_content_title;
		[STNodeProperty("content_title", "content_title")]
		public string m_content_title
		{
			get { return _m_content_title; }
			set { _m_content_title = value; this.Invalidate(); }
		}
		
		private string _m_content_decoration_title;
		[STNodeProperty("content_decoration_title", "content_decoration_title")]
		public string m_content_decoration_title
		{
			get { return _m_content_decoration_title; }
			set { _m_content_decoration_title = value; this.Invalidate(); }
		}
		
		private string _m_additional_info;
		[STNodeProperty("additional_info", "additional_info")]
		public string m_additional_info
		{
			get { return _m_additional_info; }
			set { _m_additional_info = value; this.Invalidate(); }
		}
		
		private bool _m_is_connected_to_audio_log;
		[STNodeProperty("is_connected_to_audio_log", "is_connected_to_audio_log")]
		public bool m_is_connected_to_audio_log
		{
			get { return _m_is_connected_to_audio_log; }
			set { _m_is_connected_to_audio_log = value; this.Invalidate(); }
		}
		
		private bool _m_is_triggerable;
		[STNodeProperty("is_triggerable", "is_triggerable")]
		public bool m_is_triggerable
		{
			get { return _m_is_triggerable; }
			set { _m_is_triggerable = value; this.Invalidate(); }
		}
		
		private bool _m_is_single_use;
		[STNodeProperty("is_single_use", "is_single_use")]
		public bool m_is_single_use
		{
			get { return _m_is_single_use; }
			set { _m_is_single_use = value; this.Invalidate(); }
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
			
			this.Title = "TerminalContent";
			
			
			this.OutputOptions.Add("selected", typeof(void), false);
		}
	}
}
