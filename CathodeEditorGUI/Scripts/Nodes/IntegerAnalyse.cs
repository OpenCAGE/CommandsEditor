#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class IntegerAnalyse : STNode
	{
		private int _m_Val0;
		[STNodeProperty("Val0", "Val0")]
		public int m_Val0
		{
			get { return _m_Val0; }
			set { _m_Val0 = value; this.Invalidate(); }
		}
		
		private int _m_Val1;
		[STNodeProperty("Val1", "Val1")]
		public int m_Val1
		{
			get { return _m_Val1; }
			set { _m_Val1 = value; this.Invalidate(); }
		}
		
		private int _m_Val2;
		[STNodeProperty("Val2", "Val2")]
		public int m_Val2
		{
			get { return _m_Val2; }
			set { _m_Val2 = value; this.Invalidate(); }
		}
		
		private int _m_Val3;
		[STNodeProperty("Val3", "Val3")]
		public int m_Val3
		{
			get { return _m_Val3; }
			set { _m_Val3 = value; this.Invalidate(); }
		}
		
		private int _m_Val4;
		[STNodeProperty("Val4", "Val4")]
		public int m_Val4
		{
			get { return _m_Val4; }
			set { _m_Val4 = value; this.Invalidate(); }
		}
		
		private int _m_Val5;
		[STNodeProperty("Val5", "Val5")]
		public int m_Val5
		{
			get { return _m_Val5; }
			set { _m_Val5 = value; this.Invalidate(); }
		}
		
		private int _m_Val6;
		[STNodeProperty("Val6", "Val6")]
		public int m_Val6
		{
			get { return _m_Val6; }
			set { _m_Val6 = value; this.Invalidate(); }
		}
		
		private int _m_Val7;
		[STNodeProperty("Val7", "Val7")]
		public int m_Val7
		{
			get { return _m_Val7; }
			set { _m_Val7 = value; this.Invalidate(); }
		}
		
		private int _m_Val8;
		[STNodeProperty("Val8", "Val8")]
		public int m_Val8
		{
			get { return _m_Val8; }
			set { _m_Val8 = value; this.Invalidate(); }
		}
		
		private int _m_Val9;
		[STNodeProperty("Val9", "Val9")]
		public int m_Val9
		{
			get { return _m_Val9; }
			set { _m_Val9 = value; this.Invalidate(); }
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
			
			this.Title = "IntegerAnalyse";
			
			this.InputOptions.Add("Input", typeof(int), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Result", typeof(int), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
