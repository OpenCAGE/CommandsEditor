using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetGamepadAxes : STNode
	{
		private bool _m_invert_x;
		[STNodeProperty("invert_x", "invert_x")]
		public bool m_invert_x
		{
			get { return _m_invert_x; }
			set { _m_invert_x = value; this.Invalidate(); }
		}
		
		private bool _m_invert_y;
		[STNodeProperty("invert_y", "invert_y")]
		public bool m_invert_y
		{
			get { return _m_invert_y; }
			set { _m_invert_y = value; this.Invalidate(); }
		}
		
		private bool _m_save_settings;
		[STNodeProperty("save_settings", "save_settings")]
		public bool m_save_settings
		{
			get { return _m_save_settings; }
			set { _m_save_settings = value; this.Invalidate(); }
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
			
			this.Title = "SetGamepadAxes";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
