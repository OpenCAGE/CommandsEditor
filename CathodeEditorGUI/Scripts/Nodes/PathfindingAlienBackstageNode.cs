#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PathfindingAlienBackstageNode : STNode
	{
		private bool _m_open_on_reset;
		[STNodeProperty("open_on_reset", "open_on_reset")]
		public bool m_open_on_reset
		{
			get { return _m_open_on_reset; }
			set { _m_open_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_build_into_navmesh;
		[STNodeProperty("build_into_navmesh", "build_into_navmesh")]
		public bool m_build_into_navmesh
		{
			get { return _m_build_into_navmesh; }
			set { _m_build_into_navmesh = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
		}
		
		private cTransform _m_top;
		[STNodeProperty("top", "top")]
		public cTransform m_top
		{
			get { return _m_top; }
			set { _m_top = value; this.Invalidate(); }
		}
		
		private float _m_extra_cost;
		[STNodeProperty("extra_cost", "extra_cost")]
		public float m_extra_cost
		{
			get { return _m_extra_cost; }
			set { _m_extra_cost = value; this.Invalidate(); }
		}
		
		private int _m_network_id;
		[STNodeProperty("network_id", "network_id")]
		public int m_network_id
		{
			get { return _m_network_id; }
			set { _m_network_id = value; this.Invalidate(); }
		}
		
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "PathfindingAlienBackstageNode";
			
			this.InputOptions.Add("PlayAnimData_Entry", typeof(string), false);
			this.InputOptions.Add("PlayAnimData_Exit", typeof(string), false);
			this.InputOptions.Add("Killtrap_alien", typeof(string), false);
			this.InputOptions.Add("Killtrap_victim", typeof(string), false);
			this.InputOptions.Add("open", typeof(void), false);
			this.InputOptions.Add("close", typeof(void), false);
			this.InputOptions.Add("force_killtrap", typeof(void), false);
			this.InputOptions.Add("cancel_force_killtrap", typeof(void), false);
			this.InputOptions.Add("disable_killtrap", typeof(void), false);
			this.InputOptions.Add("cancel_disable_killtrap", typeof(void), false);
			this.InputOptions.Add("hit_by_flamethrower", typeof(void), false);
			this.InputOptions.Add("cancel_hit_by_flamethrower", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("started_animating_Entry", typeof(void), false);
			this.OutputOptions.Add("stopped_animating_Entry", typeof(void), false);
			this.OutputOptions.Add("started_animating_Exit", typeof(void), false);
			this.OutputOptions.Add("stopped_animating_Exit", typeof(void), false);
			this.OutputOptions.Add("killtrap_anim_started", typeof(void), false);
			this.OutputOptions.Add("killtrap_anim_stopped", typeof(void), false);
			this.OutputOptions.Add("killtrap_fx_start", typeof(void), false);
			this.OutputOptions.Add("killtrap_fx_stop", typeof(void), false);
			this.OutputOptions.Add("on_loaded", typeof(void), false);
			this.OutputOptions.Add("opened", typeof(void), false);
			this.OutputOptions.Add("closed", typeof(void), false);
			this.OutputOptions.Add("killtrap_forced", typeof(void), false);
			this.OutputOptions.Add("canceled_force_killtrap", typeof(void), false);
			this.OutputOptions.Add("upon_hit_by_flamethrower", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
