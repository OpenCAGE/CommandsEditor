using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FloatModulateRandom : STNode
	{
		private string _m_switch_on_anim;
		[STNodeProperty("switch_on_anim", "switch_on_anim")]
		public string m_switch_on_anim
		{
			get { return _m_switch_on_anim; }
			set { _m_switch_on_anim = value; this.Invalidate(); }
		}
		
		private float _m_switch_on_delay;
		[STNodeProperty("switch_on_delay", "switch_on_delay")]
		public float m_switch_on_delay
		{
			get { return _m_switch_on_delay; }
			set { _m_switch_on_delay = value; this.Invalidate(); }
		}
		
		private float _m_switch_on_custom_frequency;
		[STNodeProperty("switch_on_custom_frequency", "switch_on_custom_frequency")]
		public float m_switch_on_custom_frequency
		{
			get { return _m_switch_on_custom_frequency; }
			set { _m_switch_on_custom_frequency = value; this.Invalidate(); }
		}
		
		private float _m_switch_on_duration;
		[STNodeProperty("switch_on_duration", "switch_on_duration")]
		public float m_switch_on_duration
		{
			get { return _m_switch_on_duration; }
			set { _m_switch_on_duration = value; this.Invalidate(); }
		}
		
		private string _m_switch_off_anim;
		[STNodeProperty("switch_off_anim", "switch_off_anim")]
		public string m_switch_off_anim
		{
			get { return _m_switch_off_anim; }
			set { _m_switch_off_anim = value; this.Invalidate(); }
		}
		
		private float _m_switch_off_custom_frequency;
		[STNodeProperty("switch_off_custom_frequency", "switch_off_custom_frequency")]
		public float m_switch_off_custom_frequency
		{
			get { return _m_switch_off_custom_frequency; }
			set { _m_switch_off_custom_frequency = value; this.Invalidate(); }
		}
		
		private float _m_switch_off_duration;
		[STNodeProperty("switch_off_duration", "switch_off_duration")]
		public float m_switch_off_duration
		{
			get { return _m_switch_off_duration; }
			set { _m_switch_off_duration = value; this.Invalidate(); }
		}
		
		private string _m_behaviour_anim;
		[STNodeProperty("behaviour_anim", "behaviour_anim")]
		public string m_behaviour_anim
		{
			get { return _m_behaviour_anim; }
			set { _m_behaviour_anim = value; this.Invalidate(); }
		}
		
		private float _m_behaviour_frequency;
		[STNodeProperty("behaviour_frequency", "behaviour_frequency")]
		public float m_behaviour_frequency
		{
			get { return _m_behaviour_frequency; }
			set { _m_behaviour_frequency = value; this.Invalidate(); }
		}
		
		private float _m_behaviour_frequency_variance;
		[STNodeProperty("behaviour_frequency_variance", "behaviour_frequency_variance")]
		public float m_behaviour_frequency_variance
		{
			get { return _m_behaviour_frequency_variance; }
			set { _m_behaviour_frequency_variance = value; this.Invalidate(); }
		}
		
		private float _m_behaviour_offset;
		[STNodeProperty("behaviour_offset", "behaviour_offset")]
		public float m_behaviour_offset
		{
			get { return _m_behaviour_offset; }
			set { _m_behaviour_offset = value; this.Invalidate(); }
		}
		
		private float _m_pulse_modulation;
		[STNodeProperty("pulse_modulation", "pulse_modulation")]
		public float m_pulse_modulation
		{
			get { return _m_pulse_modulation; }
			set { _m_pulse_modulation = value; this.Invalidate(); }
		}
		
		private float _m_oscillate_range_min;
		[STNodeProperty("oscillate_range_min", "oscillate_range_min")]
		public float m_oscillate_range_min
		{
			get { return _m_oscillate_range_min; }
			set { _m_oscillate_range_min = value; this.Invalidate(); }
		}
		
		private float _m_sparking_speed;
		[STNodeProperty("sparking_speed", "sparking_speed")]
		public float m_sparking_speed
		{
			get { return _m_sparking_speed; }
			set { _m_sparking_speed = value; this.Invalidate(); }
		}
		
		private float _m_blink_rate;
		[STNodeProperty("blink_rate", "blink_rate")]
		public float m_blink_rate
		{
			get { return _m_blink_rate; }
			set { _m_blink_rate = value; this.Invalidate(); }
		}
		
		private float _m_blink_range_min;
		[STNodeProperty("blink_range_min", "blink_range_min")]
		public float m_blink_range_min
		{
			get { return _m_blink_range_min; }
			set { _m_blink_range_min = value; this.Invalidate(); }
		}
		
		private float _m_flicker_rate;
		[STNodeProperty("flicker_rate", "flicker_rate")]
		public float m_flicker_rate
		{
			get { return _m_flicker_rate; }
			set { _m_flicker_rate = value; this.Invalidate(); }
		}
		
		private float _m_flicker_off_rate;
		[STNodeProperty("flicker_off_rate", "flicker_off_rate")]
		public float m_flicker_off_rate
		{
			get { return _m_flicker_off_rate; }
			set { _m_flicker_off_rate = value; this.Invalidate(); }
		}
		
		private float _m_flicker_range_min;
		[STNodeProperty("flicker_range_min", "flicker_range_min")]
		public float m_flicker_range_min
		{
			get { return _m_flicker_range_min; }
			set { _m_flicker_range_min = value; this.Invalidate(); }
		}
		
		private float _m_flicker_off_range_min;
		[STNodeProperty("flicker_off_range_min", "flicker_off_range_min")]
		public float m_flicker_off_range_min
		{
			get { return _m_flicker_off_range_min; }
			set { _m_flicker_off_range_min = value; this.Invalidate(); }
		}
		
		private bool _m_disable_behaviour;
		[STNodeProperty("disable_behaviour", "disable_behaviour")]
		public bool m_disable_behaviour
		{
			get { return _m_disable_behaviour; }
			set { _m_disable_behaviour = value; this.Invalidate(); }
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
			
			this.Title = "FloatModulateRandom";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_full_switched_on", typeof(void), false);
			this.OutputOptions.Add("on_full_switched_off", typeof(void), false);
			this.OutputOptions.Add("on_think", typeof(void), false);
			this.OutputOptions.Add("Result", typeof(float), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
