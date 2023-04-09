using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundSetRTPC : STNode
	{
		private string _m_rtpc_name;
		[STNodeProperty("rtpc_name", "rtpc_name")]
		public string m_rtpc_name
		{
			get { return _m_rtpc_name; }
			set { _m_rtpc_name = value; this.Invalidate(); }
		}
		
		private float _m_smooth_rate;
		[STNodeProperty("smooth_rate", "smooth_rate")]
		public float m_smooth_rate
		{
			get { return _m_smooth_rate; }
			set { _m_smooth_rate = value; this.Invalidate(); }
		}
		
		private bool _m_start_on;
		[STNodeProperty("start_on", "start_on")]
		public bool m_start_on
		{
			get { return _m_start_on; }
			set { _m_start_on = value; this.Invalidate(); }
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
			
			this.Title = "SoundSetRTPC";
			
			this.InputOptions.Add("rtpc_value", typeof(float), false);
			this.InputOptions.Add("sound_object", typeof(STNode), false);
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
