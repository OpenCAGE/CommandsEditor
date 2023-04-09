using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AssetSpawner : STNode
	{
		private bool _m_spawn_on_reset;
		[STNodeProperty("spawn_on_reset", "spawn_on_reset")]
		public bool m_spawn_on_reset
		{
			get { return _m_spawn_on_reset; }
			set { _m_spawn_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_spawn_on_load;
		[STNodeProperty("spawn_on_load", "spawn_on_load")]
		public bool m_spawn_on_load
		{
			get { return _m_spawn_on_load; }
			set { _m_spawn_on_load = value; this.Invalidate(); }
		}
		
		private bool _m_allow_forced_despawn;
		[STNodeProperty("allow_forced_despawn", "allow_forced_despawn")]
		public bool m_allow_forced_despawn
		{
			get { return _m_allow_forced_despawn; }
			set { _m_allow_forced_despawn = value; this.Invalidate(); }
		}
		
		private bool _m_persist_on_callback;
		[STNodeProperty("persist_on_callback", "persist_on_callback")]
		public bool m_persist_on_callback
		{
			get { return _m_persist_on_callback; }
			set { _m_persist_on_callback = value; this.Invalidate(); }
		}
		
		private bool _m_allow_physics;
		[STNodeProperty("allow_physics", "allow_physics")]
		public bool m_allow_physics
		{
			get { return _m_allow_physics; }
			set { _m_allow_physics = value; this.Invalidate(); }
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
			
			this.Title = "AssetSpawner";
			
			this.InputOptions.Add("asset", typeof(STNode), false);
			this.InputOptions.Add("spawn", typeof(void), false);
			this.InputOptions.Add("despawn", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("finished_spawning", typeof(void), false);
			this.OutputOptions.Add("callback_triggered", typeof(void), false);
			this.OutputOptions.Add("forced_despawn", typeof(void), false);
			this.OutputOptions.Add("spawned", typeof(void), false);
			this.OutputOptions.Add("despawned", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
