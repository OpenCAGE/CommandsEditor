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
    public partial class GUI_Resource_NavMeshBarrierResource : ResourceUserControl
    {
        public Vector3 Position { get { return new Vector3((float)POS_X.Value, (float)POS_Y.Value, (float)POS_Z.Value); } }
        public Vector3 Rotation { get { return new Vector3((float)ROT_X.Value, (float)ROT_Y.Value, (float)ROT_Z.Value); } }

        public GUI_Resource_NavMeshBarrierResource()
        {
            InitializeComponent();
        }

        public void PopulateUI(Vector3 position, Vector3 rotation)
        {
            POS_X.Value = (decimal)position.X;
            POS_Y.Value = (decimal)position.Y;
            POS_Z.Value = (decimal)position.Z;

            ROT_X.Value = (decimal)rotation.X;
            ROT_Y.Value = (decimal)rotation.Y;
            ROT_Z.Value = (decimal)rotation.Z;
        }
    }
}
