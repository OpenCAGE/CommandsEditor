using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class BindObjectsMultiplexer : STNode
	{
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
			
			this.Title = "BindObjectsMultiplexer";
			
			this.InputOptions.Add("objects", typeof(STNode), false);
			this.InputOptions.Add("Pin1", typeof(void), false);
			this.InputOptions.Add("Pin2", typeof(void), false);
			this.InputOptions.Add("Pin3", typeof(void), false);
			this.InputOptions.Add("Pin4", typeof(void), false);
			this.InputOptions.Add("Pin5", typeof(void), false);
			this.InputOptions.Add("Pin6", typeof(void), false);
			this.InputOptions.Add("Pin7", typeof(void), false);
			this.InputOptions.Add("Pin8", typeof(void), false);
			this.InputOptions.Add("Pin9", typeof(void), false);
			this.InputOptions.Add("Pin10", typeof(void), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("Pin1_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin2_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin3_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin4_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin5_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin6_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin7_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin8_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin9_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin10_Bound", typeof(void), false);
			this.OutputOptions.Add("Pin1_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin2_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin3_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin4_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin5_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin6_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin7_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin8_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin9_Instant", typeof(void), false);
			this.OutputOptions.Add("Pin10_Instant", typeof(void), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
