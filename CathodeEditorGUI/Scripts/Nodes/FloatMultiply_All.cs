using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FloatMultiply_All : STNode
	{
		private bool _m_Invert;
		[STNodeProperty("Invert", "Invert")]
		public bool m_Invert
		{
			get { return _m_Invert; }
			set { _m_Invert = value; this.Invalidate(); }
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
			
			this.Title = "FloatMultiply_All";
			
			this.InputOptions.Add("Numbers", typeof(float), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Result", typeof(float), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
