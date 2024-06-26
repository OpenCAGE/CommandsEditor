using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerCheckDifficulty : STNode
	{
		private string _m_DifficultyLevel;
		[STNodeProperty("DifficultyLevel", "DifficultyLevel")]
		public string m_DifficultyLevel
		{
			get { return _m_DifficultyLevel; }
			set { _m_DifficultyLevel = value; this.Invalidate(); }
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
			
			this.Title = "TriggerCheckDifficulty";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_success", typeof(void), false);
			this.OutputOptions.Add("on_failure", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
