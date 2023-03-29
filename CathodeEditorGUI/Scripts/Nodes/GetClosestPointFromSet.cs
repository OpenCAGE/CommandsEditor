#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class GetClosestPointFromSet : STNode
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
			
			this.Title = "GetClosestPointFromSet";
			
			this.InputOptions.Add("Position_1", typeof(STNode), false);
			this.InputOptions.Add("Position_2", typeof(STNode), false);
			this.InputOptions.Add("Position_3", typeof(STNode), false);
			this.InputOptions.Add("Position_4", typeof(STNode), false);
			this.InputOptions.Add("Position_5", typeof(STNode), false);
			this.InputOptions.Add("Position_6", typeof(STNode), false);
			this.InputOptions.Add("Position_7", typeof(STNode), false);
			this.InputOptions.Add("Position_8", typeof(STNode), false);
			this.InputOptions.Add("Position_9", typeof(STNode), false);
			this.InputOptions.Add("Position_10", typeof(STNode), false);
			this.InputOptions.Add("pos_to_be_near", typeof(cTransform), false);
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("closest_is_1", typeof(void), false);
			this.OutputOptions.Add("closest_is_2", typeof(void), false);
			this.OutputOptions.Add("closest_is_3", typeof(void), false);
			this.OutputOptions.Add("closest_is_4", typeof(void), false);
			this.OutputOptions.Add("closest_is_5", typeof(void), false);
			this.OutputOptions.Add("closest_is_6", typeof(void), false);
			this.OutputOptions.Add("closest_is_7", typeof(void), false);
			this.OutputOptions.Add("closest_is_8", typeof(void), false);
			this.OutputOptions.Add("closest_is_9", typeof(void), false);
			this.OutputOptions.Add("closest_is_10", typeof(void), false);
			this.OutputOptions.Add("position_of_closest", typeof(cTransform), false);
			this.OutputOptions.Add("index_of_closest", typeof(int), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
