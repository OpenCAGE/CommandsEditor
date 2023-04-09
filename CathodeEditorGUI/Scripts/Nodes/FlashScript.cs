using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FlashScript : STNode
	{
		private bool _m_show_on_reset;
		[STNodeProperty("show_on_reset", "show_on_reset")]
		public bool m_show_on_reset
		{
			get { return _m_show_on_reset; }
			set { _m_show_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_filename;
		[STNodeProperty("filename", "filename")]
		public string m_filename
		{
			get { return _m_filename; }
			set { _m_filename = value; this.Invalidate(); }
		}
		
		private string _m_layer_name;
		[STNodeProperty("layer_name", "layer_name")]
		public string m_layer_name
		{
			get { return _m_layer_name; }
			set { _m_layer_name = value; this.Invalidate(); }
		}
		
		private string _m_target_texture_name;
		[STNodeProperty("target_texture_name", "target_texture_name")]
		public string m_target_texture_name
		{
			get { return _m_target_texture_name; }
			set { _m_target_texture_name = value; this.Invalidate(); }
		}
		
		private string _m_type;
		[STNodeProperty("type", "type")]
		public string m_type
		{
			get { return _m_type; }
			set { _m_type = value; this.Invalidate(); }
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
			
			this.Title = "FlashScript";
			
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
		}
	}
}
