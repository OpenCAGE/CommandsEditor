#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_multi_behaviour_monitor : STNode
	{
		private bool _m_trigger_on_start;
		[STNodeProperty("trigger_on_start", "trigger_on_start")]
		public bool m_trigger_on_start
		{
			get { return _m_trigger_on_start; }
			set { _m_trigger_on_start = value; this.Invalidate(); }
		}
		
		private bool _m_trigger_on_checkpoint_restart;
		[STNodeProperty("trigger_on_checkpoint_restart", "trigger_on_checkpoint_restart")]
		public bool m_trigger_on_checkpoint_restart
		{
			get { return _m_trigger_on_checkpoint_restart; }
			set { _m_trigger_on_checkpoint_restart = value; this.Invalidate(); }
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
			
			this.Title = "NPC_multi_behaviour_monitor";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("Cinematic_set", typeof(void), false);
			this.OutputOptions.Add("Cinematic_unset", typeof(void), false);
			this.OutputOptions.Add("Damage_Response_set", typeof(void), false);
			this.OutputOptions.Add("Damage_Response_unset", typeof(void), false);
			this.OutputOptions.Add("Target_Is_NPC_set", typeof(void), false);
			this.OutputOptions.Add("Target_Is_NPC_unset", typeof(void), false);
			this.OutputOptions.Add("Breakout_set", typeof(void), false);
			this.OutputOptions.Add("Breakout_unset", typeof(void), false);
			this.OutputOptions.Add("Attack_set", typeof(void), false);
			this.OutputOptions.Add("Attack_unset", typeof(void), false);
			this.OutputOptions.Add("Stunned_set", typeof(void), false);
			this.OutputOptions.Add("Stunned_unset", typeof(void), false);
			this.OutputOptions.Add("Backstage_set", typeof(void), false);
			this.OutputOptions.Add("Backstage_unset", typeof(void), false);
			this.OutputOptions.Add("In_Vent_set", typeof(void), false);
			this.OutputOptions.Add("In_Vent_unset", typeof(void), false);
			this.OutputOptions.Add("Killtrap_set", typeof(void), false);
			this.OutputOptions.Add("Killtrap_unset", typeof(void), false);
			this.OutputOptions.Add("Threat_Aware_set", typeof(void), false);
			this.OutputOptions.Add("Threat_Aware_unset", typeof(void), false);
			this.OutputOptions.Add("Suspect_Target_Response_set", typeof(void), false);
			this.OutputOptions.Add("Suspect_Target_Response_unset", typeof(void), false);
			this.OutputOptions.Add("Player_Hiding_set", typeof(void), false);
			this.OutputOptions.Add("Player_Hiding_unset", typeof(void), false);
			this.OutputOptions.Add("Suspicious_Item_set", typeof(void), false);
			this.OutputOptions.Add("Suspicious_Item_unset", typeof(void), false);
			this.OutputOptions.Add("Search_set", typeof(void), false);
			this.OutputOptions.Add("Search_unset", typeof(void), false);
			this.OutputOptions.Add("Area_Sweep_set", typeof(void), false);
			this.OutputOptions.Add("Area_Sweep_unset", typeof(void), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
#endif
