using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

//TODO: Flesh this out and then bring it over to CathodeLib

namespace CommandsEditor
{
    #region I think these are right?
    public enum CHECKPOINT_TYPE
    {
        CAMPAIGN,
        MANUAL, //confirmed via savestation
        CAMPAIGN_MISSION,
        MISSION_TEMP_STATE = -1, //perhaps?
    }

    public enum LIGHT_ANIM
    {
        UNIFORM = -1, // this has a val of -1 somewhere so perhaps?
        PULSATE = 1,
        OSCILLATE,
        FLICKER,
        FLUCTUATE,
        FLICKER_OFF,
        SPARKING,
        BLINK,
    }
    #endregion

    public enum AGGRESSION_GAIN
    {
        LOW_AGGRESSION_GAIN,
        MED_AGGRESSION_GAIN,
        HIGH_AGGRESSION_GAIN,
    }

    public enum ALERTNESS_STATE
    {
        IGNORE_PLAYER,
        ALERT,
        AGGRESSIVE,
    }

    public enum ALIEN_CONFIGURATION_TYPE
    {
        DEFAULT,
        MILD,
        MODERATE,
        INTENSE,
        BACKSTAGEHOLD,
        MODERATELY_INTENSE,
        BACKSTAGEALERT,
        BACSTAGEHOLD_CLOSE,
        BACKSTAGEHOLD_VCLOSE,
    }

    public enum ALIEN_DEVELOPMENT_MANAGER_ABILITIES
    {
        NONE,
        THREAT_AWARE,
        LIKES_TO_CLOSE_VIA_BACKSTAGE,
        WILL_KILLTRAP,
        WILL_FLANK,
        WILL_FLANK_FROM_THREAT_AWARE,
        WILL_AMBUSH,
        SEARCH_LOCKERS,
        SEARCH_UNDER_STUFF,
    }

    public enum ALIEN_DEVELOPMENT_MANAGER_ABILITY_MASKS
    {
        NONE,
        THREAT_AWARE,
        LIKES_TO_CLOSE_VIA_BACKSTAGE,
        WILL_KILLTRAP,
        WILL_FLANK,
        WILL_FLANK_FROM_THREAT_AWARE,
        WILL_AMBUSH,
        SEARCH_LOCKERS,
        SEARCH_UNDER_STUFF,
    }

    public enum ALIEN_DEVELOPMENT_MANAGER_STAGES
    {
        NAIVE,
        THREAT_AWARE,
        GETTING_SNEAKY,
        REALLY_SNEAKY,
    }

    public enum ALLIANCE_GROUP
    {
        NEUTRAL,
        PLAYER,
        PLAYER_ALLY,
        ALIEN,
        ANDROID,
        CIVILIAN,
        SECURITY,
        DEAD,
        DEAD_MAN_WALKING,
    }

    public enum ALLIANCE_STANCE
    {
        FRIEND,
        NEUTRAL,
        ENEMY,
    }

    public enum AMBUSH_TYPE
    {
        KILLTRAP,
        FRONTSTAGE_AMBUSH,
    }

    public enum AMMO_TYPE
    {
        PISTOL_NORMAL,
        PISTOL_DUM_DUM,
        SMG_NORMAL,
        SMG_DUM_DUM,
        SHOTGUN_NORMAL,
        SHOTGUN_SLUG,
        FLAMETHROWER_NORMAL,
        FLAMETHROWER_AERATED,
        FLAMETHROWER_HIGH_DAMAGE,
        SHOTGUN_INCENDIARY,
        PISTOL_NORMAL_NPC,
        SHOTGUN_NORMAL_NPC,
        GRENADE_HE,
        GRENADE_FIRE,
        CATALYST_HE_SMALL,
        CATALYST_HE_LARGE,
        CATALYST_FIRE_SMALL,
        CATALYST_FIRE_LARGE,
        ACID_BURST_SMALL,
        ACID_BURST_LARGE,
        EMP_BURST_SMALL,
        EMP_BURST_LARGE,
        IMPACT,
        GRENADE_STUN,
        GRENADE_SMOKE,
        PISTOL_TAZER,
        MELEE_CROW_AXE,
        BOLTGUN_NORMAL,
        PUSH,
        CATTLEPROD_POWERPACK,
        EMP_BURST_LARGE_TIER2,
        EMP_BURST_LARGE_TIER3,
        GRENADE_FIRE_TIER2,
        GRENADE_FIRE_TIER3,
        GRENADE_HE_TIER2,
        GRENADE_HE_TIER3,
        GRENADE_STUN_TIER2,
        GRENADE_STUN_TIER3,
        ENVIRONMENT_FLAME,
    }

    public enum ANIM_CALLBACK_ENUM
    {
        NONE,
        STUN_DAMAGE_CALLBACK,
    }

    public enum ANIM_TRACK_TYPE
    {
        T_FLOAT,
        T_FLOAT3,
        T_POSITION,
        T_STRING,
        T_GUID,
        T_MASTERING,
    }

    public enum ANIM_TREE_ENUM
    {
        NONE,
        STUN_DAMAGE_TREE,
    }

    public enum ANIMATION_EFFECT_TYPE
    {
        STUMBLE,
    }

    public enum AREA_SWEEP_TYPE
    {
        IN_AND_OUT_BETWEEN_TARGET_AND_POSITION,
        FIXED_RADIUS_AROUND_POSITION,
    }

    public enum AREA_SWEEP_TYPE_CODE
    {
        IN_AND_OUT_BETWEEN_TARGET_AND_POSITION,
        FIXED_RADIUS_AROUND_POSITION,
        AROUND_TARGET,
    }

    public enum AUTODETECT
    {
        AUTO,
        FORCE_ON,
        FORCE_OFF,
    }

    public enum BEHAVIOUR_MOOD_SET
    {
        NEUTRAL,
        THREAT_ESCALATION_AGGRESSIVE,
        THREAT_ESCALATION_PANICKED,
        AGGRESSIVE,
        PANICKED,
        SUSPICIOUS,
    }

    public enum BEHAVIOUR_TREE_FLAGS
    {
        DO_ASSAULT_ATTACK_CHECKS,
        IS_IN_VENT,
        IS_SITTING,
        PLAYER_HIDING,
        ATTACK_HIDING_PLAYER,
        ALIEN_ALWAYS_KNOWS_WHEN_IN_VENT,
        IS_CORPSE_TRAP_ON_START,
        IS_BACKSTAGE_STALK_LOCKED,
        PLAYER_WON_HIDING_QTE,
        ANDROID_IS_INERT,
        ANDROID_IS_SHOWROOM_DUMMY,
        NEVER_AGGRESSIVE,
        MUTE_DYNAMIC_DIALOGUE,
        BLOCK_AMBUSH_AND_KILLTRAPS,
        PREVENT_GRAPPLES,
        PREVENT_ALL_ATTACKS,
        USE_AIMED_STANCE_FOR_IDLE_JOBS,
        USE_AIMED_LOW_STANCE_FOR_IDLE_JOBS,
        IGNORE_PLAYER_IN_VENT_BEHAVIOUR,
        IS_ON_LADDER,
    }

    public enum BLEND_MODE
    {
        BLEND,
        ADDITIVE,
    }

    public enum BLUEPRINT_LEVEL
    {
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
    }

    public enum CAMERA_PATH_CLASS
    {
        GENERIC,
        POSITION,
        TARGET,
        REFERENCE,
    }

    public enum CAMERA_PATH_TYPE
    {
        LINEAR,
        BEZIER,
    }

