#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GCIP_WorldPickup : STNode
	{
		private bool _m_pipe;
		[STNodeProperty("Pipe", "Pipe")]
		public bool m_pipe
		{
			get { return _m_pipe; }
			set { _m_pipe = value; this.Invalidate(); }
		}
		
		private bool _m_gasoline;
		[STNodeProperty("Gasoline", "Gasoline")]
		public bool m_gasoline
		{
			get { return _m_gasoline; }
			set { _m_gasoline = value; this.Invalidate(); }
		}
		
		private bool _m_explosive;
		[STNodeProperty("Explosive", "Explosive")]
		public bool m_explosive
		{
			get { return _m_explosive; }
			set { _m_explosive = value; this.Invalidate(); }
		}
		
		private bool _m_battery;
		[STNodeProperty("Battery", "Battery")]
		public bool m_battery
		{
			get { return _m_battery; }
			set { _m_battery = value; this.Invalidate(); }
		}
		
		private bool _m_blade;
		[STNodeProperty("Blade", "Blade")]
		public bool m_blade
		{
			get { return _m_blade; }
			set { _m_blade = value; this.Invalidate(); }
		}
		
		private bool _m_gel;
		[STNodeProperty("Gel", "Gel")]
		public bool m_gel
		{
			get { return _m_gel; }
			set { _m_gel = value; this.Invalidate(); }
		}
		
		private bool _m_adhesive;
		[STNodeProperty("Adhesive", "Adhesive")]
		public bool m_adhesive
		{
			get { return _m_adhesive; }
			set { _m_adhesive = value; this.Invalidate(); }
		}
		
		private bool _m_boltgun_ammo;
		[STNodeProperty("BoltGun Ammo", "BoltGun Ammo")]
		public bool m_boltgun_ammo
		{
			get { return _m_boltgun_ammo; }
			set { _m_boltgun_ammo = value; this.Invalidate(); }
		}
		
		private bool _m_revolver_ammo;
		[STNodeProperty("Revolver Ammo", "Revolver Ammo")]
		public bool m_revolver_ammo
		{
			get { return _m_revolver_ammo; }
			set { _m_revolver_ammo = value; this.Invalidate(); }
		}
		
		private bool _m_shotgun_ammo;
		[STNodeProperty("Shotgun Ammo", "Shotgun Ammo")]
		public bool m_shotgun_ammo
		{
			get { return _m_shotgun_ammo; }
			set { _m_shotgun_ammo = value; this.Invalidate(); }
		}
		
		private bool _m_boltgun;
		[STNodeProperty("BoltGun", "BoltGun")]
		public bool m_boltgun
		{
			get { return _m_boltgun; }
			set { _m_boltgun = value; this.Invalidate(); }
		}
		
		private bool _m_revolver;
		[STNodeProperty("Revolver", "Revolver")]
		public bool m_revolver
		{
			get { return _m_revolver; }
			set { _m_revolver = value; this.Invalidate(); }
		}
		
		private bool _m_shotgun;
		[STNodeProperty("Shotgun", "Shotgun")]
		public bool m_shotgun
		{
			get { return _m_shotgun; }
			set { _m_shotgun = value; this.Invalidate(); }
		}
		
		private bool _m_flare;
		[STNodeProperty("Flare", "Flare")]
		public bool m_flare
		{
			get { return _m_flare; }
			set { _m_flare = value; this.Invalidate(); }
		}
		
		private bool _m_flamer_fuel;
		[STNodeProperty("Flamer Fuel", "Flamer Fuel")]
		public bool m_flamer_fuel
		{
			get { return _m_flamer_fuel; }
			set { _m_flamer_fuel = value; this.Invalidate(); }
		}
		
		private bool _m_flamer;
		[STNodeProperty("Flamer", "Flamer")]
		public bool m_flamer
		{
			get { return _m_flamer; }
			set { _m_flamer = value; this.Invalidate(); }
		}
		
		private bool _m_scrap;
		[STNodeProperty("Scrap", "Scrap")]
		public bool m_scrap
		{
			get { return _m_scrap; }
			set { _m_scrap = value; this.Invalidate(); }
		}
		
		private bool _m_torch_battery;
		[STNodeProperty("Torch Battery", "Torch Battery")]
		public bool m_torch_battery
		{
			get { return _m_torch_battery; }
			set { _m_torch_battery = value; this.Invalidate(); }
		}
		
		private bool _m_torch;
		[STNodeProperty("Torch", "Torch")]
		public bool m_torch
		{
			get { return _m_torch; }
			set { _m_torch = value; this.Invalidate(); }
		}
		
		private bool _m_cattleprod_ammo;
		[STNodeProperty("Cattleprod Ammo", "Cattleprod Ammo")]
		public bool m_cattleprod_ammo
		{
			get { return _m_cattleprod_ammo; }
			set { _m_cattleprod_ammo = value; this.Invalidate(); }
		}
		
		private bool _m_cattleprod;
		[STNodeProperty("Cattleprod", "Cattleprod")]
		public bool m_cattleprod
		{
			get { return _m_cattleprod; }
			set { _m_cattleprod = value; this.Invalidate(); }
		}
		
		private bool _m_auto_propulate;
		[STNodeProperty("StartOnReset", "StartOnReset")]
		public bool m_auto_propulate
		{
			get { return _m_auto_propulate; }
			set { _m_auto_propulate = value; this.Invalidate(); }
		}
		
		private float _m_mission_number;
		[STNodeProperty("MissionNumber", "MissionNumber")]
		public float m_mission_number
		{
			get { return _m_mission_number; }
			set { _m_mission_number = value; this.Invalidate(); }
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
			
			this.Title = "GCIP_WorldPickup";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("spawn_completed", typeof(void), false);
			this.OutputOptions.Add("pickup_collected", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
