using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FloatLessThanOrEqual : STNode
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
			
			this.Title = "FloatLessThanOrEqual";
			
			this.InputOptions.Add("LHS", typeof(float), false);
			this.InputOptions.Add("RHS", typeof(float), false);
			this.InputOptions.Add("Threshold", typeof(float), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_true", typeof(void), false);
			this.OutputOptions.Add("on_false", typeof(void), false);
			this.OutputOptions.Add("Result", typeof(bool), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