    public enum CHARACTER_BB_ENTRY_TYPE
    {
        CBB_HAVE_CURRENT_TARGET,
        CBB_MOST_RECENT_TARGET_SENSE_IS_ACTIVATED,
        CBB_MOST_RECENT_TARGET_SENSE_HAS_BEEN_ACTIVATED,
        CBB_SENSE_ABOVE_FIRST,
        CBB_SENSE_ABOVE_LAST,
        CBB_SENSE_TRIGGERED_FIRST,
        CBB_SENSE_TRIGGERED_LAST,
        CBB_SENSE_BEEN_ABOVE_FIRST,
        CBB_SENSE_BEEN_ABOVE_LAST,
        CBB_SENSE_LAST_TIME_FIRST,
        CBB_SENSE_LAST_TIME_LAST,
        CBB_TARGET_CLOSEST_THRESHOLD,
        CBB_CAN_MOVE_TO_TARGET,
        CBB_HAVE_NEXT_TARGET,
        CBB_AGENT_MOTIVATION,
        CBB_GAUGE_AMOUNT_ABOVE_RETREAT,
        CBB_GAUGE_AMOUNT_ABOVE_FIRST,
        CBB_GAUGE_AMOUNT_ABOVE_MELEE_DEFENSE,
        CBB_GAUGE_AMOUNT_ABOVE_STUN_DAMAGE,
        CBB_GAUGE_AMOUNT_ABOVE_LAST,
        CBB_HAS_MELEE_ATTACK_AVAILABLE,
        CBB_ALLOWED_TO_ATTACK_TARGET,
        CBB_ALERTNESS_STATE,
        CBB_HAS_A_WEAPON,
        CBB_WEAPON_IS_EQUIPPED,
        CBB_WEAPON_NEEDS_RELOADING,
        CBB_LOGIC_CHARACTER_SUSPECT_TARGET_RESPONSE_TIMER,
        CBB_LOGIC_CHARACTER_TIMER_FIRST,
        CBB_LOGIC_CHARACTER_THREAT_AWARE_TIMEOUT_TIMER,
        CBB_LOGIC_CHARACTER_THREAT_AWARE_DURATION_TIMER,
        CBB_LOGIC_CHARACTER_SEARCH_TIMEOUT_TIMER,
        CBB_LOGIC_CHARACTER_BACKSTAGE_STALK_TIMEOUT_TIMER,
        CBB_LOGIC_CHARACTER_AMBUSH_TIMEOUT_TIMER,
        CBB_LOGIC_CHARACTER_BACKSTAGE_STALK_PICK_KILLTRAP_TIMER,
        CBB_LOGIC_CHARACTER_ATTACK_BAN_TIMER,
        CBB_LOGIC_CHARACTER_MELEE_ATTACK_BAN_TIMER,
        CBB_LOGIC_CHARACTER_VENT_BAN_TIMER,
        CBB_LOGIC_CHARACTER_NPC_STAY_IN_COVER_TIMER,
        CBB_LOGIC_CHARACTER_NPC_JUST_LEFT_COMBAT_TIMER,
        CBB_LOGIC_CHARACTER_ATTACK_KEEP_CHASING_TIMER,
        CBB_LOGIC_CHARACTER_DELAY_RETURN_TO_SPAWN_POINT_TIMER,
        CBB_LOGIC_CHARACTER_TARGET_IN_CRAWLSPACE_TIMER,
        CBB_LOGIC_CHARACTER_DURATION_SINCE_SEARCH_TIMER,
        CBB_LOGIC_CHARACTER_HEIGHTENED_SENSES_TIMER,
        CBB_LOGIC_CHARACTER_FLANKED_VENT_ATTACK_TIMER,
        CBB_LOGIC_CHARACTER_THREAT_AWARE_VISUAL_RETENTION_TIMER,
        CBB_LOGIC_CHARACTER_RESPONSE_TO_BACKSTAGE_ALIEN_TIMEOUT_TIMER,
        CBB_LOGIC_CHARACTER_VENT_ATTRACT_TIMER,
        CBB_LOGIC_CHARACTER_SEEN_PLAYER_AIM_WEAPON_TIMER,
        CBB_LOGIC_CHARACTER_SEARCH_BAN_TIMER,
        CBB_LOGIC_CHARACTER_OBSERVE_TARGET_TIMER,
        CBB_LOGIC_CHARACTER_REPEATED_PATHFIND_FAILUREST_TIMER,
        CBB_LOGIC_CHARACTER_TIMER_LAST,
        CBB_AFFECTED_BY_TARGETS_FLAME_THROWER,
    }

    public enum CHARACTER_FOLEY_SOUND
    {
        LEATHER,
        HEAVY_JACKET,
        HEAVY_OVERALLS,
        SHIRT,
        SUIT_JACKET,
        SUIT_TROUSERS,
        JEANS,
        BOOTS,
        FLATS,
        TRAINERS,
    }

    public enum CLIPPING_PLANES_PRESETS
    {
        MACRO,
        CLOSE,
        MID,
        WIDE,
        ULTRA,
    }

    public enum COLLISION_TYPE
    {
        LINE_OF_SIGHT_COL,
        CAMERA_COL,
        STANDARD_COL,
        UI,
        PLAYER_COL,
        PHYSICS_COL,
        TRANSPARENT_COL,
        DETECTABLE,
    }

    public enum COMBAT_BEHAVIOUR
    {
        ALLOW_ATTACK,
        ALLOW_AIM,
        CINEMATICS_ONLY,
        ALLOW_IDLE_JOBS,
        ALLOW_USE_COVER,
        MUTUAL_MELEE,
        VENT_MELEE,
        ALLOW_SYSTEMATIC_SEARCH,
        ALLOW_AREA_SWEEP,
        ALLOW_WITHDRAW_TO_BACKSTAGE,
        ALLOW_SUSPECT_TARGET_RESPONSE,
        ALLOW_THREAT_AWARE,
        ALLOW_BREAKOUT_WHEN_SHOT,
        ALLOW_BACKSTAGE_AMBUSH,
        ALLOW_ASSAULT,
        ALLOW_MELEE,
        ALLOW_RETREAT,
        ALLOW_CLOSE_ON_TARGET,
        ALLOW_REACT_TO_WEAPON_FIRE,
        ALLOW_AGGRESSION_ESCALATION,
        RESET_ALL_TO_DEFAULTS,
        ALLOW_ADVANCE_ON_TARGET,
        ALLOW_ALIEN_AMBUSHES,
        ALLOW_PANIC,
        ALLOW_SUSPICIOUS_ITEM,
        ALLOW_RESPONSE_TO_BACKSTAGE_ALIEN,
        ALLOW_ESCALATION_PREVENTS_SEARCH,
        ALLOW_OBSERVE_TARGET,
    }

    public enum CROUCH_MODE
    {
        FORCE_CROUCH,
        FORCE_UNCROUCH,
        ALLOW_UNCROUCH,
    }

    public enum CUSTOM_CHARACTER_ACCESSORY_OVERRIDE
    {
        ACCESSORY_OVERRIDE_NONE,
        ACCESSORY_OVERRIDE_01,
        ACCESSORY_OVERRIDE_02,
        ACCESSORY_OVERRIDE_03,
        ACCESSORY_OVERRIDE_04,
        ACCESSORY_OVERRIDE_05,
        ACCESSORY_OVERRIDE_06,
        ACCESSORY_OVERRIDE_07,
        ACCESSORY_OVERRIDE_08,
        ACCESSORY_OVERRIDE_09,
        ACCESSORY_OVERRIDE_10,
    }

