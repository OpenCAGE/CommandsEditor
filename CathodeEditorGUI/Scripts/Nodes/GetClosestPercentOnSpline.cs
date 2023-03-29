#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GetClosestPercentOnSpline : STNode
	{
		private bool _m_bidirectional;
		[STNodeProperty("bidirectional", "bidirectional")]
		public bool m_bidirectional
		{
			get { return _m_bidirectional; }
			set { _m_bidirectional = value; this.Invalidate(); }
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
			
			this.Title = "GetClosestPercentOnSpline";
			
			this.InputOptions.Add("spline", typeof(STNode), false);
			this.InputOptions.Add("pos_to_be_near", typeof(cVector3), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("position_on_spline", typeof(cTransform), false);
			this.OutputOptions.Add("Result", typeof(float), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
