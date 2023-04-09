using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class WEAPON_ImpactEffect : STNode
	{
		private string _m_Type;
		[STNodeProperty("Type", "Type")]
		public string m_Type
		{
			get { return _m_Type; }
			set { _m_Type = value; this.Invalidate(); }
		}
		
		private string _m_Orientation;
		[STNodeProperty("Orientation", "Orientation")]
		public string m_Orientation
		{
			get { return _m_Orientation; }
			set { _m_Orientation = value; this.Invalidate(); }
		}
		
		private int _m_Priority;
		[STNodeProperty("Priority", "Priority")]
		public int m_Priority
		{
			get { return _m_Priority; }
			set { _m_Priority = value; this.Invalidate(); }
		}
		
		private float _m_SafeDistant;
		[STNodeProperty("SafeDistant", "SafeDistant")]
		public float m_SafeDistant
		{
			get { return _m_SafeDistant; }
			set { _m_SafeDistant = value; this.Invalidate(); }
		}
		
		private float _m_LifeTime;
		[STNodeProperty("LifeTime", "LifeTime")]
		public float m_LifeTime
		{
			get { return _m_LifeTime; }
			set { _m_LifeTime = value; this.Invalidate(); }
		}
		
		private float _m_character_damage_offset;
		[STNodeProperty("character_damage_offset", "character_damage_offset")]
		public float m_character_damage_offset
		{
			get { return _m_character_damage_offset; }
			set { _m_character_damage_offset = value; this.Invalidate(); }
		}
		
		private bool _m_RandomRotation;
		[STNodeProperty("RandomRotation", "RandomRotation")]
		public bool m_RandomRotation
		{
			get { return _m_RandomRotation; }
			set { _m_RandomRotation = value; this.Invalidate(); }
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
			
			this.Title = "WEAPON_ImpactEffect";
			
			this.InputOptions.Add("StaticEffects", typeof(STNode), false);
			this.InputOptions.Add("DynamicEffects", typeof(STNode), false);
			this.InputOptions.Add("DynamicAttachedEffects", typeof(STNode), false);
			this.InputOptions.Add("impact", typeof(void), false);
			
			this.OutputOptions.Add("impacted", typeof(void), false);
		}
	}
}
