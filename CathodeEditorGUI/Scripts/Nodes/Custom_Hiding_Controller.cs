using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Custom_Hiding_Controller : STNode
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
			
			this.Title = "Custom_Hiding_Controller";
			
			this.InputOptions.Add("Enter_Anim", typeof(string), false);
			this.InputOptions.Add("Idle_Anim", typeof(string), false);
			this.InputOptions.Add("Exit_Anim", typeof(string), false);
			this.InputOptions.Add("has_MT", typeof(bool), false);
			this.InputOptions.Add("is_high", typeof(bool), false);
			this.InputOptions.Add("AlienBusted_Player_1", typeof(string), false);
			this.InputOptions.Add("AlienBusted_Alien_1", typeof(string), false);
			this.InputOptions.Add("AlienBusted_Player_2", typeof(string), false);
			this.InputOptions.Add("AlienBusted_Alien_2", typeof(string), false);
			this.InputOptions.Add("AlienBusted_Player_3", typeof(string), false);
			this.InputOptions.Add("AlienBusted_Alien_3", typeof(string), false);
			this.InputOptions.Add("AlienBusted_Player_4", typeof(string), false);
			this.InputOptions.Add("AlienBusted_Alien_4", typeof(string), false);
			this.InputOptions.Add("AndroidBusted_Player_1", typeof(string), false);
			this.InputOptions.Add("AndroidBusted_Android_1", typeof(string), false);
			this.InputOptions.Add("AndroidBusted_Player_2", typeof(string), false);
			this.InputOptions.Add("AndroidBusted_Android_2", typeof(string), false);
			this.InputOptions.Add("Get_In", typeof(void), false);
			this.InputOptions.Add("Add_NPC", typeof(void), false);
			this.InputOptions.Add("Start_Breathing_Game", typeof(void), false);
			this.InputOptions.Add("End_Breathing_Game", typeof(void), false);
			
			this.OutputOptions.Add("Started_Idle", typeof(void), false);
			this.OutputOptions.Add("Started_Exit", typeof(void), false);
			this.OutputOptions.Add("Got_Out", typeof(void), false);
			this.OutputOptions.Add("Prompt_1", typeof(void), false);
			this.OutputOptions.Add("Prompt_2", typeof(void), false);
			this.OutputOptions.Add("Start_choking", typeof(void), false);
			this.OutputOptions.Add("Start_oxygen_starvation", typeof(void), false);
			this.OutputOptions.Add("Show_MT", typeof(void), false);
			this.OutputOptions.Add("Hide_MT", typeof(void), false);
			this.OutputOptions.Add("Spawn_MT", typeof(void), false);
			this.OutputOptions.Add("Despawn_MT", typeof(void), false);
			this.OutputOptions.Add("Start_Busted_By_Alien", typeof(void), false);
			this.OutputOptions.Add("Start_Busted_By_Android", typeof(void), false);
			this.OutputOptions.Add("End_Busted_By_Android", typeof(void), false);
			this.OutputOptions.Add("Start_Busted_By_Human", typeof(void), false);
			this.OutputOptions.Add("End_Busted_By_Human", typeof(void), false);
			this.OutputOptions.Add("MT_pos", typeof(cTransform), false);
			this.OutputOptions.Add("Getting_in", typeof(void), false);
			this.OutputOptions.Add("Breathing_Game_Started", typeof(void), false);
			this.OutputOptions.Add("Breathing_Game_Ended", typeof(void), false);
		}
	}
}
