#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundPlaybackBaseClass : STNode
	{
		private string _m_sound_event;
		[STNodeProperty("sound_event", "sound_event")]
		public string m_sound_event
		{
			get { return _m_sound_event; }
			set { _m_sound_event = value; this.Invalidate(); }
		}
		
		private bool _m_is_occludable;
		[STNodeProperty("is_occludable", "is_occludable")]
		public bool m_is_occludable
		{
			get { return _m_is_occludable; }
			set { _m_is_occludable = value; this.Invalidate(); }
		}
		
		private string _m_argument_1;
		[STNodeProperty("argument_1", "argument_1")]
		public string m_argument_1
		{
			get { return _m_argument_1; }
			set { _m_argument_1 = value; this.Invalidate(); }
		}
		
		private string _m_argument_2;
		[STNodeProperty("argument_2", "argument_2")]
		public string m_argument_2
		{
			get { return _m_argument_2; }
			set { _m_argument_2 = value; this.Invalidate(); }
		}
		
		private string _m_argument_3;
		[STNodeProperty("argument_3", "argument_3")]
		public string m_argument_3
		{
			get { return _m_argument_3; }
			set { _m_argument_3 = value; this.Invalidate(); }
		}
		
		private string _m_argument_4;
		[STNodeProperty("argument_4", "argument_4")]
		public string m_argument_4
		{
			get { return _m_argument_4; }
			set { _m_argument_4 = value; this.Invalidate(); }
		}
		
		private string _m_argument_5;
		[STNodeProperty("argument_5", "argument_5")]
		public string m_argument_5
		{
			get { return _m_argument_5; }
			set { _m_argument_5 = value; this.Invalidate(); }
		}
		
		private string _m_namespace;
		[STNodeProperty("namespace", "namespace")]
		public string m_namespace
		{
			get { return _m_namespace; }
			set { _m_namespace = value; this.Invalidate(); }
		}
		
		private cTransform _m_object_position;
		[STNodeProperty("object_position", "object_position")]
		public cTransform m_object_position
		{
			get { return _m_object_position; }
			set { _m_object_position = value; this.Invalidate(); }
		}
		
		private bool _m_restore_on_checkpoint;
		[STNodeProperty("restore_on_checkpoint", "restore_on_checkpoint")]
		public bool m_restore_on_checkpoint
		{
			get { return _m_restore_on_checkpoint; }
			set { _m_restore_on_checkpoint = value; this.Invalidate(); }
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
			
			this.Title = "SoundPlaybackBaseClass";
			
			this.InputOptions.Add("attached_sound_object", typeof(STNode), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("on_finished", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
