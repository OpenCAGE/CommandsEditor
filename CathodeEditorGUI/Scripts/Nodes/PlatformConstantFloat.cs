using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PlatformConstantFloat : STNode
	{
		private float _m_NextGen;
		[STNodeProperty("NextGen", "NextGen")]
		public float m_NextGen
		{
			get { return _m_NextGen; }
			set { _m_NextGen = value; this.Invalidate(); }
		}
		
		private float _m_X360;
		[STNodeProperty("X360", "X360")]
		public float m_X360
		{
			get { return _m_X360; }
			set { _m_X360 = value; this.Invalidate(); }
		}
		
		private float _m_PS3;
		[STNodeProperty("PS3", "PS3")]
		public float m_PS3
		{
			get { return _m_PS3; }
			set { _m_PS3 = value; this.Invalidate(); }
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
			
			this.Title = "PlatformConstantFloat";
			
			
		}
	}
}
