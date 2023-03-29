#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugText : STNode
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
		
		private int _m_size;
		[STNodeProperty("size", "size")]
		public int m_size
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
		
		private string _m_alignment;
		[STNodeProperty("alignment", "alignment")]
		public string m_alignment
		{
			get { return _m_alignment; }
			set { _m_alignment = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private bool _m_pause_game;
		[STNodeProperty("pause_game", "pause_game")]
		public bool m_pause_game
		{
			get { return _m_pause_game; }
			set { _m_pause_game = value; this.Invalidate(); }
		}
		
		private bool _m_cancel_pause_with_button_press;
		[STNodeProperty("cancel_pause_with_button_press", "cancel_pause_with_button_press")]
		public bool m_cancel_pause_with_button_press
		{
			get { return _m_cancel_pause_with_button_press; }
			set { _m_cancel_pause_with_button_press = value; this.Invalidate(); }
		}
		
		private int _m_priority;
		[STNodeProperty("priority", "priority")]
		public int m_priority
		{
			get { return _m_priority; }
			set { _m_priority = value; this.Invalidate(); }
		}
		
		private string _m_ci_type;
		[STNodeProperty("ci_type", "ci_type")]
		public string m_ci_type
		{
			get { return _m_ci_type; }
			set { _m_ci_type = value; this.Invalidate(); }
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
			
			this.Title = "DebugText";
			
			this.InputOptions.Add("float_input", typeof(float), false);
			this.InputOptions.Add("int_input", typeof(int), false);
			this.InputOptions.Add("bool_input", typeof(bool), false);
			this.InputOptions.Add("vector_input", typeof(cVector3), false);
			this.InputOptions.Add("enum_input", typeof(cEnum), false);
			this.InputOptions.Add("text_input", typeof(string), false);
			this.InputOptions.Add("clear_all", typeof(void), false);
			this.InputOptions.Add("clear_of_alignment", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("duration_finished", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
