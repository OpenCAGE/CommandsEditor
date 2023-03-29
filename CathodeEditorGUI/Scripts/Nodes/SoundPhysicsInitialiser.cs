#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class SoundPhysicsInitialiser : STNode
	{
		private float _m_contact_max_timeout;
		[STNodeProperty("contact_max_timeout", "contact_max_timeout")]
		public float m_contact_max_timeout
		{
			get { return _m_contact_max_timeout; }
			set { _m_contact_max_timeout = value; this.Invalidate(); }
		}
		
		private float _m_contact_smoothing_attack_rate;
		[STNodeProperty("contact_smoothing_attack_rate", "contact_smoothing_attack_rate")]
		public float m_contact_smoothing_attack_rate
		{
			get { return _m_contact_smoothing_attack_rate; }
			set { _m_contact_smoothing_attack_rate = value; this.Invalidate(); }
		}
		
		private float _m_contact_smoothing_decay_rate;
		[STNodeProperty("contact_smoothing_decay_rate", "contact_smoothing_decay_rate")]
		public float m_contact_smoothing_decay_rate
		{
			get { return _m_contact_smoothing_decay_rate; }
			set { _m_contact_smoothing_decay_rate = value; this.Invalidate(); }
		}
		
		private float _m_contact_min_magnitude;
		[STNodeProperty("contact_min_magnitude", "contact_min_magnitude")]
		public float m_contact_min_magnitude
		{
			get { return _m_contact_min_magnitude; }
			set { _m_contact_min_magnitude = value; this.Invalidate(); }
		}
		
		private float _m_contact_max_trigger_distance;
		[STNodeProperty("contact_max_trigger_distance", "contact_max_trigger_distance")]
		public float m_contact_max_trigger_distance
		{
			get { return _m_contact_max_trigger_distance; }
			set { _m_contact_max_trigger_distance = value; this.Invalidate(); }
		}
		
		private float _m_impact_min_speed;
		[STNodeProperty("impact_min_speed", "impact_min_speed")]
		public float m_impact_min_speed
		{
			get { return _m_impact_min_speed; }
			set { _m_impact_min_speed = value; this.Invalidate(); }
		}
		
		private float _m_impact_max_trigger_distance;
		[STNodeProperty("impact_max_trigger_distance", "impact_max_trigger_distance")]
		public float m_impact_max_trigger_distance
		{
			get { return _m_impact_max_trigger_distance; }
			set { _m_impact_max_trigger_distance = value; this.Invalidate(); }
		}
		
		private float _m_ragdoll_min_timeout;
		[STNodeProperty("ragdoll_min_timeout", "ragdoll_min_timeout")]
		public float m_ragdoll_min_timeout
		{
			get { return _m_ragdoll_min_timeout; }
			set { _m_ragdoll_min_timeout = value; this.Invalidate(); }
		}
		
		private float _m_ragdoll_min_speed;
		[STNodeProperty("ragdoll_min_speed", "ragdoll_min_speed")]
		public float m_ragdoll_min_speed
		{
			get { return _m_ragdoll_min_speed; }
			set { _m_ragdoll_min_speed = value; this.Invalidate(); }
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
			
			this.Title = "SoundPhysicsInitialiser";
			
			
		}
	}
}
#endif
