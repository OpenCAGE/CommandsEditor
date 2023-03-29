#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundTimelineTrigger : STNode
	{
		private string _m_sound_event;
		[STNodeProperty("sound_event", "sound_event")]
		public string m_sound_event
		{
			get { return _m_sound_event; }
			set { _m_sound_event = value; this.Invalidate(); }
		}
		
		private float _m_trigger_time;
		[STNodeProperty("trigger_time", "trigger_time")]
		public float m_trigger_time
		{
			get { return _m_trigger_time; }
			set { _m_trigger_time = value; this.Invalidate(); }
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
			
			this.Title = "SoundTimelineTrigger";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("trigger_now", typeof(void), false);
			this.InputOptions.Add("abort", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("aborted", typeof(void), false);
		}
	}
}
#endif
