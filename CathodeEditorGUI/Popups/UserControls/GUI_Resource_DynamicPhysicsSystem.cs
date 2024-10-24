using CATHODE.Scripting;
using CommandsEditor.DockPanels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using CathodeLib;

namespace CommandsEditor.Popups.UserControls
{
    public partial class GUI_Resource_DynamicPhysicsSystem : ResourceUserControl
    {
        private Dictionary<EntityPath, CATHODE.PhysicsMaps.Entry> _hierarchies = new Dictionary<EntityPath, CATHODE.PhysicsMaps.Entry>();

        public GUI_Resource_DynamicPhysicsSystem() : base()
        {
            InitializeComponent();

            //TODO: populate from PHYSICS.MAP

            //NOTE: when we save commands we populate this resource index from the system_index param, so we don't need to display anything in the gui
        }

        public void PopulateUI(EntityInspector entDisplay, int physIndex)
        {
            _hierarchies.Clear();
            List<EntityPath> hierarchies = entDisplay.Content.editor_utils.GetHierarchiesForEntity(entDisplay.Composite, entDisplay.Entity);
            for (int i = 0; i < hierarchies.Count; i++)
            {
                ShortGuid instance = hierarchies[i].GenerateCompositeInstanceID();
                List<CATHODE.PhysicsMaps.Entry> physMaps = Content.resource.physics_maps.Entries.FindAll(o => o.composite_instance_id == instance);
                if (physMaps.Count != 1)
                {
                    MessageBox.Show(
                        "Unexpected amount of physics maps!\n\n" +
                        "Please share this info to GitHub:\n" +
                        " - Count: " + physMaps.Count + "\n" +
                        " - Entity: " + entDisplay.Entity.shortGUID.ToByteString() + "\n" +
                        " - Composite: " + entDisplay.Composite.shortGUID.ToByteString()
                    );
                }
                CATHODE.PhysicsMaps.Entry physMap = physMaps[0];

                //This path should resolve to one step down from hierarchies[i]:
                //EntityPath path = entDisplay.Content.editor_utils.GetHierarchyFromReference(physMap.entity);

                _hierarchies.Add(hierarchies[i], physMap);
            }

            instances.Items.Clear();
            foreach (KeyValuePair<EntityPath, CATHODE.PhysicsMaps.Entry> hierarchyMap in _hierarchies)
            {
                EntityPath pathShort = hierarchyMap.Key.Copy();
                pathShort.GoBackOneStep(); //TODO: is this right? i've converted this to the new path stuff and it needs testing.
                //Array.Resize(ref pathShort.path, pathShort.path.Length - 1);
                instances.Items.Add(pathShort.GetAsString(Content.commands, entDisplay.Composite, false));
            }
        }

        private void addNewInstanceRef_Click(object sender, EventArgs e)
        {

        }

        private void instances_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (instances.SelectedIndex == -1)
                return;

            KeyValuePair<EntityPath, CATHODE.PhysicsMaps.Entry> hierarchyMap = _hierarchies.ElementAt(instances.SelectedIndex);

            POS_X.Value = (decimal)hierarchyMap.Value.Position.X; POS_Y.Value = (decimal)hierarchyMap.Value.Position.Y; POS_Z.Value = (decimal)hierarchyMap.Value.Position.Z;

            (decimal yaw, decimal pitch, decimal roll) = hierarchyMap.Value.Rotation.ToYawPitchRoll();
            ROT_X.Value = pitch; ROT_Y.Value = yaw * -1; ROT_Z.Value = roll; //TODO: why do i have to -1 here
        }
    }
}