using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetLocationAndOrientation : STNode
	{
		private string _m_axis_is;
		[STNodeProperty("axis_is", "axis_is")]
		public string m_axis_is
		{
			get { return _m_axis_is; }
			set { _m_axis_is = value; this.Invalidate(); }
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
			
			this.Title = "SetLocationAndOrientation";
			
			this.InputOptions.Add("location", typeof(cTransform), false);
			this.InputOptions.Add("axis", typeof(cVector3), false);
			this.InputOptions.Add("local_offset", typeof(cVector3), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("result", typeof(cTransform), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
