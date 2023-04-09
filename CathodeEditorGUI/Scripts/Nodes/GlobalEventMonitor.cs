using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GlobalEventMonitor : STNode
	{
		private string _m_EventName;
		[STNodeProperty("EventName", "EventName")]
		public string m_EventName
		{
			get { return _m_EventName; }
			set { _m_EventName = value; this.Invalidate(); }
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
			
			this.Title = "GlobalEventMonitor";
			
			this.InputOptions.Add("start_monitoring", typeof(void), false);
			this.InputOptions.Add("stop_monitoring", typeof(void), false);
			
			this.OutputOptions.Add("Event_1", typeof(void), false);
			this.OutputOptions.Add("Event_2", typeof(void), false);
			this.OutputOptions.Add("Event_3", typeof(void), false);
			this.OutputOptions.Add("Event_4", typeof(void), false);
			this.OutputOptions.Add("Event_5", typeof(void), false);
			this.OutputOptions.Add("Event_6", typeof(void), false);
			this.OutputOptions.Add("Event_7", typeof(void), false);
			this.OutputOptions.Add("Event_8", typeof(void), false);
			this.OutputOptions.Add("Event_9", typeof(void), false);
			this.OutputOptions.Add("Event_10", typeof(void), false);
			this.OutputOptions.Add("Event_11", typeof(void), false);
			this.OutputOptions.Add("Event_12", typeof(void), false);
			this.OutputOptions.Add("Event_13", typeof(void), false);
			this.OutputOptions.Add("Event_14", typeof(void), false);
			this.OutputOptions.Add("Event_15", typeof(void), false);
			this.OutputOptions.Add("Event_16", typeof(void), false);
			this.OutputOptions.Add("Event_17", typeof(void), false);
			this.OutputOptions.Add("Event_18", typeof(void), false);
			this.OutputOptions.Add("Event_19", typeof(void), false);
			this.OutputOptions.Add("Event_20", typeof(void), false);
			this.OutputOptions.Add("started_monitoring", typeof(void), false);
			this.OutputOptions.Add("stopped_monitoring", typeof(void), false);
		}
	}
}
