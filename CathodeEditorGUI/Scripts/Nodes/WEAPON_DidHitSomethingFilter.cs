using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_DidHitSomethingFilter : STNode
	{
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "WEAPON_DidHitSomethingFilter";
			
			
			this.OutputOptions.Add("passed", typeof(void), false);
			this.OutputOptions.Add("failed", typeof(void), false);
		}
	}
}
