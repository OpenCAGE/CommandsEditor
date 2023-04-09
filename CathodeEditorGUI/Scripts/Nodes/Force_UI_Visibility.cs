using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Force_UI_Visibility : STNode
	{
		private bool _m_also_disable_interactions;
		[STNodeProperty("also_disable_interactions", "also_disable_interactions")]
		public bool m_also_disable_interactions
		{
			get { return _m_also_disable_interactions; }
			set { _m_also_disable_interactions = value; this.Invalidate(); }
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
			
			this.Title = "Force_UI_Visibility";
			
			this.InputOptions.Add("clear_pending_ui", typeof(void), false);
			this.InputOptions.Add("hide_objective_message", typeof(void), false);
			this.InputOptions.Add("show_objective_message", typeof(void), false);
			this.InputOptions.Add("cutting_panel_start", typeof(void), false);
			this.InputOptions.Add("cutting_panel_finish", typeof(void), false);
			this.InputOptions.Add("keypad_interaction_start", typeof(void), false);
			this.InputOptions.Add("keypad_interaction_finish", typeof(void), false);
			this.InputOptions.Add("traversal_interaction_start", typeof(void), false);
			this.InputOptions.Add("suit_change_interaction_finish", typeof(void), false);
			this.InputOptions.Add("suit_change_interaction_start", typeof(void), false);
			this.InputOptions.Add("terminal_interaction_finish", typeof(void), false);
			this.InputOptions.Add("terminal_interaction_start", typeof(void), false);
			this.InputOptions.Add("rewire_interaction_start", typeof(void), false);
			this.InputOptions.Add("rewire_interaction_finish", typeof(void), false);
			this.InputOptions.Add("hacking_interaction_start", typeof(void), false);
			this.InputOptions.Add("hacking_interaction_finish", typeof(void), false);
			this.InputOptions.Add("ladder_interaction_start", typeof(void), false);
			this.InputOptions.Add("ladder_interaction_finish", typeof(void), false);
			this.InputOptions.Add("button_interaction_start", typeof(void), false);
			this.InputOptions.Add("button_interaction_finish", typeof(void), false);
			this.InputOptions.Add("lever_interaction_start", typeof(void), false);
			this.InputOptions.Add("lever_interaction_finish", typeof(void), false);
			this.InputOptions.Add("level_fade_start", typeof(void), false);
			this.InputOptions.Add("level_fade_finish", typeof(void), false);
			this.InputOptions.Add("cutscene_visibility_start", typeof(void), false);
			this.InputOptions.Add("cutscene_visibility_finish", typeof(void), false);
			this.InputOptions.Add("hiding_visibility_start", typeof(void), false);
			this.InputOptions.Add("hiding_visibility_finish", typeof(void), false);
			
			this.OutputOptions.Add("objective_message_hidden", typeof(void), false);
			this.OutputOptions.Add("objective_message_shown", typeof(void), false);
			this.OutputOptions.Add("cutting_pannel_started", typeof(void), false);
			this.OutputOptions.Add("cutting_pannel_finished", typeof(void), false);
			this.OutputOptions.Add("keypad_interaction_started", typeof(void), false);
			this.OutputOptions.Add("keypad_interaction_finished", typeof(void), false);
			this.OutputOptions.Add("traversal_interaction_started", typeof(void), false);
			this.OutputOptions.Add("suit_change_interaction_finished", typeof(void), false);
			this.OutputOptions.Add("suit_change_interaction_started", typeof(void), false);
			this.OutputOptions.Add("terminal_interaction_finished", typeof(void), false);
			this.OutputOptions.Add("terminal_interaction_started", typeof(void), false);
			this.OutputOptions.Add("rewire_interaction_started", typeof(void), false);
			this.OutputOptions.Add("rewire_interaction_finished", typeof(void), false);
			this.OutputOptions.Add("hacking_interaction_started", typeof(void), false);
			this.OutputOptions.Add("hacking_interaction_finished", typeof(void), false);
			this.OutputOptions.Add("ladder_interaction_started", typeof(void), false);
			this.OutputOptions.Add("ladder_interaction_finished", typeof(void), false);
			this.OutputOptions.Add("button_interaction_started", typeof(void), false);
			this.OutputOptions.Add("button_interaction_finished", typeof(void), false);
			this.OutputOptions.Add("lever_interaction_started", typeof(void), false);
			this.OutputOptions.Add("lever_interaction_finished", typeof(void), false);
			this.OutputOptions.Add("level_fade_started", typeof(void), false);
			this.OutputOptions.Add("level_fade_finished", typeof(void), false);
			this.OutputOptions.Add("cutscene_visibility_started", typeof(void), false);
			this.OutputOptions.Add("cutscene_visibility_finished", typeof(void), false);
			this.OutputOptions.Add("hiding_visibility_started", typeof(void), false);
			this.OutputOptions.Add("hiding_visibility_finished", typeof(void), false);
		}
	}
}
