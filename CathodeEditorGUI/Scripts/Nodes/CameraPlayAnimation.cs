#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CameraPlayAnimation : STNode
	{
		private string _m_data_file;
		[STNodeProperty("data_file", "data_file")]
		public string m_data_file
		{
			get { return _m_data_file; }
			set { _m_data_file = value; this.Invalidate(); }
		}
		
		private int _m_start_frame;
		[STNodeProperty("start_frame", "start_frame")]
		public int m_start_frame
		{
			get { return _m_start_frame; }
			set { _m_start_frame = value; this.Invalidate(); }
		}
		
		private int _m_end_frame;
		[STNodeProperty("end_frame", "end_frame")]
		public int m_end_frame
		{
			get { return _m_end_frame; }
			set { _m_end_frame = value; this.Invalidate(); }
		}
		
		private float _m_play_speed;
		[STNodeProperty("play_speed", "play_speed")]
		public float m_play_speed
		{
			get { return _m_play_speed; }
			set { _m_play_speed = value; this.Invalidate(); }
		}
		
		private bool _m_loop_play;
		[STNodeProperty("loop_play", "loop_play")]
		public bool m_loop_play
		{
			get { return _m_loop_play; }
			set { _m_loop_play = value; this.Invalidate(); }
		}
		
		private string _m_clipping_planes_preset;
		[STNodeProperty("clipping_planes_preset", "clipping_planes_preset")]
		public string m_clipping_planes_preset
		{
			get { return _m_clipping_planes_preset; }
			set { _m_clipping_planes_preset = value; this.Invalidate(); }
		}
		
		private bool _m_is_cinematic;
		[STNodeProperty("is_cinematic", "is_cinematic")]
		public bool m_is_cinematic
		{
			get { return _m_is_cinematic; }
			set { _m_is_cinematic = value; this.Invalidate(); }
		}
		
		private int _m_dof_key;
		[STNodeProperty("dof_key", "dof_key")]
		public int m_dof_key
		{
			get { return _m_dof_key; }
			set { _m_dof_key = value; this.Invalidate(); }
		}
		
		private int _m_shot_number;
		[STNodeProperty("shot_number", "shot_number")]
		public int m_shot_number
		{
			get { return _m_shot_number; }
			set { _m_shot_number = value; this.Invalidate(); }
		}
		
		private bool _m_override_dof;
		[STNodeProperty("override_dof", "override_dof")]
		public bool m_override_dof
		{
			get { return _m_override_dof; }
			set { _m_override_dof = value; this.Invalidate(); }
		}
		
		private cVector3 _m_focal_point_offset;
		[STNodeProperty("focal_point_offset", "focal_point_offset")]
		public cVector3 m_focal_point_offset
		{
			get { return _m_focal_point_offset; }
			set { _m_focal_point_offset = value; this.Invalidate(); }
		}
		
		private string _m_bone_to_focus;
		[STNodeProperty("bone_to_focus", "bone_to_focus")]
		public string m_bone_to_focus
		{
			get { return _m_bone_to_focus; }
			set { _m_bone_to_focus = value; this.Invalidate(); }
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
			
			this.Title = "CameraPlayAnimation";
			
			this.InputOptions.Add("animated_camera", typeof(STNode), false);
			this.InputOptions.Add("position_marker", typeof(cTransform), false);
			this.InputOptions.Add("character_to_focus", typeof(STNode), false);
			this.InputOptions.Add("focal_length_mm", typeof(float), false);
			this.InputOptions.Add("focal_plane_m", typeof(float), false);
			this.InputOptions.Add("fnum", typeof(float), false);
			this.InputOptions.Add("focal_point", typeof(cTransform), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("on_animation_finished", typeof(void), false);
			this.OutputOptions.Add("animation_length", typeof(float), false);
			this.OutputOptions.Add("frames_count", typeof(int), false);
			this.OutputOptions.Add("result_transformation", typeof(cTransform), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
