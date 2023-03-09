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
using CommandsEditor.Popups.Base;

namespace CommandsEditor
{
    public partial class AddOrEditLink : BaseWindow
    {
        public Action OnSaved;

        private List<Entity> _entityList = null;
        private Entity _initialParentEntity = null;
        private ShortGuid _initialLinkID;

        //FOR CREATING A NEW LINK
        public AddOrEditLink(Composite flowgraph, Entity parentEntity) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            RefreshEntityLists(flowgraph);
            RefreshChildParamList();
            RefreshParentParamList();

            parentEntityList.SelectedIndex = _entityList.IndexOf(parentEntity);
            parentEntityList.Enabled = false;
        }

        //FOR EDITING AN EXISTING LINK
        public AddOrEditLink(Composite flowgraph, Entity parentEntity, Entity childEntity, string parentParameter, string childParameter, bool isLinkingToChild, ShortGuid initialLinkID) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            RefreshEntityLists(flowgraph);
            RefreshChildParamList();
            RefreshParentParamList();

            parentParameterList.Text = parentParameter;
            childParameterList.Text = childParameter;

            parentEntityList.SelectedIndex = _entityList.IndexOf(parentEntity);
            childEntityList.SelectedIndex = _entityList.IndexOf(childEntity);

            parentEntityList.Enabled = !isLinkingToChild;
            childEntityList.Enabled = isLinkingToChild;

            _initialParentEntity = parentEntity;
            _initialLinkID = initialLinkID;
        }

        private void RefreshEntityLists(Composite comp)
        {
            _entityList = comp.GetEntities();
            _entityList = _entityList.OrderBy(o => EditorUtils.GenerateEntityName(o, comp).Substring(13)).ToList<Entity>();

            childEntityList.Enabled = true;
            parentEntityList.Enabled = true;

            childEntityList.BeginUpdate();
            parentEntityList.BeginUpdate();
            for (int i = 0; i < _entityList.Count; i++)
            {
                childEntityList.Items.Add(EditorUtils.GenerateEntityName(_entityList[i], comp));
                parentEntityList.Items.Add(EditorUtils.GenerateEntityName(_entityList[i], comp));
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
            List<string> items = EditorUtils.GenerateParameterList(_entityList[parentEntityList.SelectedIndex]);
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
            List<string> items = EditorUtils.GenerateParameterList(_entityList[childEntityList.SelectedIndex]);
            for (int i = 0; i < items.Count; i++) childParameterList.Items.Add(items[i]);
            childParameterList.EndUpdate();
        }
    }
}
