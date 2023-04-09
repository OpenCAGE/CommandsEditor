using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TogglePlayerTorch : STNode
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "TogglePlayerTorch";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
