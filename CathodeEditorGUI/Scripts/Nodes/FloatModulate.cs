#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FloatModulate : STNode
	{
		private string _m_wave_shape;
		[STNodeProperty("wave_shape", "wave_shape")]
		public string m_wave_shape
		{
			get { return _m_wave_shape; }
			set { _m_wave_shape = value; this.Invalidate(); }
		}
		
		private float _m_frequency;
		[STNodeProperty("frequency", "frequency")]
		public float m_frequency
		{
			get { return _m_frequency; }
			set { _m_frequency = value; this.Invalidate(); }
		}
		
		private float _m_phase;
		[STNodeProperty("phase", "phase")]
		public float m_phase
		{
			get { return _m_phase; }
			set { _m_phase = value; this.Invalidate(); }
		}
		
		private float _m_amplitude;
		[STNodeProperty("amplitude", "amplitude")]
		public float m_amplitude
		{
			get { return _m_amplitude; }
			set { _m_amplitude = value; this.Invalidate(); }
		}
		
		private float _m_bias;
		[STNodeProperty("bias", "bias")]
		public float m_bias
		{
			get { return _m_bias; }
			set { _m_bias = value; this.Invalidate(); }
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
			
			this.Title = "FloatModulate";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_think", typeof(void), false);
			this.OutputOptions.Add("Result", typeof(float), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
