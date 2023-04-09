using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PhysicsApplyBuoyancy : STNode
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
			
			this.Title = "PhysicsApplyBuoyancy";
			
			this.InputOptions.Add("objects", typeof(STNode), false);
			this.InputOptions.Add("water_height", typeof(float), false);
			this.InputOptions.Add("water_density", typeof(float), false);
			this.InputOptions.Add("water_viscosity", typeof(float), false);
			this.InputOptions.Add("water_choppiness", typeof(float), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
