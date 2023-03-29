#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class FakeAILightSourceInPlayersHand : STNode
	{
		private float _m_radius;
		[STNodeProperty("radius", "radius")]
		public float m_radius
		{
			get { return _m_radius; }
			set { _m_radius = value; this.Invalidate(); }
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
			
			this.Title = "FakeAILightSourceInPlayersHand";
			
			this.InputOptions.Add("fake_light_on", typeof(void), false);
			this.InputOptions.Add("fake_light_off", typeof(void), false);
			
			this.OutputOptions.Add("fake_light_on_triggered", typeof(void), false);
			this.OutputOptions.Add("fake_light_off_triggered", typeof(void), false);
		}
	}
}
#endif
