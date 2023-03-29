#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RandomVector : STNode
	{
		private float _m_MinX;
		[STNodeProperty("MinX", "MinX")]
		public float m_MinX
		{
			get { return _m_MinX; }
			set { _m_MinX = value; this.Invalidate(); }
		}
		
		private float _m_MaxX;
		[STNodeProperty("MaxX", "MaxX")]
		public float m_MaxX
		{
			get { return _m_MaxX; }
			set { _m_MaxX = value; this.Invalidate(); }
		}
		
		private float _m_MinY;
		[STNodeProperty("MinY", "MinY")]
		public float m_MinY
		{
			get { return _m_MinY; }
			set { _m_MinY = value; this.Invalidate(); }
		}
		
		private float _m_MaxY;
		[STNodeProperty("MaxY", "MaxY")]
		public float m_MaxY
		{
			get { return _m_MaxY; }
			set { _m_MaxY = value; this.Invalidate(); }
		}
		
		private float _m_MinZ;
		[STNodeProperty("MinZ", "MinZ")]
		public float m_MinZ
		{
			get { return _m_MinZ; }
			set { _m_MinZ = value; this.Invalidate(); }
		}
		
		private float _m_MaxZ;
		[STNodeProperty("MaxZ", "MaxZ")]
		public float m_MaxZ
		{
			get { return _m_MaxZ; }
			set { _m_MaxZ = value; this.Invalidate(); }
		}
		
		private bool _m_Normalised;
		[STNodeProperty("Normalised", "Normalised")]
		public bool m_Normalised
		{
			get { return _m_Normalised; }
			set { _m_Normalised = value; this.Invalidate(); }
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
			
			this.Title = "RandomVector";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Result", typeof(cVector3), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
