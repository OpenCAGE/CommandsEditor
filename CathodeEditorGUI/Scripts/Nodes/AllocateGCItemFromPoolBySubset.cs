using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AllocateGCItemFromPoolBySubset : STNode
	{
		private bool _m_force_usage;
		[STNodeProperty("force_usage", "force_usage")]
		public bool m_force_usage
		{
			get { return _m_force_usage; }
			set { _m_force_usage = value; this.Invalidate(); }
		}
		
		private float _m_distribution_bias;
		[STNodeProperty("distribution_bias", "distribution_bias")]
		public float m_distribution_bias
		{
			get { return _m_distribution_bias; }
			set { _m_distribution_bias = value; this.Invalidate(); }
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
			
			this.Title = "AllocateGCItemFromPoolBySubset";
			
			this.InputOptions.Add("selectable_items", typeof(STNode), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_success", typeof(void), false);
			this.OutputOptions.Add("on_failure", typeof(void), false);
			this.OutputOptions.Add("item_name", typeof(string), false);
			this.OutputOptions.Add("item_quantity", typeof(int), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
