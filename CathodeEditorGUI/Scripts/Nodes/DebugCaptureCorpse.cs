using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugCaptureCorpse : STNode
	{
		private string _m_corpse_name;
		[STNodeProperty("corpse_name", "corpse_name")]
		public string m_corpse_name
		{
			get { return _m_corpse_name; }
			set { _m_corpse_name = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "DebugCaptureCorpse";
			
			this.InputOptions.Add("character", typeof(STNode), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("finished_capturing", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
