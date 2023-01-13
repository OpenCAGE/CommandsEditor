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

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddOrEditLink : Form
    {
        public Action OnSaved;

        private Entity _parentEntity = null;
        private List<Entity> _entityList = null;

        private bool _isEditing = false;
        private bool _isEditingLinkOut = false;
        private ShortGuid _existingLinkID;

        public CathodeEditorGUI_AddOrEditLink(Composite flowgraph, Entity parentEntity, Entity childEntity = null, EntityLink link = new EntityLink(), bool isLinkingOut = false)
        {
            _parentEntity = parentEntity;
            InitializeComponent();

            _entityList = flowgraph.GetEntities();
            _entityList = _entityList.OrderBy(o => EditorUtils.GenerateEntityName(o, flowgraph).Substring(13)).ToList<Entity>();

            parentEntityList.Items.Add(EditorUtils.GenerateEntityName(_parentEntity, flowgraph));
            parentEntityList.SelectedIndex = 0;
            parentEntityList.Enabled = false;

            childEntityList.BeginUpdate();
            for (int i = 0; i < _entityList.Count; i++)
            {
                childEntityList.Items.Add(EditorUtils.GenerateEntityName(_entityList[i], flowgraph));
            }
            childEntityList.EndUpdate();

            RefreshPinInParams();
            RefreshPinOutParams();

            //If we're editing an existing link...
            if (childEntity != null && link.childID.val != null)
            {
                _isEditing = true;
                _isEditingLinkOut = isLinkingOut;
                _existingLinkID = link.connectionID;

                for (int i = 0; i < _entityList.Count; i++)
                {
                    if (_entityList[i] != childEntity) continue;
                    childEntityList.SelectedIndex = i;
                    break;
                }

                //If we're allowing selection of parent instead of child, update the UI for that
                if (!isLinkingOut)
                {
                    parentEntityList.BeginUpdate();
                    parentEntityList.Enabled = true;
                    parentEntityList.Items.Clear();
                    parentEntityList.Items.AddRange(childEntityList.Items.Cast<Object>().ToArray());
                    parentEntityList.EndUpdate();

                    childEntityList.Enabled = false;
                }

                parentParameter.Text = ShortGuidUtils.FindString(link.parentParamID);
                childParameter.Text = ShortGuidUtils.FindString(link.childParamID);

                RefreshPinInParams();
                RefreshPinOutParams();
            }
        }

        private void save_pin_Click(object sender, EventArgs e)
        {
            if (childEntityList.SelectedIndex == -1 || parentParameter.Text == "" || childParameter.Text == "")
            {
                MessageBox.Show("Please complete all information for the link before saving!", "Incomplete information.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_isEditing)
            {
                _parentEntity.childLinks.RemoveAll(o => o.connectionID == _existingLinkID);
                if (!_isEditingLinkOut) _parentEntity = _entityList[parentEntityList.SelectedIndex];
            }
            _parentEntity.AddParameterLink(parentParameter.Text, _entityList[childEntityList.SelectedIndex], childParameter.Text);
            OnSaved?.Invoke();
            this.Close();
        }

        private void pin_in_node_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPinInParams();
        }
        private void RefreshPinInParams()
        {
            childParameter.BeginUpdate();
            childParameter.Items.Clear();
            if (childEntityList.SelectedIndex == -1) return;
            List<string> items = EditorUtils.GenerateParameterList(_entityList[childEntityList.SelectedIndex]);
            for (int i = 0; i < items.Count; i++) childParameter.Items.Add(items[i]);
            childParameter.EndUpdate();
        }

        private void pin_out_node_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPinOutParams();
        }
        private void RefreshPinOutParams()
        {
            parentParameter.BeginUpdate();
            parentParameter.Items.Clear();
            if (!_isEditingLinkOut && parentEntityList.SelectedIndex == -1) return;
            List<string> items = EditorUtils.GenerateParameterList((!_isEditingLinkOut) ? _entityList[parentEntityList.SelectedIndex] : _parentEntity);
            for (int i = 0; i < items.Count; i++) parentParameter.Items.Add(items[i]);
            parentParameter.EndUpdate();
        }
    }
}
