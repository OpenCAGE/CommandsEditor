using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerSelect_Direct : STNode
	{
		private bool _m_Changes_only;
		[STNodeProperty("Changes_only", "Changes_only")]
		public bool m_Changes_only
		{
			get { return _m_Changes_only; }
			set { _m_Changes_only = value; this.Invalidate(); }
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
			
			this.Title = "TriggerSelect_Direct";
			
			this.InputOptions.Add("Object_0", typeof(STNode), false);
			this.InputOptions.Add("Object_1", typeof(STNode), false);
			this.InputOptions.Add("Object_2", typeof(STNode), false);
			this.InputOptions.Add("Object_3", typeof(STNode), false);
			this.InputOptions.Add("Object_4", typeof(STNode), false);
			this.InputOptions.Add("Object_5", typeof(STNode), false);
			this.InputOptions.Add("Object_6", typeof(STNode), false);
			this.InputOptions.Add("Object_7", typeof(STNode), false);
			this.InputOptions.Add("Object_8", typeof(STNode), false);
			this.InputOptions.Add("Object_9", typeof(STNode), false);
			this.InputOptions.Add("Object_10", typeof(STNode), false);
			this.InputOptions.Add("Object_11", typeof(STNode), false);
			this.InputOptions.Add("Object_12", typeof(STNode), false);
			this.InputOptions.Add("Object_13", typeof(STNode), false);
			this.InputOptions.Add("Object_14", typeof(STNode), false);
			this.InputOptions.Add("Object_15", typeof(STNode), false);
			this.InputOptions.Add("Object_16", typeof(STNode), false);
			this.InputOptions.Add("Trigger_0", typeof(void), false);
			this.InputOptions.Add("Trigger_1", typeof(void), false);
			this.InputOptions.Add("Trigger_2", typeof(void), false);
			this.InputOptions.Add("Trigger_3", typeof(void), false);
			this.InputOptions.Add("Trigger_4", typeof(void), false);
			this.InputOptions.Add("Trigger_5", typeof(void), false);
			this.InputOptions.Add("Trigger_6", typeof(void), false);
			this.InputOptions.Add("Trigger_7", typeof(void), false);
			this.InputOptions.Add("Trigger_8", typeof(void), false);
			this.InputOptions.Add("Trigger_9", typeof(void), false);
			this.InputOptions.Add("Trigger_10", typeof(void), false);
			this.InputOptions.Add("Trigger_11", typeof(void), false);
			this.InputOptions.Add("Trigger_12", typeof(void), false);
			this.InputOptions.Add("Trigger_13", typeof(void), false);
			this.InputOptions.Add("Trigger_14", typeof(void), false);
			this.InputOptions.Add("Trigger_15", typeof(void), false);
			this.InputOptions.Add("Trigger_16", typeof(void), false);
			
			this.OutputOptions.Add("Changed_to_0", typeof(void), false);
			this.OutputOptions.Add("Changed_to_1", typeof(void), false);
			this.OutputOptions.Add("Changed_to_2", typeof(void), false);
			this.OutputOptions.Add("Changed_to_3", typeof(void), false);
			this.OutputOptions.Add("Changed_to_4", typeof(void), false);
			this.OutputOptions.Add("Changed_to_5", typeof(void), false);
			this.OutputOptions.Add("Changed_to_6", typeof(void), false);
			this.OutputOptions.Add("Changed_to_7", typeof(void), false);
			this.OutputOptions.Add("Changed_to_8", typeof(void), false);
			this.OutputOptions.Add("Changed_to_9", typeof(void), false);
			this.OutputOptions.Add("Changed_to_10", typeof(void), false);
			this.OutputOptions.Add("Changed_to_11", typeof(void), false);
			this.OutputOptions.Add("Changed_to_12", typeof(void), false);
			this.OutputOptions.Add("Changed_to_13", typeof(void), false);
			this.OutputOptions.Add("Changed_to_14", typeof(void), false);
			this.OutputOptions.Add("Changed_to_15", typeof(void), false);
			this.OutputOptions.Add("Changed_to_16", typeof(void), false);
			this.OutputOptions.Add("Result", typeof(STNode), false);
			this.OutputOptions.Add("TriggeredIndex", typeof(int), false);
			this.OutputOptions.Add("Pin_0", typeof(void), false);
			this.OutputOptions.Add("Pin_1", typeof(void), false);
			this.OutputOptions.Add("Pin_2", typeof(void), false);
			this.OutputOptions.Add("Pin_3", typeof(void), false);
			this.OutputOptions.Add("Pin_4", typeof(void), false);
			this.OutputOptions.Add("Pin_5", typeof(void), false);
			this.OutputOptions.Add("Pin_6", typeof(void), false);
			this.OutputOptions.Add("Pin_7", typeof(void), false);
			this.OutputOptions.Add("Pin_8", typeof(void), false);
			this.OutputOptions.Add("Pin_9", typeof(void), false);
			this.OutputOptions.Add("Pin_10", typeof(void), false);
			this.OutputOptions.Add("Pin_11", typeof(void), false);
			this.OutputOptions.Add("Pin_12", typeof(void), false);
			this.OutputOptions.Add("Pin_13", typeof(void), false);
			this.OutputOptions.Add("Pin_14", typeof(void), false);
			this.OutputOptions.Add("Pin_15", typeof(void), false);
			this.OutputOptions.Add("Pin_16", typeof(void), false);
		}
	}
}
