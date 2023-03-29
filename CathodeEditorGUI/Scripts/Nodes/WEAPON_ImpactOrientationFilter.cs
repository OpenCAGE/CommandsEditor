#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_ImpactOrientationFilter : STNode
	{
		private float _m_ThresholdAngle;
		[STNodeProperty("ThresholdAngle", "ThresholdAngle")]
		public float m_ThresholdAngle
		{
			get { return _m_ThresholdAngle; }
			set { _m_ThresholdAngle = value; this.Invalidate(); }
		}
		
		private string _m_Orientation;
		[STNodeProperty("Orientation", "Orientation")]
		public string m_Orientation
		{
			get { return _m_Orientation; }
			set { _m_Orientation = value; this.Invalidate(); }
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
			
			this.Title = "WEAPON_ImpactOrientationFilter";
			
			this.InputOptions.Add("impact", typeof(void), false);
			
			this.OutputOptions.Add("passed", typeof(void), false);
			this.OutputOptions.Add("failed", typeof(void), false);
			this.OutputOptions.Add("impacted", typeof(void), false);
		}
	}
}
#endif
