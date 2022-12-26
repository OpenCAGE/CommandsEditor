﻿using CATHODE;
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
                                    "POSITION",
                                    "FLOAT",
                                    "STRING",
                                    "SPLINE_DATA",
                                    "ENUM",
                                    "RESOURCE",
                                    "FILEPATH",
                                    "BOOL",
                                    "DIRECTION",
                                    "INTEGER"
                                    // TODO: we should support other types here, such as ZONE_LINK_PTR used in doors
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
                    case DataType.POSITION:
                        thisParam = new cTransform();
                        break;
                    case DataType.FLOAT:
                        thisParam = new cFloat();
                        break;
                    case DataType.FILEPATH:
                    case DataType.STRING:
                        thisParam = new cString();
                        break;
                    case DataType.SPLINE_DATA:
                        thisParam = new cSpline();
                        break;
                    case DataType.ENUM:
                        thisParam = new cEnum("ALERTNESS_STATE", 0); //ALERTNESS_STATE is the first alphabetically
                        break;
                    case DataType.RESOURCE:
                        thisParam = new cResource();
                        ((cResource)thisParam).resourceID = ShortGuidUtils.Generate(DateTime.Now.ToString("G"));
                        break;
                    case DataType.BOOL:
                        thisParam = new cBool();
                        break;
                    case DataType.DIRECTION:
                        thisParam = new cVector3();
                        break;
                    case DataType.INTEGER:
                        thisParam = new cInteger();
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
                ShortGuid function = CathodeEntityDatabase.GetEntityAtIndex(entityVariant.SelectedIndex).guid;
                switch (CommandsUtils.GetFunctionType(function))
                {
                    //TODO: find a nicer way of auto selecting this (E.G. can we reflect to class names?)
                    case FunctionType.CAGEAnimation:
                        newEntity = new CAGEAnimation(thisID);
                        break;
                    case FunctionType.TriggerSequence:
                        newEntity = new TriggerSequence(thisID);
                        break;
                }
                newEntity.function = function;
                //TODO: auto populate params here based on defaults

                // TODO: these types of entities seem to have their own non-parameterised resources:
                // - ParticleEmitterReference
                // - RibbonEmitterReference
                // - TRAV_1ShotSpline
                // - LightReference
                // - SurfaceEffectSphere
                // - FogSphere
                // - NavMeshBarrier
                // - FogBox
                // - SoundBarrier
                // - SurfaceEffectBox
                // - SimpleWater
                // - SimpleRefraction
                // - CollisionBarrier
                // - PhysicsSystem (Although these don't seem to keep the entity ID)
                // ... we should probably auto-generate these resources when adding new entities of these types.

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
                newEntity.extraId = ShortGuidUtils.Generate("temp"); //dunno what this val is meant to be, but apparently this works!

                newEntity.parameters.Add(new Parameter(ShortGuidUtils.Generate("proxy_filter_targets"), new cBool() { value = false }));
                newEntity.parameters.Add(new Parameter(ShortGuidUtils.Generate("proxy_enable_on_reset"), new cBool() { value = false }));
                newEntity.parameters.Add(new Parameter(ShortGuidUtils.Generate("proxy_enable"), new cFloat() { value = 0.0f }));
                newEntity.parameters.Add(new Parameter(ShortGuidUtils.Generate("proxy_enabled"), new cFloat() { value = 0.0f }));
                newEntity.parameters.Add(new Parameter(ShortGuidUtils.Generate("proxy_disable"), new cFloat() { value = 0.0f }));
                newEntity.parameters.Add(new Parameter(ShortGuidUtils.Generate("proxy_disabled"), new cFloat() { value = 0.0f }));
                newEntity.parameters.Add(new Parameter(ShortGuidUtils.Generate("reference"), new CATHODE.Commands.cString() { value = "" }));
                newEntity.parameters.Add(new Parameter(ShortGuidUtils.Generate("trigger"), new cFloat() { value = 0.0f }));

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
