#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerSelect : STNode
	{
		private int _m_index;
		[STNodeProperty("index", "index")]
		public int m_index
		{
			get { return _m_index; }
			set { _m_index = value; this.Invalidate(); }
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
			
			this.Title = "TriggerSelect";
			
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
			this.InputOptions.Add("trigger", typeof(void), false);
			
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
			this.OutputOptions.Add("Result", typeof(STNode), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
