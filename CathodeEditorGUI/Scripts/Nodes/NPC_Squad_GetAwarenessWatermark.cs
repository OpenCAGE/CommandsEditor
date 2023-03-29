#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_Squad_GetAwarenessWatermark : STNode
	{
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
			
			this.Title = "NPC_Squad_GetAwarenessWatermark";
			
			this.InputOptions.Add("NPC_Coordinator", typeof(STNode), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("All_Dead", typeof(void), false);
			this.OutputOptions.Add("Stunned", typeof(void), false);
			this.OutputOptions.Add("Unaware", typeof(void), false);
			this.OutputOptions.Add("Suspicious", typeof(void), false);
			this.OutputOptions.Add("SearchingArea", typeof(void), false);
			this.OutputOptions.Add("SearchingLastSensed", typeof(void), false);
			this.OutputOptions.Add("Aware", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
