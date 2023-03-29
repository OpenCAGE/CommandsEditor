#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_SetFiringRhythm : STNode
	{
		private float _m_MinShootingTime;
		[STNodeProperty("MinShootingTime", "MinShootingTime")]
		public float m_MinShootingTime
		{
			get { return _m_MinShootingTime; }
			set { _m_MinShootingTime = value; this.Invalidate(); }
		}
		
		private float _m_RandomRangeShootingTime;
		[STNodeProperty("RandomRangeShootingTime", "RandomRangeShootingTime")]
		public float m_RandomRangeShootingTime
		{
			get { return _m_RandomRangeShootingTime; }
			set { _m_RandomRangeShootingTime = value; this.Invalidate(); }
		}
		
		private float _m_MinNonShootingTime;
		[STNodeProperty("MinNonShootingTime", "MinNonShootingTime")]
		public float m_MinNonShootingTime
		{
			get { return _m_MinNonShootingTime; }
			set { _m_MinNonShootingTime = value; this.Invalidate(); }
		}
		
		private float _m_RandomRangeNonShootingTime;
		[STNodeProperty("RandomRangeNonShootingTime", "RandomRangeNonShootingTime")]
		public float m_RandomRangeNonShootingTime
		{
			get { return _m_RandomRangeNonShootingTime; }
			set { _m_RandomRangeNonShootingTime = value; this.Invalidate(); }
		}
		
		private float _m_MinCoverNonShootingTime;
		[STNodeProperty("MinCoverNonShootingTime", "MinCoverNonShootingTime")]
		public float m_MinCoverNonShootingTime
		{
			get { return _m_MinCoverNonShootingTime; }
			set { _m_MinCoverNonShootingTime = value; this.Invalidate(); }
		}
		
		private float _m_RandomRangeCoverNonShootingTime;
		[STNodeProperty("RandomRangeCoverNonShootingTime", "RandomRangeCoverNonShootingTime")]
		public float m_RandomRangeCoverNonShootingTime
		{
			get { return _m_RandomRangeCoverNonShootingTime; }
			set { _m_RandomRangeCoverNonShootingTime = value; this.Invalidate(); }
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
			
			this.Title = "NPC_SetFiringRhythm";
			
			this.InputOptions.Add("set", typeof(void), false);
			
			this.OutputOptions.Add("been_set", typeof(void), false);
		}
	}
}
#endif
