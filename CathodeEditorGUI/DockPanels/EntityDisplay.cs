using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CommandsEditor.DockPanels
{
    public partial class EntityDisplay : DockContent
    {
        private CompositeDisplay _compositeDisplay;
        private Entity _entity;


        public LevelContent Content => _compositeDisplay.Content;

        public Entity Entity => _entity;
        public Composite Composite => _compositeDisplay.Composite;

        public EntityDisplay(CompositeDisplay compositeDisplay, Entity entity)
        {
            _entity = entity;
            _compositeDisplay = compositeDisplay;

            InitializeComponent();
        }

        /* Add a new parameter */
        private void addNewParameter_Click(object sender, EventArgs e)
        {/*
            AddParameter add_parameter = new AddParameter(this, _entity);
            add_parameter.Show();
            add_parameter.FormClosed += new FormClosedEventHandler(refresh_entity_event);
        }
        private void refresh_entity_event(Object sender, FormClosedEventArgs e)
        {
            LoadEntity(Editor.selected.entity);
            this.BringToFront();
            this.Focus();*/
        }

        /* Add a new link out */
        private void addLinkOut_Click(object sender, EventArgs e)
        {
            /*
            if (Editor.selected.entity == null) return;
            AddOrEditLink add_link = new AddOrEditLink(this, Editor.selected.composite, Editor.selected.entity);
            add_link.Show();
            add_link.FormClosed += new FormClosedEventHandler(refresh_entity_event);*/
        }

        /* Remove a parameter */
        private void removeParameter_Click(object sender, EventArgs e)
        {/*
            if (Editor.selected.entity == null) return;
            if (entity_params.Controls.Count == 0) return;
            if (Editor.selected.entity.childLinks.Count + Editor.selected.entity.parameters.Count == 0) return;
            RemoveParameter remove_parameter = new RemoveParameter(this, Editor.selected.entity);
            remove_parameter.Show();
            remove_parameter.FormClosed += new FormClosedEventHandler(refresh_entity_event);*/
        }
    }
}
