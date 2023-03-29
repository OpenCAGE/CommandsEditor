#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CameraShake : STNode
	{
		private string _m_shake_type;
		[STNodeProperty("shake_type", "shake_type")]
		public string m_shake_type
		{
			get { return _m_shake_type; }
			set { _m_shake_type = value; this.Invalidate(); }
		}
		
		private cVector3 _m_shake_frequency;
		[STNodeProperty("shake_frequency", "shake_frequency")]
		public cVector3 m_shake_frequency
		{
			get { return _m_shake_frequency; }
			set { _m_shake_frequency = value; this.Invalidate(); }
		}
		
		private cVector3 _m_max_rotation_angles;
		[STNodeProperty("max_rotation_angles", "max_rotation_angles")]
		public cVector3 m_max_rotation_angles
		{
			get { return _m_max_rotation_angles; }
			set { _m_max_rotation_angles = value; this.Invalidate(); }
		}
		
		private cVector3 _m_max_position_offset;
		[STNodeProperty("max_position_offset", "max_position_offset")]
		public cVector3 m_max_position_offset
		{
			get { return _m_max_position_offset; }
			set { _m_max_position_offset = value; this.Invalidate(); }
		}
		
		private bool _m_shake_rotation;
		[STNodeProperty("shake_rotation", "shake_rotation")]
		public bool m_shake_rotation
		{
			get { return _m_shake_rotation; }
			set { _m_shake_rotation = value; this.Invalidate(); }
		}
		
		private bool _m_shake_position;
		[STNodeProperty("shake_position", "shake_position")]
		public bool m_shake_position
		{
			get { return _m_shake_position; }
			set { _m_shake_position = value; this.Invalidate(); }
		}
		
		private bool _m_bone_shaking;
		[STNodeProperty("bone_shaking", "bone_shaking")]
		public bool m_bone_shaking
		{
			get { return _m_bone_shaking; }
			set { _m_bone_shaking = value; this.Invalidate(); }
		}
		
		private bool _m_override_weapon_swing;
		[STNodeProperty("override_weapon_swing", "override_weapon_swing")]
		public bool m_override_weapon_swing
		{
			get { return _m_override_weapon_swing; }
			set { _m_override_weapon_swing = value; this.Invalidate(); }
		}
		
		private float _m_internal_radius;
		[STNodeProperty("internal_radius", "internal_radius")]
		public float m_internal_radius
		{
			get { return _m_internal_radius; }
			set { _m_internal_radius = value; this.Invalidate(); }
		}
		
		private float _m_external_radius;
		[STNodeProperty("external_radius", "external_radius")]
		public float m_external_radius
		{
			get { return _m_external_radius; }
			set { _m_external_radius = value; this.Invalidate(); }
		}
		
		private float _m_strength_damping;
		[STNodeProperty("strength_damping", "strength_damping")]
		public float m_strength_damping
		{
			get { return _m_strength_damping; }
			set { _m_strength_damping = value; this.Invalidate(); }
		}
		
		private bool _m_explosion_push_back;
		[STNodeProperty("explosion_push_back", "explosion_push_back")]
		public bool m_explosion_push_back
		{
			get { return _m_explosion_push_back; }
			set { _m_explosion_push_back = value; this.Invalidate(); }
		}
		
		private float _m_spring_constant;
		[STNodeProperty("spring_constant", "spring_constant")]
		public float m_spring_constant
		{
			get { return _m_spring_constant; }
			set { _m_spring_constant = value; this.Invalidate(); }
		}
		
		private float _m_spring_damping;
		[STNodeProperty("spring_damping", "spring_damping")]
		public float m_spring_damping
		{
			get { return _m_spring_damping; }
			set { _m_spring_damping = value; this.Invalidate(); }
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
		
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_behavior_name;
		[STNodeProperty("behavior_name", "behavior_name")]
		public string m_behavior_name
		{
			get { return _m_behavior_name; }
			set { _m_behavior_name = value; this.Invalidate(); }
		}
		
		private int _m_priority;
		[STNodeProperty("priority", "priority")]
		public int m_priority
		{
			get { return _m_priority; }
			set { _m_priority = value; this.Invalidate(); }
		}
		
		private float _m_threshold;
		[STNodeProperty("threshold", "threshold")]
		public float m_threshold
		{
			get { return _m_threshold; }
			set { _m_threshold = value; this.Invalidate(); }
		}
		
		private float _m_blend_in;
		[STNodeProperty("blend_in", "blend_in")]
		public float m_blend_in
		{
			get { return _m_blend_in; }
			set { _m_blend_in = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private float _m_blend_out;
		[STNodeProperty("blend_out", "blend_out")]
		public float m_blend_out
		{
			get { return _m_blend_out; }
			set { _m_blend_out = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "CameraShake";
			
			this.InputOptions.Add("relative_transformation", typeof(cTransform), false);
			this.InputOptions.Add("impulse_intensity", typeof(float), false);
			this.InputOptions.Add("impulse_position", typeof(cVector3), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("linked_cameras", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("activate_behavior", typeof(void), false);
			this.InputOptions.Add("deactivate_behavior", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("behavior_activated", typeof(void), false);
			this.OutputOptions.Add("behavior_deactivated", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
