using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundRTPCController : STNode
	{
		private bool _m_stealth_default_on;
		[STNodeProperty("stealth_default_on", "stealth_default_on")]
		public bool m_stealth_default_on
		{
			get { return _m_stealth_default_on; }
			set { _m_stealth_default_on = value; this.Invalidate(); }
		}
		
		private bool _m_threat_default_on;
		[STNodeProperty("threat_default_on", "threat_default_on")]
		public bool m_threat_default_on
		{
			get { return _m_threat_default_on; }
			set { _m_threat_default_on = value; this.Invalidate(); }
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
			
			this.Title = "SoundRTPCController";
			
			this.InputOptions.Add("enable_stealth", typeof(void), false);
			this.InputOptions.Add("disable_stealth", typeof(void), false);
			this.InputOptions.Add("enable_threat", typeof(void), false);
			this.InputOptions.Add("disable_threat", typeof(void), false);
			
		}
	}
}
