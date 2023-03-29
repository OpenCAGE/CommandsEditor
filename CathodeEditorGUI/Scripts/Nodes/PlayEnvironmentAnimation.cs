#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayEnvironmentAnimation : STNode
	{
		private bool _m_play_on_reset;
		[STNodeProperty("play_on_reset", "play_on_reset")]
		public bool m_play_on_reset
		{
			get { return _m_play_on_reset; }
			set { _m_play_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_jump_to_the_end_on_play;
		[STNodeProperty("jump_to_the_end_on_play", "jump_to_the_end_on_play")]
		public bool m_jump_to_the_end_on_play
		{
			get { return _m_jump_to_the_end_on_play; }
			set { _m_jump_to_the_end_on_play = value; this.Invalidate(); }
		}
		
		private string _m_animation_info;
		[STNodeProperty("animation_info", "animation_info")]
		public string m_animation_info
		{
			get { return _m_animation_info; }
			set { _m_animation_info = value; this.Invalidate(); }
		}
		
		private string _m_AnimationSet;
		[STNodeProperty("AnimationSet", "AnimationSet")]
		public string m_AnimationSet
		{
			get { return _m_AnimationSet; }
			set { _m_AnimationSet = value; this.Invalidate(); }
		}
		
		private string _m_Animation;
		[STNodeProperty("Animation", "Animation")]
		public string m_Animation
		{
			get { return _m_Animation; }
			set { _m_Animation = value; this.Invalidate(); }
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
		
		private bool _m_loop;
		[STNodeProperty("loop", "loop")]
		public bool m_loop
		{
			get { return _m_loop; }
			set { _m_loop = value; this.Invalidate(); }
		}
		
		private bool _m_is_cinematic;
		[STNodeProperty("is_cinematic", "is_cinematic")]
		public bool m_is_cinematic
		{
			get { return _m_is_cinematic; }
			set { _m_is_cinematic = value; this.Invalidate(); }
		}
		
		private int _m_shot_number;
		[STNodeProperty("shot_number", "shot_number")]
		public int m_shot_number
		{
			get { return _m_shot_number; }
			set { _m_shot_number = value; this.Invalidate(); }
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
			
			this.Title = "PlayEnvironmentAnimation";
			
			this.InputOptions.Add("geometry", typeof(STNode), false);
			this.InputOptions.Add("marker", typeof(STNode), false);
			this.InputOptions.Add("external_start_time", typeof(STNode), false);
			this.InputOptions.Add("external_time", typeof(STNode), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("on_finished", typeof(void), false);
			this.OutputOptions.Add("on_finished_streaming", typeof(void), false);
			this.OutputOptions.Add("animation_length", typeof(float), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
#endif
