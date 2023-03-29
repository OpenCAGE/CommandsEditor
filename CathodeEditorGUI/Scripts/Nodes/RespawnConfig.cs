#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RespawnConfig : STNode
	{
		private float _m_min_dist;
		[STNodeProperty("min_dist", "min_dist")]
		public float m_min_dist
		{
			get { return _m_min_dist; }
			set { _m_min_dist = value; this.Invalidate(); }
		}
		
		private float _m_preferred_dist;
		[STNodeProperty("preferred_dist", "preferred_dist")]
		public float m_preferred_dist
		{
			get { return _m_preferred_dist; }
			set { _m_preferred_dist = value; this.Invalidate(); }
		}
		
		private float _m_max_dist;
		[STNodeProperty("max_dist", "max_dist")]
		public float m_max_dist
		{
			get { return _m_max_dist; }
			set { _m_max_dist = value; this.Invalidate(); }
		}
		
		private string _m_respawn_mode;
		[STNodeProperty("respawn_mode", "respawn_mode")]
		public string m_respawn_mode
		{
			get { return _m_respawn_mode; }
			set { _m_respawn_mode = value; this.Invalidate(); }
		}
		
		private int _m_respawn_wait_time;
		[STNodeProperty("respawn_wait_time", "respawn_wait_time")]
		public int m_respawn_wait_time
		{
			get { return _m_respawn_wait_time; }
			set { _m_respawn_wait_time = value; this.Invalidate(); }
		}
		
		private int _m_uncollidable_time;
		[STNodeProperty("uncollidable_time", "uncollidable_time")]
		public int m_uncollidable_time
		{
			get { return _m_uncollidable_time; }
			set { _m_uncollidable_time = value; this.Invalidate(); }
		}
		
		private bool _m_is_default;
		[STNodeProperty("is_default", "is_default")]
		public bool m_is_default
		{
			get { return _m_is_default; }
			set { _m_is_default = value; this.Invalidate(); }
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
			
			this.Title = "RespawnConfig";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
		}
	}
}
#endif
