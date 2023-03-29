#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DistortionOverlay : STNode
	{
		private string _m_distortion_texture;
		[STNodeProperty("distortion_texture", "distortion_texture")]
		public string m_distortion_texture
		{
			get { return _m_distortion_texture; }
			set { _m_distortion_texture = value; this.Invalidate(); }
		}
		
		private bool _m_alpha_threshold_enabled;
		[STNodeProperty("alpha_threshold_enabled", "alpha_threshold_enabled")]
		public bool m_alpha_threshold_enabled
		{
			get { return _m_alpha_threshold_enabled; }
			set { _m_alpha_threshold_enabled = value; this.Invalidate(); }
		}
		
		private string _m_threshold_texture;
		[STNodeProperty("threshold_texture", "threshold_texture")]
		public string m_threshold_texture
		{
			get { return _m_threshold_texture; }
			set { _m_threshold_texture = value; this.Invalidate(); }
		}
		
		private float _m_range;
		[STNodeProperty("range", "range")]
		public float m_range
		{
			get { return _m_range; }
			set { _m_range = value; this.Invalidate(); }
		}
		
		private float _m_begin_start_time;
		[STNodeProperty("begin_start_time", "begin_start_time")]
		public float m_begin_start_time
		{
			get { return _m_begin_start_time; }
			set { _m_begin_start_time = value; this.Invalidate(); }
		}
		
		private float _m_begin_stop_time;
		[STNodeProperty("begin_stop_time", "begin_stop_time")]
		public float m_begin_stop_time
		{
			get { return _m_begin_stop_time; }
			set { _m_begin_stop_time = value; this.Invalidate(); }
		}
		
		private float _m_end_start_time;
		[STNodeProperty("end_start_time", "end_start_time")]
		public float m_end_start_time
		{
			get { return _m_end_start_time; }
			set { _m_end_start_time = value; this.Invalidate(); }
		}
		
		private float _m_end_stop_time;
		[STNodeProperty("end_stop_time", "end_stop_time")]
		public float m_end_stop_time
		{
			get { return _m_end_stop_time; }
			set { _m_end_stop_time = value; this.Invalidate(); }
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
			
			this.Title = "DistortionOverlay";
			
			this.InputOptions.Add("intensity", typeof(float), false);
			this.InputOptions.Add("time", typeof(float), false);
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
