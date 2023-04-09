using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_Coordinator : STNode
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
			
			this.Title = "NPC_Coordinator";
			
			this.InputOptions.Add("add_character", typeof(void), false);
			this.InputOptions.Add("remove_character", typeof(void), false);
			this.InputOptions.Add("update_squad_params", typeof(void), false);
			
			this.OutputOptions.Add("added", typeof(void), false);
			this.OutputOptions.Add("removed", typeof(void), false);
			this.OutputOptions.Add("squad_params_updated", typeof(void), false);
		}
	}
}
