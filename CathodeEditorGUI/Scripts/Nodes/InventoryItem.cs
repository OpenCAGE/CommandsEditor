#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class InventoryItem : STNode
	{
		private string _m_item;
		[STNodeProperty("item", "item")]
		public string m_item
		{
			get { return _m_item; }
			set { _m_item = value; this.Invalidate(); }
		}
		
		private int _m_quantity;
		[STNodeProperty("quantity", "quantity")]
		public int m_quantity
		{
			get { return _m_quantity; }
			set { _m_quantity = value; this.Invalidate(); }
		}
		
		private bool _m_clear_on_collect;
		[STNodeProperty("clear_on_collect", "clear_on_collect")]
		public bool m_clear_on_collect
		{
			get { return _m_clear_on_collect; }
			set { _m_clear_on_collect = value; this.Invalidate(); }
		}
		
		private int _m_gcip_instances_count;
		[STNodeProperty("gcip_instances_count", "gcip_instances_count")]
		public int m_gcip_instances_count
		{
			get { return _m_gcip_instances_count; }
			set { _m_gcip_instances_count = value; this.Invalidate(); }
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
			
			this.Title = "InventoryItem";
			
			this.InputOptions.Add("itemName", typeof(string), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			
			this.OutputOptions.Add("collect", typeof(void), false);
			this.OutputOptions.Add("out_itemName", typeof(string), false);
			this.OutputOptions.Add("out_quantity", typeof(int), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
		}
	}
}
#endif
