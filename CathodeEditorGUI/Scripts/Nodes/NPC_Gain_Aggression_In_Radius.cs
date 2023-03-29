#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_Gain_Aggression_In_Radius : STNode
	{
		private string _m_AggressionGain;
		[STNodeProperty("AggressionGain", "AggressionGain")]
		public string m_AggressionGain
		{
			get { return _m_AggressionGain; }
			set { _m_AggressionGain = value; this.Invalidate(); }
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
			
			this.Title = "NPC_Gain_Aggression_In_Radius";
			
			this.InputOptions.Add("Position", typeof(cTransform), false);
			this.InputOptions.Add("Radius", typeof(float), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
