using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CameraFinder : STNode
	{
		private string _m_camera_name;
		[STNodeProperty("camera_name", "camera_name")]
		public string m_camera_name
		{
			get { return _m_camera_name; }
			set { _m_camera_name = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "CameraFinder";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
		}
	}
}
