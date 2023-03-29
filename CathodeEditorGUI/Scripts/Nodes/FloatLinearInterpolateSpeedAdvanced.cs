#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FloatLinearInterpolateSpeedAdvanced : STNode
	{
		private float _m_Initial_Value;
		[STNodeProperty("Initial_Value", "Initial_Value")]
		public float m_Initial_Value
		{
			get { return _m_Initial_Value; }
			set { _m_Initial_Value = value; this.Invalidate(); }
		}
		
		private float _m_Min_Value;
		[STNodeProperty("Min_Value", "Min_Value")]
		public float m_Min_Value
		{
			get { return _m_Min_Value; }
			set { _m_Min_Value = value; this.Invalidate(); }
		}
		
		private float _m_Max_Value;
		[STNodeProperty("Max_Value", "Max_Value")]
		public float m_Max_Value
		{
			get { return _m_Max_Value; }
			set { _m_Max_Value = value; this.Invalidate(); }
		}
		
		private float _m_Speed;
		[STNodeProperty("Speed", "Speed")]
		public float m_Speed
		{
			get { return _m_Speed; }
			set { _m_Speed = value; this.Invalidate(); }
		}
		
		private bool _m_PingPong;
		[STNodeProperty("PingPong", "PingPong")]
		public bool m_PingPong
		{
			get { return _m_PingPong; }
			set { _m_PingPong = value; this.Invalidate(); }
		}
		
		private bool _m_Loop;
		[STNodeProperty("Loop", "Loop")]
		public bool m_Loop
		{
			get { return _m_Loop; }
			set { _m_Loop = value; this.Invalidate(); }
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
			
			this.Title = "FloatLinearInterpolateSpeedAdvanced";
			
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_finished", typeof(void), false);
			this.OutputOptions.Add("on_think", typeof(void), false);
			this.OutputOptions.Add("trigger_on_min", typeof(void), false);
			this.OutputOptions.Add("trigger_on_max", typeof(void), false);
			this.OutputOptions.Add("trigger_on_loop", typeof(void), false);
			this.OutputOptions.Add("Result", typeof(float), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
