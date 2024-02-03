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
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using CommandsEditor.Popups.UserControls;

namespace CommandsEditor
{
    public partial class AddOrEditLink : BaseWindow
    {
        public Action OnSaved;

        private List<Entity> _entityList = null;
        private Entity _initialParentEntity = null;
        private ShortGuid _initialLinkID;

        private EntityDisplay _entityDisplay;

        //FOR CREATING A NEW LINK
        public AddOrEditLink(EntityDisplay entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, entityDisplay.Content)
        {
            _entityDisplay = entityDisplay;
            InitializeComponent();

            RefreshEntityLists(entityDisplay.Composite);
            RefreshChildParamList();
            RefreshParentParamList();

            parentEntityList.SelectedIndex = _entityList.IndexOf(entityDisplay.Entity);
            parentEntityList.Enabled = false;
            selectEntityOut.Enabled = false;

            parentParameterList.AutoSelectOff();
            childParameterList.AutoSelectOff();
        }

        //FOR EDITING AN EXISTING LINK
        public AddOrEditLink(EntityDisplay entityDisplay, Entity parentEntity, Entity childEntity, string parentParameter, string childParameter, bool isLinkingToChild, ShortGuid initialLinkID) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, entityDisplay.Content)
        {
            _entityDisplay = entityDisplay;
            InitializeComponent();

            RefreshEntityLists(entityDisplay.Composite);
            RefreshChildParamList();
            RefreshParentParamList();

            parentParameterList.Text = parentParameter;
            childParameterList.Text = childParameter;

            parentEntityList.SelectedIndex = _entityList.IndexOf(parentEntity);
            childEntityList.SelectedIndex = _entityList.IndexOf(childEntity);

            parentEntityList.Enabled = !isLinkingToChild;
            childEntityList.Enabled = isLinkingToChild;

            selectEntityOut.Enabled = !isLinkingToChild;
            selectEntityIn.Enabled = isLinkingToChild;

            _initialParentEntity = parentEntity;
            _initialLinkID = initialLinkID;

            parentParameterList.AutoSelectOff();
            childParameterList.AutoSelectOff();
        }

        private void RefreshEntityLists(Composite comp)
        {
            _entityList = comp.GetEntities();
            _entityList = _entityList.OrderBy(o => _entityDisplay.Content.editor_utils.GenerateEntityName(o, comp).Substring(13)).ToList<Entity>();

            childEntityList.Enabled = true;
            parentEntityList.Enabled = true;

            childEntityList.BeginUpdate();
            parentEntityList.BeginUpdate();
            for (int i = 0; i < _entityList.Count; i++)
            {
                childEntityList.Items.Add(_entityDisplay.Content.editor_utils.GenerateEntityName(_entityList[i], comp));
                parentEntityList.Items.Add(_entityDisplay.Content.editor_utils.GenerateEntityName(_entityList[i], comp));
            }
            childEntityList.EndUpdate();
            parentEntityList.EndUpdate();
        }

        private void save_pin_Click(object sender, EventArgs e)
        {
            if (parentEntityList.SelectedIndex == -1 || childEntityList.SelectedIndex == -1 || parentParameterList.Text == "" || childParameterList.Text == "")
            {
                MessageBox.Show("Please complete all information for the link before saving!", "Incomplete information.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_initialParentEntity != null) _initialParentEntity.childLinks.RemoveAll(o => o.connectionID == _initialLinkID);
            _entityList[parentEntityList.SelectedIndex].AddParameterLink(parentParameterList.Text, _entityList[childEntityList.SelectedIndex], childParameterList.Text);

            OnSaved?.Invoke();
            this.Close();
        }

        private void pin_out_node_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshParentParamList();
        }
        private void RefreshParentParamList()
        {
            parentParameterList.BeginUpdate();
            parentParameterList.Items.Clear();
            if (parentEntityList.SelectedIndex == -1) return;
            List<string> items = _entityDisplay.Content.editor_utils.GenerateParameterList(_entityList[parentEntityList.SelectedIndex], _entityDisplay.Composite);
            for (int i = 0; i < items.Count; i++) parentParameterList.Items.Add(items[i]);
            parentParameterList.EndUpdate();
        }

        private void pin_in_node_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshChildParamList();
        }
        private void RefreshChildParamList()
        {
            childParameterList.BeginUpdate();
            childParameterList.Items.Clear();
            if (childEntityList.SelectedIndex == -1) return;
            List<string> items = _entityDisplay.Content.editor_utils.GenerateParameterList(_entityList[childEntityList.SelectedIndex], _entityDisplay.Composite);
            for (int i = 0; i < items.Count; i++) childParameterList.Items.Add(items[i]);
            childParameterList.EndUpdate();
        }

        EditHierarchy _selectEntOut = null;
        private void selectEntityOut_Click(object sender, EventArgs e)
        {
            if (_selectEntOut != null)
            {
                _selectEntOut.Close();
                _selectEntOut = null;
            }

            _selectEntOut = new EditHierarchy(Content, _entityDisplay.Composite, new CompositeEntityList.DisplayOptions() { ShowCheckboxes = false }, false);
            _selectEntOut.Show();
            _selectEntOut.OnHierarchyGenerated += OnSelectedEntityOut;
        }
        private void OnSelectedEntityOut(List<ShortGuid> hierarchy)
        {
            parentEntityList.SelectedIndex = _entityList.IndexOf(_entityDisplay.Composite.GetEntityByID(hierarchy[0]));
        }

        EditHierarchy _selectEntIn = null;
        private void selectEntityIn_Click(object sender, EventArgs e)
        {
            if (_selectEntIn != null)
            {
                _selectEntIn.Close();
                _selectEntIn = null;
            }

            _selectEntIn = new EditHierarchy(Content, _entityDisplay.Composite, new CompositeEntityList.DisplayOptions() { ShowCheckboxes = false }, false);
            _selectEntIn.Show();
            _selectEntIn.OnHierarchyGenerated += OnSelectedEntityIn;
        }
        private void OnSelectedEntityIn(List<ShortGuid> hierarchy)
        {
            childEntityList.SelectedIndex = _entityList.IndexOf(_entityDisplay.Composite.GetEntityByID(hierarchy[0]));
        }
    }
}
