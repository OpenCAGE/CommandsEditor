#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class AnimationMask : STNode
	{
		private bool _m_maskHips;
		[STNodeProperty("maskHips", "maskHips")]
		public bool m_maskHips
		{
			get { return _m_maskHips; }
			set { _m_maskHips = value; this.Invalidate(); }
		}
		
		private bool _m_maskTorso;
		[STNodeProperty("maskTorso", "maskTorso")]
		public bool m_maskTorso
		{
			get { return _m_maskTorso; }
			set { _m_maskTorso = value; this.Invalidate(); }
		}
		
		private bool _m_maskNeck;
		[STNodeProperty("maskNeck", "maskNeck")]
		public bool m_maskNeck
		{
			get { return _m_maskNeck; }
			set { _m_maskNeck = value; this.Invalidate(); }
		}
		
		private bool _m_maskHead;
		[STNodeProperty("maskHead", "maskHead")]
		public bool m_maskHead
		{
			get { return _m_maskHead; }
			set { _m_maskHead = value; this.Invalidate(); }
		}
		
		private bool _m_maskFace;
		[STNodeProperty("maskFace", "maskFace")]
		public bool m_maskFace
		{
			get { return _m_maskFace; }
			set { _m_maskFace = value; this.Invalidate(); }
		}
		
		private bool _m_maskLeftLeg;
		[STNodeProperty("maskLeftLeg", "maskLeftLeg")]
		public bool m_maskLeftLeg
		{
			get { return _m_maskLeftLeg; }
			set { _m_maskLeftLeg = value; this.Invalidate(); }
		}
		
		private bool _m_maskRightLeg;
		[STNodeProperty("maskRightLeg", "maskRightLeg")]
		public bool m_maskRightLeg
		{
			get { return _m_maskRightLeg; }
			set { _m_maskRightLeg = value; this.Invalidate(); }
		}
		
		private bool _m_maskLeftArm;
		[STNodeProperty("maskLeftArm", "maskLeftArm")]
		public bool m_maskLeftArm
		{
			get { return _m_maskLeftArm; }
			set { _m_maskLeftArm = value; this.Invalidate(); }
		}
		
		private bool _m_maskRightArm;
		[STNodeProperty("maskRightArm", "maskRightArm")]
		public bool m_maskRightArm
		{
			get { return _m_maskRightArm; }
			set { _m_maskRightArm = value; this.Invalidate(); }
		}
		
		private bool _m_maskLeftHand;
		[STNodeProperty("maskLeftHand", "maskLeftHand")]
		public bool m_maskLeftHand
		{
			get { return _m_maskLeftHand; }
			set { _m_maskLeftHand = value; this.Invalidate(); }
		}
		
		private bool _m_maskRightHand;
		[STNodeProperty("maskRightHand", "maskRightHand")]
		public bool m_maskRightHand
		{
			get { return _m_maskRightHand; }
			set { _m_maskRightHand = value; this.Invalidate(); }
		}
		
		private bool _m_maskLeftFingers;
		[STNodeProperty("maskLeftFingers", "maskLeftFingers")]
		public bool m_maskLeftFingers
		{
			get { return _m_maskLeftFingers; }
			set { _m_maskLeftFingers = value; this.Invalidate(); }
		}
		
		private bool _m_maskRightFingers;
		[STNodeProperty("maskRightFingers", "maskRightFingers")]
		public bool m_maskRightFingers
		{
			get { return _m_maskRightFingers; }
			set { _m_maskRightFingers = value; this.Invalidate(); }
		}
		
		private bool _m_maskTail;
		[STNodeProperty("maskTail", "maskTail")]
		public bool m_maskTail
		{
			get { return _m_maskTail; }
			set { _m_maskTail = value; this.Invalidate(); }
		}
		
		private bool _m_maskLips;
		[STNodeProperty("maskLips", "maskLips")]
		public bool m_maskLips
		{
			get { return _m_maskLips; }
			set { _m_maskLips = value; this.Invalidate(); }
		}
		
		private bool _m_maskEyes;
		[STNodeProperty("maskEyes", "maskEyes")]
		public bool m_maskEyes
		{
			get { return _m_maskEyes; }
			set { _m_maskEyes = value; this.Invalidate(); }
		}
		
		private bool _m_maskLeftShoulder;
		[STNodeProperty("maskLeftShoulder", "maskLeftShoulder")]
		public bool m_maskLeftShoulder
		{
			get { return _m_maskLeftShoulder; }
			set { _m_maskLeftShoulder = value; this.Invalidate(); }
		}
		
		private bool _m_maskRightShoulder;
		[STNodeProperty("maskRightShoulder", "maskRightShoulder")]
		public bool m_maskRightShoulder
		{
			get { return _m_maskRightShoulder; }
			set { _m_maskRightShoulder = value; this.Invalidate(); }
		}
		
		private bool _m_maskRoot;
		[STNodeProperty("maskRoot", "maskRoot")]
		public bool m_maskRoot
		{
			get { return _m_maskRoot; }
			set { _m_maskRoot = value; this.Invalidate(); }
		}
		
		private bool _m_maskPrecedingLayers;
		[STNodeProperty("maskPrecedingLayers", "maskPrecedingLayers")]
		public bool m_maskPrecedingLayers
		{
			get { return _m_maskPrecedingLayers; }
			set { _m_maskPrecedingLayers = value; this.Invalidate(); }
		}
		
		private bool _m_maskSelf;
		[STNodeProperty("maskSelf", "maskSelf")]
		public bool m_maskSelf
		{
			get { return _m_maskSelf; }
			set { _m_maskSelf = value; this.Invalidate(); }
		}
		
		private bool _m_maskFollowingLayers;
		[STNodeProperty("maskFollowingLayers", "maskFollowingLayers")]
		public bool m_maskFollowingLayers
		{
			get { return _m_maskFollowingLayers; }
			set { _m_maskFollowingLayers = value; this.Invalidate(); }
		}
		
		private float _m_weight;
		[STNodeProperty("weight", "weight")]
		public float m_weight
		{
			get { return _m_weight; }
			set { _m_weight = value; this.Invalidate(); }
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
			
			this.Title = "AnimationMask";
			
			
		}
	}
}
#endif
