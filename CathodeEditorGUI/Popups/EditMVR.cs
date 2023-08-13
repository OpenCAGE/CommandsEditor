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

namespace CommandsEditor
{
    public partial class EditMVR : BaseWindow
    {
        private int loadedMvrIndex = -1;
        private ShortGuid filteredNodeID;

        List<int> _mvrListIndexes = new List<int>();

        private EntityDisplay _entityDisplay;

        public EditMVR(EntityDisplay editor, ShortGuid nodeID = new ShortGuid()) : base(WindowClosesOn.COMMANDS_RELOAD | WindowClosesOn.NEW_ENTITY_SELECTION | WindowClosesOn.NEW_COMPOSITE_SELECTION, editor.Content)
        {
            InitializeComponent();
            _entityDisplay = editor;
            
            renderable.SetEditor(editor.Content);
            PopulateUI(nodeID);

            renderable.OnMaterialSelected += OnMaterialSelected;
            renderable.OnModelSelected += OnModelSelected;
        }

        private void PopulateUI(ShortGuid nodeID)
        {
            filteredNodeID = nodeID;

            //Get all MVR entries that match this entity
            _mvrListIndexes.Clear();
            for (int i = 0; i < Editor.mvr.Entries.Count; i++)
            {
                if (nodeID.val != null && Editor.mvr.Entries[i].entity.entity_id != nodeID) continue;
                _mvrListIndexes.Add(i);
            }

            //Fetch the hierarchies for the MVR entries that point to this entity
            EntityHierarchy[] hierarchies = new EntityHierarchy[_mvrListIndexes.Count];
            Parallel.For(0, _mvrListIndexes.Count, i =>
            {
                hierarchies[i] = _entityDisplay.Content.editor_utils.GetHierarchyFromReference(Editor.mvr.Entries[_mvrListIndexes[i]].entity);
            });

            //Write the hierarchies to the list
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            for (int i = 0; i < hierarchies.Length; i++)
            {
                listBox1.Items.Add(hierarchies[i] == null ? _mvrListIndexes[i].ToString() + " [unresolvable]" : hierarchies[i].GetHierarchyAsString(Editor.commands, _entityDisplay.Composite, false));
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

        cTransform transformData = null;
        private bool hasLoaded = false;
        private void LoadMVR(int mvrIndex)
        {
            hasLoaded = false;
            loadedMvrIndex = mvrIndex;
            Movers.MOVER_DESCRIPTOR mvr = Editor.mvr.Entries[loadedMvrIndex];
            renderable.PopulateUI((int)mvr.renderableElementIndex, (int)mvr.renderableElementCount);

            Matrix4x4.Decompose(mvr.transform, out Vector3 scale, out Quaternion rotation, out Vector3 position);

            POS_X.Value = (decimal)position.X; POS_Y.Value = (decimal)position.Y; POS_Z.Value = (decimal)position.Z;
            SCALE_X.Value = (decimal)scale.X; SCALE_Y.Value = (decimal)scale.Y; SCALE_Z.Value = (decimal)scale.Z;

            decimal yaw = Convert.ToDecimal(Math.Atan2(2 * (rotation.Y * rotation.W + rotation.X * rotation.Z), 1 - 2 * (rotation.Y * rotation.Y + rotation.X * rotation.X)) * (180 / Math.PI));
            decimal pitch = Convert.ToDecimal(Math.Asin(2 * (rotation.X * rotation.W - rotation.Z * rotation.Y)) * (180 / Math.PI));
            decimal roll = Convert.ToDecimal(Math.Atan2(2 * (rotation.Z * rotation.W + rotation.X * rotation.Y), 1 - 2 * (rotation.X * rotation.X + rotation.Z * rotation.Z)) * (180 / Math.PI));
            
            ROT_X.Value = pitch; ROT_Y.Value = yaw; ROT_Z.Value = roll;

            type_dropdown.SelectedItem = mvr.instanceTypeFlags.ToString();
            hasLoaded = true;
        }
        private void saveMover_Click(object sender, EventArgs e)
        {
            SaveMVR();
        }
        private void OnMaterialSelected(int submeshIndex, int materialIndex)
        {
            SaveMVR();
        }
        private void OnModelSelected(int modelPakIndex)
        {
            SaveMVR();
        }
        private void SaveMVR()
        {
            if (!hasLoaded || loadedMvrIndex == -1) return;
            Movers.MOVER_DESCRIPTOR mvr = Editor.mvr.Entries[loadedMvrIndex];
            mvr.renderableElementCount = (uint)renderable.SelectedMaterialIndexes.Count;
            mvr.renderableElementIndex = (uint)Editor.resource.reds.Entries.Count;

            Vector3 scale = new Vector3((float)SCALE_X.Value, (float)SCALE_Y.Value, (float)SCALE_Z.Value);
            //Quaternion rotation = Quaternion.Normalize(new Quaternion((float)ROT_X.Value, (float)ROT_Y.Value, (float)ROT_Z.Value, (float)ROT_W.Value));
            Vector3 position = new Vector3((float)POS_X.Value, (float)POS_Y.Value, (float)POS_Z.Value);

            Quaternion rotation = Quaternion.CreateFromYawPitchRoll(
                (float)Convert.ToDouble(ROT_Y.Value) * (float)Math.PI / 180.0f, 
                (float)Convert.ToDouble(ROT_X.Value) * (float)Math.PI / 180.0f, 
                (float)Convert.ToDouble(ROT_Z.Value) * (float)Math.PI / 180.0f);

            mvr.transform = Matrix4x4.CreateScale(scale) *
                            Matrix4x4.CreateFromQuaternion(rotation) *
                            Matrix4x4.CreateTranslation(position);

            mvr.instanceTypeFlags = (ushort)Convert.ToInt32(type_dropdown.SelectedItem.ToString());

            Editor.mvr.Entries[loadedMvrIndex] = mvr;

            for (int y = 0; y < renderable.SelectedMaterialIndexes.Count; y++)
            {
                RenderableElements.Element newRed = new RenderableElements.Element();
                newRed.ModelIndex = renderable.SelectedModelIndex + y; //assumes sequential write
                newRed.MaterialIndex = renderable.SelectedMaterialIndexes[y];
                if (y == 0)
                {
                    newRed.LODIndex = Editor.resource.reds.Entries.Count;
                    //newRed.LODCount = (byte)renderable.SelectedMaterialIndexes.Count;
                    newRed.LODCount = 0; //TODO!!
                }
                Editor.resource.reds.Entries.Add(newRed);
            }

            Console.WriteLine("SAVED");
            //MessageBox.Show("Saved changes for mover " + loadedMvrIndex + "!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void deleteMover_Click(object sender, EventArgs e)
        {
            if (loadedMvrIndex == -1) return;
            //CurrentInstance.moverDB.Movers.RemoveAt(loadedMvrIndex);

            Editor.mvr.Entries[loadedMvrIndex] = Editor.mvr.Entries[0];

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
    }
}
