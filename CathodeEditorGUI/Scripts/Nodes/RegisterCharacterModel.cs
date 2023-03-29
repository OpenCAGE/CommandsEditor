#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class RegisterCharacterModel : STNode
	{
		private string _m_display_model;
		[STNodeProperty("display_model", "display_model")]
		public string m_display_model
		{
			get { return _m_display_model; }
			set { _m_display_model = value; this.Invalidate(); }
		}
		
		private string _m_reference_skeleton;
		[STNodeProperty("reference_skeleton", "reference_skeleton")]
		public string m_reference_skeleton
		{
			get { return _m_reference_skeleton; }
			set { _m_reference_skeleton = value; this.Invalidate(); }
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
			
			this.Title = "RegisterCharacterModel";
			
			
		}
	}
}
#endif
