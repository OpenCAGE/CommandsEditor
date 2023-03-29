#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DoorStatus : STNode
	{
		private int _m_hacking_difficulty;
		[STNodeProperty("hacking_difficulty", "hacking_difficulty")]
		public int m_hacking_difficulty
		{
			get { return _m_hacking_difficulty; }
			set { _m_hacking_difficulty = value; this.Invalidate(); }
		}
		
		private string _m_door_mechanism;
		[STNodeProperty("door_mechanism", "door_mechanism")]
		public string m_door_mechanism
		{
			get { return _m_door_mechanism; }
			set { _m_door_mechanism = value; this.Invalidate(); }
		}
		
		private string _m_gate_type;
		[STNodeProperty("gate_type", "gate_type")]
		public string m_gate_type
		{
			get { return _m_gate_type; }
			set { _m_gate_type = value; this.Invalidate(); }
		}
		
		private bool _m_has_correct_keycard;
		[STNodeProperty("has_correct_keycard", "has_correct_keycard")]
		public bool m_has_correct_keycard
		{
			get { return _m_has_correct_keycard; }
			set { _m_has_correct_keycard = value; this.Invalidate(); }
		}
		
		private int _m_cutting_tool_level;
		[STNodeProperty("cutting_tool_level", "cutting_tool_level")]
		public int m_cutting_tool_level
		{
			get { return _m_cutting_tool_level; }
			set { _m_cutting_tool_level = value; this.Invalidate(); }
		}
		
		private bool _m_is_locked;
		[STNodeProperty("is_locked", "is_locked")]
		public bool m_is_locked
		{
			get { return _m_is_locked; }
			set { _m_is_locked = value; this.Invalidate(); }
		}
		
		private bool _m_is_powered;
		[STNodeProperty("is_powered", "is_powered")]
		public bool m_is_powered
		{
			get { return _m_is_powered; }
			set { _m_is_powered = value; this.Invalidate(); }
		}
		
		private bool _m_is_cutting_complete;
		[STNodeProperty("is_cutting_complete", "is_cutting_complete")]
		public bool m_is_cutting_complete
		{
			get { return _m_is_cutting_complete; }
			set { _m_is_cutting_complete = value; this.Invalidate(); }
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
			
			this.Title = "DoorStatus";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
		}
	}
}
#endif
