#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_PlayNPCBark : STNode
	{
		private float _m_queue_time;
		[STNodeProperty("queue_time", "queue_time")]
		public float m_queue_time
		{
			get { return _m_queue_time; }
			set { _m_queue_time = value; this.Invalidate(); }
		}
		
		private string _m_sound_event;
		[STNodeProperty("sound_event", "sound_event")]
		public string m_sound_event
		{
			get { return _m_sound_event; }
			set { _m_sound_event = value; this.Invalidate(); }
		}
		
		private string _m_speech_priority;
		[STNodeProperty("speech_priority", "speech_priority")]
		public string m_speech_priority
		{
			get { return _m_speech_priority; }
			set { _m_speech_priority = value; this.Invalidate(); }
		}
		
		private string _m_dialogue_mode;
		[STNodeProperty("dialogue_mode", "dialogue_mode")]
		public string m_dialogue_mode
		{
			get { return _m_dialogue_mode; }
			set { _m_dialogue_mode = value; this.Invalidate(); }
		}
		
		private string _m_action;
		[STNodeProperty("action", "action")]
		public string m_action
		{
			get { return _m_action; }
			set { _m_action = value; this.Invalidate(); }
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
			
			this.Title = "CHR_PlayNPCBark";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("on_speech_started", typeof(void), false);
			this.OutputOptions.Add("on_speech_finished", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
#endif
