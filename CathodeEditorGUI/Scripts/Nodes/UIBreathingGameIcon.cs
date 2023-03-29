#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class UIBreathingGameIcon : STNode
	{
		private int _m_fill_percentage;
		[STNodeProperty("fill_percentage", "fill_percentage")]
		public int m_fill_percentage
		{
			get { return _m_fill_percentage; }
			set { _m_fill_percentage = value; this.Invalidate(); }
		}
		
		private string _m_prompt_text;
		[STNodeProperty("prompt_text", "prompt_text")]
		public string m_prompt_text
		{
			get { return _m_prompt_text; }
			set { _m_prompt_text = value; this.Invalidate(); }
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
			
			this.Title = "UIBreathingGameIcon";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("exit", typeof(void), false);
			this.InputOptions.Add("refresh_value", typeof(void), false);
			this.InputOptions.Add("display_tutorial", typeof(void), false);
			this.InputOptions.Add("display_tutorial_breathing_1", typeof(void), false);
			this.InputOptions.Add("display_tutorial_breathing_2", typeof(void), false);
			this.InputOptions.Add("breathing_game_tutorial_fail", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("exited", typeof(void), false);
			this.OutputOptions.Add("value_refeshed", typeof(void), false);
		}
	}
}
#endif
