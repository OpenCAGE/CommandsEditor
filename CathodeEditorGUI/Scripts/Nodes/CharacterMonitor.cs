#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CharacterMonitor : STNode
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "CharacterMonitor";
			
			this.InputOptions.Add("character", typeof(string), false);
			
		}
	}
}
#endif
