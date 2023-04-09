using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RemoveFromGCItemPool : STNode
	{
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
		
		private int _m_gcip_instances_to_remove;
		[STNodeProperty("gcip_instances_to_remove", "gcip_instances_to_remove")]
		public int m_gcip_instances_to_remove
		{
			get { return _m_gcip_instances_to_remove; }
			set { _m_gcip_instances_to_remove = value; this.Invalidate(); }
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
			
			this.Title = "RemoveFromGCItemPool";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_success", typeof(void), false);
			this.OutputOptions.Add("on_failure", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
