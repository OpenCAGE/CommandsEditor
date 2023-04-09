using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PointTracker : STNode
	{
		private cVector3 _m_origin_offset;
		[STNodeProperty("origin_offset", "origin_offset")]
		public cVector3 m_origin_offset
		{
			get { return _m_origin_offset; }
			set { _m_origin_offset = value; this.Invalidate(); }
		}
		
		private float _m_max_speed;
		[STNodeProperty("max_speed", "max_speed")]
		public float m_max_speed
		{
			get { return _m_max_speed; }
			set { _m_max_speed = value; this.Invalidate(); }
		}
		
		private float _m_damping_factor;
		[STNodeProperty("damping_factor", "damping_factor")]
		public float m_damping_factor
		{
			get { return _m_damping_factor; }
			set { _m_damping_factor = value; this.Invalidate(); }
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
			
			this.Title = "PointTracker";
			
			this.InputOptions.Add("origin", typeof(cVector3), false);
			this.InputOptions.Add("target", typeof(cVector3), false);
			this.InputOptions.Add("target_offset", typeof(cVector3), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("result", typeof(cTransform), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
