using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class EFFECT_DirectionalPhysics : STNode
	{
		private cVector3 _m_relative_direction;
		[STNodeProperty("relative_direction", "relative_direction")]
		public cVector3 m_relative_direction
		{
			get { return _m_relative_direction; }
			set { _m_relative_direction = value; this.Invalidate(); }
		}
		
		private float _m_effect_distance;
		[STNodeProperty("effect_distance", "effect_distance")]
		public float m_effect_distance
		{
			get { return _m_effect_distance; }
			set { _m_effect_distance = value; this.Invalidate(); }
		}
		
		private float _m_angular_falloff;
		[STNodeProperty("angular_falloff", "angular_falloff")]
		public float m_angular_falloff
		{
			get { return _m_angular_falloff; }
			set { _m_angular_falloff = value; this.Invalidate(); }
		}
		
		private float _m_min_force;
		[STNodeProperty("min_force", "min_force")]
		public float m_min_force
		{
			get { return _m_min_force; }
			set { _m_min_force = value; this.Invalidate(); }
		}
		
		private float _m_max_force;
		[STNodeProperty("max_force", "max_force")]
		public float m_max_force
		{
			get { return _m_max_force; }
			set { _m_max_force = value; this.Invalidate(); }
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
			
			this.Title = "EFFECT_DirectionalPhysics";
			
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
