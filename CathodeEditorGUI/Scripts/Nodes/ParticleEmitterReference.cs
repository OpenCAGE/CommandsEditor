using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ParticleEmitterReference : STNode
	{
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_show_on_reset;
		[STNodeProperty("show_on_reset", "show_on_reset")]
		public bool m_show_on_reset
		{
			get { return _m_show_on_reset; }
			set { _m_show_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_deleted;
		[STNodeProperty("deleted", "deleted")]
		public bool m_deleted
		{
			get { return _m_deleted; }
			set { _m_deleted = value; this.Invalidate(); }
		}
		
		private bool _m_use_local_rotation;
		[STNodeProperty("use_local_rotation", "use_local_rotation")]
		public bool m_use_local_rotation
		{
			get { return _m_use_local_rotation; }
			set { _m_use_local_rotation = value; this.Invalidate(); }
		}
		
		private bool _m_include_in_planar_reflections;
		[STNodeProperty("include_in_planar_reflections", "include_in_planar_reflections")]
		public bool m_include_in_planar_reflections
		{
			get { return _m_include_in_planar_reflections; }
			set { _m_include_in_planar_reflections = value; this.Invalidate(); }
		}
		
		private string _m_material;
		[STNodeProperty("material", "material")]
		public string m_material
		{
			get { return _m_material; }
			set { _m_material = value; this.Invalidate(); }
		}
		
		private bool _m_unique_material;
		[STNodeProperty("unique_material", "unique_material")]
		public bool m_unique_material
		{
			get { return _m_unique_material; }
			set { _m_unique_material = value; this.Invalidate(); }
		}
		
		private int _m_quality_level;
		[STNodeProperty("quality_level", "quality_level")]
		public int m_quality_level
		{
			get { return _m_quality_level; }
			set { _m_quality_level = value; this.Invalidate(); }
		}
		
		private cVector3 _m_bounds_max;
		[STNodeProperty("bounds_max", "bounds_max")]
		public cVector3 m_bounds_max
		{
			get { return _m_bounds_max; }
			set { _m_bounds_max = value; this.Invalidate(); }
		}
		
		private cVector3 _m_bounds_min;
		[STNodeProperty("bounds_min", "bounds_min")]
		public cVector3 m_bounds_min
		{
			get { return _m_bounds_min; }
			set { _m_bounds_min = value; this.Invalidate(); }
		}
		
		private string _m_TEXTURE_MAP;
		[STNodeProperty("TEXTURE_MAP", "TEXTURE_MAP")]
		public string m_TEXTURE_MAP
		{
			get { return _m_TEXTURE_MAP; }
			set { _m_TEXTURE_MAP = value; this.Invalidate(); }
		}
		
		private int _m_DRAW_PASS;
		[STNodeProperty("DRAW_PASS", "DRAW_PASS")]
		public int m_DRAW_PASS
		{
			get { return _m_DRAW_PASS; }
			set { _m_DRAW_PASS = value; this.Invalidate(); }
		}
		
		private float _m_ASPECT_RATIO;
		[STNodeProperty("ASPECT_RATIO", "ASPECT_RATIO")]
		public float m_ASPECT_RATIO
		{
			get { return _m_ASPECT_RATIO; }
			set { _m_ASPECT_RATIO = value; this.Invalidate(); }
		}
		
		private float _m_FADE_AT_DISTANCE;
		[STNodeProperty("FADE_AT_DISTANCE", "FADE_AT_DISTANCE")]
		public float m_FADE_AT_DISTANCE
		{
			get { return _m_FADE_AT_DISTANCE; }
			set { _m_FADE_AT_DISTANCE = value; this.Invalidate(); }
		}
		
		private int _m_PARTICLE_COUNT;
		[STNodeProperty("PARTICLE_COUNT", "PARTICLE_COUNT")]
		public int m_PARTICLE_COUNT
		{
			get { return _m_PARTICLE_COUNT; }
			set { _m_PARTICLE_COUNT = value; this.Invalidate(); }
		}
		
		private float _m_SYSTEM_EXPIRY_TIME;
		[STNodeProperty("SYSTEM_EXPIRY_TIME", "SYSTEM_EXPIRY_TIME")]
		public float m_SYSTEM_EXPIRY_TIME
		{
			get { return _m_SYSTEM_EXPIRY_TIME; }
			set { _m_SYSTEM_EXPIRY_TIME = value; this.Invalidate(); }
		}
		
		private float _m_SIZE_START_MIN;
		[STNodeProperty("SIZE_START_MIN", "SIZE_START_MIN")]
		public float m_SIZE_START_MIN
		{
			get { return _m_SIZE_START_MIN; }
			set { _m_SIZE_START_MIN = value; this.Invalidate(); }
		}
		
		private float _m_SIZE_START_MAX;
		[STNodeProperty("SIZE_START_MAX", "SIZE_START_MAX")]
		public float m_SIZE_START_MAX
		{
			get { return _m_SIZE_START_MAX; }
			set { _m_SIZE_START_MAX = value; this.Invalidate(); }
		}
		
		private float _m_SIZE_END_MIN;
		[STNodeProperty("SIZE_END_MIN", "SIZE_END_MIN")]
		public float m_SIZE_END_MIN
		{
			get { return _m_SIZE_END_MIN; }
			set { _m_SIZE_END_MIN = value; this.Invalidate(); }
		}
		
		private float _m_SIZE_END_MAX;
		[STNodeProperty("SIZE_END_MAX", "SIZE_END_MAX")]
		public float m_SIZE_END_MAX
		{
			get { return _m_SIZE_END_MAX; }
			set { _m_SIZE_END_MAX = value; this.Invalidate(); }
		}
		
		private float _m_ALPHA_IN;
		[STNodeProperty("ALPHA_IN", "ALPHA_IN")]
		public float m_ALPHA_IN
		{
			get { return _m_ALPHA_IN; }
			set { _m_ALPHA_IN = value; this.Invalidate(); }
		}
		
		private float _m_ALPHA_OUT;
		[STNodeProperty("ALPHA_OUT", "ALPHA_OUT")]
		public float m_ALPHA_OUT
		{
			get { return _m_ALPHA_OUT; }
			set { _m_ALPHA_OUT = value; this.Invalidate(); }
		}
		
		private float _m_MASK_AMOUNT_MIN;
		[STNodeProperty("MASK_AMOUNT_MIN", "MASK_AMOUNT_MIN")]
		public float m_MASK_AMOUNT_MIN
		{
			get { return _m_MASK_AMOUNT_MIN; }
			set { _m_MASK_AMOUNT_MIN = value; this.Invalidate(); }
		}
		
		private float _m_MASK_AMOUNT_MAX;
		[STNodeProperty("MASK_AMOUNT_MAX", "MASK_AMOUNT_MAX")]
		public float m_MASK_AMOUNT_MAX
		{
			get { return _m_MASK_AMOUNT_MAX; }
			set { _m_MASK_AMOUNT_MAX = value; this.Invalidate(); }
		}
		
		private float _m_MASK_AMOUNT_MIDPOINT;
		[STNodeProperty("MASK_AMOUNT_MIDPOINT", "MASK_AMOUNT_MIDPOINT")]
		public float m_MASK_AMOUNT_MIDPOINT
		{
			get { return _m_MASK_AMOUNT_MIDPOINT; }
			set { _m_MASK_AMOUNT_MIDPOINT = value; this.Invalidate(); }
		}
		
		private float _m_PARTICLE_EXPIRY_TIME_MIN;
		[STNodeProperty("PARTICLE_EXPIRY_TIME_MIN", "PARTICLE_EXPIRY_TIME_MIN")]
		public float m_PARTICLE_EXPIRY_TIME_MIN
		{
			get { return _m_PARTICLE_EXPIRY_TIME_MIN; }
			set { _m_PARTICLE_EXPIRY_TIME_MIN = value; this.Invalidate(); }
		}
		
		private float _m_PARTICLE_EXPIRY_TIME_MAX;
		[STNodeProperty("PARTICLE_EXPIRY_TIME_MAX", "PARTICLE_EXPIRY_TIME_MAX")]
		public float m_PARTICLE_EXPIRY_TIME_MAX
		{
			get { return _m_PARTICLE_EXPIRY_TIME_MAX; }
			set { _m_PARTICLE_EXPIRY_TIME_MAX = value; this.Invalidate(); }
		}
		
		private float _m_COLOUR_SCALE_MIN;
		[STNodeProperty("COLOUR_SCALE_MIN", "COLOUR_SCALE_MIN")]
		public float m_COLOUR_SCALE_MIN
		{
			get { return _m_COLOUR_SCALE_MIN; }
			set { _m_COLOUR_SCALE_MIN = value; this.Invalidate(); }
		}
		
		private float _m_COLOUR_SCALE_MAX;
		[STNodeProperty("COLOUR_SCALE_MAX", "COLOUR_SCALE_MAX")]
		public float m_COLOUR_SCALE_MAX
		{
			get { return _m_COLOUR_SCALE_MAX; }
			set { _m_COLOUR_SCALE_MAX = value; this.Invalidate(); }
		}
		
		private float _m_WIND_X;
		[STNodeProperty("WIND_X", "WIND_X")]
		public float m_WIND_X
		{
			get { return _m_WIND_X; }
			set { _m_WIND_X = value; this.Invalidate(); }
		}
		
		private float _m_WIND_Y;
		[STNodeProperty("WIND_Y", "WIND_Y")]
		public float m_WIND_Y
		{
			get { return _m_WIND_Y; }
			set { _m_WIND_Y = value; this.Invalidate(); }
		}
		
		private float _m_WIND_Z;
		[STNodeProperty("WIND_Z", "WIND_Z")]
		public float m_WIND_Z
		{
			get { return _m_WIND_Z; }
			set { _m_WIND_Z = value; this.Invalidate(); }
		}
		
		private float _m_ALPHA_REF_VALUE;
		[STNodeProperty("ALPHA_REF_VALUE", "ALPHA_REF_VALUE")]
		public float m_ALPHA_REF_VALUE
		{
			get { return _m_ALPHA_REF_VALUE; }
			set { _m_ALPHA_REF_VALUE = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_LS;
		[STNodeProperty("BILLBOARDING_LS", "BILLBOARDING_LS")]
		public int m_BILLBOARDING_LS
		{
			get { return _m_BILLBOARDING_LS; }
			set { _m_BILLBOARDING_LS = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING;
		[STNodeProperty("BILLBOARDING", "BILLBOARDING")]
		public int m_BILLBOARDING
		{
			get { return _m_BILLBOARDING; }
			set { _m_BILLBOARDING = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_NONE;
		[STNodeProperty("BILLBOARDING_NONE", "BILLBOARDING_NONE")]
		public int m_BILLBOARDING_NONE
		{
			get { return _m_BILLBOARDING_NONE; }
			set { _m_BILLBOARDING_NONE = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_ON_AXIS_X;
		[STNodeProperty("BILLBOARDING_ON_AXIS_X", "BILLBOARDING_ON_AXIS_X")]
		public int m_BILLBOARDING_ON_AXIS_X
		{
			get { return _m_BILLBOARDING_ON_AXIS_X; }
			set { _m_BILLBOARDING_ON_AXIS_X = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_ON_AXIS_Y;
		[STNodeProperty("BILLBOARDING_ON_AXIS_Y", "BILLBOARDING_ON_AXIS_Y")]
		public int m_BILLBOARDING_ON_AXIS_Y
		{
			get { return _m_BILLBOARDING_ON_AXIS_Y; }
			set { _m_BILLBOARDING_ON_AXIS_Y = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_ON_AXIS_Z;
		[STNodeProperty("BILLBOARDING_ON_AXIS_Z", "BILLBOARDING_ON_AXIS_Z")]
		public int m_BILLBOARDING_ON_AXIS_Z
		{
			get { return _m_BILLBOARDING_ON_AXIS_Z; }
			set { _m_BILLBOARDING_ON_AXIS_Z = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_VELOCITY_ALIGNED;
		[STNodeProperty("BILLBOARDING_VELOCITY_ALIGNED", "BILLBOARDING_VELOCITY_ALIGNED")]
		public int m_BILLBOARDING_VELOCITY_ALIGNED
		{
			get { return _m_BILLBOARDING_VELOCITY_ALIGNED; }
			set { _m_BILLBOARDING_VELOCITY_ALIGNED = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_VELOCITY_STRETCHED;
		[STNodeProperty("BILLBOARDING_VELOCITY_STRETCHED", "BILLBOARDING_VELOCITY_STRETCHED")]
		public int m_BILLBOARDING_VELOCITY_STRETCHED
		{
			get { return _m_BILLBOARDING_VELOCITY_STRETCHED; }
			set { _m_BILLBOARDING_VELOCITY_STRETCHED = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_SPHERE_PROJECTION;
		[STNodeProperty("BILLBOARDING_SPHERE_PROJECTION", "BILLBOARDING_SPHERE_PROJECTION")]
		public int m_BILLBOARDING_SPHERE_PROJECTION
		{
			get { return _m_BILLBOARDING_SPHERE_PROJECTION; }
			set { _m_BILLBOARDING_SPHERE_PROJECTION = value; this.Invalidate(); }
		}
		
		private int _m_BLENDING_STANDARD;
		[STNodeProperty("BLENDING_STANDARD", "BLENDING_STANDARD")]
		public int m_BLENDING_STANDARD
		{
			get { return _m_BLENDING_STANDARD; }
			set { _m_BLENDING_STANDARD = value; this.Invalidate(); }
		}
		
		private int _m_BLENDING_ALPHA_REF;
		[STNodeProperty("BLENDING_ALPHA_REF", "BLENDING_ALPHA_REF")]
		public int m_BLENDING_ALPHA_REF
		{
			get { return _m_BLENDING_ALPHA_REF; }
			set { _m_BLENDING_ALPHA_REF = value; this.Invalidate(); }
		}
		
		private int _m_BLENDING_ADDITIVE;
		[STNodeProperty("BLENDING_ADDITIVE", "BLENDING_ADDITIVE")]
		public int m_BLENDING_ADDITIVE
		{
			get { return _m_BLENDING_ADDITIVE; }
			set { _m_BLENDING_ADDITIVE = value; this.Invalidate(); }
		}
		
		private int _m_BLENDING_PREMULTIPLIED;
		[STNodeProperty("BLENDING_PREMULTIPLIED", "BLENDING_PREMULTIPLIED")]
		public int m_BLENDING_PREMULTIPLIED
		{
			get { return _m_BLENDING_PREMULTIPLIED; }
			set { _m_BLENDING_PREMULTIPLIED = value; this.Invalidate(); }
		}
		
		private int _m_BLENDING_DISTORTION;
		[STNodeProperty("BLENDING_DISTORTION", "BLENDING_DISTORTION")]
		public int m_BLENDING_DISTORTION
		{
			get { return _m_BLENDING_DISTORTION; }
			set { _m_BLENDING_DISTORTION = value; this.Invalidate(); }
		}
		
		private int _m_LOW_RES;
		[STNodeProperty("LOW_RES", "LOW_RES")]
		public int m_LOW_RES
		{
			get { return _m_LOW_RES; }
			set { _m_LOW_RES = value; this.Invalidate(); }
		}
		
		private int _m_EARLY_ALPHA;
		[STNodeProperty("EARLY_ALPHA", "EARLY_ALPHA")]
		public int m_EARLY_ALPHA
		{
			get { return _m_EARLY_ALPHA; }
			set { _m_EARLY_ALPHA = value; this.Invalidate(); }
		}
		
		private int _m_LOOPING;
		[STNodeProperty("LOOPING", "LOOPING")]
		public int m_LOOPING
		{
			get { return _m_LOOPING; }
			set { _m_LOOPING = value; this.Invalidate(); }
		}
		
		private int _m_ANIMATED_ALPHA;
		[STNodeProperty("ANIMATED_ALPHA", "ANIMATED_ALPHA")]
		public int m_ANIMATED_ALPHA
		{
			get { return _m_ANIMATED_ALPHA; }
			set { _m_ANIMATED_ALPHA = value; this.Invalidate(); }
		}
		
		private int _m_NONE;
		[STNodeProperty("NONE", "NONE")]
		public int m_NONE
		{
			get { return _m_NONE; }
			set { _m_NONE = value; this.Invalidate(); }
		}
		
		private int _m_LIGHTING;
		[STNodeProperty("LIGHTING", "LIGHTING")]
		public int m_LIGHTING
		{
			get { return _m_LIGHTING; }
			set { _m_LIGHTING = value; this.Invalidate(); }
		}
		
		private int _m_PER_PARTICLE_LIGHTING;
		[STNodeProperty("PER_PARTICLE_LIGHTING", "PER_PARTICLE_LIGHTING")]
		public int m_PER_PARTICLE_LIGHTING
		{
			get { return _m_PER_PARTICLE_LIGHTING; }
			set { _m_PER_PARTICLE_LIGHTING = value; this.Invalidate(); }
		}
		
		private int _m_X_AXIS_FLIP;
		[STNodeProperty("X_AXIS_FLIP", "X_AXIS_FLIP")]
		public int m_X_AXIS_FLIP
		{
			get { return _m_X_AXIS_FLIP; }
			set { _m_X_AXIS_FLIP = value; this.Invalidate(); }
		}
		
		private int _m_Y_AXIS_FLIP;
		[STNodeProperty("Y_AXIS_FLIP", "Y_AXIS_FLIP")]
		public int m_Y_AXIS_FLIP
		{
			get { return _m_Y_AXIS_FLIP; }
			set { _m_Y_AXIS_FLIP = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARD_FACING;
		[STNodeProperty("BILLBOARD_FACING", "BILLBOARD_FACING")]
		public int m_BILLBOARD_FACING
		{
			get { return _m_BILLBOARD_FACING; }
			set { _m_BILLBOARD_FACING = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_ON_AXIS_FADEOUT;
		[STNodeProperty("BILLBOARDING_ON_AXIS_FADEOUT", "BILLBOARDING_ON_AXIS_FADEOUT")]
		public int m_BILLBOARDING_ON_AXIS_FADEOUT
		{
			get { return _m_BILLBOARDING_ON_AXIS_FADEOUT; }
			set { _m_BILLBOARDING_ON_AXIS_FADEOUT = value; this.Invalidate(); }
		}
		
		private int _m_BILLBOARDING_CAMERA_LOCKED;
		[STNodeProperty("BILLBOARDING_CAMERA_LOCKED", "BILLBOARDING_CAMERA_LOCKED")]
		public int m_BILLBOARDING_CAMERA_LOCKED
		{
			get { return _m_BILLBOARDING_CAMERA_LOCKED; }
			set { _m_BILLBOARDING_CAMERA_LOCKED = value; this.Invalidate(); }
		}
		
		private float _m_CAMERA_RELATIVE_POS_X;
		[STNodeProperty("CAMERA_RELATIVE_POS_X", "CAMERA_RELATIVE_POS_X")]
		public float m_CAMERA_RELATIVE_POS_X
		{
			get { return _m_CAMERA_RELATIVE_POS_X; }
			set { _m_CAMERA_RELATIVE_POS_X = value; this.Invalidate(); }
		}
		
		private float _m_CAMERA_RELATIVE_POS_Y;
		[STNodeProperty("CAMERA_RELATIVE_POS_Y", "CAMERA_RELATIVE_POS_Y")]
		public float m_CAMERA_RELATIVE_POS_Y
		{
			get { return _m_CAMERA_RELATIVE_POS_Y; }
			set { _m_CAMERA_RELATIVE_POS_Y = value; this.Invalidate(); }
		}
		
		private float _m_CAMERA_RELATIVE_POS_Z;
		[STNodeProperty("CAMERA_RELATIVE_POS_Z", "CAMERA_RELATIVE_POS_Z")]
		public float m_CAMERA_RELATIVE_POS_Z
		{
			get { return _m_CAMERA_RELATIVE_POS_Z; }
			set { _m_CAMERA_RELATIVE_POS_Z = value; this.Invalidate(); }
		}
		
		private float _m_SPHERE_PROJECTION_RADIUS;
		[STNodeProperty("SPHERE_PROJECTION_RADIUS", "SPHERE_PROJECTION_RADIUS")]
		public float m_SPHERE_PROJECTION_RADIUS
		{
			get { return _m_SPHERE_PROJECTION_RADIUS; }
			set { _m_SPHERE_PROJECTION_RADIUS = value; this.Invalidate(); }
		}
		
		private float _m_DISTORTION_STRENGTH;
		[STNodeProperty("DISTORTION_STRENGTH", "DISTORTION_STRENGTH")]
		public float m_DISTORTION_STRENGTH
		{
			get { return _m_DISTORTION_STRENGTH; }
			set { _m_DISTORTION_STRENGTH = value; this.Invalidate(); }
		}
		
		private float _m_SCALE_MODIFIER;
		[STNodeProperty("SCALE_MODIFIER", "SCALE_MODIFIER")]
		public float m_SCALE_MODIFIER
		{
			get { return _m_SCALE_MODIFIER; }
			set { _m_SCALE_MODIFIER = value; this.Invalidate(); }
		}
		
		private int _m_CPU;
		[STNodeProperty("CPU", "CPU")]
		public int m_CPU
		{
			get { return _m_CPU; }
			set { _m_CPU = value; this.Invalidate(); }
		}
		
		private float _m_SPAWN_RATE;
		[STNodeProperty("SPAWN_RATE", "SPAWN_RATE")]
		public float m_SPAWN_RATE
		{
			get { return _m_SPAWN_RATE; }
			set { _m_SPAWN_RATE = value; this.Invalidate(); }
		}
		
		private float _m_SPAWN_RATE_VAR;
		[STNodeProperty("SPAWN_RATE_VAR", "SPAWN_RATE_VAR")]
		public float m_SPAWN_RATE_VAR
		{
			get { return _m_SPAWN_RATE_VAR; }
			set { _m_SPAWN_RATE_VAR = value; this.Invalidate(); }
		}
		
		private int _m_SPAWN_NUMBER;
		[STNodeProperty("SPAWN_NUMBER", "SPAWN_NUMBER")]
		public int m_SPAWN_NUMBER
		{
			get { return _m_SPAWN_NUMBER; }
			set { _m_SPAWN_NUMBER = value; this.Invalidate(); }
		}
		
		private float _m_LIFETIME;
		[STNodeProperty("LIFETIME", "LIFETIME")]
		public float m_LIFETIME
		{
			get { return _m_LIFETIME; }
			set { _m_LIFETIME = value; this.Invalidate(); }
		}
		
		private float _m_LIFETIME_VAR;
		[STNodeProperty("LIFETIME_VAR", "LIFETIME_VAR")]
		public float m_LIFETIME_VAR
		{
			get { return _m_LIFETIME_VAR; }
			set { _m_LIFETIME_VAR = value; this.Invalidate(); }
		}
		
		private float _m_WORLD_TO_LOCAL_BLEND_START;
		[STNodeProperty("WORLD_TO_LOCAL_BLEND_START", "WORLD_TO_LOCAL_BLEND_START")]
		public float m_WORLD_TO_LOCAL_BLEND_START
		{
			get { return _m_WORLD_TO_LOCAL_BLEND_START; }
			set { _m_WORLD_TO_LOCAL_BLEND_START = value; this.Invalidate(); }
		}
		
		private float _m_WORLD_TO_LOCAL_BLEND_END;
		[STNodeProperty("WORLD_TO_LOCAL_BLEND_END", "WORLD_TO_LOCAL_BLEND_END")]
		public float m_WORLD_TO_LOCAL_BLEND_END
		{
			get { return _m_WORLD_TO_LOCAL_BLEND_END; }
			set { _m_WORLD_TO_LOCAL_BLEND_END = value; this.Invalidate(); }
		}
		
		private float _m_WORLD_TO_LOCAL_MAX_DIST;
		[STNodeProperty("WORLD_TO_LOCAL_MAX_DIST", "WORLD_TO_LOCAL_MAX_DIST")]
		public float m_WORLD_TO_LOCAL_MAX_DIST
		{
			get { return _m_WORLD_TO_LOCAL_MAX_DIST; }
			set { _m_WORLD_TO_LOCAL_MAX_DIST = value; this.Invalidate(); }
		}
		
		private int _m_CELL_EMISSION;
		[STNodeProperty("CELL_EMISSION", "CELL_EMISSION")]
		public int m_CELL_EMISSION
		{
			get { return _m_CELL_EMISSION; }
			set { _m_CELL_EMISSION = value; this.Invalidate(); }
		}
		
		private float _m_CELL_MAX_DIST;
		[STNodeProperty("CELL_MAX_DIST", "CELL_MAX_DIST")]
		public float m_CELL_MAX_DIST
		{
			get { return _m_CELL_MAX_DIST; }
			set { _m_CELL_MAX_DIST = value; this.Invalidate(); }
		}
		
		private int _m_CUSTOM_SEED_CPU;
		[STNodeProperty("CUSTOM_SEED_CPU", "CUSTOM_SEED_CPU")]
		public int m_CUSTOM_SEED_CPU
		{
			get { return _m_CUSTOM_SEED_CPU; }
			set { _m_CUSTOM_SEED_CPU = value; this.Invalidate(); }
		}
		
		private int _m_SEED;
		[STNodeProperty("SEED", "SEED")]
		public int m_SEED
		{
			get { return _m_SEED; }
			set { _m_SEED = value; this.Invalidate(); }
		}
		
		private int _m_ALPHA_TEST;
		[STNodeProperty("ALPHA_TEST", "ALPHA_TEST")]
		public int m_ALPHA_TEST
		{
			get { return _m_ALPHA_TEST; }
			set { _m_ALPHA_TEST = value; this.Invalidate(); }
		}
		
		private int _m_ZTEST;
		[STNodeProperty("ZTEST", "ZTEST")]
		public int m_ZTEST
		{
			get { return _m_ZTEST; }
			set { _m_ZTEST = value; this.Invalidate(); }
		}
		
		private int _m_START_MID_END_SPEED;
		[STNodeProperty("START_MID_END_SPEED", "START_MID_END_SPEED")]
		public int m_START_MID_END_SPEED
		{
			get { return _m_START_MID_END_SPEED; }
			set { _m_START_MID_END_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_SPEED_START_MIN;
		[STNodeProperty("SPEED_START_MIN", "SPEED_START_MIN")]
		public float m_SPEED_START_MIN
		{
			get { return _m_SPEED_START_MIN; }
			set { _m_SPEED_START_MIN = value; this.Invalidate(); }
		}
		
		private float _m_SPEED_START_MAX;
		[STNodeProperty("SPEED_START_MAX", "SPEED_START_MAX")]
		public float m_SPEED_START_MAX
		{
			get { return _m_SPEED_START_MAX; }
			set { _m_SPEED_START_MAX = value; this.Invalidate(); }
		}
		
		private float _m_SPEED_MID_MIN;
		[STNodeProperty("SPEED_MID_MIN", "SPEED_MID_MIN")]
		public float m_SPEED_MID_MIN
		{
			get { return _m_SPEED_MID_MIN; }
			set { _m_SPEED_MID_MIN = value; this.Invalidate(); }
		}
		
		private float _m_SPEED_MID_MAX;
		[STNodeProperty("SPEED_MID_MAX", "SPEED_MID_MAX")]
		public float m_SPEED_MID_MAX
		{
			get { return _m_SPEED_MID_MAX; }
			set { _m_SPEED_MID_MAX = value; this.Invalidate(); }
		}
		
		private float _m_SPEED_END_MIN;
		[STNodeProperty("SPEED_END_MIN", "SPEED_END_MIN")]
		public float m_SPEED_END_MIN
		{
			get { return _m_SPEED_END_MIN; }
			set { _m_SPEED_END_MIN = value; this.Invalidate(); }
		}
		
		private float _m_SPEED_END_MAX;
		[STNodeProperty("SPEED_END_MAX", "SPEED_END_MAX")]
		public float m_SPEED_END_MAX
		{
			get { return _m_SPEED_END_MAX; }
			set { _m_SPEED_END_MAX = value; this.Invalidate(); }
		}
		
		private int _m_LAUNCH_DECELERATE_SPEED;
		[STNodeProperty("LAUNCH_DECELERATE_SPEED", "LAUNCH_DECELERATE_SPEED")]
		public int m_LAUNCH_DECELERATE_SPEED
		{
			get { return _m_LAUNCH_DECELERATE_SPEED; }
			set { _m_LAUNCH_DECELERATE_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_LAUNCH_DECELERATE_SPEED_START_MIN;
		[STNodeProperty("LAUNCH_DECELERATE_SPEED_START_MIN", "LAUNCH_DECELERATE_SPEED_START_MIN")]
		public float m_LAUNCH_DECELERATE_SPEED_START_MIN
		{
			get { return _m_LAUNCH_DECELERATE_SPEED_START_MIN; }
			set { _m_LAUNCH_DECELERATE_SPEED_START_MIN = value; this.Invalidate(); }
		}
		
		private float _m_LAUNCH_DECELERATE_SPEED_START_MAX;
		[STNodeProperty("LAUNCH_DECELERATE_SPEED_START_MAX", "LAUNCH_DECELERATE_SPEED_START_MAX")]
		public float m_LAUNCH_DECELERATE_SPEED_START_MAX
		{
			get { return _m_LAUNCH_DECELERATE_SPEED_START_MAX; }
			set { _m_LAUNCH_DECELERATE_SPEED_START_MAX = value; this.Invalidate(); }
		}
		
		private float _m_LAUNCH_DECELERATE_DEC_RATE;
		[STNodeProperty("LAUNCH_DECELERATE_DEC_RATE", "LAUNCH_DECELERATE_DEC_RATE")]
		public float m_LAUNCH_DECELERATE_DEC_RATE
		{
			get { return _m_LAUNCH_DECELERATE_DEC_RATE; }
			set { _m_LAUNCH_DECELERATE_DEC_RATE = value; this.Invalidate(); }
		}
		
		private int _m_EMISSION_AREA;
		[STNodeProperty("EMISSION_AREA", "EMISSION_AREA")]
		public int m_EMISSION_AREA
		{
			get { return _m_EMISSION_AREA; }
			set { _m_EMISSION_AREA = value; this.Invalidate(); }
		}
		
		private float _m_EMISSION_AREA_X;
		[STNodeProperty("EMISSION_AREA_X", "EMISSION_AREA_X")]
		public float m_EMISSION_AREA_X
		{
			get { return _m_EMISSION_AREA_X; }
			set { _m_EMISSION_AREA_X = value; this.Invalidate(); }
		}
		
		private float _m_EMISSION_AREA_Y;
		[STNodeProperty("EMISSION_AREA_Y", "EMISSION_AREA_Y")]
		public float m_EMISSION_AREA_Y
		{
			get { return _m_EMISSION_AREA_Y; }
			set { _m_EMISSION_AREA_Y = value; this.Invalidate(); }
		}
		
		private float _m_EMISSION_AREA_Z;
		[STNodeProperty("EMISSION_AREA_Z", "EMISSION_AREA_Z")]
		public float m_EMISSION_AREA_Z
		{
			get { return _m_EMISSION_AREA_Z; }
			set { _m_EMISSION_AREA_Z = value; this.Invalidate(); }
		}
		
		private int _m_EMISSION_SURFACE;
		[STNodeProperty("EMISSION_SURFACE", "EMISSION_SURFACE")]
		public int m_EMISSION_SURFACE
		{
			get { return _m_EMISSION_SURFACE; }
			set { _m_EMISSION_SURFACE = value; this.Invalidate(); }
		}
		
		private int _m_EMISSION_DIRECTION_SURFACE;
		[STNodeProperty("EMISSION_DIRECTION_SURFACE", "EMISSION_DIRECTION_SURFACE")]
		public int m_EMISSION_DIRECTION_SURFACE
		{
			get { return _m_EMISSION_DIRECTION_SURFACE; }
			set { _m_EMISSION_DIRECTION_SURFACE = value; this.Invalidate(); }
		}
		
		private int _m_AREA_CUBOID;
		[STNodeProperty("AREA_CUBOID", "AREA_CUBOID")]
		public int m_AREA_CUBOID
		{
			get { return _m_AREA_CUBOID; }
			set { _m_AREA_CUBOID = value; this.Invalidate(); }
		}
		
		private int _m_AREA_SPHEROID;
		[STNodeProperty("AREA_SPHEROID", "AREA_SPHEROID")]
		public int m_AREA_SPHEROID
		{
			get { return _m_AREA_SPHEROID; }
			set { _m_AREA_SPHEROID = value; this.Invalidate(); }
		}
		
		private int _m_AREA_CYLINDER;
		[STNodeProperty("AREA_CYLINDER", "AREA_CYLINDER")]
		public int m_AREA_CYLINDER
		{
			get { return _m_AREA_CYLINDER; }
			set { _m_AREA_CYLINDER = value; this.Invalidate(); }
		}
		
		private float _m_PIVOT_X;
		[STNodeProperty("PIVOT_X", "PIVOT_X")]
		public float m_PIVOT_X
		{
			get { return _m_PIVOT_X; }
			set { _m_PIVOT_X = value; this.Invalidate(); }
		}
		
		private float _m_PIVOT_Y;
		[STNodeProperty("PIVOT_Y", "PIVOT_Y")]
		public float m_PIVOT_Y
		{
			get { return _m_PIVOT_Y; }
			set { _m_PIVOT_Y = value; this.Invalidate(); }
		}
		
		private int _m_GRAVITY;
		[STNodeProperty("GRAVITY", "GRAVITY")]
		public int m_GRAVITY
		{
			get { return _m_GRAVITY; }
			set { _m_GRAVITY = value; this.Invalidate(); }
		}
		
		private float _m_GRAVITY_STRENGTH;
		[STNodeProperty("GRAVITY_STRENGTH", "GRAVITY_STRENGTH")]
		public float m_GRAVITY_STRENGTH
		{
			get { return _m_GRAVITY_STRENGTH; }
			set { _m_GRAVITY_STRENGTH = value; this.Invalidate(); }
		}
		
		private float _m_GRAVITY_MAX_STRENGTH;
		[STNodeProperty("GRAVITY_MAX_STRENGTH", "GRAVITY_MAX_STRENGTH")]
		public float m_GRAVITY_MAX_STRENGTH
		{
			get { return _m_GRAVITY_MAX_STRENGTH; }
			set { _m_GRAVITY_MAX_STRENGTH = value; this.Invalidate(); }
		}
		
		private int _m_COLOUR_TINT;
		[STNodeProperty("COLOUR_TINT", "COLOUR_TINT")]
		public int m_COLOUR_TINT
		{
			get { return _m_COLOUR_TINT; }
			set { _m_COLOUR_TINT = value; this.Invalidate(); }
		}
		
		private cVector3 _m_COLOUR_TINT_START;
		[STNodeProperty("COLOUR_TINT_START", "COLOUR_TINT_START")]
		public cVector3 m_COLOUR_TINT_START
		{
			get { return _m_COLOUR_TINT_START; }
			set { _m_COLOUR_TINT_START = value; this.Invalidate(); }
		}
		
		private cVector3 _m_COLOUR_TINT_END;
		[STNodeProperty("COLOUR_TINT_END", "COLOUR_TINT_END")]
		public cVector3 m_COLOUR_TINT_END
		{
			get { return _m_COLOUR_TINT_END; }
			set { _m_COLOUR_TINT_END = value; this.Invalidate(); }
		}
		
		private int _m_COLOUR_USE_MID;
		[STNodeProperty("COLOUR_USE_MID", "COLOUR_USE_MID")]
		public int m_COLOUR_USE_MID
		{
			get { return _m_COLOUR_USE_MID; }
			set { _m_COLOUR_USE_MID = value; this.Invalidate(); }
		}
		
		private cVector3 _m_COLOUR_TINT_MID;
		[STNodeProperty("COLOUR_TINT_MID", "COLOUR_TINT_MID")]
		public cVector3 m_COLOUR_TINT_MID
		{
			get { return _m_COLOUR_TINT_MID; }
			set { _m_COLOUR_TINT_MID = value; this.Invalidate(); }
		}
		
		private float _m_COLOUR_MIDPOINT;
		[STNodeProperty("COLOUR_MIDPOINT", "COLOUR_MIDPOINT")]
		public float m_COLOUR_MIDPOINT
		{
			get { return _m_COLOUR_MIDPOINT; }
			set { _m_COLOUR_MIDPOINT = value; this.Invalidate(); }
		}
		
		private int _m_SPREAD_FEATURE;
		[STNodeProperty("SPREAD_FEATURE", "SPREAD_FEATURE")]
		public int m_SPREAD_FEATURE
		{
			get { return _m_SPREAD_FEATURE; }
			set { _m_SPREAD_FEATURE = value; this.Invalidate(); }
		}
		
		private float _m_SPREAD_MIN;
		[STNodeProperty("SPREAD_MIN", "SPREAD_MIN")]
		public float m_SPREAD_MIN
		{
			get { return _m_SPREAD_MIN; }
			set { _m_SPREAD_MIN = value; this.Invalidate(); }
		}
		
		private float _m_SPREAD;
		[STNodeProperty("SPREAD", "SPREAD")]
		public float m_SPREAD
		{
			get { return _m_SPREAD; }
			set { _m_SPREAD = value; this.Invalidate(); }
		}
		
		private int _m_ROTATION;
		[STNodeProperty("ROTATION", "ROTATION")]
		public int m_ROTATION
		{
			get { return _m_ROTATION; }
			set { _m_ROTATION = value; this.Invalidate(); }
		}
		
		private float _m_ROTATION_MIN;
		[STNodeProperty("ROTATION_MIN", "ROTATION_MIN")]
		public float m_ROTATION_MIN
		{
			get { return _m_ROTATION_MIN; }
			set { _m_ROTATION_MIN = value; this.Invalidate(); }
		}
		
		private float _m_ROTATION_MAX;
		[STNodeProperty("ROTATION_MAX", "ROTATION_MAX")]
		public float m_ROTATION_MAX
		{
			get { return _m_ROTATION_MAX; }
			set { _m_ROTATION_MAX = value; this.Invalidate(); }
		}
		
		private int _m_ROTATION_RANDOM_START;
		[STNodeProperty("ROTATION_RANDOM_START", "ROTATION_RANDOM_START")]
		public int m_ROTATION_RANDOM_START
		{
			get { return _m_ROTATION_RANDOM_START; }
			set { _m_ROTATION_RANDOM_START = value; this.Invalidate(); }
		}
		
		private float _m_ROTATION_BASE;
		[STNodeProperty("ROTATION_BASE", "ROTATION_BASE")]
		public float m_ROTATION_BASE
		{
			get { return _m_ROTATION_BASE; }
			set { _m_ROTATION_BASE = value; this.Invalidate(); }
		}
		
		private float _m_ROTATION_VAR;
		[STNodeProperty("ROTATION_VAR", "ROTATION_VAR")]
		public float m_ROTATION_VAR
		{
			get { return _m_ROTATION_VAR; }
			set { _m_ROTATION_VAR = value; this.Invalidate(); }
		}
		
		private int _m_ROTATION_RAMP;
		[STNodeProperty("ROTATION_RAMP", "ROTATION_RAMP")]
		public int m_ROTATION_RAMP
		{
			get { return _m_ROTATION_RAMP; }
			set { _m_ROTATION_RAMP = value; this.Invalidate(); }
		}
		
		private float _m_ROTATION_IN;
		[STNodeProperty("ROTATION_IN", "ROTATION_IN")]
		public float m_ROTATION_IN
		{
			get { return _m_ROTATION_IN; }
			set { _m_ROTATION_IN = value; this.Invalidate(); }
		}
		
		private float _m_ROTATION_OUT;
		[STNodeProperty("ROTATION_OUT", "ROTATION_OUT")]
		public float m_ROTATION_OUT
		{
			get { return _m_ROTATION_OUT; }
			set { _m_ROTATION_OUT = value; this.Invalidate(); }
		}
		
		private float _m_ROTATION_DAMP;
		[STNodeProperty("ROTATION_DAMP", "ROTATION_DAMP")]
		public float m_ROTATION_DAMP
		{
			get { return _m_ROTATION_DAMP; }
			set { _m_ROTATION_DAMP = value; this.Invalidate(); }
		}
		
		private int _m_FADE_NEAR_CAMERA;
		[STNodeProperty("FADE_NEAR_CAMERA", "FADE_NEAR_CAMERA")]
		public int m_FADE_NEAR_CAMERA
		{
			get { return _m_FADE_NEAR_CAMERA; }
			set { _m_FADE_NEAR_CAMERA = value; this.Invalidate(); }
		}
		
		private float _m_FADE_NEAR_CAMERA_MAX_DIST;
		[STNodeProperty("FADE_NEAR_CAMERA_MAX_DIST", "FADE_NEAR_CAMERA_MAX_DIST")]
		public float m_FADE_NEAR_CAMERA_MAX_DIST
		{
			get { return _m_FADE_NEAR_CAMERA_MAX_DIST; }
			set { _m_FADE_NEAR_CAMERA_MAX_DIST = value; this.Invalidate(); }
		}
		
		private float _m_FADE_NEAR_CAMERA_THRESHOLD;
		[STNodeProperty("FADE_NEAR_CAMERA_THRESHOLD", "FADE_NEAR_CAMERA_THRESHOLD")]
		public float m_FADE_NEAR_CAMERA_THRESHOLD
		{
			get { return _m_FADE_NEAR_CAMERA_THRESHOLD; }
			set { _m_FADE_NEAR_CAMERA_THRESHOLD = value; this.Invalidate(); }
		}
		
		private int _m_TEXTURE_ANIMATION;
		[STNodeProperty("TEXTURE_ANIMATION", "TEXTURE_ANIMATION")]
		public int m_TEXTURE_ANIMATION
		{
			get { return _m_TEXTURE_ANIMATION; }
			set { _m_TEXTURE_ANIMATION = value; this.Invalidate(); }
		}
		
		private int _m_TEXTURE_ANIMATION_FRAMES;
		[STNodeProperty("TEXTURE_ANIMATION_FRAMES", "TEXTURE_ANIMATION_FRAMES")]
		public int m_TEXTURE_ANIMATION_FRAMES
		{
			get { return _m_TEXTURE_ANIMATION_FRAMES; }
			set { _m_TEXTURE_ANIMATION_FRAMES = value; this.Invalidate(); }
		}
		
		private int _m_NUM_ROWS;
		[STNodeProperty("NUM_ROWS", "NUM_ROWS")]
		public int m_NUM_ROWS
		{
			get { return _m_NUM_ROWS; }
			set { _m_NUM_ROWS = value; this.Invalidate(); }
		}
		
		private float _m_TEXTURE_ANIMATION_LOOP_COUNT;
		[STNodeProperty("TEXTURE_ANIMATION_LOOP_COUNT", "TEXTURE_ANIMATION_LOOP_COUNT")]
		public float m_TEXTURE_ANIMATION_LOOP_COUNT
		{
			get { return _m_TEXTURE_ANIMATION_LOOP_COUNT; }
			set { _m_TEXTURE_ANIMATION_LOOP_COUNT = value; this.Invalidate(); }
		}
		
		private int _m_RANDOM_START_FRAME;
		[STNodeProperty("RANDOM_START_FRAME", "RANDOM_START_FRAME")]
		public int m_RANDOM_START_FRAME
		{
			get { return _m_RANDOM_START_FRAME; }
			set { _m_RANDOM_START_FRAME = value; this.Invalidate(); }
		}
		
		private int _m_WRAP_FRAMES;
		[STNodeProperty("WRAP_FRAMES", "WRAP_FRAMES")]
		public int m_WRAP_FRAMES
		{
			get { return _m_WRAP_FRAMES; }
			set { _m_WRAP_FRAMES = value; this.Invalidate(); }
		}
		
		private int _m_NO_ANIM;
		[STNodeProperty("NO_ANIM", "NO_ANIM")]
		public int m_NO_ANIM
		{
			get { return _m_NO_ANIM; }
			set { _m_NO_ANIM = value; this.Invalidate(); }
		}
		
		private int _m_SUB_FRAME_BLEND;
		[STNodeProperty("SUB_FRAME_BLEND", "SUB_FRAME_BLEND")]
		public int m_SUB_FRAME_BLEND
		{
			get { return _m_SUB_FRAME_BLEND; }
			set { _m_SUB_FRAME_BLEND = value; this.Invalidate(); }
		}
		
		private int _m_SOFTNESS;
		[STNodeProperty("SOFTNESS", "SOFTNESS")]
		public int m_SOFTNESS
		{
			get { return _m_SOFTNESS; }
			set { _m_SOFTNESS = value; this.Invalidate(); }
		}
		
		private float _m_SOFTNESS_EDGE;
		[STNodeProperty("SOFTNESS_EDGE", "SOFTNESS_EDGE")]
		public float m_SOFTNESS_EDGE
		{
			get { return _m_SOFTNESS_EDGE; }
			set { _m_SOFTNESS_EDGE = value; this.Invalidate(); }
		}
		
		private float _m_SOFTNESS_ALPHA_THICKNESS;
		[STNodeProperty("SOFTNESS_ALPHA_THICKNESS", "SOFTNESS_ALPHA_THICKNESS")]
		public float m_SOFTNESS_ALPHA_THICKNESS
		{
			get { return _m_SOFTNESS_ALPHA_THICKNESS; }
			set { _m_SOFTNESS_ALPHA_THICKNESS = value; this.Invalidate(); }
		}
		
		private float _m_SOFTNESS_ALPHA_DEPTH_MODIFIER;
		[STNodeProperty("SOFTNESS_ALPHA_DEPTH_MODIFIER", "SOFTNESS_ALPHA_DEPTH_MODIFIER")]
		public float m_SOFTNESS_ALPHA_DEPTH_MODIFIER
		{
			get { return _m_SOFTNESS_ALPHA_DEPTH_MODIFIER; }
			set { _m_SOFTNESS_ALPHA_DEPTH_MODIFIER = value; this.Invalidate(); }
		}
		
		private int _m_REVERSE_SOFTNESS;
		[STNodeProperty("REVERSE_SOFTNESS", "REVERSE_SOFTNESS")]
		public int m_REVERSE_SOFTNESS
		{
			get { return _m_REVERSE_SOFTNESS; }
			set { _m_REVERSE_SOFTNESS = value; this.Invalidate(); }
		}
		
		private float _m_REVERSE_SOFTNESS_EDGE;
		[STNodeProperty("REVERSE_SOFTNESS_EDGE", "REVERSE_SOFTNESS_EDGE")]
		public float m_REVERSE_SOFTNESS_EDGE
		{
			get { return _m_REVERSE_SOFTNESS_EDGE; }
			set { _m_REVERSE_SOFTNESS_EDGE = value; this.Invalidate(); }
		}
		
		private int _m_PIVOT_AND_TURBULENCE;
		[STNodeProperty("PIVOT_AND_TURBULENCE", "PIVOT_AND_TURBULENCE")]
		public int m_PIVOT_AND_TURBULENCE
		{
			get { return _m_PIVOT_AND_TURBULENCE; }
			set { _m_PIVOT_AND_TURBULENCE = value; this.Invalidate(); }
		}
		
		private float _m_PIVOT_OFFSET_MIN;
		[STNodeProperty("PIVOT_OFFSET_MIN", "PIVOT_OFFSET_MIN")]
		public float m_PIVOT_OFFSET_MIN
		{
			get { return _m_PIVOT_OFFSET_MIN; }
			set { _m_PIVOT_OFFSET_MIN = value; this.Invalidate(); }
		}
		
		private float _m_PIVOT_OFFSET_MAX;
		[STNodeProperty("PIVOT_OFFSET_MAX", "PIVOT_OFFSET_MAX")]
		public float m_PIVOT_OFFSET_MAX
		{
			get { return _m_PIVOT_OFFSET_MAX; }
			set { _m_PIVOT_OFFSET_MAX = value; this.Invalidate(); }
		}
		
		private float _m_TURBULENCE_FREQUENCY_MIN;
		[STNodeProperty("TURBULENCE_FREQUENCY_MIN", "TURBULENCE_FREQUENCY_MIN")]
		public float m_TURBULENCE_FREQUENCY_MIN
		{
			get { return _m_TURBULENCE_FREQUENCY_MIN; }
			set { _m_TURBULENCE_FREQUENCY_MIN = value; this.Invalidate(); }
		}
		
		private float _m_TURBULENCE_FREQUENCY_MAX;
		[STNodeProperty("TURBULENCE_FREQUENCY_MAX", "TURBULENCE_FREQUENCY_MAX")]
		public float m_TURBULENCE_FREQUENCY_MAX
		{
			get { return _m_TURBULENCE_FREQUENCY_MAX; }
			set { _m_TURBULENCE_FREQUENCY_MAX = value; this.Invalidate(); }
		}
		
		private float _m_TURBULENCE_AMOUNT_MIN;
		[STNodeProperty("TURBULENCE_AMOUNT_MIN", "TURBULENCE_AMOUNT_MIN")]
		public float m_TURBULENCE_AMOUNT_MIN
		{
			get { return _m_TURBULENCE_AMOUNT_MIN; }
			set { _m_TURBULENCE_AMOUNT_MIN = value; this.Invalidate(); }
		}
		
		private float _m_TURBULENCE_AMOUNT_MAX;
		[STNodeProperty("TURBULENCE_AMOUNT_MAX", "TURBULENCE_AMOUNT_MAX")]
		public float m_TURBULENCE_AMOUNT_MAX
		{
			get { return _m_TURBULENCE_AMOUNT_MAX; }
			set { _m_TURBULENCE_AMOUNT_MAX = value; this.Invalidate(); }
		}
		
		private int _m_ALPHATHRESHOLD;
		[STNodeProperty("ALPHATHRESHOLD", "ALPHATHRESHOLD")]
		public int m_ALPHATHRESHOLD
		{
			get { return _m_ALPHATHRESHOLD; }
			set { _m_ALPHATHRESHOLD = value; this.Invalidate(); }
		}
		
		private float _m_ALPHATHRESHOLD_TOTALTIME;
		[STNodeProperty("ALPHATHRESHOLD_TOTALTIME", "ALPHATHRESHOLD_TOTALTIME")]
		public float m_ALPHATHRESHOLD_TOTALTIME
		{
			get { return _m_ALPHATHRESHOLD_TOTALTIME; }
			set { _m_ALPHATHRESHOLD_TOTALTIME = value; this.Invalidate(); }
		}
		
		private float _m_ALPHATHRESHOLD_RANGE;
		[STNodeProperty("ALPHATHRESHOLD_RANGE", "ALPHATHRESHOLD_RANGE")]
		public float m_ALPHATHRESHOLD_RANGE
		{
			get { return _m_ALPHATHRESHOLD_RANGE; }
			set { _m_ALPHATHRESHOLD_RANGE = value; this.Invalidate(); }
		}
		
		private float _m_ALPHATHRESHOLD_BEGINSTART;
		[STNodeProperty("ALPHATHRESHOLD_BEGINSTART", "ALPHATHRESHOLD_BEGINSTART")]
		public float m_ALPHATHRESHOLD_BEGINSTART
		{
			get { return _m_ALPHATHRESHOLD_BEGINSTART; }
			set { _m_ALPHATHRESHOLD_BEGINSTART = value; this.Invalidate(); }
		}
		
		private float _m_ALPHATHRESHOLD_BEGINSTOP;
		[STNodeProperty("ALPHATHRESHOLD_BEGINSTOP", "ALPHATHRESHOLD_BEGINSTOP")]
		public float m_ALPHATHRESHOLD_BEGINSTOP
		{
			get { return _m_ALPHATHRESHOLD_BEGINSTOP; }
			set { _m_ALPHATHRESHOLD_BEGINSTOP = value; this.Invalidate(); }
		}
		
		private float _m_ALPHATHRESHOLD_ENDSTART;
		[STNodeProperty("ALPHATHRESHOLD_ENDSTART", "ALPHATHRESHOLD_ENDSTART")]
		public float m_ALPHATHRESHOLD_ENDSTART
		{
			get { return _m_ALPHATHRESHOLD_ENDSTART; }
			set { _m_ALPHATHRESHOLD_ENDSTART = value; this.Invalidate(); }
		}
		
		private float _m_ALPHATHRESHOLD_ENDSTOP;
		[STNodeProperty("ALPHATHRESHOLD_ENDSTOP", "ALPHATHRESHOLD_ENDSTOP")]
		public float m_ALPHATHRESHOLD_ENDSTOP
		{
			get { return _m_ALPHATHRESHOLD_ENDSTOP; }
			set { _m_ALPHATHRESHOLD_ENDSTOP = value; this.Invalidate(); }
		}
		
		private int _m_COLOUR_RAMP;
		[STNodeProperty("COLOUR_RAMP", "COLOUR_RAMP")]
		public int m_COLOUR_RAMP
		{
			get { return _m_COLOUR_RAMP; }
			set { _m_COLOUR_RAMP = value; this.Invalidate(); }
		}
		
		private string _m_COLOUR_RAMP_MAP;
		[STNodeProperty("COLOUR_RAMP_MAP", "COLOUR_RAMP_MAP")]
		public string m_COLOUR_RAMP_MAP
		{
			get { return _m_COLOUR_RAMP_MAP; }
			set { _m_COLOUR_RAMP_MAP = value; this.Invalidate(); }
		}
		
		private int _m_COLOUR_RAMP_ALPHA;
		[STNodeProperty("COLOUR_RAMP_ALPHA", "COLOUR_RAMP_ALPHA")]
		public int m_COLOUR_RAMP_ALPHA
		{
			get { return _m_COLOUR_RAMP_ALPHA; }
			set { _m_COLOUR_RAMP_ALPHA = value; this.Invalidate(); }
		}
		
		private int _m_DEPTH_FADE_AXIS;
		[STNodeProperty("DEPTH_FADE_AXIS", "DEPTH_FADE_AXIS")]
		public int m_DEPTH_FADE_AXIS
		{
			get { return _m_DEPTH_FADE_AXIS; }
			set { _m_DEPTH_FADE_AXIS = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_FADE_AXIS_DIST;
		[STNodeProperty("DEPTH_FADE_AXIS_DIST", "DEPTH_FADE_AXIS_DIST")]
		public float m_DEPTH_FADE_AXIS_DIST
		{
			get { return _m_DEPTH_FADE_AXIS_DIST; }
			set { _m_DEPTH_FADE_AXIS_DIST = value; this.Invalidate(); }
		}
		
		private float _m_DEPTH_FADE_AXIS_PERCENT;
		[STNodeProperty("DEPTH_FADE_AXIS_PERCENT", "DEPTH_FADE_AXIS_PERCENT")]
		public float m_DEPTH_FADE_AXIS_PERCENT
		{
			get { return _m_DEPTH_FADE_AXIS_PERCENT; }
			set { _m_DEPTH_FADE_AXIS_PERCENT = value; this.Invalidate(); }
		}
		
		private int _m_FLOW_UV_ANIMATION;
		[STNodeProperty("FLOW_UV_ANIMATION", "FLOW_UV_ANIMATION")]
		public int m_FLOW_UV_ANIMATION
		{
			get { return _m_FLOW_UV_ANIMATION; }
			set { _m_FLOW_UV_ANIMATION = value; this.Invalidate(); }
		}
		
		private string _m_FLOW_MAP;
		[STNodeProperty("FLOW_MAP", "FLOW_MAP")]
		public string m_FLOW_MAP
		{
			get { return _m_FLOW_MAP; }
			set { _m_FLOW_MAP = value; this.Invalidate(); }
		}
		
		private string _m_FLOW_TEXTURE_MAP;
		[STNodeProperty("FLOW_TEXTURE_MAP", "FLOW_TEXTURE_MAP")]
		public string m_FLOW_TEXTURE_MAP
		{
			get { return _m_FLOW_TEXTURE_MAP; }
			set { _m_FLOW_TEXTURE_MAP = value; this.Invalidate(); }
		}
		
		private float _m_CYCLE_TIME;
		[STNodeProperty("CYCLE_TIME", "CYCLE_TIME")]
		public float m_CYCLE_TIME
		{
			get { return _m_CYCLE_TIME; }
			set { _m_CYCLE_TIME = value; this.Invalidate(); }
		}
		
		private float _m_FLOW_SPEED;
		[STNodeProperty("FLOW_SPEED", "FLOW_SPEED")]
		public float m_FLOW_SPEED
		{
			get { return _m_FLOW_SPEED; }
			set { _m_FLOW_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_FLOW_TEX_SCALE;
		[STNodeProperty("FLOW_TEX_SCALE", "FLOW_TEX_SCALE")]
		public float m_FLOW_TEX_SCALE
		{
			get { return _m_FLOW_TEX_SCALE; }
			set { _m_FLOW_TEX_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_FLOW_WARP_STRENGTH;
		[STNodeProperty("FLOW_WARP_STRENGTH", "FLOW_WARP_STRENGTH")]
		public float m_FLOW_WARP_STRENGTH
		{
			get { return _m_FLOW_WARP_STRENGTH; }
			set { _m_FLOW_WARP_STRENGTH = value; this.Invalidate(); }
		}
		
		private int _m_INFINITE_PROJECTION;
		[STNodeProperty("INFINITE_PROJECTION", "INFINITE_PROJECTION")]
		public int m_INFINITE_PROJECTION
		{
			get { return _m_INFINITE_PROJECTION; }
			set { _m_INFINITE_PROJECTION = value; this.Invalidate(); }
		}
		
		private cVector3 _m_PARALLAX_POSITION;
		[STNodeProperty("PARALLAX_POSITION", "PARALLAX_POSITION")]
		public cVector3 m_PARALLAX_POSITION
		{
			get { return _m_PARALLAX_POSITION; }
			set { _m_PARALLAX_POSITION = value; this.Invalidate(); }
		}
		
		private int _m_DISTORTION_OCCLUSION;
		[STNodeProperty("DISTORTION_OCCLUSION", "DISTORTION_OCCLUSION")]
		public int m_DISTORTION_OCCLUSION
		{
			get { return _m_DISTORTION_OCCLUSION; }
			set { _m_DISTORTION_OCCLUSION = value; this.Invalidate(); }
		}
		
		private int _m_AMBIENT_LIGHTING;
		[STNodeProperty("AMBIENT_LIGHTING", "AMBIENT_LIGHTING")]
		public int m_AMBIENT_LIGHTING
		{
			get { return _m_AMBIENT_LIGHTING; }
			set { _m_AMBIENT_LIGHTING = value; this.Invalidate(); }
		}
		
		private cVector3 _m_AMBIENT_LIGHTING_COLOUR;
		[STNodeProperty("AMBIENT_LIGHTING_COLOUR", "AMBIENT_LIGHTING_COLOUR")]
		public cVector3 m_AMBIENT_LIGHTING_COLOUR
		{
			get { return _m_AMBIENT_LIGHTING_COLOUR; }
			set { _m_AMBIENT_LIGHTING_COLOUR = value; this.Invalidate(); }
		}
		
		private int _m_NO_CLIP;
		[STNodeProperty("NO_CLIP", "NO_CLIP")]
		public int m_NO_CLIP
		{
			get { return _m_NO_CLIP; }
			set { _m_NO_CLIP = value; this.Invalidate(); }
		}
		
		private bool _m_pause_on_reset;
		[STNodeProperty("pause_on_reset", "pause_on_reset")]
		public bool m_pause_on_reset
		{
			get { return _m_pause_on_reset; }
			set { _m_pause_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "ParticleEmitterReference";
			
			this.InputOptions.Add("mastered_by_visibility", typeof(STNode), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			this.InputOptions.Add("terminate", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
			this.OutputOptions.Add("terminated", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
