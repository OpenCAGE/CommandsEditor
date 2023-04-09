using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RandomInt : STNode
	{
		private int _m_Min;
		[STNodeProperty("Min", "Min")]
		public int m_Min
		{
			get { return _m_Min; }
			set { _m_Min = value; this.Invalidate(); }
		}
		
		private int _m_Max;
		[STNodeProperty("Max", "Max")]
		public int m_Max
		{
			get { return _m_Max; }
			set { _m_Max = value; this.Invalidate(); }
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
			
			this.Title = "RandomInt";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Result", typeof(int), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
