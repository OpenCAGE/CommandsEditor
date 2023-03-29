#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class UpdateSubObjective : STNode
	{
		private int _m_slot_number;
		[STNodeProperty("slot_number", "slot_number")]
		public int m_slot_number
		{
			get { return _m_slot_number; }
			set { _m_slot_number = value; this.Invalidate(); }
		}
		
		private bool _m_show_message;
		[STNodeProperty("show_message", "show_message")]
		public bool m_show_message
		{
			get { return _m_show_message; }
			set { _m_show_message = value; this.Invalidate(); }
		}
		
		private bool _m_clear_objective;
		[STNodeProperty("clear_objective", "clear_objective")]
		public bool m_clear_objective
		{
			get { return _m_clear_objective; }
			set { _m_clear_objective = value; this.Invalidate(); }
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
			
			this.Title = "UpdateSubObjective";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
