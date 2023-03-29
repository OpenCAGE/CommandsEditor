#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AILightCurveSettings : STNode
	{
		private float _m_y0;
		[STNodeProperty("y0", "y0")]
		public float m_y0
		{
			get { return _m_y0; }
			set { _m_y0 = value; this.Invalidate(); }
		}
		
		private float _m_x1;
		[STNodeProperty("x1", "x1")]
		public float m_x1
		{
			get { return _m_x1; }
			set { _m_x1 = value; this.Invalidate(); }
		}
		
		private float _m_y1;
		[STNodeProperty("y1", "y1")]
		public float m_y1
		{
			get { return _m_y1; }
			set { _m_y1 = value; this.Invalidate(); }
		}
		
		private float _m_x2;
		[STNodeProperty("x2", "x2")]
		public float m_x2
		{
			get { return _m_x2; }
			set { _m_x2 = value; this.Invalidate(); }
		}
		
		private float _m_y2;
		[STNodeProperty("y2", "y2")]
		public float m_y2
		{
			get { return _m_y2; }
			set { _m_y2 = value; this.Invalidate(); }
		}
		
		private float _m_x3;
		[STNodeProperty("x3", "x3")]
		public float m_x3
		{
			get { return _m_x3; }
			set { _m_x3 = value; this.Invalidate(); }
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
			
			this.Title = "AILightCurveSettings";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
