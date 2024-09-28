using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using CommandsEditor.Popups.UserControls;

namespace CommandsEditor
{
    public partial class SelectHierarchy : BaseWindow
    {
        public Action<Entity> OnFinalEntitySelected;
        public Action<List<ShortGuid>> OnHierarchyGenerated;
        private List<ShortGuid> hierarchy = new List<ShortGuid>();

        private Entity selectedEntity = null;
        private Composite selectedComposite = null;

        private CompositePath _path = new CompositePath();

        //PROXIES can only point to FunctionEntities - ALIASES can point to FunctionEntities, ProxyEntities, VariableEntities
        public SelectHierarchy(Composite startingComposite, CompositeEntityList.DisplayOptions displayOptions, bool allowFollowThrough = true) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            displayOptions.ShowCheckboxes = false;
            compositeEntityList1.Setup(startingComposite, displayOptions);
            compositeEntityList1.SelectedEntityChanged += OnSelectedEntityChanged;

            LoadComposite(startingComposite);
            FollowEntityThrough.Visible = allowFollowThrough;
        }

        /* Select a new entity from the composite, show fall through option if available */
        private void OnSelectedEntityChanged(Entity entity)
        {
            if (entity == null) return;

            selectedEntity = entity;
            SelectEntity.Enabled = true;
            FollowEntityThrough.Enabled = false;

            if (selectedEntity.variant != EntityVariant.FUNCTION) return;
            FollowEntityThrough.Enabled = Content.commands.GetComposite(((FunctionEntity)selectedEntity).function) != null;
        }

        /* Load a composite into the UI */
        private void LoadComposite(Composite composite)
        {
            if (selectedEntity != null)
            {
                hierarchy.Add(selectedEntity.shortGUID);
                selectedEntity = null;
            }
            SelectEntity.Enabled = false;
            FollowEntityThrough.Enabled = false;

            selectedComposite = composite;
            pathDisplay.Text = _path.GetPath(composite);

            compositeEntityList1.LoadComposite(selectedComposite);
        }

        /* If selected entity is a composite instance, allow jump to it */
        private void FollowEntityThrough_Click(object sender, EventArgs e)
        {
            if (selectedEntity == null) return;
            if (selectedEntity.variant != EntityVariant.FUNCTION) return;

            Composite composite = Content.commands.GetComposite(((FunctionEntity)selectedEntity).function);
            if (composite == null) return;

            _path.StepForwards(selectedComposite, selectedEntity);
            LoadComposite(composite);
        }

        /* Generate the hierarchy */
        private void SelectEntity_Click(object sender, EventArgs e)
        {
            //TODO: should use the proper hierarchy class here
            hierarchy.Add(selectedEntity.shortGUID);
            hierarchy.Add(ShortGuid.Invalid);
            OnHierarchyGenerated?.Invoke(hierarchy);
            OnFinalEntitySelected?.Invoke(selectedEntity);
            this.Close();
        }

        private void goBackOnPath_Click(object sender, EventArgs e)
        {
            if (_path.StepBackwards(out Composite composite, out Entity entity))
            {
                LoadComposite(composite);
            }
        }
    }
}
