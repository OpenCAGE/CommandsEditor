using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TRAV_1ShotLeap : STNode
	{
		private bool _m_enable_on_reset;
		[STNodeProperty("enable_on_reset", "enable_on_reset")]
		public bool m_enable_on_reset
		{
			get { return _m_enable_on_reset; }
			set { _m_enable_on_reset = value; this.Invalidate(); }
		}
		
		private float _m_MissDistance;
		[STNodeProperty("MissDistance", "MissDistance")]
		public float m_MissDistance
		{
			get { return _m_MissDistance; }
			set { _m_MissDistance = value; this.Invalidate(); }
		}
		
		private float _m_NearMissDistance;
		[STNodeProperty("NearMissDistance", "NearMissDistance")]
		public float m_NearMissDistance
		{
			get { return _m_NearMissDistance; }
			set { _m_NearMissDistance = value; this.Invalidate(); }
		}
		
		private string _m_character_classes;
		[STNodeProperty("character_classes", "character_classes")]
		public string m_character_classes
		{
			get { return _m_character_classes; }
			set { _m_character_classes = value; this.Invalidate(); }
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
			
			this.Title = "TRAV_1ShotLeap";
			
			this.InputOptions.Add("StartEdgeLinePath", typeof(string), false);
			this.InputOptions.Add("EndEdgeLinePath", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			
			this.OutputOptions.Add("OnEnter", typeof(void), false);
			this.OutputOptions.Add("OnExit", typeof(void), false);
			this.OutputOptions.Add("OnSuccess", typeof(void), false);
			this.OutputOptions.Add("OnFailure", typeof(void), false);
			this.OutputOptions.Add("InUse", typeof(bool), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
		}
	}
}
