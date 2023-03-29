#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GPU_PFXEmitterReference : STNode
	{
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_deleted;
		[STNodeProperty("deleted", "deleted")]
		public bool m_deleted
		{
			get { return _m_deleted; }
			set { _m_deleted = value; this.Invalidate(); }
		}
		
		private string _m_EFFECT_NAME;
		[STNodeProperty("EFFECT_NAME", "EFFECT_NAME")]
		public string m_EFFECT_NAME
		{
			get { return _m_EFFECT_NAME; }
			set { _m_EFFECT_NAME = value; this.Invalidate(); }
		}
		
		private int _m_SPAWN_NUMBER;
		[STNodeProperty("SPAWN_NUMBER", "SPAWN_NUMBER")]
		public int m_SPAWN_NUMBER
		{
			get { return _m_SPAWN_NUMBER; }
			set { _m_SPAWN_NUMBER = value; this.Invalidate(); }
		}
		
		private float _m_SPAWN_RATE;
		[STNodeProperty("SPAWN_RATE", "SPAWN_RATE")]
		public float m_SPAWN_RATE
		{
			get { return _m_SPAWN_RATE; }
			set { _m_SPAWN_RATE = value; this.Invalidate(); }
		}
		
		private float _m_SPREAD_MIN;
		[STNodeProperty("SPREAD_MIN", "SPREAD_MIN")]
		public float m_SPREAD_MIN
		{
			get { return _m_SPREAD_MIN; }
			set { _m_SPREAD_MIN = value; this.Invalidate(); }
		}
		
		private float _m_SPREAD_MAX;
		[STNodeProperty("SPREAD_MAX", "SPREAD_MAX")]
		public float m_SPREAD_MAX
		{
			get { return _m_SPREAD_MAX; }
			set { _m_SPREAD_MAX = value; this.Invalidate(); }
		}
		
		private float _m_EMITTER_SIZE;
		[STNodeProperty("EMITTER_SIZE", "EMITTER_SIZE")]
		public float m_EMITTER_SIZE
		{
			get { return _m_EMITTER_SIZE; }
			set { _m_EMITTER_SIZE = value; this.Invalidate(); }
		}
		
		private float _m_SPEED;
		[STNodeProperty("SPEED", "SPEED")]
		public float m_SPEED
		{
			get { return _m_SPEED; }
			set { _m_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_SPEED_VAR;
		[STNodeProperty("SPEED_VAR", "SPEED_VAR")]
		public float m_SPEED_VAR
		{
			get { return _m_SPEED_VAR; }
			set { _m_SPEED_VAR = value; this.Invalidate(); }
		}
		
		private float _m_LIFETIME;
		[STNodeProperty("LIFETIME", "LIFETIME")]
		public float m_LIFETIME
		{
			get { return _m_LIFETIME; }
			set { _m_LIFETIME = value; this.Invalidate(); }
		}
		
		private float _m_LIFETIME_VAR;
		[STNodeProperty("LIFETIME_VAR", "LIFETIME_VAR")]
		public float m_LIFETIME_VAR
		{
			get { return _m_LIFETIME_VAR; }
			set { _m_LIFETIME_VAR = value; this.Invalidate(); }
		}
		
		private bool _m_pause_on_reset;
		[STNodeProperty("pause_on_reset", "pause_on_reset")]
		public bool m_pause_on_reset
		{
			get { return _m_pause_on_reset; }
			set { _m_pause_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
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
			
			this.Title = "GPU_PFXEmitterReference";
			
			this.InputOptions.Add("mastered_by_visibility", typeof(STNode), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
