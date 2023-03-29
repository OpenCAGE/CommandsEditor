#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ALLIANCE_SetDisposition : STNode
	{
		private string _m_A;
		[STNodeProperty("A", "A")]
		public string m_A
		{
			get { return _m_A; }
			set { _m_A = value; this.Invalidate(); }
		}
		
		private string _m_B;
		[STNodeProperty("B", "B")]
		public string m_B
		{
			get { return _m_B; }
			set { _m_B = value; this.Invalidate(); }
		}
		
		private string _m_Disposition;
		[STNodeProperty("Disposition", "Disposition")]
		public string m_Disposition
		{
			get { return _m_Disposition; }
			set { _m_Disposition = value; this.Invalidate(); }
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
			
			this.Title = "ALLIANCE_SetDisposition";
			
			this.InputOptions.Add("set", typeof(void), false);
			
			this.OutputOptions.Add("been_set", typeof(void), false);
		}
	}
}
#endif
