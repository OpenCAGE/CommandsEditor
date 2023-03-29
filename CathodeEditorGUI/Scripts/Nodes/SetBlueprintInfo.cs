#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetBlueprintInfo : STNode
	{
		private string _m_type;
		[STNodeProperty("type", "type")]
		public string m_type
		{
			get { return _m_type; }
			set { _m_type = value; this.Invalidate(); }
		}
		
		private string _m_level;
		[STNodeProperty("level", "level")]
		public string m_level
		{
			get { return _m_level; }
			set { _m_level = value; this.Invalidate(); }
		}
		
		private bool _m_available;
		[STNodeProperty("available", "available")]
		public bool m_available
		{
			get { return _m_available; }
			set { _m_available = value; this.Invalidate(); }
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
			
			this.Title = "SetBlueprintInfo";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
