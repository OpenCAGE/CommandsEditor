#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugCheckpoint : STNode
	{
		private string _m_section;
		[STNodeProperty("section", "section")]
		public string m_section
		{
			get { return _m_section; }
			set { _m_section = value; this.Invalidate(); }
		}
		
		private bool _m_level_reset;
		[STNodeProperty("level_reset", "level_reset")]
		public bool m_level_reset
		{
			get { return _m_level_reset; }
			set { _m_level_reset = value; this.Invalidate(); }
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
			
			this.Title = "DebugCheckpoint";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_checkpoint", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
