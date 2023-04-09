using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FilmGrainSettings : STNode
	{
		private int _m_priority;
		[STNodeProperty("priority", "priority")]
		public int m_priority
		{
			get { return _m_priority; }
			set { _m_priority = value; this.Invalidate(); }
		}
		
		private string _m_blend_mode;
		[STNodeProperty("blend_mode", "blend_mode")]
		public string m_blend_mode
		{
			get { return _m_blend_mode; }
			set { _m_blend_mode = value; this.Invalidate(); }
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
			
			this.Title = "FilmGrainSettings";
			
			this.InputOptions.Add("low_lum_amplifier", typeof(float), false);
			this.InputOptions.Add("mid_lum_amplifier", typeof(float), false);
			this.InputOptions.Add("high_lum_amplifier", typeof(float), false);
			this.InputOptions.Add("low_lum_range", typeof(float), false);
			this.InputOptions.Add("mid_lum_range", typeof(float), false);
			this.InputOptions.Add("high_lum_range", typeof(float), false);
			this.InputOptions.Add("noise_texture_scale", typeof(float), false);
			this.InputOptions.Add("adaptive", typeof(bool), false);
			this.InputOptions.Add("adaptation_scalar", typeof(float), false);
			this.InputOptions.Add("adaptation_time_scalar", typeof(float), false);
			this.InputOptions.Add("unadapted_low_lum_amplifier", typeof(float), false);
			this.InputOptions.Add("unadapted_mid_lum_amplifier", typeof(float), false);
			this.InputOptions.Add("unadapted_high_lum_amplifier", typeof(float), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("intensity", typeof(float), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
