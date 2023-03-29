#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MusicTrigger : STNode
	{
		private string _m_music_event;
		[STNodeProperty("music_event", "music_event")]
		public string m_music_event
		{
			get { return _m_music_event; }
			set { _m_music_event = value; this.Invalidate(); }
		}
		
		private float _m_smooth_rate;
		[STNodeProperty("smooth_rate", "smooth_rate")]
		public float m_smooth_rate
		{
			get { return _m_smooth_rate; }
			set { _m_smooth_rate = value; this.Invalidate(); }
		}
		
		private float _m_queue_time;
		[STNodeProperty("queue_time", "queue_time")]
		public float m_queue_time
		{
			get { return _m_queue_time; }
			set { _m_queue_time = value; this.Invalidate(); }
		}
		
		private bool _m_interrupt_all;
		[STNodeProperty("interrupt_all", "interrupt_all")]
		public bool m_interrupt_all
		{
			get { return _m_interrupt_all; }
			set { _m_interrupt_all = value; this.Invalidate(); }
		}
		
		private bool _m_trigger_once;
		[STNodeProperty("trigger_once", "trigger_once")]
		public bool m_trigger_once
		{
			get { return _m_trigger_once; }
			set { _m_trigger_once = value; this.Invalidate(); }
		}
		
		private string _m_rtpc_set_mode;
		[STNodeProperty("rtpc_set_mode", "rtpc_set_mode")]
		public string m_rtpc_set_mode
		{
			get { return _m_rtpc_set_mode; }
			set { _m_rtpc_set_mode = value; this.Invalidate(); }
		}
		
		private float _m_rtpc_target_value;
		[STNodeProperty("rtpc_target_value", "rtpc_target_value")]
		public float m_rtpc_target_value
		{
			get { return _m_rtpc_target_value; }
			set { _m_rtpc_target_value = value; this.Invalidate(); }
		}
		
		private float _m_rtpc_duration;
		[STNodeProperty("rtpc_duration", "rtpc_duration")]
		public float m_rtpc_duration
		{
			get { return _m_rtpc_duration; }
			set { _m_rtpc_duration = value; this.Invalidate(); }
		}
		
		private string _m_rtpc_set_return_mode;
		[STNodeProperty("rtpc_set_return_mode", "rtpc_set_return_mode")]
		public string m_rtpc_set_return_mode
		{
			get { return _m_rtpc_set_return_mode; }
			set { _m_rtpc_set_return_mode = value; this.Invalidate(); }
		}
		
		private float _m_rtpc_return_value;
		[STNodeProperty("rtpc_return_value", "rtpc_return_value")]
		public float m_rtpc_return_value
		{
			get { return _m_rtpc_return_value; }
			set { _m_rtpc_return_value = value; this.Invalidate(); }
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
			
			this.Title = "MusicTrigger";
			
			this.InputOptions.Add("connected_object", typeof(STNode), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_triggered", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
