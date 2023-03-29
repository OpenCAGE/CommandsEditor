#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FlashInvoke : STNode
	{
		private string _m_method;
		[STNodeProperty("method", "method")]
		public string m_method
		{
			get { return _m_method; }
			set { _m_method = value; this.Invalidate(); }
		}
		
		private string _m_invoke_type;
		[STNodeProperty("invoke_type", "invoke_type")]
		public string m_invoke_type
		{
			get { return _m_invoke_type; }
			set { _m_invoke_type = value; this.Invalidate(); }
		}
		
		private int _m_int_argument_0;
		[STNodeProperty("int_argument_0", "int_argument_0")]
		public int m_int_argument_0
		{
			get { return _m_int_argument_0; }
			set { _m_int_argument_0 = value; this.Invalidate(); }
		}
		
		private int _m_int_argument_1;
		[STNodeProperty("int_argument_1", "int_argument_1")]
		public int m_int_argument_1
		{
			get { return _m_int_argument_1; }
			set { _m_int_argument_1 = value; this.Invalidate(); }
		}
		
		private int _m_int_argument_2;
		[STNodeProperty("int_argument_2", "int_argument_2")]
		public int m_int_argument_2
		{
			get { return _m_int_argument_2; }
			set { _m_int_argument_2 = value; this.Invalidate(); }
		}
		
		private int _m_int_argument_3;
		[STNodeProperty("int_argument_3", "int_argument_3")]
		public int m_int_argument_3
		{
			get { return _m_int_argument_3; }
			set { _m_int_argument_3 = value; this.Invalidate(); }
		}
		
		private float _m_float_argument_0;
		[STNodeProperty("float_argument_0", "float_argument_0")]
		public float m_float_argument_0
		{
			get { return _m_float_argument_0; }
			set { _m_float_argument_0 = value; this.Invalidate(); }
		}
		
		private float _m_float_argument_1;
		[STNodeProperty("float_argument_1", "float_argument_1")]
		public float m_float_argument_1
		{
			get { return _m_float_argument_1; }
			set { _m_float_argument_1 = value; this.Invalidate(); }
		}
		
		private float _m_float_argument_2;
		[STNodeProperty("float_argument_2", "float_argument_2")]
		public float m_float_argument_2
		{
			get { return _m_float_argument_2; }
			set { _m_float_argument_2 = value; this.Invalidate(); }
		}
		
		private float _m_float_argument_3;
		[STNodeProperty("float_argument_3", "float_argument_3")]
		public float m_float_argument_3
		{
			get { return _m_float_argument_3; }
			set { _m_float_argument_3 = value; this.Invalidate(); }
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
			
			this.Title = "FlashInvoke";
			
			this.InputOptions.Add("layer_name", typeof(string), false);
			this.InputOptions.Add("mrtt_texture", typeof(string), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
