#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Character : STNode
	{
		private bool _m_spawn_on_reset;
		[STNodeProperty("spawn_on_reset", "spawn_on_reset")]
		public bool m_spawn_on_reset
		{
			get { return _m_spawn_on_reset; }
			set { _m_spawn_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_show_on_reset;
		[STNodeProperty("show_on_reset", "show_on_reset")]
		public bool m_show_on_reset
		{
			get { return _m_show_on_reset; }
			set { _m_show_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_PopToNavMesh;
		[STNodeProperty("PopToNavMesh", "PopToNavMesh")]
		public bool m_PopToNavMesh
		{
			get { return _m_PopToNavMesh; }
			set { _m_PopToNavMesh = value; this.Invalidate(); }
		}
		
		private bool _m_is_cinematic;
		[STNodeProperty("is_cinematic", "is_cinematic")]
		public bool m_is_cinematic
		{
			get { return _m_is_cinematic; }
			set { _m_is_cinematic = value; this.Invalidate(); }
		}
		
		private bool _m_disable_dead_container;
		[STNodeProperty("disable_dead_container", "disable_dead_container")]
		public bool m_disable_dead_container
		{
			get { return _m_disable_dead_container; }
			set { _m_disable_dead_container = value; this.Invalidate(); }
		}
		
		private bool _m_allow_container_without_death;
		[STNodeProperty("allow_container_without_death", "allow_container_without_death")]
		public bool m_allow_container_without_death
		{
			get { return _m_allow_container_without_death; }
			set { _m_allow_container_without_death = value; this.Invalidate(); }
		}
		
		private string _m_container_interaction_text;
		[STNodeProperty("container_interaction_text", "container_interaction_text")]
		public string m_container_interaction_text
		{
			get { return _m_container_interaction_text; }
			set { _m_container_interaction_text = value; this.Invalidate(); }
		}
		
		private string _m_anim_set;
		[STNodeProperty("anim_set", "anim_set")]
		public string m_anim_set
		{
			get { return _m_anim_set; }
			set { _m_anim_set = value; this.Invalidate(); }
		}
		
		private string _m_anim_tree_set;
		[STNodeProperty("anim_tree_set", "anim_tree_set")]
		public string m_anim_tree_set
		{
			get { return _m_anim_tree_set; }
			set { _m_anim_tree_set = value; this.Invalidate(); }
		}
		
		private string _m_attribute_set;
		[STNodeProperty("attribute_set", "attribute_set")]
		public string m_attribute_set
		{
			get { return _m_attribute_set; }
			set { _m_attribute_set = value; this.Invalidate(); }
		}
		
		private bool _m_is_player;
		[STNodeProperty("is_player", "is_player")]
		public bool m_is_player
		{
			get { return _m_is_player; }
			set { _m_is_player = value; this.Invalidate(); }
		}
		
		private bool _m_is_backstage;
		[STNodeProperty("is_backstage", "is_backstage")]
		public bool m_is_backstage
		{
			get { return _m_is_backstage; }
			set { _m_is_backstage = value; this.Invalidate(); }
		}
		
		private bool _m_force_backstage_on_respawn;
		[STNodeProperty("force_backstage_on_respawn", "force_backstage_on_respawn")]
		public bool m_force_backstage_on_respawn
		{
			get { return _m_force_backstage_on_respawn; }
			set { _m_force_backstage_on_respawn = value; this.Invalidate(); }
		}
		
		private string _m_character_class;
		[STNodeProperty("character_class", "character_class")]
		public string m_character_class
		{
			get { return _m_character_class; }
			set { _m_character_class = value; this.Invalidate(); }
		}
		
		private string _m_alliance_group;
		[STNodeProperty("alliance_group", "alliance_group")]
		public string m_alliance_group
		{
			get { return _m_alliance_group; }
			set { _m_alliance_group = value; this.Invalidate(); }
		}
		
		private string _m_dialogue_voice;
		[STNodeProperty("dialogue_voice", "dialogue_voice")]
		public string m_dialogue_voice
		{
			get { return _m_dialogue_voice; }
			set { _m_dialogue_voice = value; this.Invalidate(); }
		}
		
		private int _m_spawn_id;
		[STNodeProperty("spawn_id", "spawn_id")]
		public int m_spawn_id
		{
			get { return _m_spawn_id; }
			set { _m_spawn_id = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
		}
		
		private string _m_display_model;
		[STNodeProperty("display_model", "display_model")]
		public string m_display_model
		{
			get { return _m_display_model; }
			set { _m_display_model = value; this.Invalidate(); }
		}
		
		private string _m_reference_skeleton;
		[STNodeProperty("reference_skeleton", "reference_skeleton")]
		public string m_reference_skeleton
		{
			get { return _m_reference_skeleton; }
			set { _m_reference_skeleton = value; this.Invalidate(); }
		}
		
		private string _m_torso_sound;
		[STNodeProperty("torso_sound", "torso_sound")]
		public string m_torso_sound
		{
			get { return _m_torso_sound; }
			set { _m_torso_sound = value; this.Invalidate(); }
		}
		
		private string _m_leg_sound;
		[STNodeProperty("leg_sound", "leg_sound")]
		public string m_leg_sound
		{
			get { return _m_leg_sound; }
			set { _m_leg_sound = value; this.Invalidate(); }
		}
		
		private string _m_footwear_sound;
		[STNodeProperty("footwear_sound", "footwear_sound")]
		public string m_footwear_sound
		{
			get { return _m_footwear_sound; }
			set { _m_footwear_sound = value; this.Invalidate(); }
		}
		
		private string _m_custom_character_type;
		[STNodeProperty("custom_character_type", "custom_character_type")]
		public string m_custom_character_type
		{
			get { return _m_custom_character_type; }
			set { _m_custom_character_type = value; this.Invalidate(); }
		}
		
		private string _m_custom_character_accessory_override;
		[STNodeProperty("custom_character_accessory_override", "custom_character_accessory_override")]
		public string m_custom_character_accessory_override
		{
			get { return _m_custom_character_accessory_override; }
			set { _m_custom_character_accessory_override = value; this.Invalidate(); }
		}
		
		private string _m_custom_character_population_type;
		[STNodeProperty("custom_character_population_type", "custom_character_population_type")]
		public string m_custom_character_population_type
		{
			get { return _m_custom_character_population_type; }
			set { _m_custom_character_population_type = value; this.Invalidate(); }
		}
		
		private string _m_named_custom_character;
		[STNodeProperty("named_custom_character", "named_custom_character")]
		public string m_named_custom_character
		{
			get { return _m_named_custom_character; }
			set { _m_named_custom_character = value; this.Invalidate(); }
		}
		
		private string _m_named_custom_character_assets_set;
		[STNodeProperty("named_custom_character_assets_set", "named_custom_character_assets_set")]
		public string m_named_custom_character_assets_set
		{
			get { return _m_named_custom_character_assets_set; }
			set { _m_named_custom_character_assets_set = value; this.Invalidate(); }
		}
		
		private float _m_gcip_distribution_bias;
		[STNodeProperty("gcip_distribution_bias", "gcip_distribution_bias")]
		public float m_gcip_distribution_bias
		{
			get { return _m_gcip_distribution_bias; }
			set { _m_gcip_distribution_bias = value; this.Invalidate(); }
		}
		
		private string _m_inventory_set;
		[STNodeProperty("inventory_set", "inventory_set")]
		public string m_inventory_set
		{
			get { return _m_inventory_set; }
			set { _m_inventory_set = value; this.Invalidate(); }
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
			
			this.Title = "Character";
			
			this.InputOptions.Add("contents_of_dead_container", typeof(string), false);
			this.InputOptions.Add("spawn", typeof(void), false);
			this.InputOptions.Add("despawn", typeof(void), false);
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			
			this.OutputOptions.Add("finished_spawning", typeof(void), false);
			this.OutputOptions.Add("finished_respawning", typeof(void), false);
			this.OutputOptions.Add("dead_container_take_slot", typeof(void), false);
			this.OutputOptions.Add("dead_container_emptied", typeof(void), false);
			this.OutputOptions.Add("on_ragdoll_impact", typeof(void), false);
			this.OutputOptions.Add("on_footstep", typeof(void), false);
			this.OutputOptions.Add("on_despawn_requested", typeof(void), false);
			this.OutputOptions.Add("spawned", typeof(void), false);
			this.OutputOptions.Add("despawned", typeof(void), false);
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
		}
	}
}
#endif
