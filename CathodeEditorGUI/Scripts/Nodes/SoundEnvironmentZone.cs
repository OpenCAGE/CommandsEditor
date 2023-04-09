using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundEnvironmentZone : STNode
	{
		private string _m_reverb_name;
		[STNodeProperty("reverb_name", "reverb_name")]
		public string m_reverb_name
		{
			get { return _m_reverb_name; }
			set { _m_reverb_name = value; this.Invalidate(); }
		}
		
		private int _m_priority;
		[STNodeProperty("priority", "priority")]
		public int m_priority
		{
			get { return _m_priority; }
			set { _m_priority = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
		}
		
		private cVector3 _m_half_dimensions;
		[STNodeProperty("half_dimensions", "half_dimensions")]
		public cVector3 m_half_dimensions
		{
			get { return _m_half_dimensions; }
			set { _m_half_dimensions = value; this.Invalidate(); }
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
			
			this.Title = "SoundEnvironmentZone";
			
			
		}
	}
}
