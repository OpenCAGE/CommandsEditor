﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE;
using CATHODE.Commands;
using CATHODE.Misc;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_EditHierarchy : Form
    {
        public List<ShortGuid> GeneratedHierarchy { get { return hierarchy; } }
        private List<ShortGuid> hierarchy = new List<ShortGuid>();

        private CathodeEntity selectedEntity = null;
        private CathodeComposite selectedComposite = null;

        public CathodeEditorGUI_EditHierarchy(CathodeComposite startingComposite)
        {
            InitializeComponent();
            LoadComposite(startingComposite.name);
        }

        /* Select a new entity from the composite, show fall through option if available */
        private void composite_content_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (composite_content.SelectedIndex == -1 || selectedComposite == null) return;
            try
            {
                ShortGuid entityID = new ShortGuid(composite_content.SelectedItem.ToString().Substring(1, 11));
                selectedEntity = selectedComposite.GetEntityByID(entityID);
                SelectEntity.Enabled = true;
                FollowEntityThrough.Enabled = false;

                if (selectedEntity.variant != EntityVariant.FUNCTION) return;
                FollowEntityThrough.Enabled = CurrentInstance.commandsPAK.GetComposite(((FunctionEntity)selectedEntity).function) != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Encountered an issue while looking up entity!\nPlease report this on GitHub!\n" + ex.Message, "Failed lookup!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /* Load a composite into the UI */
        private void LoadComposite(string FileName)
        {
            if (selectedEntity != null)
            {
                hierarchy.Add(selectedEntity.shortGUID);
                selectedEntity = null;
            }
            SelectEntity.Enabled = false;
            FollowEntityThrough.Enabled = false;

            selectedComposite = CurrentInstance.commandsPAK.Composites[CurrentInstance.commandsPAK.GetFileIndex(FileName)];
            compositeName.Text = selectedComposite.name;
            composite_content.BeginUpdate();
            //We only populate function entities here
            for (int i = 0; i < selectedComposite.functions.Count; i++)
            {
                string desc = EditorUtils.GenerateEntityName(selectedComposite.functions[i], selectedComposite);
                composite_content.Items.Add(desc);
            }
            composite_content.EndUpdate();
        }

        /* If selected entity is a composite instance, allow jump to it */
        private void FollowEntityThrough_Click(object sender, EventArgs e)
        {
            if (selectedEntity == null) return;
            if (selectedEntity.variant != EntityVariant.FUNCTION) return;

            CathodeComposite composite = CurrentInstance.commandsPAK.GetComposite(((FunctionEntity)selectedEntity).function);
            if (composite == null) return;

            LoadComposite(composite.name);
        }

        /* Generate the hierarchy */
        private void SelectEntity_Click(object sender, EventArgs e)
        {
            hierarchy.Add(selectedEntity.shortGUID);
            hierarchy.Add(new ShortGuid("00-00-00-00"));
        }
    }
}