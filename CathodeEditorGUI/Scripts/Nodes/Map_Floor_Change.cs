#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Map_Floor_Change : STNode
	{
		private string _m_floor_name;
		[STNodeProperty("floor_name", "floor_name")]
		public string m_floor_name
		{
			get { return _m_floor_name; }
			set { _m_floor_name = value; this.Invalidate(); }
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
			
			this.Title = "Map_Floor_Change";
			
			this.InputOptions.Add("set_true", typeof(void), false);
			
			this.OutputOptions.Add("set_to_true", typeof(void), false);
		}
	}
}
#endif
