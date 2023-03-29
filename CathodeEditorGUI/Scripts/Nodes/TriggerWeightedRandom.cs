#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerWeightedRandom : STNode
	{
		private float _m_Weighting_01;
		[STNodeProperty("Weighting_01", "Weighting_01")]
		public float m_Weighting_01
		{
			get { return _m_Weighting_01; }
			set { _m_Weighting_01 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_02;
		[STNodeProperty("Weighting_02", "Weighting_02")]
		public float m_Weighting_02
		{
			get { return _m_Weighting_02; }
			set { _m_Weighting_02 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_03;
		[STNodeProperty("Weighting_03", "Weighting_03")]
		public float m_Weighting_03
		{
			get { return _m_Weighting_03; }
			set { _m_Weighting_03 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_04;
		[STNodeProperty("Weighting_04", "Weighting_04")]
		public float m_Weighting_04
		{
			get { return _m_Weighting_04; }
			set { _m_Weighting_04 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_05;
		[STNodeProperty("Weighting_05", "Weighting_05")]
		public float m_Weighting_05
		{
			get { return _m_Weighting_05; }
			set { _m_Weighting_05 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_06;
		[STNodeProperty("Weighting_06", "Weighting_06")]
		public float m_Weighting_06
		{
			get { return _m_Weighting_06; }
			set { _m_Weighting_06 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_07;
		[STNodeProperty("Weighting_07", "Weighting_07")]
		public float m_Weighting_07
		{
			get { return _m_Weighting_07; }
			set { _m_Weighting_07 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_08;
		[STNodeProperty("Weighting_08", "Weighting_08")]
		public float m_Weighting_08
		{
			get { return _m_Weighting_08; }
			set { _m_Weighting_08 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_09;
		[STNodeProperty("Weighting_09", "Weighting_09")]
		public float m_Weighting_09
		{
			get { return _m_Weighting_09; }
			set { _m_Weighting_09 = value; this.Invalidate(); }
		}
		
		private float _m_Weighting_10;
		[STNodeProperty("Weighting_10", "Weighting_10")]
		public float m_Weighting_10
		{
			get { return _m_Weighting_10; }
			set { _m_Weighting_10 = value; this.Invalidate(); }
		}
		
		private bool _m_allow_same_pin_in_succession;
		[STNodeProperty("allow_same_pin_in_succession", "allow_same_pin_in_succession")]
		public bool m_allow_same_pin_in_succession
		{
			get { return _m_allow_same_pin_in_succession; }
			set { _m_allow_same_pin_in_succession = value; this.Invalidate(); }
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
			
			this.Title = "TriggerWeightedRandom";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Random_1", typeof(void), false);
			this.OutputOptions.Add("Random_2", typeof(void), false);
			this.OutputOptions.Add("Random_3", typeof(void), false);
			this.OutputOptions.Add("Random_4", typeof(void), false);
			this.OutputOptions.Add("Random_5", typeof(void), false);
			this.OutputOptions.Add("Random_6", typeof(void), false);
			this.OutputOptions.Add("Random_7", typeof(void), false);
			this.OutputOptions.Add("Random_8", typeof(void), false);
			this.OutputOptions.Add("Random_9", typeof(void), false);
			this.OutputOptions.Add("Random_10", typeof(void), false);
			this.OutputOptions.Add("current", typeof(int), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