    public enum CUSTOM_CHARACTER_ASSETS
    {
        ASSETSET_01,
        ASSETSET_02,
        ASSETSET_03,
        ASSETSET_04,
        ASSETSET_05,
        ASSETSET_06,
        ASSETSET_07,
        ASSETSET_08,
        ASSETSET_09,
        ASSETSET_10,
    }

    public enum CUSTOM_CHARACTER_BUILD
    {
        STANDARD,
        HEAVY,
    }

    public enum CUSTOM_CHARACTER_COMPONENT
    {
        TORSO,
        LEGS,
        SHOES,
        HEAD,
        ARMS,
        COLLISION,
    }

    public enum CUSTOM_CHARACTER_ETHNICITY
    {
        AFRICAN,
        CAUCASIAN,
        ASIAN,
    }

    public enum CUSTOM_CHARACTER_GENDER
    {
        MALE,
        FEMALE,
    }

    public enum CUSTOM_CHARACTER_MODEL
    {
        NPC,
        ANDROID,
        CORPSE,
    }

    public enum CUSTOM_CHARACTER_POPULATION
    {
        POPULATION_01,
        POPULATION_02,
        POPULATION_03,
        POPULATION_04,
        POPULATION_05,
        POPULATION_06,
        POPULATION_07,
        POPULATION_08,
        POPULATION_09,
        POPULATION_10,
    }

    public enum CUSTOM_CHARACTER_SLEEVETYPE
    {
        LONG,
        MEDIUM,
        SHORT,
    }

    public enum CUSTOM_CHARACTER_TYPE
    {
        NONE,
        RANDOM,
        NAMED,
    }

    public enum DAMAGE_EFFECT_TYPE_FLAGS
    {
        NONE,
        INCENDIARY,
        STUN,
        BLIND,
        EMP,
        ACID,
        GAS,
        IMPACT,
        COLLISION,
        SLIDING,
        MELEE,
        ALL,
        INVALID_FLAG_MASK,
        PROJECTILE,
        ANY_IMPACTING_WEAPON,
    }

    public enum DAMAGE_EFFECTS
    {
        INCENDIARY,
        EMP,
        ACID,
        GAS,
        IMPACT,
        BALLISTIC,
        COLLISION,
        SLIDING,
        MELEE,
        BALLISTIC_OR_MELEE,
        ANY,
    }

    public enum DEATH_STYLE
    {
        PDS_DROP_DEAD,
        PDS_SKIP_ALL_ANIMS,
        PDS_SKIP_ALL_ANIMS_NO_RAGDOLL,
    }

    public enum DEVICE_INTERACTION_MODE
    {
        NONE,
        HACKING,
        HACKING_REACTION,
        REWIRE,
        CONTAINER,
        KEYPAD,
        INTERACTIVE_TERMINAL,
        CUTTING_PANEL,
    }

    public enum DIALOGUE_ACTION
    {
        Suspicious_Warning,
        Suspicious_Warning_Fail,
        Missing_Buddy,
        Search_Starts,
        Search_Loop,
        Search_Fail,
        Detected_Enemy,
        Interrogative,
        Warning,
        Last_Chance,
        Stand_Down,
        Use_Cover,
        No_Cover,
        Shoot_From_Cover,
        Cover_Invalidated,
        Alamo,
        Panic,
        Attack,
        Hit_By_Weapon,
        Blocks_Blow,
        Final_Hit,
        Ally_Death,
        Incoming_IED,
        Alert_Squad,
        Idle_Chatter,
        Enter_Grapple,
        Grab_From_Cover,
        Player_Observed,
        Suspicious_Item_Initial,
        Suspicious_Item_CloseTo,
        Melee,
        Advance,
        Death,
        Alien_Heard_Backstage,
    }

    public enum DIALOGUE_ARGUMENT
    {
        Character_Voice,
        Dialogue_Progression,
        Target_Character,
        NPC_Group_Status,
        NPC_Dialogue_Mode,
        Action,
        Seen_Target,
        Call_Response,
        Android_Escalation,
        Suspicious_Item_Type,
        Last_Activated_Sense,
        Last_Hit_By_Weapon,
        Injured_State,
    }

    public enum DIALOGUE_NPC_COMBAT_MODE
    {
        Area_Sweep,
        Attack,
        Idle,
        Suspect_Response,
        Systematic_Search,
        Target_Lost,
    }

    public enum DIALOGUE_NPC_CONTEXT
    {
        Alamo,
        Been_In_Combat,
        Seen_Target,
        Unknown_Target,
    }

    public enum DIALOGUE_NPC_EVENT
    {
        SUSPICIOUS_WARNING,
        SUSPICIOUS_WARNING_FAIL,
        MISSING_BUDDY,
        SEARCH_STARTED,
        SEARCH_LOOP,
        SEARCH_COMPLETED,
        DETECTED_ENEMY,
        INTERROGATIVE,
        WARNING,
        LAST_CHANCE,
        STAND_DOWN,
        GO_TO_COVER,
        NO_COVER,
        SHOOT_FROM_COVER,
        COVER_BROKEN,
        ALAMO,
        PANIC,
        ATTACK,
        HIT_BY_WEAPON,
        BLOCK,
        FINAL_HIT,
        ALLY_DEATH,
        INCOMING_IED,
        ALERT_SQUAD,
        IDLE_PASSIVE,
        IDLE_AGGRESSIVE,
        ENTER_GRAPPLE,
        GRAPPLE_FROM_COVER,
        PLAYER_OBSERVED,
        SUSPICIOUS_ITEM_INITIAL,
        SUSPICIOUS_ITEM_CLOSE,
        MELEE,
        ADVANCE,
        MY_DEATH,
        ALIEN_HEARD_BACKSTAGE,
        ALIEN_SIGHTED,
        ALIEN_ATTACKING_ME,
    }

    public enum DIALOGUE_VOICE_ACTOR
    {
        AUTO,
        CV1,
        CV2,
        CV3,
        CV4,
        CV5,
        CV6,
        RT1,
        RT2,
        RT3,
        RT4,
        AN1,
        AN2,
        AN3,
        ANH,
        TOTAL,
        FIRST_CIVILIAN_MALE_VOICE,
        LAST_CIVILIAN_MALE_VOICE,
        FIRST_CIVILIAN_FEMALE_VOICE,
        LAST_CIVILIAN_FEMALE_VOICE,
        FIRST_SECURITY_MALE_VOICE,
        LAST_SECURITY_MALE_VOICE,
        FIRST_ANDROID_VOICE,
        LAST_ANDROID_VOICE,
    }

    public enum DIFFICULTY_SETTING_TYPE
    {
        EASY,
        MEDIUM,
        HARD,
        IRON,
        NOVICE,
    }

    public enum DUCK_HEIGHT
    {
        LOW,
        LOWER,
        LOWEST,
    }

    public enum ENEMY_TYPE
    {
        PLAYER,
        HUMAN,
        HUMAN_AND_PLAYER,
        ANDROID,
        NPC_HUMANOID,
        HUMANOID,
        ALIEN,
        ANY,
    }

    public enum ENVIRONMENT_ARCHETYPE
    {
        SCIENCE,
        HABITATION,
        TECHNICAL,
        ENGINEERING,
    }

    public enum EVENT_OCCURED_TYPE
    {
        SENSED_TARGET,
        SENSED_SUSPICIOUS_ITEM,
        TARGET_HIDEING,
        SUSPECT_TARGET_RESPONSE,
    }

    public enum FLAG_CHANGE_SOURCE_TYPE
    {
        SCRIPT,
        ACTION,
    }

