using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class EFFECT_EntityGenerator : STNode
	{
		private bool _m_trigger_on_reset;
		[STNodeProperty("trigger_on_reset", "trigger_on_reset")]
		public bool m_trigger_on_reset
		{
			get { return _m_trigger_on_reset; }
			set { _m_trigger_on_reset = value; this.Invalidate(); }
		}
		
		private int _m_count;
		[STNodeProperty("count", "count")]
		public int m_count
		{
			get { return _m_count; }
			set { _m_count = value; this.Invalidate(); }
		}
		
		private float _m_spread;
		[STNodeProperty("spread", "spread")]
		public float m_spread
		{
			get { return _m_spread; }
			set { _m_spread = value; this.Invalidate(); }
		}
		
		private float _m_force_min;
		[STNodeProperty("force_min", "force_min")]
		public float m_force_min
		{
			get { return _m_force_min; }
			set { _m_force_min = value; this.Invalidate(); }
		}
		
		private float _m_force_max;
		[STNodeProperty("force_max", "force_max")]
		public float m_force_max
		{
			get { return _m_force_max; }
			set { _m_force_max = value; this.Invalidate(); }
		}
		
		private float _m_force_offset_XY_min;
		[STNodeProperty("force_offset_XY_min", "force_offset_XY_min")]
		public float m_force_offset_XY_min
		{
			get { return _m_force_offset_XY_min; }
			set { _m_force_offset_XY_min = value; this.Invalidate(); }
		}
		
		private float _m_force_offset_XY_max;
		[STNodeProperty("force_offset_XY_max", "force_offset_XY_max")]
		public float m_force_offset_XY_max
		{
			get { return _m_force_offset_XY_max; }
			set { _m_force_offset_XY_max = value; this.Invalidate(); }
		}
		
		private float _m_force_offset_Z_min;
		[STNodeProperty("force_offset_Z_min", "force_offset_Z_min")]
		public float m_force_offset_Z_min
		{
			get { return _m_force_offset_Z_min; }
			set { _m_force_offset_Z_min = value; this.Invalidate(); }
		}
		
		private float _m_force_offset_Z_max;
		[STNodeProperty("force_offset_Z_max", "force_offset_Z_max")]
		public float m_force_offset_Z_max
		{
			get { return _m_force_offset_Z_max; }
			set { _m_force_offset_Z_max = value; this.Invalidate(); }
		}
		
		private float _m_lifetime_min;
		[STNodeProperty("lifetime_min", "lifetime_min")]
		public float m_lifetime_min
		{
			get { return _m_lifetime_min; }
			set { _m_lifetime_min = value; this.Invalidate(); }
		}
		
		private float _m_lifetime_max;
		[STNodeProperty("lifetime_max", "lifetime_max")]
		public float m_lifetime_max
		{
			get { return _m_lifetime_max; }
			set { _m_lifetime_max = value; this.Invalidate(); }
		}
		
		private bool _m_use_local_rotation;
		[STNodeProperty("use_local_rotation", "use_local_rotation")]
		public bool m_use_local_rotation
		{
			get { return _m_use_local_rotation; }
			set { _m_use_local_rotation = value; this.Invalidate(); }
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
			
			this.Title = "EFFECT_EntityGenerator";
			
			this.InputOptions.Add("entities", typeof(STNode), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
