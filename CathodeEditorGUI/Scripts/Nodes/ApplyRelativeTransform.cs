using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ApplyRelativeTransform : STNode
	{
		private bool _m_use_trigger_entity;
		[STNodeProperty("use_trigger_entity", "use_trigger_entity")]
		public bool m_use_trigger_entity
		{
			get { return _m_use_trigger_entity; }
			set { _m_use_trigger_entity = value; this.Invalidate(); }
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
			
			this.Title = "ApplyRelativeTransform";
			
			this.InputOptions.Add("origin", typeof(cTransform), false);
			this.InputOptions.Add("destination", typeof(cTransform), false);
			this.InputOptions.Add("input", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("output", typeof(cTransform), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
