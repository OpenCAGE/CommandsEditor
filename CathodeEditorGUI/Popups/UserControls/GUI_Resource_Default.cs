using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CATHODE.Scripting;
using CathodeLib;
using CATHODE;
using CATHODE.LEGACY;
using System.Numerics;

namespace CommandsEditor.Popups.UserControls
{
    public partial class GUI_Resource_Default : ResourceUserControl
    {
        public GUI_Resource_Default() : base()
        {
            InitializeComponent();
        }

        public void PopulateUI(ResourceType type)
        {
            groupBox1.Text = type.ToString();
        }
    }
}
