using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SpeechScript : STNode
	{
		private string _m_speech_priority;
		[STNodeProperty("speech_priority", "speech_priority")]
		public string m_speech_priority
		{
			get { return _m_speech_priority; }
			set { _m_speech_priority = value; this.Invalidate(); }
		}
		
		private bool _m_is_occludable;
		[STNodeProperty("is_occludable", "is_occludable")]
		public bool m_is_occludable
		{
			get { return _m_is_occludable; }
			set { _m_is_occludable = value; this.Invalidate(); }
		}
		
		private string _m_line_01_event;
		[STNodeProperty("line_01_event", "line_01_event")]
		public string m_line_01_event
		{
			get { return _m_line_01_event; }
			set { _m_line_01_event = value; this.Invalidate(); }
		}
		
		private int _m_line_01_character;
		[STNodeProperty("line_01_character", "line_01_character")]
		public int m_line_01_character
		{
			get { return _m_line_01_character; }
			set { _m_line_01_character = value; this.Invalidate(); }
		}
		
		private float _m_line_02_delay;
		[STNodeProperty("line_02_delay", "line_02_delay")]
		public float m_line_02_delay
		{
			get { return _m_line_02_delay; }
			set { _m_line_02_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_02_event;
		[STNodeProperty("line_02_event", "line_02_event")]
		public string m_line_02_event
		{
			get { return _m_line_02_event; }
			set { _m_line_02_event = value; this.Invalidate(); }
		}
		
		private int _m_line_02_character;
		[STNodeProperty("line_02_character", "line_02_character")]
		public int m_line_02_character
		{
			get { return _m_line_02_character; }
			set { _m_line_02_character = value; this.Invalidate(); }
		}
		
		private float _m_line_03_delay;
		[STNodeProperty("line_03_delay", "line_03_delay")]
		public float m_line_03_delay
		{
			get { return _m_line_03_delay; }
			set { _m_line_03_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_03_event;
		[STNodeProperty("line_03_event", "line_03_event")]
		public string m_line_03_event
		{
			get { return _m_line_03_event; }
			set { _m_line_03_event = value; this.Invalidate(); }
		}
		
		private int _m_line_03_character;
		[STNodeProperty("line_03_character", "line_03_character")]
		public int m_line_03_character
		{
			get { return _m_line_03_character; }
			set { _m_line_03_character = value; this.Invalidate(); }
		}
		
		private float _m_line_04_delay;
		[STNodeProperty("line_04_delay", "line_04_delay")]
		public float m_line_04_delay
		{
			get { return _m_line_04_delay; }
			set { _m_line_04_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_04_event;
		[STNodeProperty("line_04_event", "line_04_event")]
		public string m_line_04_event
		{
			get { return _m_line_04_event; }
			set { _m_line_04_event = value; this.Invalidate(); }
		}
		
		private int _m_line_04_character;
		[STNodeProperty("line_04_character", "line_04_character")]
		public int m_line_04_character
		{
			get { return _m_line_04_character; }
			set { _m_line_04_character = value; this.Invalidate(); }
		}
		
		private float _m_line_05_delay;
		[STNodeProperty("line_05_delay", "line_05_delay")]
		public float m_line_05_delay
		{
			get { return _m_line_05_delay; }
			set { _m_line_05_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_05_event;
		[STNodeProperty("line_05_event", "line_05_event")]
		public string m_line_05_event
		{
			get { return _m_line_05_event; }
			set { _m_line_05_event = value; this.Invalidate(); }
		}
		
		private int _m_line_05_character;
		[STNodeProperty("line_05_character", "line_05_character")]
		public int m_line_05_character
		{
			get { return _m_line_05_character; }
			set { _m_line_05_character = value; this.Invalidate(); }
		}
		
		private float _m_line_06_delay;
		[STNodeProperty("line_06_delay", "line_06_delay")]
		public float m_line_06_delay
		{
			get { return _m_line_06_delay; }
			set { _m_line_06_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_06_event;
		[STNodeProperty("line_06_event", "line_06_event")]
		public string m_line_06_event
		{
			get { return _m_line_06_event; }
			set { _m_line_06_event = value; this.Invalidate(); }
		}
		
		private int _m_line_06_character;
		[STNodeProperty("line_06_character", "line_06_character")]
		public int m_line_06_character
		{
			get { return _m_line_06_character; }
			set { _m_line_06_character = value; this.Invalidate(); }
		}
		
		private float _m_line_07_delay;
		[STNodeProperty("line_07_delay", "line_07_delay")]
		public float m_line_07_delay
		{
			get { return _m_line_07_delay; }
			set { _m_line_07_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_07_event;
		[STNodeProperty("line_07_event", "line_07_event")]
		public string m_line_07_event
		{
			get { return _m_line_07_event; }
			set { _m_line_07_event = value; this.Invalidate(); }
		}
		
		private int _m_line_07_character;
		[STNodeProperty("line_07_character", "line_07_character")]
		public int m_line_07_character
		{
			get { return _m_line_07_character; }
			set { _m_line_07_character = value; this.Invalidate(); }
		}
		
		private float _m_line_08_delay;
		[STNodeProperty("line_08_delay", "line_08_delay")]
		public float m_line_08_delay
		{
			get { return _m_line_08_delay; }
			set { _m_line_08_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_08_event;
		[STNodeProperty("line_08_event", "line_08_event")]
		public string m_line_08_event
		{
			get { return _m_line_08_event; }
			set { _m_line_08_event = value; this.Invalidate(); }
		}
		
		private int _m_line_08_character;
		[STNodeProperty("line_08_character", "line_08_character")]
		public int m_line_08_character
		{
			get { return _m_line_08_character; }
			set { _m_line_08_character = value; this.Invalidate(); }
		}
		
		private float _m_line_09_delay;
		[STNodeProperty("line_09_delay", "line_09_delay")]
		public float m_line_09_delay
		{
			get { return _m_line_09_delay; }
			set { _m_line_09_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_09_event;
		[STNodeProperty("line_09_event", "line_09_event")]
		public string m_line_09_event
		{
			get { return _m_line_09_event; }
			set { _m_line_09_event = value; this.Invalidate(); }
		}
		
		private int _m_line_09_character;
		[STNodeProperty("line_09_character", "line_09_character")]
		public int m_line_09_character
		{
			get { return _m_line_09_character; }
			set { _m_line_09_character = value; this.Invalidate(); }
		}
		
		private float _m_line_10_delay;
		[STNodeProperty("line_10_delay", "line_10_delay")]
		public float m_line_10_delay
		{
			get { return _m_line_10_delay; }
			set { _m_line_10_delay = value; this.Invalidate(); }
		}
		
		private string _m_line_10_event;
		[STNodeProperty("line_10_event", "line_10_event")]
		public string m_line_10_event
		{
			get { return _m_line_10_event; }
			set { _m_line_10_event = value; this.Invalidate(); }
		}
		
		private int _m_line_10_character;
		[STNodeProperty("line_10_character", "line_10_character")]
		public int m_line_10_character
		{
			get { return _m_line_10_character; }
			set { _m_line_10_character = value; this.Invalidate(); }
		}
		
		private bool _m_restore_on_checkpoint;
		[STNodeProperty("restore_on_checkpoint", "restore_on_checkpoint")]
		public bool m_restore_on_checkpoint
		{
			get { return _m_restore_on_checkpoint; }
			set { _m_restore_on_checkpoint = value; this.Invalidate(); }
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
			
			this.Title = "SpeechScript";
			
			this.InputOptions.Add("character_01", typeof(STNode), false);
			this.InputOptions.Add("character_02", typeof(STNode), false);
			this.InputOptions.Add("character_03", typeof(STNode), false);
			this.InputOptions.Add("character_04", typeof(STNode), false);
			this.InputOptions.Add("character_05", typeof(STNode), false);
			this.InputOptions.Add("alt_character_01", typeof(STNode), false);
			this.InputOptions.Add("alt_character_02", typeof(STNode), false);
			this.InputOptions.Add("alt_character_03", typeof(STNode), false);
			this.InputOptions.Add("alt_character_04", typeof(STNode), false);
			this.InputOptions.Add("alt_character_05", typeof(STNode), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_script_ended", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
