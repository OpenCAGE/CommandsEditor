using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CHR_DeathMonitor : STNode
	{
		private string _m_DamageType;
		[STNodeProperty("DamageType", "DamageType")]
		public string m_DamageType
		{
			get { return _m_DamageType; }
			set { _m_DamageType = value; this.Invalidate(); }
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
			
			this.Title = "CHR_DeathMonitor";
			
			this.InputOptions.Add("KillerFilter", typeof(bool), false);
			this.InputOptions.Add("apply_start", typeof(void), false);
			this.InputOptions.Add("apply_stop", typeof(void), false);
			
			this.OutputOptions.Add("dying", typeof(void), false);
			this.OutputOptions.Add("killed", typeof(void), false);
			this.OutputOptions.Add("Killer", typeof(STNode), false);
			this.OutputOptions.Add("start_applied", typeof(void), false);
			this.OutputOptions.Add("stop_applied", typeof(void), false);
		}
	}
}
