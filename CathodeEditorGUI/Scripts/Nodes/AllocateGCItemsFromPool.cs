#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AllocateGCItemsFromPool : STNode
	{
		private int _m_force_usage_count;
		[STNodeProperty("force_usage_count", "force_usage_count")]
		public int m_force_usage_count
		{
			get { return _m_force_usage_count; }
			set { _m_force_usage_count = value; this.Invalidate(); }
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
			
			this.Title = "AllocateGCItemsFromPool";
			
			this.InputOptions.Add("items", typeof(string), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_success", typeof(void), false);
			this.OutputOptions.Add("on_failure", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
