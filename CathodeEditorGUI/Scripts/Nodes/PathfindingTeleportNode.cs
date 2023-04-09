using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PathfindingTeleportNode : STNode
	{
		private bool _m_build_into_navmesh;
		[STNodeProperty("build_into_navmesh", "build_into_navmesh")]
		public bool m_build_into_navmesh
		{
			get { return _m_build_into_navmesh; }
			set { _m_build_into_navmesh = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
		}
		
		private float _m_extra_cost;
		[STNodeProperty("extra_cost", "extra_cost")]
		public float m_extra_cost
		{
			get { return _m_extra_cost; }
			set { _m_extra_cost = value; this.Invalidate(); }
		}
		
		private string _m_character_classes;
		[STNodeProperty("character_classes", "character_classes")]
		public string m_character_classes
		{
			get { return _m_character_classes; }
			set { _m_character_classes = value; this.Invalidate(); }
		}
		
		private bool _m_open_on_reset;
		[STNodeProperty("open_on_reset", "open_on_reset")]
		public bool m_open_on_reset
		{
			get { return _m_open_on_reset; }
			set { _m_open_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "PathfindingTeleportNode";
			
			this.InputOptions.Add("destination", typeof(cTransform), false);
			this.InputOptions.Add("update_cost", typeof(void), false);
			this.InputOptions.Add("open", typeof(void), false);
			this.InputOptions.Add("close", typeof(void), false);
			
			this.OutputOptions.Add("started_teleporting", typeof(void), false);
			this.OutputOptions.Add("stopped_teleporting", typeof(void), false);
			this.OutputOptions.Add("on_updated_cost", typeof(void), false);
			this.OutputOptions.Add("opened", typeof(void), false);
			this.OutputOptions.Add("closed", typeof(void), false);
		}
	}
}
