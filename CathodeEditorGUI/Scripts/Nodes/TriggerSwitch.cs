#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerSwitch : STNode
	{
		private int _m_num;
		[STNodeProperty("num", "num")]
		public int m_num
		{
			get { return _m_num; }
			set { _m_num = value; this.Invalidate(); }
		}
		
		private bool _m_loop;
		[STNodeProperty("loop", "loop")]
		public bool m_loop
		{
			get { return _m_loop; }
			set { _m_loop = value; this.Invalidate(); }
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
			
			this.Title = "TriggerSwitch";
			
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("Up", typeof(void), false);
			this.InputOptions.Add("Down", typeof(void), false);
			this.InputOptions.Add("Random", typeof(void), false);
			
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
			this.OutputOptions.Add("current", typeof(int), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("on_Up", typeof(void), false);
			this.OutputOptions.Add("on_Down", typeof(void), false);
			this.OutputOptions.Add("on_Random", typeof(void), false);
		}
	}
}
#endif
