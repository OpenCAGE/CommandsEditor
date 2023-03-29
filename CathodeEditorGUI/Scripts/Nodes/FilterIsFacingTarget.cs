#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FilterIsFacingTarget : STNode
	{
		private float _m_tolerance;
		[STNodeProperty("tolerance", "tolerance")]
		public float m_tolerance
		{
			get { return _m_tolerance; }
			set { _m_tolerance = value; this.Invalidate(); }
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
			
			this.Title = "FilterIsFacingTarget";
			
			this.InputOptions.Add("target", typeof(cVector3), false);
			
		}
	}
}
#endif
