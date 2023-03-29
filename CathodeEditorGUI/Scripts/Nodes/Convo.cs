#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class Convo : STNode
	{
		private bool _m_alwaysTalkToPlayerIfPresent;
		[STNodeProperty("alwaysTalkToPlayerIfPresent", "alwaysTalkToPlayerIfPresent")]
		public bool m_alwaysTalkToPlayerIfPresent
		{
			get { return _m_alwaysTalkToPlayerIfPresent; }
			set { _m_alwaysTalkToPlayerIfPresent = value; this.Invalidate(); }
		}
		
		private bool _m_playerCanJoin;
		[STNodeProperty("playerCanJoin", "playerCanJoin")]
		public bool m_playerCanJoin
		{
			get { return _m_playerCanJoin; }
			set { _m_playerCanJoin = value; this.Invalidate(); }
		}
		
		private bool _m_playerCanLeave;
		[STNodeProperty("playerCanLeave", "playerCanLeave")]
		public bool m_playerCanLeave
		{
			get { return _m_playerCanLeave; }
			set { _m_playerCanLeave = value; this.Invalidate(); }
		}
		
		private bool _m_positionNPCs;
		[STNodeProperty("positionNPCs", "positionNPCs")]
		public bool m_positionNPCs
		{
			get { return _m_positionNPCs; }
			set { _m_positionNPCs = value; this.Invalidate(); }
		}
		
		private bool _m_circularShape;
		[STNodeProperty("circularShape", "circularShape")]
		public bool m_circularShape
		{
			get { return _m_circularShape; }
			set { _m_circularShape = value; this.Invalidate(); }
		}
		
		private STNode _m_convoPosition;
		[STNodeProperty("convoPosition", "convoPosition")]
		public STNode m_convoPosition
		{
			get { return _m_convoPosition; }
			set { _m_convoPosition = value; this.Invalidate(); }
		}
		
		private float _m_personalSpaceRadius;
		[STNodeProperty("personalSpaceRadius", "personalSpaceRadius")]
		public float m_personalSpaceRadius
		{
			get { return _m_personalSpaceRadius; }
			set { _m_personalSpaceRadius = value; this.Invalidate(); }
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
			
			this.Title = "Convo";
			
			this.InputOptions.Add("members", typeof(string), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			
			this.OutputOptions.Add("everyoneArrived", typeof(void), false);
			this.OutputOptions.Add("playerJoined", typeof(void), false);
			this.OutputOptions.Add("playerLeft", typeof(void), false);
			this.OutputOptions.Add("npcJoined", typeof(void), false);
			this.OutputOptions.Add("speaker", typeof(STNode), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
		}
	}
}
#endif
