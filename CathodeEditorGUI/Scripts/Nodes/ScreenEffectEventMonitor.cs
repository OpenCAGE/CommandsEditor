using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ScreenEffectEventMonitor : STNode
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
			
			this.Title = "ScreenEffectEventMonitor";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("MeleeHit", typeof(void), false);
			this.OutputOptions.Add("BulletHit", typeof(void), false);
			this.OutputOptions.Add("MedkitHeal", typeof(void), false);
			this.OutputOptions.Add("StartStrangle", typeof(void), false);
			this.OutputOptions.Add("StopStrangle", typeof(void), false);
			this.OutputOptions.Add("StartLowHealth", typeof(void), false);
			this.OutputOptions.Add("StopLowHealth", typeof(void), false);
			this.OutputOptions.Add("StartDeath", typeof(void), false);
			this.OutputOptions.Add("StopDeath", typeof(void), false);
			this.OutputOptions.Add("AcidHit", typeof(void), false);
			this.OutputOptions.Add("FlashbangHit", typeof(void), false);
			this.OutputOptions.Add("HitAndRun", typeof(void), false);
			this.OutputOptions.Add("CancelHitAndRun", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
