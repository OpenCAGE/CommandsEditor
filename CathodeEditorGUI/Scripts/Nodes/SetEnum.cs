using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetEnum : STNode
	{
		private cEnum _m_initial_value;
		[STNodeProperty("initial_value", "initial_value")]
		public cEnum m_initial_value
		{
			get { return _m_initial_value; }
			set { _m_initial_value = value; this.Invalidate(); }
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
			
			this.Title = "SetEnum";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Output", typeof(cEnum), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
