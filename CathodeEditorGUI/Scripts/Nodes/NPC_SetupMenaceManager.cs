#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_SetupMenaceManager : STNode
	{
		private bool _m_AgressiveMenace;
		[STNodeProperty("AgressiveMenace", "AgressiveMenace")]
		public bool m_AgressiveMenace
		{
			get { return _m_AgressiveMenace; }
			set { _m_AgressiveMenace = value; this.Invalidate(); }
		}
		
		private float _m_ProgressionFraction;
		[STNodeProperty("ProgressionFraction", "ProgressionFraction")]
		public float m_ProgressionFraction
		{
			get { return _m_ProgressionFraction; }
			set { _m_ProgressionFraction = value; this.Invalidate(); }
		}
		
		private bool _m_ResetMenaceMeter;
		[STNodeProperty("ResetMenaceMeter", "ResetMenaceMeter")]
		public bool m_ResetMenaceMeter
		{
			get { return _m_ResetMenaceMeter; }
			set { _m_ResetMenaceMeter = value; this.Invalidate(); }
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
			
			this.Title = "NPC_SetupMenaceManager";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
