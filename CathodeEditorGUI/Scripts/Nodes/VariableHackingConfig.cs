using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class VariableHackingConfig : STNode
	{
		private int _m_nodes;
		[STNodeProperty("nodes", "nodes")]
		public int m_nodes
		{
			get { return _m_nodes; }
			set { _m_nodes = value; this.Invalidate(); }
		}
		
		private int _m_sensors;
		[STNodeProperty("sensors", "sensors")]
		public int m_sensors
		{
			get { return _m_sensors; }
			set { _m_sensors = value; this.Invalidate(); }
		}
		
		private int _m_victory_nodes;
		[STNodeProperty("victory_nodes", "victory_nodes")]
		public int m_victory_nodes
		{
			get { return _m_victory_nodes; }
			set { _m_victory_nodes = value; this.Invalidate(); }
		}
		
		private int _m_victory_sensors;
		[STNodeProperty("victory_sensors", "victory_sensors")]
		public int m_victory_sensors
		{
			get { return _m_victory_sensors; }
			set { _m_victory_sensors = value; this.Invalidate(); }
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
			
			this.Title = "VariableHackingConfig";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("on_changed", typeof(void), false);
			this.OutputOptions.Add("on_restored", typeof(void), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
