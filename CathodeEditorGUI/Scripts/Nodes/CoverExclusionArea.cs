#if DEBUG
using CATHODE.Scripting;
using ST.Library.UI.NodeEditor;

namespace CommandsEditor.Nodes
{
	[STNode("/")]
	public class CoverExclusionArea : STNode
	{
		private cTransform _m_position;
		[STNodeProperty("position", "position")]
		public cTransform m_position
		{
			get { return _m_position; }
			set { _m_position = value; this.Invalidate(); }
		}
		
		private cVector3 _m_half_dimensions;
		[STNodeProperty("half_dimensions", "half_dimensions")]
		public cVector3 m_half_dimensions
		{
			get { return _m_half_dimensions; }
			set { _m_half_dimensions = value; this.Invalidate(); }
		}
		
		private bool _m_exclude_cover;
		[STNodeProperty("exclude_cover", "exclude_cover")]
		public bool m_exclude_cover
		{
			get { return _m_exclude_cover; }
			set { _m_exclude_cover = value; this.Invalidate(); }
		}
		
		private bool _m_exclude_vaults;
		[STNodeProperty("exclude_vaults", "exclude_vaults")]
		public bool m_exclude_vaults
		{
			get { return _m_exclude_vaults; }
			set { _m_exclude_vaults = value; this.Invalidate(); }
		}
		
		private bool _m_exclude_mantles;
		[STNodeProperty("exclude_mantles", "exclude_mantles")]
		public bool m_exclude_mantles
		{
			get { return _m_exclude_mantles; }
			set { _m_exclude_mantles = value; this.Invalidate(); }
		}
		
		private bool _m_exclude_jump_downs;
		[STNodeProperty("exclude_jump_downs", "exclude_jump_downs")]
		public bool m_exclude_jump_downs
		{
			get { return _m_exclude_jump_downs; }
			set { _m_exclude_jump_downs = value; this.Invalidate(); }
		}
		
		private bool _m_exclude_crawl_space_spotting_positions;
		[STNodeProperty("exclude_crawl_space_spotting_positions", "exclude_crawl_space_spotting_positions")]
		public bool m_exclude_crawl_space_spotting_positions
		{
			get { return _m_exclude_crawl_space_spotting_positions; }
			set { _m_exclude_crawl_space_spotting_positions = value; this.Invalidate(); }
		}
		
		private bool _m_exclude_spotting_positions;
		[STNodeProperty("exclude_spotting_positions", "exclude_spotting_positions")]
		public bool m_exclude_spotting_positions
		{
			get { return _m_exclude_spotting_positions; }
			set { _m_exclude_spotting_positions = value; this.Invalidate(); }
		}
		
		private bool _m_exclude_assault_positions;
		[STNodeProperty("exclude_assault_positions", "exclude_assault_positions")]
		public bool m_exclude_assault_positions
		{
			get { return _m_exclude_assault_positions; }
			set { _m_exclude_assault_positions = value; this.Invalidate(); }
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
			
			this.Title = "CoverExclusionArea";
			
			
		}
	}
}
#endif
