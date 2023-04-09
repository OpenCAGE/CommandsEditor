using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Rewire : STNode
	{
		private string _m_map_keyframe;
		[STNodeProperty("map_keyframe", "map_keyframe")]
		public string m_map_keyframe
		{
			get { return _m_map_keyframe; }
			set { _m_map_keyframe = value; this.Invalidate(); }
		}
		
		private int _m_total_power;
		[STNodeProperty("total_power", "total_power")]
		public int m_total_power
		{
			get { return _m_total_power; }
			set { _m_total_power = value; this.Invalidate(); }
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
			
			this.Title = "Rewire";
			
			this.InputOptions.Add("locations", typeof(string), false);
			this.InputOptions.Add("access_points", typeof(string), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("cancel", typeof(void), false);
			
			this.OutputOptions.Add("closed", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("cancelled", typeof(void), false);
		}
	}
}
