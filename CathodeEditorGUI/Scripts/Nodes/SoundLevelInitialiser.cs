#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundLevelInitialiser : STNode
	{
		private bool _m_auto_generate_networks;
		[STNodeProperty("auto_generate_networks", "auto_generate_networks")]
		public bool m_auto_generate_networks
		{
			get { return _m_auto_generate_networks; }
			set { _m_auto_generate_networks = value; this.Invalidate(); }
		}
		
		private float _m_network_node_min_spacing;
		[STNodeProperty("network_node_min_spacing", "network_node_min_spacing")]
		public float m_network_node_min_spacing
		{
			get { return _m_network_node_min_spacing; }
			set { _m_network_node_min_spacing = value; this.Invalidate(); }
		}
		
		private float _m_network_node_max_visibility;
		[STNodeProperty("network_node_max_visibility", "network_node_max_visibility")]
		public float m_network_node_max_visibility
		{
			get { return _m_network_node_max_visibility; }
			set { _m_network_node_max_visibility = value; this.Invalidate(); }
		}
		
		private float _m_network_node_ceiling_height;
		[STNodeProperty("network_node_ceiling_height", "network_node_ceiling_height")]
		public float m_network_node_ceiling_height
		{
			get { return _m_network_node_ceiling_height; }
			set { _m_network_node_ceiling_height = value; this.Invalidate(); }
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
			
			this.Title = "SoundLevelInitialiser";
			
			
		}
	}
}
#endif
