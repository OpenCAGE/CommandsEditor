using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class EnvironmentMap : STNode
	{
		private int _m_Priority;
		[STNodeProperty("Priority", "Priority")]
		public int m_Priority
		{
			get { return _m_Priority; }
			set { _m_Priority = value; this.Invalidate(); }
		}
		
		private cVector3 _m_ColourFactor;
		[STNodeProperty("ColourFactor", "ColourFactor")]
		public cVector3 m_ColourFactor
		{
			get { return _m_ColourFactor; }
			set { _m_ColourFactor = value; this.Invalidate(); }
		}
		
		private float _m_EmissiveFactor;
		[STNodeProperty("EmissiveFactor", "EmissiveFactor")]
		public float m_EmissiveFactor
		{
			get { return _m_EmissiveFactor; }
			set { _m_EmissiveFactor = value; this.Invalidate(); }
		}
		
		private string _m_Texture;
		[STNodeProperty("Texture", "Texture")]
		public string m_Texture
		{
			get { return _m_Texture; }
			set { _m_Texture = value; this.Invalidate(); }
		}
		
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
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
			
			this.Title = "EnvironmentMap";
			
			this.InputOptions.Add("Entities", typeof(STNode), false);
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("purge", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("purged", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
