using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class StealCamera : STNode
	{
		private string _m_steal_type;
		[STNodeProperty("steal_type", "steal_type")]
		public string m_steal_type
		{
			get { return _m_steal_type; }
			set { _m_steal_type = value; this.Invalidate(); }
		}
		
		private bool _m_check_line_of_sight;
		[STNodeProperty("check_line_of_sight", "check_line_of_sight")]
		public bool m_check_line_of_sight
		{
			get { return _m_check_line_of_sight; }
			set { _m_check_line_of_sight = value; this.Invalidate(); }
		}
		
		private float _m_blend_in_duration;
		[STNodeProperty("blend_in_duration", "blend_in_duration")]
		public float m_blend_in_duration
		{
			get { return _m_blend_in_duration; }
			set { _m_blend_in_duration = value; this.Invalidate(); }
		}
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			this.Title = "StealCamera";
			
			this.InputOptions.Add("focus_position", typeof(cTransform), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			
			this.OutputOptions.Add("on_converged", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
		}
	}
}
