#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class LightAdaptationSettings : STNode
	{
		private string _m_adaptation_mechanism;
		[STNodeProperty("adaptation_mechanism", "adaptation_mechanism")]
		public string m_adaptation_mechanism
		{
			get { return _m_adaptation_mechanism; }
			set { _m_adaptation_mechanism = value; this.Invalidate(); }
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
			
			this.Title = "LightAdaptationSettings";
			
			this.InputOptions.Add("fast_neural_t0", typeof(float), false);
			this.InputOptions.Add("slow_neural_t0", typeof(float), false);
			this.InputOptions.Add("pigment_bleaching_t0", typeof(float), false);
			this.InputOptions.Add("fb_luminance_to_candelas_per_m2", typeof(float), false);
			this.InputOptions.Add("max_adaptation_lum", typeof(float), false);
			this.InputOptions.Add("min_adaptation_lum", typeof(float), false);
			this.InputOptions.Add("adaptation_percentile", typeof(float), false);
			this.InputOptions.Add("low_bracket", typeof(float), false);
			this.InputOptions.Add("high_bracket", typeof(float), false);
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
