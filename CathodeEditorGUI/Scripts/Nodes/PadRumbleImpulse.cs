#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PadRumbleImpulse : STNode
	{
		private float _m_low_frequency_rumble;
		[STNodeProperty("low_frequency_rumble", "low_frequency_rumble")]
		public float m_low_frequency_rumble
		{
			get { return _m_low_frequency_rumble; }
			set { _m_low_frequency_rumble = value; this.Invalidate(); }
		}
		
		private float _m_high_frequency_rumble;
		[STNodeProperty("high_frequency_rumble", "high_frequency_rumble")]
		public float m_high_frequency_rumble
		{
			get { return _m_high_frequency_rumble; }
			set { _m_high_frequency_rumble = value; this.Invalidate(); }
		}
		
		private float _m_left_trigger_impulse;
		[STNodeProperty("left_trigger_impulse", "left_trigger_impulse")]
		public float m_left_trigger_impulse
		{
			get { return _m_left_trigger_impulse; }
			set { _m_left_trigger_impulse = value; this.Invalidate(); }
		}
		
		private float _m_right_trigger_impulse;
		[STNodeProperty("right_trigger_impulse", "right_trigger_impulse")]
		public float m_right_trigger_impulse
		{
			get { return _m_right_trigger_impulse; }
			set { _m_right_trigger_impulse = value; this.Invalidate(); }
		}
		
		private float _m_aim_trigger_impulse;
		[STNodeProperty("aim_trigger_impulse", "aim_trigger_impulse")]
		public float m_aim_trigger_impulse
		{
			get { return _m_aim_trigger_impulse; }
			set { _m_aim_trigger_impulse = value; this.Invalidate(); }
		}
		
		private float _m_shoot_trigger_impulse;
		[STNodeProperty("shoot_trigger_impulse", "shoot_trigger_impulse")]
		public float m_shoot_trigger_impulse
		{
			get { return _m_shoot_trigger_impulse; }
			set { _m_shoot_trigger_impulse = value; this.Invalidate(); }
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
			
			this.Title = "PadRumbleImpulse";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
