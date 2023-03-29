#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundEnvironmentMarker : STNode
	{
		private string _m_reverb_name;
		[STNodeProperty("reverb_name", "reverb_name")]
		public string m_reverb_name
		{
			get { return _m_reverb_name; }
			set { _m_reverb_name = value; this.Invalidate(); }
		}
		
		private string _m_on_enter_event;
		[STNodeProperty("on_enter_event", "on_enter_event")]
		public string m_on_enter_event
		{
			get { return _m_on_enter_event; }
			set { _m_on_enter_event = value; this.Invalidate(); }
		}
		
		private string _m_on_exit_event;
		[STNodeProperty("on_exit_event", "on_exit_event")]
		public string m_on_exit_event
		{
			get { return _m_on_exit_event; }
			set { _m_on_exit_event = value; this.Invalidate(); }
		}
		
		private float _m_linked_network_occlusion_scaler;
		[STNodeProperty("linked_network_occlusion_scaler", "linked_network_occlusion_scaler")]
		public float m_linked_network_occlusion_scaler
		{
			get { return _m_linked_network_occlusion_scaler; }
			set { _m_linked_network_occlusion_scaler = value; this.Invalidate(); }
		}
		
		private string _m_room_size;
		[STNodeProperty("room_size", "room_size")]
		public string m_room_size
		{
			get { return _m_room_size; }
			set { _m_room_size = value; this.Invalidate(); }
		}
		
		private bool _m_disable_network_creation;
		[STNodeProperty("disable_network_creation", "disable_network_creation")]
		public bool m_disable_network_creation
		{
			get { return _m_disable_network_creation; }
			set { _m_disable_network_creation = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
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
			
			this.Title = "SoundEnvironmentMarker";
			
			
		}
	}
}
#endif
