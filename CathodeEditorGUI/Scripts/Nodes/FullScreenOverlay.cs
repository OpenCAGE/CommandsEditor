using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FullScreenOverlay : STNode
	{
		private string _m_overlay_texture;
		[STNodeProperty("overlay_texture", "overlay_texture")]
		public string m_overlay_texture
		{
			get { return _m_overlay_texture; }
			set { _m_overlay_texture = value; this.Invalidate(); }
		}
		
		private float _m_threshold_value;
		[STNodeProperty("threshold_value", "threshold_value")]
		public float m_threshold_value
		{
			get { return _m_threshold_value; }
			set { _m_threshold_value = value; this.Invalidate(); }
		}
		
		private float _m_threshold_start;
		[STNodeProperty("threshold_start", "threshold_start")]
		public float m_threshold_start
		{
			get { return _m_threshold_start; }
			set { _m_threshold_start = value; this.Invalidate(); }
		}
		
		private float _m_threshold_stop;
		[STNodeProperty("threshold_stop", "threshold_stop")]
		public float m_threshold_stop
		{
			get { return _m_threshold_stop; }
			set { _m_threshold_stop = value; this.Invalidate(); }
		}
		
		private float _m_threshold_range;
		[STNodeProperty("threshold_range", "threshold_range")]
		public float m_threshold_range
		{
			get { return _m_threshold_range; }
			set { _m_threshold_range = value; this.Invalidate(); }
		}
		
		private float _m_alpha_scalar;
		[STNodeProperty("alpha_scalar", "alpha_scalar")]
		public float m_alpha_scalar
		{
			get { return _m_alpha_scalar; }
			set { _m_alpha_scalar = value; this.Invalidate(); }
		}
		
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
			
			this.Title = "FullScreenOverlay";
			
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
