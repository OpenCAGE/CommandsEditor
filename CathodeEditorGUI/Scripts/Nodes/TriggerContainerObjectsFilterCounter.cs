#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerContainerObjectsFilterCounter : STNode
	{
		private STNode _m_container;
		[STNodeProperty("container", "container")]
		public STNode m_container
		{
			get { return _m_container; }
			set { _m_container = value; this.Invalidate(); }
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
			
			this.Title = "TriggerContainerObjectsFilterCounter";
			
			this.InputOptions.Add("filter", typeof(bool), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("none_passed", typeof(void), false);
			this.OutputOptions.Add("some_passed", typeof(void), false);
			this.OutputOptions.Add("all_passed", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
