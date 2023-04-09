using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SpaceTransform : STNode
	{
		private float _m_yaw_speed;
		[STNodeProperty("yaw_speed", "yaw_speed")]
		public float m_yaw_speed
		{
			get { return _m_yaw_speed; }
			set { _m_yaw_speed = value; this.Invalidate(); }
		}
		
		private float _m_pitch_speed;
		[STNodeProperty("pitch_speed", "pitch_speed")]
		public float m_pitch_speed
		{
			get { return _m_pitch_speed; }
			set { _m_pitch_speed = value; this.Invalidate(); }
		}
		
		private float _m_roll_speed;
		[STNodeProperty("roll_speed", "roll_speed")]
		public float m_roll_speed
		{
			get { return _m_roll_speed; }
			set { _m_roll_speed = value; this.Invalidate(); }
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
			
			this.Title = "SpaceTransform";
			
			this.InputOptions.Add("affected_geometry", typeof(STNode), false);
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
