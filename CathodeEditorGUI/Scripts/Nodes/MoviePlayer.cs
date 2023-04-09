using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MoviePlayer : STNode
	{
		private bool _m_trigger_end_on_skipped;
		[STNodeProperty("trigger_end_on_skipped", "trigger_end_on_skipped")]
		public bool m_trigger_end_on_skipped
		{
			get { return _m_trigger_end_on_skipped; }
			set { _m_trigger_end_on_skipped = value; this.Invalidate(); }
		}
		
		private string _m_filename;
		[STNodeProperty("filename", "filename")]
		public string m_filename
		{
			get { return _m_filename; }
			set { _m_filename = value; this.Invalidate(); }
		}
		
		private bool _m_skippable;
		[STNodeProperty("skippable", "skippable")]
		public bool m_skippable
		{
			get { return _m_skippable; }
			set { _m_skippable = value; this.Invalidate(); }
		}
		
		private bool _m_enable_debug_skip;
		[STNodeProperty("enable_debug_skip", "enable_debug_skip")]
		public bool m_enable_debug_skip
		{
			get { return _m_enable_debug_skip; }
			set { _m_enable_debug_skip = value; this.Invalidate(); }
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
			
			this.Title = "MoviePlayer";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("start", typeof(void), false);
			this.OutputOptions.Add("end", typeof(void), false);
			this.OutputOptions.Add("skipped", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
