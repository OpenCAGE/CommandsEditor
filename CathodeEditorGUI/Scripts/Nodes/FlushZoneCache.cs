#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FlushZoneCache : STNode
	{
		private bool _m_CurrentGen;
		[STNodeProperty("CurrentGen", "CurrentGen")]
		public bool m_CurrentGen
		{
			get { return _m_CurrentGen; }
			set { _m_CurrentGen = value; this.Invalidate(); }
		}
		
		private bool _m_NextGen;
		[STNodeProperty("NextGen", "NextGen")]
		public bool m_NextGen
		{
			get { return _m_NextGen; }
			set { _m_NextGen = value; this.Invalidate(); }
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
			
			this.Title = "FlushZoneCache";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
