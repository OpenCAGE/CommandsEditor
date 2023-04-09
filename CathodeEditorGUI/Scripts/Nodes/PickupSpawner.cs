using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PickupSpawner : STNode
	{
		private bool _m_spawn_on_reset;
		[STNodeProperty("spawn_on_reset", "spawn_on_reset")]
		public bool m_spawn_on_reset
		{
			get { return _m_spawn_on_reset; }
			set { _m_spawn_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_item_name;
		[STNodeProperty("item_name", "item_name")]
		public string m_item_name
		{
			get { return _m_item_name; }
			set { _m_item_name = value; this.Invalidate(); }
		}
		
		private int _m_item_quantity;
		[STNodeProperty("item_quantity", "item_quantity")]
		public int m_item_quantity
		{
			get { return _m_item_quantity; }
			set { _m_item_quantity = value; this.Invalidate(); }
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
			
			this.Title = "PickupSpawner";
			
			this.InputOptions.Add("pos", typeof(cTransform), false);
			this.InputOptions.Add("spawn", typeof(void), false);
			this.InputOptions.Add("despawn", typeof(void), false);
			
			this.OutputOptions.Add("collect", typeof(void), false);
			this.OutputOptions.Add("spawned", typeof(void), false);
			this.OutputOptions.Add("despawned", typeof(void), false);
		}
	}
}
