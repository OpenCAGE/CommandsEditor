using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayerDiscardsTools : STNode
	{
		private bool _m_discard_motion_tracker;
		[STNodeProperty("discard_motion_tracker", "discard_motion_tracker")]
		public bool m_discard_motion_tracker
		{
			get { return _m_discard_motion_tracker; }
			set { _m_discard_motion_tracker = value; this.Invalidate(); }
		}
		
		private bool _m_discard_cutting_torch;
		[STNodeProperty("discard_cutting_torch", "discard_cutting_torch")]
		public bool m_discard_cutting_torch
		{
			get { return _m_discard_cutting_torch; }
			set { _m_discard_cutting_torch = value; this.Invalidate(); }
		}
		
		private bool _m_discard_hacking_tool;
		[STNodeProperty("discard_hacking_tool", "discard_hacking_tool")]
		public bool m_discard_hacking_tool
		{
			get { return _m_discard_hacking_tool; }
			set { _m_discard_hacking_tool = value; this.Invalidate(); }
		}
		
		private bool _m_discard_keycard;
		[STNodeProperty("discard_keycard", "discard_keycard")]
		public bool m_discard_keycard
		{
			get { return _m_discard_keycard; }
			set { _m_discard_keycard = value; this.Invalidate(); }
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
			
			this.Title = "PlayerDiscardsTools";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
