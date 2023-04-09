using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayerWeaponMonitor : STNode
	{
		private string _m_weapon_type;
		[STNodeProperty("weapon_type", "weapon_type")]
		public string m_weapon_type
		{
			get { return _m_weapon_type; }
			set { _m_weapon_type = value; this.Invalidate(); }
		}
		
		private float _m_ammo_percentage_in_clip;
		[STNodeProperty("ammo_percentage_in_clip", "ammo_percentage_in_clip")]
		public float m_ammo_percentage_in_clip
		{
			get { return _m_ammo_percentage_in_clip; }
			set { _m_ammo_percentage_in_clip = value; this.Invalidate(); }
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
			
			this.Title = "PlayerWeaponMonitor";
			
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			
			this.OutputOptions.Add("on_clip_above_percentage", typeof(void), false);
			this.OutputOptions.Add("on_clip_below_percentage", typeof(void), false);
			this.OutputOptions.Add("on_clip_empty", typeof(void), false);
			this.OutputOptions.Add("on_clip_full", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
		}
	}
}
