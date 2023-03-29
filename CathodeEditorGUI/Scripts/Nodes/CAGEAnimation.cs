#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CAGEAnimation : STNode
	{
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_use_external_time;
		[STNodeProperty("use_external_time", "use_external_time")]
		public bool m_use_external_time
		{
			get { return _m_use_external_time; }
			set { _m_use_external_time = value; this.Invalidate(); }
		}
		
		private bool _m_rewind_on_stop;
		[STNodeProperty("rewind_on_stop", "rewind_on_stop")]
		public bool m_rewind_on_stop
		{
			get { return _m_rewind_on_stop; }
			set { _m_rewind_on_stop = value; this.Invalidate(); }
		}
		
		private bool _m_jump_to_the_end;
		[STNodeProperty("jump_to_the_end", "jump_to_the_end")]
		public bool m_jump_to_the_end
		{
			get { return _m_jump_to_the_end; }
			set { _m_jump_to_the_end = value; this.Invalidate(); }
		}
		
		private float _m_playspeed;
		[STNodeProperty("playspeed", "playspeed")]
		public float m_playspeed
		{
			get { return _m_playspeed; }
			set { _m_playspeed = value; this.Invalidate(); }
		}
		
		private float _m_anim_length;
		[STNodeProperty("anim_length", "anim_length")]
		public float m_anim_length
		{
			get { return _m_anim_length; }
			set { _m_anim_length = value; this.Invalidate(); }
		}
		
		private bool _m_is_cinematic;
		[STNodeProperty("is_cinematic", "is_cinematic")]
		public bool m_is_cinematic
		{
			get { return _m_is_cinematic; }
			set { _m_is_cinematic = value; this.Invalidate(); }
		}
		
		private bool _m_is_cinematic_skippable;
		[STNodeProperty("is_cinematic_skippable", "is_cinematic_skippable")]
		public bool m_is_cinematic_skippable
		{
			get { return _m_is_cinematic_skippable; }
			set { _m_is_cinematic_skippable = value; this.Invalidate(); }
		}
		
		private float _m_skippable_timer;
		[STNodeProperty("skippable_timer", "skippable_timer")]
		public float m_skippable_timer
		{
			get { return _m_skippable_timer; }
			set { _m_skippable_timer = value; this.Invalidate(); }
		}
		
		private bool _m_capture_video;
		[STNodeProperty("capture_video", "capture_video")]
		public bool m_capture_video
		{
			get { return _m_capture_video; }
			set { _m_capture_video = value; this.Invalidate(); }
		}
		
		private string _m_capture_clip_name;
		[STNodeProperty("capture_clip_name", "capture_clip_name")]
		public string m_capture_clip_name
		{
			get { return _m_capture_clip_name; }
			set { _m_capture_clip_name = value; this.Invalidate(); }
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
			
			this.Title = "CAGEAnimation";
			
			this.InputOptions.Add("external_time", typeof(float), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("rewind", typeof(void), false);
			this.InputOptions.Add("load_cutscene", typeof(void), false);
			this.InputOptions.Add("unload_cutscene", typeof(void), false);
			this.InputOptions.Add("start_cutscene", typeof(void), false);
			this.InputOptions.Add("stop_cutscene", typeof(void), false);
			this.InputOptions.Add("pause_cutscene", typeof(void), false);
			this.InputOptions.Add("resume_cutscene", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("animation_finished", typeof(void), false);
			this.OutputOptions.Add("animation_interrupted", typeof(void), false);
			this.OutputOptions.Add("animation_changed", typeof(void), false);
			this.OutputOptions.Add("cinematic_loaded", typeof(void), false);
			this.OutputOptions.Add("cinematic_unloaded", typeof(void), false);
			this.OutputOptions.Add("current_time", typeof(float), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("rewound", typeof(void), false);
			this.OutputOptions.Add("cutscene_started", typeof(void), false);
			this.OutputOptions.Add("cutscene_stopped", typeof(void), false);
			this.OutputOptions.Add("cutscene_paused", typeof(void), false);
			this.OutputOptions.Add("cutscene_resumed", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
