using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_MeleeContext : STNode
	{
		private string _m_Context_Type;
		[STNodeProperty("Context_Type", "Context_Type")]
		public string m_Context_Type
		{
			get { return _m_Context_Type; }
			set { _m_Context_Type = value; this.Invalidate(); }
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
			
			this.Title = "NPC_MeleeContext";
			
			this.InputOptions.Add("ConvergePos", typeof(cTransform), false);
			this.InputOptions.Add("Radius", typeof(float), false);
			
		}
	}
}
