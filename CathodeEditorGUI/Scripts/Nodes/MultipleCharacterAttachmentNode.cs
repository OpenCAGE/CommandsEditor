#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MultipleCharacterAttachmentNode : STNode
	{
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_node;
		[STNodeProperty("node", "node")]
		public string m_node
		{
			get { return _m_node; }
			set { _m_node = value; this.Invalidate(); }
		}
		
		private bool _m_use_offset;
		[STNodeProperty("use_offset", "use_offset")]
		public bool m_use_offset
		{
			get { return _m_use_offset; }
			set { _m_use_offset = value; this.Invalidate(); }
		}
		
		private cVector3 _m_translation;
		[STNodeProperty("translation", "translation")]
		public cVector3 m_translation
		{
			get { return _m_translation; }
			set { _m_translation = value; this.Invalidate(); }
		}
		
		private cVector3 _m_rotation;
		[STNodeProperty("rotation", "rotation")]
		public cVector3 m_rotation
		{
			get { return _m_rotation; }
			set { _m_rotation = value; this.Invalidate(); }
		}
		
		private bool _m_is_cinematic;
		[STNodeProperty("is_cinematic", "is_cinematic")]
		public bool m_is_cinematic
		{
			get { return _m_is_cinematic; }
			set { _m_is_cinematic = value; this.Invalidate(); }
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
			
			this.Title = "MultipleCharacterAttachmentNode";
			
			this.InputOptions.Add("character_01", typeof(string), false);
			this.InputOptions.Add("attachment_01", typeof(STNode), false);
			this.InputOptions.Add("character_02", typeof(string), false);
			this.InputOptions.Add("attachment_02", typeof(STNode), false);
			this.InputOptions.Add("character_03", typeof(string), false);
			this.InputOptions.Add("attachment_03", typeof(STNode), false);
			this.InputOptions.Add("character_04", typeof(string), false);
			this.InputOptions.Add("attachment_04", typeof(STNode), false);
			this.InputOptions.Add("character_05", typeof(string), false);
			this.InputOptions.Add("attachment_05", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
#endif
