#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GetClosestPointOnSpline : STNode
	{
		private float _m_look_ahead_distance;
		[STNodeProperty("look_ahead_distance", "look_ahead_distance")]
		public float m_look_ahead_distance
		{
			get { return _m_look_ahead_distance; }
			set { _m_look_ahead_distance = value; this.Invalidate(); }
		}
		
		private bool _m_unidirectional;
		[STNodeProperty("unidirectional", "unidirectional")]
		public bool m_unidirectional
		{
			get { return _m_unidirectional; }
			set { _m_unidirectional = value; this.Invalidate(); }
		}
		
		private float _m_directional_damping_threshold;
		[STNodeProperty("directional_damping_threshold", "directional_damping_threshold")]
		public float m_directional_damping_threshold
		{
			get { return _m_directional_damping_threshold; }
			set { _m_directional_damping_threshold = value; this.Invalidate(); }
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
			
			this.Title = "GetClosestPointOnSpline";
			
			this.InputOptions.Add("spline", typeof(STNode), false);
			this.InputOptions.Add("pos_to_be_near", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("position_on_spline", typeof(cTransform), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
