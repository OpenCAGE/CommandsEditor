using CATHODE;
using CATHODE.Scripting;
using CATHODE.Scripting.Internal;
using CathodeLib;
using CommandsEditor.DockPanels;
using CommandsEditor.Popups.Base;
using OpenCAGE;
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
        public EntityVariant Variant;
        public bool Composite;

        public Action<Entity> OnNewEntity;

        List<Composite> composites = null;
        List<CathodeEntityDatabase.EntityDefinition> availableEntities = null;
        List<ShortGuid> hierarchy = null;

        private CompositeDisplay _compositeDisplay;

        public AddEntity(CompositeDisplay compositeDisplay, EntityVariant variant = EntityVariant.FUNCTION, bool composite = false) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_COMPOSITE_SELECTION, compositeDisplay.Content)
        {
            _compositeDisplay = compositeDisplay;

            composites = compositeDisplay.Content.commands.Entries.OrderBy(o => o.name).ToList();
            availableEntities = CathodeEntityDatabase.GetEntities();
            InitializeComponent();

            //quick hack to reload dropdown
            createDatatypeEntity.Checked = true;
            createFunctionEntity.Checked = true;

            Variant = variant;
            Composite = composite;

            switch (variant)
            {
                case EntityVariant.PROXY:
                    Text = "Create Proxy";
                    createProxyEntity.Checked = true;
                    break;
                case EntityVariant.FUNCTION:
                    Text = "Create " + (composite ? "Prefab Instance" : "Function");
                    createFunctionEntity.Checked = !composite;
                    createCompositeEntity.Checked = composite;
                    break;
                case EntityVariant.VARIABLE:
                    Text = "Create Prefab Parameter";
                    createDatatypeEntity.Checked = true;
                    break;
                case EntityVariant.ALIAS:
                    Text = "Create Alias";
                    createOverrideEntity.Checked = true;
                    break;
            }
        }

        //Repopulate UI
        private void selectedDatatypeEntity(object sender, EventArgs e)
        {
            //Datatype
            entityVariant.Visible = true;
            label2.Visible = true;
            generateHierarchy.Visible = false;
            select_composite.Visible = false;
            createNewEntity.Enabled = true;
            entityVariant.Enabled = true;
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
            label2.Visible = true;
            generateHierarchy.Visible = false;
            select_composite.Visible = false;
            createNewEntity.Enabled = true;
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < availableEntities.Count; i++) 
                entityVariant.Items.Add(availableEntities[i].className);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDown;
            addDefaultParams.Visible = true;
        }
        private void selectedCompositeEntity(object sender, EventArgs e)
        {
            //Composite
            label2.Visible = true;
            generateHierarchy.Visible = false;
            select_composite.Visible = true;
            createNewEntity.Enabled = true;
            entityVariant.BeginUpdate();
            entityVariant.Items.Clear();
            for (int i = 0; i < composites.Count; i++) 
                entityVariant.Items.Add(composites[i].name);
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDownList;
            entityVariant.Enabled = false;
            addDefaultParams.Visible = true;
        }
        private void selectedProxyEntity(object sender, EventArgs e)
        {
            //Proxy
            entityVariant.SelectedIndex = -1;
            entityVariant.Enabled = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            select_composite.Visible = false;
            createNewEntity.Enabled = false;
            hierarchy = null;
            addDefaultParams.Visible = true;
        }
        private void selectedOverrideEntity(object sender, EventArgs e)
        {
            //Override
            entityVariant.SelectedIndex = -1;
            entityVariant.Enabled = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            select_composite.Visible = false;
            createNewEntity.Enabled = false;
            hierarchy = null;
            addDefaultParams.Visible = false;
        }

        /* Generate path for proxy/alias */
        private void generateHierarchy_Click(object sender, EventArgs e)
        {
            EditHierarchy hierarchyEditor = null;
            if (createProxyEntity.Checked)
            {
                hierarchyEditor = new EditHierarchy(_content, Content.commands.EntryPoints[0], true);
            }
            else if (createOverrideEntity.Checked)
            {
                hierarchyEditor = new EditHierarchy(_content, _compositeDisplay.Composite, false);
            }
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            if (createProxyEntity.Checked)
            {
                hierarchy = new List<ShortGuid>();
                hierarchy.Add(Content.commands.EntryPoints[0].shortGUID);
                hierarchy.AddRange(generatedHierarchy);
                createNewEntity.Enabled = true;
            }
            else if(createOverrideEntity.Checked)
            {
                hierarchy = generatedHierarchy;
                createNewEntity.Enabled = true;
            }

            CommandsUtils.ResolveHierarchy(Content.commands, _compositeDisplay.Composite, generatedHierarchy, out Composite containedComp, out string hierarchyString, SettingsManager.GetBool("CS_ShowEntityIDs"));
            entityVariant.Enabled = true; 
            entityVariant.Text = hierarchyString;
            entityVariant.Enabled = false;

            this.Focus();
            this.BringToFront();
        }

        /* Select composite for composite entities (this populates the dropdown) */
        SelectComposite compositeSelector;
        private void select_composite_Click(object sender, EventArgs e)
        {
            if (compositeSelector != null)
                compositeSelector.Close();

            compositeSelector = new SelectComposite(_content, entityVariant.Text);
            compositeSelector.OnCompositeGenerated += OnCompositeGenerated;
            compositeSelector.Show();
        }
        private void OnCompositeGenerated(Composite composite)
        {
            entityVariant.Text = composite.name;
            this.Focus();
            this.BringToFront();
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
                if (function == FunctionType.PhysicsSystem && _compositeDisplay.Composite.functions.FirstOrDefault(o => o.function == CommandsUtils.GetFunctionTypeGUID(FunctionType.PhysicsSystem)) != null)
                {
                    MessageBox.Show("You are trying to add a PhysicsSystem entity to a prefab that already has one applied.", "PhysicsSystem error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                newEntity = _compositeDisplay.Composite.AddFunction(function, addDefaultParams.Checked);

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
                    MessageBox.Show("Failed to look up prefab!\nPlease report this issue on GitHub.\n\n" + entityVariant.Text, "Could not find prefab!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //Check logic errors (we can't have cyclical references)
                if (compRef == _compositeDisplay.Composite)
                {
                    MessageBox.Show("You cannot create an entity which instances the prefab it is contained with - this will result in an infinite loop at runtime! Please check your logic!.", "Logic error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newEntity = _compositeDisplay.Composite.AddFunction(compRef, addDefaultParams.Checked);
                Content.editor_utils.GenerateCompositeInstances(Content.commands);
            }
            else if (createDatatypeEntity.Checked)
                newEntity = _compositeDisplay.Composite.AddVariable(textBox1.Text, (DataType)entityVariant.SelectedIndex, true);
            else if (createProxyEntity.Checked)
                newEntity = _compositeDisplay.Composite.AddProxy(Content.commands, hierarchy, addDefaultParams.Checked);
            else if (createOverrideEntity.Checked)
                newEntity = _compositeDisplay.Composite.AddAlias(hierarchy);
            else
                return;

            if (!createDatatypeEntity.Checked) 
                EntityUtils.SetName(_compositeDisplay.Composite, newEntity, textBox1.Text);

            OnNewEntity?.Invoke(newEntity);
            this.Close();
        }
    }
}
