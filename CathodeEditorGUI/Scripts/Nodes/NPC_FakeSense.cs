#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_FakeSense : STNode
	{
		private string _m_Sense;
		[STNodeProperty("Sense", "Sense")]
		public string m_Sense
		{
			get { return _m_Sense; }
			set { _m_Sense = value; this.Invalidate(); }
		}
		
		private string _m_ForceThreshold;
		[STNodeProperty("ForceThreshold", "ForceThreshold")]
		public string m_ForceThreshold
		{
			get { return _m_ForceThreshold; }
			set { _m_ForceThreshold = value; this.Invalidate(); }
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
			
			this.Title = "NPC_FakeSense";
			
			this.InputOptions.Add("SensedObject", typeof(STNode), false);
			this.InputOptions.Add("FakePosition", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
