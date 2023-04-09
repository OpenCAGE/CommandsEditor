using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugGraph : STNode
	{
		private float _m_scale;
		[STNodeProperty("scale", "scale")]
		public float m_scale
		{
			get { return _m_scale; }
			set { _m_scale = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private float _m_samples_per_second;
		[STNodeProperty("samples_per_second", "samples_per_second")]
		public float m_samples_per_second
		{
			get { return _m_samples_per_second; }
			set { _m_samples_per_second = value; this.Invalidate(); }
		}
		
		private bool _m_auto_scale;
		[STNodeProperty("auto_scale", "auto_scale")]
		public bool m_auto_scale
		{
			get { return _m_auto_scale; }
			set { _m_auto_scale = value; this.Invalidate(); }
		}
		
		private bool _m_auto_scroll;
		[STNodeProperty("auto_scroll", "auto_scroll")]
		public bool m_auto_scroll
		{
			get { return _m_auto_scroll; }
			set { _m_auto_scroll = value; this.Invalidate(); }
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
			
			this.Title = "DebugGraph";
			
			this.InputOptions.Add("Inputs", typeof(float), false);
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
