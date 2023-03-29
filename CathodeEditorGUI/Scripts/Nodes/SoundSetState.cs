#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundSetState : STNode
	{
		private string _m_state_name;
		[STNodeProperty("state_name", "state_name")]
		public string m_state_name
		{
			get { return _m_state_name; }
			set { _m_state_name = value; this.Invalidate(); }
		}
		
		private string _m_state_value;
		[STNodeProperty("state_value", "state_value")]
		public string m_state_value
		{
			get { return _m_state_value; }
			set { _m_state_value = value; this.Invalidate(); }
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
			
			this.Title = "SoundSetState";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
