using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_ResetSensesAndMemory : STNode
	{
		private bool _m_ResetMenaceToFull;
		[STNodeProperty("ResetMenaceToFull", "ResetMenaceToFull")]
		public bool m_ResetMenaceToFull
		{
			get { return _m_ResetMenaceToFull; }
			set { _m_ResetMenaceToFull = value; this.Invalidate(); }
		}
		
		private bool _m_ResetSensesLimiters;
		[STNodeProperty("ResetSensesLimiters", "ResetSensesLimiters")]
		public bool m_ResetSensesLimiters
		{
			get { return _m_ResetSensesLimiters; }
			set { _m_ResetSensesLimiters = value; this.Invalidate(); }
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
			
			this.Title = "NPC_ResetSensesAndMemory";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
