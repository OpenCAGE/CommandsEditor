using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerSequence : STNode
	{
		private bool _m_proxy_enable_on_reset;
		[STNodeProperty("proxy_enable_on_reset", "proxy_enable_on_reset")]
		public bool m_proxy_enable_on_reset
		{
			get { return _m_proxy_enable_on_reset; }
			set { _m_proxy_enable_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_trigger_mode;
		[STNodeProperty("trigger_mode", "trigger_mode")]
		public string m_trigger_mode
		{
			get { return _m_trigger_mode; }
			set { _m_trigger_mode = value; this.Invalidate(); }
		}
		
		private float _m_random_seed;
		[STNodeProperty("random_seed", "random_seed")]
		public float m_random_seed
		{
			get { return _m_random_seed; }
			set { _m_random_seed = value; this.Invalidate(); }
		}
		
		private bool _m_use_random_intervals;
		[STNodeProperty("use_random_intervals", "use_random_intervals")]
		public bool m_use_random_intervals
		{
			get { return _m_use_random_intervals; }
			set { _m_use_random_intervals = value; this.Invalidate(); }
		}
		
		private bool _m_no_duplicates;
		[STNodeProperty("no_duplicates", "no_duplicates")]
		public bool m_no_duplicates
		{
			get { return _m_no_duplicates; }
			set { _m_no_duplicates = value; this.Invalidate(); }
		}
		
		private float _m_interval_multiplier;
		[STNodeProperty("interval_multiplier", "interval_multiplier")]
		public float m_interval_multiplier
		{
			get { return _m_interval_multiplier; }
			set { _m_interval_multiplier = value; this.Invalidate(); }
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
			
			this.Title = "TriggerSequence";
			
			this.InputOptions.Add("proxy_enable", typeof(void), false);
			this.InputOptions.Add("proxy_disable", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("duration", typeof(float), false);
			this.OutputOptions.Add("proxy_enabled", typeof(void), false);
			this.OutputOptions.Add("proxy_disabled", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
