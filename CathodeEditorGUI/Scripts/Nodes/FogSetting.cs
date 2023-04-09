using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FogSetting : STNode
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
			
			this.Title = "FogSetting";
			
			this.InputOptions.Add("linear_distance", typeof(float), false);
			this.InputOptions.Add("max_distance", typeof(float), false);
			this.InputOptions.Add("linear_density", typeof(float), false);
			this.InputOptions.Add("exponential_density", typeof(float), false);
			this.InputOptions.Add("near_colour", typeof(cVector3), false);
			this.InputOptions.Add("far_colour", typeof(cVector3), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
