using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetPrimaryObjective : STNode
	{
		private string _m_title;
		[STNodeProperty("title", "title")]
		public string m_title
		{
			get { return _m_title; }
			set { _m_title = value; this.Invalidate(); }
		}
		
		private string _m_additional_info;
		[STNodeProperty("additional_info", "additional_info")]
		public string m_additional_info
		{
			get { return _m_additional_info; }
			set { _m_additional_info = value; this.Invalidate(); }
		}
		
		private string _m_title_list;
		[STNodeProperty("title_list", "title_list")]
		public string m_title_list
		{
			get { return _m_title_list; }
			set { _m_title_list = value; this.Invalidate(); }
		}
		
		private string _m_additional_info_list;
		[STNodeProperty("additional_info_list", "additional_info_list")]
		public string m_additional_info_list
		{
			get { return _m_additional_info_list; }
			set { _m_additional_info_list = value; this.Invalidate(); }
		}
		
		private bool _m_show_message;
		[STNodeProperty("show_message", "show_message")]
		public bool m_show_message
		{
			get { return _m_show_message; }
			set { _m_show_message = value; this.Invalidate(); }
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
			
			this.Title = "SetPrimaryObjective";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
