using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class ButtonMashPrompt : STNode
	{
		private int _m_mashes_to_completion;
		[STNodeProperty("mashes_to_completion", "mashes_to_completion")]
		public int m_mashes_to_completion
		{
			get { return _m_mashes_to_completion; }
			set { _m_mashes_to_completion = value; this.Invalidate(); }
		}
		
		private float _m_time_between_degrades;
		[STNodeProperty("time_between_degrades", "time_between_degrades")]
		public float m_time_between_degrades
		{
			get { return _m_time_between_degrades; }
			set { _m_time_between_degrades = value; this.Invalidate(); }
		}
		
		private bool _m_use_degrade;
		[STNodeProperty("use_degrade", "use_degrade")]
		public bool m_use_degrade
		{
			get { return _m_use_degrade; }
			set { _m_use_degrade = value; this.Invalidate(); }
		}
		
		private bool _m_hold_to_charge;
		[STNodeProperty("hold_to_charge", "hold_to_charge")]
		public bool m_hold_to_charge
		{
			get { return _m_hold_to_charge; }
			set { _m_hold_to_charge = value; this.Invalidate(); }
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
			
			this.Title = "ButtonMashPrompt";
			
			this.InputOptions.Add("trigger", typeof(void), false);
			this.InputOptions.Add("cancel", typeof(void), false);
			
			this.OutputOptions.Add("on_back_to_zero", typeof(void), false);
			this.OutputOptions.Add("on_degrade", typeof(void), false);
			this.OutputOptions.Add("on_mashed", typeof(void), false);
			this.OutputOptions.Add("on_success", typeof(void), false);
			this.OutputOptions.Add("count", typeof(int), false);
			this.OutputOptions.Add("triggered", typeof(void), false);
			this.OutputOptions.Add("cancelled", typeof(void), false);
		}
	}
}
