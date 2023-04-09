using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SetupGCDistribution : STNode
	{
		private float _m_c00;
		[STNodeProperty("c00", "c00")]
		public float m_c00
		{
			get { return _m_c00; }
			set { _m_c00 = value; this.Invalidate(); }
		}
		
		private float _m_c01;
		[STNodeProperty("c01", "c01")]
		public float m_c01
		{
			get { return _m_c01; }
			set { _m_c01 = value; this.Invalidate(); }
		}
		
		private float _m_c02;
		[STNodeProperty("c02", "c02")]
		public float m_c02
		{
			get { return _m_c02; }
			set { _m_c02 = value; this.Invalidate(); }
		}
		
		private float _m_c03;
		[STNodeProperty("c03", "c03")]
		public float m_c03
		{
			get { return _m_c03; }
			set { _m_c03 = value; this.Invalidate(); }
		}
		
		private float _m_c04;
		[STNodeProperty("c04", "c04")]
		public float m_c04
		{
			get { return _m_c04; }
			set { _m_c04 = value; this.Invalidate(); }
		}
		
		private float _m_c05;
		[STNodeProperty("c05", "c05")]
		public float m_c05
		{
			get { return _m_c05; }
			set { _m_c05 = value; this.Invalidate(); }
		}
		
		private float _m_c06;
		[STNodeProperty("c06", "c06")]
		public float m_c06
		{
			get { return _m_c06; }
			set { _m_c06 = value; this.Invalidate(); }
		}
		
		private float _m_c07;
		[STNodeProperty("c07", "c07")]
		public float m_c07
		{
			get { return _m_c07; }
			set { _m_c07 = value; this.Invalidate(); }
		}
		
		private float _m_c08;
		[STNodeProperty("c08", "c08")]
		public float m_c08
		{
			get { return _m_c08; }
			set { _m_c08 = value; this.Invalidate(); }
		}
		
		private float _m_c09;
		[STNodeProperty("c09", "c09")]
		public float m_c09
		{
			get { return _m_c09; }
			set { _m_c09 = value; this.Invalidate(); }
		}
		
		private float _m_c10;
		[STNodeProperty("c10", "c10")]
		public float m_c10
		{
			get { return _m_c10; }
			set { _m_c10 = value; this.Invalidate(); }
		}
		
		private float _m_minimum_multiplier;
		[STNodeProperty("minimum_multiplier", "minimum_multiplier")]
		public float m_minimum_multiplier
		{
			get { return _m_minimum_multiplier; }
			set { _m_minimum_multiplier = value; this.Invalidate(); }
		}
		
		private float _m_divisor;
		[STNodeProperty("divisor", "divisor")]
		public float m_divisor
		{
			get { return _m_divisor; }
			set { _m_divisor = value; this.Invalidate(); }
		}
		
		private float _m_lookup_decrease_time;
		[STNodeProperty("lookup_decrease_time", "lookup_decrease_time")]
		public float m_lookup_decrease_time
		{
			get { return _m_lookup_decrease_time; }
			set { _m_lookup_decrease_time = value; this.Invalidate(); }
		}
		
		private int _m_lookup_point_increase;
		[STNodeProperty("lookup_point_increase", "lookup_point_increase")]
		public int m_lookup_point_increase
		{
			get { return _m_lookup_point_increase; }
			set { _m_lookup_point_increase = value; this.Invalidate(); }
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
			
			this.Title = "SetupGCDistribution";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
