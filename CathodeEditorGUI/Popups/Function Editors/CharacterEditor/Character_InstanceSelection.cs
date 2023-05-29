using CATHODE.Scripting;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor.Popups.Function_Editors.CharacterEditor
{
    public partial class Character_InstanceSelection : BaseWindow
    {
        public Action<ShortGuid> OnInstanceSelected;

        private List<EntityHierarchy> _hierarchies = new List<EntityHierarchy>();

        public Character_InstanceSelection(CommandsEditor editor, List<ShortGuid> existing) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            InitializeComponent(); 
            
            List<EntityHierarchy> hierarchies = EditorUtils.GetHierarchiesForEntity(Editor.selected.composite, Editor.selected.entity);
            for (int i = 0; i < hierarchies.Count; i++)
            {
                if (existing.Contains(hierarchies[i].GenerateInstance())) continue;
                characterInstances.Items.Add(hierarchies[i].GetHierarchyAsString(Editor.commands, Editor.selected.composite, false));
                _hierarchies.Add(hierarchies[i]);
            }

            if (characterInstances.Items.Count == 0)
            {
                MessageBox.Show("There are no other Character instances to be populated!", "Characters populated.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else characterInstances.SelectedIndex = 0;
        }

        private void addCharacter_Click(object sender, EventArgs e)
        {
            if (characterInstances.SelectedIndex == -1) return;
            OnInstanceSelected?.Invoke(_hierarchies[characterInstances.SelectedIndex].GenerateInstance());
            this.Close();
        }
    }
}