    public enum FLASH_INVOKE_TYPE
    {
        NONE,
        INT,
        FLOAT,
        INT_INT,
        FLOAT_FLOAT,
        INT_FLOAT,
        FLOAT_INT,
        INT_INT_INT,
        FLOAT_FLOAT_FLOAT,
    }

    public enum FLASH_SCRIPT_RENDER_TYPE
    {
        NORMAL,
        RENDER_TO_TEXTURE,
        MULTI_RENDER_TO_TEXTURE,
    }

    public enum FOG_BOX_TYPE
    {
        BOX,
        PLANE,
    }

    public enum FOLLOW_CAMERA_MODIFIERS
    {
        WALKING,
        RUNNING,
        CROUCHING,
        AIMING,
        AIMING_CROUCHED,
        AIMING_THROW,
        AIMING_CROUCHED_THROW,
        EDGE_HORIZONTAL,
        EDGE_VERTICAL,
        COVER_LEFT,
        COVER_RIGHT,
        PEEKING_LEFT,
        PEEKING_RIGHT,
        PEEKING_OVER_LEFT,
        PEEKING_OVER_RIGHT,
        COVER_AIM_LEFT,
        COVER_AIM_RIGHT,
        COVER_AIM_OVER_LEFT,
        COVER_AIM_OVER_RIGHT,
        VENTS_CROUCH,
        VENTS_AIM,
        HIDING_COVER,
        TRAVERSAL_GAP,
        TRAVERSAL_AIMING,
        TRAVERSAL_CLIMB_OVER_VAULT_LOW,
        TRAVERSAL_CLIMB_OVER_VAULT_HIGH,
        TRAVERSAL_CLIMB_OVER_CLAMBER_LOW,
        TRAVERSAL_CLIMB_OVER_CLAMBER_MEDIUM,
        TRAVERSAL_CLIMB_OVER_CLAMBER_HIGH,
        TRAVERSAL_CLIMB_OVER_MANTLE_LOW,
        TRAVERSAL_CLIMB_OVER_MANTLE_MEDIUM,
        TRAVERSAL_CLIMB_OVER_MANTLE_HIGH,
        TRAVERSAL_CLIMB_UNDER,
        TRAVERSAL_LEAP,
        MELEE_ATTACK,
        COVER_LEFT_STANDING,
        COVER_RIGHT_STANDING,
        COVER_TRANSITION_LEFT,
        COVER_TRANSITION_RIGHT,
        HUB_ACCESSED,
        COVER_AIM_LEFT_STANDING,
        COVER_AIM_RIGHT_STANDING,
        SAFE_MODIFIER_STAND,
        SAFE_MODIFIER_CROUCH,
        HIDE_CROUCH,
        SAFE_MODIFIER_AIMING,
        SAFE_MODIFIER_AIMING_CROUCH,
        MOTION_TRACKER,
        MOTION_TRACKER_CROUCHED,
        MOTION_TRACKER_VENTS,
        BOLTGUN_AIM,
        BOLTGUN_AIM_CROUCHED,
        BOLTGUN_AIM_VENTS,
    }

    public enum FRAME_FLAGS
    {
        SUSPICIOUS_ITEM_LOW_PRIORITY,
        SUSPICIOUS_ITEM_MEDIUM_PRIORITY,
        SUSPICIOUS_ITEM_HIGH_PRIORITY,
        COULD_SEARCH,
        COULD_RESPOND_TO_HIDING_PLAYER,
        COULD_DO_SUSPICIOUS_ITEM_HIGH_PRIORITY,
        COULD_DO_SUSPECT_TARGET_RESPONSE_MOVE_TO,
    }

    public enum FRONTEND_STATE
    {
        FRONTEND_STATE_SPLASH,
        FRONTEND_STATE_MENU,
        FRONTEND_STATE_ATTRACT,
        FRONTEND_STATE_DLC_MAP,
    }

    public enum GAME_CLIP
    {
        DEATH_FROM_BELOW,
        THE_HUNT_BEGINS,
        SYNTHETIC_OFFLINE,
        ACCESS_ALL_AREAS,
        GOING_DOWN,
        JUST_IN_TIME,
        SYNTHETIC_INFERNO,
        SYSTEMS_SHOCK,
        HIGHLY_ADAPTABLE,
        GET_BACK,
        HUNTED,
        A_HAZARD_CONTAINED,
        ON_TARGET,
        FLAMIN_JOE,
        DEATH_FROM_ABOVE,
    }

    public enum IDLE
    {
        STAND,
        QUAD,
        DONT_CHANGE,
        DEFAULT,
        CROUCHED,
        IN_COVER,
    }

    public enum IDLE_STYLE
    {
        NO_FIDGETS,
        NORMAL,
        SEARCH,
        CORPSE_TRAP,
    }

    public enum IMPACT_CHARACTER_BODY_LOCATION_TYPE
    {
        ALL,
        ARMS,
        HEAD,
        NECK,
        TORSO,
        LEGS,
        HEAD_NECK,
        TORSO_LEGS,
        TORSO_ARMS,
        TORSO_ARMS_LEGS,
        ARMS_LEGS,
    }

    public enum INPUT_DEVICE_TYPE
    {
        MOUSE_AND_KEYBOARD,
        GAMEPAD,
    }

    public enum JOB_TYPE
    {
        IDLE,
        DESPAWN,
        AREA_SWEEP,
        AREA_SWEEP_FLARE,
        SYSTEMATIC_SEARCH_TARGET_JOB,
        SYSTEMATIC_SEARCH,
        SYSTEMATIC_SEARCH_FLARE,
        SYSTEMATIC_SEARCH_SPOTTING_POSITION,
        SYSTEMATIC_SEARCH_CRAWL_SPACE_SPOTTING_POSITION,
        PICKUP_WEAPON,
        ASSAULT,
        PLAYER_HIDING,
        FOLLOW,
        FOLLOW_CENTRE,
        PANIC,
    }

    public enum LEVEL_HEAP_TAG
    {
        AI_MEM_A_CHARACTER,
        AI_MEM_A_ENTITIES,
        AI_MEM_A_JOBS,
        AI_MEM_A_NAV_MESH,
        AI_MEM_AGG_MANAGER,
        AI_MEM_ENEMY_RECORD_CONTAINER,
        AI_MEM_SPOT_TASK_CONTAINER,
        AI_MEM_SUS_ITEM_CONTAINER,
        AI_MEM_SEARCH_TASK_CONTAINER,
        AI_MEM_VENT_MANAGER_CONTAINER,
        AI_MEM_COVER_CONTAINER,
        AI_MEM_DIALOGUE_CONTAINER,
        AI_MEM_SQUAD_CONTAINER,
        AI_MEM_NPC_COMBAT_CONTAINER,
        AI_MEM_NPC_CHARACTER_CONTAINER,
        AI_MEM_NPC_GROUP_MEMBER_CONTAINER,
        AI_MEM_JOB_CONTAINER,
        AI_MEM_VENT_CONTAINER,
        AI_MEM_BLACKBOARD_CONTAINER,
        AI_MEM_CATHODE_SUBS_CONTAINER,
        AI_MEM_PUBLISHER_CONTAINER,
        AI_MEM_THROWN_CONTAINER,
        AI_MEM_SMOKE_MANAGER_CONTAINER,
    }

    public enum LIGHT_ADAPTATION_MECHANISM
    {
        PERCENTILE,
        BRACKETED_MEAN,
    }

    public enum LIGHT_FADE_TYPE
    {
        NONE,
        SHADOW,
        LIGHT,
    }

    public enum LIGHT_TRANSITION
    {
        INSTANT,
        FADE,
        FLICKER,
        FLICKER_CUSTOM,
        FADE_FLICKER_CUSTOM,
    }

