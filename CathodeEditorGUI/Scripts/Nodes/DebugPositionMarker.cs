using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugPositionMarker : STNode
	{
		private cTransform _m_world_pos;
		[STNodeProperty("world_pos", "world_pos")]
		public cTransform m_world_pos
		{
			get { return _m_world_pos; }
			set { _m_world_pos = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "DebugPositionMarker";
			
			
		}
	}
}
