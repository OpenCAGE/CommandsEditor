using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RotateAtSpeed : STNode
	{
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private float _m_speed_X;
		[STNodeProperty("speed_X", "speed_X")]
		public float m_speed_X
		{
			get { return _m_speed_X; }
			set { _m_speed_X = value; this.Invalidate(); }
		}
		
		private float _m_speed_Y;
		[STNodeProperty("speed_Y", "speed_Y")]
		public float m_speed_Y
		{
			get { return _m_speed_Y; }
			set { _m_speed_Y = value; this.Invalidate(); }
		}
		
		private float _m_speed_Z;
		[STNodeProperty("speed_Z", "speed_Z")]
		public float m_speed_Z
		{
			get { return _m_speed_Z; }
			set { _m_speed_Z = value; this.Invalidate(); }
		}
		
		private bool _m_loop;
		[STNodeProperty("loop", "loop")]
		public bool m_loop
		{
			get { return _m_loop; }
			set { _m_loop = value; this.Invalidate(); }
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
			
			this.Title = "RotateAtSpeed";
			
			this.InputOptions.Add("start_pos", typeof(cTransform), false);
			this.InputOptions.Add("origin", typeof(cTransform), false);
			this.InputOptions.Add("timer", typeof(float), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_finished", typeof(void), false);
			this.OutputOptions.Add("on_think", typeof(void), false);
			this.OutputOptions.Add("Result", typeof(cTransform), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
