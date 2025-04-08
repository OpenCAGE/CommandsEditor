using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.Popups.UserControls;
using CommandsEditor.Properties;
using CommandsEditor.UserControls;
using OpenCAGE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.DockPanels
{
    public partial class EntityList : DockContent
    {
        protected LevelContent Content => Singleton.Editor?.CommandsDisplay?.Content;

        public CompositeEntityList List => compositeEntityList1;

        public EntityList()
        {
            InitializeComponent();

            compositeEntityList1.ContextMenuStrip = EntityListContextMenu;

            compositeEntityList1.SelectedEntityChanged += OnEntitySelected;
            this.FormClosed += EntityList_FormClosed;

            this.DockStateChanged += EntityList_DockStateChanged;

            this.CloseButtonVisible = false;
        }

        private void EntityList_DockStateChanged(object sender, EventArgs e)
        {
            if (DockState == DockState.Unknown || DockState == DockState.Hidden)
                return;

            if (DockState == _previousDockState) return;
            _previousDockState = DockState;

            SettingsManager.SetString(Singleton.Settings.EntityListState, DockState.ToString());
        }

        private void EntityList_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.FormClosed -= EntityList_FormClosed;
            if (_entityRenameDialog != null)
                _entityRenameDialog.FormClosed -= _entityRenameDialog_FormClosed;
        }

        private void OnEntitySelected(Entity entity)
        {
            Singleton.OnEntitySelected?.Invoke(entity);
        }

        //disable entity-related actions on the context menu if no entity is selected
        private void EntityListContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasSelectedEntity = compositeEntityList1.SelectedEntity != null;

            deleteToolStripMenuItem.Enabled = hasSelectedEntity;
            renameToolStripMenuItem.Enabled = hasSelectedEntity && compositeEntityList1.SelectedEntity.variant != EntityVariant.ALIAS && compositeEntityList1.SelectedEntity.variant != EntityVariant.VARIABLE;
            duplicateToolStripMenuItem.Enabled = hasSelectedEntity && compositeEntityList1.SelectedEntity.variant != EntityVariant.ALIAS && compositeEntityList1.SelectedEntity.variant != EntityVariant.VARIABLE;
        }

        //Temporarily hijacked these options here: they should be handled in CompositeDisplay really...
        private void createParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Editor.CommandsDisplay.CompositeDisplay.CreateEntity(EntityVariant.VARIABLE);
        }
        private void createFunctionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Editor.CommandsDisplay.CompositeDisplay.CreateEntity(EntityVariant.FUNCTION);
        }
        private void createInstanceOfCompositeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Editor.CommandsDisplay.CompositeDisplay.CreateEntity(EntityVariant.FUNCTION, true);
        }
        private void createProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Editor.CommandsDisplay.CompositeDisplay.CreateEntity(EntityVariant.PROXY);
        }
        private void createAliasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Singleton.Editor.CommandsDisplay.CompositeDisplay.CreateEntity(EntityVariant.ALIAS);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Editor.CommandsDisplay.CompositeDisplay.DeleteEntity(List.SelectedEntity);
        }
        RenameEntity _entityRenameDialog = null;
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_entityRenameDialog != null)
                _entityRenameDialog.Close();

            _entityRenameDialog = new RenameEntity(List.SelectedEntity, Singleton.Editor.CommandsDisplay.CompositeDisplay.Composite);
            _entityRenameDialog.Show();
            _entityRenameDialog.FormClosed += _entityRenameDialog_FormClosed;
        }
        private void _entityRenameDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            _entityRenameDialog = null;
        }
        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Singleton.Editor.CommandsDisplay.CompositeDisplay.DuplicateEntity(List.SelectedEntity);
        }
    }
}
