using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Checkpoint : STNode
	{
		private bool _m_is_first_checkpoint;
		[STNodeProperty("is_first_checkpoint", "is_first_checkpoint")]
		public bool m_is_first_checkpoint
		{
			get { return _m_is_first_checkpoint; }
			set { _m_is_first_checkpoint = value; this.Invalidate(); }
		}
		
		private bool _m_is_first_autorun_checkpoint;
		[STNodeProperty("is_first_autorun_checkpoint", "is_first_autorun_checkpoint")]
		public bool m_is_first_autorun_checkpoint
		{
			get { return _m_is_first_autorun_checkpoint; }
			set { _m_is_first_autorun_checkpoint = value; this.Invalidate(); }
		}
		
		private string _m_section;
		[STNodeProperty("section", "section")]
		public string m_section
		{
			get { return _m_section; }
			set { _m_section = value; this.Invalidate(); }
		}
		
		private float _m_mission_number;
		[STNodeProperty("mission_number", "mission_number")]
		public float m_mission_number
		{
			get { return _m_mission_number; }
			set { _m_mission_number = value; this.Invalidate(); }
		}
		
		private string _m_checkpoint_type;
		[STNodeProperty("checkpoint_type", "checkpoint_type")]
		public string m_checkpoint_type
		{
			get { return _m_checkpoint_type; }
			set { _m_checkpoint_type = value; this.Invalidate(); }
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
			
			this.Title = "Checkpoint";
			
			this.InputOptions.Add("player_spawn_position", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_checkpoint", typeof(void), false);
			this.OutputOptions.Add("on_captured", typeof(void), false);
			this.OutputOptions.Add("on_saved", typeof(void), false);
			this.OutputOptions.Add("finished_saving", typeof(void), false);
			this.OutputOptions.Add("finished_loading", typeof(void), false);
			this.OutputOptions.Add("cancelled_saving", typeof(void), false);
			this.OutputOptions.Add("finished_saving_to_hdd", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
