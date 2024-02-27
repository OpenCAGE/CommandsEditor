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

        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        private string _currentSearch = "";
        private DisplayOptions _displayOptions;

        public CompositeEntityList()
        {
            InitializeComponent();
            ClearSearch();

            clearSearchBtn.BringToFront();

            this.Disposed += CompositeEntityList_Disposed;

            Singleton.OnEntityRenamed += OnEntityRenamed;
            Singleton.OnCompositeRenamed += OnCompositeRenamed;
        }

        private void CompositeEntityList_Disposed(object sender, EventArgs e)
        {
            this.Disposed -= CompositeEntityList_Disposed;

            Singleton.OnEntityRenamed -= OnEntityRenamed;
            Singleton.OnCompositeRenamed -= OnCompositeRenamed;

            composite_content.Items.Clear();
        }

        //TODO: this is not as efficient as it could be: really we should only modify the ListViewItems that have been affected
        private void OnEntityRenamed(Entity entity, string name)
        {
            ReloadComposite();
        }
        private void OnCompositeRenamed(Composite composite, string name)
        {
            ReloadComposite();
        }

        /* This UserControl differs from BaseUserControl because we don't instantiate at runtime - so make sure to call setup in code to pass this construction info before you use it. */
        public void Setup(Composite composite, DisplayOptions displayOptions = null, bool doReload = true)
        {
            _composite = composite;
            SetDisplayOptions(displayOptions, doReload);
        }

        /* Update the display options to handle filtering out certain entity types */
        public void SetDisplayOptions(DisplayOptions displayOptions, bool doReload = true)
        {
            _displayOptions = displayOptions == null ? new DisplayOptions() : displayOptions;
            composite_content.CheckBoxes = _displayOptions.ShowCheckboxes;

            if (doReload)
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

            //By calling a search again, we won't necessarily show ALL entities when loading, but we'll respect the user's search, which is better
            DoSearch();
        }

        /* Add a new entity to the list */
        public ListViewItem AddNewEntity(Entity entity, bool useCache = true, bool skipSanityChecks = false)
        {
            if (!skipSanityChecks)
            {
                if (entity.variant == EntityVariant.ALIAS && !_displayOptions.DisplayAliases)
                    return null;
                if (entity.variant == EntityVariant.PROXY && !_displayOptions.DisplayProxies)
                    return null;
                if (entity.variant == EntityVariant.FUNCTION && !_displayOptions.DisplayFunctions)
                    return null;
                if (entity.variant == EntityVariant.VARIABLE && !_displayOptions.DisplayVariables)
                    return null;
            }

            ListViewItem item = Content.GenerateListViewItem(entity, _composite, useCache ? LevelContent.CacheMethod.CHECK_OR_POPULATE_CACHE : LevelContent.CacheMethod.IGNORE_CACHE);

            //Keep these indexes in sync with ListViewGroup 
            switch (entity.variant)
            {
                case EntityVariant.VARIABLE:
                    item.Group = composite_content.Groups[0];
                    item.ImageIndex = 0;
                    break;
                case EntityVariant.FUNCTION:
                    if (!CommandsUtils.FunctionTypeExists(((FunctionEntity)entity).function))
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

            return item;
        }

        /* Focus the entity list */
        public void FocusOnList()
        {
            composite_content.Focus();
        }

        private void PopulateEntities(List<Entity> entities)
        {
            bool hasID = composite_content.Columns.ContainsKey("ID");
            bool showID = SettingsManager.GetBool(Singleton.Settings.EntIdOpt);
            if (showID && !hasID)
                composite_content.Columns.Add(new ColumnHeader() { Name = "ID", Text = "ID", Width = 100 });
            else if (!showID && hasID)
                composite_content.Columns.RemoveByKey("ID");

            List<Entity> ents = entities.FindAll(entity =>
                (entity.variant == EntityVariant.ALIAS && _displayOptions.DisplayAliases) ||
                (entity.variant == EntityVariant.PROXY && _displayOptions.DisplayProxies) ||
                (entity.variant == EntityVariant.FUNCTION && _displayOptions.DisplayFunctions) ||
                (entity.variant == EntityVariant.VARIABLE && _displayOptions.DisplayVariables)
            );

            ListViewItem[] items = new ListViewItem[ents.Count];
            Parallel.For(0, ents.Count, (i) =>
            {
                items[i] = AddNewEntity(entities[i], false, true);
            });

            composite_content.BeginUpdate();
            composite_content.SuspendLayout();
            composite_content.Items.Clear();
            composite_content.Items.AddRange(items);
            //composite_content.SetGroupState(ListViewGroupState.Collapsible);
            composite_content.EndUpdate();
            composite_content.ResumeLayout();
        }

        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedEntityChanged?.Invoke(SelectedEntity);
        }

        private void entity_search_btn_Click(object sender, EventArgs e)
        {
            if (entity_search_box.Text == _currentSearch) return;
            _currentSearch = entity_search_box.Text;

            clearSearchBtn.Visible = _currentSearch != "";

            DoSearch();
        }

        private void clearSearchBtn_Click(object sender, EventArgs e)
        {
            if (entity_search_box.Text == "" && _currentSearch == "")
                return;

            ClearSearch();
            DoSearch();
        }

        private void DoSearch()
        {
            List<Entity> allEntities = GetDisplayableEntities();
            List<Entity> filteredEntities = new List<Entity>();

            //NOTE: we look at current search, NOT the text in the textbox - we want to respect the user's button click when reloading
            if (_currentSearch == "")
            {
                filteredEntities = allEntities;
            }
            else
            {
                for (int i = 0; i < allEntities.Count; i++)
                {
                    ListViewItem item = Content.GenerateListViewItem(allEntities[i], _composite);
                    for (int x = 0; x < item.SubItems.Count; x++)
                    {
                        if (!item.SubItems[x].Text.ToUpper().Contains(_currentSearch.ToUpper()))
                            continue;

                        filteredEntities.Add(allEntities[i]);
                        break;
                    }
                }
            }

            PopulateEntities(filteredEntities);
        }

        private void ClearSearch()
        {
            _currentSearch = "";
            entity_search_box.Text = "";
            clearSearchBtn.Visible = false;
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
