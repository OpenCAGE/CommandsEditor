#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Squad_SetMaxEscalationLevel : STNode
	{
		private string _m_max_level;
		[STNodeProperty("max_level", "max_level")]
		public string m_max_level
		{
			get { return _m_max_level; }
			set { _m_max_level = value; this.Invalidate(); }
		}
		
		private STNode _m_squad_coordinator;
		[STNodeProperty("squad_coordinator", "squad_coordinator")]
		public STNode m_squad_coordinator
		{
			get { return _m_squad_coordinator; }
			set { _m_squad_coordinator = value; this.Invalidate(); }
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
			
			this.Title = "Squad_SetMaxEscalationLevel";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
