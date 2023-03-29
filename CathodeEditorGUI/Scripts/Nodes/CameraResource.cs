#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CameraResource : STNode
	{
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_camera_name;
		[STNodeProperty("camera_name", "camera_name")]
		public string m_camera_name
		{
			get { return _m_camera_name; }
			set { _m_camera_name = value; this.Invalidate(); }
		}
		
		private bool _m_is_camera_transformation_local;
		[STNodeProperty("is_camera_transformation_local", "is_camera_transformation_local")]
		public bool m_is_camera_transformation_local
		{
			get { return _m_is_camera_transformation_local; }
			set { _m_is_camera_transformation_local = value; this.Invalidate(); }
		}
		
		private cTransform _m_camera_transformation;
		[STNodeProperty("camera_transformation", "camera_transformation")]
		public cTransform m_camera_transformation
		{
			get { return _m_camera_transformation; }
			set { _m_camera_transformation = value; this.Invalidate(); }
		}
		
		private float _m_fov;
		[STNodeProperty("fov", "fov")]
		public float m_fov
		{
			get { return _m_fov; }
			set { _m_fov = value; this.Invalidate(); }
		}
		
		private string _m_clipping_planes_preset;
		[STNodeProperty("clipping_planes_preset", "clipping_planes_preset")]
		public string m_clipping_planes_preset
		{
			get { return _m_clipping_planes_preset; }
			set { _m_clipping_planes_preset = value; this.Invalidate(); }
		}
		
		private bool _m_is_ghost;
		[STNodeProperty("is_ghost", "is_ghost")]
		public bool m_is_ghost
		{
			get { return _m_is_ghost; }
			set { _m_is_ghost = value; this.Invalidate(); }
		}
		
		private bool _m_converge_to_player_camera;
		[STNodeProperty("converge_to_player_camera", "converge_to_player_camera")]
		public bool m_converge_to_player_camera
		{
			get { return _m_converge_to_player_camera; }
			set { _m_converge_to_player_camera = value; this.Invalidate(); }
		}
		
		private bool _m_reset_player_camera_on_exit;
		[STNodeProperty("reset_player_camera_on_exit", "reset_player_camera_on_exit")]
		public bool m_reset_player_camera_on_exit
		{
			get { return _m_reset_player_camera_on_exit; }
			set { _m_reset_player_camera_on_exit = value; this.Invalidate(); }
		}
		
		private bool _m_enable_enter_transition;
		[STNodeProperty("enable_enter_transition", "enable_enter_transition")]
		public bool m_enable_enter_transition
		{
			get { return _m_enable_enter_transition; }
			set { _m_enable_enter_transition = value; this.Invalidate(); }
		}
		
		private string _m_transition_curve_direction;
		[STNodeProperty("transition_curve_direction", "transition_curve_direction")]
		public string m_transition_curve_direction
		{
			get { return _m_transition_curve_direction; }
			set { _m_transition_curve_direction = value; this.Invalidate(); }
		}
		
		private float _m_transition_curve_strength;
		[STNodeProperty("transition_curve_strength", "transition_curve_strength")]
		public float m_transition_curve_strength
		{
			get { return _m_transition_curve_strength; }
			set { _m_transition_curve_strength = value; this.Invalidate(); }
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
		
		private bool _m_enable_exit_transition;
		[STNodeProperty("enable_exit_transition", "enable_exit_transition")]
		public bool m_enable_exit_transition
		{
			get { return _m_enable_exit_transition; }
			set { _m_enable_exit_transition = value; this.Invalidate(); }
		}
		
		private string _m_exit_transition_curve_direction;
		[STNodeProperty("exit_transition_curve_direction", "exit_transition_curve_direction")]
		public string m_exit_transition_curve_direction
		{
			get { return _m_exit_transition_curve_direction; }
			set { _m_exit_transition_curve_direction = value; this.Invalidate(); }
		}
		
		private float _m_exit_transition_curve_strength;
		[STNodeProperty("exit_transition_curve_strength", "exit_transition_curve_strength")]
		public float m_exit_transition_curve_strength
		{
			get { return _m_exit_transition_curve_strength; }
			set { _m_exit_transition_curve_strength = value; this.Invalidate(); }
		}
		
		private float _m_exit_transition_duration;
		[STNodeProperty("exit_transition_duration", "exit_transition_duration")]
		public float m_exit_transition_duration
		{
			get { return _m_exit_transition_duration; }
			set { _m_exit_transition_duration = value; this.Invalidate(); }
		}
		
		private float _m_exit_transition_ease_in;
		[STNodeProperty("exit_transition_ease_in", "exit_transition_ease_in")]
		public float m_exit_transition_ease_in
		{
			get { return _m_exit_transition_ease_in; }
			set { _m_exit_transition_ease_in = value; this.Invalidate(); }
		}
		
		private float _m_exit_transition_ease_out;
		[STNodeProperty("exit_transition_ease_out", "exit_transition_ease_out")]
		public float m_exit_transition_ease_out
		{
			get { return _m_exit_transition_ease_out; }
			set { _m_exit_transition_ease_out = value; this.Invalidate(); }
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
			
			this.Title = "CameraResource";
			
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("activate_camera", typeof(void), false);
			this.InputOptions.Add("deactivate_camera", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("on_enter_transition_finished", typeof(void), false);
			this.OutputOptions.Add("on_exit_transition_finished", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("camera_activated", typeof(void), false);
			this.OutputOptions.Add("camera_deactivated", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
