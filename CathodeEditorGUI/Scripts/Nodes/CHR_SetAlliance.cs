#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_SetAlliance : STNode
	{
		private string _m_Alliance;
		[STNodeProperty("Alliance", "Alliance")]
		public string m_Alliance
		{
			get { return _m_Alliance; }
			set { _m_Alliance = value; this.Invalidate(); }
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
			
			this.Title = "CHR_SetAlliance";
			
			this.InputOptions.Add("set", typeof(void), false);
			
			this.OutputOptions.Add("been_set", typeof(void), false);
		}
	}
}
#endif
