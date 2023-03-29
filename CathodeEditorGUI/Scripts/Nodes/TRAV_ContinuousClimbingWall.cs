#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class TRAV_ContinuousClimbingWall : STNode
	{
		private string _m_Dangling;
		[STNodeProperty("Dangling", "Dangling")]
		public string m_Dangling
		{
			get { return _m_Dangling; }
			set { _m_Dangling = value; this.Invalidate(); }
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
			
			this.Title = "TRAV_ContinuousClimbingWall";
			
			this.InputOptions.Add("LinePath", typeof(string), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			
			this.OutputOptions.Add("OnEnter", typeof(void), false);
			this.OutputOptions.Add("OnExit", typeof(void), false);
			this.OutputOptions.Add("InUse", typeof(bool), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
		}
	}
}
#endif
