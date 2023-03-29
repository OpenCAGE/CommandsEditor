#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Interaction : STNode
	{
		private bool _m_interruptible_on_start;
		[STNodeProperty("interruptible_on_start", "interruptible_on_start")]
		public bool m_interruptible_on_start
		{
			get { return _m_interruptible_on_start; }
			set { _m_interruptible_on_start = value; this.Invalidate(); }
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
			
			this.Title = "Interaction";
			
			this.InputOptions.Add("start_interaction", typeof(void), false);
			this.InputOptions.Add("stop_interaction", typeof(void), false);
			this.InputOptions.Add("allow_interrupt", typeof(void), false);
			this.InputOptions.Add("disallow_interrupt", typeof(void), false);
			
			this.OutputOptions.Add("on_damaged", typeof(void), false);
			this.OutputOptions.Add("on_interrupt", typeof(void), false);
			this.OutputOptions.Add("on_killed", typeof(void), false);
			this.OutputOptions.Add("interaction_started", typeof(void), false);
			this.OutputOptions.Add("interaction_stopped", typeof(void), false);
			this.OutputOptions.Add("interrupt_allowed", typeof(void), false);
			this.OutputOptions.Add("interrupt_disallowed", typeof(void), false);
		}
	}
}
#endif
