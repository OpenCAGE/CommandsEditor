using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Display_Element_On_Map : STNode
	{
		private string _m_map_name;
		[STNodeProperty("map_name", "map_name")]
		public string m_map_name
		{
			get { return _m_map_name; }
			set { _m_map_name = value; this.Invalidate(); }
		}
		
		private string _m_element_name;
		[STNodeProperty("element_name", "element_name")]
		public string m_element_name
		{
			get { return _m_element_name; }
			set { _m_element_name = value; this.Invalidate(); }
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
			
			this.Title = "Display_Element_On_Map";
			
			this.InputOptions.Add("set_true", typeof(void), false);
			this.InputOptions.Add("set_false", typeof(void), false);
			
			this.OutputOptions.Add("set_to_true", typeof(void), false);
			this.OutputOptions.Add("set_to_false", typeof(void), false);
		}
	}
}
