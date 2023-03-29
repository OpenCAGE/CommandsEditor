#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class EggSpawner : STNode
	{
		private cTransform _m_egg_position;
		[STNodeProperty("egg_position", "egg_position")]
		public cTransform m_egg_position
		{
			get { return _m_egg_position; }
			set { _m_egg_position = value; this.Invalidate(); }
		}
		
		private bool _m_hostile_egg;
		[STNodeProperty("hostile_egg", "hostile_egg")]
		public bool m_hostile_egg
		{
			get { return _m_hostile_egg; }
			set { _m_hostile_egg = value; this.Invalidate(); }
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
			
			this.Title = "EggSpawner";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
