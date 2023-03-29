#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_DeepCrouch : STNode
	{
		private float _m_crouch_amount;
		[STNodeProperty("crouch_amount", "crouch_amount")]
		public float m_crouch_amount
		{
			get { return _m_crouch_amount; }
			set { _m_crouch_amount = value; this.Invalidate(); }
		}
		
		private float _m_smooth_damping;
		[STNodeProperty("smooth_damping", "smooth_damping")]
		public float m_smooth_damping
		{
			get { return _m_smooth_damping; }
			set { _m_smooth_damping = value; this.Invalidate(); }
		}
		
		private bool _m_allow_stand_up;
		[STNodeProperty("allow_stand_up", "allow_stand_up")]
		public bool m_allow_stand_up
		{
			get { return _m_allow_stand_up; }
			set { _m_allow_stand_up = value; this.Invalidate(); }
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
			
			this.Title = "CHR_DeepCrouch";
			
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
#endif
