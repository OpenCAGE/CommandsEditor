using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ZoneExclusionLink : STNode
	{
		private bool _m_exclude_streaming;
		[STNodeProperty("exclude_streaming", "exclude_streaming")]
		public bool m_exclude_streaming
		{
			get { return _m_exclude_streaming; }
			set { _m_exclude_streaming = value; this.Invalidate(); }
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
			
			this.Title = "ZoneExclusionLink";
			
			this.InputOptions.Add("ZoneA", typeof(string), false);
			this.InputOptions.Add("ZoneB", typeof(string), false);
			
		}
	}
}
