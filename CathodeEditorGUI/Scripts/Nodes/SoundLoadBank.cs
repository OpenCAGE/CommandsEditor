#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundLoadBank : STNode
	{
		private bool _m_trigger_via_pin;
		[STNodeProperty("trigger_via_pin", "trigger_via_pin")]
		public bool m_trigger_via_pin
		{
			get { return _m_trigger_via_pin; }
			set { _m_trigger_via_pin = value; this.Invalidate(); }
		}
		
		private string _m_memory_pool;
		[STNodeProperty("memory_pool", "memory_pool")]
		public string m_memory_pool
		{
			get { return _m_memory_pool; }
			set { _m_memory_pool = value; this.Invalidate(); }
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
			
			this.Title = "SoundLoadBank";
			
			this.InputOptions.Add("sound_bank", typeof(string), false);
			this.InputOptions.Add("load_bank", typeof(void), false);
			this.InputOptions.Add("unload_bank", typeof(void), false);
			
			this.OutputOptions.Add("bank_loaded", typeof(void), false);
		}
	}
}
#endif
