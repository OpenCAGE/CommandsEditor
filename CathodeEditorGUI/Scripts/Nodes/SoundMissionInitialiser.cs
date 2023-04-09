using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundMissionInitialiser : STNode
	{
		private float _m_human_max_threat;
		[STNodeProperty("human_max_threat", "human_max_threat")]
		public float m_human_max_threat
		{
			get { return _m_human_max_threat; }
			set { _m_human_max_threat = value; this.Invalidate(); }
		}
		
		private float _m_android_max_threat;
		[STNodeProperty("android_max_threat", "android_max_threat")]
		public float m_android_max_threat
		{
			get { return _m_android_max_threat; }
			set { _m_android_max_threat = value; this.Invalidate(); }
		}
		
		private float _m_alien_max_threat;
		[STNodeProperty("alien_max_threat", "alien_max_threat")]
		public float m_alien_max_threat
		{
			get { return _m_alien_max_threat; }
			set { _m_alien_max_threat = value; this.Invalidate(); }
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
			
			this.Title = "SoundMissionInitialiser";
			
			
		}
	}
}
