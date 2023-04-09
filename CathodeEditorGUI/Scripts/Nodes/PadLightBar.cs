using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class PadLightBar : STNode
	{
		private cVector3 _m_colour;
		[STNodeProperty("colour", "colour")]
		public cVector3 m_colour
		{
			get { return _m_colour; }
			set { _m_colour = value; this.Invalidate(); }
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
			
			this.Title = "PadLightBar";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
