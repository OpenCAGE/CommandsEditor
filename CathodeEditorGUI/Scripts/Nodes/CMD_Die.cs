using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_Die : STNode
	{
		private string _m_death_style;
		[STNodeProperty("death_style", "death_style")]
		public string m_death_style
		{
			get { return _m_death_style; }
			set { _m_death_style = value; this.Invalidate(); }
		}
		
		private bool _m_override_all_ai;
		[STNodeProperty("override_all_ai", "override_all_ai")]
		public bool m_override_all_ai
		{
			get { return _m_override_all_ai; }
			set { _m_override_all_ai = value; this.Invalidate(); }
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
			
			this.Title = "CMD_Die";
			
			this.InputOptions.Add("Killer", typeof(STNode), false);
			this.InputOptions.Add("kill", typeof(void), false);
			
			this.OutputOptions.Add("killed", typeof(void), false);
			this.OutputOptions.Add("command_started", typeof(void), false);
		}
	}
}
