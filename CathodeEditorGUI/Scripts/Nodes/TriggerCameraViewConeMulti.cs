using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerCameraViewConeMulti : STNode
	{
		private int _m_number_of_inputs;
		[STNodeProperty("number_of_inputs", "number_of_inputs")]
		public int m_number_of_inputs
		{
			get { return _m_number_of_inputs; }
			set { _m_number_of_inputs = value; this.Invalidate(); }
		}
		
		private bool _m_use_camera_fov;
		[STNodeProperty("use_camera_fov", "use_camera_fov")]
		public bool m_use_camera_fov
		{
			get { return _m_use_camera_fov; }
			set { _m_use_camera_fov = value; this.Invalidate(); }
		}
		
		private string _m_visible_area_type;
		[STNodeProperty("visible_area_type", "visible_area_type")]
		public string m_visible_area_type
		{
			get { return _m_visible_area_type; }
			set { _m_visible_area_type = value; this.Invalidate(); }
		}
		
		private float _m_visible_area_horizontal;
		[STNodeProperty("visible_area_horizontal", "visible_area_horizontal")]
		public float m_visible_area_horizontal
		{
			get { return _m_visible_area_horizontal; }
			set { _m_visible_area_horizontal = value; this.Invalidate(); }
		}
		
		private float _m_visible_area_vertical;
		[STNodeProperty("visible_area_vertical", "visible_area_vertical")]
		public float m_visible_area_vertical
		{
			get { return _m_visible_area_vertical; }
			set { _m_visible_area_vertical = value; this.Invalidate(); }
		}
		
		private float _m_raycast_grace;
		[STNodeProperty("raycast_grace", "raycast_grace")]
		public float m_raycast_grace
		{
			get { return _m_raycast_grace; }
			set { _m_raycast_grace = value; this.Invalidate(); }
		}
		
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_pause_on_reset;
		[STNodeProperty("pause_on_reset", "pause_on_reset")]
		public bool m_pause_on_reset
		{
			get { return _m_pause_on_reset; }
			set { _m_pause_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "TriggerCameraViewConeMulti";
			
			this.InputOptions.Add("target", typeof(cTransform), false);
			this.InputOptions.Add("target1", typeof(cTransform), false);
			this.InputOptions.Add("target2", typeof(cTransform), false);
			this.InputOptions.Add("target3", typeof(cTransform), false);
			this.InputOptions.Add("target4", typeof(cTransform), false);
			this.InputOptions.Add("target5", typeof(cTransform), false);
			this.InputOptions.Add("target6", typeof(cTransform), false);
			this.InputOptions.Add("target7", typeof(cTransform), false);
			this.InputOptions.Add("target8", typeof(cTransform), false);
			this.InputOptions.Add("target9", typeof(cTransform), false);
			this.InputOptions.Add("fov", typeof(float), false);
			this.InputOptions.Add("aspect_ratio", typeof(float), false);
			this.InputOptions.Add("intersect_with_geometry", typeof(bool), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("enter", typeof(void), false);
			this.OutputOptions.Add("exit", typeof(void), false);
			this.OutputOptions.Add("enter1", typeof(void), false);
			this.OutputOptions.Add("exit1", typeof(void), false);
			this.OutputOptions.Add("enter2", typeof(void), false);
			this.OutputOptions.Add("exit2", typeof(void), false);
			this.OutputOptions.Add("enter3", typeof(void), false);
			this.OutputOptions.Add("exit3", typeof(void), false);
			this.OutputOptions.Add("enter4", typeof(void), false);
			this.OutputOptions.Add("exit4", typeof(void), false);
			this.OutputOptions.Add("enter5", typeof(void), false);
			this.OutputOptions.Add("exit5", typeof(void), false);
			this.OutputOptions.Add("enter6", typeof(void), false);
			this.OutputOptions.Add("exit6", typeof(void), false);
			this.OutputOptions.Add("enter7", typeof(void), false);
			this.OutputOptions.Add("exit7", typeof(void), false);
			this.OutputOptions.Add("enter8", typeof(void), false);
			this.OutputOptions.Add("exit8", typeof(void), false);
			this.OutputOptions.Add("enter9", typeof(void), false);
			this.OutputOptions.Add("exit9", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
