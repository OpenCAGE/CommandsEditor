using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using CommandsEditor.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class RemoveParameter : BaseWindow
    {
        public Action OnSaved;

        private EntityDisplay _entityDisplay;

        public RemoveParameter(EntityDisplay entityDisplay) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();

            _entityDisplay = entityDisplay;

            parameterToDelete.BeginUpdate();

            //Links in
            List<Entity> ents = _entityDisplay.Composite.GetEntities();
            foreach (Entity ent in ents)
            {
                foreach (EntityConnector link in ent.childLinks)
                {
                    if (link.childID != _entityDisplay.Entity.shortGUID) 
                        continue;

                    EntLinkRef linkRef = new EntLinkRef()
                    {
                        Entity = ent,
                        Connector = link
                    };

                    ListViewItem item = new ListViewItem(ShortGuidUtils.FindString(link.childParamID));
                    item.Group = parameterToDelete.Groups[0];
                    item.SubItems.Add("[" + ShortGuidUtils.FindString(link.parentParamID) + "] " + _entityDisplay.Content.editor_utils.GenerateEntityName(ent, _entityDisplay.Composite));
                    item.Tag = linkRef;
                    item.ImageIndex = 1;
                    parameterToDelete.Items.Add(item);
                }
            }

            //Parameters
            for (int i = 0; i < _entityDisplay.Entity.parameters.Count; i++)
            {
                Parameter param = _entityDisplay.Entity.parameters[i];

                ListViewItem item = new ListViewItem(ShortGuidUtils.FindString(param.name));
                item.Group = parameterToDelete.Groups[1];
                item.SubItems.Add(param.content.dataType.ToString());
                item.Tag = param;
                item.ImageIndex = 2;
                parameterToDelete.Items.Add(item);
            }

            //Links out
            for (int i = 0; i < _entityDisplay.Entity.childLinks.Count; i++)
            {
                EntityConnector link = _entityDisplay.Entity.childLinks[i];

                ListViewItem item = new ListViewItem(ShortGuidUtils.FindString(link.parentParamID));
                item.Group = parameterToDelete.Groups[2];
                item.SubItems.Add("[" + ShortGuidUtils.FindString(link.childParamID) + "] " + _entityDisplay.Content.editor_utils.GenerateEntityName(_entityDisplay.Composite.GetEntityByID(link.childID), _entityDisplay.Composite));
                item.Tag = link;
                item.ImageIndex = 0;
                parameterToDelete.Items.Add(item);
            }

            if (parameterToDelete.Items.Count == 0)
            {
                this.Close();
                return;
            }

            parameterToDelete.EndUpdate();
        }

        private void delete_param_Click(object sender, EventArgs e)
        {
            if (parameterToDelete.CheckedItems.Count == 0) 
                return;

            if (MessageBox.Show("You are about to remove " + parameterToDelete.CheckedItems.Count + " parameter(s)/link(s). Are you sure? ", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) 
                return;

            foreach (ListViewItem item in  parameterToDelete.CheckedItems)
            {
                switch (item.Group.Name)
                {
                    case "Links In":
                        EntLinkRef linkRef = (EntLinkRef)item.Tag;
                        linkRef.Entity.childLinks.Remove(linkRef.Connector);
                        break;
                    case "Parameters":
                        _entityDisplay.Entity.parameters.Remove((Parameter)item.Tag);
                        break;
                    case "Links Out":
                        _entityDisplay.Entity.childLinks.Remove((EntityConnector)item.Tag);
                        break;
                }
            }

            OnSaved?.Invoke();
            this.Close();
        }

        private class EntLinkRef
        {
            public Entity Entity;
            public EntityConnector Connector;
        }
    }
}
