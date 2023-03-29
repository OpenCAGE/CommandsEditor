#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class UnlockMapDetail : STNode
	{
		private string _m_map_keyframe;
		[STNodeProperty("map_keyframe", "map_keyframe")]
		public string m_map_keyframe
		{
			get { return _m_map_keyframe; }
			set { _m_map_keyframe = value; this.Invalidate(); }
		}
		
		private string _m_details;
		[STNodeProperty("details", "details")]
		public string m_details
		{
			get { return _m_details; }
			set { _m_details = value; this.Invalidate(); }
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
			
			this.Title = "UnlockMapDetail";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
