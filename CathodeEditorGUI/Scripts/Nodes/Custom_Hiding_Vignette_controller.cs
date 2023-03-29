#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Custom_Hiding_Vignette_controller : STNode
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
			
			this.Title = "Custom_Hiding_Vignette_controller";
			
			this.InputOptions.Add("Breath", typeof(int), false);
			this.InputOptions.Add("Blackout_start_time", typeof(int), false);
			this.InputOptions.Add("run_out_time", typeof(int), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("StartFade", typeof(void), false);
			this.OutputOptions.Add("StopFade", typeof(void), false);
			this.OutputOptions.Add("Vignette", typeof(float), false);
			this.OutputOptions.Add("FadeValue", typeof(float), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
#endif
