using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RibbonEmitterReference : STNode
	{
		private bool _m_deleted;
		[STNodeProperty("deleted", "deleted")]
		public bool m_deleted
		{
			get { return _m_deleted; }
			set { _m_deleted = value; this.Invalidate(); }
		}
		
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
		
		private int _m_NO_MIPS;
		[STNodeProperty("NO_MIPS", "NO_MIPS")]
		public int m_NO_MIPS
		{
			get { return _m_NO_MIPS; }
			set { _m_NO_MIPS = value; this.Invalidate(); }
		}
		
		private int _m_UV_SQUARED;
		[STNodeProperty("UV_SQUARED", "UV_SQUARED")]
		public int m_UV_SQUARED
		{
			get { return _m_UV_SQUARED; }
			set { _m_UV_SQUARED = value; this.Invalidate(); }
		}
		
		private int _m_LOW_RES;
		[STNodeProperty("LOW_RES", "LOW_RES")]
		public int m_LOW_RES
		{
			get { return _m_LOW_RES; }
			set { _m_LOW_RES = value; this.Invalidate(); }
		}
		
		private int _m_LIGHTING;
		[STNodeProperty("LIGHTING", "LIGHTING")]
		public int m_LIGHTING
		{
			get { return _m_LIGHTING; }
			set { _m_LIGHTING = value; this.Invalidate(); }
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
		
		private int _m_DRAW_PASS;
		[STNodeProperty("DRAW_PASS", "DRAW_PASS")]
		public int m_DRAW_PASS
		{
			get { return _m_DRAW_PASS; }
			set { _m_DRAW_PASS = value; this.Invalidate(); }
		}
		
		private float _m_SYSTEM_EXPIRY_TIME;
		[STNodeProperty("SYSTEM_EXPIRY_TIME", "SYSTEM_EXPIRY_TIME")]
		public float m_SYSTEM_EXPIRY_TIME
		{
			get { return _m_SYSTEM_EXPIRY_TIME; }
			set { _m_SYSTEM_EXPIRY_TIME = value; this.Invalidate(); }
		}
		
		private float _m_LIFETIME;
		[STNodeProperty("LIFETIME", "LIFETIME")]
		public float m_LIFETIME
		{
			get { return _m_LIFETIME; }
			set { _m_LIFETIME = value; this.Invalidate(); }
		}
		
		private int _m_SMOOTHED;
		[STNodeProperty("SMOOTHED", "SMOOTHED")]
		public int m_SMOOTHED
		{
			get { return _m_SMOOTHED; }
			set { _m_SMOOTHED = value; this.Invalidate(); }
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
		
		private int _m_TEXTURE;
		[STNodeProperty("TEXTURE", "TEXTURE")]
		public int m_TEXTURE
		{
			get { return _m_TEXTURE; }
			set { _m_TEXTURE = value; this.Invalidate(); }
		}
		
		private string _m_TEXTURE_MAP;
		[STNodeProperty("TEXTURE_MAP", "TEXTURE_MAP")]
		public string m_TEXTURE_MAP
		{
			get { return _m_TEXTURE_MAP; }
			set { _m_TEXTURE_MAP = value; this.Invalidate(); }
		}
		
		private float _m_UV_REPEAT;
		[STNodeProperty("UV_REPEAT", "UV_REPEAT")]
		public float m_UV_REPEAT
		{
			get { return _m_UV_REPEAT; }
			set { _m_UV_REPEAT = value; this.Invalidate(); }
		}
		
		private float _m_UV_SCROLLSPEED;
		[STNodeProperty("UV_SCROLLSPEED", "UV_SCROLLSPEED")]
		public float m_UV_SCROLLSPEED
		{
			get { return _m_UV_SCROLLSPEED; }
			set { _m_UV_SCROLLSPEED = value; this.Invalidate(); }
		}
		
		private int _m_MULTI_TEXTURE;
		[STNodeProperty("MULTI_TEXTURE", "MULTI_TEXTURE")]
		public int m_MULTI_TEXTURE
		{
			get { return _m_MULTI_TEXTURE; }
			set { _m_MULTI_TEXTURE = value; this.Invalidate(); }
		}
		
		private float _m_U2_SCALE;
		[STNodeProperty("U2_SCALE", "U2_SCALE")]
		public float m_U2_SCALE
		{
			get { return _m_U2_SCALE; }
			set { _m_U2_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_V2_REPEAT;
		[STNodeProperty("V2_REPEAT", "V2_REPEAT")]
		public float m_V2_REPEAT
		{
			get { return _m_V2_REPEAT; }
			set { _m_V2_REPEAT = value; this.Invalidate(); }
		}
		
		private float _m_V2_SCROLLSPEED;
		[STNodeProperty("V2_SCROLLSPEED", "V2_SCROLLSPEED")]
		public float m_V2_SCROLLSPEED
		{
			get { return _m_V2_SCROLLSPEED; }
			set { _m_V2_SCROLLSPEED = value; this.Invalidate(); }
		}
		
		private int _m_MULTI_TEXTURE_BLEND;
		[STNodeProperty("MULTI_TEXTURE_BLEND", "MULTI_TEXTURE_BLEND")]
		public int m_MULTI_TEXTURE_BLEND
		{
			get { return _m_MULTI_TEXTURE_BLEND; }
			set { _m_MULTI_TEXTURE_BLEND = value; this.Invalidate(); }
		}
		
		private int _m_MULTI_TEXTURE_ADD;
		[STNodeProperty("MULTI_TEXTURE_ADD", "MULTI_TEXTURE_ADD")]
		public int m_MULTI_TEXTURE_ADD
		{
			get { return _m_MULTI_TEXTURE_ADD; }
			set { _m_MULTI_TEXTURE_ADD = value; this.Invalidate(); }
		}
		
		private int _m_MULTI_TEXTURE_MULT;
		[STNodeProperty("MULTI_TEXTURE_MULT", "MULTI_TEXTURE_MULT")]
		public int m_MULTI_TEXTURE_MULT
		{
			get { return _m_MULTI_TEXTURE_MULT; }
			set { _m_MULTI_TEXTURE_MULT = value; this.Invalidate(); }
		}
		
		private int _m_MULTI_TEXTURE_MAX;
		[STNodeProperty("MULTI_TEXTURE_MAX", "MULTI_TEXTURE_MAX")]
		public int m_MULTI_TEXTURE_MAX
		{
			get { return _m_MULTI_TEXTURE_MAX; }
			set { _m_MULTI_TEXTURE_MAX = value; this.Invalidate(); }
		}
		
		private int _m_MULTI_TEXTURE_MIN;
		[STNodeProperty("MULTI_TEXTURE_MIN", "MULTI_TEXTURE_MIN")]
		public int m_MULTI_TEXTURE_MIN
		{
			get { return _m_MULTI_TEXTURE_MIN; }
			set { _m_MULTI_TEXTURE_MIN = value; this.Invalidate(); }
		}
		
		private int _m_SECOND_TEXTURE;
		[STNodeProperty("SECOND_TEXTURE", "SECOND_TEXTURE")]
		public int m_SECOND_TEXTURE
		{
			get { return _m_SECOND_TEXTURE; }
			set { _m_SECOND_TEXTURE = value; this.Invalidate(); }
		}
		
		private string _m_TEXTURE_MAP2;
		[STNodeProperty("TEXTURE_MAP2", "TEXTURE_MAP2")]
		public string m_TEXTURE_MAP2
		{
			get { return _m_TEXTURE_MAP2; }
			set { _m_TEXTURE_MAP2 = value; this.Invalidate(); }
		}
		
		private int _m_CONTINUOUS;
		[STNodeProperty("CONTINUOUS", "CONTINUOUS")]
		public int m_CONTINUOUS
		{
			get { return _m_CONTINUOUS; }
			set { _m_CONTINUOUS = value; this.Invalidate(); }
		}
		
		private int _m_BASE_LOCKED;
		[STNodeProperty("BASE_LOCKED", "BASE_LOCKED")]
		public int m_BASE_LOCKED
		{
			get { return _m_BASE_LOCKED; }
			set { _m_BASE_LOCKED = value; this.Invalidate(); }
		}
		
		private float _m_SPAWN_RATE;
		[STNodeProperty("SPAWN_RATE", "SPAWN_RATE")]
		public float m_SPAWN_RATE
		{
			get { return _m_SPAWN_RATE; }
			set { _m_SPAWN_RATE = value; this.Invalidate(); }
		}
		
		private int _m_TRAILING;
		[STNodeProperty("TRAILING", "TRAILING")]
		public int m_TRAILING
		{
			get { return _m_TRAILING; }
			set { _m_TRAILING = value; this.Invalidate(); }
		}
		
		private int _m_INSTANT;
		[STNodeProperty("INSTANT", "INSTANT")]
		public int m_INSTANT
		{
			get { return _m_INSTANT; }
			set { _m_INSTANT = value; this.Invalidate(); }
		}
		
		private int _m_RATE;
		[STNodeProperty("RATE", "RATE")]
		public int m_RATE
		{
			get { return _m_RATE; }
			set { _m_RATE = value; this.Invalidate(); }
		}
		
		private float _m_TRAIL_SPAWN_RATE;
		[STNodeProperty("TRAIL_SPAWN_RATE", "TRAIL_SPAWN_RATE")]
		public float m_TRAIL_SPAWN_RATE
		{
			get { return _m_TRAIL_SPAWN_RATE; }
			set { _m_TRAIL_SPAWN_RATE = value; this.Invalidate(); }
		}
		
		private float _m_TRAIL_DELAY;
		[STNodeProperty("TRAIL_DELAY", "TRAIL_DELAY")]
		public float m_TRAIL_DELAY
		{
			get { return _m_TRAIL_DELAY; }
			set { _m_TRAIL_DELAY = value; this.Invalidate(); }
		}
		
		private float _m_MAX_TRAILS;
		[STNodeProperty("MAX_TRAILS", "MAX_TRAILS")]
		public float m_MAX_TRAILS
		{
			get { return _m_MAX_TRAILS; }
			set { _m_MAX_TRAILS = value; this.Invalidate(); }
		}
		
		private int _m_POINT_TO_POINT;
		[STNodeProperty("POINT_TO_POINT", "POINT_TO_POINT")]
		public int m_POINT_TO_POINT
		{
			get { return _m_POINT_TO_POINT; }
			set { _m_POINT_TO_POINT = value; this.Invalidate(); }
		}
		
		private cVector3 _m_TARGET_POINT_POSITION;
		[STNodeProperty("TARGET_POINT_POSITION", "TARGET_POINT_POSITION")]
		public cVector3 m_TARGET_POINT_POSITION
		{
			get { return _m_TARGET_POINT_POSITION; }
			set { _m_TARGET_POINT_POSITION = value; this.Invalidate(); }
		}
		
		private float _m_DENSITY;
		[STNodeProperty("DENSITY", "DENSITY")]
		public float m_DENSITY
		{
			get { return _m_DENSITY; }
			set { _m_DENSITY = value; this.Invalidate(); }
		}
		
		private float _m_ABS_FADE_IN_0;
		[STNodeProperty("ABS_FADE_IN_0", "ABS_FADE_IN_0")]
		public float m_ABS_FADE_IN_0
		{
			get { return _m_ABS_FADE_IN_0; }
			set { _m_ABS_FADE_IN_0 = value; this.Invalidate(); }
		}
		
		private float _m_ABS_FADE_IN_1;
		[STNodeProperty("ABS_FADE_IN_1", "ABS_FADE_IN_1")]
		public float m_ABS_FADE_IN_1
		{
			get { return _m_ABS_FADE_IN_1; }
			set { _m_ABS_FADE_IN_1 = value; this.Invalidate(); }
		}
		
		private int _m_FORCES;
		[STNodeProperty("FORCES", "FORCES")]
		public int m_FORCES
		{
			get { return _m_FORCES; }
			set { _m_FORCES = value; this.Invalidate(); }
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
		
		private float _m_DRAG_STRENGTH;
		[STNodeProperty("DRAG_STRENGTH", "DRAG_STRENGTH")]
		public float m_DRAG_STRENGTH
		{
			get { return _m_DRAG_STRENGTH; }
			set { _m_DRAG_STRENGTH = value; this.Invalidate(); }
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
		
		private int _m_WIDTH;
		[STNodeProperty("WIDTH", "WIDTH")]
		public int m_WIDTH
		{
			get { return _m_WIDTH; }
			set { _m_WIDTH = value; this.Invalidate(); }
		}
		
		private float _m_WIDTH_START;
		[STNodeProperty("WIDTH_START", "WIDTH_START")]
		public float m_WIDTH_START
		{
			get { return _m_WIDTH_START; }
			set { _m_WIDTH_START = value; this.Invalidate(); }
		}
		
		private float _m_WIDTH_MID;
		[STNodeProperty("WIDTH_MID", "WIDTH_MID")]
		public float m_WIDTH_MID
		{
			get { return _m_WIDTH_MID; }
			set { _m_WIDTH_MID = value; this.Invalidate(); }
		}
		
		private float _m_WIDTH_END;
		[STNodeProperty("WIDTH_END", "WIDTH_END")]
		public float m_WIDTH_END
		{
			get { return _m_WIDTH_END; }
			set { _m_WIDTH_END = value; this.Invalidate(); }
		}
		
		private float _m_WIDTH_IN;
		[STNodeProperty("WIDTH_IN", "WIDTH_IN")]
		public float m_WIDTH_IN
		{
			get { return _m_WIDTH_IN; }
			set { _m_WIDTH_IN = value; this.Invalidate(); }
		}
		
		private float _m_WIDTH_OUT;
		[STNodeProperty("WIDTH_OUT", "WIDTH_OUT")]
		public float m_WIDTH_OUT
		{
			get { return _m_WIDTH_OUT; }
			set { _m_WIDTH_OUT = value; this.Invalidate(); }
		}
		
		private int _m_COLOUR_TINT;
		[STNodeProperty("COLOUR_TINT", "COLOUR_TINT")]
		public int m_COLOUR_TINT
		{
			get { return _m_COLOUR_TINT; }
			set { _m_COLOUR_TINT = value; this.Invalidate(); }
		}
		
		private float _m_COLOUR_SCALE_START;
		[STNodeProperty("COLOUR_SCALE_START", "COLOUR_SCALE_START")]
		public float m_COLOUR_SCALE_START
		{
			get { return _m_COLOUR_SCALE_START; }
			set { _m_COLOUR_SCALE_START = value; this.Invalidate(); }
		}
		
		private float _m_COLOUR_SCALE_MID;
		[STNodeProperty("COLOUR_SCALE_MID", "COLOUR_SCALE_MID")]
		public float m_COLOUR_SCALE_MID
		{
			get { return _m_COLOUR_SCALE_MID; }
			set { _m_COLOUR_SCALE_MID = value; this.Invalidate(); }
		}
		
		private float _m_COLOUR_SCALE_END;
		[STNodeProperty("COLOUR_SCALE_END", "COLOUR_SCALE_END")]
		public float m_COLOUR_SCALE_END
		{
			get { return _m_COLOUR_SCALE_END; }
			set { _m_COLOUR_SCALE_END = value; this.Invalidate(); }
		}
		
		private cVector3 _m_COLOUR_TINT_START;
		[STNodeProperty("COLOUR_TINT_START", "COLOUR_TINT_START")]
		public cVector3 m_COLOUR_TINT_START
		{
			get { return _m_COLOUR_TINT_START; }
			set { _m_COLOUR_TINT_START = value; this.Invalidate(); }
		}
		
		private cVector3 _m_COLOUR_TINT_MID;
		[STNodeProperty("COLOUR_TINT_MID", "COLOUR_TINT_MID")]
		public cVector3 m_COLOUR_TINT_MID
		{
			get { return _m_COLOUR_TINT_MID; }
			set { _m_COLOUR_TINT_MID = value; this.Invalidate(); }
		}
		
		private cVector3 _m_COLOUR_TINT_END;
		[STNodeProperty("COLOUR_TINT_END", "COLOUR_TINT_END")]
		public cVector3 m_COLOUR_TINT_END
		{
			get { return _m_COLOUR_TINT_END; }
			set { _m_COLOUR_TINT_END = value; this.Invalidate(); }
		}
		
		private int _m_ALPHA_FADE;
		[STNodeProperty("ALPHA_FADE", "ALPHA_FADE")]
		public int m_ALPHA_FADE
		{
			get { return _m_ALPHA_FADE; }
			set { _m_ALPHA_FADE = value; this.Invalidate(); }
		}
		
		private float _m_FADE_IN;
		[STNodeProperty("FADE_IN", "FADE_IN")]
		public float m_FADE_IN
		{
			get { return _m_FADE_IN; }
			set { _m_FADE_IN = value; this.Invalidate(); }
		}
		
		private float _m_FADE_OUT;
		[STNodeProperty("FADE_OUT", "FADE_OUT")]
		public float m_FADE_OUT
		{
			get { return _m_FADE_OUT; }
			set { _m_FADE_OUT = value; this.Invalidate(); }
		}
		
		private int _m_EDGE_FADE;
		[STNodeProperty("EDGE_FADE", "EDGE_FADE")]
		public int m_EDGE_FADE
		{
			get { return _m_EDGE_FADE; }
			set { _m_EDGE_FADE = value; this.Invalidate(); }
		}
		
		private int _m_ALPHA_ERODE;
		[STNodeProperty("ALPHA_ERODE", "ALPHA_ERODE")]
		public int m_ALPHA_ERODE
		{
			get { return _m_ALPHA_ERODE; }
			set { _m_ALPHA_ERODE = value; this.Invalidate(); }
		}
		
		private int _m_SIDE_ON_FADE;
		[STNodeProperty("SIDE_ON_FADE", "SIDE_ON_FADE")]
		public int m_SIDE_ON_FADE
		{
			get { return _m_SIDE_ON_FADE; }
			set { _m_SIDE_ON_FADE = value; this.Invalidate(); }
		}
		
		private float _m_SIDE_FADE_START;
		[STNodeProperty("SIDE_FADE_START", "SIDE_FADE_START")]
		public float m_SIDE_FADE_START
		{
			get { return _m_SIDE_FADE_START; }
			set { _m_SIDE_FADE_START = value; this.Invalidate(); }
		}
		
		private float _m_SIDE_FADE_END;
		[STNodeProperty("SIDE_FADE_END", "SIDE_FADE_END")]
		public float m_SIDE_FADE_END
		{
			get { return _m_SIDE_FADE_END; }
			set { _m_SIDE_FADE_END = value; this.Invalidate(); }
		}
		
		private int _m_DISTANCE_SCALING;
		[STNodeProperty("DISTANCE_SCALING", "DISTANCE_SCALING")]
		public int m_DISTANCE_SCALING
		{
			get { return _m_DISTANCE_SCALING; }
			set { _m_DISTANCE_SCALING = value; this.Invalidate(); }
		}
		
		private float _m_DIST_SCALE;
		[STNodeProperty("DIST_SCALE", "DIST_SCALE")]
		public float m_DIST_SCALE
		{
			get { return _m_DIST_SCALE; }
			set { _m_DIST_SCALE = value; this.Invalidate(); }
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
			
			this.Title = "RibbonEmitterReference";
			
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
