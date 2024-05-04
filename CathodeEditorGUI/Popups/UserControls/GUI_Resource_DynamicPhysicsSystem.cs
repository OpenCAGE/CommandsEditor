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
    public partial class GUI_Resource_DynamicPhysicsSystem : ResourceUserControl
    {
        public GUI_Resource_DynamicPhysicsSystem() : base()
        {
            InitializeComponent();

            //TODO: populate from PHYSICS.MAP

            //NOTE: when we save commands we populate this resource index from the system_index param, so we don't need to display anything in the gui
        }

        public void PopulateUI()
        {
            //todo: show instances & instance data. allow adding new instance ref similar to character editor
        }
    }
}
