using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RewireSystem : STNode
	{
		private string _m_display_name;
		[STNodeProperty("display_name", "display_name")]
		public string m_display_name
		{
			get { return _m_display_name; }
			set { _m_display_name = value; this.Invalidate(); }
		}
		
		private string _m_display_name_enum;
		[STNodeProperty("display_name_enum", "display_name_enum")]
		public string m_display_name_enum
		{
			get { return _m_display_name_enum; }
			set { _m_display_name_enum = value; this.Invalidate(); }
		}
		
		private bool _m_on_by_default;
		[STNodeProperty("on_by_default", "on_by_default")]
		public bool m_on_by_default
		{
			get { return _m_on_by_default; }
			set { _m_on_by_default = value; this.Invalidate(); }
		}
		
		private int _m_running_cost;
		[STNodeProperty("running_cost", "running_cost")]
		public int m_running_cost
		{
			get { return _m_running_cost; }
			set { _m_running_cost = value; this.Invalidate(); }
		}
		
		private string _m_system_type;
		[STNodeProperty("system_type", "system_type")]
		public string m_system_type
		{
			get { return _m_system_type; }
			set { _m_system_type = value; this.Invalidate(); }
		}
		
		private string _m_map_name;
		[STNodeProperty("map_name", "map_name")]
		public string m_map_name
		{
			get { return _m_map_name; }
			set { _m_map_name = value; this.Invalidate(); }
		}
		
		private string _m_element_name;
		[STNodeProperty("element_name", "element_name")]
		public string m_element_name
		{
			get { return _m_element_name; }
			set { _m_element_name = value; this.Invalidate(); }
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
			
			this.Title = "RewireSystem";
			
			this.InputOptions.Add("world_pos", typeof(cTransform), false);
			this.InputOptions.Add("turn_on_system", typeof(void), false);
			this.InputOptions.Add("turn_off_system", typeof(void), false);
			
			this.OutputOptions.Add("on", typeof(void), false);
			this.OutputOptions.Add("off", typeof(void), false);
		}
	}
}
