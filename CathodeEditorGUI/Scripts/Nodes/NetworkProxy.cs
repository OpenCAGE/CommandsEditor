#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NetworkProxy : STNode
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "NetworkProxy";
			
			
		}
	}
}
#endif
