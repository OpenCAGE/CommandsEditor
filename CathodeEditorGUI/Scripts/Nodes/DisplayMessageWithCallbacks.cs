#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DisplayMessageWithCallbacks : STNode
	{
		private string _m_title_text;
		[STNodeProperty("title_text", "title_text")]
		public string m_title_text
		{
			get { return _m_title_text; }
			set { _m_title_text = value; this.Invalidate(); }
		}
		
		private string _m_message_text;
		[STNodeProperty("message_text", "message_text")]
		public string m_message_text
		{
			get { return _m_message_text; }
			set { _m_message_text = value; this.Invalidate(); }
		}
		
		private string _m_yes_text;
		[STNodeProperty("yes_text", "yes_text")]
		public string m_yes_text
		{
			get { return _m_yes_text; }
			set { _m_yes_text = value; this.Invalidate(); }
		}
		
		private string _m_no_text;
		[STNodeProperty("no_text", "no_text")]
		public string m_no_text
		{
			get { return _m_no_text; }
			set { _m_no_text = value; this.Invalidate(); }
		}
		
		private string _m_cancel_text;
		[STNodeProperty("cancel_text", "cancel_text")]
		public string m_cancel_text
		{
			get { return _m_cancel_text; }
			set { _m_cancel_text = value; this.Invalidate(); }
		}
		
		private bool _m_yes_button;
		[STNodeProperty("yes_button", "yes_button")]
		public bool m_yes_button
		{
			get { return _m_yes_button; }
			set { _m_yes_button = value; this.Invalidate(); }
		}
		
		private bool _m_no_button;
		[STNodeProperty("no_button", "no_button")]
		public bool m_no_button
		{
			get { return _m_no_button; }
			set { _m_no_button = value; this.Invalidate(); }
		}
		
		private bool _m_cancel_button;
		[STNodeProperty("cancel_button", "cancel_button")]
		public bool m_cancel_button
		{
			get { return _m_cancel_button; }
			set { _m_cancel_button = value; this.Invalidate(); }
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
			
			this.Title = "DisplayMessageWithCallbacks";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_yes", typeof(void), false);
			this.OutputOptions.Add("on_no", typeof(void), false);
			this.OutputOptions.Add("on_cancel", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
