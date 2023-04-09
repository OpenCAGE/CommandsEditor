using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_SetMood : STNode
	{
		private string _m_mood;
		[STNodeProperty("mood", "mood")]
		public string m_mood
		{
			get { return _m_mood; }
			set { _m_mood = value; this.Invalidate(); }
		}
		
		private string _m_moodIntensity;
		[STNodeProperty("moodIntensity", "moodIntensity")]
		public string m_moodIntensity
		{
			get { return _m_moodIntensity; }
			set { _m_moodIntensity = value; this.Invalidate(); }
		}
		
		private float _m_timeOut;
		[STNodeProperty("timeOut", "timeOut")]
		public float m_timeOut
		{
			get { return _m_timeOut; }
			set { _m_timeOut = value; this.Invalidate(); }
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
			
			this.Title = "CHR_SetMood";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
