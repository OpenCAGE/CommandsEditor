using CATHODE.Scripting;
using CommandsEditor.Popups.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using CATHODE;
using CommandsEditor.Popups.Base;
using CommandsEditor.DockPanels;
using CathodeLib;
using OpenCAGE;
using System.Windows;
using static CATHODE.Movers;

namespace CommandsEditor
{
    public partial class EditMVR : BaseWindow
    {
        private int loadedMvrIndex = -1;
        private ShortGuid filteredNodeID;

        List<int> _mvrListIndexes = new List<int>();

        private EntityInspector _entityDisplay;

        public EditMVR(EntityInspector editor) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION)
        {
            InitializeComponent();
            _entityDisplay = editor;
            
            PopulateUI(editor.Entity.shortGUID);

            renderable.OnMaterialSelected += OnMaterialSelected;
            renderable.OnModelSelected += OnModelSelected;

#if !DEBUG
            DEBUG_clear.Visible = false;
#endif

            POS_X.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStep);
            POS_Y.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStep);
            POS_Z.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStep);
            ROT_X.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStepRot);
            ROT_Y.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStepRot);
            ROT_Z.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStepRot);
            SCALE_X.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStep);
            SCALE_Y.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStep);
            SCALE_Z.Increment = (decimal)SettingsManager.GetFloat(Singleton.Settings.NumericStep);
        }

        private void PopulateUI(ShortGuid nodeID)
        {
            filteredNodeID = nodeID;

            //Get all MVR entries that match this entity
            _mvrListIndexes.Clear();
            for (int i = 0; i < Content.mvr.Entries.Count; i++)
            {
                if (Content.mvr.Entries[i].entity.entity_id != nodeID) continue;
                _mvrListIndexes.Add(i);
            }

            //Fetch the hierarchies for the MVR entries that point to this entity
            EntityPath[] hierarchies = new EntityPath[_mvrListIndexes.Count];
            Parallel.For(0, _mvrListIndexes.Count, i =>
            {
                hierarchies[i] = _entityDisplay.Content.editor_utils.GetHierarchyFromHandle(Content.mvr.Entries[_mvrListIndexes[i]].entity);
            });

            //Write the hierarchies to the list
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            for (int i = 0; i < hierarchies.Length; i++)
            {
                listBox1.Items.Add(hierarchies[i] == null ? _mvrListIndexes[i].ToString() + " [unresolvable]" : hierarchies[i].GetAsString(Content.commands, _entityDisplay.Composite, false));
            }
            listBox1.EndUpdate();
            if (listBox1.Items.Count != 0) listBox1.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadedMvrIndex = -1;
            if (listBox1.SelectedIndex == -1) return;
            LoadMVR(_mvrListIndexes[listBox1.SelectedIndex]);
        }

        private bool hasLoaded = false;
        private void LoadMVR(int mvrIndex)
        {
            hasLoaded = false;
            loadedMvrIndex = mvrIndex;
            Movers.MOVER_DESCRIPTOR mvr = Content.mvr.Entries[loadedMvrIndex];
            renderable.PopulateUI((int)mvr.renderable_element_index, (int)mvr.renderable_element_count);

            Matrix4x4.Decompose(mvr.transform, out Vector3 scale, out Quaternion rotation, out Vector3 position);

            POS_X.Value = (decimal)position.X; POS_Y.Value = (decimal)position.Y; POS_Z.Value = (decimal)position.Z;
            SCALE_X.Value = (decimal)scale.X; SCALE_Y.Value = (decimal)scale.Y; SCALE_Z.Value = (decimal)scale.Z;

            (decimal yaw, decimal pitch, decimal roll) = rotation.ToYawPitchRoll();
            ROT_X.Value = pitch; ROT_Y.Value = yaw; ROT_Z.Value = roll;

            //NOTE: Obviously, all these values actually come from the scripting (e.g. disable_size_culling is the cull flag).
            //      Really we should write all this data automatically, and update the entities appropriately.
            //      I need to write a sanity checker that re-populates the Commands data with the things that got stripped, but are in MVR.

            Console.WriteLine("Emissive Flags: " + mvr.emissive_flags.ToString());

            emReplaceTint.Checked = mvr.emissive_flags.HasFlag(EmissiveFlag.ReplaceTint);
            emReplaceIntensity.Checked = mvr.emissive_flags.HasFlag(EmissiveFlag.ReplaceIntensity);
            emMasterOff.Checked = mvr.emissive_flags.HasFlag(EmissiveFlag.MasterOff);

            Console.WriteLine("Cull Flags: " + mvr.cull_flags.ToString());

            cfDontShadows.Checked = mvr.cull_flags.HasFlag(CullFlag.NO_CAST_SHADOWS);
            cfDontRender.Checked = mvr.cull_flags.HasFlag(CullFlag.NO_RENDER);
            cfReflections.Checked = mvr.cull_flags.HasFlag(CullFlag.INCLUDE_IN_REFLECTIVE);
            cfNoSize.Checked = mvr.cull_flags.HasFlag(CullFlag.NO_SIZE_CULLING); 
            cfNoTorch.Checked = mvr.cull_flags.HasFlag(CullFlag.NO_CAST_TORCH_SHADOW);
            cfAlwaysPass.Checked = mvr.cull_flags.HasFlag(CullFlag.ALWAYS_PASS);

            requiresScript.Checked = mvr.flags.requires_script;
            isVisible.Checked = mvr.flags.visible;
            isStationary.Checked = mvr.flags.stationary;

            emTint.BackColor = Color.FromArgb((int)mvr.emissive_tint.X, (int)mvr.emissive_tint.Y, (int)mvr.emissive_tint.Z);
            emIntensityMultiplier.Value = (decimal)mvr.emissive_intensity_multiplier;
            emRadiosityMultiplier.Value = (decimal)mvr.emissive_radiosity_multiplier;

            hasLoaded = true;
        }
        private void saveMover_Click(object sender, EventArgs e)
        {
            //SaveMVR();
        }
        private void OnMaterialSelected(int submeshIndex, int materialIndex)
        {
            //SaveMVR();
        }
        private void OnModelSelected(int modelPakIndex)
        {
            //SaveMVR();
        }
        private void SaveMVR()
        {
            if (!hasLoaded || loadedMvrIndex == -1) return;
            Movers.MOVER_DESCRIPTOR mvr = Content.mvr.Entries[loadedMvrIndex];
            mvr.renderable_element_count = renderable.SelectedMaterialIndexes.Count;
            mvr.renderable_element_index = Content.resource.reds.Entries.Count;

            Vector3 scale = new Vector3((float)SCALE_X.Value, (float)SCALE_Y.Value, (float)SCALE_Z.Value);
            Vector3 position = new Vector3((float)POS_X.Value, (float)POS_Y.Value, (float)POS_Z.Value);

            Quaternion rotation = Quaternion.CreateFromYawPitchRoll(
                (float)Convert.ToDouble(ROT_Y.Value) * (float)Math.PI / 180.0f, 
                (float)Convert.ToDouble(ROT_X.Value) * (float)Math.PI / 180.0f, 
                (float)Convert.ToDouble(ROT_Z.Value) * (float)Math.PI / 180.0f);

            mvr.transform = Matrix4x4.CreateScale(scale) *
                            Matrix4x4.CreateFromQuaternion(rotation) *
                            Matrix4x4.CreateTranslation(position);

            for (int y = 0; y < renderable.SelectedMaterialIndexes.Count; y++)
            {
                RenderableElements.Element newRed = new RenderableElements.Element();
                newRed.ModelIndex = renderable.SelectedModelIndex + y; //assumes sequential write
                newRed.MaterialIndex = renderable.SelectedMaterialIndexes[y];
                if (y == 0)
                {
                    newRed.LODIndex = Content.resource.reds.Entries.Count;
                    //newRed.LODCount = (byte)renderable.SelectedMaterialIndexes.Count;
                    newRed.LODCount = 0; //TODO!!
                }
                Content.resource.reds.Entries.Add(newRed);
            }

            mvr.emissive_flags = EmissiveFlag.None;
            if (emReplaceTint.Checked) mvr.emissive_flags |= EmissiveFlag.ReplaceTint;
            if (emReplaceIntensity.Checked) mvr.emissive_flags |= EmissiveFlag.ReplaceIntensity;
            if (emMasterOff.Checked) mvr.emissive_flags |= EmissiveFlag.MasterOff;

            mvr.cull_flags = CullFlag.DEFAULT;
            if (cfDontShadows.Checked) mvr.cull_flags |= CullFlag.NO_CAST_SHADOWS;
            if (cfDontRender.Checked) mvr.cull_flags |= CullFlag.NO_RENDER;
            if (cfReflections.Checked) mvr.cull_flags |= CullFlag.INCLUDE_IN_REFLECTIVE;
            if (cfNoSize.Checked) mvr.cull_flags |= CullFlag.NO_SIZE_CULLING;
            if (cfNoTorch.Checked) mvr.cull_flags |= CullFlag.NO_CAST_TORCH_SHADOW;
            if (cfAlwaysPass.Checked) mvr.cull_flags |= CullFlag.ALWAYS_PASS;

            mvr.flags.requires_script = requiresScript.Checked;
            mvr.flags.visible = isVisible.Checked;
            mvr.flags.stationary = isStationary.Checked;

            mvr.emissive_tint = new Vector3(emTint.BackColor.R, emTint.BackColor.G, emTint.BackColor.B);
            mvr.emissive_intensity_multiplier = (float)emIntensityMultiplier.Value;
            mvr.emissive_radiosity_multiplier = (float)emRadiosityMultiplier.Value;

            Console.WriteLine("SAVED");
            //MessageBox.Show("Saved changes for mover " + loadedMvrIndex + "!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void deleteMover_Click(object sender, EventArgs e)
        {
            if (loadedMvrIndex == -1) return;
            //CurrentInstance.moverDB.Movers.RemoveAt(loadedMvrIndex);

            Content.mvr.Entries[loadedMvrIndex] = Content.mvr.Entries[0];

            PopulateUI(filteredNodeID);
        }

        private void POS_X_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void POS_Y_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void POS_Z_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void ROT_X_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void ROT_Y_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void ROT_Z_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void ROT_W_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void SCALE_X_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void SCALE_Y_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void SCALE_Z_ValueChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void type_dropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveMVR();
        }

        private void DEBUG_clear_Click(object sender, EventArgs e)
        {
            Movers.MOVER_DESCRIPTOR mvr = Content.mvr.Entries[loadedMvrIndex];

            var resource = Content.resource.resources.Entries[mvr.resource_index];
            var renderable_element = Content.resource.reds.Entries[(int)mvr.renderable_element_index];

            for (int i = 0; i < Content.resource.collision_maps.Entries.Count; i++)
            {
                Console.WriteLine(Content.resource.collision_maps.Entries[i].zone_id);
            }

            Content.resource.collision_maps.Entries.FirstOrDefault(o => o.entity == mvr.entity).entity = new EntityHandle();

            mvr.entity = new EntityHandle();

           // mvr.entity = new CommandsEntityReference();
            //mvr.resource_index = -1;
        }

        private void openColourPicker_Click(object sender, EventArgs e)
        {
            ColorDialog colourPicker = new ColorDialog();
            colourPicker.Color = emTint.BackColor;
            if (colourPicker.ShowDialog() == DialogResult.OK)
                emTint.BackColor = colourPicker.Color;
        }

        private void doSave_Click(object sender, EventArgs e)
        {
            SaveMVR();
        }
    }
}
