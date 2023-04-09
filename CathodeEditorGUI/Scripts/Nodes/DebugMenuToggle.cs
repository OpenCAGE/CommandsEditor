using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugMenuToggle : STNode
	{
		private string _m_debug_variable;
		[STNodeProperty("debug_variable", "debug_variable")]
		public string m_debug_variable
		{
			get { return _m_debug_variable; }
			set { _m_debug_variable = value; this.Invalidate(); }
		}
		
		private bool _m_value;
		[STNodeProperty("value", "value")]
		public bool m_value
		{
			get { return _m_value; }
			set { _m_value = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "DebugMenuToggle";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
