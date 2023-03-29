#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetSubObjective : STNode
	{
		private string _m_title;
		[STNodeProperty("title", "title")]
		public string m_title
		{
			get { return _m_title; }
			set { _m_title = value; this.Invalidate(); }
		}
		
		private string _m_map_description;
		[STNodeProperty("map_description", "map_description")]
		public string m_map_description
		{
			get { return _m_map_description; }
			set { _m_map_description = value; this.Invalidate(); }
		}
		
		private string _m_title_list;
		[STNodeProperty("title_list", "title_list")]
		public string m_title_list
		{
			get { return _m_title_list; }
			set { _m_title_list = value; this.Invalidate(); }
		}
		
		private string _m_map_description_list;
		[STNodeProperty("map_description_list", "map_description_list")]
		public string m_map_description_list
		{
			get { return _m_map_description_list; }
			set { _m_map_description_list = value; this.Invalidate(); }
		}
		
		private int _m_slot_number;
		[STNodeProperty("slot_number", "slot_number")]
		public int m_slot_number
		{
			get { return _m_slot_number; }
			set { _m_slot_number = value; this.Invalidate(); }
		}
		
		private string _m_objective_type;
		[STNodeProperty("objective_type", "objective_type")]
		public string m_objective_type
		{
			get { return _m_objective_type; }
			set { _m_objective_type = value; this.Invalidate(); }
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
			
			this.Title = "SetSubObjective";
			
			this.InputOptions.Add("target_position", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
