#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayerCamera : STNode
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "PlayerCamera";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
		}
	}
}
#endif
