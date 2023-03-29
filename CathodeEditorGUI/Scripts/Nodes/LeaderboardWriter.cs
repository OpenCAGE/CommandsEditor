#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class LeaderboardWriter : STNode
	{
		private float _m_time_elapsed;
		[STNodeProperty("time_elapsed", "time_elapsed")]
		public float m_time_elapsed
		{
			get { return _m_time_elapsed; }
			set { _m_time_elapsed = value; this.Invalidate(); }
		}
		
		private int _m_score;
		[STNodeProperty("score", "score")]
		public int m_score
		{
			get { return _m_score; }
			set { _m_score = value; this.Invalidate(); }
		}
		
		private int _m_level_number;
		[STNodeProperty("level_number", "level_number")]
		public int m_level_number
		{
			get { return _m_level_number; }
			set { _m_level_number = value; this.Invalidate(); }
		}
		
		private int _m_grade;
		[STNodeProperty("grade", "grade")]
		public int m_grade
		{
			get { return _m_grade; }
			set { _m_grade = value; this.Invalidate(); }
		}
		
		private int _m_player_character;
		[STNodeProperty("player_character", "player_character")]
		public int m_player_character
		{
			get { return _m_player_character; }
			set { _m_player_character = value; this.Invalidate(); }
		}
		
		private int _m_combat;
		[STNodeProperty("combat", "combat")]
		public int m_combat
		{
			get { return _m_combat; }
			set { _m_combat = value; this.Invalidate(); }
		}
		
		private int _m_stealth;
		[STNodeProperty("stealth", "stealth")]
		public int m_stealth
		{
			get { return _m_stealth; }
			set { _m_stealth = value; this.Invalidate(); }
		}
		
		private int _m_improv;
		[STNodeProperty("improv", "improv")]
		public int m_improv
		{
			get { return _m_improv; }
			set { _m_improv = value; this.Invalidate(); }
		}
		
		private bool _m_star1;
		[STNodeProperty("star1", "star1")]
		public bool m_star1
		{
			get { return _m_star1; }
			set { _m_star1 = value; this.Invalidate(); }
		}
		
		private bool _m_star2;
		[STNodeProperty("star2", "star2")]
		public bool m_star2
		{
			get { return _m_star2; }
			set { _m_star2 = value; this.Invalidate(); }
		}
		
		private bool _m_star3;
		[STNodeProperty("star3", "star3")]
		public bool m_star3
		{
			get { return _m_star3; }
			set { _m_star3 = value; this.Invalidate(); }
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
			
			this.Title = "LeaderboardWriter";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
