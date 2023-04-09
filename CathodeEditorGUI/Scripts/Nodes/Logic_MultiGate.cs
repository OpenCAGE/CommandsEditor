using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Logic_MultiGate : STNode
	{
		private int _m_trigger_pin;
		[STNodeProperty("trigger_pin", "trigger_pin")]
		public int m_trigger_pin
		{
			get { return _m_trigger_pin; }
			set { _m_trigger_pin = value; this.Invalidate(); }
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
			
			this.Title = "Logic_MultiGate";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Underflow", typeof(void), false);
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
			this.OutputOptions.Add("Pin_17", typeof(void), false);
			this.OutputOptions.Add("Pin_18", typeof(void), false);
			this.OutputOptions.Add("Pin_19", typeof(void), false);
			this.OutputOptions.Add("Pin_20", typeof(void), false);
			this.OutputOptions.Add("Overflow", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
