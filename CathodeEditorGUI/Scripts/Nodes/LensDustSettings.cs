#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class LensDustSettings : STNode
	{
		private float _m_DUST_MAX_REFLECTED_BLOOM_INTENSITY;
		[STNodeProperty("DUST_MAX_REFLECTED_BLOOM_INTENSITY", "DUST_MAX_REFLECTED_BLOOM_INTENSITY")]
		public float m_DUST_MAX_REFLECTED_BLOOM_INTENSITY
		{
			get { return _m_DUST_MAX_REFLECTED_BLOOM_INTENSITY; }
			set { _m_DUST_MAX_REFLECTED_BLOOM_INTENSITY = value; this.Invalidate(); }
		}
		
		private float _m_DUST_REFLECTED_BLOOM_INTENSITY_SCALAR;
		[STNodeProperty("DUST_REFLECTED_BLOOM_INTENSITY_SCALAR", "DUST_REFLECTED_BLOOM_INTENSITY_SCALAR")]
		public float m_DUST_REFLECTED_BLOOM_INTENSITY_SCALAR
		{
			get { return _m_DUST_REFLECTED_BLOOM_INTENSITY_SCALAR; }
			set { _m_DUST_REFLECTED_BLOOM_INTENSITY_SCALAR = value; this.Invalidate(); }
		}
		
		private float _m_DUST_MAX_BLOOM_INTENSITY;
		[STNodeProperty("DUST_MAX_BLOOM_INTENSITY", "DUST_MAX_BLOOM_INTENSITY")]
		public float m_DUST_MAX_BLOOM_INTENSITY
		{
			get { return _m_DUST_MAX_BLOOM_INTENSITY; }
			set { _m_DUST_MAX_BLOOM_INTENSITY = value; this.Invalidate(); }
		}
		
		private float _m_DUST_BLOOM_INTENSITY_SCALAR;
		[STNodeProperty("DUST_BLOOM_INTENSITY_SCALAR", "DUST_BLOOM_INTENSITY_SCALAR")]
		public float m_DUST_BLOOM_INTENSITY_SCALAR
		{
			get { return _m_DUST_BLOOM_INTENSITY_SCALAR; }
			set { _m_DUST_BLOOM_INTENSITY_SCALAR = value; this.Invalidate(); }
		}
		
		private float _m_DUST_THRESHOLD;
		[STNodeProperty("DUST_THRESHOLD", "DUST_THRESHOLD")]
		public float m_DUST_THRESHOLD
		{
			get { return _m_DUST_THRESHOLD; }
			set { _m_DUST_THRESHOLD = value; this.Invalidate(); }
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
			
			this.Title = "LensDustSettings";
			
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
#endif
