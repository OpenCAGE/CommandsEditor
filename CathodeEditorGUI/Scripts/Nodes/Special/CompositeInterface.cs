using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;
using System.Linq;

namespace CommandsEditor.Nodes
{
	//[STNode("/")]
	public class CompositeInterface : STNode
	{
		private bool _m_is_template;
		[STNodeProperty("is_template", "is_template")]
		public bool m_is_template
		{
			get { return _m_is_template; }
			set { _m_is_template = value; this.Invalidate(); }
		}
		
		private bool _m_local_only;
		[STNodeProperty("local_only", "local_only")]
		public bool m_local_only
		{
			get { return _m_local_only; }
			set { _m_local_only = value; this.Invalidate(); }
		}
		
		private bool _m_suspend_on_reset;
		[STNodeProperty("suspend_on_reset", "suspend_on_reset")]
		public bool m_suspend_on_reset
		{
			get { return _m_suspend_on_reset; }
			set { _m_suspend_on_reset = value; this.Invalidate(); }
		}
		
		private bool _m_deleted;
		[STNodeProperty("deleted", "deleted")]
		public bool m_deleted
		{
			get { return _m_deleted; }
			set { _m_deleted = value; this.Invalidate(); }
		}
		
		private bool _m_is_shared;
		[STNodeProperty("is_shared", "is_shared")]
		public bool m_is_shared
		{
			get { return _m_is_shared; }
			set { _m_is_shared = value; this.Invalidate(); }
		}
		
		private bool _m_requires_script_for_current_gen;
		[STNodeProperty("requires_script_for_current_gen", "requires_script_for_current_gen")]
		public bool m_requires_script_for_current_gen
		{
			get { return _m_requires_script_for_current_gen; }
			set { _m_requires_script_for_current_gen = value; this.Invalidate(); }
		}
		
		private bool _m_requires_script_for_next_gen;
		[STNodeProperty("requires_script_for_next_gen", "requires_script_for_next_gen")]
		public bool m_requires_script_for_next_gen
		{
			get { return _m_requires_script_for_next_gen; }
			set { _m_requires_script_for_next_gen = value; this.Invalidate(); }
		}
		
		private bool _m_convert_to_physics;
		[STNodeProperty("convert_to_physics", "convert_to_physics")]
		public bool m_convert_to_physics
		{
			get { return _m_convert_to_physics; }
			set { _m_convert_to_physics = value; this.Invalidate(); }
		}
		
		private bool _m_delete_standard_collision;
		[STNodeProperty("delete_standard_collision", "delete_standard_collision")]
		public bool m_delete_standard_collision
		{
			get { return _m_delete_standard_collision; }
			set { _m_delete_standard_collision = value; this.Invalidate(); }
		}
		
		private bool _m_delete_ballistic_collision;
		[STNodeProperty("delete_ballistic_collision", "delete_ballistic_collision")]
		public bool m_delete_ballistic_collision
		{
			get { return _m_delete_ballistic_collision; }
			set { _m_delete_ballistic_collision = value; this.Invalidate(); }
		}
		
		private bool _m_disable_display;
		[STNodeProperty("disable_display", "disable_display")]
		public bool m_disable_display
		{
			get { return _m_disable_display; }
			set { _m_disable_display = value; this.Invalidate(); }
		}
		
		private bool _m_disable_collision;
		[STNodeProperty("disable_collision", "disable_collision")]
		public bool m_disable_collision
		{
			get { return _m_disable_collision; }
			set { _m_disable_collision = value; this.Invalidate(); }
		}
		
		private bool _m_disable_simulation;
		[STNodeProperty("disable_simulation", "disable_simulation")]
		public bool m_disable_simulation
		{
			get { return _m_disable_simulation; }
			set { _m_disable_simulation = value; this.Invalidate(); }
		}
		
		private string _m_mapping;
		[STNodeProperty("mapping", "mapping")]
		public string m_mapping
		{
			get { return _m_mapping; }
			set { _m_mapping = value; this.Invalidate(); }
		}
		
		private bool _m_include_in_planar_reflections;
		[STNodeProperty("include_in_planar_reflections", "include_in_planar_reflections")]
		public bool m_include_in_planar_reflections
		{
			get { return _m_include_in_planar_reflections; }
			set { _m_include_in_planar_reflections = value; this.Invalidate(); }
		}
		
		private bool _m_attach_on_reset;
		[STNodeProperty("attach_on_reset", "attach_on_reset")]
		public bool m_attach_on_reset
		{
			get { return _m_attach_on_reset; }
			set { _m_attach_on_reset = value; this.Invalidate(); }
		}
		
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
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
			
			this.Title = "CompositeInterface";
			
			this.InputOptions.Add("show", typeof(void), false);
			this.InputOptions.Add("hide", typeof(void), false);
			this.InputOptions.Add("enable", typeof(void), false);
			this.InputOptions.Add("disable", typeof(void), false);
			this.InputOptions.Add("simulate", typeof(void), false);
			this.InputOptions.Add("keyframe", typeof(void), false);
			this.InputOptions.Add("suspend", typeof(void), false);
			this.InputOptions.Add("allow", typeof(void), false);
			this.InputOptions.Add("attachment", typeof(STNode), false);
			this.InputOptions.Add("attach", typeof(void), false);
			this.InputOptions.Add("detach", typeof(void), false);
			
			this.OutputOptions.Add("shown", typeof(void), false);
			this.OutputOptions.Add("hidden", typeof(void), false);
			this.OutputOptions.Add("enabled", typeof(void), false);
			this.OutputOptions.Add("disabled", typeof(void), false);
			this.OutputOptions.Add("simulating", typeof(void), false);
			this.OutputOptions.Add("keyframed", typeof(void), false);
			this.OutputOptions.Add("suspended", typeof(void), false);
			this.OutputOptions.Add("allowed", typeof(void), false);
			this.OutputOptions.Add("attached", typeof(void), false);
			this.OutputOptions.Add("detached", typeof(void), false);
		}

		public void AddOptions(string[] inputOptions, string[] outputOptions)
        {
            if (inputOptions != null)
                for (int i = 0; i < inputOptions.Length; i++)
                    this.InputOptions.Add(inputOptions[i], typeof(void), false);
            if (outputOptions != null)
                for (int i = 0; i < outputOptions.Length; i++)
                    this.OutputOptions.Add(outputOptions[i], typeof(void), false);
        }
	}
}
