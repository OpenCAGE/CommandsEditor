#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_Squad_DialogueMonitor : STNode
	{
		private STNode _m_squad_coordinator;
		[STNodeProperty("squad_coordinator", "squad_coordinator")]
		public STNode m_squad_coordinator
		{
			get { return _m_squad_coordinator; }
			set { _m_squad_coordinator = value; this.Invalidate(); }
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
			
			this.Title = "NPC_Squad_DialogueMonitor";
			
			this.InputOptions.Add("start_monitor", typeof(void), false);
			this.InputOptions.Add("stop_monitor", typeof(void), false);
			
			this.OutputOptions.Add("Suspicious_Item_Initial", typeof(void), false);
			this.OutputOptions.Add("Suspicious_Item_Close", typeof(void), false);
			this.OutputOptions.Add("Suspicious_Warning", typeof(void), false);
			this.OutputOptions.Add("Suspicious_Warning_Fail", typeof(void), false);
			this.OutputOptions.Add("Missing_Buddy", typeof(void), false);
			this.OutputOptions.Add("Search_Started", typeof(void), false);
			this.OutputOptions.Add("Search_Loop", typeof(void), false);
			this.OutputOptions.Add("Search_Complete", typeof(void), false);
			this.OutputOptions.Add("Detected_Enemy", typeof(void), false);
			this.OutputOptions.Add("Alien_Heard_Backstage", typeof(void), false);
			this.OutputOptions.Add("Interrogative", typeof(void), false);
			this.OutputOptions.Add("Warning", typeof(void), false);
			this.OutputOptions.Add("Last_Chance", typeof(void), false);
			this.OutputOptions.Add("Stand_Down", typeof(void), false);
			this.OutputOptions.Add("Attack", typeof(void), false);
			this.OutputOptions.Add("Advance", typeof(void), false);
			this.OutputOptions.Add("Melee", typeof(void), false);
			this.OutputOptions.Add("Hit_By_Weapon", typeof(void), false);
			this.OutputOptions.Add("Go_to_Cover", typeof(void), false);
			this.OutputOptions.Add("No_Cover", typeof(void), false);
			this.OutputOptions.Add("Shoot_From_Cover", typeof(void), false);
			this.OutputOptions.Add("Cover_Broken", typeof(void), false);
			this.OutputOptions.Add("Retreat", typeof(void), false);
			this.OutputOptions.Add("Panic", typeof(void), false);
			this.OutputOptions.Add("Final_Hit", typeof(void), false);
			this.OutputOptions.Add("Ally_Death", typeof(void), false);
			this.OutputOptions.Add("Incoming_IED", typeof(void), false);
			this.OutputOptions.Add("Alert_Squad", typeof(void), false);
			this.OutputOptions.Add("My_Death", typeof(void), false);
			this.OutputOptions.Add("Idle_Passive", typeof(void), false);
			this.OutputOptions.Add("Idle_Aggressive", typeof(void), false);
			this.OutputOptions.Add("Block", typeof(void), false);
			this.OutputOptions.Add("Enter_Grapple", typeof(void), false);
			this.OutputOptions.Add("Grapple_From_Cover", typeof(void), false);
			this.OutputOptions.Add("Player_Observed", typeof(void), false);
			this.OutputOptions.Add("started_monitor", typeof(void), false);
			this.OutputOptions.Add("stopped_monitor", typeof(void), false);
		}
	}
}
#endif
