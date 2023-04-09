using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_IsWithinRange : STNode
	{
		private string _m_Range_test_shape;
		[STNodeProperty("Range_test_shape", "Range_test_shape")]
		public string m_Range_test_shape
		{
			get { return _m_Range_test_shape; }
			set { _m_Range_test_shape = value; this.Invalidate(); }
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
			
			this.Title = "CHR_IsWithinRange";
			
			this.InputOptions.Add("Position", typeof(cTransform), false);
			this.InputOptions.Add("Radius", typeof(float), false);
			this.InputOptions.Add("Height", typeof(float), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("In_range", typeof(void), false);
			this.OutputOptions.Add("Out_of_range", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
