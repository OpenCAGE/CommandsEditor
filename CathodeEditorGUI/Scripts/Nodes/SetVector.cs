using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetVector : STNode
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
			
			this.Title = "SetVector";
			
			this.InputOptions.Add("x", typeof(float), false);
			this.InputOptions.Add("y", typeof(float), false);
			this.InputOptions.Add("z", typeof(float), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Result", typeof(cVector3), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
