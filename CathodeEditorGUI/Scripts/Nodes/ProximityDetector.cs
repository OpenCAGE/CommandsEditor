using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ProximityDetector : STNode
	{
		private float _m_min_distance;
		[STNodeProperty("min_distance", "min_distance")]
		public float m_min_distance
		{
			get { return _m_min_distance; }
			set { _m_min_distance = value; this.Invalidate(); }
		}
		
		private float _m_max_distance;
		[STNodeProperty("max_distance", "max_distance")]
		public float m_max_distance
		{
			get { return _m_max_distance; }
			set { _m_max_distance = value; this.Invalidate(); }
		}
		
		private bool _m_requires_line_of_sight;
		[STNodeProperty("requires_line_of_sight", "requires_line_of_sight")]
		public bool m_requires_line_of_sight
		{
			get { return _m_requires_line_of_sight; }
			set { _m_requires_line_of_sight = value; this.Invalidate(); }
		}
		
		private float _m_proximity_duration;
		[STNodeProperty("proximity_duration", "proximity_duration")]
		public float m_proximity_duration
		{
			get { return _m_proximity_duration; }
			set { _m_proximity_duration = value; this.Invalidate(); }
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
			
			this.Title = "ProximityDetector";
			
			this.InputOptions.Add("filter", typeof(bool), false);
			this.InputOptions.Add("detector_position", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("in_proximity", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
