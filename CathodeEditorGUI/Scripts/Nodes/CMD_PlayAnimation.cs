#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CMD_PlayAnimation : STNode
	{
		private string _m_AnimationSet;
		[STNodeProperty("AnimationSet", "AnimationSet")]
		public string m_AnimationSet
		{
			get { return _m_AnimationSet; }
			set { _m_AnimationSet = value; this.Invalidate(); }
		}
		
		private string _m_Animation;
		[STNodeProperty("Animation", "Animation")]
		public string m_Animation
		{
			get { return _m_Animation; }
			set { _m_Animation = value; this.Invalidate(); }
		}
		
		private int _m_StartFrame;
		[STNodeProperty("StartFrame", "StartFrame")]
		public int m_StartFrame
		{
			get { return _m_StartFrame; }
			set { _m_StartFrame = value; this.Invalidate(); }
		}
		
		private int _m_EndFrame;
		[STNodeProperty("EndFrame", "EndFrame")]
		public int m_EndFrame
		{
			get { return _m_EndFrame; }
			set { _m_EndFrame = value; this.Invalidate(); }
		}
		
		private int _m_PlayCount;
		[STNodeProperty("PlayCount", "PlayCount")]
		public int m_PlayCount
		{
			get { return _m_PlayCount; }
			set { _m_PlayCount = value; this.Invalidate(); }
		}
		
		private float _m_PlaySpeed;
		[STNodeProperty("PlaySpeed", "PlaySpeed")]
		public float m_PlaySpeed
		{
			get { return _m_PlaySpeed; }
			set { _m_PlaySpeed = value; this.Invalidate(); }
		}
		
		private bool _m_AllowGravity;
		[STNodeProperty("AllowGravity", "AllowGravity")]
		public bool m_AllowGravity
		{
			get { return _m_AllowGravity; }
			set { _m_AllowGravity = value; this.Invalidate(); }
		}
		
		private bool _m_AllowCollision;
		[STNodeProperty("AllowCollision", "AllowCollision")]
		public bool m_AllowCollision
		{
			get { return _m_AllowCollision; }
			set { _m_AllowCollision = value; this.Invalidate(); }
		}
		
		private bool _m_Start_Instantly;
		[STNodeProperty("Start_Instantly", "Start_Instantly")]
		public bool m_Start_Instantly
		{
			get { return _m_Start_Instantly; }
			set { _m_Start_Instantly = value; this.Invalidate(); }
		}
		
		private bool _m_AllowInterruption;
		[STNodeProperty("AllowInterruption", "AllowInterruption")]
		public bool m_AllowInterruption
		{
			get { return _m_AllowInterruption; }
			set { _m_AllowInterruption = value; this.Invalidate(); }
		}
		
		private bool _m_RemoveMotion;
		[STNodeProperty("RemoveMotion", "RemoveMotion")]
		public bool m_RemoveMotion
		{
			get { return _m_RemoveMotion; }
			set { _m_RemoveMotion = value; this.Invalidate(); }
		}
		
		private bool _m_DisableGunLayer;
		[STNodeProperty("DisableGunLayer", "DisableGunLayer")]
		public bool m_DisableGunLayer
		{
			get { return _m_DisableGunLayer; }
			set { _m_DisableGunLayer = value; this.Invalidate(); }
		}
		
		private float _m_BlendInTime;
		[STNodeProperty("BlendInTime", "BlendInTime")]
		public float m_BlendInTime
		{
			get { return _m_BlendInTime; }
			set { _m_BlendInTime = value; this.Invalidate(); }
		}
		
		private bool _m_GaitSyncStart;
		[STNodeProperty("GaitSyncStart", "GaitSyncStart")]
		public bool m_GaitSyncStart
		{
			get { return _m_GaitSyncStart; }
			set { _m_GaitSyncStart = value; this.Invalidate(); }
		}
		
		private float _m_ConvergenceTime;
		[STNodeProperty("ConvergenceTime", "ConvergenceTime")]
		public float m_ConvergenceTime
		{
			get { return _m_ConvergenceTime; }
			set { _m_ConvergenceTime = value; this.Invalidate(); }
		}
		
		private bool _m_LocationConvergence;
		[STNodeProperty("LocationConvergence", "LocationConvergence")]
		public bool m_LocationConvergence
		{
			get { return _m_LocationConvergence; }
			set { _m_LocationConvergence = value; this.Invalidate(); }
		}
		
		private bool _m_OrientationConvergence;
		[STNodeProperty("OrientationConvergence", "OrientationConvergence")]
		public bool m_OrientationConvergence
		{
			get { return _m_OrientationConvergence; }
			set { _m_OrientationConvergence = value; this.Invalidate(); }
		}
		
		private bool _m_UseExitConvergence;
		[STNodeProperty("UseExitConvergence", "UseExitConvergence")]
		public bool m_UseExitConvergence
		{
			get { return _m_UseExitConvergence; }
			set { _m_UseExitConvergence = value; this.Invalidate(); }
		}
		
		private float _m_ExitConvergenceTime;
		[STNodeProperty("ExitConvergenceTime", "ExitConvergenceTime")]
		public float m_ExitConvergenceTime
		{
			get { return _m_ExitConvergenceTime; }
			set { _m_ExitConvergenceTime = value; this.Invalidate(); }
		}
		
		private bool _m_Mirror;
		[STNodeProperty("Mirror", "Mirror")]
		public bool m_Mirror
		{
			get { return _m_Mirror; }
			set { _m_Mirror = value; this.Invalidate(); }
		}
		
		private bool _m_FullCinematic;
		[STNodeProperty("FullCinematic", "FullCinematic")]
		public bool m_FullCinematic
		{
			get { return _m_FullCinematic; }
			set { _m_FullCinematic = value; this.Invalidate(); }
		}
		
		private bool _m_RagdollEnabled;
		[STNodeProperty("RagdollEnabled", "RagdollEnabled")]
		public bool m_RagdollEnabled
		{
			get { return _m_RagdollEnabled; }
			set { _m_RagdollEnabled = value; this.Invalidate(); }
		}
		
		private bool _m_NoIK;
		[STNodeProperty("NoIK", "NoIK")]
		public bool m_NoIK
		{
			get { return _m_NoIK; }
			set { _m_NoIK = value; this.Invalidate(); }
		}
		
		private bool _m_NoFootIK;
		[STNodeProperty("NoFootIK", "NoFootIK")]
		public bool m_NoFootIK
		{
			get { return _m_NoFootIK; }
			set { _m_NoFootIK = value; this.Invalidate(); }
		}
		
		private bool _m_NoLayers;
		[STNodeProperty("NoLayers", "NoLayers")]
		public bool m_NoLayers
		{
			get { return _m_NoLayers; }
			set { _m_NoLayers = value; this.Invalidate(); }
		}
		
		private bool _m_PlayerAnimDrivenView;
		[STNodeProperty("PlayerAnimDrivenView", "PlayerAnimDrivenView")]
		public bool m_PlayerAnimDrivenView
		{
			get { return _m_PlayerAnimDrivenView; }
			set { _m_PlayerAnimDrivenView = value; this.Invalidate(); }
		}
		
		private float _m_ExertionFactor;
		[STNodeProperty("ExertionFactor", "ExertionFactor")]
		public float m_ExertionFactor
		{
			get { return _m_ExertionFactor; }
			set { _m_ExertionFactor = value; this.Invalidate(); }
		}
		
		private bool _m_AutomaticZoning;
		[STNodeProperty("AutomaticZoning", "AutomaticZoning")]
		public bool m_AutomaticZoning
		{
			get { return _m_AutomaticZoning; }
			set { _m_AutomaticZoning = value; this.Invalidate(); }
		}
		
		private bool _m_ManualLoading;
		[STNodeProperty("ManualLoading", "ManualLoading")]
		public bool m_ManualLoading
		{
			get { return _m_ManualLoading; }
			set { _m_ManualLoading = value; this.Invalidate(); }
		}
		
		private bool _m_IsCrouchedAnim;
		[STNodeProperty("IsCrouchedAnim", "IsCrouchedAnim")]
		public bool m_IsCrouchedAnim
		{
			get { return _m_IsCrouchedAnim; }
			set { _m_IsCrouchedAnim = value; this.Invalidate(); }
		}
		
		private bool _m_InitiallyBackstage;
		[STNodeProperty("InitiallyBackstage", "InitiallyBackstage")]
		public bool m_InitiallyBackstage
		{
			get { return _m_InitiallyBackstage; }
			set { _m_InitiallyBackstage = value; this.Invalidate(); }
		}
		
		private bool _m_Death_by_ragdoll_only;
		[STNodeProperty("Death_by_ragdoll_only", "Death_by_ragdoll_only")]
		public bool m_Death_by_ragdoll_only
		{
			get { return _m_Death_by_ragdoll_only; }
			set { _m_Death_by_ragdoll_only = value; this.Invalidate(); }
		}
		
		private int _m_dof_key;
		[STNodeProperty("dof_key", "dof_key")]
		public int m_dof_key
		{
			get { return _m_dof_key; }
			set { _m_dof_key = value; this.Invalidate(); }
		}
		
		private int _m_shot_number;
		[STNodeProperty("shot_number", "shot_number")]
		public int m_shot_number
		{
			get { return _m_shot_number; }
			set { _m_shot_number = value; this.Invalidate(); }
		}
		
		private bool _m_UseShivaArms;
		[STNodeProperty("UseShivaArms", "UseShivaArms")]
		public bool m_UseShivaArms
		{
			get { return _m_UseShivaArms; }
			set { _m_UseShivaArms = value; this.Invalidate(); }
		}
		
		private bool _m_override_all_ai;
		[STNodeProperty("override_all_ai", "override_all_ai")]
		public bool m_override_all_ai
		{
			get { return _m_override_all_ai; }
			set { _m_override_all_ai = value; this.Invalidate(); }
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
			
			this.Title = "CMD_PlayAnimation";
			
			this.InputOptions.Add("SafePos", typeof(STNode), false);
			this.InputOptions.Add("Marker", typeof(STNode), false);
			this.InputOptions.Add("ExitPosition", typeof(STNode), false);
			this.InputOptions.Add("ExternalStartTime", typeof(STNode), false);
			this.InputOptions.Add("ExternalTime", typeof(STNode), false);
			this.InputOptions.Add("OverrideCharacter", typeof(STNode), false);
			this.InputOptions.Add("OptionalMask", typeof(STNode), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			this.InputOptions.Add("request_load", typeof(void), false);
			this.InputOptions.Add("cancel_load", typeof(void), false);
			
			this.OutputOptions.Add("Interrupted", typeof(void), false);
			this.OutputOptions.Add("finished", typeof(void), false);
			this.OutputOptions.Add("badInterrupted", typeof(void), false);
			this.OutputOptions.Add("on_loaded", typeof(void), false);
			this.OutputOptions.Add("animationLength", typeof(float), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("load_requested", typeof(void), false);
			this.OutputOptions.Add("load_cancelled", typeof(void), false);
			this.OutputOptions.Add("command_started", typeof(void), false);
		}
	}
}
#endif
