using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TorchDynamicMovement : STNode
	{
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private float _m_max_spatial_velocity;
		[STNodeProperty("max_spatial_velocity", "max_spatial_velocity")]
		public float m_max_spatial_velocity
		{
			get { return _m_max_spatial_velocity; }
			set { _m_max_spatial_velocity = value; this.Invalidate(); }
		}
		
		private float _m_max_angular_velocity;
		[STNodeProperty("max_angular_velocity", "max_angular_velocity")]
		public float m_max_angular_velocity
		{
			get { return _m_max_angular_velocity; }
			set { _m_max_angular_velocity = value; this.Invalidate(); }
		}
		
		private float _m_max_position_displacement;
		[STNodeProperty("max_position_displacement", "max_position_displacement")]
		public float m_max_position_displacement
		{
			get { return _m_max_position_displacement; }
			set { _m_max_position_displacement = value; this.Invalidate(); }
		}
		
		private float _m_max_target_displacement;
		[STNodeProperty("max_target_displacement", "max_target_displacement")]
		public float m_max_target_displacement
		{
			get { return _m_max_target_displacement; }
			set { _m_max_target_displacement = value; this.Invalidate(); }
		}
		
		private float _m_position_damping;
		[STNodeProperty("position_damping", "position_damping")]
		public float m_position_damping
		{
			get { return _m_position_damping; }
			set { _m_position_damping = value; this.Invalidate(); }
		}
		
		private float _m_target_damping;
		[STNodeProperty("target_damping", "target_damping")]
		public float m_target_damping
		{
			get { return _m_target_damping; }
			set { _m_target_damping = value; this.Invalidate(); }
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
			
			this.Title = "TorchDynamicMovement";
			
			this.InputOptions.Add("torch", typeof(STNode), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
		}
	}
}
