#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class UI_Icon : STNode
	{
		private bool _m_lock_on_reset;
		[STNodeProperty("lock_on_reset", "lock_on_reset")]
		public bool m_lock_on_reset
		{
			get { return _m_lock_on_reset; }
			set { _m_lock_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_show_on_reset;
		[STNodeProperty("show_on_reset", "show_on_reset")]
		public bool m_show_on_reset
		{
			get { return _m_show_on_reset; }
			set { _m_show_on_reset = value; this.Invalidate(); }
		}
		
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
		
		private string _m_action_text;
		[STNodeProperty("action_text", "action_text")]
		public string m_action_text
		{
			get { return _m_action_text; }
			set { _m_action_text = value; this.Invalidate(); }
		}
		
		private string _m_icon_keyframe;
		[STNodeProperty("icon_keyframe", "icon_keyframe")]
		public string m_icon_keyframe
		{
			get { return _m_icon_keyframe; }
			set { _m_icon_keyframe = value; this.Invalidate(); }
		}
		
		private bool _m_can_be_used;
		[STNodeProperty("can_be_used", "can_be_used")]
		public bool m_can_be_used
		{
			get { return _m_can_be_used; }
			set { _m_can_be_used = value; this.Invalidate(); }
		}
		
		private string _m_category;
		[STNodeProperty("category", "category")]
		public string m_category
		{
			get { return _m_category; }
			set { _m_category = value; this.Invalidate(); }
		}
		
		private float _m_push_hold_time;
		[STNodeProperty("push_hold_time", "push_hold_time")]
		public float m_push_hold_time
		{
			get { return _m_push_hold_time; }
			set { _m_push_hold_time = value; this.Invalidate(); }
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
			
			this.Title = "UI_Icon";
			
			this.InputOptions.Add("geometry", typeof(STNode), false);
			this.InputOptions.Add("highlight_geometry", typeof(STNode), false);
			this.InputOptions.Add("target_pickup_item", typeof(STNode), false);
			this.InputOptions.Add("highlight_distance_threshold", typeof(float), false);
			this.InputOptions.Add("interaction_distance_threshold", typeof(float), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("lock", typeof(void), false);
			this.InputOptions.Add("unlock", typeof(void), false);
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			this.InputOptions.Add("light_switch_on", typeof(void), false);
			this.InputOptions.Add("light_switch_off", typeof(void), false);
			this.InputOptions.Add("clear_user", typeof(void), false);
			this.InputOptions.Add("force_disable_highlight", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("start", typeof(void), false);
			this.OutputOptions.Add("start_fail", typeof(void), false);
			this.OutputOptions.Add("button_released", typeof(void), false);
			this.OutputOptions.Add("broadcasted_start", typeof(void), false);
			this.OutputOptions.Add("highlight", typeof(void), false);
			this.OutputOptions.Add("unhighlight", typeof(void), false);
			this.OutputOptions.Add("lock_looked_at", typeof(void), false);
			this.OutputOptions.Add("lock_interaction", typeof(void), false);
			this.OutputOptions.Add("icon_user", typeof(STNode), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("locked", typeof(void), false);
			this.OutputOptions.Add("unlocked", typeof(void), false);
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
			this.OutputOptions.Add("light_switched_on", typeof(void), false);
			this.OutputOptions.Add("light_switched_off", typeof(void), false);
			this.OutputOptions.Add("user_cleared", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
