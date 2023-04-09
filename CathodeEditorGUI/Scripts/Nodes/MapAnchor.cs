using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class MapAnchor : STNode
	{
		private string _m_keyframe;
		[STNodeProperty("keyframe", "keyframe")]
		public string m_keyframe
		{
			get { return _m_keyframe; }
			set { _m_keyframe = value; this.Invalidate(); }
		}
		
		private string _m_keyframe1;
		[STNodeProperty("keyframe1", "keyframe1")]
		public string m_keyframe1
		{
			get { return _m_keyframe1; }
			set { _m_keyframe1 = value; this.Invalidate(); }
		}
		
		private string _m_keyframe2;
		[STNodeProperty("keyframe2", "keyframe2")]
		public string m_keyframe2
		{
			get { return _m_keyframe2; }
			set { _m_keyframe2 = value; this.Invalidate(); }
		}
		
		private string _m_keyframe3;
		[STNodeProperty("keyframe3", "keyframe3")]
		public string m_keyframe3
		{
			get { return _m_keyframe3; }
			set { _m_keyframe3 = value; this.Invalidate(); }
		}
		
		private string _m_keyframe4;
		[STNodeProperty("keyframe4", "keyframe4")]
		public string m_keyframe4
		{
			get { return _m_keyframe4; }
			set { _m_keyframe4 = value; this.Invalidate(); }
		}
		
		private string _m_keyframe5;
		[STNodeProperty("keyframe5", "keyframe5")]
		public string m_keyframe5
		{
			get { return _m_keyframe5; }
			set { _m_keyframe5 = value; this.Invalidate(); }
		}
		
		private cTransform _m_world_pos;
		[STNodeProperty("world_pos", "world_pos")]
		public cTransform m_world_pos
		{
			get { return _m_world_pos; }
			set { _m_world_pos = value; this.Invalidate(); }
		}
		
		private bool _m_is_default_for_items;
		[STNodeProperty("is_default_for_items", "is_default_for_items")]
		public bool m_is_default_for_items
		{
			get { return _m_is_default_for_items; }
			set { _m_is_default_for_items = value; this.Invalidate(); }
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
			
			this.Title = "MapAnchor";
			
			this.InputOptions.Add("map_north", typeof(cVector3), false);
			this.InputOptions.Add("map_pos", typeof(cVector3), false);
			this.InputOptions.Add("map_scale", typeof(float), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
