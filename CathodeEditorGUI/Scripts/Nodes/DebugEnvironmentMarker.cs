#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugEnvironmentMarker : STNode
	{
		private string _m_text;
		[STNodeProperty("text", "text")]
		public string m_text
		{
			get { return _m_text; }
			set { _m_text = value; this.Invalidate(); }
		}
		
		private string _m_namespace;
		[STNodeProperty("namespace", "namespace")]
		public string m_namespace
		{
			get { return _m_namespace; }
			set { _m_namespace = value; this.Invalidate(); }
		}
		
		private float _m_size;
		[STNodeProperty("size", "size")]
		public float m_size
		{
			get { return _m_size; }
			set { _m_size = value; this.Invalidate(); }
		}
		
		private cVector3 _m_colour;
		[STNodeProperty("colour", "colour")]
		public cVector3 m_colour
		{
			get { return _m_colour; }
			set { _m_colour = value; this.Invalidate(); }
		}
		
		private cTransform _m_world_pos;
		[STNodeProperty("world_pos", "world_pos")]
		public cTransform m_world_pos
		{
			get { return _m_world_pos; }
			set { _m_world_pos = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private bool _m_scale_with_distance;
		[STNodeProperty("scale_with_distance", "scale_with_distance")]
		public bool m_scale_with_distance
		{
			get { return _m_scale_with_distance; }
			set { _m_scale_with_distance = value; this.Invalidate(); }
		}
		
		private int _m_max_string_length;
		[STNodeProperty("max_string_length", "max_string_length")]
		public int m_max_string_length
		{
			get { return _m_max_string_length; }
			set { _m_max_string_length = value; this.Invalidate(); }
		}
		
		private float _m_scroll_speed;
		[STNodeProperty("scroll_speed", "scroll_speed")]
		public float m_scroll_speed
		{
			get { return _m_scroll_speed; }
			set { _m_scroll_speed = value; this.Invalidate(); }
		}
		
		private bool _m_show_distance_from_target;
		[STNodeProperty("show_distance_from_target", "show_distance_from_target")]
		public bool m_show_distance_from_target
		{
			get { return _m_show_distance_from_target; }
			set { _m_show_distance_from_target = value; this.Invalidate(); }
		}
		
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_pause_on_reset;
		[STNodeProperty("pause_on_reset", "pause_on_reset")]
		public bool m_pause_on_reset
		{
			get { return _m_pause_on_reset; }
			set { _m_pause_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "DebugEnvironmentMarker";
			
			this.InputOptions.Add("target", typeof(STNode), false);
			this.InputOptions.Add("float_input", typeof(float), false);
			this.InputOptions.Add("int_input", typeof(int), false);
			this.InputOptions.Add("bool_input", typeof(bool), false);
			this.InputOptions.Add("vector_input", typeof(cVector3), false);
			this.InputOptions.Add("enum_input", typeof(cEnum), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
