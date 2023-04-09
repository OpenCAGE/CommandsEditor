using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_SetAlienDevelopmentStage : STNode
	{
		private string _m_AlienStage;
		[STNodeProperty("AlienStage", "AlienStage")]
		public string m_AlienStage
		{
			get { return _m_AlienStage; }
			set { _m_AlienStage = value; this.Invalidate(); }
		}
		
		private bool _m_Reset;
		[STNodeProperty("Reset", "Reset")]
		public bool m_Reset
		{
			get { return _m_Reset; }
			set { _m_Reset = value; this.Invalidate(); }
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
			
			this.Title = "NPC_SetAlienDevelopmentStage";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
