using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_TriggerShootRequest : STNode
	{
		private bool _m_empty_current_clip;
		[STNodeProperty("empty_current_clip", "empty_current_clip")]
		public bool m_empty_current_clip
		{
			get { return _m_empty_current_clip; }
			set { _m_empty_current_clip = value; this.Invalidate(); }
		}
		
		private int _m_shot_count;
		[STNodeProperty("shot_count", "shot_count")]
		public int m_shot_count
		{
			get { return _m_shot_count; }
			set { _m_shot_count = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private bool _m_clear_current_requests;
		[STNodeProperty("clear_current_requests", "clear_current_requests")]
		public bool m_clear_current_requests
		{
			get { return _m_clear_current_requests; }
			set { _m_clear_current_requests = value; this.Invalidate(); }
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
			
			this.Title = "NPC_TriggerShootRequest";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("started_shooting", typeof(void), false);
			this.OutputOptions.Add("finished_shooting", typeof(void), false);
			this.OutputOptions.Add("interrupted", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