    public enum LIGHT_TYPE
    {
        OMNI,
        SPOT,
        STRIP,
    }

    public enum LOCOMOTION_STATE
    {
        WALKING,
        RUNNING,
        CROUCHING,
        IN_VENT,
        AIMING,
        TRAVERSING,
        IDLING,
    }

    public enum LOCOMOTION_TARGET_SPEED
    {
        SLOWEST,
        SLOW,
        FAST,
        FASTEST,
    }

    public enum LOGIC_CHARACTER_FLAGS
    {
        DONE_BREAKOUT,
        SHOULD_RESET,
        DO_ASSAULT_ATTACK_CHECKS,
        IS_IN_VENT,
        BANNED_FROM_VENT,
        HAS_DONE_GRAPPLE_BREAK,
        HAS_RECEIVED_DOT,
        IS_SITTING,
        DONE_ESCALATION_JOB,
        SHOULD_BREAKOUT,
        SHOULD_ATTACK,
        SHOULD_HIT_AND_RUN,
        DONE_HIT_AND_RUN,
        PLAYER_HIDING,
        ATTACK_HIDING_PLAYER,
        ALIEN_ALWAYS_KNOWS_WHEN_IN_VENT,
        IS_CORPSE_TRAP_ON_START,
        SHOULD_DESPAWN,
        ATTACK_HAS_GOT_WITHIN_ROUTING_THRESHOLD,
        LOCK_BACKSTAGE_STALK,
        TOTALLY_BLIND_IN_DARK,
        PLAYER_WON_HIDING_QTE,
        ANDROID_IS_INERT,
        ANDROID_IS_SHOWROOM_DUMMY,
        SHOULD_AMBUSH,
        NEVER_AGGRESSIVE,
        MUTE_DYNAMIC_DIALOGUE,
        DOING_THREAT_AWARE_ANIM,
        DONE_THREAT_AWARE,
        BLOCK_AMBUSH_AND_KILLTRAPS,
        PREVENT_GRAPPLES,
        PREVENT_ALL_ATTACKS,
        ALLOW_FLANKED_VENT_ATTACK,
        IGNORE_PLAYER_IN_VENT_BEHAVIOUR,
        USE_AIMED_STANCE_FOR_IDLE_JOBS,
        USE_AIMED_LOW_STANCE_FOR_IDLE_JOBS,
        CLOSE_TO_BACKSTAGE_ALIEN,
        IS_IN_EXPLOITABLE_AREA,
        IS_ON_LADDER,
        HAS_REPEATED_PATHFIND_FAILURES,
    }

    public enum LOGIC_CHARACTER_GAUGE_TYPE
    {
        RETREAT_GAUGE,
        STUN_DAMAGE_GAUGE,
    }

    public enum LOGIC_CHARACTER_TIMER_TYPE
    {
        SUSPECT_TARGET_RESPONSE_DELAY_TIMER,
        FIRST_LOGIC_CHARACTER_TIMER,
        THREAT_AWARE_TIMEOUT_TIMER,
        THREAT_AWARE_DURATION_TIMER,
        SEARCH_TIMEOUT_TIMER,
        BACKSTAGE_STALK_TIMEOUT_TIMER,
        AMBUSH_TIMEOUT_TIMER,
        ATTACK_BAN_TIMER,
        MELEE_ATTACK_BAN_TIMER,
        VENT_BAN_TIMER,
        NPC_STAY_IN_COVER_SHOOT_TIMER,
        NPC_JUST_LEFT_COMBAT_TIMER,
        ATTACK_KEEP_CHASING_TIMER,
        DELAY_RETURN_TO_SPAWN_POINT_TIMER,
        TARGET_IN_CRAWLSPACE_TIMER,
        DURATION_SINCE_SEARCH_TIMER,
        HEIGHTENED_SENSES_TIMER,
        BACKSTAGE_STALK_PICK_KILLTRAP_TIMER,
        FLANKED_VENT_ATTACK_TIMER,
        THREAT_AWARE_VISUAL_RETENTION_TIMER,
        RESPONSE_TO_BACKSTAGE_ALIEN_TIMEOUT_TIMER,
        VENT_ATTRACT_TIMER,
        SEEN_PLAYER_AIM_WEAPON_TIMER,
        SEARCH_BAN_TIMER,
        OBSERVE_TARGET_TIMER,
        REPEATED_PATHFIND_FAILUREST_TIMER,
    }

    public enum LOOK_SPEED
    {
        SLOW,
        MODERATE,
        FAST,
        FREE_LOOK,
        LEADING_LOOK,
    }

    public enum MELEE_ATTACK_TYPE
    {
        HIT_ATTACK,
        GRAPPLE_ATTACK,
        KILL_ATTACK,
        VENT,
        TRAP_ATTACK,
        ANY,
        NONE,
    }

    public enum MELEE_CONTEXT_TYPE
    {
        ANDROID_LOW_COVER_GRAPPLE,
        ANDROID_HIGH_COVER_GRAPPLE,
        ANDROID_CORPSE_TRAP_GRAPPLE,
    }

    public enum MOOD_INTENSITY
    {
        LOW,
        MEDIUM,
        HIGH,
    }

    public enum MUSIC_RTPC_MODE
    {
        UNCHANGED,
        THREAT,
        STEALTH,
        ALIEN_DISTANCE,
        MANUAL,
        OBJECT_DISTANCE,
    }

    public enum NAV_MESH_AREA_TYPE
    {
        BACKSTAGE,
        EXPENSIVE,
    }

    public enum NAVIGATION_CHARACTER_CLASS
    {
        PLAYER,
        ALIEN,
        ANDROID,
        HUMAN_NPC,
        FACEHUGGER,
    }

    public enum NAVIGATION_CHARACTER_CLASS_COMBINATION
    {
        NONE,
        PLAYER,
        ALIEN,
        ANDROID,
        HUMAN_NPC,
        FACEHUGGER,
        HUMAN,
        HUMANOID_NPC,
        HUMANOID,
        ANDROID_AND_ALIEN,
        PLAYER_AND_ALIEN,
        ALL,
    }

    public enum NOISE_TYPE
    {
        HARMONIC,
        FRACTAL,
    }

    public enum NPC_AGGRO_LEVEL
    {
        NONE,
        STAND_DOWN,
        INTERROGATIVE,
        WARNING,
        LAST_CHANCE,
        NO_LIMIT,
        AGGRESSIVE,
    }

    public enum NPC_COMBAT_STATE
    {
        NONE,
        WARNING,
        ATTACKING,
        REACHED_OBJECTIVE,
        ENTERED_COVER,
        LEAVE_COVER,
        START_RETREATING,
        REACHED_RETREAT,
        LOST_SENSE,
        SUSPICIOUS_WARNING,
        SUSPICIOUS_WARNING_FAILED,
        START_ADVANCE,
        DONE_ADVANCE,
        BLOCKING,
        HEARD_BS_ALIEN,
        ALIEN_SIGHTED,
    }

    public enum NPC_COVER_REQUEST_TYPE
    {
        DEFAULT,
        RETREAT,
        AGGRESSIVE,
        DEFENSIVE,
        ALIEN,
        PLAYER_IN_VENT,
    }

    public enum NPC_GUN_AIM_MODE
    {
        AUTO,
        GUN_DOWN,
        GUN_RAISED,
    }

    public enum ORIENTATION_AXIS
    {
        X_AXIS,
        Y_AXIS,
        Z_AXIS,
    }

    public enum PATH_DRIVEN_TYPE
    {
        CHARACTER_PROJECTION,
        POINT_PROJECTION,
        TIME_PROGRESS,
    }

