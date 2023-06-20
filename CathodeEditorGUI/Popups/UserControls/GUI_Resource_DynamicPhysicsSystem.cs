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
        public GUI_Resource_DynamicPhysicsSystem(CommandsEditor editor) : base(editor)
        {
            InitializeComponent();

            //TODO: populate from PHYSICS.MAP
        }

        public void PopulateUI()
        {
        }
    }
}
