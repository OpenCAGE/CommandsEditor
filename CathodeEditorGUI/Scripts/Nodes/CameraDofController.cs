#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CameraDofController : STNode
	{
		private cVector3 _m_focal_point_offset;
		[STNodeProperty("focal_point_offset", "focal_point_offset")]
		public cVector3 m_focal_point_offset
		{
			get { return _m_focal_point_offset; }
			set { _m_focal_point_offset = value; this.Invalidate(); }
		}
		
		private string _m_bone_to_focus;
		[STNodeProperty("bone_to_focus", "bone_to_focus")]
		public string m_bone_to_focus
		{
			get { return _m_bone_to_focus; }
			set { _m_bone_to_focus = value; this.Invalidate(); }
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
		
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_behavior_name;
		[STNodeProperty("behavior_name", "behavior_name")]
		public string m_behavior_name
		{
			get { return _m_behavior_name; }
			set { _m_behavior_name = value; this.Invalidate(); }
		}
		
		private int _m_priority;
		[STNodeProperty("priority", "priority")]
		public int m_priority
		{
			get { return _m_priority; }
			set { _m_priority = value; this.Invalidate(); }
		}
		
		private float _m_threshold;
		[STNodeProperty("threshold", "threshold")]
		public float m_threshold
		{
			get { return _m_threshold; }
			set { _m_threshold = value; this.Invalidate(); }
		}
		
		private float _m_blend_in;
		[STNodeProperty("blend_in", "blend_in")]
		public float m_blend_in
		{
			get { return _m_blend_in; }
			set { _m_blend_in = value; this.Invalidate(); }
		}
		
		private float _m_duration;
		[STNodeProperty("duration", "duration")]
		public float m_duration
		{
			get { return _m_duration; }
			set { _m_duration = value; this.Invalidate(); }
		}
		
		private float _m_blend_out;
		[STNodeProperty("blend_out", "blend_out")]
		public float m_blend_out
		{
			get { return _m_blend_out; }
			set { _m_blend_out = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "CameraDofController";
			
			this.InputOptions.Add("character_to_focus", typeof(STNode), false);
			this.InputOptions.Add("focal_length_mm", typeof(float), false);
			this.InputOptions.Add("focal_plane_m", typeof(float), false);
			this.InputOptions.Add("fnum", typeof(float), false);
			this.InputOptions.Add("focal_point", typeof(cTransform), false);
			this.InputOptions.Add("linked_cameras", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("activate_behavior", typeof(void), false);
			this.InputOptions.Add("deactivate_behavior", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("behavior_activated", typeof(void), false);
			this.OutputOptions.Add("behavior_deactivated", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
