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
            renameToolStripMenuItem.Enabled = hasSelectedEntity && compositeEntityList1.SelectedEntity.variant != EntityVariant.ALIAS;
            duplicateToolStripMenuItem.Enabled = hasSelectedEntity;
        }

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
    }
}
