using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_HolsterWeapon : STNode
	{
		private bool _m_should_holster;
		[STNodeProperty("should_holster", "should_holster")]
		public bool m_should_holster
		{
			get { return _m_should_holster; }
			set { _m_should_holster = value; this.Invalidate(); }
		}
		
		private bool _m_skip_anims;
		[STNodeProperty("skip_anims", "skip_anims")]
		public bool m_skip_anims
		{
			get { return _m_skip_anims; }
			set { _m_skip_anims = value; this.Invalidate(); }
		}
		
		private string _m_equipment_slot;
		[STNodeProperty("equipment_slot", "equipment_slot")]
		public string m_equipment_slot
		{
			get { return _m_equipment_slot; }
			set { _m_equipment_slot = value; this.Invalidate(); }
		}
		
		private bool _m_force_player_unarmed_on_holster;
		[STNodeProperty("force_player_unarmed_on_holster", "force_player_unarmed_on_holster")]
		public bool m_force_player_unarmed_on_holster
		{
			get { return _m_force_player_unarmed_on_holster; }
			set { _m_force_player_unarmed_on_holster = value; this.Invalidate(); }
		}
		
		private bool _m_force_drop_held_item;
		[STNodeProperty("force_drop_held_item", "force_drop_held_item")]
		public bool m_force_drop_held_item
		{
			get { return _m_force_drop_held_item; }
			set { _m_force_drop_held_item = value; this.Invalidate(); }
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
			
			this.Title = "CMD_HolsterWeapon";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("failed", typeof(void), false);
			this.OutputOptions.Add("success", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
