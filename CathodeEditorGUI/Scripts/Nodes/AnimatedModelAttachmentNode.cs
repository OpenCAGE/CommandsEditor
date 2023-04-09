using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AnimatedModelAttachmentNode : STNode
	{
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_bone_name;
		[STNodeProperty("bone_name", "bone_name")]
		public string m_bone_name
		{
			get { return _m_bone_name; }
			set { _m_bone_name = value; this.Invalidate(); }
		}
		
		private bool _m_use_offset;
		[STNodeProperty("use_offset", "use_offset")]
		public bool m_use_offset
		{
			get { return _m_use_offset; }
			set { _m_use_offset = value; this.Invalidate(); }
		}
		
		private cTransform _m_offset;
		[STNodeProperty("offset", "offset")]
		public cTransform m_offset
		{
			get { return _m_offset; }
			set { _m_offset = value; this.Invalidate(); }
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
			
			this.Title = "AnimatedModelAttachmentNode";
			
			this.InputOptions.Add("animated_model", typeof(STNode), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
