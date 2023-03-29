#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class BulletChamber : STNode
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
			
			this.Title = "BulletChamber";
			
			this.InputOptions.Add("Slot1", typeof(STNode), false);
			this.InputOptions.Add("Slot2", typeof(STNode), false);
			this.InputOptions.Add("Slot3", typeof(STNode), false);
			this.InputOptions.Add("Slot4", typeof(STNode), false);
			this.InputOptions.Add("Slot5", typeof(STNode), false);
			this.InputOptions.Add("Slot6", typeof(STNode), false);
			this.InputOptions.Add("Weapon", typeof(STNode), false);
			this.InputOptions.Add("Geometry", typeof(STNode), false);
			this.InputOptions.Add("reload_fill", typeof(void), false);
			this.InputOptions.Add("reload_open", typeof(void), false);
			this.InputOptions.Add("reload_empty", typeof(void), false);
			this.InputOptions.Add("reload_load", typeof(void), false);
			this.InputOptions.Add("reload_fire", typeof(void), false);
			this.InputOptions.Add("reload_finish", typeof(void), false);
			
			this.OutputOptions.Add("reload_filled", typeof(void), false);
			this.OutputOptions.Add("reload_opened", typeof(void), false);
			this.OutputOptions.Add("reload_emptied", typeof(void), false);
			this.OutputOptions.Add("reload_loaded", typeof(void), false);
			this.OutputOptions.Add("reload_fired", typeof(void), false);
			this.OutputOptions.Add("reload_finished", typeof(void), false);
		}
	}
}
#endif
