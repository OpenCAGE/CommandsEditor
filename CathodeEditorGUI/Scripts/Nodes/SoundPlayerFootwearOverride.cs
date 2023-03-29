#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundPlayerFootwearOverride : STNode
	{
		private string _m_footwear_sound;
		[STNodeProperty("footwear_sound", "footwear_sound")]
		public string m_footwear_sound
		{
			get { return _m_footwear_sound; }
			set { _m_footwear_sound = value; this.Invalidate(); }
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
			
			this.Title = "SoundPlayerFootwearOverride";
			
			this.InputOptions.Add("enable_override", typeof(void), false);
			this.InputOptions.Add("disable_override", typeof(void), false);
			
		}
	}
}
#endif
