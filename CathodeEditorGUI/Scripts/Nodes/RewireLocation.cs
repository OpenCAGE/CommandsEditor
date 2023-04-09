using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RewireLocation : STNode
	{
		private string _m_element_name;
		[STNodeProperty("element_name", "element_name")]
		public string m_element_name
		{
			get { return _m_element_name; }
			set { _m_element_name = value; this.Invalidate(); }
		}
		
		private string _m_display_name;
		[STNodeProperty("display_name", "display_name")]
		public string m_display_name
		{
			get { return _m_display_name; }
			set { _m_display_name = value; this.Invalidate(); }
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
			
			this.Title = "RewireLocation";
			
			this.InputOptions.Add("systems", typeof(string), false);
			
			this.OutputOptions.Add("power_draw_increased", typeof(void), false);
			this.OutputOptions.Add("power_draw_reduced", typeof(void), false);
		}
	}
}
