using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AIMED_WEAPON : STNode
	{
		private string _m_weapon_type;
		[STNodeProperty("weapon_type", "weapon_type")]
		public string m_weapon_type
		{
			get { return _m_weapon_type; }
			set { _m_weapon_type = value; this.Invalidate(); }
		}
		
		private bool _m_requires_turning_on;
		[STNodeProperty("requires_turning_on", "requires_turning_on")]
		public bool m_requires_turning_on
		{
			get { return _m_requires_turning_on; }
			set { _m_requires_turning_on = value; this.Invalidate(); }
		}
		
		private bool _m_ejectsShellsOnFiring;
		[STNodeProperty("ejectsShellsOnFiring", "ejectsShellsOnFiring")]
		public bool m_ejectsShellsOnFiring
		{
			get { return _m_ejectsShellsOnFiring; }
			set { _m_ejectsShellsOnFiring = value; this.Invalidate(); }
		}
		
		private float _m_aim_assist_scale;
		[STNodeProperty("aim_assist_scale", "aim_assist_scale")]
		public float m_aim_assist_scale
		{
			get { return _m_aim_assist_scale; }
			set { _m_aim_assist_scale = value; this.Invalidate(); }
		}
		
		private string _m_default_ammo_type;
		[STNodeProperty("default_ammo_type", "default_ammo_type")]
		public string m_default_ammo_type
		{
			get { return _m_default_ammo_type; }
			set { _m_default_ammo_type = value; this.Invalidate(); }
		}
		
		private int _m_starting_ammo;
		[STNodeProperty("starting_ammo", "starting_ammo")]
		public int m_starting_ammo
		{
			get { return _m_starting_ammo; }
			set { _m_starting_ammo = value; this.Invalidate(); }
		}
		
		private int _m_clip_size;
		[STNodeProperty("clip_size", "clip_size")]
		public int m_clip_size
		{
			get { return _m_clip_size; }
			set { _m_clip_size = value; this.Invalidate(); }
		}
		
		private float _m_consume_ammo_over_time_when_turned_on;
		[STNodeProperty("consume_ammo_over_time_when_turned_on", "consume_ammo_over_time_when_turned_on")]
		public float m_consume_ammo_over_time_when_turned_on
		{
			get { return _m_consume_ammo_over_time_when_turned_on; }
			set { _m_consume_ammo_over_time_when_turned_on = value; this.Invalidate(); }
		}
		
		private float _m_max_auto_shots_per_second;
		[STNodeProperty("max_auto_shots_per_second", "max_auto_shots_per_second")]
		public float m_max_auto_shots_per_second
		{
			get { return _m_max_auto_shots_per_second; }
			set { _m_max_auto_shots_per_second = value; this.Invalidate(); }
		}
		
		private float _m_max_manual_shots_per_second;
		[STNodeProperty("max_manual_shots_per_second", "max_manual_shots_per_second")]
		public float m_max_manual_shots_per_second
		{
			get { return _m_max_manual_shots_per_second; }
			set { _m_max_manual_shots_per_second = value; this.Invalidate(); }
		}
		
		private float _m_wind_down_time_in_seconds;
		[STNodeProperty("wind_down_time_in_seconds", "wind_down_time_in_seconds")]
		public float m_wind_down_time_in_seconds
		{
			get { return _m_wind_down_time_in_seconds; }
			set { _m_wind_down_time_in_seconds = value; this.Invalidate(); }
		}
		
		private float _m_maximum_continous_fire_time_in_seconds;
		[STNodeProperty("maximum_continous_fire_time_in_seconds", "maximum_continous_fire_time_in_seconds")]
		public float m_maximum_continous_fire_time_in_seconds
		{
			get { return _m_maximum_continous_fire_time_in_seconds; }
			set { _m_maximum_continous_fire_time_in_seconds = value; this.Invalidate(); }
		}
		
		private float _m_overheat_recharge_time_in_seconds;
		[STNodeProperty("overheat_recharge_time_in_seconds", "overheat_recharge_time_in_seconds")]
		public float m_overheat_recharge_time_in_seconds
		{
			get { return _m_overheat_recharge_time_in_seconds; }
			set { _m_overheat_recharge_time_in_seconds = value; this.Invalidate(); }
		}
		
		private bool _m_automatic_firing;
		[STNodeProperty("automatic_firing", "automatic_firing")]
		public bool m_automatic_firing
		{
			get { return _m_automatic_firing; }
			set { _m_automatic_firing = value; this.Invalidate(); }
		}
		
		private bool _m_overheats;
		[STNodeProperty("overheats", "overheats")]
		public bool m_overheats
		{
			get { return _m_overheats; }
			set { _m_overheats = value; this.Invalidate(); }
		}
		
		private bool _m_charged_firing;
		[STNodeProperty("charged_firing", "charged_firing")]
		public bool m_charged_firing
		{
			get { return _m_charged_firing; }
			set { _m_charged_firing = value; this.Invalidate(); }
		}
		
		private float _m_charging_duration;
		[STNodeProperty("charging_duration", "charging_duration")]
		public float m_charging_duration
		{
			get { return _m_charging_duration; }
			set { _m_charging_duration = value; this.Invalidate(); }
		}
		
		private float _m_min_charge_to_fire;
		[STNodeProperty("min_charge_to_fire", "min_charge_to_fire")]
		public float m_min_charge_to_fire
		{
			get { return _m_min_charge_to_fire; }
			set { _m_min_charge_to_fire = value; this.Invalidate(); }
		}
		
		private float _m_overcharge_timer;
		[STNodeProperty("overcharge_timer", "overcharge_timer")]
		public float m_overcharge_timer
		{
			get { return _m_overcharge_timer; }
			set { _m_overcharge_timer = value; this.Invalidate(); }
		}
		
		private float _m_charge_noise_start_time;
		[STNodeProperty("charge_noise_start_time", "charge_noise_start_time")]
		public float m_charge_noise_start_time
		{
			get { return _m_charge_noise_start_time; }
			set { _m_charge_noise_start_time = value; this.Invalidate(); }
		}
		
		private bool _m_reloadIndividualAmmo;
		[STNodeProperty("reloadIndividualAmmo", "reloadIndividualAmmo")]
		public bool m_reloadIndividualAmmo
		{
			get { return _m_reloadIndividualAmmo; }
			set { _m_reloadIndividualAmmo = value; this.Invalidate(); }
		}
		
		private bool _m_alwaysDoFullReloadOfClips;
		[STNodeProperty("alwaysDoFullReloadOfClips", "alwaysDoFullReloadOfClips")]
		public bool m_alwaysDoFullReloadOfClips
		{
			get { return _m_alwaysDoFullReloadOfClips; }
			set { _m_alwaysDoFullReloadOfClips = value; this.Invalidate(); }
		}
		
		private float _m_movement_accuracy_penalty_per_second;
		[STNodeProperty("movement_accuracy_penalty_per_second", "movement_accuracy_penalty_per_second")]
		public float m_movement_accuracy_penalty_per_second
		{
			get { return _m_movement_accuracy_penalty_per_second; }
			set { _m_movement_accuracy_penalty_per_second = value; this.Invalidate(); }
		}
		
		private float _m_aim_rotation_accuracy_penalty_per_second;
		[STNodeProperty("aim_rotation_accuracy_penalty_per_second", "aim_rotation_accuracy_penalty_per_second")]
		public float m_aim_rotation_accuracy_penalty_per_second
		{
			get { return _m_aim_rotation_accuracy_penalty_per_second; }
			set { _m_aim_rotation_accuracy_penalty_per_second = value; this.Invalidate(); }
		}
		
		private float _m_accuracy_penalty_per_shot;
		[STNodeProperty("accuracy_penalty_per_shot", "accuracy_penalty_per_shot")]
		public float m_accuracy_penalty_per_shot
		{
			get { return _m_accuracy_penalty_per_shot; }
			set { _m_accuracy_penalty_per_shot = value; this.Invalidate(); }
		}
		
		private float _m_accuracy_accumulated_per_second;
		[STNodeProperty("accuracy_accumulated_per_second", "accuracy_accumulated_per_second")]
		public float m_accuracy_accumulated_per_second
		{
			get { return _m_accuracy_accumulated_per_second; }
			set { _m_accuracy_accumulated_per_second = value; this.Invalidate(); }
		}
		
		private float _m_player_exposed_accuracy_penalty_per_shot;
		[STNodeProperty("player_exposed_accuracy_penalty_per_shot", "player_exposed_accuracy_penalty_per_shot")]
		public float m_player_exposed_accuracy_penalty_per_shot
		{
			get { return _m_player_exposed_accuracy_penalty_per_shot; }
			set { _m_player_exposed_accuracy_penalty_per_shot = value; this.Invalidate(); }
		}
		
		private float _m_player_exposed_accuracy_accumulated_per_second;
		[STNodeProperty("player_exposed_accuracy_accumulated_per_second", "player_exposed_accuracy_accumulated_per_second")]
		public float m_player_exposed_accuracy_accumulated_per_second
		{
			get { return _m_player_exposed_accuracy_accumulated_per_second; }
			set { _m_player_exposed_accuracy_accumulated_per_second = value; this.Invalidate(); }
		}
		
		private bool _m_recoils_on_fire;
		[STNodeProperty("recoils_on_fire", "recoils_on_fire")]
		public bool m_recoils_on_fire
		{
			get { return _m_recoils_on_fire; }
			set { _m_recoils_on_fire = value; this.Invalidate(); }
		}
		
		private bool _m_alien_threat_aware;
		[STNodeProperty("alien_threat_aware", "alien_threat_aware")]
		public bool m_alien_threat_aware
		{
			get { return _m_alien_threat_aware; }
			set { _m_alien_threat_aware = value; this.Invalidate(); }
		}
		
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
			
			this.Title = "AIMED_WEAPON";
			
			this.InputOptions.Add("spawn", typeof(void), false);
			this.InputOptions.Add("despawn", typeof(void), false);
			this.InputOptions.Add("item_animated_asset", typeof(STNode), false);
			
			this.OutputOptions.Add("on_fired_success", typeof(void), false);
			this.OutputOptions.Add("on_fired_fail", typeof(void), false);
			this.OutputOptions.Add("on_fired_fail_single", typeof(void), false);
			this.OutputOptions.Add("on_impact", typeof(void), false);
			this.OutputOptions.Add("on_reload_started", typeof(void), false);
			this.OutputOptions.Add("on_reload_another", typeof(void), false);
			this.OutputOptions.Add("on_reload_empty_clip", typeof(void), false);
			this.OutputOptions.Add("on_reload_canceled", typeof(void), false);
			this.OutputOptions.Add("on_reload_success", typeof(void), false);
			this.OutputOptions.Add("on_reload_fail", typeof(void), false);
			this.OutputOptions.Add("on_shooting_started", typeof(void), false);
			this.OutputOptions.Add("on_shooting_wind_down", typeof(void), false);
			this.OutputOptions.Add("on_shooting_finished", typeof(void), false);
			this.OutputOptions.Add("on_overheated", typeof(void), false);
			this.OutputOptions.Add("on_cooled_down", typeof(void), false);
			this.OutputOptions.Add("on_charge_complete", typeof(void), false);
			this.OutputOptions.Add("on_charge_started", typeof(void), false);
			this.OutputOptions.Add("on_charge_stopped", typeof(void), false);
			this.OutputOptions.Add("on_turned_on", typeof(void), false);
			this.OutputOptions.Add("on_turned_off", typeof(void), false);
			this.OutputOptions.Add("on_torch_on_requested", typeof(void), false);
			this.OutputOptions.Add("on_torch_off_requested", typeof(void), false);
			this.OutputOptions.Add("ammoRemainingInClip", typeof(int), false);
			this.OutputOptions.Add("ammoToFillClip", typeof(int), false);
			this.OutputOptions.Add("ammoThatWasInClip", typeof(int), false);
			this.OutputOptions.Add("charge_percentage", typeof(float), false);
			this.OutputOptions.Add("charge_noise_percentage", typeof(float), false);
			this.OutputOptions.Add("spawned", typeof(void), false);
			this.OutputOptions.Add("despawned", typeof(void), false);
			this.OutputOptions.Add("on_started_aiming", typeof(void), false);
			this.OutputOptions.Add("on_stopped_aiming", typeof(void), false);
			this.OutputOptions.Add("on_display_on", typeof(void), false);
			this.OutputOptions.Add("on_display_off", typeof(void), false);
			this.OutputOptions.Add("on_effect_on", typeof(void), false);
			this.OutputOptions.Add("on_effect_off", typeof(void), false);
			this.OutputOptions.Add("target_position", typeof(cTransform), false);
			this.OutputOptions.Add("average_target_distance", typeof(float), false);
			this.OutputOptions.Add("min_target_distance", typeof(float), false);
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
