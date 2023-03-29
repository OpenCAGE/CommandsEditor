#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_SetTacticalPosition : STNode
	{
		private string _m_sweep_type;
		[STNodeProperty("sweep_type", "sweep_type")]
		public string m_sweep_type
		{
			get { return _m_sweep_type; }
			set { _m_sweep_type = value; this.Invalidate(); }
		}
		
		private float _m_fixed_sweep_radius;
		[STNodeProperty("fixed_sweep_radius", "fixed_sweep_radius")]
		public float m_fixed_sweep_radius
		{
			get { return _m_fixed_sweep_radius; }
			set { _m_fixed_sweep_radius = value; this.Invalidate(); }
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
			
			this.Title = "CHR_SetTacticalPosition";
			
			this.InputOptions.Add("tactical_position", typeof(cTransform), false);
			this.InputOptions.Add("set", typeof(void), false);
			this.InputOptions.Add("clear", typeof(void), false);
			
			this.OutputOptions.Add("been_set", typeof(void), false);
			this.OutputOptions.Add("cleared", typeof(void), false);
		}
	}
}
#endif
