#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TRAV_1ShotSpline : STNode
	{
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_open_on_reset;
		[STNodeProperty("open_on_reset", "open_on_reset")]
		public bool m_open_on_reset
		{
			get { return _m_open_on_reset; }
			set { _m_open_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_template;
		[STNodeProperty("template", "template")]
		public bool m_template
		{
			get { return _m_template; }
			set { _m_template = value; this.Invalidate(); }
		}
		
		private float _m_headroom;
		[STNodeProperty("headroom", "headroom")]
		public float m_headroom
		{
			get { return _m_headroom; }
			set { _m_headroom = value; this.Invalidate(); }
		}
		
		private float _m_extra_cost;
		[STNodeProperty("extra_cost", "extra_cost")]
		public float m_extra_cost
		{
			get { return _m_extra_cost; }
			set { _m_extra_cost = value; this.Invalidate(); }
		}
		
		private bool _m_fit_end_to_edge;
		[STNodeProperty("fit_end_to_edge", "fit_end_to_edge")]
		public bool m_fit_end_to_edge
		{
			get { return _m_fit_end_to_edge; }
			set { _m_fit_end_to_edge = value; this.Invalidate(); }
		}
		
		private string _m_min_speed;
		[STNodeProperty("min_speed", "min_speed")]
		public string m_min_speed
		{
			get { return _m_min_speed; }
			set { _m_min_speed = value; this.Invalidate(); }
		}
		
		private string _m_max_speed;
		[STNodeProperty("max_speed", "max_speed")]
		public string m_max_speed
		{
			get { return _m_max_speed; }
			set { _m_max_speed = value; this.Invalidate(); }
		}
		
		private string _m_animationTree;
		[STNodeProperty("animationTree", "animationTree")]
		public string m_animationTree
		{
			get { return _m_animationTree; }
			set { _m_animationTree = value; this.Invalidate(); }
		}
		
		private string _m_character_classes;
		[STNodeProperty("character_classes", "character_classes")]
		public string m_character_classes
		{
			get { return _m_character_classes; }
			set { _m_character_classes = value; this.Invalidate(); }
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
			
			this.Title = "TRAV_1ShotSpline";
			
			this.InputOptions.Add("EntrancePath", typeof(string), false);
			this.InputOptions.Add("ExitPath", typeof(string), false);
			this.InputOptions.Add("MinimumPath", typeof(string), false);
			this.InputOptions.Add("MaximumPath", typeof(string), false);
			this.InputOptions.Add("MinimumSupport", typeof(string), false);
			this.InputOptions.Add("MaximumSupport", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("open", typeof(void), false);
			this.InputOptions.Add("close", typeof(void), false);
			
			this.OutputOptions.Add("OnEnter", typeof(void), false);
			this.OutputOptions.Add("OnExit", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("opened", typeof(void), false);
			this.OutputOptions.Add("closed", typeof(void), false);
		}
	}
}
#endif
