#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CameraPath : STNode
	{
		private string _m_path_name;
		[STNodeProperty("path_name", "path_name")]
		public string m_path_name
		{
			get { return _m_path_name; }
			set { _m_path_name = value; this.Invalidate(); }
		}
		
		private string _m_path_type;
		[STNodeProperty("path_type", "path_type")]
		public string m_path_type
		{
			get { return _m_path_type; }
			set { _m_path_type = value; this.Invalidate(); }
		}
		
		private string _m_path_class;
		[STNodeProperty("path_class", "path_class")]
		public string m_path_class
		{
			get { return _m_path_class; }
			set { _m_path_class = value; this.Invalidate(); }
		}
		
		private bool _m_is_local;
		[STNodeProperty("is_local", "is_local")]
		public bool m_is_local
		{
			get { return _m_is_local; }
			set { _m_is_local = value; this.Invalidate(); }
		}
		
		private cTransform _m_relative_position;
		[STNodeProperty("relative_position", "relative_position")]
		public cTransform m_relative_position
		{
			get { return _m_relative_position; }
			set { _m_relative_position = value; this.Invalidate(); }
		}
		
		private bool _m_is_loop;
		[STNodeProperty("is_loop", "is_loop")]
		public bool m_is_loop
		{
			get { return _m_is_loop; }
			set { _m_is_loop = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "CameraPath";
			
			this.InputOptions.Add("linked_splines", typeof(string), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
		}
	}
}
#endif
