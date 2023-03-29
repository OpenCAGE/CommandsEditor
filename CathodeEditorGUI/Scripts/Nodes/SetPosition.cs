#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetPosition : STNode
	{
		private bool _m_set_on_reset;
		[STNodeProperty("set_on_reset", "set_on_reset")]
		public bool m_set_on_reset
		{
			get { return _m_set_on_reset; }
			set { _m_set_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "SetPosition";
			
			this.InputOptions.Add("Translation", typeof(cVector3), false);
			this.InputOptions.Add("Rotation", typeof(cVector3), false);
			this.InputOptions.Add("Input", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Result", typeof(cTransform), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