    public enum PAUSE_SENSES_TYPE
    {
        KNOCKED_OUT,
    }

    public enum PLATFORM_TYPE
    {
        PL_PC,
        PL_PS3,
        PL_X360,
        PL_PS4,
        PL_XBOXONE,
        PL_OLDGEN,
        PL_NEXTGEN,
    }

    public enum PLAYER_INVENTORY_SET
    {
        DEFAULT_PLAYER,
        LV426_PLAYER,
        OTHER_PLAYER,
    }

    public enum POPUP_MESSAGE_ICON
    {
        ALERT,
        AUDIOLOG,
    }

    public enum POPUP_MESSAGE_SOUND
    {
        NONE,
        OBJECTIVE_NEW,
        OBJECTIVE_UPDATED,
        OBJECTIVE_COMPLETED,
    }

    public enum PRIORITY
    {
        LOWEST,
        LOW,
        MEDIUM,
        HIGH,
        HIGHEST,
    }

    public enum RANGE_TEST_SHAPE
    {
        SPHERE,
        VERTICAL_CYLINDER,
    }

    public enum RAYCAST_PRIORITY
    {
        LOW,
        MEDIUM,
        HIGH,
        CRITICAL,
    }

    public enum RESPAWN_MODE
    {
        ON_DEATH_POINT,
        NEAR_DEATH_POINT,
        NEAR_COMPANION,
    }

    public enum SECONDARY_ANIMATION_LAYER
    {
        GENERAL_ADDITIVE,
        BREATHE,
        GUN,
        TAIL,
        LOOK,
        HANDS,
        FACE,
    }

    public enum SENSE_SET
    {
        SET_1,
        SET_2,
        SET_3,
    }

    public enum SENSE_SET_DEFAULT
    {
        SET_NORMAL,
        SET_HEIGHTENED,
    }

    public enum SENSE_SET_SYSTEM
    {
        SET_1_NORMAL,
        SET_1_HEIGHTENED,
        SET_LAST_DEFAULT,
        SET_2,
        SET_3,
    }

    public enum SENSORY_TYPE
    {
        NONE,
        VISUAL,
        WEAPON_SOUND,
        MOVEMENT_SOUND,
        DAMAGE_CAUSED,
        TOUCHED,
        AFFECTED_BY_FLAME_THROWER,
        SEE_FLASH_LIGHT,
        COMBINED_SENSE,
    }

    public enum SHAKE_TYPE
    {
        CONSTANT,
        IMPULSE,
    }

    public enum SIDE
    {
        LEFT,
        RIGHT,
    }

    public enum SOUND_POOL
    {
        GENERAL,
        PLAYER_WEAPON,
    }

    public enum SPEECH_PRIORITY
    {
        LOW,
        MEDIUM,
        HIGH,
        STREAMED_COMBAT,
        COMBAT,
        DEATH,
    }

    public enum STEAL_CAMERA_TYPE
    {
        FORCE_STEAL,
        BUTTON_PROMPT,
        JUST_CONVERGE,
    }

    public enum SUSPECT_RESPONSE_PHASE
    {
        HEARD_GUN_SHOT,
        IDLE,
        VISUAL_SUCCESS,
        VISUAL_FAIL,
    }

    public enum SUSPICIOUS_ITEM
    {
        EXPLOSION,
        DEAD_BODY,
        ALARM,
        VENT_HISS,
        ACTIVE_FLARE,
        CUT_PANEL,
        FIRE_EXTINGUISHER,
        LIGHTS_ON,
        LIGHTS_OFF,
        DOOR_OPENED,
        DOOR_CLOSED,
        DISGUARDED_GUN,
        DEAD_FLARE,
        GLOW_STICK,
        OPEN_CONTAINER,
        NOISE_MAKER,
        EMP_MINE,
        FLASH_BANG,
        MOLOTOV,
        PIPE_BOMB,
        SMOKE_BOMB,
    }

    public enum SUSPICIOUS_ITEM_BEHAVIOUR_TREE_PRIORITY
    {
        LOW,
        MEDIUM,
        HIGH,
    }

    public enum SUSPICIOUS_ITEM_CLOSE_REACTION_DETAIL
    {
        VISUAL_FLOOR,
        VISUAL_WALL,
    }

    public enum SUSPICIOUS_ITEM_REACTION
    {
        INITIAL_REACTION,
        CLOSE_TO_FIRST_GROUP_MEMBER_REACTION,
        CLOSE_TO_SUBSEQUENT_GROUP_MEMBER_REACTION,
    }

    public enum SUSPICIOUS_ITEM_STAGE
    {
        NONE,
        FIRST_SENSED,
        INITIAL_REACTION,
        WAIT_FOR_TEAM_MEMBERS_ROUTING,
        MOVE_CLOSE_TO,
        CLOSE_TO_REACTION,
        CLOSE_TO_WAIT_FOR_GROUP_MEMBERS,
        SEARCH_AREA,
    }

    public enum SUSPICIOUS_ITEM_START_OR_CONTINUE_STATE
    {
        VALID_CURRENT_AND_IN_PROGRESS,
        VALID_TO_DO_INITIAL_REACTION,
        VALID_TO_DO_FURTHER_REACTION,
        LAST_VALID_STATE,
        DELAYED_INTERACTION_DELAY,
        LAST_VALID_OR_DELAYED_STATE,
        INVALID_NO_ITEM,
        INVALID_ALREADY_COMPLETED,
        INVALID_ALREADY_DONE_MIN_STAGE,
        INVALID_NO_MUST_DO_STAGE,
        INVALID_INTERACTION_DELAYED,
        INVALID_MEMBER_NOT_ALLOWED_TO_PROGRESS,
        INVALID_FUTHER_REACTION_TIMED_OUT_IN_PROGRESS,
        INVALID_FUTHER_REACTION_TIMED_OUT_LAST_TIME_TRIGGERED,
    }

    public enum SUSPICIOUS_ITEM_TRIGGER
    {
        VISBLE,
        INSTANT,
        CONTINUOUS,
    }

    public enum TASK_CHARACTER_CLASS_FILTER
    {
        USE_CHARACTER_PIN,
        PLAYER_ONLY,
        ALIEN_ONLY,
        ANDROID_ONLY,
        CIVILIAN_ONLY,
        SECURITY_ONLY,
        FACEHUGGER_ONLY,
        INNOCENT_ONLY,
        ANDROID_HEAVY_ONLY,
        MOTION_TRACKER,
        MELEE_HUMAN_ONLY,
        ANDROIDS,
        ALIENS,
        HUMAN_NPC,
        HUMAN,
        HUMANOID_NPC,
        HUMANOID,
        ANDROIDS_AND_ALIEN,
        PLAYER_AND_ALIEN,
        ALL,
        USE_FILTER_PIN,
        EXCLUDE_CHARACTER_PIN,
    }

    public enum TASK_PRIORITY
    {
        NORMAL,
        MEDIUM,
        HIGH,
    }

    public enum TERMINAL_LOCATION
    {
        SEVASTOPOL,
        TORRENS,
        NOSTROMO,
    }

    public enum TEXT_ALIGNMENT
    {
        TOP_LEFT,
        TOP_CENTRE,
        TOP_RIGHT,
        LEFT,
        CENTRE,
        RIGHT,
        BOTTOM_LEFT,
        BOTTOM_CENTRE,
        BOTTOM_RIGHT,
    }

    public enum THRESHOLD_QUALIFIER
    {
        UNSET,
        NONE,
        TRACE,
        LOWER,
        ACTIVATED,
        UPPER,
    }

