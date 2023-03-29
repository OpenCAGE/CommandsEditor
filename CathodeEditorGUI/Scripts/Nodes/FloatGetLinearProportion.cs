#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FloatGetLinearProportion : STNode
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
			
			this.Title = "FloatGetLinearProportion";
			
			this.InputOptions.Add("Min", typeof(float), false);
			this.InputOptions.Add("Input", typeof(float), false);
			this.InputOptions.Add("Max", typeof(float), false);
			this.InputOptions.Add("Evaluate", typeof(void), false);
			
			this.OutputOptions.Add("Proportion", typeof(float), false);
			this.OutputOptions.Add("Evaluated", typeof(void), false);
		}
	}
}
#endif
