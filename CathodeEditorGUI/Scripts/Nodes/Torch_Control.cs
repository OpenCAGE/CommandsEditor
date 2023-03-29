#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Torch_Control : STNode
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
			
			this.Title = "Torch_Control";
			
			this.InputOptions.Add("character", typeof(string), false);
			this.InputOptions.Add("turn_off_torch", typeof(void), false);
			this.InputOptions.Add("turn_on_torch", typeof(void), false);
			this.InputOptions.Add("toggle_torch", typeof(void), false);
			this.InputOptions.Add("resume_torch", typeof(void), false);
			this.InputOptions.Add("allow_torch", typeof(void), false);
			
			this.OutputOptions.Add("torch_switched_off", typeof(void), false);
			this.OutputOptions.Add("torch_switched_on", typeof(void), false);
			this.OutputOptions.Add("Turn_off_", typeof(void), false);
			this.OutputOptions.Add("Turn_on_", typeof(void), false);
			this.OutputOptions.Add("Toggle_Torch_", typeof(void), false);
			this.OutputOptions.Add("Resume_", typeof(void), false);
			this.OutputOptions.Add("Allow_", typeof(void), false);
		}
	}
}
#endif
