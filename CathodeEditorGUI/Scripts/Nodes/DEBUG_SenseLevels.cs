#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class DEBUG_SenseLevels : STNode
	{
		private string _m_Sense;
		[STNodeProperty("Sense", "Sense")]
		public string m_Sense
		{
			get { return _m_Sense; }
			set { _m_Sense = value; this.Invalidate(); }
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
			
			this.Title = "DEBUG_SenseLevels";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("no_activation", typeof(void), false);
			this.OutputOptions.Add("trace_activation", typeof(void), false);
			this.OutputOptions.Add("lower_activation", typeof(void), false);
			this.OutputOptions.Add("normal_activation", typeof(void), false);
			this.OutputOptions.Add("upper_activation", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
