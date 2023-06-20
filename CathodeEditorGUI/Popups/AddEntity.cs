using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.Popups.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class AddEntity : BaseWindow
    {
        public Action<Entity> OnNewEntity;

        Composite composite = null;
        List<Composite> composites = null;
        List<CathodeEntityDatabase.EntityDefinition> availableEntities = null;
        List<ShortGuid> hierarchy = null;

        public AddEntity(CommandsEditor editor, Composite _comp, List<Composite> _comps) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor)
        {
            composite = _comp;
            composites = _comps.OrderBy(o => o.name).ToList();
            availableEntities = CathodeEntityDatabase.GetEntities();
            InitializeComponent();

            //quick hack to reload dropdown
            createDatatypeEntity.Checked = true;
            createFunctionEntity.Checked = true;
        }

        //Repopulate UI
        private void selectedDatatypeEntity(object sender, EventArgs e)
        {
            //Datatype
            entityVariant.Visible = true;
            label2.Visible = true;
            generateHierarchy.Visible = false;
            createNewEntity.Enabled = true;
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            entityVariant.Items.AddRange(new object[] {
                                    "STRING",
                                    "FLOAT",
                                    "INTEGER",
                                    "BOOL",
                                    "VECTOR",
                                    "TRANSFORM",
                                    "ENUM",
                                    "SPLINE",
                                    "RESOURCE"
                                    // TODO: we should support other types here, such as ZONE_LINK_PTR used in doors
            });
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDownList;
            addDefaultParams.Visible = false;
        }
        private void selectedFunctionEntity(object sender, EventArgs e)
        {
            //Function
            entityVariant.Visible = true;
            label2.Visible = true;
            generateHierarchy.Visible = false;
            createNewEntity.Enabled = true;
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < availableEntities.Count; i++) entityVariant.Items.Add(availableEntities[i].className);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDown;
            addDefaultParams.Visible = true;
        }
        private void selectedCompositeEntity(object sender, EventArgs e)
        {
            //Composite
            entityVariant.Visible = true;
            label2.Visible = true;
            generateHierarchy.Visible = false;
            createNewEntity.Enabled = true;
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < composites.Count; i++) entityVariant.Items.Add(composites[i].name);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDownList;
            addDefaultParams.Visible = true;
        }
        private void selectedProxyEntity(object sender, EventArgs e)
        {
            //Proxy
            entityVariant.Visible = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            createNewEntity.Enabled = false;
            hierarchy = null;
            addDefaultParams.Visible = true;
        }
        private void selectedOverrideEntity(object sender, EventArgs e)
        {
            //Override
            entityVariant.Visible = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            createNewEntity.Enabled = false;
            hierarchy = null;
            addDefaultParams.Visible = false;
        }

        /* Generate hierarchy for proxy/override */
        private void generateHierarchy_Click(object sender, EventArgs e)
        {
            EditHierarchy hierarchyEditor = null;
            if (createProxyEntity.Checked)
            {
                hierarchyEditor = new EditHierarchy(_editor, Editor.commands.EntryPoints[0], true);
            }
            else if (createOverrideEntity.Checked)
            {
                hierarchyEditor = new EditHierarchy(_editor, Editor.selected.composite, false);
            }
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            if (createProxyEntity.Checked)
            {
                hierarchy = new List<ShortGuid>();
                hierarchy.Add(Editor.commands.EntryPoints[0].shortGUID);
                hierarchy.AddRange(generatedHierarchy);
                createNewEntity.Enabled = true;
            }
            else if(createOverrideEntity.Checked)
            {
                hierarchy = generatedHierarchy;
                createNewEntity.Enabled = true;
            }
        }

        private void createEntity(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter an entity name!", "No name.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Entity newEntity = null;
            if (createFunctionEntity.Checked)
            {
                if (!Enum.TryParse(entityVariant.Text, out FunctionType function))
                {
                    MessageBox.Show("Please make sure you have typed or selected a valid entity class to create.", "Invalid entity class", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //A composite can only have one PhysicsSystem
                if (function == FunctionType.PhysicsSystem && composite.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.PhysicsSystem)) != null)
                {
                    MessageBox.Show("You are trying to add a PhysicsSystem entity to a composite that already has one applied.", "PhysicsSystem error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                newEntity = composite.AddFunction(function, addDefaultParams.Checked);

                //TODO: currently we don't support these properly
                if (addDefaultParams.Checked)
                {
                    newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.NONE); //TODO
                    newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.RESOURCE); //TODO
                }
            }
            else if (createCompositeEntity.Checked)
            {
                //Make sure the composite is valid
                Composite compRef = composites.FirstOrDefault(o => o.name == entityVariant.Text);
                if (compRef == null)
                { 
                    MessageBox.Show("Failed to look up composite!\nPlease report this issue on GitHub.\n\n" + entityVariant.Text, "Could not find composite!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newEntity = composite.AddFunction(compRef, addDefaultParams.Checked);
                EditorUtils.GenerateCompositeInstances(Editor.commands);
            }
            else if (createDatatypeEntity.Checked)
                newEntity = composite.AddVariable(textBox1.Text, (DataType)entityVariant.SelectedIndex, true);
            else if (createProxyEntity.Checked)
                newEntity = composite.AddProxy(Editor.commands, hierarchy, addDefaultParams.Checked);
            else if (createOverrideEntity.Checked)
                newEntity = composite.AddOverride(hierarchy);
            else
                return;

            if (!createDatatypeEntity.Checked) 
                EntityUtils.SetName(composite, newEntity, textBox1.Text);

            OnNewEntity?.Invoke(newEntity);
            this.Close();
        }
    }
}
