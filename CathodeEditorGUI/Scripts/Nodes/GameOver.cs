#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GameOver : STNode
	{
		private string _m_tip_string_id;
		[STNodeProperty("tip_string_id", "tip_string_id")]
		public string m_tip_string_id
		{
			get { return _m_tip_string_id; }
			set { _m_tip_string_id = value; this.Invalidate(); }
		}
		
		private bool _m_default_tips_enabled;
		[STNodeProperty("default_tips_enabled", "default_tips_enabled")]
		public bool m_default_tips_enabled
		{
			get { return _m_default_tips_enabled; }
			set { _m_default_tips_enabled = value; this.Invalidate(); }
		}
		
		private bool _m_level_tips_enabled;
		[STNodeProperty("level_tips_enabled", "level_tips_enabled")]
		public bool m_level_tips_enabled
		{
			get { return _m_level_tips_enabled; }
			set { _m_level_tips_enabled = value; this.Invalidate(); }
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
			
			this.Title = "GameOver";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
