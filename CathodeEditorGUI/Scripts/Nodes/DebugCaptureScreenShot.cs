using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DebugCaptureScreenShot : STNode
	{
		private int _m_wait_for_streamer;
		[STNodeProperty("wait_for_streamer", "wait_for_streamer")]
		public int m_wait_for_streamer
		{
			get { return _m_wait_for_streamer; }
			set { _m_wait_for_streamer = value; this.Invalidate(); }
		}
		
		private string _m_capture_filename;
		[STNodeProperty("capture_filename", "capture_filename")]
		public string m_capture_filename
		{
			get { return _m_capture_filename; }
			set { _m_capture_filename = value; this.Invalidate(); }
		}
		
		private float _m_fov;
		[STNodeProperty("fov", "fov")]
		public float m_fov
		{
			get { return _m_fov; }
			set { _m_fov = value; this.Invalidate(); }
		}
		
		private float _m_near;
		[STNodeProperty("near", "near")]
		public float m_near
		{
			get { return _m_near; }
			set { _m_near = value; this.Invalidate(); }
		}
		
		private float _m_far;
		[STNodeProperty("far", "far")]
		public float m_far
		{
			get { return _m_far; }
			set { _m_far = value; this.Invalidate(); }
		}
		
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "DebugCaptureScreenShot";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("finished_capturing", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
