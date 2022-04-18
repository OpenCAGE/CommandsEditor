﻿using CATHODE;
using CATHODE.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_RenameEntity : Form
    {
        public string EntityName { get { return entity_name.Text; } }
        public ShortGuid EntityID;
        public bool didSave = false;

        public CathodeEditorGUI_RenameEntity(ShortGuid entityID)
        {
            InitializeComponent();
            entity_name.Text = CurrentInstance.compositeLookup.GetEntityName(CurrentInstance.selectedComposite.shortGUID, entityID);
            EntityID = entityID;
        }

        private void save_entity_name_Click(object sender, EventArgs e)
        {
            if (entity_name.Text == "") return;
            didSave = true;
            this.Close();
        }
    }
}