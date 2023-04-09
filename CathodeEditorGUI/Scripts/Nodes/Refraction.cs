using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Refraction : STNode
	{
		private float _m_SCALE_X;
		[STNodeProperty("SCALE_X", "SCALE_X")]
		public float m_SCALE_X
		{
			get { return _m_SCALE_X; }
			set { _m_SCALE_X = value; this.Invalidate(); }
		}
		
		private float _m_SCALE_Z;
		[STNodeProperty("SCALE_Z", "SCALE_Z")]
		public float m_SCALE_Z
		{
			get { return _m_SCALE_Z; }
			set { _m_SCALE_Z = value; this.Invalidate(); }
		}
		
		private float _m_DISTANCEFACTOR;
		[STNodeProperty("DISTANCEFACTOR", "DISTANCEFACTOR")]
		public float m_DISTANCEFACTOR
		{
			get { return _m_DISTANCEFACTOR; }
			set { _m_DISTANCEFACTOR = value; this.Invalidate(); }
		}
		
		private float _m_REFRACTFACTOR;
		[STNodeProperty("REFRACTFACTOR", "REFRACTFACTOR")]
		public float m_REFRACTFACTOR
		{
			get { return _m_REFRACTFACTOR; }
			set { _m_REFRACTFACTOR = value; this.Invalidate(); }
		}
		
		private float _m_SPEED;
		[STNodeProperty("SPEED", "SPEED")]
		public float m_SPEED
		{
			get { return _m_SPEED; }
			set { _m_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_SCALE;
		[STNodeProperty("SCALE", "SCALE")]
		public float m_SCALE
		{
			get { return _m_SCALE; }
			set { _m_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_SECONDARY_REFRACTFACTOR;
		[STNodeProperty("SECONDARY_REFRACTFACTOR", "SECONDARY_REFRACTFACTOR")]
		public float m_SECONDARY_REFRACTFACTOR
		{
			get { return _m_SECONDARY_REFRACTFACTOR; }
			set { _m_SECONDARY_REFRACTFACTOR = value; this.Invalidate(); }
		}
		
		private float _m_SECONDARY_SPEED;
		[STNodeProperty("SECONDARY_SPEED", "SECONDARY_SPEED")]
		public float m_SECONDARY_SPEED
		{
			get { return _m_SECONDARY_SPEED; }
			set { _m_SECONDARY_SPEED = value; this.Invalidate(); }
		}
		
		private float _m_SECONDARY_SCALE;
		[STNodeProperty("SECONDARY_SCALE", "SECONDARY_SCALE")]
		public float m_SECONDARY_SCALE
		{
			get { return _m_SECONDARY_SCALE; }
			set { _m_SECONDARY_SCALE = value; this.Invalidate(); }
		}
		
		private float _m_MIN_OCCLUSION_DISTANCE;
		[STNodeProperty("MIN_OCCLUSION_DISTANCE", "MIN_OCCLUSION_DISTANCE")]
		public float m_MIN_OCCLUSION_DISTANCE
		{
			get { return _m_MIN_OCCLUSION_DISTANCE; }
			set { _m_MIN_OCCLUSION_DISTANCE = value; this.Invalidate(); }
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
		
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_pause_on_reset;
		[STNodeProperty("pause_on_reset", "pause_on_reset")]
		public bool m_pause_on_reset
		{
			get { return _m_pause_on_reset; }
			set { _m_pause_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "Refraction";
			
			this.InputOptions.Add("refraction_resource", typeof(STNode), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
