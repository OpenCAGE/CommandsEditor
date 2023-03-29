#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Thinker : STNode
	{
		private float _m_delay_between_triggers;
		[STNodeProperty("delay_between_triggers", "delay_between_triggers")]
		public float m_delay_between_triggers
		{
			get { return _m_delay_between_triggers; }
			set { _m_delay_between_triggers = value; this.Invalidate(); }
		}
		
		private bool _m_is_continuous;
		[STNodeProperty("is_continuous", "is_continuous")]
		public bool m_is_continuous
		{
			get { return _m_is_continuous; }
			set { _m_is_continuous = value; this.Invalidate(); }
		}
		
		private bool _m_use_random_start;
		[STNodeProperty("use_random_start", "use_random_start")]
		public bool m_use_random_start
		{
			get { return _m_use_random_start; }
			set { _m_use_random_start = value; this.Invalidate(); }
		}
		
		private float _m_random_start_delay;
		[STNodeProperty("random_start_delay", "random_start_delay")]
		public float m_random_start_delay
		{
			get { return _m_random_start_delay; }
			set { _m_random_start_delay = value; this.Invalidate(); }
		}
		
		private float _m_total_thinking_time;
		[STNodeProperty("total_thinking_time", "total_thinking_time")]
		public float m_total_thinking_time
		{
			get { return _m_total_thinking_time; }
			set { _m_total_thinking_time = value; this.Invalidate(); }
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
			
			this.Title = "Thinker";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_think", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