    public enum TRANSITION_DIRECTION
    {
        POSITIVE_X,
        NEGATIVE_X,
        POSITIVE_Y,
        NEGATIVE_Y,
        CENTER,
    }

    public enum TRAVERSAL_ANIMS
    {
        Leap_Small,
        Leap_Medium,
        Leap_Large,
        Mantle_Small,
        Mantle_Medium,
        Mantle_Large,
        Vault_Small,
        Vault_Medium,
        Vault_Large,
        Custom,
    }

    public enum TRAVERSAL_TYPE
    {
        VAULT,
        MANTLE,
        CLIMB_OVER,
        JUMP_DOWN,
        VENT_ENTRY,
        VENT_EXIT,
        LADDER,
        FLOOR_VENT_ENTRY,
        FLOOR_VENT_EXIT,
    }

    public enum UI_ICON_ICON
    {
        IMPORTANT,
        CONTAINER,
    }

    public enum VENT_LOCK_REASON
    {
        FLANKED_VENT_ATTACK_FROM_ATTACK,
        FLANKED_VENT_ATTACK_FROM_THREAT_AWARE,
    }

    public enum VISIBILITY_SETTINGS_TYPE
    {
        VIEWCONESET_NONE,
        VIEWCONESET_STANDARD,
        VIEWCONESET_STANDARD_HIEGHTENED,
        VIEWCONESET_HUMAN,
        VIEWCONESET_HUMAN_HIEGHTENED,
        VIEWCONESET_SLEEPING,
        VIEWCONESET_ANDROID,
        VIEWCONESET_ANDROID_HIEGHTENED,
    }

    public enum WEAPON_HANDEDNESS
    {
        TWO_HANDED,
        ONE_HANDED,
        ONE_OR_TWO_HANDED,
    }

    public enum WEAPON_IMPACT_EFFECT_ORIENTATION
    {
        HIT_NORMAL,
        DIRECTION,
        REFLECTION,
        UP,
    }

    public enum WEAPON_IMPACT_EFFECT_TYPE
    {
        STANDARD,
        CHARACTER_DAMAGE,
    }

    public enum WEAPON_IMPACT_FILTER_ORIENTATION
    {
        CEILING,
        FLOOR,
        WALL,
    }

    public enum WEAPON_PROPERTY
    {
        ALIEN_THREAT_AWARE_OF,
    }

    public enum WEAPON_TYPE
    {
        NO_WEAPON,
        FLAMETHROWER,
        PISTOL,
        SHOTGUN,
        MELEE,
        BOLTGUN,
        CATTLEPROD,
    }


    #region Dan thinks these are ok

    public enum ANIM_MODE
    {
        FORWARD,
        BACKWARD,
        BOUNCE,
        RANDOM,
    }

    public enum FOLDER_LOCK_TYPE
    {
        LOCKED,
        NONE,
        KEYCODE,
    }

    public enum CHARACTER_NODE
    {
        HEAD1,
        HEAD,
        HIPS,
        TORSO,
        SPINE2,
        SPINE1,
        SPINE,
        LEFT_ARM,
        RIGHT_ARM,
        LEFT_HAND,
        RIGHT_HAND,
        LEFT_LEG,
        RIGHT_LEG,
        LEFT_FOOT,
        RIGHT_FOOT,
        LEFT_WEAPON_BONE,
        RIGHT_WEAPON_BONE,
        LEFT_SHOULDER,
        RIGHT_SHOULDER,
        WEAPON,
        ROOT,
    }

    public enum BEHAVIOR_TREE_BRANCH_TYPE
    {
        NONE,
        CINEMATIC_BRANCH,
        ATTACK_BRANCH,
        AIM_BRANCH,
        DESPAWN_BRANCH,
        FOLLOW_BRANCH,
        STANDARD_BRANCH,
        SEARCH_BRANCH,
        AREA_SWEEP_BRANCH,
        BACKSTAGE_AREA_SWEEP_BRANCH,
        SHOT_BRANCH,
        SUSPECT_TARGET_RESPONSE_BRANCH,
        THREAT_AWARE_BRANCH,
        BACKSTAGE_AMBUSH_BRANCH,
        IDLE_JOB_BRANCH,
        USE_COVER_BRANCH,
        ASSAULT_BRANCH,
        MELEE_BRANCH,
        RETREAT_BRANCH,
        CLOSE_ON_TARGET_BRANCH,
        MUTUAL_MELEE_ONLY_BRANCH,
        VENT_MELEE_BRANCH,
        ASSAULT_NOT_ALLOWED_BRANCH,
        IN_VENT_BRANCH,
        CLOSE_COMBAT_BRANCH,
        DRAW_WEAPON_BRANCH,
        PURSUE_TARGET_BRANCH,
        RANGED_ATTACK_BRANCH,
        RANGED_COMBAT_BRANCH,
        PRIMARY_CONTROL_RESPONSE_BRANCH,
        DEAD_BRANCH,
        SCRIPT_BRANCH,
        IDLE_BRANCH,
        DOWN_BUT_NOT_OUT_BRANCH,
        MELEE_BLOCK_BRANCH,
        AGRESSIVE_BRANCH,
        ALERT_BRANCH,
        SHOOTING_BRANCH,
        GRAPPLE_BREAK_BRANCH,
        REACT_TO_WEAPON_FIRE_BRANCH,
        IN_COVER_BRANCH,
        SUSPICIOUS_ITEM_BRANCH_HIGH,
        SUSPICIOUS_ITEM_BRANCH_MEDIUM,
        SUSPICIOUS_ITEM_BRANCH_LOW,
        AGGRESSION_ESCALATION_BRANCH,
        STUN_DAMAGE_BRANCH,
        BREAKOUT_BRANCH,
        SUSPEND_BRANCH,
        TARGET_IS_NPC_BRANCH,
        PLAYER_HIDING_BRANCH,
        ATTACK_CORE_BRANCH,
        CORPSE_TRAP_BRANCH,
        OBSERVE_TARGET_BRANCH,
        TARGET_IN_CRAWLSPACE_BRANCH,
        MB_SUSPICIOUS_ITEM_ATLEAST_MOVE_CLOSE_TO,
        MB_THREAT_AWARE_ATTACK_TARGET_WITHIN_CLOSE_RANGE,
        MB_THREAT_AWARE_ATTACK_TARGET_WITHIN_VERY_CLOSE_RANGE,
        MB_THREAT_AWARE_ATTACK_TARGET_FLAMED_ME,
        MB_THREAT_AWARE_ATTACK_WEAPON_NOT_AIMED,
        MB_THREAT_AWARE_MOVE_TO_LOST_VISUAL,
        MB_THREAT_AWARE_MOVE_TO_BEFORE_ANIM,
        MB_THREAT_AWARE_MOVE_TO_AFTER_ANIM,
        MB_THREAT_AWARE_MOVE_TO_FLANKED_VENT,
        KILLTRAP_BRANCH,
        PANIC_BRANCH,
        BACKSTAGE_ALIEN_RESPONSE_BRANCH,
        NPC_VS_ALIEN_BRANCH,
        USE_COVER_VS_ALIEN_BRANCH,
        IN_COVER_VS_ALIEN_BRANCH,
        REPEATED_PATHFIND_FAILS_BRANCH,
        ALL_SEARCH_VARIANTS_BRANCH,
    }

