using CATHODE.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommandsEditor
{
    public partial class EditSpline : Form
    {
        public Action<cSpline> OnSaved;

        public EditSpline(cSpline spline)
        {
            InitializeComponent();
        }

        private void addPoint_Click(object sender, EventArgs e)
        {

        }

        private void removePoint_Click(object sender, EventArgs e)
        {

        }

        private void saveSpline_Click(object sender, EventArgs e)
        {

        }
    }
}
