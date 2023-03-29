#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CharacterShivaArms : STNode
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
			
			this.Title = "CharacterShivaArms";
			
			this.InputOptions.Add("apply_hide", typeof(void), false);
			this.InputOptions.Add("apply_show", typeof(void), false);
			
			this.OutputOptions.Add("hide_applied", typeof(void), false);
			this.OutputOptions.Add("show_applied", typeof(void), false);
		}
	}
}
#endif
