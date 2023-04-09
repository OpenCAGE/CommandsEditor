using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TriggerDelay : STNode
	{
		private float _m_Hrs;
		[STNodeProperty("Hrs", "Hrs")]
		public float m_Hrs
		{
			get { return _m_Hrs; }
			set { _m_Hrs = value; this.Invalidate(); }
		}
		
		private float _m_Min;
		[STNodeProperty("Min", "Min")]
		public float m_Min
		{
			get { return _m_Min; }
			set { _m_Min = value; this.Invalidate(); }
		}
		
		private float _m_Sec;
		[STNodeProperty("Sec", "Sec")]
		public float m_Sec
		{
			get { return _m_Sec; }
			set { _m_Sec = value; this.Invalidate(); }
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
			
			this.Title = "TriggerDelay";
			
			this.InputOptions.Add("abort", typeof(void), false);
			this.InputOptions.Add("purge", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("delayed_trigger", typeof(void), false);
			this.OutputOptions.Add("purged_trigger", typeof(void), false);
			this.OutputOptions.Add("time_left", typeof(float), false);
			this.OutputOptions.Add("aborted", typeof(void), false);
			this.OutputOptions.Add("purged", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
