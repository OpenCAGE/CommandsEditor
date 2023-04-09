using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Weapon_AINotifier : STNode
	{
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
			
			this.Title = "Weapon_AINotifier";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("impact", typeof(void), false);
			this.InputOptions.Add("reloading", typeof(void), false);
			this.InputOptions.Add("out_of_ammo", typeof(void), false);
			this.InputOptions.Add("started_aiming", typeof(void), false);
			this.InputOptions.Add("stopped_aiming", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("impacted", typeof(void), false);
			this.OutputOptions.Add("reloading_handled", typeof(void), false);
			this.OutputOptions.Add("out_of_ammo_handled", typeof(void), false);
			this.OutputOptions.Add("started_aiming_handled", typeof(void), false);
			this.OutputOptions.Add("stopped_aiming_handled", typeof(void), false);
		}
	}
}
