using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_SetHealth : STNode
	{
		private int _m_HealthPercentage;
		[STNodeProperty("HealthPercentage", "HealthPercentage")]
		public int m_HealthPercentage
		{
			get { return _m_HealthPercentage; }
			set { _m_HealthPercentage = value; this.Invalidate(); }
		}
		
		private bool _m_UsePercentageOfCurrentHeath;
		[STNodeProperty("UsePercentageOfCurrentHeath", "UsePercentageOfCurrentHeath")]
		public bool m_UsePercentageOfCurrentHeath
		{
			get { return _m_UsePercentageOfCurrentHeath; }
			set { _m_UsePercentageOfCurrentHeath = value; this.Invalidate(); }
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
			
			this.Title = "CHR_SetHealth";
			
			this.InputOptions.Add("set", typeof(void), false);
			
			this.OutputOptions.Add("been_set", typeof(void), false);
		}
	}
}
