using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NetPlayerCounter : STNode
	{
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
			
			this.Title = "NetPlayerCounter";
			
			this.InputOptions.Add("enter", typeof(void), false);
			this.InputOptions.Add("exit", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("on_full", typeof(void), false);
			this.OutputOptions.Add("on_empty", typeof(void), false);
			this.OutputOptions.Add("on_intermediate", typeof(void), false);
			this.OutputOptions.Add("is_full", typeof(bool), false);
			this.OutputOptions.Add("is_empty", typeof(bool), false);
			this.OutputOptions.Add("contains_local_player", typeof(bool), false);
			this.OutputOptions.Add("entered", typeof(void), false);
			this.OutputOptions.Add("exited", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
