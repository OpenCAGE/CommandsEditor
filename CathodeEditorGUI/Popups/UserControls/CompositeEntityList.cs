using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.UserControls;
using ListViewGroupCollapse;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CommandsEditor.Popups.UserControls.CompositeEntityList;

namespace CommandsEditor.Popups.UserControls
{
    public partial class CompositeEntityList : UserControl
    {
        public Entity SelectedEntity
        {
            get
            {
                if (composite_content.SelectedItems.Count == 0) return null;
                return (Entity)composite_content.SelectedItems[0].Tag;
            }
        }
        public List<Entity> CheckedEntities
        {
            get
            {
                List<Entity> toReturn = new List<Entity>();
                if (composite_content.CheckedItems.Count == 0) return toReturn;

                foreach (ListViewItem item in composite_content.CheckedItems)
                    toReturn.Add((Entity)item.Tag);
                return toReturn;
            }
        }

        public Action<Entity> SelectedEntityChanged;

        public Composite Composite => _composite;
        private Composite _composite;

        private string _currentSearch = "";
        private DisplayOptions _displayOptions;

        private LevelContent _content;

        public CompositeEntityList()
        {
            InitializeComponent();
        }

        /* This UserControl differs from BaseUserControl because we don't instantiate at runtime - so make sure to call setup in code to pass this construction info before you use it. */
        public void Setup(Composite composite, LevelContent editor, DisplayOptions displayOptions = null)
        {
            _content = editor;
            _composite = composite;

            SetDisplayOptions(displayOptions);
            ReloadComposite();
        }

        /* Update the display options to handle filtering out certain entity types */
        public void SetDisplayOptions(DisplayOptions displayOptions)
        {
            _displayOptions = displayOptions == null ? new DisplayOptions() : displayOptions;
            composite_content.CheckBoxes = _displayOptions.ShowCheckboxes;
            ReloadComposite();
        }

        /* Reload the active composite's entities */
        public void ReloadComposite(bool clearSearch = false)
        {
            if (clearSearch)
                ClearSearch();

            LoadComposite(_composite);
        }

        /* Load a new composite into the entity list */
        public void LoadComposite(Composite composite, bool clearSearch = false)
        {
            _composite = composite;

            if (clearSearch) 
                ClearSearch();

            PopulateEntities(GetDisplayableEntities());
        }

        /* Add a new entity to the list after initial population */
        public void AddNewEntity(Entity newEnt)
        {
            if (newEnt.variant == EntityVariant.ALIAS && !_displayOptions.DisplayAliases)
                return;
            if (newEnt.variant == EntityVariant.PROXY && !_displayOptions.DisplayProxies)
                return;
            if (newEnt.variant == EntityVariant.FUNCTION && !_displayOptions.DisplayFunctions)
                return;
            if (newEnt.variant == EntityVariant.VARIABLE && !_displayOptions.DisplayVariables)
                return;

            if (_currentSearch == "")
                AddEntityToListView(newEnt);
            else
                ReloadComposite();
        }

        /* Focus the entity list */
        public void FocusOnList()
        {
            composite_content.Focus();
        }

        private void PopulateEntities(List<Entity> entities)
        {
            composite_content.BeginUpdate();
            composite_content.Items.Clear();

            bool hasID = composite_content.Columns.ContainsKey("ID");
            bool showID = SettingsManager.GetBool(Singleton.Settings.EntIdOpt);
            if (showID && !hasID)
                composite_content.Columns.Add(new ColumnHeader() { Name = "ID", Text = "ID", Width = 100 });
            else if (!showID && hasID)
                composite_content.Columns.RemoveByKey("ID");

            for (int i = 0; i < entities.Count; i++)
                AddNewEntity(entities[i]);

            composite_content.SetGroupState(ListViewGroupState.Collapsible);
            composite_content.EndUpdate();
        }

        private void AddEntityToListView(Entity entity)
        {
            ListViewItem item = (ListViewItem)_content.GenerateListViewItem(entity, _composite).Clone();

            //Keep these indexes in sync with ListViewGroup 
            switch (entity.variant)
            {
                case EntityVariant.VARIABLE:
                    item.Group = composite_content.Groups[0];
                    item.ImageIndex = 0;
                    break;
                case EntityVariant.FUNCTION:
                    if (_content.commands.GetComposite(((FunctionEntity)entity).function) != null)
                    {
                        item.Group = composite_content.Groups[2];
                        item.ImageIndex = 2;
                    }
                    else
                    {
                        item.Group = composite_content.Groups[1];
                        item.ImageIndex = 1;
                    }
                    break;
                case EntityVariant.PROXY:
                    item.Group = composite_content.Groups[3];
                    item.ImageIndex = 3;
                    break;
                case EntityVariant.ALIAS:
                    item.Group = composite_content.Groups[4];
                    item.ImageIndex = 4;
                    break;
            }

            composite_content.Items.Add(item);
        }

        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedEntityChanged?.Invoke(SelectedEntity);
        }

        private void entity_search_btn_Click(object sender, EventArgs e)
        {
            if (entity_search_box.Text == _currentSearch) return;

            List<Entity> allEntities = GetDisplayableEntities();
            List<Entity> filteredEntities = new List<Entity>();

            foreach (Entity entity in allEntities)
            {
                foreach (ListViewItem.ListViewSubItem subitem in _content.composite_content_cache[_composite][entity].SubItems)
                {
                    if (!subitem.Text.ToUpper().Contains(entity_search_box.Text.ToUpper())) continue;

                    filteredEntities.Add(entity);
                    break;
                }
            }

            PopulateEntities(filteredEntities);
            _currentSearch = entity_search_box.Text;
        }

        private void ClearSearch()
        {
            _currentSearch = "";
            entity_search_box.Text = "";
        }

        private List<Entity> GetDisplayableEntities()
        {
            List<Entity> entities = new List<Entity>();
            if (_displayOptions.DisplayAliases)
                entities.AddRange(_composite.aliases);
            if (_displayOptions.DisplayProxies)
                entities.AddRange(_composite.proxies);
            if (_displayOptions.DisplayFunctions)
                entities.AddRange(_composite.functions);
            if (_displayOptions.DisplayVariables)
                entities.AddRange(_composite.variables);
            return entities;
        }

        public class DisplayOptions
        {
            public bool ShowCheckboxes = true;

            public bool DisplayAliases = true;
            public bool DisplayProxies = true;
            public bool DisplayFunctions = true;
            public bool DisplayVariables = true;
        }
    }
}
