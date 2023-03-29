#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AIMED_ITEM : STNode
	{
		private float _m_fixed_target_distance_for_local_player;
		[STNodeProperty("fixed_target_distance_for_local_player", "fixed_target_distance_for_local_player")]
		public float m_fixed_target_distance_for_local_player
		{
			get { return _m_fixed_target_distance_for_local_player; }
			set { _m_fixed_target_distance_for_local_player = value; this.Invalidate(); }
		}
		
		private bool _m_spawn_on_reset;
		[STNodeProperty("spawn_on_reset", "spawn_on_reset")]
		public bool m_spawn_on_reset
		{
			get { return _m_spawn_on_reset; }
			set { _m_spawn_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_character_animation_context;
		[STNodeProperty("character_animation_context", "character_animation_context")]
		public string m_character_animation_context
		{
			get { return _m_character_animation_context; }
			set { _m_character_animation_context = value; this.Invalidate(); }
		}
		
		private string _m_character_activate_animation_context;
		[STNodeProperty("character_activate_animation_context", "character_activate_animation_context")]
		public string m_character_activate_animation_context
		{
			get { return _m_character_activate_animation_context; }
			set { _m_character_activate_animation_context = value; this.Invalidate(); }
		}
		
		private bool _m_left_handed;
		[STNodeProperty("left_handed", "left_handed")]
		public bool m_left_handed
		{
			get { return _m_left_handed; }
			set { _m_left_handed = value; this.Invalidate(); }
		}
		
		private string _m_inventory_name;
		[STNodeProperty("inventory_name", "inventory_name")]
		public string m_inventory_name
		{
			get { return _m_inventory_name; }
			set { _m_inventory_name = value; this.Invalidate(); }
		}
		
		private string _m_equipment_slot;
		[STNodeProperty("equipment_slot", "equipment_slot")]
		public string m_equipment_slot
		{
			get { return _m_equipment_slot; }
			set { _m_equipment_slot = value; this.Invalidate(); }
		}
		
		private bool _m_holsters_on_owner;
		[STNodeProperty("holsters_on_owner", "holsters_on_owner")]
		public bool m_holsters_on_owner
		{
			get { return _m_holsters_on_owner; }
			set { _m_holsters_on_owner = value; this.Invalidate(); }
		}
		
		private string _m_holster_node;
		[STNodeProperty("holster_node", "holster_node")]
		public string m_holster_node
		{
			get { return _m_holster_node; }
			set { _m_holster_node = value; this.Invalidate(); }
		}
		
		private float _m_holster_scale;
		[STNodeProperty("holster_scale", "holster_scale")]
		public float m_holster_scale
		{
			get { return _m_holster_scale; }
			set { _m_holster_scale = value; this.Invalidate(); }
		}
		
		private string _m_weapon_handedness;
		[STNodeProperty("weapon_handedness", "weapon_handedness")]
		public string m_weapon_handedness
		{
			get { return _m_weapon_handedness; }
			set { _m_weapon_handedness = value; this.Invalidate(); }
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
			
			this.Title = "AIMED_ITEM";
			
			this.InputOptions.Add("spawn", typeof(void), false);
			this.InputOptions.Add("despawn", typeof(void), false);
			this.InputOptions.Add("item_animated_asset", typeof(STNode), false);
			
			this.OutputOptions.Add("on_started_aiming", typeof(void), false);
			this.OutputOptions.Add("on_stopped_aiming", typeof(void), false);
			this.OutputOptions.Add("on_display_on", typeof(void), false);
			this.OutputOptions.Add("on_display_off", typeof(void), false);
			this.OutputOptions.Add("on_effect_on", typeof(void), false);
			this.OutputOptions.Add("on_effect_off", typeof(void), false);
			this.OutputOptions.Add("target_position", typeof(cTransform), false);
			this.OutputOptions.Add("average_target_distance", typeof(float), false);
			this.OutputOptions.Add("min_target_distance", typeof(float), false);
			this.OutputOptions.Add("spawned", typeof(void), false);
			this.OutputOptions.Add("despawned", typeof(void), false);
			this.OutputOptions.Add("finished_spawning", typeof(void), false);
			this.OutputOptions.Add("equipped", typeof(void), false);
			this.OutputOptions.Add("unequipped", typeof(void), false);
			this.OutputOptions.Add("on_pickup", typeof(void), false);
			this.OutputOptions.Add("on_discard", typeof(void), false);
			this.OutputOptions.Add("on_melee_impact", typeof(void), false);
			this.OutputOptions.Add("on_used_basic_function", typeof(void), false);
			this.OutputOptions.Add("owner", typeof(STNode), false);
			this.OutputOptions.Add("has_owner", typeof(bool), false);
		}
	}
}
#endif
