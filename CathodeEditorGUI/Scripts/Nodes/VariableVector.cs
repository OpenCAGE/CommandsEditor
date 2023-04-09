using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class VariableVector : STNode
	{
		private float _m_initial_x;
		[STNodeProperty("initial_x", "initial_x")]
		public float m_initial_x
		{
			get { return _m_initial_x; }
			set { _m_initial_x = value; this.Invalidate(); }
		}
		
		private float _m_initial_y;
		[STNodeProperty("initial_y", "initial_y")]
		public float m_initial_y
		{
			get { return _m_initial_y; }
			set { _m_initial_y = value; this.Invalidate(); }
		}
		
		private float _m_initial_z;
		[STNodeProperty("initial_z", "initial_z")]
		public float m_initial_z
		{
			get { return _m_initial_z; }
			set { _m_initial_z = value; this.Invalidate(); }
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
			
			this.Title = "VariableVector";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("on_changed", typeof(void), false);
			this.OutputOptions.Add("on_restored", typeof(void), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
