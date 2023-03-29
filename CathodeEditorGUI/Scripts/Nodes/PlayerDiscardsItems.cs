#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayerDiscardsItems : STNode
	{
		private bool _m_discard_ieds;
		[STNodeProperty("discard_ieds", "discard_ieds")]
		public bool m_discard_ieds
		{
			get { return _m_discard_ieds; }
			set { _m_discard_ieds = value; this.Invalidate(); }
		}
		
		private bool _m_discard_medikits;
		[STNodeProperty("discard_medikits", "discard_medikits")]
		public bool m_discard_medikits
		{
			get { return _m_discard_medikits; }
			set { _m_discard_medikits = value; this.Invalidate(); }
		}
		
		private bool _m_discard_ammo;
		[STNodeProperty("discard_ammo", "discard_ammo")]
		public bool m_discard_ammo
		{
			get { return _m_discard_ammo; }
			set { _m_discard_ammo = value; this.Invalidate(); }
		}
		
		private bool _m_discard_flares_and_lights;
		[STNodeProperty("discard_flares_and_lights", "discard_flares_and_lights")]
		public bool m_discard_flares_and_lights
		{
			get { return _m_discard_flares_and_lights; }
			set { _m_discard_flares_and_lights = value; this.Invalidate(); }
		}
		
		private bool _m_discard_materials;
		[STNodeProperty("discard_materials", "discard_materials")]
		public bool m_discard_materials
		{
			get { return _m_discard_materials; }
			set { _m_discard_materials = value; this.Invalidate(); }
		}
		
		private bool _m_discard_batteries;
		[STNodeProperty("discard_batteries", "discard_batteries")]
		public bool m_discard_batteries
		{
			get { return _m_discard_batteries; }
			set { _m_discard_batteries = value; this.Invalidate(); }
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
			
			this.Title = "PlayerDiscardsItems";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
