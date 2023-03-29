#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class EFFECT_ImpactGenerator : STNode
	{
		private bool _m_trigger_on_reset;
		[STNodeProperty("trigger_on_reset", "trigger_on_reset")]
		public bool m_trigger_on_reset
		{
			get { return _m_trigger_on_reset; }
			set { _m_trigger_on_reset = value; this.Invalidate(); }
		}
		
		private float _m_min_distance;
		[STNodeProperty("min_distance", "min_distance")]
		public float m_min_distance
		{
			get { return _m_min_distance; }
			set { _m_min_distance = value; this.Invalidate(); }
		}
		
		private float _m_distance;
		[STNodeProperty("distance", "distance")]
		public float m_distance
		{
			get { return _m_distance; }
			set { _m_distance = value; this.Invalidate(); }
		}
		
		private int _m_max_count;
		[STNodeProperty("max_count", "max_count")]
		public int m_max_count
		{
			get { return _m_max_count; }
			set { _m_max_count = value; this.Invalidate(); }
		}
		
		private int _m_count;
		[STNodeProperty("count", "count")]
		public int m_count
		{
			get { return _m_count; }
			set { _m_count = value; this.Invalidate(); }
		}
		
		private float _m_spread;
		[STNodeProperty("spread", "spread")]
		public float m_spread
		{
			get { return _m_spread; }
			set { _m_spread = value; this.Invalidate(); }
		}
		
		private bool _m_skip_characters;
		[STNodeProperty("skip_characters", "skip_characters")]
		public bool m_skip_characters
		{
			get { return _m_skip_characters; }
			set { _m_skip_characters = value; this.Invalidate(); }
		}
		
		private bool _m_use_local_rotation;
		[STNodeProperty("use_local_rotation", "use_local_rotation")]
		public bool m_use_local_rotation
		{
			get { return _m_use_local_rotation; }
			set { _m_use_local_rotation = value; this.Invalidate(); }
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
			
			this.Title = "EFFECT_ImpactGenerator";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("on_impact", typeof(void), false);
			this.OutputOptions.Add("on_failed", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
