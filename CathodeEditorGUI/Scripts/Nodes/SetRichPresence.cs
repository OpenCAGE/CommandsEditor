using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetRichPresence : STNode
	{
		private string _m_presence_id;
		[STNodeProperty("presence_id", "presence_id")]
		public string m_presence_id
		{
			get { return _m_presence_id; }
			set { _m_presence_id = value; this.Invalidate(); }
		}
		
		private float _m_mission_number;
		[STNodeProperty("mission_number", "mission_number")]
		public float m_mission_number
		{
			get { return _m_mission_number; }
			set { _m_mission_number = value; this.Invalidate(); }
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
			
			this.Title = "SetRichPresence";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
