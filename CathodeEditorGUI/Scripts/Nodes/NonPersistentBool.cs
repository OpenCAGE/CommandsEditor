using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NonPersistentBool : STNode
	{
		private bool _m_initial_value;
		[STNodeProperty("initial_value", "initial_value")]
		public bool m_initial_value
		{
			get { return _m_initial_value; }
			set { _m_initial_value = value; this.Invalidate(); }
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
			
			this.Title = "NonPersistentBool";
			
			this.InputOptions.Add("set_true", typeof(void), false);
			this.InputOptions.Add("set_false", typeof(void), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("set_to_true", typeof(void), false);
			this.OutputOptions.Add("set_to_false", typeof(void), false);
			this.OutputOptions.Add("on_changed", typeof(void), false);
			this.OutputOptions.Add("on_restored", typeof(void), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
