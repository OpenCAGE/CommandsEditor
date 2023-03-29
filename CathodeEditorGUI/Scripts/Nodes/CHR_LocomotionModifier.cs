#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_LocomotionModifier : STNode
	{
		private bool _m_Can_Run;
		[STNodeProperty("Can_Run", "Can_Run")]
		public bool m_Can_Run
		{
			get { return _m_Can_Run; }
			set { _m_Can_Run = value; this.Invalidate(); }
		}
		
		private bool _m_Can_Crouch;
		[STNodeProperty("Can_Crouch", "Can_Crouch")]
		public bool m_Can_Crouch
		{
			get { return _m_Can_Crouch; }
			set { _m_Can_Crouch = value; this.Invalidate(); }
		}
		
		private bool _m_Can_Aim;
		[STNodeProperty("Can_Aim", "Can_Aim")]
		public bool m_Can_Aim
		{
			get { return _m_Can_Aim; }
			set { _m_Can_Aim = value; this.Invalidate(); }
		}
		
		private bool _m_Can_Injured;
		[STNodeProperty("Can_Injured", "Can_Injured")]
		public bool m_Can_Injured
		{
			get { return _m_Can_Injured; }
			set { _m_Can_Injured = value; this.Invalidate(); }
		}
		
		private bool _m_Must_Walk;
		[STNodeProperty("Must_Walk", "Must_Walk")]
		public bool m_Must_Walk
		{
			get { return _m_Must_Walk; }
			set { _m_Must_Walk = value; this.Invalidate(); }
		}
		
		private bool _m_Must_Run;
		[STNodeProperty("Must_Run", "Must_Run")]
		public bool m_Must_Run
		{
			get { return _m_Must_Run; }
			set { _m_Must_Run = value; this.Invalidate(); }
		}
		
		private bool _m_Must_Crouch;
		[STNodeProperty("Must_Crouch", "Must_Crouch")]
		public bool m_Must_Crouch
		{
			get { return _m_Must_Crouch; }
			set { _m_Must_Crouch = value; this.Invalidate(); }
		}
		
		private bool _m_Must_Aim;
		[STNodeProperty("Must_Aim", "Must_Aim")]
		public bool m_Must_Aim
		{
			get { return _m_Must_Aim; }
			set { _m_Must_Aim = value; this.Invalidate(); }
		}
		
		private bool _m_Must_Injured;
		[STNodeProperty("Must_Injured", "Must_Injured")]
		public bool m_Must_Injured
		{
			get { return _m_Must_Injured; }
			set { _m_Must_Injured = value; this.Invalidate(); }
		}
		
		private bool _m_Is_In_Spacesuit;
		[STNodeProperty("Is_In_Spacesuit", "Is_In_Spacesuit")]
		public bool m_Is_In_Spacesuit
		{
			get { return _m_Is_In_Spacesuit; }
			set { _m_Is_In_Spacesuit = value; this.Invalidate(); }
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
			
			this.Title = "CHR_LocomotionModifier";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			
			this.OutputOptions.Add("triggered", typeof(void), false);
		}
	}
}
#endif
