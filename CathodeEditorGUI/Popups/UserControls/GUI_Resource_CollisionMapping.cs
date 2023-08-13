using CATHODE.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor.Popups.UserControls
{
    public partial class GUI_Resource_CollisionMapping : ResourceUserControl
    {
        public Vector3 Position { get { return new Vector3((float)POS_X.Value, (float)POS_Y.Value, (float)POS_Z.Value); } }
        public Vector3 Rotation { get { return new Vector3((float)ROT_X.Value, (float)ROT_Y.Value, (float)ROT_Z.Value); } }

        public ShortGuid CollisionID { get { return new ShortGuid(collisionID.Text == "" ? "FF-FF-FF-FF" : collisionID.Text); } } //todo: this is a temp hack for unresolved collision ids

        public GUI_Resource_CollisionMapping(LevelContent editor) : base(editor)
        {

            //TODO: Fetch this data correctly from COLLISION.MAP, and then re-write it too!


            InitializeComponent();

            collisionID.BeginUpdate();
            collisionID.Items.Clear();
            collisionID.Items.Add("FF-FF-FF-FF");
            for (int i = 0; i < Editor.resource.collision_maps.Entries.Count; i++)
            {
                string id = Editor.resource.collision_maps.Entries[i].entity.entity_id.ToByteString();
                if (collisionID.Items.Contains(id)) continue;
                collisionID.Items.Add(id);
            }
            collisionID.EndUpdate();
        }

        public void PopulateUI(Vector3 position, Vector3 rotation, ShortGuid collisionID)
        {
            POS_X.Value = (decimal)position.X;
            POS_Y.Value = (decimal)position.Y;
            POS_Z.Value = (decimal)position.Z;

            ROT_X.Value = (decimal)rotation.X;
            ROT_Y.Value = (decimal)rotation.Y;
            ROT_Z.Value = (decimal)rotation.Z;

            this.collisionID.SelectedItem = collisionID.ToByteString();
        }
    }
}
