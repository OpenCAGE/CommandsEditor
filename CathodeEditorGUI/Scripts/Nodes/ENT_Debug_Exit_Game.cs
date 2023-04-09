using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ENT_Debug_Exit_Game : STNode
	{
		private string _m_FailureText;
		[STNodeProperty("FailureText", "FailureText")]
		public string m_FailureText
		{
			get { return _m_FailureText; }
			set { _m_FailureText = value; this.Invalidate(); }
		}
		
		private int _m_FailureCode;
		[STNodeProperty("FailureCode", "FailureCode")]
		public int m_FailureCode
		{
			get { return _m_FailureCode; }
			set { _m_FailureCode = value; this.Invalidate(); }
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
			
			this.Title = "ENT_Debug_Exit_Game";
			
			this.InputOptions.Add("fail_game", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
