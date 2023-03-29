#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_SetRateOfFire : STNode
	{
		private float _m_MinTimeBetweenShots;
		[STNodeProperty("MinTimeBetweenShots", "MinTimeBetweenShots")]
		public float m_MinTimeBetweenShots
		{
			get { return _m_MinTimeBetweenShots; }
			set { _m_MinTimeBetweenShots = value; this.Invalidate(); }
		}
		
		private float _m_RandomRange;
		[STNodeProperty("RandomRange", "RandomRange")]
		public float m_RandomRange
		{
			get { return _m_RandomRange; }
			set { _m_RandomRange = value; this.Invalidate(); }
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
			
			this.Title = "NPC_SetRateOfFire";
			
			this.InputOptions.Add("set", typeof(void), false);
			
			this.OutputOptions.Add("been_set", typeof(void), false);
		}
	}
}
#endif
