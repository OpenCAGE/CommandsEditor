#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TerminalFolder : STNode
	{
		private bool _m_lock_on_reset;
		[STNodeProperty("lock_on_reset", "lock_on_reset")]
		public bool m_lock_on_reset
		{
			get { return _m_lock_on_reset; }
			set { _m_lock_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_code;
		[STNodeProperty("code", "code")]
		public string m_code
		{
			get { return _m_code; }
			set { _m_code = value; this.Invalidate(); }
		}
		
		private string _m_folder_title;
		[STNodeProperty("folder_title", "folder_title")]
		public string m_folder_title
		{
			get { return _m_folder_title; }
			set { _m_folder_title = value; this.Invalidate(); }
		}
		
		private string _m_folder_lock_type;
		[STNodeProperty("folder_lock_type", "folder_lock_type")]
		public string m_folder_lock_type
		{
			get { return _m_folder_lock_type; }
			set { _m_folder_lock_type = value; this.Invalidate(); }
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
			
			this.Title = "TerminalFolder";
			
			this.InputOptions.Add("content0", typeof(string), false);
			this.InputOptions.Add("content1", typeof(string), false);
			this.InputOptions.Add("refresh_value", typeof(void), false);
			this.InputOptions.Add("refresh_text", typeof(void), false);
			this.InputOptions.Add("lock", typeof(void), false);
			this.InputOptions.Add("unlock", typeof(void), false);
			
			this.OutputOptions.Add("code_success", typeof(void), false);
			this.OutputOptions.Add("code_fail", typeof(void), false);
			this.OutputOptions.Add("selected", typeof(void), false);
			this.OutputOptions.Add("value_refeshed", typeof(void), false);
			this.OutputOptions.Add("text_refeshed", typeof(void), false);
			this.OutputOptions.Add("locked", typeof(void), false);
			this.OutputOptions.Add("unlocked", typeof(void), false);
		}
	}
}
#endif
