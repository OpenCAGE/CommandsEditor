using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PopupMessage : STNode
	{
		private string _m_header_text;
		[STNodeProperty("header_text", "header_text")]
		public string m_header_text
		{
			get { return _m_header_text; }
			set { _m_header_text = value; this.Invalidate(); }
		}
		
		private string _m_main_text;
		[STNodeProperty("main_text", "main_text")]
		public string m_main_text
		{
			get { return _m_main_text; }
			set { _m_main_text = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private string _m_sound_event;
		[STNodeProperty("sound_event", "sound_event")]
		public string m_sound_event
		{
			get { return _m_sound_event; }
			set { _m_sound_event = value; this.Invalidate(); }
		}
		
		private string _m_icon_keyframe;
		[STNodeProperty("icon_keyframe", "icon_keyframe")]
		public string m_icon_keyframe
		{
			get { return _m_icon_keyframe; }
			set { _m_icon_keyframe = value; this.Invalidate(); }
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
			
			this.Title = "PopupMessage";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("display", typeof(void), false);
			this.OutputOptions.Add("finished", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
