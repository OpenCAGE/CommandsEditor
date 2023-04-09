using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CharacterAttachmentNode : STNode
	{
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
		}
		
		private string _m_Node;
		[STNodeProperty("Node", "Node")]
		public string m_Node
		{
			get { return _m_Node; }
			set { _m_Node = value; this.Invalidate(); }
		}
		
		private string _m_AdditiveNode;
		[STNodeProperty("AdditiveNode", "AdditiveNode")]
		public string m_AdditiveNode
		{
			get { return _m_AdditiveNode; }
			set { _m_AdditiveNode = value; this.Invalidate(); }
		}
		
		private float _m_AdditiveNodeIntensity;
		[STNodeProperty("AdditiveNodeIntensity", "AdditiveNodeIntensity")]
		public float m_AdditiveNodeIntensity
		{
			get { return _m_AdditiveNodeIntensity; }
			set { _m_AdditiveNodeIntensity = value; this.Invalidate(); }
		}
		
		private bool _m_UseOffset;
		[STNodeProperty("UseOffset", "UseOffset")]
		public bool m_UseOffset
		{
			get { return _m_UseOffset; }
			set { _m_UseOffset = value; this.Invalidate(); }
		}
		
		private cVector3 _m_Translation;
		[STNodeProperty("Translation", "Translation")]
		public cVector3 m_Translation
		{
			get { return _m_Translation; }
			set { _m_Translation = value; this.Invalidate(); }
		}
		
		private cVector3 _m_Rotation;
		[STNodeProperty("Rotation", "Rotation")]
		public cVector3 m_Rotation
		{
			get { return _m_Rotation; }
			set { _m_Rotation = value; this.Invalidate(); }
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
			
			this.Title = "CharacterAttachmentNode";
			
			this.InputOptions.Add("character", typeof(string), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}
	}
}
