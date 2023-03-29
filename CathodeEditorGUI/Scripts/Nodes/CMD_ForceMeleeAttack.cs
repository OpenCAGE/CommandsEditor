#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_ForceMeleeAttack : STNode
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
			
			this.Title = "CMD_ForceMeleeAttack";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			
			this.OutputOptions.Add("start_applied", typeof(void), false);
		}
	}
}
#endif
