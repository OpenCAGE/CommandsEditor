#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class NPC_SuspiciousItem : STNode
	{
		private string _m_Item;
		[STNodeProperty("Item", "Item")]
		public string m_Item
		{
			get { return _m_Item; }
			set { _m_Item = value; this.Invalidate(); }
		}
		
		private float _m_InitialReactionValidStartDuration;
		[STNodeProperty("InitialReactionValidStartDuration", "InitialReactionValidStartDuration")]
		public float m_InitialReactionValidStartDuration
		{
			get { return _m_InitialReactionValidStartDuration; }
			set { _m_InitialReactionValidStartDuration = value; this.Invalidate(); }
		}
		
		private float _m_FurtherReactionValidStartDuration;
		[STNodeProperty("FurtherReactionValidStartDuration", "FurtherReactionValidStartDuration")]
		public float m_FurtherReactionValidStartDuration
		{
			get { return _m_FurtherReactionValidStartDuration; }
			set { _m_FurtherReactionValidStartDuration = value; this.Invalidate(); }
		}
		
		private float _m_RetriggerDelay;
		[STNodeProperty("RetriggerDelay", "RetriggerDelay")]
		public float m_RetriggerDelay
		{
			get { return _m_RetriggerDelay; }
			set { _m_RetriggerDelay = value; this.Invalidate(); }
		}
		
		private string _m_Trigger;
		[STNodeProperty("Trigger", "Trigger")]
		public string m_Trigger
		{
			get { return _m_Trigger; }
			set { _m_Trigger = value; this.Invalidate(); }
		}
		
		private bool _m_ShouldMakeAggressive;
		[STNodeProperty("ShouldMakeAggressive", "ShouldMakeAggressive")]
		public bool m_ShouldMakeAggressive
		{
			get { return _m_ShouldMakeAggressive; }
			set { _m_ShouldMakeAggressive = value; this.Invalidate(); }
		}
		
		private int _m_MaxGroupMembersInteract;
		[STNodeProperty("MaxGroupMembersInteract", "MaxGroupMembersInteract")]
		public int m_MaxGroupMembersInteract
		{
			get { return _m_MaxGroupMembersInteract; }
			set { _m_MaxGroupMembersInteract = value; this.Invalidate(); }
		}
		
		private float _m_SystematicSearchRadius;
		[STNodeProperty("SystematicSearchRadius", "SystematicSearchRadius")]
		public float m_SystematicSearchRadius
		{
			get { return _m_SystematicSearchRadius; }
			set { _m_SystematicSearchRadius = value; this.Invalidate(); }
		}
		
		private bool _m_AllowSamePriorityToOveride;
		[STNodeProperty("AllowSamePriorityToOveride", "AllowSamePriorityToOveride")]
		public bool m_AllowSamePriorityToOveride
		{
			get { return _m_AllowSamePriorityToOveride; }
			set { _m_AllowSamePriorityToOveride = value; this.Invalidate(); }
		}
		
		private bool _m_UseSamePriorityCloserDistanceConstraint;
		[STNodeProperty("UseSamePriorityCloserDistanceConstraint", "UseSamePriorityCloserDistanceConstraint")]
		public bool m_UseSamePriorityCloserDistanceConstraint
		{
			get { return _m_UseSamePriorityCloserDistanceConstraint; }
			set { _m_UseSamePriorityCloserDistanceConstraint = value; this.Invalidate(); }
		}
		
		private float _m_SamePriorityCloserDistanceConstraint;
		[STNodeProperty("SamePriorityCloserDistanceConstraint", "SamePriorityCloserDistanceConstraint")]
		public float m_SamePriorityCloserDistanceConstraint
		{
			get { return _m_SamePriorityCloserDistanceConstraint; }
			set { _m_SamePriorityCloserDistanceConstraint = value; this.Invalidate(); }
		}
		
		private bool _m_UseSamePriorityRecentTimeConstraint;
		[STNodeProperty("UseSamePriorityRecentTimeConstraint", "UseSamePriorityRecentTimeConstraint")]
		public bool m_UseSamePriorityRecentTimeConstraint
		{
			get { return _m_UseSamePriorityRecentTimeConstraint; }
			set { _m_UseSamePriorityRecentTimeConstraint = value; this.Invalidate(); }
		}
		
		private float _m_SamePriorityRecentTimeConstraint;
		[STNodeProperty("SamePriorityRecentTimeConstraint", "SamePriorityRecentTimeConstraint")]
		public float m_SamePriorityRecentTimeConstraint
		{
			get { return _m_SamePriorityRecentTimeConstraint; }
			set { _m_SamePriorityRecentTimeConstraint = value; this.Invalidate(); }
		}
		
		private string _m_BehaviourTreePriority;
		[STNodeProperty("BehaviourTreePriority", "BehaviourTreePriority")]
		public string m_BehaviourTreePriority
		{
			get { return _m_BehaviourTreePriority; }
			set { _m_BehaviourTreePriority = value; this.Invalidate(); }
		}
		
		private int _m_InteruptSubPriority;
		[STNodeProperty("InteruptSubPriority", "InteruptSubPriority")]
		public int m_InteruptSubPriority
		{
			get { return _m_InteruptSubPriority; }
			set { _m_InteruptSubPriority = value; this.Invalidate(); }
		}
		
		private bool _m_DetectableByBackstageAlien;
		[STNodeProperty("DetectableByBackstageAlien", "DetectableByBackstageAlien")]
		public bool m_DetectableByBackstageAlien
		{
			get { return _m_DetectableByBackstageAlien; }
			set { _m_DetectableByBackstageAlien = value; this.Invalidate(); }
		}
		
		private bool _m_DoIntialReaction;
		[STNodeProperty("DoIntialReaction", "DoIntialReaction")]
		public bool m_DoIntialReaction
		{
			get { return _m_DoIntialReaction; }
			set { _m_DoIntialReaction = value; this.Invalidate(); }
		}
		
		private bool _m_MoveCloseToSuspectPosition;
		[STNodeProperty("MoveCloseToSuspectPosition", "MoveCloseToSuspectPosition")]
		public bool m_MoveCloseToSuspectPosition
		{
			get { return _m_MoveCloseToSuspectPosition; }
			set { _m_MoveCloseToSuspectPosition = value; this.Invalidate(); }
		}
		
		private bool _m_DoCloseToReaction;
		[STNodeProperty("DoCloseToReaction", "DoCloseToReaction")]
		public bool m_DoCloseToReaction
		{
			get { return _m_DoCloseToReaction; }
			set { _m_DoCloseToReaction = value; this.Invalidate(); }
		}
		
		private bool _m_DoCloseToWaitForGroupMembers;
		[STNodeProperty("DoCloseToWaitForGroupMembers", "DoCloseToWaitForGroupMembers")]
		public bool m_DoCloseToWaitForGroupMembers
		{
			get { return _m_DoCloseToWaitForGroupMembers; }
			set { _m_DoCloseToWaitForGroupMembers = value; this.Invalidate(); }
		}
		
		private bool _m_DoSystematicSearch;
		[STNodeProperty("DoSystematicSearch", "DoSystematicSearch")]
		public bool m_DoSystematicSearch
		{
			get { return _m_DoSystematicSearch; }
			set { _m_DoSystematicSearch = value; this.Invalidate(); }
		}
		
		private string _m_GroupNotify;
		[STNodeProperty("GroupNotify", "GroupNotify")]
		public string m_GroupNotify
		{
			get { return _m_GroupNotify; }
			set { _m_GroupNotify = value; this.Invalidate(); }
		}
		
		private bool _m_DoIntialReactionSubsequentGroupMember;
		[STNodeProperty("DoIntialReactionSubsequentGroupMember", "DoIntialReactionSubsequentGroupMember")]
		public bool m_DoIntialReactionSubsequentGroupMember
		{
			get { return _m_DoIntialReactionSubsequentGroupMember; }
			set { _m_DoIntialReactionSubsequentGroupMember = value; this.Invalidate(); }
		}
		
		private bool _m_MoveCloseToSuspectPositionSubsequentGroupMember;
		[STNodeProperty("MoveCloseToSuspectPositionSubsequentGroupMember", "MoveCloseToSuspectPositionSubsequentGroupMember")]
		public bool m_MoveCloseToSuspectPositionSubsequentGroupMember
		{
			get { return _m_MoveCloseToSuspectPositionSubsequentGroupMember; }
			set { _m_MoveCloseToSuspectPositionSubsequentGroupMember = value; this.Invalidate(); }
		}
		
		private bool _m_DoCloseToReactionSubsequentGroupMember;
		[STNodeProperty("DoCloseToReactionSubsequentGroupMember", "DoCloseToReactionSubsequentGroupMember")]
		public bool m_DoCloseToReactionSubsequentGroupMember
		{
			get { return _m_DoCloseToReactionSubsequentGroupMember; }
			set { _m_DoCloseToReactionSubsequentGroupMember = value; this.Invalidate(); }
		}
		
		private bool _m_DoCloseToWaitForGroupMembersSubsequentGroupMember;
		[STNodeProperty("DoCloseToWaitForGroupMembersSubsequentGroupMember", "DoCloseToWaitForGroupMembersSubsequentGroupMember")]
		public bool m_DoCloseToWaitForGroupMembersSubsequentGroupMember
		{
			get { return _m_DoCloseToWaitForGroupMembersSubsequentGroupMember; }
			set { _m_DoCloseToWaitForGroupMembersSubsequentGroupMember = value; this.Invalidate(); }
		}
		
		private bool _m_DoSystematicSearchSubsequentGroupMember;
		[STNodeProperty("DoSystematicSearchSubsequentGroupMember", "DoSystematicSearchSubsequentGroupMember")]
		public bool m_DoSystematicSearchSubsequentGroupMember
		{
			get { return _m_DoSystematicSearchSubsequentGroupMember; }
			set { _m_DoSystematicSearchSubsequentGroupMember = value; this.Invalidate(); }
		}
		
		private bool _m_start_on_reset;
		[STNodeProperty("start_on_reset", "start_on_reset")]
		public bool m_start_on_reset
		{
			get { return _m_start_on_reset; }
			set { _m_start_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_pause_on_reset;
		[STNodeProperty("pause_on_reset", "pause_on_reset")]
		public bool m_pause_on_reset
		{
			get { return _m_pause_on_reset; }
			set { _m_pause_on_reset = value; this.Invalidate(); }
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
			
			this.Title = "NPC_SuspiciousItem";
			
			this.InputOptions.Add("ItemPosition", typeof(cTransform), false);
			this.InputOptions.Add("enter", typeof(void), false);
			this.InputOptions.Add("exit", typeof(void), false);
			this.InputOptions.Add("start", typeof(void), false);
			this.InputOptions.Add("stop", typeof(void), false);
			this.InputOptions.Add("pause", typeof(void), false);
			this.InputOptions.Add("resume", typeof(void), false);
			
			this.OutputOptions.Add("entered", typeof(void), false);
			this.OutputOptions.Add("exited", typeof(void), false);
			this.OutputOptions.Add("started", typeof(void), false);
			this.OutputOptions.Add("stopped", typeof(void), false);
			this.OutputOptions.Add("paused", typeof(void), false);
			this.OutputOptions.Add("resumed", typeof(void), false);
		}
	}
}
#endif
