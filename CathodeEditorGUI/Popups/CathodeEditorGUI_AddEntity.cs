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
using System.Windows.Forms;

namespace CathodeEditorGUI
{
    public partial class CathodeEditorGUI_AddEntity : Form
    {
        public Action<CathodeEntity> OnNewEntity;

        CathodeComposite composite = null;
        List<CathodeComposite> composites = null;
        List<CathodeEntityDatabase.EntityDefinition> availableEntities = null;
        List<ShortGuid> hierarchy = null;

        public CathodeEditorGUI_AddEntity(CathodeComposite _comp, List<CathodeComposite> _comps)
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
                                    "POSITION",
                                    "FLOAT",
                                    "STRING",
                                    "SPLINE_DATA",
                                    "ENUM",
                                    "SHORT_GUID",
                                    "FILEPATH",
                                    "BOOL",
                                    "DIRECTION",
                                    "INTEGER"
            });
            entityVariant.EndUpdate();
            entityVariant.SelectedIndex = 0;
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
        }
        private void selectedProxyEntity(object sender, EventArgs e)
        {
            //Proxy
            entityVariant.Visible = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            createNewEntity.Enabled = false;
            hierarchy = null;
        }
        private void selectedOverrideEntity(object sender, EventArgs e)
        {
            //Override
            entityVariant.Visible = false;
            label2.Visible = false;
            generateHierarchy.Visible = true;
            createNewEntity.Enabled = false;
            hierarchy = null;
        }

        /* Generate hierarchy for proxy/override */
        private void generateHierarchy_Click(object sender, EventArgs e)
        {
            CathodeEditorGUI_EditHierarchy hierarchyEditor = null;
            if (createProxyEntity.Checked)
            {
                hierarchyEditor = new CathodeEditorGUI_EditHierarchy(CurrentInstance.commandsPAK.EntryPoints[0]);
            }
            else if (createOverrideEntity.Checked)
            {
                hierarchyEditor = new CathodeEditorGUI_EditHierarchy(CurrentInstance.selectedComposite);
            }
            hierarchyEditor.Show();
            hierarchyEditor.OnHierarchyGenerated += HierarchyEditor_HierarchyGenerated;
        }
        private void HierarchyEditor_HierarchyGenerated(List<ShortGuid> generatedHierarchy)
        {
            if (createProxyEntity.Checked)
            {
                hierarchy = new List<ShortGuid>();
                hierarchy.Add(CurrentInstance.commandsPAK.EntryPoints[0].shortGUID);
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
                newEntity.type = (CathodeDataType)entityVariant.SelectedIndex;
                newEntity.parameter = ShortGuidUtils.Generate(textBox1.Text);

                //Make the parameter to give this DatatypeEntity a value (the only time you WOULDN'T want this is if the val is coming from a linked entity)
                CathodeParameter thisParam = null;
                switch (newEntity.type)
                {
                    case CathodeDataType.POSITION:
                        thisParam = new CathodeTransform();
                        break;
                    case CathodeDataType.FLOAT:
                        thisParam = new CathodeFloat();
                        break;
                    case CathodeDataType.FILEPATH:
                    case CathodeDataType.STRING:
                        thisParam = new CathodeString();
                        break;
                    case CathodeDataType.SPLINE_DATA:
                        thisParam = new CathodeSpline();
                        break;
                    case CathodeDataType.ENUM:
                        thisParam = new CathodeEnum();
                        ((CathodeEnum)thisParam).enumID = new ShortGuid("4C-B9-82-48"); //ALERTNESS_STATE is the first alphabetically
                        break;
                    case CathodeDataType.SHORT_GUID:
                        thisParam = new CathodeResource();
                        ((CathodeResource)thisParam).resourceID = new ShortGuid("00-00-00-00");
                        break;
                    case CathodeDataType.BOOL:
                        thisParam = new CathodeBool();
                        break;
                    case CathodeDataType.DIRECTION:
                        thisParam = new CathodeVector3();
                        break;
                    case CathodeDataType.INTEGER:
                        thisParam = new CathodeInteger();
                        break;
                }
                newEntity.parameters.Add(new CathodeLoadedParameter(newEntity.parameter, thisParam));

                //Add to composite & save name
                composite.datatypes.Add(newEntity);
                ShortGuidUtils.Generate(textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createFunctionEntity.Checked)
            {
                //Create FunctionEntity
                FunctionEntity newEntity = new FunctionEntity(thisID);
                ShortGuid function = CathodeEntityDatabase.GetEntityAtIndex(entityVariant.SelectedIndex).guid;
                switch (CommandsUtils.GetFunctionType(function))
                {
                    //TODO: find a nicer way of auto selecting this (E.G. can we reflect to class names?)
                    case CathodeFunctionType.CAGEAnimation:
                        newEntity = new CAGEAnimation(thisID);
                        break;
                    case CathodeFunctionType.TriggerSequence:
                        newEntity = new TriggerSequence(thisID);
                        break;
                }
                newEntity.function = function;
                //TODO: auto populate params here based on defaults

                //Add to composite & save name
                composite.functions.Add(newEntity);
                CurrentInstance.compositeLookup.SetEntityName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createCompositeEntity.Checked)
            {
                //Create FunctionEntity
                FunctionEntity newEntity = new FunctionEntity(thisID);
                CathodeComposite composite = composites.FirstOrDefault(o => o.name == entityVariant.Text);
                if (composite == null)
                { 
                    MessageBox.Show("Failed to look up composite!\nPlease report this issue on GitHub.\n\n" + entityVariant.Text, "Could not find composite!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                newEntity.function = composite.shortGUID;

                //Add to composite & save name
                this.composite.functions.Add(newEntity);
                CurrentInstance.compositeLookup.SetEntityName(this.composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createProxyEntity.Checked)
            {
                //Create ProxyEntity
                ProxyEntity newEntity = new ProxyEntity(thisID);
                newEntity.hierarchy = hierarchy;
                newEntity.extraId = ShortGuidUtils.Generate("temp"); //TODO: how do we generate this?

                newEntity.parameters.Add(new CathodeLoadedParameter(ShortGuidUtils.Generate("proxy_filter_targets"), new CathodeBool() { value = false }));
                newEntity.parameters.Add(new CathodeLoadedParameter(ShortGuidUtils.Generate("proxy_enable_on_reset"), new CathodeBool() { value = false }));
                newEntity.parameters.Add(new CathodeLoadedParameter(ShortGuidUtils.Generate("proxy_enable"), new CathodeFloat() { value = 0.0f }));
                newEntity.parameters.Add(new CathodeLoadedParameter(ShortGuidUtils.Generate("proxy_enabled"), new CathodeFloat() { value = 0.0f }));
                newEntity.parameters.Add(new CathodeLoadedParameter(ShortGuidUtils.Generate("proxy_disable"), new CathodeFloat() { value = 0.0f }));
                newEntity.parameters.Add(new CathodeLoadedParameter(ShortGuidUtils.Generate("proxy_disabled"), new CathodeFloat() { value = 0.0f }));
                newEntity.parameters.Add(new CathodeLoadedParameter(ShortGuidUtils.Generate("reference"), new CathodeString() { value = "" }));
                newEntity.parameters.Add(new CathodeLoadedParameter(ShortGuidUtils.Generate("trigger"), new CathodeFloat() { value = 0.0f }));

                //Add to composite & save name
                composite.proxies.Add(newEntity);
                CurrentInstance.compositeLookup.SetEntityName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            else if (createOverrideEntity.Checked)
            {
                //Create OverrideEntity
                OverrideEntity newEntity = new OverrideEntity(thisID);
                newEntity.hierarchy = hierarchy;
                newEntity.checksum = ShortGuidUtils.Generate("temp"); //TODO: how do we generate this?

                //Add to composite & save name
                composite.overrides.Add(newEntity);
                CurrentInstance.compositeLookup.SetEntityName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            this.Close();
        }
    }
}
