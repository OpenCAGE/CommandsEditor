using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_LaunchMeleeAttack : STNode
	{
		private string _m_melee_attack_type;
		[STNodeProperty("melee_attack_type", "melee_attack_type")]
		public string m_melee_attack_type
		{
			get { return _m_melee_attack_type; }
			set { _m_melee_attack_type = value; this.Invalidate(); }
		}
		
		private string _m_enemy_type;
		[STNodeProperty("enemy_type", "enemy_type")]
		public string m_enemy_type
		{
			get { return _m_enemy_type; }
			set { _m_enemy_type = value; this.Invalidate(); }
		}
		
		private int _m_melee_attack_index;
		[STNodeProperty("melee_attack_index", "melee_attack_index")]
		public int m_melee_attack_index
		{
			get { return _m_melee_attack_index; }
			set { _m_melee_attack_index = value; this.Invalidate(); }
		}
		
		private bool _m_skip_convergence;
		[STNodeProperty("skip_convergence", "skip_convergence")]
		public bool m_skip_convergence
		{
			get { return _m_skip_convergence; }
			set { _m_skip_convergence = value; this.Invalidate(); }
		}
		
		private bool _m_override_all_ai;
		[STNodeProperty("override_all_ai", "override_all_ai")]
		public bool m_override_all_ai
		{
			get { return _m_override_all_ai; }
			set { _m_override_all_ai = value; this.Invalidate(); }
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
			
			this.Title = "CMD_LaunchMeleeAttack";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("finished", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("command_started", typeof(void), false);
		}
	}
}