    public enum EQUIPMENT_SLOT
    {
        USE_CURRENT_SLOT = -2,
        ANY_WEAPON_SLOT,
        WEAPON_SLOT_SHOTGUN,
        WEAPON_SLOT_PISTOL,
        MELEE_SLOT,
        CATTLEPROD_SLOT,
        WEAPON_SLOT_FLAMETHROWER,
        WEAPON_SLOT_BOLTGUN,
        MOTION_TRACKER_SLOT,
        TORCH_SLOT,
        MEDIPEN_SLOT,
        TEMPORARY_WEAPON_SLOT,
        NO_WEAPON_SLOT = -3,
    }

    public enum DAMAGE_MODE
    {
        DAMAGED_BY_ALL,
        DAMAGED_BY_PLAYER_ONLY,
        INVINCIBLE,
    }

    public enum MAP_ICON_TYPE
    {
        REWIRE,
        HACKING,
        KEYPAD,
        LOCKED,
        SAVEPOINT,
        WAYPOINT,
        TOOL_PICKUP,
        WEAPON_PICKUP,
        CUTTING_POINT,
        LEVEL_LOAD,
        LADDER,
        CONTAINER,
        TERMINAL_INTERACTION,
        LOCKED_DOOR,
        UNLOCKED_DOOR,
        HIDDEN,
        GENERIC_INTERACTION,
        POWERED_DOWN_DOOR,
        KEYCODE,
    }

    public enum GATING_TOOL_TYPE
    {
        KEY,
        AXE,
        CROWBAR,
        GAS_MASK,
        CUTTING_TOOL,
        HACKING_TOOL,
        REWIRE,
        CABLE_CLAMPS,
    }

    public enum MOOD
    {
        NEUTRAL,
        SCARED,
        ANGRY,
        HAPPY,
        SAD,
        SUSPICIOUS,
        INJURED,
    }

    public enum FOLLOW_TYPE
    {
        LEADING_THE_WAY,
        EXPLORATION,
    }

    public enum TASK_OPERATION_MODE
    {
        FULLY_SHARED,
        SINGLE_AND_EXCLUSIVE,
    }

    public enum MOVE
    {
        SLOW_WALK,
        WALK,
        FAST_WALK,
        RUN,
        OBSOLETE_1,
        OBSOLETE_2,
        TELEPORT,
    }

    public enum SUB_OBJECTIVE_TYPE
    {
        NONE,
        POINT,
        SMALL_AREA,
        MEDIUM_AREA,
        LARGE_AREA,
    }

    public enum LEVER_TYPE
    {
        PULL,
        ROTATE,
    }

    public enum BUTTON_TYPE
    {
        KEYS,
        DISK,
    }

    public enum DOOR_MECHANISM
    {
        NONE,
        BLANK,
        KEYPAD,
        LEVER,
        BUTTON,
        HACKING,
        HIDDEN_KEYPAD,
        HIDDEN_LEVER,
        HIDDEN_HACKING,
        HIDDEN_BUTTON,
    }

    public enum DOOR_STATE
    {
        USE_KEYCARD,
        USE_KEYCODE,
        USE_LEVER,
        USE_HACKING,
        USE_CUTTING,
        USE_BUTTON,
        USE_MECHANISM,
        UPGRADE_HACKING,
        HACKING_REQUIRED,
        RESTORE_POWER,
        UPGRADE_CUTTING,
        CUTTING_REQUIRED,
        KEYCARD_REQUIRED,
        LOCKED,
    }

    public enum PICKUP_CATEGORY
    {
        UNKNOWN,
        WEAPON,
        AMMO,
        ITEM,
        KILLTRAP,
        DOOR,
        COMPUTER,
        ALIEN,
        SAVE_TERMINAL,
    }

    public enum REWIRE_SYSTEM_NAME
    {
        AI_UI_MAIN_LIGHTING,
        AI_UI_DOOR_SYSTEM,
        AI_UI_VENT_ACCESS,
        AI_UI_SECURITY_CAMERAS,
        AI_UI_TANNOY,
        AI_UI_ALARMS,
        AI_UI_SPRINKLERS,
        AI_UI_RELEASE_VALVE,
        AI_UI_GAS_LEAK,
        AI_UI_AIR_CONDITIONING,
        AI_UI_UNSTABLE_SYSTEM,
    }

    public enum REWIRE_SYSTEM_TYPE
    {
        AI_UI_MAIN_LIGHTING,
        AI_UI_DOOR_SYSTEM,
        AI_UI_VENT_ACCESS,
        AI_UI_SECURITY_CAMERAS,
        AI_UI_TANNOY,
        AI_UI_ALARMS,
        AI_UI_SPRINKLERS,
        AI_UI_RELEASE_VALVE,
        AI_UI_GAS_LEAK,
        AI_UI_AIR_CONDITIONING,
        AI_UI_UNSTABLE_SYSTEM,
    }

    public enum EXIT_WAYPOINT
    {
        SOLACE,
        AIRPORT,
        TECH_HUB,
        TECH_COMMS,
        SCI_HUB,
        HOSPITAL_UPPER,
        HOSPITAL_LOWER,
        ANDROID_LAB,
        SHOPPING_CENTRE,
        LV426,
        TECH_RND,
        REACTOR_CORE,
        RND_HAZLAB,
        CORP_PENT,
        APPOLLO_CORE,
        DRY_DOCKS,
        GRAVITY_ANCHORS,
        TOWING_PLATFORM,
    }

    public enum VIEWCONE_TYPE
    {
        RECTANGLE = 1,
        ELLIPSE,
    }

    public enum WAVE_SHAPE
    {
        SIN,
        SAW,
        REV_SAW,
        SQUARE,
        TRIANGLE,
    }

    public enum UI_KEYGATE_TYPE
    {
        KEYCARD,
        KEYPAD,
    }
    #endregion


    //Below here I'm confident on the indexes

    public enum CHARACTER_STANCE
    {
        DONT_CHANGE,
        STAND,
        CROUCHED,
    }

    public enum CI_MESSAGE_TYPE 
    {
        MSG_NORMAL,
        MSG_IMPORTANT,
        MSG_ERROR,
    }

    public enum CHARACTER_CLASS
    {
        PLAYER,
        ALIEN,
        ANDROID,
        CIVILIAN,
        SECURITY,
        FACEHUGGER,
        INNOCENT,
        ANDROID_HEAVY,
        MOTION_TRACKER,
        MELEE_HUMAN,
    }

    public enum CHARACTER_CLASS_COMBINATION
    {
        NONE = 0x0,
        PLAYER_ONLY = 0x1,
        ALIEN_ONLY = 0x2,
        ANDROID_ONLY = 0x4,
        CIVILIAN_ONLY = 0x8,
        SECURITY_ONLY = 0x10,
        FACEHUGGER_ONLY = 0x20,
        INNOCENT_ONLY = 0x40,
        ANDROID_HEAVY_ONLY = 0x80,
        MOTION_TRACKER = 0x100,
        MELEE_HUMAN_ONLY = 0x200,
        ANDROIDS = ANDROID_ONLY | ANDROID_HEAVY_ONLY,
        ALIENS = ALIEN_ONLY | FACEHUGGER_ONLY,
        HUMAN_NPC = CIVILIAN_ONLY | SECURITY_ONLY | INNOCENT_ONLY | MELEE_HUMAN_ONLY,
        HUMAN = HUMAN_NPC | PLAYER_ONLY,
        HUMANOID_NPC = HUMAN_NPC | ANDROIDS,
        HUMANOID = HUMANOID_NPC | PLAYER_ONLY,
        ANDROIDS_AND_ALIEN = ANDROIDS | ALIENS,
        PLAYER_AND_ALIEN = PLAYER_ONLY | ALIEN_ONLY,
        ALL = ALIENS | HUMANOID | MOTION_TRACKER,
    }
}
