#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RewireAccess_Point : STNode
	{
		private int _m_additional_power;
		[STNodeProperty("additional_power", "additional_power")]
		public int m_additional_power
		{
			get { return _m_additional_power; }
			set { _m_additional_power = value; this.Invalidate(); }
		}
		
		private string _m_display_name;
		[STNodeProperty("display_name", "display_name")]
		public string m_display_name
		{
			get { return _m_display_name; }
			set { _m_display_name = value; this.Invalidate(); }
		}
		
		private string _m_map_element_name;
		[STNodeProperty("map_element_name", "map_element_name")]
		public string m_map_element_name
		{
			get { return _m_map_element_name; }
			set { _m_map_element_name = value; this.Invalidate(); }
		}
		
		private string _m_map_name;
		[STNodeProperty("map_name", "map_name")]
		public string m_map_name
		{
			get { return _m_map_name; }
			set { _m_map_name = value; this.Invalidate(); }
		}
		
		private float _m_map_x_offset;
		[STNodeProperty("map_x_offset", "map_x_offset")]
		public float m_map_x_offset
		{
			get { return _m_map_x_offset; }
			set { _m_map_x_offset = value; this.Invalidate(); }
		}
		
		private float _m_map_y_offset;
		[STNodeProperty("map_y_offset", "map_y_offset")]
		public float m_map_y_offset
		{
			get { return _m_map_y_offset; }
			set { _m_map_y_offset = value; this.Invalidate(); }
		}
		
		private float _m_map_zoom;
		[STNodeProperty("map_zoom", "map_zoom")]
		public float m_map_zoom
		{
			get { return _m_map_zoom; }
			set { _m_map_zoom = value; this.Invalidate(); }
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
			
			this.Title = "RewireAccess_Point";
			
			this.InputOptions.Add("interactive_locations", typeof(string), false);
			this.InputOptions.Add("visible_locations", typeof(string), false);
			this.InputOptions.Add("display_tutorial", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("cancel", typeof(void), false);
			this.InputOptions.Add("finished_closing_container", typeof(void), false);
			
			this.OutputOptions.Add("closed", typeof(void), false);
			this.OutputOptions.Add("ui_breakout_triggered", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("cancelled", typeof(void), false);
			this.OutputOptions.Add("closing_container_finished", typeof(void), false);
		}
	}
}
#endif
