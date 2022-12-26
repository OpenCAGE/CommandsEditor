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
                ShortGuid function = CathodeEntityDatabase.GetEntity(entityVariant.Text).guid;
                if (function.val == null)
                {
                    MessageBox.Show("Please make sure you have typed or selected a valid entity class to create.", "Invalid entity class", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                switch (CommandsUtils.GetFunctionType(function))
                {
                    //TODO: find a nicer way of auto selecting this (E.G. can we reflect to class names?)
                    case FunctionType.CAGEAnimation:
                        newEntity = new CAGEAnimation(thisID);
                        break;
                    case FunctionType.TriggerSequence:
                        newEntity = new TriggerSequence(thisID);
                        break;

                    case FunctionType.ModelReference:
                        cResource resourceData = new cResource(newEntity.shortGUID);
                        resourceData.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        resourceData.AddResource(ResourceType.COLLISION_MAPPING); //TODO: do we really wanna add this?
                        newEntity.parameters.Add(new Parameter("resource", resourceData));
                        break;
                    case FunctionType.SoundBarrier:
                        newEntity.AddResource(ResourceType.COLLISION_MAPPING);
                        break;
                    case FunctionType.ExclusiveMaster:
                        newEntity.AddResource(ResourceType.EXCLUSIVE_MASTER_STATE_RESOURCE);
                        break;
                    case FunctionType.CollisionBarrier:
                        newEntity.AddResource(ResourceType.COLLISION_MAPPING);
                        break;
                    case FunctionType.TRAV_1ShotSpline:
                        //TODO: There are loads of TRAV_ entities which are unused in the vanilla game, so I'm not sure if they should apply to those too...
                        newEntity.AddResource(ResourceType.TRAVERSAL_SEGMENT);
                        break;
                    case FunctionType.NavMeshBarrier:
                        newEntity.AddResource(ResourceType.NAV_MESH_BARRIER_RESOURCE);
                        newEntity.AddResource(ResourceType.COLLISION_MAPPING);
                        break;
                    case FunctionType.PhysicsSystem:
                        newEntity.parameters.Add(new Parameter("system_index", new cInteger(0)));
                        newEntity.AddResource(ResourceType.DYNAMIC_PHYSICS_SYSTEM).startIndex = 0;
                        break;
                    case FunctionType.EnvironmentModelReference:
                        cResource resourceData2 = new cResource(newEntity.shortGUID);
                        resourceData2.AddResource(ResourceType.ANIMATED_MODEL); //TODO: need to figure out what startIndex links to, so we can set that!
                        newEntity.parameters.Add(new Parameter("resource", resourceData2));
                        break;

                    // TODO: these should point to the commented models, but we need to figure out the REDS index!
                    case FunctionType.ParticleEmitterReference:   /// [dynamic_mesh]
                    case FunctionType.RibbonEmitterReference:     /// [dynamic_mesh]
                    case FunctionType.SurfaceEffectBox:           /// Global/Props/fogbox.CS2 -> [VolumeFog]
                    case FunctionType.FogBox:                     /// Global/Props/fogplane.CS2 -> [Plane01] 
                    case FunctionType.SurfaceEffectSphere:        /// Global/Props/fogsphere.CS2 -> [Sphere01]
                    case FunctionType.FogSphere:                  /// Global/Props/fogsphere.CS2 -> [Sphere01]
                    case FunctionType.SimpleRefraction:           /// Global/Props/refraction.CS2 -> [Plane01]
                    case FunctionType.SimpleWater:                /// Global/Props/noninteractive_water.CS2 -> [Plane01]
                    case FunctionType.LightReference:             /// Global/Props/deferred_point_light.cs2 -> [Sphere01],  Global/Props/deferred_spot_light.cs2 -> [Sphere02], Global/Props/deferred_strip_light.cs2 -> [Sphere01]
                        newEntity.AddResource(ResourceType.RENDERABLE_INSTANCE);
                        break;
                }
                newEntity.parameters.Add(new Parameter("position", new cTransform()));
                newEntity.function = function;

                //Add to composite & save name
                composite.functions.Add(newEntity);
                CurrentInstance.compositeLookup.SetEntityName(composite.shortGUID, thisID, textBox1.Text);
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
                CurrentInstance.compositeLookup.SetEntityName(composite.shortGUID, thisID, textBox1.Text);
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
                CurrentInstance.compositeLookup.SetEntityName(composite.shortGUID, thisID, textBox1.Text);
                OnNewEntity?.Invoke(newEntity);
            }
            this.Close();
        }
    }
}
