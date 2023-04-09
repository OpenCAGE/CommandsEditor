using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Persistent_TriggerRandomSequence : STNode
	{
		private int _m_num;
		[STNodeProperty("num", "num")]
		public int m_num
		{
			get { return _m_num; }
			set { _m_num = value; this.Invalidate(); }
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
			
			this.Title = "Persistent_TriggerRandomSequence";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("reset_all", typeof(void), false);
			this.InputOptions.Add("reset_Random_1", typeof(void), false);
			this.InputOptions.Add("reset_Random_2", typeof(void), false);
			this.InputOptions.Add("reset_Random_3", typeof(void), false);
			this.InputOptions.Add("reset_Random_4", typeof(void), false);
			this.InputOptions.Add("reset_Random_5", typeof(void), false);
			this.InputOptions.Add("reset_Random_6", typeof(void), false);
			this.InputOptions.Add("reset_Random_7", typeof(void), false);
			this.InputOptions.Add("reset_Random_8", typeof(void), false);
			this.InputOptions.Add("reset_Random_9", typeof(void), false);
			this.InputOptions.Add("reset_Random_10", typeof(void), false);
			
			this.OutputOptions.Add("Random_1", typeof(void), false);
			this.OutputOptions.Add("Random_2", typeof(void), false);
			this.OutputOptions.Add("Random_3", typeof(void), false);
			this.OutputOptions.Add("Random_4", typeof(void), false);
			this.OutputOptions.Add("Random_5", typeof(void), false);
			this.OutputOptions.Add("Random_6", typeof(void), false);
			this.OutputOptions.Add("Random_7", typeof(void), false);
			this.OutputOptions.Add("Random_8", typeof(void), false);
			this.OutputOptions.Add("Random_9", typeof(void), false);
			this.OutputOptions.Add("Random_10", typeof(void), false);
			this.OutputOptions.Add("All_triggered", typeof(void), false);
			this.OutputOptions.Add("current", typeof(int), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("on_reset_all", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_1", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_2", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_3", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_4", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_5", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_6", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_7", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_8", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_9", typeof(void), false);
			this.OutputOptions.Add("on_reset_Random_10", typeof(void), false);
		}
	}
}
