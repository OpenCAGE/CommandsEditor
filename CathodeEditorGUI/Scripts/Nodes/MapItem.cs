using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MapItem : STNode
	{
		private bool _m_show_ui_on_reset;
		[STNodeProperty("show_ui_on_reset", "show_ui_on_reset")]
		public bool m_show_ui_on_reset
		{
			get { return _m_show_ui_on_reset; }
			set { _m_show_ui_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_item_type;
		[STNodeProperty("item_type", "item_type")]
		public string m_item_type
		{
			get { return _m_item_type; }
			set { _m_item_type = value; this.Invalidate(); }
		}
		
		private string _m_map_keyframe;
		[STNodeProperty("map_keyframe", "map_keyframe")]
		public string m_map_keyframe
		{
			get { return _m_map_keyframe; }
			set { _m_map_keyframe = value; this.Invalidate(); }
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
			
			this.Title = "MapItem";
			
			this.InputOptions.Add("refresh_value", typeof(void), false);
			this.InputOptions.Add("hide_ui", typeof(void), false);
			this.InputOptions.Add("show_ui", typeof(void), false);
			
			this.OutputOptions.Add("value_refeshed", typeof(void), false);
			this.OutputOptions.Add("ui_hidden", typeof(void), false);
			this.OutputOptions.Add("ui_shown", typeof(void), false);
		}
	}
}
