#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class LogicPressurePad : STNode
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
			
			this.Title = "LogicPressurePad";
			
			this.InputOptions.Add("Limit", typeof(int), false);
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("enter", typeof(void), false);
			this.InputOptions.Add("exit", typeof(void), false);
			this.InputOptions.Add("bind_all", typeof(void), false);
			this.InputOptions.Add("verify", typeof(void), false);
			
			this.OutputOptions.Add("Pad_Activated", typeof(void), false);
			this.OutputOptions.Add("Pad_Deactivated", typeof(void), false);
			this.OutputOptions.Add("bound_characters", typeof(void), false);
			this.OutputOptions.Add("Count", typeof(int), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("entered", typeof(void), false);
			this.OutputOptions.Add("exited", typeof(void), false);
		}
	}
}
#endif
