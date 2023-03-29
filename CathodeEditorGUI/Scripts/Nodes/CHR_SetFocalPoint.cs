#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_SetFocalPoint : STNode
	{
		private string _m_priority;
		[STNodeProperty("priority", "priority")]
		public string m_priority
		{
			get { return _m_priority; }
			set { _m_priority = value; this.Invalidate(); }
		}
		
		private string _m_speed;
		[STNodeProperty("speed", "speed")]
		public string m_speed
		{
			get { return _m_speed; }
			set { _m_speed = value; this.Invalidate(); }
		}
		
		private bool _m_steal_camera;
		[STNodeProperty("steal_camera", "steal_camera")]
		public bool m_steal_camera
		{
			get { return _m_steal_camera; }
			set { _m_steal_camera = value; this.Invalidate(); }
		}
		
		private bool _m_line_of_sight_test;
		[STNodeProperty("line_of_sight_test", "line_of_sight_test")]
		public bool m_line_of_sight_test
		{
			get { return _m_line_of_sight_test; }
			set { _m_line_of_sight_test = value; this.Invalidate(); }
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
			
			this.Title = "CHR_SetFocalPoint";
			
			this.InputOptions.Add("focal_point", typeof(cTransform), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
#endif
