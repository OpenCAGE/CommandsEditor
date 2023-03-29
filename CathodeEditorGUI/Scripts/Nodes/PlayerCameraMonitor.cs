#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlayerCameraMonitor : STNode
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
			
			this.Title = "PlayerCameraMonitor";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("AndroidNeckSnap", typeof(void), false);
			this.OutputOptions.Add("AlienKill", typeof(void), false);
			this.OutputOptions.Add("AlienKillBroken", typeof(void), false);
			this.OutputOptions.Add("AlienKillInVent", typeof(void), false);
			this.OutputOptions.Add("StandardAnimDrivenView", typeof(void), false);
			this.OutputOptions.Add("StopNonStandardCameras", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
#endif
