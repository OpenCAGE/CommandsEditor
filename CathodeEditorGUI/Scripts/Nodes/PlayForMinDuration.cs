#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayForMinDuration : STNode
	{
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
			
			this.Title = "PlayForMinDuration";
			
			this.InputOptions.Add("MinDuration", typeof(float), false);
			this.InputOptions.Add("start_timer", typeof(void), false);
			this.InputOptions.Add("stop_timer", typeof(void), false);
			this.InputOptions.Add("notify_animation_started", typeof(void), false);
			this.InputOptions.Add("notify_animation_finished", typeof(void), false);
			
			this.OutputOptions.Add("timer_expired", typeof(void), false);
			this.OutputOptions.Add("first_animation_started", typeof(void), false);
			this.OutputOptions.Add("next_animation", typeof(void), false);
			this.OutputOptions.Add("all_animations_finished", typeof(void), false);
			this.OutputOptions.Add("timer_started", typeof(void), false);
			this.OutputOptions.Add("timer_stopped", typeof(void), false);
		}
	}
}
#endif
