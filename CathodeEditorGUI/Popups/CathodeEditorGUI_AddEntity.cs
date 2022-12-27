using CATHODE;
using CATHODE.Commands;
using CathodeLib;
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

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddEntity : Form
    {
        public Action<Entity> OnNewEntity;

        Composite composite = null;
        List<Composite> composites = null;
        List<CathodeEntityDatabase.EntityDefinition> availableEntities = null;
        List<ShortGuid> hierarchy = null;

        public CathodeEditorGUI_AddEntity(Composite _comp, List<Composite> _comps)
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
                                    "SPLINE"
                                    // TODO: we should support other types here, such as ZONE_LINK_PTR used in doors
            });
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
            entityVariant.DropDownStyle = ComboBoxStyle.DropDownList;
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
            addDefaultParams.Visible = false;
        }
        private void selectedProxyEntity(object sender, EventArgs e)
        {
            //Proxy
            entityVariant.Visible = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            createNewEntity.Enabled = false;
            hierarchy = null;
            addDefaultParams.Visible = false;
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

            //TODO: Remove this warning when the checksum is figured out :)
            if (createOverrideEntity.Checked)
                MessageBox.Show("Please be aware that overrides are currently non-functional in-game due to an extra checksum used by Cathode to verify their existence, which has not yet been reverse engineered.\n\nIf you think you can help, feel free to submit a PR to CathodeLib on GitHub with the fix!", "Wait a minute!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /* Generate hierarchy for proxy/override */
        private void generateHierarchy_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_EditHierarchy hierarchyEditor = null;
            if (createProxyEntity.Checked)
            {
                hierarchyEditor = new CathodeEditorGUI_EditHierarchy(Editor.commands.EntryPoints[0]);
            }
            else if (createOverrideEntity.Checked)
            {
                hierarchyEditor = new CathodeEditorGUI_EditHierarchy(Editor.selected.composite);
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

            ShortGuid thisID = ShortGuidUtils.Generate(DateTime.Now.ToString("G"));

            if (createDatatypeEntity.Checked)
            {
                //Make the DatatypeEntity
                DatatypeEntity newEntity = new DatatypeEntity(thisID);
                newEntity.type = (DataType)entityVariant.SelectedIndex;
                newEntity.parameter = ShortGuidUtils.Generate(textBox1.Text);

                //Make the parameter to give this DatatypeEntity a value (the only time you WOULDN'T want this is if the val is coming from a linked entity)
                ParameterData thisParam = null;
                switch (newEntity.type)
                {
                    case DataType.STRING:
                        thisParam = new cString("");
                        break;
                    case DataType.FLOAT:
                        thisParam = new cFloat(0.0f);
                        break;
                    case DataType.INTEGER:
                        thisParam = new cInteger(0);
                        break;
                    case DataType.BOOL:
                        thisParam = new cBool(true);
                        break;
                    case DataType.VECTOR:
                        thisParam = new cVector3(new Vector3(0, 0, 0));
                        break;
                    case DataType.TRANSFORM:
                        thisParam = new cTransform(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
                        break;
                    case DataType.ENUM:
                        thisParam = new cEnum("ALERTNESS_STATE", 0); //ALERTNESS_STATE is the first alphabetically
                        break;
                    case DataType.SPLINE:
                        thisParam = new cSpline();
                        break;
                }
                newEntity.parameters.Add(new Parameter(newEntity.parameter, thisParam));

                //Add to composite & save name
                composite.datatypes.Add(newEntity);
                ShortGuidUtils.Generate(textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createFunctionEntity.Checked)
            {
                //Create FunctionEntity
                FunctionEntity newEntity = new FunctionEntity(thisID);
                newEntity.function = CathodeEntityDatabase.GetEntity(entityVariant.Text).guid;
                if (newEntity.function.val == null)
                {
                    MessageBox.Show("Please make sure you have typed or selected a valid entity class to create.", "Invalid entity class", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (addDefaultParams.Checked)
                {
                    Editor.util.entity.ApplyDefaults(newEntity);
                    newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.NONE); //TODO
                    newEntity.parameters.RemoveAll(o => o.content.dataType == DataType.RESOURCE); //TODO
                }


                //Add to composite & save name
                composite.functions.Add(newEntity);
                Editor.util.entity.SetName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createCompositeEntity.Checked)
            {
                //Create FunctionEntity
                FunctionEntity newEntity = new FunctionEntity(thisID);
                Composite composite = composites.FirstOrDefault(o => o.name == entityVariant.Text);
                if (composite == null)
                { 
                    MessageBox.Show("Failed to look up composite!\nPlease report this issue on GitHub.\n\n" + entityVariant.Text, "Could not find composite!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newEntity.function = CommandsUtils.GetFunctionTypeGUID(FunctionType.CompositeInterface);
                Editor.util.entity.ApplyDefaults(newEntity);
                newEntity.function = composite.shortGUID;

                //Add to composite & save name
                this.composite.functions.Add(newEntity);
                Editor.util.entity.SetName(this.composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createProxyEntity.Checked)
            {
                //Create ProxyEntity
                ProxyEntity newEntity = new ProxyEntity(thisID);
                newEntity.hierarchy = hierarchy;
                newEntity.extraId = ShortGuidUtils.Generate(DateTime.Now.ToString("G") + "temp");
                newEntity.parameters.Add(new Parameter("proxy_filter_targets", new cBool(false)));
                newEntity.parameters.Add(new Parameter("proxy_enable_on_reset", new cBool(false)));
                newEntity.parameters.Add(new Parameter("proxy_enable", new cFloat(0.0f)));
                newEntity.parameters.Add(new Parameter("proxy_enabled", new cFloat(0.0f)));
                newEntity.parameters.Add(new Parameter("proxy_disable", new cFloat(0.0f)));
                newEntity.parameters.Add(new Parameter("proxy_disabled", new cFloat(0.0f)));
                newEntity.parameters.Add(new Parameter("reference", new cString("")));
                newEntity.parameters.Add(new Parameter("trigger", new cFloat(0.0f)));

                //Add to composite & save name
                composite.proxies.Add(newEntity);
                Editor.util.entity.SetName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createOverrideEntity.Checked)
            {
                //Create OverrideEntity
                OverrideEntity newEntity = new OverrideEntity(thisID);
                newEntity.hierarchy = hierarchy;
                newEntity.checksum = ShortGuidUtils.Generate("temp"); //TODO: how do we generate this? without it, i think overrides won't work.

                //Add to composite & save name
                composite.overrides.Add(newEntity);
                Editor.util.entity.SetName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            this.Close();
        }
    }
}
