#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Door : STNode
	{
		private string _m_unlocked_text;
		[STNodeProperty("unlocked_text", "unlocked_text")]
		public string m_unlocked_text
		{
			get { return _m_unlocked_text; }
			set { _m_unlocked_text = value; this.Invalidate(); }
		}
		
		private string _m_locked_text;
		[STNodeProperty("locked_text", "locked_text")]
		public string m_locked_text
		{
			get { return _m_locked_text; }
			set { _m_locked_text = value; this.Invalidate(); }
		}
		
		private string _m_icon_keyframe;
		[STNodeProperty("icon_keyframe", "icon_keyframe")]
		public string m_icon_keyframe
		{
			get { return _m_icon_keyframe; }
			set { _m_icon_keyframe = value; this.Invalidate(); }
		}
		
		private bool _m_detach_anim;
		[STNodeProperty("detach_anim", "detach_anim")]
		public bool m_detach_anim
		{
			get { return _m_detach_anim; }
			set { _m_detach_anim = value; this.Invalidate(); }
		}
		
		private bool _m_invert_nav_mesh_barrier;
		[STNodeProperty("invert_nav_mesh_barrier", "invert_nav_mesh_barrier")]
		public bool m_invert_nav_mesh_barrier
		{
			get { return _m_invert_nav_mesh_barrier; }
			set { _m_invert_nav_mesh_barrier = value; this.Invalidate(); }
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
			
			this.Title = "Door";
			
			this.InputOptions.Add("zone_link", typeof(string), false);
			this.InputOptions.Add("animation", typeof(string), false);
			this.InputOptions.Add("trigger_filter", typeof(bool), false);
			this.InputOptions.Add("icon_pos", typeof(cTransform), false);
			this.InputOptions.Add("icon_usable_radius", typeof(float), false);
			this.InputOptions.Add("show_icon_when_locked", typeof(bool), false);
			this.InputOptions.Add("nav_mesh", typeof(string), false);
			this.InputOptions.Add("wait_point_1", typeof(int), false);
			this.InputOptions.Add("wait_point_2", typeof(int), false);
			this.InputOptions.Add("geometry", typeof(STNode), false);
			this.InputOptions.Add("is_scripted", typeof(bool), false);
			this.InputOptions.Add("wait_to_open", typeof(bool), false);
			this.InputOptions.Add("request_open", typeof(void), false);
			this.InputOptions.Add("request_close", typeof(void), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("request_open_on_reset", typeof(bool), false);
			this.InputOptions.Add("request_lock_on_reset", typeof(bool), false);
			this.InputOptions.Add("force_open_on_reset", typeof(bool), false);
			this.InputOptions.Add("force_close_on_reset", typeof(bool), false);
			this.InputOptions.Add("is_auto", typeof(bool), false);
			this.InputOptions.Add("auto_close_delay", typeof(float), false);
			this.InputOptions.Add("request_lock", typeof(void), false);
			this.InputOptions.Add("request_unlock", typeof(void), false);
			this.InputOptions.Add("force_open", typeof(void), false);
			this.InputOptions.Add("force_close", typeof(void), false);
			this.InputOptions.Add("request_restore", typeof(void), false);
			
			this.OutputOptions.Add("started_opening", typeof(void), false);
			this.OutputOptions.Add("started_closing", typeof(void), false);
			this.OutputOptions.Add("finished_opening", typeof(void), false);
			this.OutputOptions.Add("finished_closing", typeof(void), false);
			this.OutputOptions.Add("used_locked", typeof(void), false);
			this.OutputOptions.Add("used_unlocked", typeof(void), false);
			this.OutputOptions.Add("used_forced_open", typeof(void), false);
			this.OutputOptions.Add("used_forced_closed", typeof(void), false);
			this.OutputOptions.Add("waiting_to_open", typeof(void), false);
			this.OutputOptions.Add("highlight", typeof(void), false);
			this.OutputOptions.Add("unhighlight", typeof(void), false);
			this.OutputOptions.Add("is_waiting", typeof(bool), false);
			this.OutputOptions.Add("requested_open", typeof(void), false);
			this.OutputOptions.Add("requested_close", typeof(void), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("gate_status_changed", typeof(void), false);
			this.OutputOptions.Add("is_open", typeof(bool), false);
			this.OutputOptions.Add("is_locked", typeof(bool), false);
			this.OutputOptions.Add("gate_status", typeof(int), false);
			this.OutputOptions.Add("requested_lock", typeof(void), false);
			this.OutputOptions.Add("requested_unlock", typeof(void), false);
			this.OutputOptions.Add("forced_open", typeof(void), false);
			this.OutputOptions.Add("forced_close", typeof(void), false);
			this.OutputOptions.Add("requested_restore", typeof(void), false);
		}
	}
}
#endif
