#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class VariableAnimationInfo : STNode
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
			
			this.Title = "VariableAnimationInfo";
			
			this.InputOptions.Add("refresh", typeof(void), false);
			this.InputOptions.Add("reset", typeof(void), false);
			
			this.OutputOptions.Add("on_changed", typeof(void), false);
			this.OutputOptions.Add("on_restored", typeof(void), false);
			this.OutputOptions.Add("refreshed", typeof(void), false);
			this.OutputOptions.Add("reseted", typeof(void), false);
		}
	}
}
#endif
