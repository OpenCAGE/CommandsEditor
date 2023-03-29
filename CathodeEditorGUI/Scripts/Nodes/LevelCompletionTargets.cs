#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class LevelCompletionTargets : STNode
	{
		private float _m_TargetTime;
		[STNodeProperty("TargetTime", "TargetTime")]
		public float m_TargetTime
		{
			get { return _m_TargetTime; }
			set { _m_TargetTime = value; this.Invalidate(); }
		}
		
		private int _m_NumDeaths;
		[STNodeProperty("NumDeaths", "NumDeaths")]
		public int m_NumDeaths
		{
			get { return _m_NumDeaths; }
			set { _m_NumDeaths = value; this.Invalidate(); }
		}
		
		private int _m_TeamRespawnBonus;
		[STNodeProperty("TeamRespawnBonus", "TeamRespawnBonus")]
		public int m_TeamRespawnBonus
		{
			get { return _m_TeamRespawnBonus; }
			set { _m_TeamRespawnBonus = value; this.Invalidate(); }
		}
		
		private int _m_NoLocalRespawnBonus;
		[STNodeProperty("NoLocalRespawnBonus", "NoLocalRespawnBonus")]
		public int m_NoLocalRespawnBonus
		{
			get { return _m_NoLocalRespawnBonus; }
			set { _m_NoLocalRespawnBonus = value; this.Invalidate(); }
		}
		
		private int _m_NoRespawnBonus;
		[STNodeProperty("NoRespawnBonus", "NoRespawnBonus")]
		public int m_NoRespawnBonus
		{
			get { return _m_NoRespawnBonus; }
			set { _m_NoRespawnBonus = value; this.Invalidate(); }
		}
		
		private int _m_GrappleBreakBonus;
		[STNodeProperty("GrappleBreakBonus", "GrappleBreakBonus")]
		public int m_GrappleBreakBonus
		{
			get { return _m_GrappleBreakBonus; }
			set { _m_GrappleBreakBonus = value; this.Invalidate(); }
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
			
			this.Title = "LevelCompletionTargets";
			
			this.InputOptions.Add("set_true", typeof(void), false);
			
			this.OutputOptions.Add("set_to_true", typeof(void), false);
		}
	}
}
#endif
