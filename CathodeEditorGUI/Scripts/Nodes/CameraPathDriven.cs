#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CameraPathDriven : STNode
	{
		private string _m_path_driven_type;
		[STNodeProperty("path_driven_type", "path_driven_type")]
		public string m_path_driven_type
		{
			get { return _m_path_driven_type; }
			set { _m_path_driven_type = value; this.Invalidate(); }
		}
		
		private bool _m_invert_progression;
		[STNodeProperty("invert_progression", "invert_progression")]
		public bool m_invert_progression
		{
			get { return _m_invert_progression; }
			set { _m_invert_progression = value; this.Invalidate(); }
		}
		
		private float _m_position_path_offset;
		[STNodeProperty("position_path_offset", "position_path_offset")]
		public float m_position_path_offset
		{
			get { return _m_position_path_offset; }
			set { _m_position_path_offset = value; this.Invalidate(); }
		}
		
		private float _m_target_path_offset;
		[STNodeProperty("target_path_offset", "target_path_offset")]
		public float m_target_path_offset
		{
			get { return _m_target_path_offset; }
			set { _m_target_path_offset = value; this.Invalidate(); }
		}
		
		private float _m_animation_duration;
		[STNodeProperty("animation_duration", "animation_duration")]
		public float m_animation_duration
		{
			get { return _m_animation_duration; }
			set { _m_animation_duration = value; this.Invalidate(); }
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
			
			this.Title = "CameraPathDriven";
			
			this.InputOptions.Add("position_path", typeof(string), false);
			this.InputOptions.Add("target_path", typeof(string), false);
			this.InputOptions.Add("reference_path", typeof(string), false);
			this.InputOptions.Add("position_path_transform", typeof(cTransform), false);
			this.InputOptions.Add("target_path_transform", typeof(cTransform), false);
			this.InputOptions.Add("reference_path_transform", typeof(cTransform), false);
			this.InputOptions.Add("point_to_project", typeof(cVector3), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("linked_cameras", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("activate_behavior", typeof(void), false);
			this.InputOptions.Add("deactivate_behavior", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
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
