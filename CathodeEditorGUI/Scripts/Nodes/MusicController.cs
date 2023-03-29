#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MusicController : STNode
	{
		private string _m_music_start_event;
		[STNodeProperty("music_start_event", "music_start_event")]
		public string m_music_start_event
		{
			get { return _m_music_start_event; }
			set { _m_music_start_event = value; this.Invalidate(); }
		}
		
		private string _m_music_end_event;
		[STNodeProperty("music_end_event", "music_end_event")]
		public string m_music_end_event
		{
			get { return _m_music_end_event; }
			set { _m_music_end_event = value; this.Invalidate(); }
		}
		
		private string _m_music_restart_event;
		[STNodeProperty("music_restart_event", "music_restart_event")]
		public string m_music_restart_event
		{
			get { return _m_music_restart_event; }
			set { _m_music_restart_event = value; this.Invalidate(); }
		}
		
		private string _m_layer_control_rtpc;
		[STNodeProperty("layer_control_rtpc", "layer_control_rtpc")]
		public string m_layer_control_rtpc
		{
			get { return _m_layer_control_rtpc; }
			set { _m_layer_control_rtpc = value; this.Invalidate(); }
		}
		
		private float _m_smooth_rate;
		[STNodeProperty("smooth_rate", "smooth_rate")]
		public float m_smooth_rate
		{
			get { return _m_smooth_rate; }
			set { _m_smooth_rate = value; this.Invalidate(); }
		}
		
		private float _m_alien_max_distance;
		[STNodeProperty("alien_max_distance", "alien_max_distance")]
		public float m_alien_max_distance
		{
			get { return _m_alien_max_distance; }
			set { _m_alien_max_distance = value; this.Invalidate(); }
		}
		
		private float _m_object_max_distance;
		[STNodeProperty("object_max_distance", "object_max_distance")]
		public float m_object_max_distance
		{
			get { return _m_object_max_distance; }
			set { _m_object_max_distance = value; this.Invalidate(); }
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
			
			this.Title = "MusicController";
			
			this.InputOptions.Add("enable_music", typeof(void), false);
			this.InputOptions.Add("disable_music", typeof(void), false);
			
		}
	}
}
#endif
