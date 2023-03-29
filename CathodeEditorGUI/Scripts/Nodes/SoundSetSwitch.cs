#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundSetSwitch : STNode
	{
		private string _m_switch_name;
		[STNodeProperty("switch_name", "switch_name")]
		public string m_switch_name
		{
			get { return _m_switch_name; }
			set { _m_switch_name = value; this.Invalidate(); }
		}
		
		private string _m_switch_value;
		[STNodeProperty("switch_value", "switch_value")]
		public string m_switch_value
		{
			get { return _m_switch_value; }
			set { _m_switch_value = value; this.Invalidate(); }
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
			
			this.Title = "SoundSetSwitch";
			
			this.InputOptions.Add("sound_object", typeof(STNode), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
