using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerRandom : STNode
	{
		private int _m_Num;
		[STNodeProperty("Num", "Num")]
		public int m_Num
		{
			get { return _m_Num; }
			set { _m_Num = value; this.Invalidate(); }
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
			
			this.Title = "TriggerRandom";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
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
			this.OutputOptions.Add("Random_11", typeof(void), false);
			this.OutputOptions.Add("Random_12", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
