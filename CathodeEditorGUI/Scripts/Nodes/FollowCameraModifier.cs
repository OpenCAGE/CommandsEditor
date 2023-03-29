#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FollowCameraModifier : STNode
	{
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_modifier_type;
		[STNodeProperty("modifier_type", "modifier_type")]
		public string m_modifier_type
		{
			get { return _m_modifier_type; }
			set { _m_modifier_type = value; this.Invalidate(); }
		}
		
		private cVector3 _m_position_offset;
		[STNodeProperty("position_offset", "position_offset")]
		public cVector3 m_position_offset
		{
			get { return _m_position_offset; }
			set { _m_position_offset = value; this.Invalidate(); }
		}
		
		private cVector3 _m_target_offset;
		[STNodeProperty("target_offset", "target_offset")]
		public cVector3 m_target_offset
		{
			get { return _m_target_offset; }
			set { _m_target_offset = value; this.Invalidate(); }
		}
		
		private float _m_field_of_view;
		[STNodeProperty("field_of_view", "field_of_view")]
		public float m_field_of_view
		{
			get { return _m_field_of_view; }
			set { _m_field_of_view = value; this.Invalidate(); }
		}
		
		private bool _m_force_state;
		[STNodeProperty("force_state", "force_state")]
		public bool m_force_state
		{
			get { return _m_force_state; }
			set { _m_force_state = value; this.Invalidate(); }
		}
		
		private float _m_force_state_initial_value;
		[STNodeProperty("force_state_initial_value", "force_state_initial_value")]
		public float m_force_state_initial_value
		{
			get { return _m_force_state_initial_value; }
			set { _m_force_state_initial_value = value; this.Invalidate(); }
		}
		
		private bool _m_can_mirror;
		[STNodeProperty("can_mirror", "can_mirror")]
		public bool m_can_mirror
		{
			get { return _m_can_mirror; }
			set { _m_can_mirror = value; this.Invalidate(); }
		}
		
		private bool _m_is_first_person;
		[STNodeProperty("is_first_person", "is_first_person")]
		public bool m_is_first_person
		{
			get { return _m_is_first_person; }
			set { _m_is_first_person = value; this.Invalidate(); }
		}
		
		private float _m_bone_blending_ratio;
		[STNodeProperty("bone_blending_ratio", "bone_blending_ratio")]
		public float m_bone_blending_ratio
		{
			get { return _m_bone_blending_ratio; }
			set { _m_bone_blending_ratio = value; this.Invalidate(); }
		}
		
		private float _m_movement_speed;
		[STNodeProperty("movement_speed", "movement_speed")]
		public float m_movement_speed
		{
			get { return _m_movement_speed; }
			set { _m_movement_speed = value; this.Invalidate(); }
		}
		
		private float _m_movement_speed_vertical;
		[STNodeProperty("movement_speed_vertical", "movement_speed_vertical")]
		public float m_movement_speed_vertical
		{
			get { return _m_movement_speed_vertical; }
			set { _m_movement_speed_vertical = value; this.Invalidate(); }
		}
		
		private float _m_movement_damping;
		[STNodeProperty("movement_damping", "movement_damping")]
		public float m_movement_damping
		{
			get { return _m_movement_damping; }
			set { _m_movement_damping = value; this.Invalidate(); }
		}
		
		private float _m_horizontal_limit_min;
		[STNodeProperty("horizontal_limit_min", "horizontal_limit_min")]
		public float m_horizontal_limit_min
		{
			get { return _m_horizontal_limit_min; }
			set { _m_horizontal_limit_min = value; this.Invalidate(); }
		}
		
		private float _m_horizontal_limit_max;
		[STNodeProperty("horizontal_limit_max", "horizontal_limit_max")]
		public float m_horizontal_limit_max
		{
			get { return _m_horizontal_limit_max; }
			set { _m_horizontal_limit_max = value; this.Invalidate(); }
		}
		
		private float _m_vertical_limit_min;
		[STNodeProperty("vertical_limit_min", "vertical_limit_min")]
		public float m_vertical_limit_min
		{
			get { return _m_vertical_limit_min; }
			set { _m_vertical_limit_min = value; this.Invalidate(); }
		}
		
		private float _m_vertical_limit_max;
		[STNodeProperty("vertical_limit_max", "vertical_limit_max")]
		public float m_vertical_limit_max
		{
			get { return _m_vertical_limit_max; }
			set { _m_vertical_limit_max = value; this.Invalidate(); }
		}
		
		private float _m_mouse_speed_hori;
		[STNodeProperty("mouse_speed_hori", "mouse_speed_hori")]
		public float m_mouse_speed_hori
		{
			get { return _m_mouse_speed_hori; }
			set { _m_mouse_speed_hori = value; this.Invalidate(); }
		}
		
		private float _m_mouse_speed_vert;
		[STNodeProperty("mouse_speed_vert", "mouse_speed_vert")]
		public float m_mouse_speed_vert
		{
			get { return _m_mouse_speed_vert; }
			set { _m_mouse_speed_vert = value; this.Invalidate(); }
		}
		
		private float _m_acceleration_duration;
		[STNodeProperty("acceleration_duration", "acceleration_duration")]
		public float m_acceleration_duration
		{
			get { return _m_acceleration_duration; }
			set { _m_acceleration_duration = value; this.Invalidate(); }
		}
		
		private float _m_acceleration_ease_in;
		[STNodeProperty("acceleration_ease_in", "acceleration_ease_in")]
		public float m_acceleration_ease_in
		{
			get { return _m_acceleration_ease_in; }
			set { _m_acceleration_ease_in = value; this.Invalidate(); }
		}
		
		private float _m_acceleration_ease_out;
		[STNodeProperty("acceleration_ease_out", "acceleration_ease_out")]
		public float m_acceleration_ease_out
		{
			get { return _m_acceleration_ease_out; }
			set { _m_acceleration_ease_out = value; this.Invalidate(); }
		}
		
		private float _m_transition_duration;
		[STNodeProperty("transition_duration", "transition_duration")]
		public float m_transition_duration
		{
			get { return _m_transition_duration; }
			set { _m_transition_duration = value; this.Invalidate(); }
		}
		
		private float _m_transition_ease_in;
		[STNodeProperty("transition_ease_in", "transition_ease_in")]
		public float m_transition_ease_in
		{
			get { return _m_transition_ease_in; }
			set { _m_transition_ease_in = value; this.Invalidate(); }
		}
		
		private float _m_transition_ease_out;
		[STNodeProperty("transition_ease_out", "transition_ease_out")]
		public float m_transition_ease_out
		{
			get { return _m_transition_ease_out; }
			set { _m_transition_ease_out = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "FollowCameraModifier";
			
			this.InputOptions.Add("position_curve", typeof(STNode), false);
			this.InputOptions.Add("target_curve", typeof(STNode), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
		}
	}
}
#endif
