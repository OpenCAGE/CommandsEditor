#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundImpact : STNode
	{
		private string _m_sound_event;
		[STNodeProperty("sound_event", "sound_event")]
		public string m_sound_event
		{
			get { return _m_sound_event; }
			set { _m_sound_event = value; this.Invalidate(); }
		}
		
		private bool _m_is_occludable;
		[STNodeProperty("is_occludable", "is_occludable")]
		public bool m_is_occludable
		{
			get { return _m_is_occludable; }
			set { _m_is_occludable = value; this.Invalidate(); }
		}
		
		private string _m_argument_1;
		[STNodeProperty("argument_1", "argument_1")]
		public string m_argument_1
		{
			get { return _m_argument_1; }
			set { _m_argument_1 = value; this.Invalidate(); }
		}
		
		private string _m_argument_2;
		[STNodeProperty("argument_2", "argument_2")]
		public string m_argument_2
		{
			get { return _m_argument_2; }
			set { _m_argument_2 = value; this.Invalidate(); }
		}
		
		private string _m_argument_3;
		[STNodeProperty("argument_3", "argument_3")]
		public string m_argument_3
		{
			get { return _m_argument_3; }
			set { _m_argument_3 = value; this.Invalidate(); }
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
			
			this.Title = "SoundImpact";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
