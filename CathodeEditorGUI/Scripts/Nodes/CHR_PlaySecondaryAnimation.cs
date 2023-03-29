#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_PlaySecondaryAnimation : STNode
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
		
		private bool _m_StartInstantly;
		[STNodeProperty("StartInstantly", "StartInstantly")]
		public bool m_StartInstantly
		{
			get { return _m_StartInstantly; }
			set { _m_StartInstantly = value; this.Invalidate(); }
		}
		
		private bool _m_AllowInterruption;
		[STNodeProperty("AllowInterruption", "AllowInterruption")]
		public bool m_AllowInterruption
		{
			get { return _m_AllowInterruption; }
			set { _m_AllowInterruption = value; this.Invalidate(); }
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
		
		private bool _m_Mirror;
		[STNodeProperty("Mirror", "Mirror")]
		public bool m_Mirror
		{
			get { return _m_Mirror; }
			set { _m_Mirror = value; this.Invalidate(); }
		}
		
		private string _m_AnimationLayer;
		[STNodeProperty("AnimationLayer", "AnimationLayer")]
		public string m_AnimationLayer
		{
			get { return _m_AnimationLayer; }
			set { _m_AnimationLayer = value; this.Invalidate(); }
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
			
			this.Title = "CHR_PlaySecondaryAnimation";
			
			this.InputOptions.Add("Marker", typeof(STNode), false);
			this.InputOptions.Add("OptionalMask", typeof(STNode), false);
			this.InputOptions.Add("ExternalStartTime", typeof(STNode), false);
			this.InputOptions.Add("ExternalTime", typeof(STNode), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			this.InputOptions.Add("request_load", typeof(void), false);
			this.InputOptions.Add("cancel_load", typeof(void), false);
			
			this.OutputOptions.Add("Interrupted", typeof(void), false);
			this.OutputOptions.Add("finished", typeof(void), false);
			this.OutputOptions.Add("on_loaded", typeof(void), false);
			this.OutputOptions.Add("animationLength", typeof(float), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
			this.OutputOptions.Add("load_requested", typeof(void), false);
			this.OutputOptions.Add("load_cancelled", typeof(void), false);
		}
	}
}
#endif
