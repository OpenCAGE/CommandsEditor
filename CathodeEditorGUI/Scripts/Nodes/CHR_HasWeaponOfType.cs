#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_HasWeaponOfType : STNode
	{
		private string _m_weapon_type;
		[STNodeProperty("weapon_type", "weapon_type")]
		public string m_weapon_type
		{
			get { return _m_weapon_type; }
			set { _m_weapon_type = value; this.Invalidate(); }
		}
		
		private bool _m_check_if_weapon_draw;
		[STNodeProperty("check_if_weapon_draw", "check_if_weapon_draw")]
		public bool m_check_if_weapon_draw
		{
			get { return _m_check_if_weapon_draw; }
			set { _m_check_if_weapon_draw = value; this.Invalidate(); }
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
			
			this.Title = "CHR_HasWeaponOfType";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_true", typeof(void), false);
			this.OutputOptions.Add("on_false", typeof(void), false);
			this.OutputOptions.Add("Result", typeof(bool), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
