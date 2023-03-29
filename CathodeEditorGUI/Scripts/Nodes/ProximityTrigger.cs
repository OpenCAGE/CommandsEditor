#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ProximityTrigger : STNode
	{
		private float _m_fire_spread_rate;
		[STNodeProperty("fire_spread_rate", "fire_spread_rate")]
		public float m_fire_spread_rate
		{
			get { return _m_fire_spread_rate; }
			set { _m_fire_spread_rate = value; this.Invalidate(); }
		}
		
		private float _m_water_permeate_rate;
		[STNodeProperty("water_permeate_rate", "water_permeate_rate")]
		public float m_water_permeate_rate
		{
			get { return _m_water_permeate_rate; }
			set { _m_water_permeate_rate = value; this.Invalidate(); }
		}
		
		private float _m_electrical_conduction_rate;
		[STNodeProperty("electrical_conduction_rate", "electrical_conduction_rate")]
		public float m_electrical_conduction_rate
		{
			get { return _m_electrical_conduction_rate; }
			set { _m_electrical_conduction_rate = value; this.Invalidate(); }
		}
		
		private float _m_gas_diffusion_rate;
		[STNodeProperty("gas_diffusion_rate", "gas_diffusion_rate")]
		public float m_gas_diffusion_rate
		{
			get { return _m_gas_diffusion_rate; }
			set { _m_gas_diffusion_rate = value; this.Invalidate(); }
		}
		
		private float _m_ignition_range;
		[STNodeProperty("ignition_range", "ignition_range")]
		public float m_ignition_range
		{
			get { return _m_ignition_range; }
			set { _m_ignition_range = value; this.Invalidate(); }
		}
		
		private float _m_electrical_arc_range;
		[STNodeProperty("electrical_arc_range", "electrical_arc_range")]
		public float m_electrical_arc_range
		{
			get { return _m_electrical_arc_range; }
			set { _m_electrical_arc_range = value; this.Invalidate(); }
		}
		
		private float _m_water_flow_range;
		[STNodeProperty("water_flow_range", "water_flow_range")]
		public float m_water_flow_range
		{
			get { return _m_water_flow_range; }
			set { _m_water_flow_range = value; this.Invalidate(); }
		}
		
		private float _m_gas_dispersion_range;
		[STNodeProperty("gas_dispersion_range", "gas_dispersion_range")]
		public float m_gas_dispersion_range
		{
			get { return _m_gas_dispersion_range; }
			set { _m_gas_dispersion_range = value; this.Invalidate(); }
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
			
			this.Title = "ProximityTrigger";
			
			this.InputOptions.Add("ignite", typeof(void), false);
			this.InputOptions.Add("electrify", typeof(void), false);
			this.InputOptions.Add("drench", typeof(void), false);
			this.InputOptions.Add("poison", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("ignited", typeof(void), false);
			this.OutputOptions.Add("electrified", typeof(void), false);
			this.OutputOptions.Add("drenched", typeof(void), false);
			this.OutputOptions.Add("poisoned", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
